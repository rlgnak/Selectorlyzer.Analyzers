﻿using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace Selectorlyzer.LightJson.Serialization
{
    using ErrorType = JsonParseException.ErrorType;

    /// <summary>
    /// Represents a reader that can read JsonValues.
    /// </summary>
    public sealed class JsonReader
    {
        private TextScanner scanner;

        private JsonReader(TextReader reader)
        {
            scanner = new TextScanner(reader);
        }

        private string ReadJsonKey()
        {
            return ReadString();
        }

        private JsonValue ReadJsonValue()
        {
            scanner.SkipWhitespace();

            var next = scanner.Peek();

            if (char.IsNumber(next))
            {
                return ReadNumber();
            }

            switch (next)
            {
                case '{':
                    return ReadObject();

                case '[':
                    return ReadArray();

                case '"':
                    return ReadString();

                case '-':
                    return ReadNumber();

                case 't':
                case 'f':
                    return ReadBoolean();

                case 'n':
                    return ReadNull();

                default:
                    throw new JsonParseException(
                        ErrorType.InvalidOrUnexpectedCharacter,
                        scanner.Position
                    );
            }
        }

        private JsonValue ReadNull()
        {
            scanner.Assert("null");
            return JsonValue.Null;
        }

        private JsonValue ReadBoolean()
        {
            switch (scanner.Peek())
            {
                case 't':
                    scanner.Assert("true");
                    return true;

                case 'f':
                    scanner.Assert("false");
                    return false;

                default:
                    throw new JsonParseException(
                        ErrorType.InvalidOrUnexpectedCharacter,
                        scanner.Position
                    );
            }
        }

        private void ReadDigits(StringBuilder builder)
        {
            while (scanner.CanRead && char.IsDigit(scanner.Peek()))
            {
                builder.Append(scanner.Read());
            }
        }

        private JsonValue ReadNumber()
        {
            var builder = new StringBuilder();

            if (scanner.Peek() == '-')
            {
                builder.Append(scanner.Read());
            }

            if (scanner.Peek() == '0')
            {
                builder.Append(scanner.Read());
            }
            else
            {
                ReadDigits(builder);
            }

            if (scanner.CanRead && scanner.Peek() == '.')
            {
                builder.Append(scanner.Read());
                ReadDigits(builder);
            }

            if (scanner.CanRead && char.ToLowerInvariant(scanner.Peek()) == 'e')
            {
                builder.Append(scanner.Read());

                var next = scanner.Peek();

                switch (next)
                {
                    case '+':
                    case '-':
                        builder.Append(scanner.Read());
                        break;
                }

                ReadDigits(builder);
            }

            return double.Parse(
                builder.ToString(),
                CultureInfo.InvariantCulture
            );
        }

        private string ReadString()
        {
            var builder = new StringBuilder();

            scanner.Assert('"');

            while (true)
            {
                var c = scanner.Read();

                if (c == '\\')
                {
                    c = scanner.Read();

                    switch (char.ToLower(c))
                    {
                        case '"':  // "
                        case '\\': // \
                        case '/':  // /
                            builder.Append(c);
                            break;
                        case 'b':
                            builder.Append('\b');
                            break;
                        case 'f':
                            builder.Append('\f');
                            break;
                        case 'n':
                            builder.Append('\n');
                            break;
                        case 'r':
                            builder.Append('\r');
                            break;
                        case 't':
                            builder.Append('\t');
                            break;
                        case 'u':
                            builder.Append(ReadUnicodeLiteral());
                            break;
                        default:
                            throw new JsonParseException(
                                ErrorType.InvalidOrUnexpectedCharacter,
                                scanner.Position
                            );
                    }
                }
                else if (c == '"')
                {
                    break;
                }
                else
                {
                    /*
                     * According to the spec:
                     * 
                     * unescaped = %x20-21 / %x23-5B / %x5D-10FFFF
                     * 
                     * i.e. c cannot be < 0x20, be 0x22 (a double quote) or a 
                     * backslash (0x5C).
                     * 
                     * c cannot be a back slash or double quote as the above 
                     * would have hit. So just check for < 0x20.
                     * 
                     * > 0x10FFFF is unnecessary *I think* because it's obviously
                     * out of the range of a character but we might need to look ahead
                     * to get the whole utf-16 codepoint
                     */
                    if (c < '\u0020')
                    {
                        throw new JsonParseException(
                            ErrorType.InvalidOrUnexpectedCharacter,
                            scanner.Position
                        );
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
            }

            return builder.ToString();
        }

        private int ReadHexDigit()
        {
            switch (char.ToUpper(scanner.Read()))
            {
                case '0':
                    return 0;

                case '1':
                    return 1;

                case '2':
                    return 2;

                case '3':
                    return 3;

                case '4':
                    return 4;

                case '5':
                    return 5;

                case '6':
                    return 6;

                case '7':
                    return 7;

                case '8':
                    return 8;

                case '9':
                    return 9;

                case 'A':
                    return 10;

                case 'B':
                    return 11;

                case 'C':
                    return 12;

                case 'D':
                    return 13;

                case 'E':
                    return 14;

                case 'F':
                    return 15;

                default:
                    throw new JsonParseException(
                        ErrorType.InvalidOrUnexpectedCharacter,
                        scanner.Position
                    );
            }
        }

        private char ReadUnicodeLiteral()
        {
            int value = 0;

            value += ReadHexDigit() * 4096; // 16^3
            value += ReadHexDigit() * 256;  // 16^2
            value += ReadHexDigit() * 16;   // 16^1
            value += ReadHexDigit();        // 16^0

            return (char)value;
        }

        private JsonObject ReadObject()
        {
            return ReadObject(new JsonObject());
        }

        private JsonObject ReadObject(JsonObject jsonObject)
        {
            scanner.Assert('{');

            scanner.SkipWhitespace();

            if (scanner.Peek() == '}')
            {
                scanner.Read();
            }
            else
            {
                while (true)
                {
                    scanner.SkipWhitespace();

                    var key = ReadJsonKey();

                    if (jsonObject.ContainsKey(key))
                    {
                        throw new JsonParseException(
                            ErrorType.DuplicateObjectKeys,
                            scanner.Position
                        );
                    }

                    scanner.SkipWhitespace();

                    scanner.Assert(':');

                    scanner.SkipWhitespace();

                    var value = ReadJsonValue();

                    jsonObject.Add(key, value);

                    scanner.SkipWhitespace();

                    var next = scanner.Read();

                    if (next == '}')
                    {
                        break;
                    }
                    else if (next == ',')
                    {
                        continue;
                    }
                    else
                    {
                        throw new JsonParseException(
                            ErrorType.InvalidOrUnexpectedCharacter,
                            scanner.Position
                        );
                    }
                }
            }

            return jsonObject;
        }

        private JsonArray ReadArray()
        {
            return ReadArray(new JsonArray());
        }

        private JsonArray ReadArray(JsonArray jsonArray)
        {
            scanner.Assert('[');

            scanner.SkipWhitespace();

            if (scanner.Peek() == ']')
            {
                scanner.Read();
            }
            else
            {
                while (true)
                {
                    scanner.SkipWhitespace();

                    var value = ReadJsonValue();

                    jsonArray.Add(value);

                    scanner.SkipWhitespace();

                    var next = scanner.Read();

                    if (next == ']')
                    {
                        break;
                    }
                    else if (next == ',')
                    {
                        continue;
                    }
                    else
                    {
                        throw new JsonParseException(
                            ErrorType.InvalidOrUnexpectedCharacter,
                            scanner.Position
                        );
                    }
                }
            }

            return jsonArray;
        }

        private JsonValue Parse()
        {
            scanner.SkipWhitespace();
            return ReadJsonValue();
        }

        /// <summary>
        /// Creates a JsonValue by using the given TextReader.
        /// </summary>
        /// <param name="reader">The TextReader used to read a JSON message.</param>
        public static JsonValue Parse(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return new JsonReader(reader).Parse();
        }

        /// <summary>
        /// Creates a JsonValue by reader the JSON message in the given string.
        /// </summary>
        /// <param name="source">The string containing the JSON message.</param>
        public static JsonValue Parse(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            using (var reader = new StringReader(source))
            {
                return new JsonReader(reader).Parse();
            }
        }

        /// <summary>
        /// Creates a JsonValue by reading the given file.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        public static JsonValue ParseFile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // NOTE: FileAccess.Read is needed to be able to open read-only files
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                return new JsonReader(reader).Parse();
            }
        }
    }
}
