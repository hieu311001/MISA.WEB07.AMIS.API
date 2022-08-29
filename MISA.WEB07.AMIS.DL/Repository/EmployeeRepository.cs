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
        public object FilterEmployees(string? keyword, int pageSize = 10, int pageNumber = 1)
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
            var employees = multipleResults.Read<Employee>().ToList();
            var totalCount = multipleResults.Read<long>().Single();
            PagingData<Employee> pagingData = new PagingData<Employee>()
            {
                Data = employees,
                TotalCount = totalCount
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
            string storedNewEmployeeCode = "Proc_Employee_GetMaxCode";

            string newEmployeeCode = mySqlConnection.QueryFirstOrDefault<String>(storedNewEmployeeCode, commandType: System.Data.CommandType.StoredProcedure);

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
            var storeProc = "Proc_CheckDuplicateCode";

            var parameters = new DynamicParameters();

            parameters.Add("$EmployeeID", employeeID);
            parameters.Add("$EmployeeCode", employeeCode);

            var employeeCodeDuplidate = mySqlConnection.QueryFirstOrDefault(sql: storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            if (employeeCodeDuplidate != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
