using cukcuk.api.Entities.DTO;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.API.Entities;
using MISA.CukCuk.API.Enums;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.WEB07.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Method

        /// <summary>
        /// Lấy ra toàn bộ nhân viên
        /// </summary>
        /// <returns>Trả về thông tin của toàn bộ nhân viên</returns>
        /// CreatedBy VMHieu 20/08/2022
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Department>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]

        public IActionResult GetAllEmployee()
        {
            try
            {
                string connectionString = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
                var mySqlConnection = new MySqlConnection(connectionString);

                var getAllEmployeeCommand = "Proc_Employee_GetAllEmployee";

                var result = mySqlConnection.Query<Employee>(getAllEmployeeCommand, commandType: System.Data.CommandType.StoredProcedure);

                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API thêm 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên muốn thêm vào</param>
        /// <returns>ID của nhân viên được thêm mới</returns>
        /// CreatedBy VMHieu 20/08/2022
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Department>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị câu lệnh INSERT INTO
                string insertCommand = "INSERT INTO employee (EmployeeID, EmployeeCode, EmployeeName, DateOfBirth, Gender, IdentityNumber, IdentityPlace, IdentityDate, Email, PhoneNumber, DepartmentID, PositionName, BankAccount, BankName, BankBranch, Role, CreatedDate, CreatedBy, ModifiedBy, ModifiedDate, HotLine)" +
                    "VALUES(@EmployeeID, @EmployeeCode, @EmployeeName, @DateOfBirth, @Gender, @IdentityNumber, @IdentityPlace, @IdentityDate, @Email, @PhoneNumber, @DepartmentID, @PositionName, @BankAccount, @BankName, @BankBranch, @Role, @CreatedDate, @CreatedBy, @ModifiedBy, @ModifiedDate, @HotLine) ";

                // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
                var parameters = new DynamicParameters();
                var dateTimeNow = DateTime.Now;
                var EmployeeId = Guid.NewGuid();
                parameters.Add("@EmployeeID", EmployeeId);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityPlace", employee.IdentityPlace);
                parameters.Add("@IdentityDate", employee.IdentityDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionName", employee.PositionName);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@BankAccount", employee.BankAccount);
                parameters.Add("@BankName", employee.BankName);
                parameters.Add("@BankBranch", employee.BankBranch);
                parameters.Add("@Role", employee.Role);
                parameters.Add("@HotLine", employee.HotLine);
                parameters.Add("@CreatedDate", dateTimeNow);
                parameters.Add("@CreatedBy", employee.CreatedBy);
                parameters.Add("@ModifiedDate", dateTimeNow);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);

                // Thực hiện gọi vào db để chạy câu lệnh INSERT INTO với tham số đầu vào ở trên
                var affectedRows = mySqlConnection.Execute(insertCommand, parameters);

                // Xử lý kết quả trả về ở db
                if (affectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, EmployeeId);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API sửa 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên muốn thêm vào</param>
        /// <returns>ID của nhân viên được thêm mới</returns>
        /// CreatedBy VMHieu 20/08/2022
        [HttpPut("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Department>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateEmployee([FromBody] Employee employee, [FromRoute] Guid employeeID)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Update
                string updateEmployeeCommand = "UPDATE employee e " +
                    "SET EmployeeCode = @EmployeeCode, " +
                        "EmployeeName = @EmployeeName, " +
                        "DateOfBirth = @DateOfBirth, " +
                        "Gender = @Gender, " +
                        "IdentityNumber = @IdentityNumber, " +
                        "IdentityPlace = @IdentityPlace, " +
                        "IdentityDate = @IdentityDate, " +
                        "Email = @Email, " +
                        "PhoneNumber = @PhoneNumber, " +
                        "PositionName = @PositionName, " +
                        "DepartmentID = @DepartmentID, " +
                        "BankAccount = @BankAccount, " +
                        "BankName = @BankName, " +
                        "BankBranch = @BankBranch, " +
                        "Role = @Role, " +
                        "HotLine = @Hotline, " +
                        "ModifiedDate = @ModifiedDate, " +
                        "ModifiedBy = @ModifiedBy " +
                        "WHERE EmployeeID = @EmployeeID;";

                // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
                var parameters = new DynamicParameters();
                var dateTimeNow = DateTime.Now;
                parameters.Add("@EmployeeID", employeeID);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityPlace", employee.IdentityPlace);
                parameters.Add("@IdentityDate", employee.IdentityDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionName", employee.PositionName);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@BankAccount", employee.BankAccount);
                parameters.Add("@BankName", employee.BankName);
                parameters.Add("@BankBranch", employee.BankBranch);
                parameters.Add("@Role", employee.Role);
                parameters.Add("@HotLine", employee.HotLine);
                parameters.Add("@ModifiedDate", dateTimeNow);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);

                // Thực hiện gọi vào db để chạy câu lệnh INSERT INTO với tham số đầu vào ở trên
                var affectedRows = mySqlConnection.Execute(updateEmployeeCommand, parameters);

                // Xử lý kết quả trả về ở db
                if (affectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API xóa 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn xóa</param>
        /// <returns>ID của nhân viên đã xóa</returns>
        /// CreatedBy VMHieu 20/08/2022
        [HttpDelete("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Department>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteEmployee([FromRoute] Guid employeeID)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Update
                string deleteEmployeeCommand = "DELETE FROM employee WHERE EmployeeID = @EmployeeID";

                // Chuẩn bị tham số đầu vào cho câu lệnh Update
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);

                // Thực hiện gọi vào db để chạy câu lệnh UPDATE với tham số đầu vào ở trên
                var affectedRows = mySqlConnection.Execute(deleteEmployeeCommand, parameters);

                // Xử lý kết quả trả về ở db
                if (affectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// Lấy thông tin của 1 nhân viên thông qua ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên</param>
        /// <returns>Bản ghi chứa thông tin nv đó</returns>
        /// CreatedBy VMHieu 21/08/2022
        [HttpGet("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Department>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                string connectionString = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
                var mySqlConnection = new MySqlConnection(connectionString);

                string storedEmployeeName = "Proc_Employee_GetByEmployeeId";

                var parameters = new DynamicParameters();
                parameters.Add("@$EmployeeID", employeeID);

                var employee = mySqlConnection.QueryFirstOrDefault<Employee>(storedEmployeeName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (employee != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employee);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }

        }

        /// <summary>
        /// Lấy ra mã EmployeeCode lớn nhất sau đó + 1
        /// </summary>
        /// <returns> EmployeeCodeMax+1 </returns>
        /// CreatedBy VMHieu 21/08/2022
        [HttpGet("new-code")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Department>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult getNewEmployeeCode()
        {
            try
            {
                string connectionString = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
                var mySqlConnection = new MySqlConnection(connectionString);

                string storedNewEmployeeCode = "Proc_Employee_GetMaxCode";

                string newEmployeeCode = mySqlConnection.QueryFirstOrDefault<String>(storedNewEmployeeCode, commandType: System.Data.CommandType.StoredProcedure);

                string newCode = "NV" + (Int64.Parse(newEmployeeCode.Substring(2)) + 1).ToString();

                if (newCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new NewEmployeeCode()
                    {
                        ///data = newCode
                        data = newEmployeeCode
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
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
        [HttpGet("paging")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<Employee>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterEmployees(
            [FromQuery] string? keyword,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
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
                if (multipleResults != null)
                {
                    var employees = multipleResults.Read<Employee>().ToList();
                    var totalCount = multipleResults.Read<long>().Single();
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Employee>()
                    {
                        Data = employees,
                        TotalCount = totalCount
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        #endregion
    }
}
