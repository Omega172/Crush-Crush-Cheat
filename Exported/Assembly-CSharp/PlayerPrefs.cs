using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000101 RID: 257
public class PlayerPrefs
{
	// Token: 0x060005E4 RID: 1508 RVA: 0x0002E6CC File Offset: 0x0002C8CC
	private static void ExecuteSaveCallbacks()
	{
		if (global::PlayerPrefs.SaveCallbacks.Count == 0)
		{
			return;
		}
		foreach (Action action in global::PlayerPrefs.SaveCallbacks)
		{
			action();
		}
		global::PlayerPrefs.SaveCallbacks.Clear();
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0002E74C File Offset: 0x0002C94C
	public static Dictionary<string, object> Import(string base64)
	{
		string s = string.Empty;
		try
		{
			if (base64.StartsWith("lzfc"))
			{
				s = Encoding.UTF8.GetString(Utilities.CLZF2.Decompress(Convert.FromBase64String(base64.Substring(4))));
			}
			else
			{
				s = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
			}
		}
		catch (Exception ex)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "PlayerPrefs.Import: " + ex.Message);
			return new Dictionary<string, object>();
		}
		StringReader stringReader = new StringReader(s);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string str = string.Empty;
		for (;;)
		{
			string text = stringReader.ReadLine();
			if (text == null || text.Length == 0)
			{
				break;
			}
			if (text.StartsWith("::"))
			{
				if (text.Length == 2)
				{
					str = string.Empty;
				}
				else
				{
					str = text.Substring(2);
				}
			}
			else if (text.Contains(":"))
			{
				string text2 = str + text.Substring(0, text.IndexOf(":"));
				string text3 = text.Substring(text.IndexOf(":") + 1);
				if (dictionary.ContainsKey(text2.Trim()))
				{
					Utilities.SendAnalytic(Utilities.AnalyticType.Exception, text2.Trim() + ":" + text3 + " was duplicate.  PlayerPrefs.cs line 73");
				}
				else if (text3.EndsWith("f"))
				{
					float num;
					if (float.TryParse(text3.Substring(0, text3.Length - 1), out num))
					{
						dictionary.Add(text2.Trim(), num);
					}
					else
					{
						dictionary.Add(text2.Trim(), text3);
						if (text2.Trim() != "GameStateBuild" && text2.Trim() != "CompletedEvents" && ((text2.Length != 3 && text2.Length != 4) || text2[0] != 'C' || text2[text2.Length - 1] != 'P'))
						{
							Utilities.SendAnalytic(Utilities.AnalyticType.Exception, string.Format("PlayerPrefs {0} contained {1}", text2.Trim(), text3));
						}
					}
				}
				else if (text3.EndsWith("i"))
				{
					int num2 = 0;
					if (int.TryParse(text3.Substring(0, text3.Length - 1), out num2))
					{
						dictionary.Add(text2.Trim(), num2);
					}
					else
					{
						dictionary.Add(text2.Trim(), text3);
						if (text2.Trim() != "GameStateBuild" && text2.Trim() != "CompletedEvents" && ((text2.Length != 3 && text2.Length != 4) || text2[0] != 'C' || text2[text2.Length - 1] != 'P'))
						{
							Utilities.SendAnalytic(Utilities.AnalyticType.Exception, string.Format("PlayerPrefs {0} contained {1}", text2.Trim(), text3));
						}
					}
				}
				else
				{
					dictionary.Add(text2.Trim(), text3);
				}
			}
			else if (dictionary.ContainsKey((str + text).Trim()))
			{
				Utilities.SendAnalytic(Utilities.AnalyticType.Exception, (str + text).Trim() + ":1 was duplicate.  PlayerPrefs.cs line 99");
			}
			else
			{
				dictionary.Add((str + text).Trim(), 1);
			}
		}
		return dictionary;
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0002EB08 File Offset: 0x0002CD08
	public static void Import(Dictionary<string, object> values, bool loadScene0 = true)
	{
		global::PlayerPrefs.DeleteAll();
		if (loadScene0)
		{
			if (GameState.CurrentState != null)
			{
				GameState.CurrentState.PrepareReset();
			}
			Girl.Reset(false);
			TaskManager.CompletedEvents = new byte[0];
		}
		foreach (KeyValuePair<string, object> keyValuePair in values)
		{
			if (!global::PlayerPrefs.prefs.ContainsKey(keyValuePair.Key))
			{
				global::PlayerPrefs.prefs.Add(keyValuePair.Key, keyValuePair.Value);
			}
			else
			{
				global::PlayerPrefs.prefs[keyValuePair.Key] = keyValuePair.Value;
			}
			if (!global::PlayerPrefs.dirty.Contains(keyValuePair.Key))
			{
				global::PlayerPrefs.dirty.Add(keyValuePair.Key);
			}
		}
		global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
		global::PlayerPrefs.SetInt("PlayfabInventory", (int)Playfab.InventoryObjects);
		global::PlayerPrefs.Save();
		if (loadScene0)
		{
			GameState.Initialized = false;
			SceneManager.LoadScene(0);
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002EC40 File Offset: 0x0002CE40
	public static void ExportToSteam()
	{
		try
		{
			SteamManager.Instance.SaveToCloudSave();
		}
		catch (Exception ex)
		{
			Debug.LogWarning(string.Format("There was an error while writing the saves file to disk: {0}", ex.Message));
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "ExportToSteam: " + ex.Message);
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0002ECAC File Offset: 0x0002CEAC
	public static string Export()
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, object> keyValuePair in global::PlayerPrefs.prefs)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort();
		foreach (string text in list)
		{
			if (global::PlayerPrefs.prefs.ContainsKey(text))
			{
				object obj = global::PlayerPrefs.prefs[text];
				if (!text.Contains("DO-NOT-BACKUP"))
				{
					if (obj is int && (int)obj == 1)
					{
						stringBuilder.AppendLine(global::PlayerPrefs.ShrinkKey(text, stringBuilder));
					}
					else if (!(obj is int) || (int)obj != 0 || text.StartsWith("Skill"))
					{
						if (obj is float)
						{
							stringBuilder.AppendLine(string.Format("{0}:{1}", global::PlayerPrefs.ShrinkKey(text, stringBuilder), obj.ToString() + "f"));
						}
						else if (obj is int)
						{
							stringBuilder.AppendLine(string.Format("{0}:{1}", global::PlayerPrefs.ShrinkKey(text, stringBuilder), obj.ToString() + "i"));
						}
						else
						{
							stringBuilder.AppendLine(string.Format("{0}:{1}", global::PlayerPrefs.ShrinkKey(text, stringBuilder), obj.ToString()));
						}
					}
				}
			}
		}
		byte[] inArray = Utilities.CLZF2.Compress(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
		string str = Convert.ToBase64String(inArray);
		return "lzfc" + str;
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0002EECC File Offset: 0x0002D0CC
	private static string ShrinkKey(string key, StringBuilder sb)
	{
		foreach (string text in global::PlayerPrefs.validPrefixes)
		{
			if (key.StartsWith(text))
			{
				if (global::PlayerPrefs.currentKeyPrefix != text)
				{
					global::PlayerPrefs.currentKeyPrefix = text;
					sb.AppendLine("::" + text);
				}
				return key.Substring(text.Length);
			}
		}
		sb.AppendLine("::");
		return key;
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0002EF48 File Offset: 0x0002D148
	public static void DeleteAll()
	{
		global::PlayerPrefs.prefs.Clear();
		global::PlayerPrefs.dirty.Clear();
		int @int = UnityEngine.PlayerPrefs.GetInt("Screenmanager Is Fullscreen mode");
		int int2 = UnityEngine.PlayerPrefs.GetInt("Screenmanager Resolution Height");
		int int3 = UnityEngine.PlayerPrefs.GetInt("Screenmanager Resolution Width");
		int int4 = UnityEngine.PlayerPrefs.GetInt("UnityGraphicsQuality");
		int int5 = UnityEngine.PlayerPrefs.GetInt("UnitySelectMonitor");
		UnityEngine.PlayerPrefs.DeleteAll();
		UnityEngine.PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", @int);
		UnityEngine.PlayerPrefs.SetInt("Screenmanager Resolution Height", int2);
		UnityEngine.PlayerPrefs.SetInt("Screenmanager Resolution Width", int3);
		UnityEngine.PlayerPrefs.SetInt("UnityGraphicsQuality", int4);
		UnityEngine.PlayerPrefs.SetInt("UnitySelectMonitor", int5);
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0002EFE0 File Offset: 0x0002D1E0
	public static void Save()
	{
		if (global::PlayerPrefs.dirty.Count == 0)
		{
			return;
		}
		global::PlayerPrefs.ExportToSteam();
		int num = 0;
		foreach (string text in global::PlayerPrefs.dirty)
		{
			if (global::PlayerPrefs.prefs.ContainsKey(text))
			{
				object obj = global::PlayerPrefs.prefs[text];
				string encodedString = global::PlayerPrefs.GetEncodedString(text);
				long num2;
				if (obj is string && text != "GameStateDiamonds" && text != "GameStateMoney" && text != "GameStateTotalIncome" && text != "GameStateDate" && long.TryParse((string)obj, out num2))
				{
					obj = num2;
				}
				if (obj is int)
				{
					UnityEngine.PlayerPrefs.SetInt(encodedString, (int)obj);
				}
				else if (obj is float)
				{
					UnityEngine.PlayerPrefs.SetFloat(encodedString, (float)obj);
				}
				else if (obj is string)
				{
					UnityEngine.PlayerPrefs.SetString(encodedString, (string)obj);
				}
				else if (obj is long)
				{
					UnityEngine.PlayerPrefs.SetInt(encodedString, (int)((long)obj & (long)((ulong)-1)));
					UnityEngine.PlayerPrefs.SetInt(encodedString + "h", (int)((long)obj >> 32));
				}
				else
				{
					Debug.Log(string.Concat(new object[]
					{
						"The preference ",
						text,
						" has an invalid type of ",
						global::PlayerPrefs.prefs[text].GetType()
					}));
				}
			}
			num++;
		}
		UnityEngine.PlayerPrefs.Save();
		global::PlayerPrefs.dirty.Clear();
		global::PlayerPrefs.ExecuteSaveCallbacks();
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0002F1C0 File Offset: 0x0002D3C0
	private static string GetEncodedString(string pref)
	{
		if (!global::PlayerPrefs.encodedStrings.ContainsKey(pref))
		{
			global::PlayerPrefs.encodedStrings.Add(pref, Convert.ToBase64String(Encoding.UTF8.GetBytes(pref)));
		}
		return global::PlayerPrefs.encodedStrings[pref];
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0002F204 File Offset: 0x0002D404
	public static float GetFloat(string name)
	{
		return global::PlayerPrefs.GetFloat(name, 0f);
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0002F214 File Offset: 0x0002D414
	public static float GetFloat(string name, float defaultValue)
	{
		if (!global::PlayerPrefs.prefs.ContainsKey(name))
		{
			float @float = UnityEngine.PlayerPrefs.GetFloat(global::PlayerPrefs.GetEncodedString(name), defaultValue);
			global::PlayerPrefs.prefs.Add(name, @float);
			return @float;
		}
		if (global::PlayerPrefs.prefs[name] is float)
		{
			return (float)global::PlayerPrefs.prefs[name];
		}
		Debug.Log("Tried to get float from preferences when actual type was " + global::PlayerPrefs.prefs[name].GetType());
		return defaultValue;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0002F298 File Offset: 0x0002D498
	public static void SetFloat(string name, float value)
	{
		if (global::PlayerPrefs.prefs.ContainsKey(name))
		{
			if (global::PlayerPrefs.prefs[name] is float && (float)global::PlayerPrefs.prefs[name] == value)
			{
				return;
			}
			global::PlayerPrefs.prefs[name] = value;
		}
		else
		{
			global::PlayerPrefs.prefs.Add(name, value);
		}
		if (!global::PlayerPrefs.dirty.Contains(name))
		{
			global::PlayerPrefs.dirty.Add(name);
		}
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0002F324 File Offset: 0x0002D524
	public static int GetInt(string name)
	{
		return global::PlayerPrefs.GetInt(name, 0);
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0002F330 File Offset: 0x0002D530
	public static int GetInt(string name, int defaultValue)
	{
		if (!global::PlayerPrefs.prefs.ContainsKey(name))
		{
			int @int = UnityEngine.PlayerPrefs.GetInt(global::PlayerPrefs.GetEncodedString(name), defaultValue);
			global::PlayerPrefs.prefs.Add(name, @int);
			return @int;
		}
		if (global::PlayerPrefs.prefs[name] is int)
		{
			return (int)global::PlayerPrefs.prefs[name];
		}
		if (int.TryParse(global::PlayerPrefs.prefs[name].ToString(), out defaultValue))
		{
			return defaultValue;
		}
		Debug.Log("Tried to get int from preferences when actual type was " + global::PlayerPrefs.prefs[name].GetType());
		return defaultValue;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0002F3D4 File Offset: 0x0002D5D4
	public static void SetInt(string name, int value)
	{
		if (global::PlayerPrefs.prefs.ContainsKey(name))
		{
			if (global::PlayerPrefs.prefs[name] is int && (int)global::PlayerPrefs.prefs[name] == value)
			{
				return;
			}
			global::PlayerPrefs.prefs[name] = value;
		}
		else
		{
			global::PlayerPrefs.prefs.Add(name, value);
		}
		if (!global::PlayerPrefs.dirty.Contains(name))
		{
			global::PlayerPrefs.dirty.Add(name);
		}
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0002F460 File Offset: 0x0002D660
	public static long GetLong(string name, long defaultValue)
	{
		if (!global::PlayerPrefs.prefs.ContainsKey(name))
		{
			long num = (long)UnityEngine.PlayerPrefs.GetInt(global::PlayerPrefs.GetEncodedString(name), (int)(defaultValue & (long)((ulong)-1)));
			long num2 = (long)UnityEngine.PlayerPrefs.GetInt(global::PlayerPrefs.GetEncodedString(name) + "h", (int)(defaultValue >> 32 & (long)((ulong)-1)));
			long num3 = (long)((ulong)((uint)num) | (ulong)((ulong)num2 << 32));
			global::PlayerPrefs.prefs.Add(name, num3);
			return num3;
		}
		if (global::PlayerPrefs.prefs[name] is long)
		{
			return (long)global::PlayerPrefs.prefs[name];
		}
		if (global::PlayerPrefs.prefs[name] is string)
		{
			long num4 = defaultValue;
			if (long.TryParse((string)global::PlayerPrefs.prefs[name], out num4))
			{
				global::PlayerPrefs.prefs[name] = num4;
			}
			else
			{
				global::PlayerPrefs.prefs[name] = defaultValue;
			}
			return num4;
		}
		if (global::PlayerPrefs.prefs[name] is int)
		{
			long num5 = (long)((int)global::PlayerPrefs.prefs[name]);
			global::PlayerPrefs.prefs[name] = num5;
			return num5;
		}
		Debug.Log("Tried to get long from preferences when actual type was " + global::PlayerPrefs.prefs[name].GetType());
		return defaultValue;
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0002F5AC File Offset: 0x0002D7AC
	public static void SetLong(string name, long value)
	{
		if (global::PlayerPrefs.prefs.ContainsKey(name))
		{
			if (global::PlayerPrefs.prefs[name] is long && (long)global::PlayerPrefs.prefs[name] == value)
			{
				return;
			}
			global::PlayerPrefs.prefs[name] = value;
		}
		else
		{
			global::PlayerPrefs.prefs.Add(name, value);
		}
		if (!global::PlayerPrefs.dirty.Contains(name))
		{
			global::PlayerPrefs.dirty.Add(name);
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0002F638 File Offset: 0x0002D838
	public static string GetString(string name)
	{
		return global::PlayerPrefs.GetString(name, string.Empty);
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0002F648 File Offset: 0x0002D848
	public static string GetString(string name, string defaultValue)
	{
		if (!global::PlayerPrefs.prefs.ContainsKey(name))
		{
			string @string = UnityEngine.PlayerPrefs.GetString(global::PlayerPrefs.GetEncodedString(name), defaultValue);
			global::PlayerPrefs.prefs.Add(name, @string);
			return @string;
		}
		if (global::PlayerPrefs.prefs[name] is string)
		{
			return (string)global::PlayerPrefs.prefs[name];
		}
		Debug.Log("Tried to get string from preferences when actual type was " + global::PlayerPrefs.prefs[name].GetType());
		return defaultValue;
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0002F6C8 File Offset: 0x0002D8C8
	public static void SetString(string name, string value)
	{
		if (global::PlayerPrefs.prefs.ContainsKey(name))
		{
			if (global::PlayerPrefs.prefs[name] is string && (string)global::PlayerPrefs.prefs[name] == value)
			{
				return;
			}
			global::PlayerPrefs.prefs[name] = value;
		}
		else
		{
			global::PlayerPrefs.prefs.Add(name, value);
		}
		if (!global::PlayerPrefs.dirty.Contains(name))
		{
			global::PlayerPrefs.dirty.Add(name);
		}
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0002F750 File Offset: 0x0002D950
	public static void DeleteKey(string name, bool isLong = false)
	{
		if (global::PlayerPrefs.prefs.ContainsKey(name))
		{
			global::PlayerPrefs.prefs.Remove(name);
		}
		global::PlayerPrefs.dirty.Add(name);
		UnityEngine.PlayerPrefs.DeleteKey(global::PlayerPrefs.GetEncodedString(name));
		if (isLong || (global::PlayerPrefs.prefs.ContainsKey(name) && global::PlayerPrefs.prefs[name] is long))
		{
			UnityEngine.PlayerPrefs.DeleteKey(global::PlayerPrefs.GetEncodedString(name) + "h");
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0002F7D0 File Offset: 0x0002D9D0
	public static bool HasKey(string name)
	{
		return global::PlayerPrefs.prefs.ContainsKey(name) || UnityEngine.PlayerPrefs.HasKey(global::PlayerPrefs.GetEncodedString(name));
	}

	// Token: 0x040005AD RID: 1453
	public const string DoNotBackupPrefix = "DO-NOT-BACKUP";

	// Token: 0x040005AE RID: 1454
	private static Dictionary<string, object> prefs = new Dictionary<string, object>();

	// Token: 0x040005AF RID: 1455
	private static HashSet<string> dirty = new HashSet<string>();

	// Token: 0x040005B0 RID: 1456
	public static List<Action> SaveCallbacks = new List<Action>();

	// Token: 0x040005B1 RID: 1457
	private static string currentKeyPrefix = string.Empty;

	// Token: 0x040005B2 RID: 1458
	private static string[] validPrefixes = new string[]
	{
		"JobFAST FOOD",
		"JobRESTAURANT",
		"JobLIFEGUARD",
		"JobCLEANING",
		"JobART",
		"JobMOVIES",
		"JobWIZARD",
		"JobSLAYING",
		"JobCASINO",
		"JobCOMPUTERS",
		"JobZOO",
		"JobHUNTING",
		"JobSPORTS",
		"JobLEGAL",
		"JobLOVE",
		"JobSPACE",
		"HobbySuave",
		"HobbyFunny",
		"HobbyBuff",
		"HobbyTechSavvy",
		"HobbyTenderness",
		"HobbyMotivation",
		"HobbyWisdom",
		"HobbyBadass",
		"HobbySmart",
		"HobbyAngst",
		"HobbyMysterious",
		"HobbyLucky",
		"GirlCassie",
		"GirlMio",
		"GirlQuill",
		"GirlElle",
		"GirlIro",
		"GirlBonnibel",
		"GirlFumi",
		"GirlBearverly",
		"GirlNina",
		"GirlPamu",
		"GirlAlpha",
		"GirlSutra",
		"GirlKarma",
		"GirlEva",
		"GirlLuna",
		"GirlQPiddy",
		"GirlDarkOne",
		"GirlNutaku",
		"GirlAyano",
		"GirlJelle",
		"GirlBonchovy",
		"GirlQuillzone",
		"GirlSpectrum",
		"GirlCharlotte",
		"GirlOdango",
		"GirlShibuki",
		"GirlSirina",
		"GirlCatara",
		"GirlVellatrix",
		"GirlPeanut",
		"GirlRoxxy",
		"GirlTessa",
		"GirlClaudia",
		"GirlRosa",
		"GirlJuliet",
		"Settings",
		"ACH",
		"Skill",
		"GameState",
		"Task",
		"Playfab",
		"album",
		"Completed"
	};

	// Token: 0x040005B3 RID: 1459
	private static Dictionary<string, string> encodedStrings = new Dictionary<string, string>();
}
