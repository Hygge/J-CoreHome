using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Result
{
    // 响应体封装
    public class R
    {
        public int code { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
        public DateTime dateTime = DateTime.Now;

        public R() { }
        public R(int code, string message, object data) 
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
        public R(int code, string message )
        {
            this.code = code;
            this.message = message;          
        }
        public static R Ok()
        {
            return new R(200, "ok!");
        }
        public static R Ok(object data)
        {
            return new R(200, "ok!", data);
        }
        public static R Ok(object data, string message)
        {
            return new R(200, message, data);
        }
        public static R Ok(int code, object data)
        {
            return new R(code, "ok!", data);
        }

        public static R Fail(string message)
        {
            return new R(424, message);
        }
        public static R Fail(int code, string message)
        {
            return new R(code, message);
        }


    }
}
