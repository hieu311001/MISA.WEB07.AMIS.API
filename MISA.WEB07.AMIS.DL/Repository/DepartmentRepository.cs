using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MISA.WEB07.AMIS.Common.Entities;
using MISA.WEB07.AMIS.DL.Interfaces;
using MySqlConnector;

namespace MISA.WEB07.AMIS.DL.Repository
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentDL
    {

    }
}
