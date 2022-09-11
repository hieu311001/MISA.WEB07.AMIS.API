using MISA.WEB07.AMIS.BL.Interfaces;
using MISA.WEB07.AMIS.Common.Entities;
using MISA.WEB07.AMIS.DL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB07.AMIS.BL.Services
{
    public class DepartmentBL : BaseBL<Department>, IDepartmentBL
    {
        #region Field

        private IDepartmentDL _departmentDL;

        #endregion

        #region Constructor

        public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
        {
            _departmentDL = departmentDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Validate dữ liệu nhân viên
        /// </summary>
        /// <param name="entity">Dữ liệu bản ghi cần validate</param>
        /// <returns>true - nếu hợp lệ, false - nếu không hợp lệ</returns>
        /// CreatedBy VMHieu 28/08/2022
        protected override bool Validate(Department department, Guid? id)
        {
            // Xử lý nghiệp vụ
            return true;
        }

        #endregion
    }
}
