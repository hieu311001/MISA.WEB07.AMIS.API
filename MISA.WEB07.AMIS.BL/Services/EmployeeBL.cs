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

        public object FilterEmployees(string? keyword, int pageSize = 10, int pageNumber = 1)
        {
            return _employeeDL.FilterEmployees(keyword, pageSize, pageNumber);
        }

        public string getNewEmployeeCode()
        {
            return _employeeDL.getNewEmployeeCode();
        }

        #endregion
    }
}
