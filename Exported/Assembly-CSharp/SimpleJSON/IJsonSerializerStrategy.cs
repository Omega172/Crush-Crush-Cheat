using System;
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;

namespace SimpleJSON
{
	// Token: 0x0200008C RID: 140
	[GeneratedCode("simple-json", "1.0.0")]
	public interface IJsonSerializerStrategy
	{
		// Token: 0x06000203 RID: 515
		[SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
		bool TrySerializeNonPrimitiveObject(object input, out object output);

		// Token: 0x06000204 RID: 516
		object DeserializeObject(object value, Type type);
	}
}
