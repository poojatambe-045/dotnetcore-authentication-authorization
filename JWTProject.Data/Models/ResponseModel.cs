using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTProject.Data.Models
{
    public class ResponseModel
    {
        public string token { get; set; }    
        public string message { get; set; }
        public bool success {  get; set; }
    }
}
