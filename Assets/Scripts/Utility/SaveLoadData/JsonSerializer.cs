using Newtonsoft.Json;

namespace Utils.Persistence
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

