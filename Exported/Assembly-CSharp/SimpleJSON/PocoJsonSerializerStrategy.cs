using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace SimpleJSON
{
	// Token: 0x0200008D RID: 141
	[GeneratedCode("simple-json", "1.0.0")]
	public class PocoJsonSerializerStrategy : IJsonSerializerStrategy
	{
		// Token: 0x06000205 RID: 517 RVA: 0x0000F194 File Offset: 0x0000D394
		public PocoJsonSerializerStrategy()
		{
			this.ConstructorCache = new ReflectionUtils.ThreadSafeDictionary<Type, ReflectionUtils.ConstructorDelegate>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, ReflectionUtils.ConstructorDelegate>(this.ContructorDelegateFactory));
			this.GetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<MemberInfo, ReflectionUtils.GetDelegate>>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, IDictionary<MemberInfo, ReflectionUtils.GetDelegate>>(this.GetterValueFactory));
			this.SetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(this.SetterValueFactory));
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000F244 File Offset: 0x0000D444
		protected virtual string MapClrMemberNameToJsonFieldName(MemberInfo memberInfo)
		{
			foreach (JsonProperty jsonProperty in memberInfo.GetCustomAttributes(typeof(JsonProperty), true))
			{
				if (!string.IsNullOrEmpty(jsonProperty.PropertyName))
				{
					return jsonProperty.PropertyName;
				}
			}
			return memberInfo.Name;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000F2A0 File Offset: 0x0000D4A0
		protected virtual void MapClrMemberNameToJsonFieldName(MemberInfo memberInfo, out string jsonName, out JsonProperty jsonProp)
		{
			jsonName = memberInfo.Name;
			jsonProp = null;
			foreach (JsonProperty jsonProperty in memberInfo.GetCustomAttributes(typeof(JsonProperty), true))
			{
				jsonProp = jsonProperty;
				if (!string.IsNullOrEmpty(jsonProperty.PropertyName))
				{
					jsonName = jsonProperty.PropertyName;
				}
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000F304 File Offset: 0x0000D504
		internal virtual ReflectionUtils.ConstructorDelegate ContructorDelegateFactory(Type key)
		{
			return ReflectionUtils.GetContructor(key, (!key.IsArray) ? PocoJsonSerializerStrategy.EmptyTypes : PocoJsonSerializerStrategy.ArrayConstructorParameterTypes);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000F334 File Offset: 0x0000D534
		internal virtual IDictionary<MemberInfo, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
		{
			IDictionary<MemberInfo, ReflectionUtils.GetDelegate> dictionary = new Dictionary<MemberInfo, ReflectionUtils.GetDelegate>();
			foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
			{
				if (propertyInfo.CanRead)
				{
					MethodInfo getterMethodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
					if (!getterMethodInfo.IsStatic && getterMethodInfo.IsPublic)
					{
						dictionary[propertyInfo] = ReflectionUtils.GetGetMethod(propertyInfo);
					}
				}
			}
			foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
			{
				if (!fieldInfo.IsStatic && fieldInfo.IsPublic)
				{
					dictionary[fieldInfo] = ReflectionUtils.GetGetMethod(fieldInfo);
				}
			}
			return dictionary;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000F450 File Offset: 0x0000D650
		internal virtual IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
		{
			IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> dictionary = new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
			foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
			{
				if (propertyInfo.CanWrite)
				{
					MethodInfo setterMethodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
					if (!setterMethodInfo.IsStatic && setterMethodInfo.IsPublic)
					{
						dictionary[this.MapClrMemberNameToJsonFieldName(propertyInfo)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(propertyInfo.PropertyType, ReflectionUtils.GetSetMethod(propertyInfo));
					}
				}
			}
			foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
			{
				if (!fieldInfo.IsInitOnly && !fieldInfo.IsStatic && fieldInfo.IsPublic)
				{
					dictionary[this.MapClrMemberNameToJsonFieldName(fieldInfo)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(fieldInfo.FieldType, ReflectionUtils.GetSetMethod(fieldInfo));
				}
			}
			return dictionary;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000F59C File Offset: 0x0000D79C
		public virtual bool TrySerializeNonPrimitiveObject(object input, out object output)
		{
			return this.TrySerializeKnownTypes(input, out output) || this.TrySerializeUnknownTypes(input, out output);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000F5B8 File Offset: 0x0000D7B8
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		public virtual object DeserializeObject(object value, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (value != null && type.IsInstanceOfType(value))
			{
				return value;
			}
			string text = value as string;
			if (type == typeof(Guid) && string.IsNullOrEmpty(text))
			{
				return default(Guid);
			}
			if (value == null)
			{
				return null;
			}
			object obj = null;
			if (text != null)
			{
				if (text.Length != 0)
				{
					if (type == typeof(DateTime) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTime)))
					{
						return DateTime.ParseExact(text, PocoJsonSerializerStrategy.Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
					}
					if (type == typeof(DateTimeOffset) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset)))
					{
						return DateTimeOffset.ParseExact(text, PocoJsonSerializerStrategy.Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
					}
					if (type == typeof(Guid) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid)))
					{
						return new Guid(text);
					}
					if (type == typeof(Uri))
					{
						bool flag = Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute);
						Uri result;
						if (flag && Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out result))
						{
							return result;
						}
						return null;
					}
					else
					{
						if (type == typeof(string))
						{
							return text;
						}
						return Convert.ChangeType(text, type, CultureInfo.InvariantCulture);
					}
				}
				else
				{
					if (type == typeof(Guid))
					{
						obj = default(Guid);
					}
					else if (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
					{
						obj = null;
					}
					else
					{
						obj = text;
					}
					if (!ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
					{
						return text;
					}
				}
			}
			else if (value is bool)
			{
				return value;
			}
			bool flag2 = value is long;
			bool flag3 = value is ulong;
			bool flag4 = value is double;
			Type underlyingType = Nullable.GetUnderlyingType(type);
			if (underlyingType != null && SimpleJson.NumberTypes.IndexOf(underlyingType) != -1)
			{
				type = underlyingType;
			}
			bool flag5 = SimpleJson.NumberTypes.IndexOf(type) != -1;
			bool isEnum = type.IsEnum;
			if ((flag2 && type == typeof(long)) || (flag3 && type == typeof(ulong)) || (flag4 && type == typeof(double)))
			{
				return value;
			}
			if ((flag2 || flag3 || flag4) && isEnum)
			{
				return Enum.ToObject(type, Convert.ChangeType(value, Enum.GetUnderlyingType(type), CultureInfo.InvariantCulture));
			}
			if ((flag2 || flag3 || flag4) && flag5)
			{
				return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
			}
			IDictionary<string, object> dictionary = value as IDictionary<string, object>;
			if (dictionary == null)
			{
				IList<object> list = value as IList<object>;
				if (list != null)
				{
					IList<object> list2 = list;
					IList list3 = null;
					if (type.IsArray)
					{
						list3 = (IList)this.ConstructorCache[type](new object[]
						{
							list2.Count
						});
						int num = 0;
						foreach (object value2 in list2)
						{
							list3[num++] = this.DeserializeObject(value2, type.GetElementType());
						}
					}
					else if (ReflectionUtils.IsTypeGenericeCollectionInterface(type) || ReflectionUtils.IsAssignableFrom(typeof(IList), type) || type == typeof(object))
					{
						Type genericListElementType = ReflectionUtils.GetGenericListElementType(type);
						ReflectionUtils.ConstructorDelegate constructorDelegate = null;
						if (type != typeof(object))
						{
							constructorDelegate = this.ConstructorCache[type];
						}
						if (constructorDelegate == null)
						{
							constructorDelegate = this.ConstructorCache[typeof(List<>).MakeGenericType(new Type[]
							{
								genericListElementType
							})];
						}
						list3 = (IList)constructorDelegate(new object[0]);
						foreach (object value3 in list2)
						{
							list3.Add(this.DeserializeObject(value3, genericListElementType));
						}
					}
					obj = list3;
				}
				return obj;
			}
			IDictionary<string, object> dictionary2 = dictionary;
			if (ReflectionUtils.IsTypeDictionary(type))
			{
				Type[] genericTypeArguments = ReflectionUtils.GetGenericTypeArguments(type);
				Type type2 = genericTypeArguments[0];
				Type type3 = genericTypeArguments[1];
				Type key = typeof(Dictionary<, >).MakeGenericType(new Type[]
				{
					type2,
					type3
				});
				IDictionary dictionary3 = (IDictionary)this.ConstructorCache[key](new object[0]);
				foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
				{
					dictionary3.Add(keyValuePair.Key, this.DeserializeObject(keyValuePair.Value, type3));
				}
				obj = dictionary3;
			}
			else if (type == typeof(object))
			{
				obj = value;
			}
			else
			{
				try
				{
					obj = this.ConstructorCache[type](new object[0]);
				}
				catch (Exception ex)
				{
					Debug.LogError("type " + type + " was not in the constructor cache.  It needs a parameterless constructor");
				}
				foreach (KeyValuePair<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> keyValuePair2 in this.SetCache[type])
				{
					object value4;
					if (dictionary2.TryGetValue(keyValuePair2.Key, out value4))
					{
						value4 = this.DeserializeObject(value4, keyValuePair2.Value.Key);
						keyValuePair2.Value.Value(obj, value4);
					}
				}
			}
			if (ReflectionUtils.IsNullableType(type))
			{
				return ReflectionUtils.ToNullableType(obj, type);
			}
			return obj;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000FCA0 File Offset: 0x0000DEA0
		protected virtual object SerializeEnum(Enum p)
		{
			return Convert.ToDouble(p, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
		[SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
		protected virtual bool TrySerializeKnownTypes(object input, out object output)
		{
			bool result = true;
			if (input is DateTime)
			{
				output = ((DateTime)input).ToUniversalTime().ToString(PocoJsonSerializerStrategy.Iso8601Format[0], CultureInfo.InvariantCulture);
			}
			else if (input is DateTimeOffset)
			{
				output = ((DateTimeOffset)input).ToUniversalTime().ToString(PocoJsonSerializerStrategy.Iso8601Format[0], CultureInfo.InvariantCulture);
			}
			else if (input is Guid)
			{
				output = ((Guid)input).ToString("D");
			}
			else if (input is Uri)
			{
				output = input.ToString();
			}
			else
			{
				Enum @enum = input as Enum;
				if (@enum != null)
				{
					output = this.SerializeEnum(@enum);
				}
				else
				{
					result = false;
					output = null;
				}
			}
			return result;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000FD8C File Offset: 0x0000DF8C
		[SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
		protected virtual bool TrySerializeUnknownTypes(object input, out object output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			output = null;
			Type type = input.GetType();
			if (type.FullName == null)
			{
				return false;
			}
			IDictionary<string, object> dictionary = new JsonObject();
			IDictionary<MemberInfo, ReflectionUtils.GetDelegate> dictionary2 = this.GetCache[type];
			foreach (KeyValuePair<MemberInfo, ReflectionUtils.GetDelegate> keyValuePair in dictionary2)
			{
				if (keyValuePair.Value != null)
				{
					string text;
					JsonProperty jsonProperty;
					this.MapClrMemberNameToJsonFieldName(keyValuePair.Key, out text, out jsonProperty);
					if (dictionary.ContainsKey(text))
					{
						throw new Exception("The given key is defined multiple times in the same type: " + input.GetType().Name + "." + text);
					}
					object obj = keyValuePair.Value(input);
					if (jsonProperty == null || jsonProperty.NullValueHandling == NullValueHandling.Include || obj != null)
					{
						dictionary.Add(text, obj);
					}
				}
			}
			output = dictionary;
			return true;
		}

		// Token: 0x040002B3 RID: 691
		internal IDictionary<Type, ReflectionUtils.ConstructorDelegate> ConstructorCache;

		// Token: 0x040002B4 RID: 692
		internal IDictionary<Type, IDictionary<MemberInfo, ReflectionUtils.GetDelegate>> GetCache;

		// Token: 0x040002B5 RID: 693
		internal IDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>> SetCache;

		// Token: 0x040002B6 RID: 694
		internal static readonly Type[] EmptyTypes = new Type[0];

		// Token: 0x040002B7 RID: 695
		internal static readonly Type[] ArrayConstructorParameterTypes = new Type[]
		{
			typeof(int)
		};

		// Token: 0x040002B8 RID: 696
		private static readonly string[] Iso8601Format = new string[]
		{
			"yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z",
			"yyyy-MM-dd\\THH:mm:ss\\Z",
			"yyyy-MM-dd\\THH:mm:ssK"
		};
	}
}
