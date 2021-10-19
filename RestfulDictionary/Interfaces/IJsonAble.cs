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
    }
}
