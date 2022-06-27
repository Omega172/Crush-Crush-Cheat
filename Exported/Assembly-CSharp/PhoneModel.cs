using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class PhoneModel
{
	// Token: 0x06000952 RID: 2386 RVA: 0x0004ECEC File Offset: 0x0004CEEC
	public PhoneModel(AssetBundle gamedata, string[] csv)
	{
		if (csv[3] == "Locked" || string.IsNullOrEmpty(csv[2]) || csv[4] == "Locked")
		{
			return;
		}
		this.Id = short.Parse(csv[0]);
		this.Name = csv[1];
		this.AssetBundle = csv[2];
		this.DatePref = new CachedPlayerPref(string.Format("C{0}D", csv[0]));
		this.PathPref = new PhoneModel.PhonePathPref(this);
		this.Messages = new List<PhoneModel.PhoneMessage>();
		bool flag = false;
		int num = 0;
		string translationId = PhoneModel.PhoneMessage.GetTranslationId(this.AssetBundle, this.Id);
		TextAsset textAsset = gamedata.LoadAsset<TextAsset>(translationId);
		if (textAsset == null)
		{
			Debug.LogError("There was no text sheet for " + this.Name);
		}
		else
		{
			List<string[]> list = Universe.FromCsv(textAsset.text);
			PhoneModel.PhoneMessage phoneMessage = null;
			int i = 1;
			while (i < list.Count)
			{
				if (string.IsNullOrEmpty(list[i][3]) && string.IsNullOrEmpty(list[i][1]) && (list[i].Length < 9 || string.IsNullOrEmpty(list[i][8])))
				{
					break;
				}
				PhoneModel.PhoneMessage phoneMessage2 = null;
				if (string.IsNullOrEmpty(list[i][1]))
				{
					if (this.StartsWithSprite(list[i][3]))
					{
						phoneMessage2 = new PhoneModel.PhoneImage
						{
							SpriteName1 = list[i][3].Substring(7)
						};
						this.SpriteCount++;
					}
					else if (list[i].Length > 8 && this.StartsWithSprite(list[i][8]))
					{
						phoneMessage2 = new PhoneModel.PhoneImage
						{
							SpriteName1 = list[i][8].Substring(7),
							SpriteName2 = list[i][10].Substring(7)
						};
						this.SpriteCount++;
					}
					else if (this.StartsWithHidden(list[i][3]))
					{
						phoneMessage2 = new PhoneModel.PhoneImage
						{
							SpriteName1 = list[i][3].Substring(7),
							InGallery = false
						};
					}
					else
					{
						phoneMessage2 = new PhoneModel.PhoneMessage
						{
							Message = list[i][3],
							MessageNSFW = list[i][4]
						};
						if (list[i].Length > 8 && !string.IsNullOrEmpty(list[i][8]))
						{
							phoneMessage2.Message = list[i][8];
							phoneMessage2.MessageNSFW = list[i][9];
							phoneMessage2.AlternateMessage = list[i][10];
							if (list[i].Length > 11)
							{
								phoneMessage2.AlternateMessageNSFW = list[i][11];
							}
						}
					}
					goto IL_4D8;
				}
				if (list[i][1] == "-")
				{
					flag = true;
					num++;
				}
				else
				{
					if (this.StartsWithSprite(list[i][1]))
					{
						phoneMessage2 = new PhoneModel.PhoneImage
						{
							SpriteName1 = list[i][1].Substring(7),
							FromPlayer = true
						};
						this.SpriteCount++;
						goto IL_4D8;
					}
					if (this.StartsWithHidden(list[i][1]))
					{
						phoneMessage2 = new PhoneModel.PhoneImage
						{
							SpriteName1 = list[i][1].Substring(7),
							FromPlayer = true,
							InGallery = false
						};
						goto IL_4D8;
					}
					if (this.Id == 9 && list[i][1].Contains("20XX"))
					{
						list[i][1] = list[i][1].Replace("20XX", DateTime.Now.Year.ToString());
					}
					phoneMessage2 = new PhoneModel.PhoneResponse
					{
						Response1 = new PhoneModel.PhoneMessage
						{
							Message = list[i][1],
							MessageNSFW = list[i][2]
						},
						Response2 = new PhoneModel.PhoneMessage
						{
							Message = list[i][5],
							MessageNSFW = list[i][6]
						}
					};
					((PhoneModel.PhoneResponse)phoneMessage2).Response1.Parent = (PhoneModel.PhoneResponse)phoneMessage2;
					((PhoneModel.PhoneResponse)phoneMessage2).Response2.Parent = (PhoneModel.PhoneResponse)phoneMessage2;
					goto IL_4D8;
				}
				IL_675:
				i++;
				continue;
				IL_4D8:
				phoneMessage2.Id = short.Parse(list[i][0]);
				try
				{
					if (this.Id == 16 && phoneMessage2.Id >= 170 && phoneMessage2.Id <= 237)
					{
						phoneMessage2.Time = 0f;
					}
					else if (this.Id == 13 && phoneMessage2.Id >= 151 && phoneMessage2.Id <= 261)
					{
						phoneMessage2.Time = 0f;
					}
					else
					{
						phoneMessage2.Time = (float)int.Parse(list[i][7]);
					}
				}
				catch (Exception)
				{
					Debug.LogError(string.Concat(new object[]
					{
						this.Name,
						" fling ",
						list[i][7],
						" (row ",
						i,
						") contained bad data"
					}));
				}
				phoneMessage2.NewConversation = flag;
				if (flag)
				{
					phoneMessage2.ConversationId = phoneMessage.ConversationId + 1;
				}
				else if (phoneMessage != null)
				{
					phoneMessage2.ConversationId = phoneMessage.ConversationId;
				}
				flag = false;
				if (phoneMessage != null)
				{
					phoneMessage.NextMessage = phoneMessage2;
				}
				else
				{
					this.FirstMessage = phoneMessage2;
				}
				if (phoneMessage is PhoneModel.PhoneResponse)
				{
					phoneMessage2.Time = Mathf.Max(2f, phoneMessage2.Time);
				}
				phoneMessage = phoneMessage2;
				this.Messages.Add(phoneMessage);
				this.MessageCount++;
				goto IL_675;
			}
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000953 RID: 2387 RVA: 0x0004F3A0 File Offset: 0x0004D5A0
	// (set) Token: 0x06000954 RID: 2388 RVA: 0x0004F3A8 File Offset: 0x0004D5A8
	public CachedPlayerPref DatePref { get; private set; }

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000955 RID: 2389 RVA: 0x0004F3B4 File Offset: 0x0004D5B4
	// (set) Token: 0x06000956 RID: 2390 RVA: 0x0004F3BC File Offset: 0x0004D5BC
	public PhoneModel.PhonePathPref PathPref { get; private set; }

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000957 RID: 2391 RVA: 0x0004F3C8 File Offset: 0x0004D5C8
	// (set) Token: 0x06000958 RID: 2392 RVA: 0x0004F3D0 File Offset: 0x0004D5D0
	public List<PhoneModel.PhoneMessage> Messages { get; private set; }

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000959 RID: 2393 RVA: 0x0004F3DC File Offset: 0x0004D5DC
	// (set) Token: 0x0600095A RID: 2394 RVA: 0x0004F3E4 File Offset: 0x0004D5E4
	public short Id { get; private set; }

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x0600095B RID: 2395 RVA: 0x0004F3F0 File Offset: 0x0004D5F0
	// (set) Token: 0x0600095C RID: 2396 RVA: 0x0004F3F8 File Offset: 0x0004D5F8
	public string AssetBundle { get; private set; }

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x0600095D RID: 2397 RVA: 0x0004F404 File Offset: 0x0004D604
	// (set) Token: 0x0600095E RID: 2398 RVA: 0x0004F40C File Offset: 0x0004D60C
	public string Name { get; private set; }

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x0600095F RID: 2399 RVA: 0x0004F418 File Offset: 0x0004D618
	// (set) Token: 0x06000960 RID: 2400 RVA: 0x0004F420 File Offset: 0x0004D620
	public PhoneModel.PhoneMessage FirstMessage { get; private set; }

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000961 RID: 2401 RVA: 0x0004F42C File Offset: 0x0004D62C
	// (set) Token: 0x06000962 RID: 2402 RVA: 0x0004F434 File Offset: 0x0004D634
	public int MessageCount { get; private set; }

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000963 RID: 2403 RVA: 0x0004F440 File Offset: 0x0004D640
	// (set) Token: 0x06000964 RID: 2404 RVA: 0x0004F448 File Offset: 0x0004D648
	public int SpriteCount { get; private set; }

	// Token: 0x06000965 RID: 2405 RVA: 0x0004F454 File Offset: 0x0004D654
	private bool StartsWithSprite(string s)
	{
		return s.Length > 6 && s[0] == 'S' && s.StartsWith("Sprite");
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0004F48C File Offset: 0x0004D68C
	private bool StartsWithHidden(string s)
	{
		return s.Length > 6 && s[0] == 'H' && s.StartsWith("Hidden");
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0004F4C4 File Offset: 0x0004D6C4
	public void ConvertSave()
	{
		if (global::PlayerPrefs.HasKey("Cell" + this.Id.ToString() + "Date") && global::PlayerPrefs.HasKey("Cell" + this.Id.ToString() + "Path"))
		{
			long @long = new CachedPlayerPref(string.Format("Cell{0}Date", this.Id.ToString())).GetLong(0L);
			this.DatePref.SetLong(@long);
			string @string = new CachedPlayerPref(string.Format("Cell{0}Path", this.Id.ToString())).GetString(string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				this.PathPref.SetString(@string);
			}
			global::PlayerPrefs.DeleteKey("Cell" + this.Id.ToString() + "Date", false);
			global::PlayerPrefs.DeleteKey("Cell" + this.Id.ToString() + "Path", false);
			global::PlayerPrefs.Save();
		}
		else
		{
			this.PathPref.LoadState();
		}
	}

	// Token: 0x02000152 RID: 338
	public class PhonePathPref
	{
		// Token: 0x06000968 RID: 2408 RVA: 0x0004F5F0 File Offset: 0x0004D7F0
		public PhonePathPref(PhoneModel parent)
		{
			this.Parent = parent;
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0004F618 File Offset: 0x0004D818
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0004F620 File Offset: 0x0004D820
		public PhoneModel Parent { get; private set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0004F62C File Offset: 0x0004D82C
		public PhoneModel.PhonePathPref.PhonePrefs Pref
		{
			get
			{
				return this._prefs;
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0004F634 File Offset: 0x0004D834
		public void SetString(string s)
		{
			this._cache = string.Empty;
			if (string.IsNullOrEmpty(s))
			{
				return;
			}
			this._prefs = PhoneModel.PhonePathPref.PhonePrefs.FromOldSave(s);
			this.Save();
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0004F660 File Offset: 0x0004D860
		public string GetString(string defaultValue)
		{
			if (this._prefs != null && string.IsNullOrEmpty(this._cache))
			{
				return this._cache = this._prefs.ToOldSave(this.Parent);
			}
			if (this._prefs != null)
			{
				return this._cache;
			}
			return defaultValue;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0004F6B8 File Offset: 0x0004D8B8
		public void DeleteKey()
		{
			if (this._prefs != null)
			{
				global::PlayerPrefs.DeleteKey(string.Format("C{0}P", this.Parent.Id.ToString()), false);
				this._cache = string.Empty;
				this._prefs = new PhoneModel.PhonePathPref.PhonePrefs();
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0004F70C File Offset: 0x0004D90C
		private void Save()
		{
			if (this._prefs != null)
			{
				byte[] array = new byte[this._prefs.PlayerChoices.Length + 8];
				byte[] bytes = BitConverter.GetBytes(this._prefs.NumMessages);
				byte[] bytes2 = BitConverter.GetBytes(this._prefs.NumChoices);
				byte[] bytes3 = BitConverter.GetBytes(this._prefs.CurrentTime);
				array[0] = bytes[0];
				array[1] = bytes[1];
				array[2] = bytes2[0];
				array[3] = bytes2[1];
				array[4] = bytes3[0];
				array[5] = bytes3[1];
				array[6] = bytes3[2];
				array[7] = bytes3[3];
				for (int i = 0; i < this._prefs.PlayerChoices.Length; i++)
				{
					array[8 + i] = this._prefs.PlayerChoices[i];
				}
				global::PlayerPrefs.SetString(string.Format("C{0}P", this.Parent.Id.ToString()), Convert.ToBase64String(array));
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0004F7FC File Offset: 0x0004D9FC
		public void LoadState()
		{
			string @string = global::PlayerPrefs.GetString(string.Format("C{0}P", this.Parent.Id.ToString()), string.Empty);
			this._cache = string.Empty;
			if (!string.IsNullOrEmpty(@string))
			{
				byte[] array = Convert.FromBase64String(@string);
				this._prefs = new PhoneModel.PhonePathPref.PhonePrefs();
				this._prefs.NumMessages = BitConverter.ToUInt16(array, 0);
				this._prefs.NumChoices = BitConverter.ToUInt16(array, 2);
				this._prefs.CurrentTime = BitConverter.ToInt32(array, 4);
				this._prefs.PlayerChoices = new byte[array.Length - 8];
				for (int i = 0; i < this._prefs.PlayerChoices.Length; i++)
				{
					this._prefs.PlayerChoices[i] = array[8 + i];
				}
			}
			else
			{
				this._prefs = new PhoneModel.PhonePathPref.PhonePrefs();
			}
		}

		// Token: 0x0400096C RID: 2412
		private PhoneModel.PhonePathPref.PhonePrefs _prefs = new PhoneModel.PhonePathPref.PhonePrefs();

		// Token: 0x0400096D RID: 2413
		private string _cache = string.Empty;

		// Token: 0x02000153 RID: 339
		public class PhonePrefs
		{
			// Token: 0x17000136 RID: 310
			// (get) Token: 0x06000972 RID: 2418 RVA: 0x0004F8EC File Offset: 0x0004DAEC
			// (set) Token: 0x06000973 RID: 2419 RVA: 0x0004F8F4 File Offset: 0x0004DAF4
			public ushort NumMessages { get; set; }

			// Token: 0x17000137 RID: 311
			// (get) Token: 0x06000974 RID: 2420 RVA: 0x0004F900 File Offset: 0x0004DB00
			// (set) Token: 0x06000975 RID: 2421 RVA: 0x0004F908 File Offset: 0x0004DB08
			public ushort NumChoices { get; set; }

			// Token: 0x17000138 RID: 312
			// (get) Token: 0x06000976 RID: 2422 RVA: 0x0004F914 File Offset: 0x0004DB14
			// (set) Token: 0x06000977 RID: 2423 RVA: 0x0004F91C File Offset: 0x0004DB1C
			public byte[] PlayerChoices { get; set; }

			// Token: 0x17000139 RID: 313
			// (get) Token: 0x06000978 RID: 2424 RVA: 0x0004F928 File Offset: 0x0004DB28
			// (set) Token: 0x06000979 RID: 2425 RVA: 0x0004F930 File Offset: 0x0004DB30
			public int CurrentTime { get; set; }

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x0600097A RID: 2426 RVA: 0x0004F93C File Offset: 0x0004DB3C
			public int Size
			{
				get
				{
					return 4 + this.PlayerChoices.Length + 4;
				}
			}

			// Token: 0x0600097B RID: 2427 RVA: 0x0004F94C File Offset: 0x0004DB4C
			public static PhoneModel.PhonePathPref.PhonePrefs FromOldSave(string path)
			{
				PhoneModel.PhonePathPref.PhonePrefs phonePrefs = new PhoneModel.PhonePathPref.PhonePrefs();
				string[] array = path.Split(new char[]
				{
					'`'
				});
				int[] array2 = new int[array.Length];
				phonePrefs.CurrentTime = int.MaxValue;
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = int.Parse(array[i]);
					if (array2[i] < 0)
					{
						num2++;
						num++;
					}
					else if (array2[i] == 0 && i < array2.Length - 1)
					{
						num++;
					}
					else
					{
						phonePrefs.CurrentTime = array2[i];
					}
				}
				phonePrefs.NumChoices = (ushort)num2;
				phonePrefs.NumMessages = (ushort)num;
				phonePrefs.PlayerChoices = new byte[(int)Math.Ceiling((double)phonePrefs.NumChoices / 8.0)];
				int j = 0;
				int num3 = 0;
				int num4 = 0;
				while (j < array2.Length)
				{
					if (array2[j] < 0)
					{
						if (array2[j] == -2)
						{
							byte[] playerChoices = phonePrefs.PlayerChoices;
							int num5 = num4;
							playerChoices[num5] |= (byte)(1 << num3);
						}
						if (array2[j] == -4)
						{
							phonePrefs.CurrentTime = -4;
						}
						if (num3 == 7)
						{
							num4++;
							num3 = 0;
						}
						else
						{
							num3++;
						}
					}
					j++;
				}
				return phonePrefs;
			}

			// Token: 0x0600097C RID: 2428 RVA: 0x0004FAA0 File Offset: 0x0004DCA0
			public string ToOldSave(PhoneModel girl)
			{
				if (this.NumMessages == 0)
				{
					return string.Empty;
				}
				int[] array = new int[(int)this.NumMessages];
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				PhoneModel.PhoneMessage phoneMessage = girl.FirstMessage;
				for (int i = 0; i < (int)this.NumMessages; i++)
				{
					if (phoneMessage is PhoneModel.PhoneResponse && num3 < (int)this.NumChoices)
					{
						if ((this.PlayerChoices[num2] >> num & 1) == 1)
						{
							array[i] = -2;
						}
						else
						{
							array[i] = -1;
						}
						num3++;
						if (num == 7)
						{
							num2++;
							num = 0;
						}
						else
						{
							num++;
						}
					}
					else
					{
						array[i] = 0;
					}
					if (phoneMessage.NextMessage != null)
					{
						phoneMessage = phoneMessage.NextMessage;
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				int num4 = array.Length - 1;
				if (this.CurrentTime == -4)
				{
					num4--;
				}
				for (int j = 0; j < num4; j++)
				{
					stringBuilder.Append(string.Format("{0}`", array[j].ToString()));
				}
				if (array.Length > 0)
				{
					stringBuilder.Append(array[array.Length - 1].ToString());
				}
				if (this.CurrentTime != 2147483647)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(string.Format("`{0}", this.CurrentTime.ToString()));
					}
					else
					{
						stringBuilder.Append(this.CurrentTime.ToString());
					}
				}
				return stringBuilder.ToString();
			}
		}
	}

	// Token: 0x02000154 RID: 340
	public class PhoneMessage
	{
		// Token: 0x0600097E RID: 2430 RVA: 0x0004FC4C File Offset: 0x0004DE4C
		public bool IsPhoneCall(PhoneModel girl)
		{
			int num = (int)(girl.PathPref.Pref.NumMessages + 1);
			return (girl.Id == 16 && num >= 168 && num <= 237) || (girl.Id == 13 && num >= 149 && num <= 260);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0004FCB8 File Offset: 0x0004DEB8
		public void PlayAudio(PhoneModel girl, string column)
		{
			if (this.IsPhoneCall(girl))
			{
				Balance.GirlName girlName = (girl.Id != 16) ? Balance.GirlName.NovaPF : Balance.GirlName.ReneePF;
				string arg = girlName.ToLowerFriendlyString();
				bool flag = GameState.Voiceover.Play(girlName, Voiceover.BundleType.Special, string.Format("{0}_{1}_{2}", arg, this.Id.ToString(), column), 0f);
				if (!flag)
				{
					if (column == "D")
					{
						flag = GameState.Voiceover.Play(girlName, Voiceover.BundleType.Special, string.Format("{0}_{1}_{2}", arg, this.Id.ToString(), "I"), 0f);
					}
					else if (column == "E")
					{
						flag = GameState.Voiceover.Play(girlName, Voiceover.BundleType.Special, string.Format("{0}_{1}_{2}", arg, this.Id.ToString(), "J"), 0f);
					}
				}
				if (!flag)
				{
					Debug.LogError("Could not find clip " + this.Id.ToString() + " column " + column);
				}
				else if (this.NextMessage != null)
				{
					this.NextMessage.Time = GameState.Voiceover.GetClipDuration() + 1f;
				}
			}
		}

		// Token: 0x1700013B RID: 315
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x0004FDF4 File Offset: 0x0004DFF4
		public string Message
		{
			set
			{
				this._message = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0004FE00 File Offset: 0x0004E000
		public string MessageNSFW
		{
			set
			{
				this._messageNSFW = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x0004FE0C File Offset: 0x0004E00C
		public string AlternateMessage
		{
			set
			{
				this._alternateMessage = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x0004FE18 File Offset: 0x0004E018
		public string AlternateMessageNSFW
		{
			set
			{
				this._alternateMessageNsfw = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0004FE24 File Offset: 0x0004E024
		public bool HasAlternateMessage
		{
			get
			{
				return !string.IsNullOrEmpty(this._alternateMessage);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0004FE34 File Offset: 0x0004E034
		public bool HasMessage
		{
			get
			{
				return !string.IsNullOrEmpty(this._message);
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0004FE44 File Offset: 0x0004E044
		private string GetEnglishMessage()
		{
			if (GameState.NSFW && !string.IsNullOrEmpty(this._messageNSFW))
			{
				return this._messageNSFW;
			}
			return this._message;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0004FE70 File Offset: 0x0004E070
		public string GetTranslatedAlternateMessage(PhoneModel girl)
		{
			this.PlayAudio(girl, (!GameState.NSFW || string.IsNullOrEmpty(this._alternateMessageNsfw)) ? (string.IsNullOrEmpty(this._alternateMessage) ? ((!GameState.NSFW || string.IsNullOrEmpty(this._messageNSFW)) ? "D" : "E") : "K") : "L");
			if (Translations.PreferredLanguage == Translations.Language.English)
			{
				return (!GameState.NSFW || string.IsNullOrEmpty(this._alternateMessageNsfw)) ? this._alternateMessage : this._alternateMessageNsfw;
			}
			string str = string.Format("{0}_{1}_", PhoneModel.PhoneMessage.GetTranslationId(girl.Name, girl.Id), this.Id.ToString());
			string text = Translations.TryGetTranslation(str + "8");
			if (!string.IsNullOrEmpty(text))
			{
				if (GameState.NSFW)
				{
					string text2 = Translations.TryGetTranslation(str + "9");
					if (!string.IsNullOrEmpty(text2))
					{
						return text2;
					}
				}
				return text;
			}
			return (!GameState.NSFW || string.IsNullOrEmpty(this._alternateMessageNsfw)) ? this._alternateMessage : this._alternateMessageNsfw;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0004FFBC File Offset: 0x0004E1BC
		public static string GetTranslationId(string girlName, short id)
		{
			switch (id)
			{
			case 8:
				return "Desiree";
			case 16:
				return "Renee";
			case 21:
				return "TheDarkOne";
			case 22:
				return "QPernikiss_PF";
			case 23:
				return "Cassie_PF";
			case 24:
				return "Mio_PF";
			case 25:
				return "Quill_PF";
			case 26:
				return "Elle_PF";
			case 27:
				return "Iro_PF";
			case 28:
				return "Bonnibel_PF";
			case 29:
				return "Fumi_PF";
			case 30:
				return "Bearverly_PF";
			case 31:
				return "Nina_PF";
			case 32:
				return "Alpha_PF";
			}
			return girlName;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00050094 File Offset: 0x0004E294
		public string GetTranslatedMessage(PhoneModel girl)
		{
			if (this.Parent != null)
			{
				return this.GetTranslatedResponse(girl, this.Parent);
			}
			this.PlayAudio(girl, (!GameState.NSFW || string.IsNullOrEmpty(this._messageNSFW)) ? "D" : "E");
			if (Translations.PreferredLanguage == Translations.Language.English)
			{
				return this.GetEnglishMessage();
			}
			string str = string.Format("{0}_{1}_", PhoneModel.PhoneMessage.GetTranslationId(girl.Name, girl.Id), this.Id.ToString());
			string text = Translations.TryGetTranslation(str + ((!this.HasAlternateMessage) ? "2" : "6"));
			if (!string.IsNullOrEmpty(text))
			{
				if (GameState.NSFW)
				{
					string text2 = Translations.TryGetTranslation(str + ((!this.HasAlternateMessage) ? "3" : "7"));
					if (!string.IsNullOrEmpty(text2))
					{
						return text2;
					}
				}
				return text;
			}
			return this.GetEnglishMessage();
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00050198 File Offset: 0x0004E398
		private string GetTranslatedResponse(PhoneModel girl, PhoneModel.PhoneResponse response)
		{
			if (Translations.PreferredLanguage == Translations.Language.English)
			{
				return this.GetEnglishMessage();
			}
			string str = string.Format("{0}_{1}_", PhoneModel.PhoneMessage.GetTranslationId(girl.Name, girl.Id), response.Id.ToString());
			bool flag = this == response.Response2;
			string str2 = (!flag) ? "1" : "5";
			string str3 = (!flag) ? "0" : "4";
			if (GameState.NSFW)
			{
				string text = Translations.TryGetTranslation(str + str2);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			string text2 = Translations.TryGetTranslation(str + str3);
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			return this.GetEnglishMessage();
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00050260 File Offset: 0x0004E460
		public override string ToString()
		{
			return string.Format("{0}:{1}", this.Id.ToString(), this._message);
		}

		// Token: 0x04000973 RID: 2419
		public float Time;

		// Token: 0x04000974 RID: 2420
		public short Id;

		// Token: 0x04000975 RID: 2421
		private string _message;

		// Token: 0x04000976 RID: 2422
		private string _messageNSFW;

		// Token: 0x04000977 RID: 2423
		private string _alternateMessage;

		// Token: 0x04000978 RID: 2424
		private string _alternateMessageNsfw;

		// Token: 0x04000979 RID: 2425
		public PhoneModel.PhoneResponse Parent;

		// Token: 0x0400097A RID: 2426
		public PhoneModel.PhoneMessage NextMessage;

		// Token: 0x0400097B RID: 2427
		public bool NewConversation;

		// Token: 0x0400097C RID: 2428
		public int ConversationId;
	}

	// Token: 0x02000155 RID: 341
	public class PhoneResponse : PhoneModel.PhoneMessage
	{
		// Token: 0x0400097D RID: 2429
		public PhoneModel.PhoneMessage Response1;

		// Token: 0x0400097E RID: 2430
		public PhoneModel.PhoneMessage Response2;
	}

	// Token: 0x02000156 RID: 342
	public class PhoneImage : PhoneModel.PhoneMessage
	{
		// Token: 0x0400097F RID: 2431
		public string SpriteName1;

		// Token: 0x04000980 RID: 2432
		public string SpriteName2;

		// Token: 0x04000981 RID: 2433
		public bool FromPlayer;

		// Token: 0x04000982 RID: 2434
		public bool InGallery = true;
	}
}
