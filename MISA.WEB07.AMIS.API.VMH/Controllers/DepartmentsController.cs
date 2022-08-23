using Microsoft.AspNetCore.Mvc;
using MISA.WEB07.AMIS.BL.Interfaces;
using MISA.WEB07.AMIS.Common.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.WEB07.AMIS.API.VMH.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController<Department>
    {
        #region Field

        private IDepartmentBL _departmentBL;

        #endregion

        #region Constructor

        public DepartmentsController(IDepartmentBL departmentBL) : base(departmentBL)
        {
            _departmentBL = departmentBL;
        }

        #endregion

        #region Method

        #endregion
    }
}
