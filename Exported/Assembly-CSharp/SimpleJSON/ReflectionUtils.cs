using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SimpleJSON
{
	// Token: 0x0200008E RID: 142
	[GeneratedCode("reflection-utils", "1.0.0")]
	internal class ReflectionUtils
	{
		// Token: 0x06000213 RID: 531 RVA: 0x0000FEC4 File Offset: 0x0000E0C4
		public static Type GetTypeInfo(Type type)
		{
			return type;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000FEC8 File Offset: 0x0000E0C8
		public static Attribute GetAttribute(MemberInfo info, Type type)
		{
			if (info == null || type == null || !Attribute.IsDefined(info, type))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(info, type);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000FEEC File Offset: 0x0000E0EC
		public static Type GetGenericListElementType(Type type)
		{
			if (type == typeof(object))
			{
				return type;
			}
			IEnumerable<Type> interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (ReflectionUtils.IsTypeGeneric(type2) && type2.GetGenericTypeDefinition() == typeof(IList<>))
				{
					return ReflectionUtils.GetGenericTypeArguments(type2)[0];
				}
			}
			return ReflectionUtils.GetGenericTypeArguments(type)[0];
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000FF94 File Offset: 0x0000E194
		public static Attribute GetAttribute(Type objectType, Type attributeType)
		{
			if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(objectType, attributeType);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		public static Type[] GetGenericTypeArguments(Type type)
		{
			return type.GetGenericArguments();
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000FFC0 File Offset: 0x0000E1C0
		public static bool IsTypeGeneric(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsGenericType;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000FFD0 File Offset: 0x0000E1D0
		public static bool IsTypeGenericeCollectionInterface(Type type)
		{
			if (!ReflectionUtils.IsTypeGeneric(type))
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IEnumerable<>);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00010024 File Offset: 0x0000E224
		public static bool IsAssignableFrom(Type type1, Type type2)
		{
			return ReflectionUtils.GetTypeInfo(type1).IsAssignableFrom(ReflectionUtils.GetTypeInfo(type2));
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00010038 File Offset: 0x0000E238
		public static bool IsTypeDictionary(Type type)
		{
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				return true;
			}
			if (!ReflectionUtils.GetTypeInfo(type).IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IDictionary<, >) || genericTypeDefinition == typeof(Dictionary<, >);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00010098 File Offset: 0x0000E298
		public static bool IsNullableType(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000100CC File Offset: 0x0000E2CC
		public static object ToNullableType(object obj, Type nullableType)
		{
			return (obj != null) ? Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), CultureInfo.InvariantCulture) : null;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000100EC File Offset: 0x0000E2EC
		public static bool IsValueType(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsValueType;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000100FC File Offset: 0x0000E2FC
		public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
		{
			return type.GetConstructors();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00010104 File Offset: 0x0000E304
		public static ConstructorInfo GetConstructorInfo(Type type, params Type[] argsType)
		{
			IEnumerable<ConstructorInfo> constructors = ReflectionUtils.GetConstructors(type);
			foreach (ConstructorInfo constructorInfo in constructors)
			{
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				if (argsType.Length == parameters.Length)
				{
					int num = 0;
					bool flag = true;
					foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
					{
						if (parameterInfo.ParameterType != argsType[num])
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return constructorInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000101D4 File Offset: 0x0000E3D4
		public static IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000101E0 File Offset: 0x0000E3E0
		public static IEnumerable<FieldInfo> GetFields(Type type)
		{
			return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000101EC File Offset: 0x0000E3EC
		public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000101F8 File Offset: 0x0000E3F8
		public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00010204 File Offset: 0x0000E404
		public static ReflectionUtils.ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
		{
			return ReflectionUtils.GetConstructorByReflection(constructorInfo);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0001020C File Offset: 0x0000E40C
		public static ReflectionUtils.ConstructorDelegate GetContructor(Type type, params Type[] argsType)
		{
			return ReflectionUtils.GetConstructorByReflection(type, argsType);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00010218 File Offset: 0x0000E418
		public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(ConstructorInfo constructorInfo)
		{
			return delegate(object[] args)
			{
				ConstructorInfo constructorInfo2 = constructorInfo;
				return constructorInfo2.Invoke(args);
			};
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00010240 File Offset: 0x0000E440
		public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(Type type, params Type[] argsType)
		{
			ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
			return (constructorInfo != null) ? ReflectionUtils.GetConstructorByReflection(constructorInfo) : null;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00010268 File Offset: 0x0000E468
		public static ReflectionUtils.GetDelegate GetGetMethod(PropertyInfo propertyInfo)
		{
			return ReflectionUtils.GetGetMethodByReflection(propertyInfo);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00010270 File Offset: 0x0000E470
		public static ReflectionUtils.GetDelegate GetGetMethod(FieldInfo fieldInfo)
		{
			return ReflectionUtils.GetGetMethodByReflection(fieldInfo);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00010278 File Offset: 0x0000E478
		public static ReflectionUtils.GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
		{
			MethodInfo methodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
			return (object source) => methodInfo.Invoke(source, ReflectionUtils.EmptyObjects);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000102A4 File Offset: 0x0000E4A4
		public static ReflectionUtils.GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
		{
			return (object source) => fieldInfo.GetValue(source);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000102CC File Offset: 0x0000E4CC
		public static ReflectionUtils.SetDelegate GetSetMethod(PropertyInfo propertyInfo)
		{
			return ReflectionUtils.GetSetMethodByReflection(propertyInfo);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000102D4 File Offset: 0x0000E4D4
		public static ReflectionUtils.SetDelegate GetSetMethod(FieldInfo fieldInfo)
		{
			return ReflectionUtils.GetSetMethodByReflection(fieldInfo);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000102DC File Offset: 0x0000E4DC
		public static ReflectionUtils.SetDelegate GetSetMethodByReflection(PropertyInfo propertyInfo)
		{
			MethodInfo methodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
			return delegate(object source, object value)
			{
				if (ReflectionUtils._1ObjArray == null)
				{
					ReflectionUtils._1ObjArray = new object[1];
				}
				ReflectionUtils._1ObjArray[0] = value;
				methodInfo.Invoke(source, ReflectionUtils._1ObjArray);
			};
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00010308 File Offset: 0x0000E508
		public static ReflectionUtils.SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
		{
			return delegate(object source, object value)
			{
				fieldInfo.SetValue(source, value);
			};
		}

		// Token: 0x040002B9 RID: 697
		private static readonly object[] EmptyObjects = new object[0];

		// Token: 0x040002BA RID: 698
		[ThreadStatic]
		private static object[] _1ObjArray;

		// Token: 0x0200008F RID: 143
		public sealed class ThreadSafeDictionary<TKey, TValue> : IEnumerable, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
		{
			// Token: 0x06000231 RID: 561 RVA: 0x00010330 File Offset: 0x0000E530
			public ThreadSafeDictionary(ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
			{
				this._valueFactory = valueFactory;
			}

			// Token: 0x06000232 RID: 562 RVA: 0x0001034C File Offset: 0x0000E54C
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}

			// Token: 0x06000233 RID: 563 RVA: 0x00010360 File Offset: 0x0000E560
			private TValue Get(TKey key)
			{
				if (this._dictionary == null)
				{
					return this.AddValue(key);
				}
				TValue result;
				if (!this._dictionary.TryGetValue(key, out result))
				{
					return this.AddValue(key);
				}
				return result;
			}

			// Token: 0x06000234 RID: 564 RVA: 0x0001039C File Offset: 0x0000E59C
			private TValue AddValue(TKey key)
			{
				TValue tvalue = this._valueFactory(key);
				object @lock = this._lock;
				lock (@lock)
				{
					if (this._dictionary == null)
					{
						this._dictionary = new Dictionary<TKey, TValue>();
						this._dictionary[key] = tvalue;
					}
					else
					{
						TValue result;
						if (this._dictionary.TryGetValue(key, out result))
						{
							return result;
						}
						Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(this._dictionary);
						dictionary[key] = tvalue;
						this._dictionary = dictionary;
					}
				}
				return tvalue;
			}

			// Token: 0x06000235 RID: 565 RVA: 0x00010450 File Offset: 0x0000E650
			public void Add(TKey key, TValue value)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000236 RID: 566 RVA: 0x00010458 File Offset: 0x0000E658
			public bool ContainsKey(TKey key)
			{
				return this._dictionary.ContainsKey(key);
			}

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x06000237 RID: 567 RVA: 0x00010468 File Offset: 0x0000E668
			public ICollection<TKey> Keys
			{
				get
				{
					return this._dictionary.Keys;
				}
			}

			// Token: 0x06000238 RID: 568 RVA: 0x00010478 File Offset: 0x0000E678
			public bool Remove(TKey key)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000239 RID: 569 RVA: 0x00010480 File Offset: 0x0000E680
			public bool TryGetValue(TKey key, out TValue value)
			{
				value = this[key];
				return true;
			}

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x0600023A RID: 570 RVA: 0x00010490 File Offset: 0x0000E690
			public ICollection<TValue> Values
			{
				get
				{
					return this._dictionary.Values;
				}
			}

			// Token: 0x1700002C RID: 44
			public TValue this[TKey key]
			{
				get
				{
					return this.Get(key);
				}
				set
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x0600023D RID: 573 RVA: 0x000104B4 File Offset: 0x0000E6B4
			public void Add(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600023E RID: 574 RVA: 0x000104BC File Offset: 0x0000E6BC
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600023F RID: 575 RVA: 0x000104C4 File Offset: 0x0000E6C4
			public bool Contains(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000240 RID: 576 RVA: 0x000104CC File Offset: 0x0000E6CC
			public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x06000241 RID: 577 RVA: 0x000104D4 File Offset: 0x0000E6D4
			public int Count
			{
				get
				{
					return this._dictionary.Count;
				}
			}

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x06000242 RID: 578 RVA: 0x000104E4 File Offset: 0x0000E6E4
			public bool IsReadOnly
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x06000243 RID: 579 RVA: 0x000104EC File Offset: 0x0000E6EC
			public bool Remove(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000244 RID: 580 RVA: 0x000104F4 File Offset: 0x0000E6F4
			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}

			// Token: 0x040002BB RID: 699
			private readonly object _lock = new object();

			// Token: 0x040002BC RID: 700
			private readonly ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;

			// Token: 0x040002BD RID: 701
			private Dictionary<TKey, TValue> _dictionary;
		}

		// Token: 0x02000235 RID: 565
		// (Invoke) Token: 0x060011D8 RID: 4568
		public delegate object GetDelegate(object source);

		// Token: 0x02000236 RID: 566
		// (Invoke) Token: 0x060011DC RID: 4572
		public delegate void SetDelegate(object source, object value);

		// Token: 0x02000237 RID: 567
		// (Invoke) Token: 0x060011E0 RID: 4576
		public delegate object ConstructorDelegate(params object[] args);

		// Token: 0x02000238 RID: 568
		// (Invoke) Token: 0x060011E4 RID: 4580
		public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);
	}
}
