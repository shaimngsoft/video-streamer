using Newtonsoft.Json.Linq;
using System;

namespace BlobMetadata.Extensions
{
    public static class ExceptionExtensions
    {
        public static JObject ToJson(this Exception e)
        {
            return new JObject(
                new JProperty("type", e.GetType().ToString()),
                new JProperty("message", e.Message),
                new JProperty("stacktrace", e.StackTrace));
        }
    }
}
