using System;
using System.IO;
using System.Linq;

namespace BlobMetadata.Extensions
{
    public enum ContentType
    {
        Other,
        Image,
        Video
    }

    public static class StringExtensions
    {
        public static ContentType ResolveType(this string contentType)
        {
            // ref https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
            if (string.IsNullOrWhiteSpace(contentType))
                return ContentType.Other;
            if (contentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                return ContentType.Image;
            if (contentType.StartsWith("video", StringComparison.OrdinalIgnoreCase))
                return ContentType.Video;
            return ContentType.Other;
        }

        public static string ExtractCId(this string name)
        {
            return string.Join('/', name.Split('/').Take(2).ToArray());
        }

        public static string Sanitize(this string name)
        {
            //string illegal = "\"M\"\\a/ry/ h**ad:>> a\\/:*?\"| li*tt|le|| la\"mb.?";
            //string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            //Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            //illegal = r.Replace(illegal, "");

            return string.Concat(name.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
