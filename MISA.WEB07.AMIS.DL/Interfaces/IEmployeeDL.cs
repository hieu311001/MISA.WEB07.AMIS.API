using MISA.WEB07.AMIS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB07.AMIS.DL.Interfaces
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// Lấy ra mã EmployeeCode lớn nhất sau đó + 1
        /// </summary>
        /// <returns> EmployeeCodeMax+1 </returns>
        /// CreatedBy VMHieu 21/08/2022
        public string getNewEmployeeCode();

        /// <summary>
        /// API Lấy danh sách nhân viên cho phép lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa cần tìm kiếm</param>
        /// <param name="pageSize">Số trang muốn lấy</param>
        /// <param name="pageNumber">Thứ tự trang muốn lấy</param>
        /// <returns>Một đối tượng gồm:
        /// + Danh sách nhân viên thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số nhân viên thỏa mãn điều kiện</returns>
        /// Created by VMHieu (21/08/2022)
        public PagingData<Employee> FilterEmployees(
            string? keyword,
            int pageSize = 10,
            int pageNumber = 1);

        /// <summary>
        /// Kiểm tra trùng mã nhân viên
        /// </summary>
        /// <param name="employeeCode">mã nhân viên đang xét</param>
        /// <returns>true-nếu có mã nhân viên trùng false- nếu mã nhân viên không trùng</returns>
        /// CreatedBy VMHieu 28/08/2022
        public bool IsDuplicate(Guid? employeeID, string employeeCode);

        /// <summary>
        /// Xóa nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="ids">Chuỗi chứa các id của nhân viên cần xóa</param>
        /// <returns></returns>
        /// CreatedBy VMHieu 09/09/2022
        public int deleteMultiple(string ids);
    }
}
