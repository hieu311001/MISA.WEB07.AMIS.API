using MISA.WEB07.AMIS.BL.Interfaces;
using MISA.WEB07.AMIS.DL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB07.AMIS.BL.Services
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi của bảng T</returns>
        /// CreatedBy VMHieu 23/08/2022
        public IEnumerable<T> GetAll()
        {
            return _baseDL.GetAll();
        }

        /// <summary>
        /// Lấy dữ liệu bản ghi qua id
        /// </summary>
        /// <param name="id">Id của bản ghi cần lấy</param>
        /// <returns>Thông tin bản ghi</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual T GetById(Guid id)
        {
            return _baseDL.GetById(id);
        }

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual int Insert(T entity)
        {
            return _baseDL.Insert(entity);
        }

        /// <summary>
        /// Sửa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual int Update(T entity)
        {
            return _baseDL.Update(entity);
        }

        /// <summary>
        /// Xóa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual int Delete(Guid id)
        {
            return _baseDL.Delete(id);
        }

        #endregion
    }
}
