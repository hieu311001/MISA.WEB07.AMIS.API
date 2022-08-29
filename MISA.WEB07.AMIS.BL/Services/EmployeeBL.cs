using MISA.WEB07.AMIS.BL.Interfaces;
using MISA.WEB07.AMIS.Common;
using MISA.WEB07.AMIS.Common.Entities;
using MISA.WEB07.AMIS.DL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.WEB07.AMIS.BL.Services
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {

        #region Field

        private IEmployeeDL _employeeDL;

        #endregion

        #region Constructor

        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Validate dữ liệu nhân viên
        /// </summary>
        /// <param name="entity">Dữ liệu bản ghi cần validate</param>
        /// <returns>true - nếu hợp lệ, false - nếu không hợp lệ</returns>
        /// CreatedBy VMHieu 28/08/2022
        protected override bool Validate(Employee employee)
        {
            // Xử lý nghiệp vụ
            // Thông tin mã nhân viên bắt buộc nhập
            if (string.IsNullOrEmpty(employee.EmployeeCode))
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "InvalidEmployeeCode"));
            }
            // Họ tên không được phép để trống
            if (string.IsNullOrEmpty(employee.EmployeeName))
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "InvalidFullName"));
            }
            // Phòng ban không được phép để trống
            if (employee.DepartmentID == Guid.Empty || employee.DepartmentID == null)
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "InvalidDepartment"));
            }
            // Email phải đúng định dạng
            if (!string.IsNullOrEmpty(employee.Email) && !IsValidEmail(employee.Email))
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "InvalidEmail"));
            }
            // Ngày sinh, ngày cấp không lớn hơn ngày hiện tại
            if (employee.DateOfBirth.HasValue && employee.DateOfBirth >= DateTime.Now)
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "InvalidDate"));
            }
            if (employee.IdentityDate.HasValue && employee.IdentityDate >= DateTime.Now)
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "InvalidDate"));
            }
            // Kiểm tra mã nhân viên đã tồn tại hay chưa
            if (!string.IsNullOrEmpty(employee.EmployeeCode) && _employeeDL.IsDuplicate(employee.EmployeeID, employee.EmployeeCode))
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "DuplicateEmployeeCode"));
            }

            if (errorList != null)
            {
                return false;
            }
            else
            {
                return true;
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
        public object FilterEmployees(string? keyword, int pageSize = 10, int pageNumber = 1)
        {
            return _employeeDL.FilterEmployees(keyword, pageSize, pageNumber);
        }

        /// <summary>
        /// Lấy ra mã EmployeeCode lớn nhất sau đó + 1
        /// </summary>
        /// <returns> EmployeeCodeMax+1 </returns>
        /// CreatedBy VMHieu 21/08/2022
        public string getNewEmployeeCode()
        {
            return _employeeDL.getNewEmployeeCode();
        }

        /// <summary>
        /// Kiểm tra định dạng Email
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>true = Đúng định dạng, false = Sai định dạng</returns>
        /// CreatedBy VMHieu 28/08/2022
        public Boolean IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            return regex.IsMatch(email);
        }


        #endregion
    }
}
