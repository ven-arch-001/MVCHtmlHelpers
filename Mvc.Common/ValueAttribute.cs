using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Common
{
    public class ValueAttribute : Attribute
    {
        public string Data { set; get; } = string.Empty;

        public ValueAttribute()
        { }

        public ValueAttribute(string data)
        {
            this.Data = data;
        }
    }
}
