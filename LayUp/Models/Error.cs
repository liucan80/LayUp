﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayUp.Models
{
  public  class Error
    {
        public int ID { get; set; }
        public string ErrorRegister { get; set; }
        public string ErrorDescription { get; set; }
        public string Level { get; set; }
    }
}
