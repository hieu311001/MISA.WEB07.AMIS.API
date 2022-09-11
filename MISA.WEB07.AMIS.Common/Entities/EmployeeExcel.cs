using MISA.WEB07.AMIS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB07.AMIS.Common.Entities
{
    public class EmployeeExcel
    {
        // Số thứ tự
        public int Index { get; set; }

        // Mã nhân viên
        public string? EmployeeCode { get; set; }

        // Tên Nhân viên
        public string? EmployeeName { get; set; }

        // Giới tính
        public string? GenderName { get; set; }

        // Ngày sinh
        public DateTime? DateOfBirth { get; set; }

        // Tên đơn vị
        public string? DepartmentName { get; }

        // Tên chức danh
        public string? PositionName { get; }

        // Số CMND
        public string? IdentityNumber { get; set; }

        // Ngày cấp
        public DateTime? IdentityDate { get; set; }

        // Nơi cấp
        public string? IdentityPlace { get; set; }

        // Email
        public string? Email { get; set; }

        // Số điện thoại di động
        public string? PhoneNumber { get; set; }

        // Số điện thoại cố định
        public string? HotLine { get; set; }

        // Địa chỉ
        public string? Address { get; set; }

        // Tài khoản ngân hàng
        public string? BankAccount { get; set; }

        // Tên ngân hàng
        public string? BankName { get; set; }

        // Tên chi nhánh
        public string? BankBranch { get; set; }

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="index">Số thứ tự</param>
        /// <param name="employee">Dữ liệu Employee</param>
        /// CreatedBy VMHieu 09/09/2022
        public EmployeeExcel(int index, Employee employee)
        {
            Index = index;
            EmployeeCode = employee.EmployeeCode;
            EmployeeName = employee.EmployeeName;
            DepartmentName = employee.DepartmentName;
            PositionName = employee.PositionName;
            DateOfBirth = employee.DateOfBirth;
            GenderName = employee.GenderName;
            IdentityNumber = employee.IdentityNumber;
            IdentityDate = employee.IdentityDate;
            IdentityPlace = employee.IdentityPlace;
            Email = employee.Email;
            PhoneNumber = employee.PhoneNumber;
            HotLine = employee.HotLine;
            Address = employee.Address;
            BankAccount = employee.BankAccount;
            BankName = employee.BankName;
            BankBranch = employee.BankBranch;
        }
    }
}
