using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WEB07.AMIS.Common.Entities;
using MISA.WEB07.AMIS.DL.Interfaces;
using MySqlConnector;

namespace MISA.WEB07.AMIS.DL.Repository
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentDL
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="configuration"></param>
        /// CreatedBy VMHieu 28/08/2022
        public DepartmentRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
