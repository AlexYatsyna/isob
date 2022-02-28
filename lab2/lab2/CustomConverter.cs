
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class CustomConverter <T>
    {
        public static /*byte[]*/string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data).ToString();
            // var js = JsonConvert.SerializeObject(data);
            //return Encoding.UTF8.GetBytes(js);
        }

        public static T Deserialize(/*byte[]*/ string data)
        {
            //var js = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string ToStringFromBytes(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static byte[] ToBytesFromString(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
    }
}
