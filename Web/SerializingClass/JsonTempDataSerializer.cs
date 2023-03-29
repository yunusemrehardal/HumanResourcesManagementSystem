using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Text.Json;
using System.Text;
using Web.Interfaces;

namespace Web.SerializingClass
{
    public class JsonTempDataSerializer : ITempDataSerializer
    {
        private readonly JsonSerializerOptions _options;

        public JsonTempDataSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        public byte[] Serialize(object value)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _options));
        }

        public object? Deserialize(byte[] value)
        {
            if (value == null)
            {
                return null;
            }

            return JsonSerializer.Deserialize(Encoding.UTF8.GetString(value), typeof(object), _options);
        }
    }

}
