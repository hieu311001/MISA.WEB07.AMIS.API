using MISA.WEB07.AMIS.BL.Interfaces;
using MISA.WEB07.AMIS.Common;
using MISA.WEB07.AMIS.Common.Entities;
using MISA.WEB07.AMIS.DL.Interfaces;
using OfficeOpenXml;
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
        /// <param name="employee">Dữ liệu bản ghi cần validate</param>
        /// <param name="id> ID bản ghi cần validate</param>
        /// <returns>true - nếu hợp lệ, false - nếu không hợp lệ</returns>
        /// CreatedBy VMHieu 28/08/2022
        protected override bool Validate(Employee employee, Guid? id)
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
            // Đơn vị không được phép để trống
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
                errorList.Add(Resources.ResourceManager.GetString(name: "InvalidDateOfBirth"));
            }
            if (employee.IdentityDate.HasValue && employee.IdentityDate >= DateTime.Now)
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "InvalidIdentityDate"));
            }
            // Kiểm tra mã nhân viên đã tồn tại hay chưa
            if (!string.IsNullOrEmpty(employee.EmployeeCode) && _employeeDL.IsDuplicate(id, employee.EmployeeCode))
            {
                errorList.Add(Resources.ResourceManager.GetString(name: "DuplicateEmployeeCode"));
            }

            if (errorList.Count > 0)
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
        public PagingData<Employee> FilterEmployees(string? keyword, int pageSize = 10, int pageNumber = 1)
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


        /// <summary>
        /// Lấy dữ liệu Employee ra file Excel
        /// </summary>
        /// <returns>Dữ liệu file</returns>
        /// CreatedBy VMHieu 09/09/2022
        public MemoryStream ExportService(string? keyword)
        {
            // Lấy dữ liệu Employee theo dạng EmployeeExcel:
            var employees = _employeeDL.FilterEmployees(keyword, 1000, 1);
            if (employees == null)
            {
                throw new NullReferenceException();
            }
            var employeeExcel = new List<EmployeeExcel>();
            var index = 1;
            foreach (var employee in employees.Data)
            {
                employeeExcel.Add(new EmployeeExcel(index, employee));
                index++;
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                // Tạo Sheet Excel
                var workSheet = package.Workbook.Worksheets.Add("Danh_sach_nhan_vien");

                // Set default width cho tất cả column
                workSheet.DefaultColWidth = 15;

                // Nạp dữ liệu
                workSheet.Cells.LoadFromCollection(employeeExcel, false);

                // Phần tiêu đề:
                workSheet.InsertRow(1, 3);
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A2:I2"].Merge = true;

                // Nội dung tiêu đề:
                workSheet.Cells["A1"].LoadFromText("DANH SÁCH NHÂN VIÊN");

                // Style tiêu đề:
                workSheet.Cells["A1"].Style.Font.Bold = true;
                workSheet.Cells["A1"].Style.Font.Size = 16;
                workSheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Phần tên cột bảng:
                workSheet.Cells[3, 1].LoadFromText("STT, Mã nhân viên, Tên nhân viên, Giới tính, Ngày sinh, Chức danh, Tên Đơn Vị, Số tài khoản, Tên ngân hàng");

                // Định dạng ngày tháng là dd/mm/yyyy:
                workSheet.Column(5).Style.Numberformat.Format = "dd/mm/yyyy";
                
                // Căn giữa ngày sinh và STT
                workSheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Style hàng tên cột
                workSheet.Cells["A3:I3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["A3:I3"].Style.Font.Bold = true;
                workSheet.Cells["A3:I3"].Style.Font.Size = 10;
                workSheet.Cells["A3:I3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells["A3:I3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(216, 216, 216));

                // Lấy range vào tạo format cho range đó
                using (var range = workSheet.Cells[$"A3:I{employeeExcel.Count + 3}"])
                {
                    // Set Border
                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                for (int i = 1; i <= 18; i++)
                {
                    workSheet.Column(i).AutoFit();
                }

                // Lưu lại dữ liệu:
                package.Save();
            }
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Xóa nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="ids">Chuỗi chứa các id của nhân viên cần xóa</param>
        /// <returns></returns>
        /// CreatedBy VMHieu 09/09/2022
        public int deleteMultiple(string ids)
        {
            return _employeeDL.deleteMultiple(ids);
        }


        #endregion
    }
}
