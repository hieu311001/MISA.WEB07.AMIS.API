using MISA.WEB07.AMIS.BL.Interfaces;
using MISA.WEB07.AMIS.Common;
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
        protected List<string> errorList;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
            errorList = new List<string>();
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
            var isValid = Validate(entity, Guid.Empty);
            if (isValid)
            {
                return _baseDL.Insert(entity);
            } else
            {
                throw new ErrorException(devmsg: Resources.ResourceManager.GetString(name: "InvalidData"), listErrors: errorList);
            }
}

        /// <summary>
        /// Sửa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual int Update(T entity, Guid id)
        {
            var isValid = Validate(entity, id);
            if (isValid)
            {
                return _baseDL.Update(entity, id);
            }
            else
            {
                throw new ErrorException(devmsg: Resources.ResourceManager.GetString(name: "InvalidData"), listErrors: errorList);
            }
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

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu bản ghi cần validate</param>
        /// <returns>true - nếu hợp lệ, false - nếu không hợp lệ</returns>
        /// CreatedBy VMHieu 28/08/2022
        protected virtual bool Validate(T entity, Guid? id)
        {
            return true;
        }

        #endregion
    }
}
