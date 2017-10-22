namespace API
{
    using Newtonsoft.Json;
    #region using directives

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;
    using System.Text;
    using System.Threading;
    using System.Web;

    #endregion using directives

    internal class ApiProxy<TInterface> : RealProxy
    {
        private readonly String appType;
        private readonly String servicepoint;

        public ApiProxy()
            : base(typeof(TInterface))
        {
        }

        public ApiProxy(String servicePoint, String appType)
            : base(typeof(TInterface))
        {
            this.servicepoint = servicePoint;
            this.appType = appType;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var result = default(IMessage);
            var methodCallMessage = msg as IMethodCallMessage;

            var paramMap = new Dictionary<String, Object>();
            for (var i = 0; i < methodCallMessage.ArgCount; i++)
            {
                paramMap.Add(methodCallMessage.GetInArgName(i), methodCallMessage.GetArg((i)));
            }
            var apiAttribute = methodCallMessage.MethodBase.GetCustomAttributes(typeof(ApiAttribute), false)[0] as ApiAttribute;
            var requestUrl = apiAttribute.Url.ToLowerInvariant();
            if (paramMap.Count > 0)
            {
                var keyList = new List<String>(paramMap.Keys);
                for (var i = keyList.Count - 1; i >= 0; i--)
                {
                    var key = keyList[i];
                    var value = paramMap[key];
                    var holder = "{" + key.ToLowerInvariant() + "}";
                    if (requestUrl.Contains(holder))
                    {
                        requestUrl = requestUrl.Replace(holder, HttpUtility.UrlEncode(value.ToString()).Replace("+", "%20"));
                        paramMap.Remove(key);
                    }
                }
                if (apiAttribute.HttpMethod.Equals("GET"))
                {
                    var paramArr = paramMap.Select(param => String.Format("{0}={1}", param.Key, param.Value));
                    requestUrl += ("?" + String.Join("&", paramArr.ToArray()));
                }
            }
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            for (var retryCount = 0; retryCount < 3; retryCount++)
            {
                var httpRequest = WebRequest.Create(String.Format(@"{0}/{1}", this.servicepoint, requestUrl)) as HttpWebRequest;
                httpRequest.Method = apiAttribute.HttpMethod;
                httpRequest.Timeout = 60 * 60 * 1000;
                httpRequest.ContentType = apiAttribute.ContentType ?? "application/json";
                var accessToken = this.CreateToken(apiAttribute.HttpMethod, 0, this.servicepoint);
                //httpRequest.Headers.Add("Authorization", "Basic {0}".FormatWith(accessToken));
                httpRequest.Headers.Add("AppType", appType);
                try
                {
                    if (!apiAttribute.HttpMethod.Equals("GET") && paramMap.Count > 0)
                    {
                        if(httpRequest.ContentType.Equals("application/json"))
                        {
                            var jsonString = ConvertDictionaryToJson(paramMap);
                            using (var writer = new StreamWriter(httpRequest.GetRequestStream(), Encoding.UTF8))
                            {
                                writer.Write(jsonString);
                                writer.Flush();
                            }
                        }
                        else if(httpRequest.ContentType.Equals("application/x-www-form-urlencoded"))
                        {
                            var paramArr = paramMap.Select(param => String.Format("{0}={1}", param.Key, param.Value));
                            var jsonString = String.Join("&", paramArr.ToArray());
                            using (var writer = new StreamWriter(httpRequest.GetRequestStream(), Encoding.UTF8))
                            {
                                writer.Write(jsonString);
                                writer.Flush();
                            } 
                        }
                    }
                    var responseText = this.GetResponseText(httpRequest);
                    var returnValue = JsonConvert.DeserializeObject(responseText, (methodCallMessage.MethodBase as MethodInfo).ReturnType);
                    var apiModel = returnValue as ApiModelBase;
                    //if (apiModel != null && apiModel.ErrorCode != 0)
                    //{
                    //    throw new ApiException(apiModel.ErrorMessage) { ErrorCode = apiModel.ErrorCode };
                    //}
                    result = new ReturnMessage(returnValue, null, 0, null, null);
                    break;
                }
                catch (Exception e)
                {
                    if (retryCount == 2)
                    {
                        result = new ReturnMessage(e, (IMethodCallMessage)msg);
                    }
                    Thread.Sleep(1000);
                }
            }
            return result;
        }

        public String GetResponseText(WebRequest httpRequest)
        {
            WebResponse response;
            var hasError = false;
            try
            {
                response = httpRequest.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                hasError = true;
                response = e.Response as HttpWebResponse;
            }
            String responseText;
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseText = reader.ReadToEnd();
            }
            response.Close();
            response.Dispose();
            if (hasError)
            {
                //Trace.TraceError(String.Format("The error response content is: {0}", responseText));
                ////Response error context is a html document which the API server system has automatically generated.
                //if (responseText.StartsWith("<!DOCTYPE html", StringComparison.OrdinalIgnoreCase))
                //{
                //    throw new ApiException("Server error") { ErrorCode = 404 };
                //}
                //var error = JsonConvert.DeserializeObject<Error>(responseText);
                //throw new ApiException(error.Message) { ErrorCode = error.ErrorCode };
            }
            return responseText;
        }

        private static String ConvertDictionaryToJson(Dictionary<String, Object> parameterMap)
        {
            if (parameterMap.Count == 1)
            {
                return JsonConvert.SerializeObject(parameterMap.Values.First());
            }
            var parameters = parameterMap.Select(param =>
               String.Format("\"{0}\": {1}", param.Key, JsonConvert.SerializeObject(param.Value)));
            return "{" + String.Join(",", parameters.ToArray()) + "}";
        }

        private String CreateToken(String method, Int32 role, String requestUrl)
        {
            //var digitalSignatureHelperFactory = new DigitalSignatureHelperFactory();
            //var digitalSignatureService = digitalSignatureHelperFactory.Create(this.appType);
            //var plainText = new StringBuilder(method);
            //plainText.Append(":")
            //    .Append(DateTime.UtcNow.Ticks)
            //    .Append(":")
            //    .Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(requestUrl)));
            //var signature = digitalSignatureService.SignData(plainText.ToString());
            //return plainText + ":" + Convert.ToBase64String(Encoding.UTF8.GetBytes(signature));
            return "";
        }
    }
}