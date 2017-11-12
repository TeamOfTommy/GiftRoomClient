 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API.Interface.Account
{
    public interface IRoomInterface
    {
        [Api(Url = "roomapi/room/getall.do", HttpMethod = "GET")]
        GetAllRoomResult GetAll();
    }
}