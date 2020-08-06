using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxplc_comm
{
   public class Error
    {

        //错误代码
        private string _ID;

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        //错误描述
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        //错误时间
        private DateTime _errorTime;

        public DateTime ErrorTime
        {
            get { return _errorTime; }
            set { _errorTime = value; }
        }
        public string[] toArray()
        {
            string[] element=new string[2];
            element[0] = this.ID;
            element[1] = this.Description; 
           
            return element ;
        }
        public string[] toArray4()
        {
            string[] element = new string[4];
            element[0] = "1";
            element[1] =this.ErrorTime.ToString("yyyy-MM-dd HH:mm:ss") ;
            element[2]=  this.ID;
            element[3]=this.Description;
            return element;
        }
        
    }
}
