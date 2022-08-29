using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB07.AMIS.Common
{
    /// <summary>
    /// Xử lý các exception
    /// CreatedBy VMHieu 28/08/2022
    /// </summary>
    public class ErrorException : Exception
    {
        // Khai báo các thuộc tính
        string devMsg;
        IDictionary errors;

        // Khởi tạo
        public ErrorException(string devmsg = null, List<string> listErrors = null)
        {
            this.devMsg = devmsg;
            errors = new Dictionary<string, List<string>>();
            errors.Add("Error", listErrors);
        }

        // Override
        public override string Message => this.devMsg;
        public override IDictionary Data => this.errors;
    }
}
