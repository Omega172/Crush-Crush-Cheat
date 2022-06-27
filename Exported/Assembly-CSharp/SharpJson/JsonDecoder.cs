using System;
using System.Collections.Generic;

namespace SharpJson
{
	// Token: 0x020001B5 RID: 437
	public class JsonDecoder
	{
		// Token: 0x06000D9B RID: 3483 RVA: 0x000606D0 File Offset: 0x0005E8D0
		public JsonDecoder()
		{
			this.errorMessage = null;
			this.parseNumbersAsFloat = false;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x000606E8 File Offset: 0x0005E8E8
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x000606F0 File Offset: 0x0005E8F0
		public string errorMessage { get; private set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x000606FC File Offset: 0x0005E8FC
		// (set) Token: 0x06000D9F RID: 3487 RVA: 0x00060704 File Offset: 0x0005E904
		public bool parseNumbersAsFloat { get; set; }

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00060710 File Offset: 0x0005E910
		public object Decode(string text)
		{
			this.errorMessage = null;
			this.lexer = new Lexer(text);
			this.lexer.parseNumbersAsFloat = this.parseNumbersAsFloat;
			return this.ParseValue();
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00060748 File Offset: 0x0005E948
		public static object DecodeText(string text)
		{
			JsonDecoder jsonDecoder = new JsonDecoder();
			return jsonDecoder.Decode(text);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00060764 File Offset: 0x0005E964
		private IDictionary<string, object> ParseObject()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			this.lexer.NextToken();
			for (;;)
			{
				Lexer.Token token = this.lexer.LookAhead();
				Lexer.Token token2 = token;
				if (token2 == Lexer.Token.None)
				{
					break;
				}
				if (token2 != Lexer.Token.Comma)
				{
					if (token2 == Lexer.Token.CurlyClose)
					{
						goto IL_5C;
					}
					string key = this.EvalLexer<string>(this.lexer.ParseString());
					if (this.errorMessage != null)
					{
						goto Block_4;
					}
					token = this.lexer.NextToken();
					if (token != Lexer.Token.Colon)
					{
						goto Block_5;
					}
					object value = this.ParseValue();
					if (this.errorMessage != null)
					{
						goto Block_6;
					}
					dictionary[key] = value;
				}
				else
				{
					this.lexer.NextToken();
				}
			}
			this.TriggerError("Invalid token");
			return null;
			IL_5C:
			this.lexer.NextToken();
			return dictionary;
			Block_4:
			return null;
			Block_5:
			this.TriggerError("Invalid token; expected ':'");
			return null;
			Block_6:
			return null;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00060840 File Offset: 0x0005EA40
		private IList<object> ParseArray()
		{
			List<object> list = new List<object>();
			this.lexer.NextToken();
			for (;;)
			{
				Lexer.Token token = this.lexer.LookAhead();
				Lexer.Token token2 = token;
				if (token2 == Lexer.Token.None)
				{
					break;
				}
				if (token2 != Lexer.Token.Comma)
				{
					if (token2 == Lexer.Token.SquaredClose)
					{
						goto IL_58;
					}
					object item = this.ParseValue();
					if (this.errorMessage != null)
					{
						goto Block_4;
					}
					list.Add(item);
				}
				else
				{
					this.lexer.NextToken();
				}
			}
			this.TriggerError("Invalid token");
			return null;
			IL_58:
			this.lexer.NextToken();
			return list;
			Block_4:
			return null;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000608D8 File Offset: 0x0005EAD8
		private object ParseValue()
		{
			switch (this.lexer.LookAhead())
			{
			case Lexer.Token.Null:
				this.lexer.NextToken();
				return null;
			case Lexer.Token.True:
				this.lexer.NextToken();
				return true;
			case Lexer.Token.False:
				this.lexer.NextToken();
				return false;
			case Lexer.Token.String:
				return this.EvalLexer<string>(this.lexer.ParseString());
			case Lexer.Token.Number:
				if (this.parseNumbersAsFloat)
				{
					return this.EvalLexer<float>(this.lexer.ParseFloatNumber());
				}
				return this.EvalLexer<double>(this.lexer.ParseDoubleNumber());
			case Lexer.Token.CurlyOpen:
				return this.ParseObject();
			case Lexer.Token.SquaredOpen:
				return this.ParseArray();
			}
			this.TriggerError("Unable to parse value");
			return null;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000609C8 File Offset: 0x0005EBC8
		private void TriggerError(string message)
		{
			this.errorMessage = string.Format("Error: '{0}' at line {1}", message, this.lexer.lineNumber);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000609EC File Offset: 0x0005EBEC
		private T EvalLexer<T>(T value)
		{
			if (this.lexer.hasError)
			{
				this.TriggerError("Lexical error ocurred");
			}
			return value;
		}

		// Token: 0x04000BCA RID: 3018
		private Lexer lexer;
	}
}
