using Microsoft.AspNetCore.Mvc;
using MISA.WEB07.AMIS.BL.Interfaces;
using MISA.WEB07.AMIS.Common.Entities;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MISA.WEB07.AMIS.API.VMH.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        #region Field

        private IEmployeeBL _employeeBL;

        #endregion

        #region Constructor

        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
        {
            _employeeBL = employeeBL;
        }

        #endregion

        #region Method

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
                string newCode = _employeeBL.getNewEmployeeCode();

                    return StatusCode(StatusCodes.Status200OK, new NewEmployeeCode()
                    {
                        data = newCode
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// API Lấy danh sách nhân viên cho phép lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa cần tìm kiếm</param>
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
                var multipleResults = _employeeBL.FilterEmployees(keyword, pageSize, pageNumber);

                return Ok(multipleResults);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return HandleException(exception);
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="ids">Chuỗi chứa các id của nhân viên cần xóa</param>
        /// <returns></returns>
        /// CreatedBy VMHieu 09/09/2022
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpDelete("deleteMultiple")]
        public IActionResult deleteMultiple(string ids)
        {
            try
            {
                return Ok(_employeeBL.deleteMultiple(ids));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xuất dữ liệu Employee ra file Excel
        /// </summary>
        /// <param name="employees">Danh sách employee</param>
        /// <returns>File</returns>
        /// CreatedBy VMHieu 09/09/2022
        [HttpGet("Export")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult Export(string? keyword)
        {
            try
            {
                var response = _employeeBL.ExportService(keyword);
                return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Danh_sach_nhan_vien.xlsx");
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }


        #endregion
    }
}
