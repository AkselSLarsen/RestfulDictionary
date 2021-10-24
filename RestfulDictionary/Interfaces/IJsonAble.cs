using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestfulDictionary.Interfaces {
    public interface IJsonAble<T> {
        public string ToJson();

        public static T FromJson(string json) {
            Object o = JsonSerializer.Deserialize(json, typeof(T));
            if (o is T) {
                T type = (T)o;
                return type;
            }
            throw new Exception("Could not create T from JSON");
        }

        public static T[] FromJsonArray(string json) {
            List<T> tList = new List<T>();

            while(json.Contains("{")) {
                int start = json.LastIndexOf("{");
                int end = json.LastIndexOf("}");

                tList.Add(FromJson(json.Substring(start, (end-start)+1)));

                json = json.Substring(0, start);
            }

            return tList.ToArray();
        }
    }
}
