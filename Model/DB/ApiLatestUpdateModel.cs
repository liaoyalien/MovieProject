using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class ApiLatestUpdateModel
    {
        public string ApiName { get; set; }
        public string MethodName { get; set; }
        public int CurrentPages { get; set; }
        public int TotalPages { get; set; }
    }
}
