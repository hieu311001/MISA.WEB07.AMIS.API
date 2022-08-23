using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB07.AMIS.BL.Interfaces
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi của bảng T</returns>
        /// CreatedBy VMHieu 23/08/2022
        public IEnumerable<T> GetAll();
        /// <summary>
        /// Lấy dữ liệu bản ghi qua id
        /// </summary>
        /// <param name="id">Id của bản ghi cần lấy</param>
        /// <returns>Thông tin bản ghi</returns>
        /// CreatedBy VMHieu 23/08/2022
        public T GetById(Guid id);

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Insert(T entity);

        /// <summary>
        /// Sửa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Update(T entity);

        /// <summary>
        /// Xóa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Delete(Guid id);
    }
}
