using System;
namespace LayUp.Models
{
   public class ErrorRecord
    {
        // [JsonIgnore]
        public int ID { get; set; }
        public string ErrorRegister { get; set; }
        public string ErrorDescription { get; set; }
        public string Level { get; set; }
        //错误发生时间  
        public DateTime ErrorTime{ get; set;}
    }
}
