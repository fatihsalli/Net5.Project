using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Project.WEB.Utils
{
    public class SessionHelper
    {
        //Set
        //Client ile server arasındaki haberleşme http ile gerçekleştirilir. Websitelerindeki https://www.google.com s takısı da güvenlik sertifikası ile ilgilidir.
        public static void SetProductJson(ISession session,string key,object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        //Get
        public static T GetProductFromJson<T>(ISession session,string key)
        {
            var result=session.GetString(key);

            if (result==null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

    }
}
