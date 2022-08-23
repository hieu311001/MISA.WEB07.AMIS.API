using MISA.WEB07.AMIS.Common.Entities;
using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.WEB07.AMIS.DL.Interfaces;

namespace MISA.WEB07.AMIS.DL.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeDL
    {
        public object FilterEmployees(string? keyword, int pageSize = 10, int pageNumber = 1)
        {
            // Khởi tạo kết nối tới DB MySQL
            string connectionString = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
            var mySqlConnection = new MySqlConnection(connectionString);

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

        public string getNewEmployeeCode()
        {
            string connectionString = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
            var mySqlConnection = new MySqlConnection(connectionString);

            string storedNewEmployeeCode = "Proc_Employee_GetMaxCode";

            string newEmployeeCode = mySqlConnection.QueryFirstOrDefault<String>(storedNewEmployeeCode, commandType: System.Data.CommandType.StoredProcedure);

            string newCode = "NV" + (Int64.Parse(newEmployeeCode.Substring(2)) + 1).ToString();

            return newCode;
        }
    }
}
