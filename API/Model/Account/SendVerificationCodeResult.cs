
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API
{
    public class SendVerificationCodeResult:ApiModelBase
    {

        public Data data { get; set; }
        

        public class Data
        {
            public string msgId { get; set; }
        } 
    }
}