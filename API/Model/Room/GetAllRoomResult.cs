 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API
{
   public  class GetAllRoomResult : ApiModelBase
    {
        public List<Data> data { get; set; }


        public class Data
        {
            public string imgUrl { get; set; }

            public string RoomTitle { get; set; }

            public int UserOnlineNum { get; set; }
        }
    }
}