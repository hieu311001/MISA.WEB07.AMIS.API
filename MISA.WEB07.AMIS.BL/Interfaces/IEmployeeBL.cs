using MISA.WEB07.AMIS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB07.AMIS.BL.Interfaces
{
    public interface IEmployeeBL : IBaseBL<Employee>
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
        /// <returns>Một đối tượng gồm:
        /// + Danh sách nhân viên thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số nhân viên thỏa mãn điều kiện</returns>
        /// Created by VMHieu (21/08/2022)
        public object FilterEmployees(
            string? keyword,
            int pageSize = 10,
            int pageNumber = 1);

        /// <summary>
        /// Lấy dữ liệu Employee ra file Excel
        /// </summary>
        /// <returns>Dữ liệu file</returns>
        /// CreatedBy VMHieu 09/09/2022
        public MemoryStream ExportService(string? keyword);

        /// <summary>
        /// Xóa nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="ids">Chuỗi chứa các id của nhân viên cần xóa</param>
        /// <returns></returns>
        /// CreatedBy VMHieu 09/09/2022
        public int deleteMultiple(string ids);
    }
}
