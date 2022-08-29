﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using MISA.WEB07.AMIS.Common.Entities;
using MISA.WEB07.AMIS.DL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using MISA.WEB07.AMIS.Common;

namespace MISA.WEB07.AMIS.DL.Repository
{
    public class BaseRepository<T> : IBaseDL<T>
    {
        #region Field

        /// <summary>
        /// Khởi tạo variable
        /// </summary>
        protected string connectionString;
        protected MySqlConnection mySqlConnection;
        protected string className;
        protected List<string> errorList;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <exception cref="ErrorException">Gọi đến khi lỗi kết nối database</exception>
        /// CreatedBy VMHieu 28/08/2022
        public BaseRepository(IConfiguration configuration)
        {
            errorList = new List<string>();
            // Khai báo thông tin kết nối
            connectionString = configuration.GetConnectionString("dataBase");
            // Khai báo tên bảng
            className = typeof(T).Name;
            mySqlConnection = new MySqlConnection(connectionString);
            if (mySqlConnection != null && mySqlConnection.State != ConnectionState.Open)
            {
                mySqlConnection.Open();
            }
            else
            {
                throw new ErrorException(devmsg: Resources.ResourceManager.GetString(name: "ExceptionConnection"));
            }
        }

        /// <summary>
        /// Giải phóng bộ nhớ
        /// </summary>
        /// CreatedBy VMHieu 28/02/2022
        public void Dispose()
        {
            mySqlConnection.Dispose();
            mySqlConnection.Close();
        }

        #endregion

        #region Method
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi của bảng T</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual IEnumerable<T> GetAll()
        {
            var getAllEmployeeCommand = $"Proc_{className}_GetAll";

            var result = mySqlConnection.Query<T>(getAllEmployeeCommand, commandType: System.Data.CommandType.StoredProcedure);

            if (result == null)
            {
                throw new ErrorException(devmsg: Resources.ResourceManager.GetString(name: "NullData"));
            }
            Dispose();
            return result;
        }

        /// <summary>
        /// Lấy dữ liệu bản ghi qua id
        /// </summary>
        /// <param name="id">Id của bản ghi cần lấy</param>
        /// <returns>Thông tin bản ghi</returns>
        /// CreatedBy VMHieu 23/08/2022
        public virtual T GetById(Guid id)
        {
            var getByIdCommand = $"Proc_{className}_GetById";

            var parameters = new DynamicParameters();
            parameters.Add($"${className}Id", id);

            var result = mySqlConnection.QueryFirstOrDefault<T>(getByIdCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

            if(result == null)
            {
                throw new ErrorException(devmsg: Resources.ResourceManager.GetString(name: "NullData"));
            }
            Dispose();
            return result;
        }

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Insert(T entity)
        {
            var storeProc = $"Proc_{className}_Insert";

            var parameters = new DynamicParameters(entity);

            var result = mySqlConnection.Execute(storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            if(result == 0)
            {
                throw new ErrorException(devmsg: Resources.ResourceManager.GetString(name: "ExceptionInsert"));
            }
            Dispose();
            return result;
        }

        /// <summary>
        /// Sửa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Update(T entity, Guid id)
        {
            var storeProc = $"Proc_{className}_Update";

            var parameters = new DynamicParameters(entity);
            parameters.Add($"{className}ID", id);

            var result = mySqlConnection.Execute(storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            if(result == 0)
            {
                throw new ErrorException(devmsg: Resources.ResourceManager.GetString(name: "ExceptionUpdate"));
            }
            Dispose();
            return result;
        }

        /// <summary>
        /// Xóa 1 bản ghi 
        /// </summary>
        /// <returns>Số dòng thay đổi sau câu lệnh</returns>
        /// CreatedBy VMHieu 23/08/2022
        public int Delete(Guid id)
        {
            var getByIdCommand = $"Proc_{className}_Delete";

            var parameters = new DynamicParameters();
            parameters.Add($"${className}Id", id);

            var result = mySqlConnection.Execute(getByIdCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            if (result == 0)
            {
                throw new ErrorException(devmsg: Resources.ResourceManager.GetString(name: "ExceptionDelete"));
            }
            Dispose();
            return result;
        }

        #endregion
    }
}
