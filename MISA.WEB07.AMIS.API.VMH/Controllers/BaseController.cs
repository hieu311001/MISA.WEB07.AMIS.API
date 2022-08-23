using Microsoft.AspNetCore.Mvc;
using MISA.WEB07.AMIS.BL.Interfaces;

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
        public IEnumerable<T> GetAll()
        {
            return _baseBL.GetAll();
        }

        /// <summary>
        /// Lấy dữ liệu bản ghi qua id
        /// </summary>
        /// <param name="id">Id của bản ghi cần lấy</param>
        /// <returns>Thông tin bản ghi</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpGet("{id}")]
        public T GetById(Guid id)
        {
            return _baseBL.GetById(id);
        }

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpPost]
        public int Insert(T entity)
        {
            return _baseBL.Insert(entity);
        }

        /// <summary>
        /// Sửa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpPut]
        public int Update(T entity)
        {
            return _baseBL.Update(entity);
        }

        /// <summary>
        /// Xóa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        [HttpDelete]
        public int Delete(Guid id)
        {
            return _baseBL.Delete(id);
        }

        #endregion
    }
}
