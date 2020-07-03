using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayUp.Models
{
    class Error:ObservableObject
    {
        public int ErrorCode { get; set; }
        public DateTime ErrorTime { get; set; }
        public string ErrorDescription { get; set; }
        [JsonIgnore]
        public string ErrorRegister { get; set; }
    }
}
