using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Project.WEB.Utils
{
    public static class SessionHelper
    {
        //Client ile server arasındaki haberleşme http ile gerçekleştirilir. Websitelerindeki https://www.google.com s takısı da güvenlik sertifikası ile ilgilidir.
        //Set
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

        //Remove
        public static void RemoveSession(this ISession session, string key)
        {
            session.Remove(key);
        }



    }
}
