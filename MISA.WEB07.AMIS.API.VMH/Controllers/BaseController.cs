using Microsoft.AspNetCore.Mvc;
using MISA.WEB07.AMIS.BL.Interfaces;
using MISA.WEB07.AMIS.Common;

namespace MISA.WEB07.AMIS.API.VMH.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi của bảng T</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _baseBL.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Lấy dữ liệu bản ghi qua id
        /// </summary>
        /// <param name="id">Id của bản ghi cần lấy</param>
        /// <returns>Thông tin bản ghi</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _baseBL.GetById(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpPost]
        public IActionResult Insert(T entity)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created, _baseBL.Insert(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Sửa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpPut("{id}")]
        public IActionResult Update(T entity, Guid id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _baseBL.Update(entity, id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xóa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _baseBL.Delete(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Exception xử lý ngoại lệ
        /// </summary>
        /// <param name="ex">Ngoại lệ được bắt</param>
        /// <returns></returns>
        /// CreatedBy VMHieu 28/08/2022
        protected IActionResult HandleException(Exception ex)
        {
            var error = new
            {
                devMsg = ex.Message,
                userMsg = Resources.ResourceManager.GetString(name: "ErrorException"),
                errorMsg = ex.Data["Error"]
            };

            if (ex is ErrorException)
            {
                return BadRequest(error);
            }
            return StatusCode(500, error);
        }

        #endregion
    }
}
