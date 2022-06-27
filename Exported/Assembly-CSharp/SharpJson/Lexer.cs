using System;
using System.Globalization;
using System.Text;

namespace SharpJson
{
	// Token: 0x020001B3 RID: 435
	internal class Lexer
	{
		// Token: 0x06000D8B RID: 3467 RVA: 0x0006002C File Offset: 0x0005E22C
		public Lexer(string text)
		{
			this.Reset();
			this.json = text.ToCharArray();
			this.parseNumbersAsFloat = false;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00060070 File Offset: 0x0005E270
		public bool hasError
		{
			get
			{
				return !this.success;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x0006007C File Offset: 0x0005E27C
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x00060084 File Offset: 0x0005E284
		public int lineNumber { get; private set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00060090 File Offset: 0x0005E290
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x00060098 File Offset: 0x0005E298
		public bool parseNumbersAsFloat { get; set; }

		// Token: 0x06000D91 RID: 3473 RVA: 0x000600A4 File Offset: 0x0005E2A4
		public void Reset()
		{
			this.index = 0;
			this.lineNumber = 1;
			this.success = true;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000600BC File Offset: 0x0005E2BC
		public string ParseString()
		{
			int num = 0;
			StringBuilder stringBuilder = null;
			this.SkipWhiteSpaces();
			char c = this.json[this.index++];
			bool flag = false;
			bool flag2 = false;
			while (!flag2 && !flag)
			{
				if (this.index == this.json.Length)
				{
					break;
				}
				c = this.json[this.index++];
				if (c == '"')
				{
					flag2 = true;
					break;
				}
				if (c == '\\')
				{
					if (this.index == this.json.Length)
					{
						break;
					}
					c = this.json[this.index++];
					char c2 = c;
					switch (c2)
					{
					case 'n':
						this.stringBuffer[num++] = '\n';
						break;
					default:
						if (c2 != '"')
						{
							if (c2 != '/')
							{
								if (c2 != '\\')
								{
									if (c2 != 'b')
									{
										if (c2 == 'f')
										{
											this.stringBuffer[num++] = '\f';
										}
									}
									else
									{
										this.stringBuffer[num++] = '\b';
									}
								}
								else
								{
									this.stringBuffer[num++] = '\\';
								}
							}
							else
							{
								this.stringBuffer[num++] = '/';
							}
						}
						else
						{
							this.stringBuffer[num++] = '"';
						}
						break;
					case 'r':
						this.stringBuffer[num++] = '\r';
						break;
					case 't':
						this.stringBuffer[num++] = '\t';
						break;
					case 'u':
					{
						int num2 = this.json.Length - this.index;
						if (num2 >= 4)
						{
							string value = new string(this.json, this.index, 4);
							this.stringBuffer[num++] = (char)Convert.ToInt32(value, 16);
							this.index += 4;
						}
						else
						{
							flag = true;
						}
						break;
					}
					}
				}
				else
				{
					this.stringBuffer[num++] = c;
				}
				if (num >= this.stringBuffer.Length)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder();
					}
					stringBuilder.Append(this.stringBuffer, 0, num);
					num = 0;
				}
			}
			if (!flag2)
			{
				this.success = false;
				return null;
			}
			if (stringBuilder != null)
			{
				return stringBuilder.ToString();
			}
			return new string(this.stringBuffer, 0, num);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00060340 File Offset: 0x0005E540
		private string GetNumberString()
		{
			this.SkipWhiteSpaces();
			int lastIndexOfNumber = this.GetLastIndexOfNumber(this.index);
			int length = lastIndexOfNumber - this.index + 1;
			string result = new string(this.json, this.index, length);
			this.index = lastIndexOfNumber + 1;
			return result;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00060388 File Offset: 0x0005E588
		public float ParseFloatNumber()
		{
			string numberString = this.GetNumberString();
			float result;
			if (!float.TryParse(numberString, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
			{
				return 0f;
			}
			return result;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000603BC File Offset: 0x0005E5BC
		public double ParseDoubleNumber()
		{
			string numberString = this.GetNumberString();
			double result;
			if (!double.TryParse(numberString, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return 0.0;
			}
			return result;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x000603F4 File Offset: 0x0005E5F4
		private int GetLastIndexOfNumber(int index)
		{
			int i;
			for (i = index; i < this.json.Length; i++)
			{
				char c = this.json[i];
				if ((c < '0' || c > '9') && c != '+' && c != '-' && c != '.' && c != 'e' && c != 'E')
				{
					break;
				}
			}
			return i - 1;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00060464 File Offset: 0x0005E664
		private void SkipWhiteSpaces()
		{
			while (this.index < this.json.Length)
			{
				char c = this.json[this.index];
				if (c == '\n')
				{
					this.lineNumber++;
				}
				if (!char.IsWhiteSpace(this.json[this.index]))
				{
					break;
				}
				this.index++;
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x000604D8 File Offset: 0x0005E6D8
		public Lexer.Token LookAhead()
		{
			this.SkipWhiteSpaces();
			int num = this.index;
			return Lexer.NextToken(this.json, ref num);
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00060500 File Offset: 0x0005E700
		public Lexer.Token NextToken()
		{
			this.SkipWhiteSpaces();
			return Lexer.NextToken(this.json, ref this.index);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0006051C File Offset: 0x0005E71C
		private static Lexer.Token NextToken(char[] json, ref int index)
		{
			if (index == json.Length)
			{
				return Lexer.Token.None;
			}
			char c = json[index++];
			char c2 = c;
			switch (c2)
			{
			case '"':
				return Lexer.Token.String;
			default:
				switch (c2)
				{
				case '[':
					return Lexer.Token.SquaredOpen;
				default:
				{
					switch (c2)
					{
					case '{':
						return Lexer.Token.CurlyOpen;
					case '}':
						return Lexer.Token.CurlyClose;
					}
					index--;
					int num = json.Length - index;
					if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
					{
						index += 5;
						return Lexer.Token.False;
					}
					if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
					{
						index += 4;
						return Lexer.Token.True;
					}
					if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
					{
						index += 4;
						return Lexer.Token.Null;
					}
					return Lexer.Token.None;
				}
				case ']':
					return Lexer.Token.SquaredClose;
				}
				break;
			case ',':
				return Lexer.Token.Comma;
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
				return Lexer.Token.Number;
			case ':':
				return Lexer.Token.Colon;
			}
		}

		// Token: 0x04000BB7 RID: 2999
		private char[] json;

		// Token: 0x04000BB8 RID: 3000
		private int index;

		// Token: 0x04000BB9 RID: 3001
		private bool success = true;

		// Token: 0x04000BBA RID: 3002
		private char[] stringBuffer = new char[4096];

		// Token: 0x020001B4 RID: 436
		public enum Token
		{
			// Token: 0x04000BBE RID: 3006
			None,
			// Token: 0x04000BBF RID: 3007
			Null,
			// Token: 0x04000BC0 RID: 3008
			True,
			// Token: 0x04000BC1 RID: 3009
			False,
			// Token: 0x04000BC2 RID: 3010
			Colon,
			// Token: 0x04000BC3 RID: 3011
			Comma,
			// Token: 0x04000BC4 RID: 3012
			String,
			// Token: 0x04000BC5 RID: 3013
			Number,
			// Token: 0x04000BC6 RID: 3014
			CurlyOpen,
			// Token: 0x04000BC7 RID: 3015
			CurlyClose,
			// Token: 0x04000BC8 RID: 3016
			SquaredOpen,
			// Token: 0x04000BC9 RID: 3017
			SquaredClose
		}
	}
}
