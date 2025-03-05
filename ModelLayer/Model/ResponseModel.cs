using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class ResponseModel<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        public T? Data { get; set; } = default(T);
    }
   
}
