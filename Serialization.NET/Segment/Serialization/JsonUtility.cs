using System.Text;
using System.Text.Json;

namespace Segment.Serialization
{
    public static class JsonUtility
    {
        private static readonly string[] EscapeChars = GetEscapeChars();

        public static string PrintQuoted(string content)
        {
            var builder = new StringBuilder();
            builder.Append('"');

            var lastPos = 0;
            var length = content.Length;

            for (var index = 0; index < length; index++)
            {
                int character = content[index];

                if (character >= EscapeChars.Length)
                {
                    continue;
                }

                var esc = EscapeChars[character];
                if (esc == null)
                {
                    continue;
                }

                builder.Append(content, lastPos, index - lastPos);
                builder.Append(esc);
                lastPos = index + 1;
            }

            builder.Append(content, lastPos, content.Length - lastPos);

            builder.Append('"');
            return builder.ToString();
        }

        private static string[] GetEscapeChars()
        {
            var ascii = new string[128];

            // the first 32 chars (0 - 1f) in ascii table should escape in hex form \uXXXX
            for (var character = 0; character <= 0x1f; character++)
            {
                var c1 = ToHexChar(character >> 12);
                var c2 = ToHexChar(character >> 8);
                var c3 = ToHexChar(character >> 4);
                var c4 = ToHexChar(character);

                ascii[character] = "\\u" + c1 + c2 + c3 + c4;
            }

            ascii['"'] = "\\\"";
            ascii['\\'] = "\\\\";
            ascii['\t'] = "\\t";
            ascii['\b'] = "\\b";
            ascii['\n'] = "\\n";
            ascii['\r'] = "\\r";
            ascii[0x0c] = "\\f";

            return ascii;
        }

        private static char ToHexChar(int i)
        {
            // use 1111 in binary to keep the last for digits
            var d = i & 0xf;

            // convert d to hex char (0-9, a-f)
            if (d < 10)
            {
                return (char)(d + '0');
            }
            else
            {
                return (char)(d - 10 + 'a');
            }
        }

        public static string ToJson(object value, bool pretty = false) => JsonSerializer.Serialize(value, pretty ? new JsonSerializerOptions {  WriteIndented = true } : null);

        public static T FromJson<T>(string json) => JsonSerializer.Deserialize<T>(json);
    }
}