using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using MISA.WEB07.AMIS.Common.Entities;
using MISA.WEB07.AMIS.DL.Interfaces;

namespace MISA.WEB07.AMIS.DL.Repository
{
    public class BaseRepository<T> : IBaseDL<T>
    {
        #region Field

        private const string CONNECTION_STRING = "Server=localhost;Port=3306;Database=amis_db;Uid=root;Pwd=Hieu311001.";
        private string className = typeof(T).Name;

        #endregion

        #region Method
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi của bảng T</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual IEnumerable<T> GetAll()
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                var getAllEmployeeCommand = $"Proc_{className}_GetAll";

                var result = mySqlConnection.Query<T>(getAllEmployeeCommand, commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }

        /// <summary>
        /// Lấy dữ liệu bản ghi qua id
        /// </summary>
        /// <param name="id">Id của bản ghi cần lấy</param>
        /// <returns>Thông tin bản ghi</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual T GetById(Guid id)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                var getByIdCommand = $"Proc_{className}_GetById";

                var parameters = new DynamicParameters();
                parameters.Add($"${className}Id", id);

                var result = mySqlConnection.QueryFirstOrDefault<T>(getByIdCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Insert(T entity)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                var storeProc = $"Proc_{className}_Insert";

                var parameters = new DynamicParameters(entity);

                var result = mySqlConnection.Execute(storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }

        /// <summary>
        /// Sửa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Update(T entity)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                var storeProc = $"Proc_{className}_Update";

                var parameters = new DynamicParameters(entity);

                var result = mySqlConnection.Execute(storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }

        /// <summary>
        /// Xóa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Delete(Guid id)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                var getByIdCommand = $"Proc_{className}_Delete";

                var parameters = new DynamicParameters();
                parameters.Add($"${className}Id", id);

                var result = mySqlConnection.Execute(getByIdCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }

        #endregion
    }
}
