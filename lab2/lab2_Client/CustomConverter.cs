
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_Client
{
    public class CustomConverter <T>
    {
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data).ToString();

        }

        public static T Deserialize( string data)
        {
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
