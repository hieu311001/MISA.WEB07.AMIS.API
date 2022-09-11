using MISA.WEB07.AMIS.Common.Entities;
using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.WEB07.AMIS.DL.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MISA.WEB07.AMIS.DL.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeDL
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="configuration"></param>
        /// CreatedBy VMHieu 28/08/2022
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// API Lấy danh sách nhân viên cho phép lọc và phân trang
        /// </summary>
        /// <param name="code">Mã nhân viên</param>
        /// <param name="name">Tên nhân viên</param>
        /// <param name="pageSize">Số trang muốn lấy</param>
        /// <param name="pageNumber">Thứ tự trang muốn lấy</param>
        /// <returns>Một đối tượng gồm:
        /// + Danh sách nhân viên thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số nhân viên thỏa mãn điều kiện</returns>
        /// Created by VMHieu (21/08/2022)
        public PagingData<Employee> FilterEmployees(string? keyword, int pageSize, int pageNumber)
        {
            // Chuẩn bị tên Stored procedure
            string storedProcedureName = "Proc_Employee_GetPaging";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@$Skip", (pageNumber - 1) * pageSize);
            parameters.Add("@$Take", pageSize);
            parameters.Add("@$Sort", "ModifiedDate DESC");

            var orConditions = new List<string>();
            string whereClause = "";

            if (keyword != null)
            {
                orConditions.Add($"EmployeeCode LIKE '%{keyword}%'");
                orConditions.Add($"EmployeeName LIKE '%{keyword}%'");
            }
            if (orConditions.Count > 0)
            {
                whereClause = $"WHERE ({string.Join(" OR ", orConditions)})";
            }

            parameters.Add("@$Where", whereClause);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả trả về từ DB
            PagingData<Employee> pagingData = new PagingData<Employee>()
            {
                Data = (List<Employee>)multipleResults.Read<Employee>(),
                TotalCount = multipleResults.Read<int>().Single(),
            };
            return pagingData;
        }

        /// <summary>
        /// Lấy ra mã EmployeeCode lớn nhất sau đó + 1
        /// </summary>
        /// <returns> EmployeeCodeMax+1 </returns>
        /// CreatedBy VMHieu 21/08/2022
        public string getNewEmployeeCode()
        {
            // Chuẩn bị câu lệnh 
            string storedNewEmployeeCode = "Proc_Employee_GetMaxCode";

            // Thực hiện gọi vào db để chạy câu lệnh GetNewEmployeeCode với tham số đầu vào ở trên
            string newEmployeeCode = mySqlConnection.QueryFirstOrDefault<String>(storedNewEmployeeCode, commandType: System.Data.CommandType.StoredProcedure);
            
            // Xử lý kết quả trả về ở db
            string newCode = "NV" + (Int64.Parse(newEmployeeCode.Substring(2)) + 1).ToString();

            return newCode;
        }

        /// <summary>
        /// Kiểm tra trùng mã nhân viên
        /// </summary>
        /// <returns>true-nếu có mã nhân viên trùng false- nếu mã nhân viên không trùng</returns>
        /// CreatedBy VMHieu 28/08/2022
        public bool IsDuplicate(Guid? employeeID, string employeeCode)
        {
            // Chuẩn bị câu lệnh 
            var storeProc = "Proc_CheckDuplicateCode";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();

            parameters.Add("$EmployeeID", employeeID);
            parameters.Add("$EmployeeCode", employeeCode);

            // Thực hiện gọi vào db để chạy câu lệnh với tham số đầu vào ở trên
            var employeeCodeDuplidate = mySqlConnection.QueryFirstOrDefault(sql: storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả của db
            if (employeeCodeDuplidate != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="ids">Chuỗi chứa các id của nhân viên cần xóa</param>
        /// <returns></returns>
        /// CreatedBy VMHieu 09/09/2022
        public int deleteMultiple(string ids)
        {
            // Tách dữ liệu id từ chuỗi ids:
            List<string> selectedIds = ids.Split('/').ToList();

            // Chuẩn bị câu lệnh Proc
            var sqlCommand = "Proc_Employee_DeleteMultiple";

            // Chuẩn bị tham số đầu vào cho câu lệnh sql
            var parameters = new DynamicParameters();

            var whereClause = "";
            foreach (var id in selectedIds)
            {
                if (id.Equals(selectedIds.Last()))
                {
                    whereClause += $"'{id}'";
                }
                else
                {
                    whereClause += $"'{id}', ";
                }
            }
            whereClause = '"' + whereClause + '"';

            parameters.Add("$EmployeeID", whereClause);

            // Thực hiện gọi vào db để chạy câu lệnh với tham số đầu vào ở trên
            var result = mySqlConnection.Execute(sqlCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả của db
            return result;
        }
    }
}
