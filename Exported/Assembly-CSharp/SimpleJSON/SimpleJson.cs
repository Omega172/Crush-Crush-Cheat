using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200008A RID: 138
	[GeneratedCode("simple-json", "1.0.0")]
	public static class SimpleJson
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x0000E034 File Offset: 0x0000C234
		static SimpleJson()
		{
			SimpleJson.EscapeTable = new char[93];
			SimpleJson.EscapeTable[34] = '"';
			SimpleJson.EscapeTable[92] = '\\';
			SimpleJson.EscapeTable[8] = 'b';
			SimpleJson.EscapeTable[12] = 'f';
			SimpleJson.EscapeTable[10] = 'n';
			SimpleJson.EscapeTable[13] = 'r';
			SimpleJson.EscapeTable[9] = 't';
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000E174 File Offset: 0x0000C374
		public static object DeserializeObject(string json)
		{
			object result;
			if (SimpleJson.TryDeserializeObject(json, out result))
			{
				return result;
			}
			throw new SerializationException("Invalid JSON string");
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000E19C File Offset: 0x0000C39C
		[SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
		public static bool TryDeserializeObject(string json, out object obj)
		{
			bool result = true;
			if (json != null)
			{
				int num = 0;
				obj = SimpleJson.ParseValue(json, ref num, ref result);
			}
			else
			{
				obj = null;
			}
			return result;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000E1C8 File Offset: 0x0000C3C8
		public static object DeserializeObject(string json, Type type, IJsonSerializerStrategy jsonSerializerStrategy = null)
		{
			object obj = SimpleJson.DeserializeObject(json);
			if (type == null || (obj != null && ReflectionUtils.IsAssignableFrom(obj.GetType(), type)))
			{
				return obj;
			}
			return (jsonSerializerStrategy ?? SimpleJson.CurrentJsonSerializerStrategy).DeserializeObject(obj, type);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000E210 File Offset: 0x0000C410
		public static T DeserializeObject<T>(string json, IJsonSerializerStrategy jsonSerializerStrategy = null)
		{
			return (T)((object)SimpleJson.DeserializeObject(json, typeof(T), jsonSerializerStrategy));
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000E228 File Offset: 0x0000C428
		public static string SerializeObject(object json, IJsonSerializerStrategy jsonSerializerStrategy = null)
		{
			if (SimpleJson._serializeObjectBuilder == null)
			{
				SimpleJson._serializeObjectBuilder = new StringBuilder(2000);
			}
			SimpleJson._serializeObjectBuilder.Length = 0;
			if (jsonSerializerStrategy == null)
			{
				jsonSerializerStrategy = SimpleJson.CurrentJsonSerializerStrategy;
			}
			bool flag = SimpleJson.SerializeValue(jsonSerializerStrategy, json, SimpleJson._serializeObjectBuilder);
			return (!flag) ? null : SimpleJson._serializeObjectBuilder.ToString();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000E28C File Offset: 0x0000C48C
		public static string EscapeToJavascriptString(string jsonString)
		{
			if (string.IsNullOrEmpty(jsonString))
			{
				return jsonString;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			while (i < jsonString.Length)
			{
				char c = jsonString[i++];
				if (c == '\\')
				{
					int num = jsonString.Length - i;
					if (num >= 2)
					{
						char c2 = jsonString[i];
						if (c2 == '\\')
						{
							stringBuilder.Append('\\');
							i++;
						}
						else if (c2 == '"')
						{
							stringBuilder.Append("\"");
							i++;
						}
						else if (c2 == 't')
						{
							stringBuilder.Append('\t');
							i++;
						}
						else if (c2 == 'b')
						{
							stringBuilder.Append('\b');
							i++;
						}
						else if (c2 == 'n')
						{
							stringBuilder.Append('\n');
							i++;
						}
						else if (c2 == 'r')
						{
							stringBuilder.Append('\r');
							i++;
						}
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000E3A0 File Offset: 0x0000C5A0
		private static IDictionary<string, object> ParseObject(string json, ref int index, ref bool success)
		{
			IDictionary<string, object> dictionary = new JsonObject();
			SimpleJson.NextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				SimpleJson.TokenType tokenType = SimpleJson.LookAhead(json, index);
				if (tokenType == SimpleJson.TokenType.NONE)
				{
					success = false;
					return null;
				}
				if (tokenType == SimpleJson.TokenType.COMMA)
				{
					SimpleJson.NextToken(json, ref index);
				}
				else
				{
					if (tokenType == SimpleJson.TokenType.CURLY_CLOSE)
					{
						SimpleJson.NextToken(json, ref index);
						return dictionary;
					}
					string key = SimpleJson.ParseString(json, ref index, ref success);
					if (!success)
					{
						success = false;
						return null;
					}
					tokenType = SimpleJson.NextToken(json, ref index);
					if (tokenType != SimpleJson.TokenType.COLON)
					{
						success = false;
						return null;
					}
					object value = SimpleJson.ParseValue(json, ref index, ref success);
					if (!success)
					{
						success = false;
						return null;
					}
					dictionary[key] = value;
				}
			}
			return dictionary;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000E44C File Offset: 0x0000C64C
		private static JsonArray ParseArray(string json, ref int index, ref bool success)
		{
			JsonArray jsonArray = new JsonArray();
			SimpleJson.NextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				SimpleJson.TokenType tokenType = SimpleJson.LookAhead(json, index);
				if (tokenType == SimpleJson.TokenType.NONE)
				{
					success = false;
					return null;
				}
				if (tokenType == SimpleJson.TokenType.COMMA)
				{
					SimpleJson.NextToken(json, ref index);
				}
				else
				{
					if (tokenType == SimpleJson.TokenType.SQUARED_CLOSE)
					{
						SimpleJson.NextToken(json, ref index);
						break;
					}
					object item = SimpleJson.ParseValue(json, ref index, ref success);
					if (!success)
					{
						return null;
					}
					jsonArray.Add(item);
				}
			}
			return jsonArray;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000E4CC File Offset: 0x0000C6CC
		private static object ParseValue(string json, ref int index, ref bool success)
		{
			switch (SimpleJson.LookAhead(json, index))
			{
			case SimpleJson.TokenType.CURLY_OPEN:
				return SimpleJson.ParseObject(json, ref index, ref success);
			case SimpleJson.TokenType.SQUARED_OPEN:
				return SimpleJson.ParseArray(json, ref index, ref success);
			case SimpleJson.TokenType.STRING:
				return SimpleJson.ParseString(json, ref index, ref success);
			case SimpleJson.TokenType.NUMBER:
				return SimpleJson.ParseNumber(json, ref index, ref success);
			case SimpleJson.TokenType.TRUE:
				SimpleJson.NextToken(json, ref index);
				return true;
			case SimpleJson.TokenType.FALSE:
				SimpleJson.NextToken(json, ref index);
				return false;
			case SimpleJson.TokenType.NULL:
				SimpleJson.NextToken(json, ref index);
				return null;
			}
			success = false;
			return null;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000E574 File Offset: 0x0000C774
		private static string ParseString(string json, ref int index, ref bool success)
		{
			if (SimpleJson._parseStringBuilder == null)
			{
				SimpleJson._parseStringBuilder = new StringBuilder(2000);
			}
			SimpleJson._parseStringBuilder.Length = 0;
			SimpleJson.EatWhitespace(json, ref index);
			char c = json[index++];
			bool flag = false;
			while (!flag)
			{
				if (index == json.Length)
				{
					break;
				}
				c = json[index++];
				if (c == '"')
				{
					flag = true;
					break;
				}
				if (c == '\\')
				{
					if (index == json.Length)
					{
						break;
					}
					c = json[index++];
					if (c == '"')
					{
						SimpleJson._parseStringBuilder.Append('"');
					}
					else if (c == '\\')
					{
						SimpleJson._parseStringBuilder.Append('\\');
					}
					else if (c == '/')
					{
						SimpleJson._parseStringBuilder.Append('/');
					}
					else if (c == 'b')
					{
						SimpleJson._parseStringBuilder.Append('\b');
					}
					else if (c == 'f')
					{
						SimpleJson._parseStringBuilder.Append('\f');
					}
					else if (c == 'n')
					{
						SimpleJson._parseStringBuilder.Append('\n');
					}
					else if (c == 'r')
					{
						SimpleJson._parseStringBuilder.Append('\r');
					}
					else if (c == 't')
					{
						SimpleJson._parseStringBuilder.Append('\t');
					}
					else if (c == 'u')
					{
						int num = json.Length - index;
						if (num < 4)
						{
							break;
						}
						uint num2;
						if (!(success = uint.TryParse(json.Substring(index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2)))
						{
							return string.Empty;
						}
						if (55296U <= num2 && num2 <= 56319U)
						{
							index += 4;
							num = json.Length - index;
							uint num3;
							if (num < 6 || !(json.Substring(index, 2) == "\\u") || !uint.TryParse(json.Substring(index + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num3) || 56320U > num3 || num3 > 57343U)
							{
								success = false;
								return string.Empty;
							}
							SimpleJson._parseStringBuilder.Append((char)num2);
							SimpleJson._parseStringBuilder.Append((char)num3);
							index += 6;
						}
						else
						{
							SimpleJson._parseStringBuilder.Append(SimpleJson.ConvertFromUtf32((int)num2));
							index += 4;
						}
					}
				}
				else
				{
					SimpleJson._parseStringBuilder.Append(c);
				}
			}
			if (!flag)
			{
				success = false;
				return null;
			}
			return SimpleJson._parseStringBuilder.ToString();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000E834 File Offset: 0x0000CA34
		private static string ConvertFromUtf32(int utf32)
		{
			if (utf32 < 0 || utf32 > 1114111)
			{
				throw new ArgumentOutOfRangeException("utf32", "The argument must be from 0 to 0x10FFFF.");
			}
			if (55296 <= utf32 && utf32 <= 57343)
			{
				throw new ArgumentOutOfRangeException("utf32", "The argument must not be in surrogate pair range.");
			}
			if (utf32 < 65536)
			{
				return new string((char)utf32, 1);
			}
			utf32 -= 65536;
			return new string(new char[]
			{
				(char)((utf32 >> 10) + 55296),
				(char)(utf32 % 1024 + 56320)
			});
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000E8D0 File Offset: 0x0000CAD0
		private static object ParseNumber(string json, ref int index, ref bool success)
		{
			SimpleJson.EatWhitespace(json, ref index);
			int lastIndexOfNumber = SimpleJson.GetLastIndexOfNumber(json, index);
			int length = lastIndexOfNumber - index + 1;
			string text = json.Substring(index, length);
			object result;
			if (text.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || text.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
			{
				double num;
				success = double.TryParse(json.Substring(index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out num);
				result = num;
			}
			else if (text.IndexOf("-", StringComparison.OrdinalIgnoreCase) == -1)
			{
				ulong num2;
				success = ulong.TryParse(json.Substring(index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out num2);
				result = num2;
			}
			else
			{
				long num3;
				success = long.TryParse(json.Substring(index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out num3);
				result = num3;
			}
			index = lastIndexOfNumber + 1;
			return result;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000E9B0 File Offset: 0x0000CBB0
		private static int GetLastIndexOfNumber(string json, int index)
		{
			int i;
			for (i = index; i < json.Length; i++)
			{
				if ("0123456789+-.eE".IndexOf(json[i]) == -1)
				{
					break;
				}
			}
			return i - 1;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
		private static void EatWhitespace(string json, ref int index)
		{
			while (index < json.Length)
			{
				if (" \t\n\r\b\f".IndexOf(json[index]) == -1)
				{
					break;
				}
				index++;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000EA2C File Offset: 0x0000CC2C
		private static SimpleJson.TokenType LookAhead(string json, int index)
		{
			int num = index;
			return SimpleJson.NextToken(json, ref num);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000EA44 File Offset: 0x0000CC44
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		private static SimpleJson.TokenType NextToken(string json, ref int index)
		{
			SimpleJson.EatWhitespace(json, ref index);
			if (index == json.Length)
			{
				return SimpleJson.TokenType.NONE;
			}
			char c = json[index];
			index++;
			char c2 = c;
			switch (c2)
			{
			case '"':
				return SimpleJson.TokenType.STRING;
			default:
				switch (c2)
				{
				case '[':
					return SimpleJson.TokenType.SQUARED_OPEN;
				default:
				{
					switch (c2)
					{
					case '{':
						return SimpleJson.TokenType.CURLY_OPEN;
					case '}':
						return SimpleJson.TokenType.CURLY_CLOSE;
					}
					index--;
					int num = json.Length - index;
					if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
					{
						index += 5;
						return SimpleJson.TokenType.FALSE;
					}
					if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
					{
						index += 4;
						return SimpleJson.TokenType.TRUE;
					}
					if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
					{
						index += 4;
						return SimpleJson.TokenType.NULL;
					}
					return SimpleJson.TokenType.NONE;
				}
				case ']':
					return SimpleJson.TokenType.SQUARED_CLOSE;
				}
				break;
			case ',':
				return SimpleJson.TokenType.COMMA;
			case '-':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return SimpleJson.TokenType.NUMBER;
			case ':':
				return SimpleJson.TokenType.COLON;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000EC3C File Offset: 0x0000CE3C
		private static bool SerializeValue(IJsonSerializerStrategy jsonSerializerStrategy, object value, StringBuilder builder)
		{
			bool flag = true;
			string text = value as string;
			if (value == null)
			{
				builder.Append("null");
			}
			else if (text != null)
			{
				flag = SimpleJson.SerializeString(text, builder);
			}
			else
			{
				IDictionary<string, object> dictionary = value as IDictionary<string, object>;
				Type type = value.GetType();
				Type[] genericTypeArguments = ReflectionUtils.GetGenericTypeArguments(type);
				bool flag2 = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<, >) && genericTypeArguments[0] == typeof(string);
				if (flag2)
				{
					IDictionary dictionary2 = value as IDictionary;
					flag = SimpleJson.SerializeObject(jsonSerializerStrategy, dictionary2.Keys, dictionary2.Values, builder);
				}
				else if (dictionary != null)
				{
					flag = SimpleJson.SerializeObject(jsonSerializerStrategy, dictionary.Keys, dictionary.Values, builder);
				}
				else
				{
					IDictionary<string, string> dictionary3 = value as IDictionary<string, string>;
					if (dictionary3 != null)
					{
						flag = SimpleJson.SerializeObject(jsonSerializerStrategy, dictionary3.Keys, dictionary3.Values, builder);
					}
					else
					{
						IEnumerable enumerable = value as IEnumerable;
						if (enumerable != null)
						{
							flag = SimpleJson.SerializeArray(jsonSerializerStrategy, enumerable, builder);
						}
						else if (SimpleJson.IsNumeric(value))
						{
							flag = SimpleJson.SerializeNumber(value, builder);
						}
						else if (value is bool)
						{
							builder.Append((!(bool)value) ? "false" : "true");
						}
						else
						{
							object value2;
							flag = jsonSerializerStrategy.TrySerializeNonPrimitiveObject(value, out value2);
							if (flag)
							{
								SimpleJson.SerializeValue(jsonSerializerStrategy, value2, builder);
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000EDBC File Offset: 0x0000CFBC
		private static bool SerializeObject(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable keys, IEnumerable values, StringBuilder builder)
		{
			builder.Append("{");
			IEnumerator enumerator = keys.GetEnumerator();
			IEnumerator enumerator2 = values.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				object obj = enumerator.Current;
				object value = enumerator2.Current;
				if (!flag)
				{
					builder.Append(",");
				}
				string text = obj as string;
				if (text != null)
				{
					SimpleJson.SerializeString(text, builder);
				}
				else if (!SimpleJson.SerializeValue(jsonSerializerStrategy, value, builder))
				{
					return false;
				}
				builder.Append(":");
				if (!SimpleJson.SerializeValue(jsonSerializerStrategy, value, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000EE7C File Offset: 0x0000D07C
		private static bool SerializeArray(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable anArray, StringBuilder builder)
		{
			builder.Append("[");
			bool flag = true;
			foreach (object value in anArray)
			{
				if (!flag)
				{
					builder.Append(",");
				}
				if (!SimpleJson.SerializeValue(jsonSerializerStrategy, value, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("]");
			return true;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000EF24 File Offset: 0x0000D124
		private static bool SerializeString(string aString, StringBuilder builder)
		{
			if (aString.IndexOfAny(SimpleJson.EscapeCharacters) == -1)
			{
				builder.Append('"');
				builder.Append(aString);
				builder.Append('"');
				return true;
			}
			builder.Append('"');
			int num = 0;
			char[] array = aString.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				if ((int)c >= SimpleJson.EscapeTable.Length || SimpleJson.EscapeTable[(int)c] == '\0')
				{
					num++;
				}
				else
				{
					if (num > 0)
					{
						builder.Append(array, i - num, num);
						num = 0;
					}
					builder.Append('\\');
					builder.Append(SimpleJson.EscapeTable[(int)c]);
				}
			}
			if (num > 0)
			{
				builder.Append(array, array.Length - num, num);
			}
			builder.Append('"');
			return true;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
		private static bool SerializeNumber(object number, StringBuilder builder)
		{
			if (number is decimal)
			{
				builder.Append(((decimal)number).ToString("R", CultureInfo.InvariantCulture));
			}
			else if (number is double)
			{
				builder.Append(((double)number).ToString("R", CultureInfo.InvariantCulture));
			}
			else if (number is float)
			{
				builder.Append(((float)number).ToString("R", CultureInfo.InvariantCulture));
			}
			else if (SimpleJson.NumberTypes.IndexOf(number.GetType()) != -1)
			{
				builder.Append(number);
			}
			return true;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000F0B4 File Offset: 0x0000D2B4
		private static bool IsNumeric(object value)
		{
			return value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000F154 File Offset: 0x0000D354
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000F170 File Offset: 0x0000D370
		public static IJsonSerializerStrategy CurrentJsonSerializerStrategy
		{
			get
			{
				IJsonSerializerStrategy result;
				if ((result = SimpleJson._currentJsonSerializerStrategy) == null)
				{
					result = (SimpleJson._currentJsonSerializerStrategy = SimpleJson.PocoJsonSerializerStrategy);
				}
				return result;
			}
			set
			{
				SimpleJson._currentJsonSerializerStrategy = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000F178 File Offset: 0x0000D378
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static PocoJsonSerializerStrategy PocoJsonSerializerStrategy
		{
			get
			{
				PocoJsonSerializerStrategy result;
				if ((result = SimpleJson._pocoJsonSerializerStrategy) == null)
				{
					result = (SimpleJson._pocoJsonSerializerStrategy = new PocoJsonSerializerStrategy());
				}
				return result;
			}
		}

		// Token: 0x0400029E RID: 670
		private const int BUILDER_INIT = 2000;

		// Token: 0x0400029F RID: 671
		private static readonly char[] EscapeTable;

		// Token: 0x040002A0 RID: 672
		private static readonly char[] EscapeCharacters = new char[]
		{
			'"',
			'\\',
			'\b',
			'\f',
			'\n',
			'\r',
			'\t'
		};

		// Token: 0x040002A1 RID: 673
		internal static readonly List<Type> NumberTypes = new List<Type>
		{
			typeof(bool),
			typeof(byte),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(sbyte),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(double),
			typeof(float),
			typeof(decimal)
		};

		// Token: 0x040002A2 RID: 674
		[ThreadStatic]
		private static StringBuilder _serializeObjectBuilder;

		// Token: 0x040002A3 RID: 675
		[ThreadStatic]
		private static StringBuilder _parseStringBuilder;

		// Token: 0x040002A4 RID: 676
		private static IJsonSerializerStrategy _currentJsonSerializerStrategy;

		// Token: 0x040002A5 RID: 677
		private static PocoJsonSerializerStrategy _pocoJsonSerializerStrategy;

		// Token: 0x0200008B RID: 139
		private enum TokenType : byte
		{
			// Token: 0x040002A7 RID: 679
			NONE,
			// Token: 0x040002A8 RID: 680
			CURLY_OPEN,
			// Token: 0x040002A9 RID: 681
			CURLY_CLOSE,
			// Token: 0x040002AA RID: 682
			SQUARED_OPEN,
			// Token: 0x040002AB RID: 683
			SQUARED_CLOSE,
			// Token: 0x040002AC RID: 684
			COLON,
			// Token: 0x040002AD RID: 685
			COMMA,
			// Token: 0x040002AE RID: 686
			STRING,
			// Token: 0x040002AF RID: 687
			NUMBER,
			// Token: 0x040002B0 RID: 688
			TRUE,
			// Token: 0x040002B1 RID: 689
			FALSE,
			// Token: 0x040002B2 RID: 690
			NULL
		}
	}
}
