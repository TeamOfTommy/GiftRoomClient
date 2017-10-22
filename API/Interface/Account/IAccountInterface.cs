
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API
{
    public interface IAccountInterface
    {
        [Api(Url= "userapi/user/login/{telnumber}.do", HttpMethod ="GET")]
        CheckPasswordResult CheckPassword(string telnumber, string password);

        [Api(Url = "userapi/user/reg/{telnumber}.do", HttpMethod = "GET", ContentType= "application/x-www-form-urlencoded")]
        SignupResult Signup(string nickname, string telnumber, string password, string code,string msgId );


        [Api(Url = "userapi/user/smscode/send/{telnumber}.do", HttpMethod = "GET")]
        SendVerificationCodeResult SendVerificationCode(string telnumber); 
    }
}