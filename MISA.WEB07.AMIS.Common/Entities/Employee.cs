using MISA.WEB07.AMIS.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.WEB07.AMIS.Common.Entities
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    [Table("employee")]
    public class Employee
    {
        #region Property

        /// <summary>
        /// ID nhân viên
        /// </summary>
        [Key]
        public Guid EmployeeID { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required(ErrorMessage = "e005")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính Alt Enter
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Giới tính VN
        /// </summary>
        public string? GenderName
        {
            get
            {
                switch (Gender)
                {
                    case Enum.Gender.Male:
                        return Resources.ResourceManager.GetString("Gender_Male");
                    case Enum.Gender.Female:
                        return Resources.ResourceManager.GetString("Gender_Female");
                    default:
                        return Resources.ResourceManager.GetString("Gender_Other");
                }
            }
        }

        /// <summary>
        /// Số CMND
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Nơi cấp CMND 
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Ngày cấp CMND
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        public string? HotLine { get; set; }

        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// ID đơn vị
        /// </summary>
        [Required(ErrorMessage = "e006")]
        public Guid? DepartmentID { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        public string? BankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string? BankBranch { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Vai trò
        /// </summary>
        public Role? Role { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }

        #endregion
    }
}