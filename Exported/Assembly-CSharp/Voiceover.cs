using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetBundles;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class Voiceover : MonoBehaviour
{
	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000A70 RID: 2672 RVA: 0x00054580 File Offset: 0x00052780
	private bool HasVoiceBundle
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00054584 File Offset: 0x00052784
	private bool HasVolume
	{
		get
		{
			return Settings.VoiceVolume > 0f;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00054594 File Offset: 0x00052794
	public bool IsVoiceEnabled
	{
		get
		{
			return this.HasVolume;
		}
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0005459C File Offset: 0x0005279C
	private void Reset()
	{
		this._lastLoadedGirlBundle = string.Empty;
		if (this.VoiceoverSource != null)
		{
			this.VoiceoverSource.Stop();
			this.VoiceoverSource.clip = null;
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x000545D4 File Offset: 0x000527D4
	public void Init(bool cacheBundles = true)
	{
		if (this.IsVoiceEnabled == this._cachedReadyValue)
		{
			return;
		}
		if (this.IsVoiceEnabled)
		{
			this._cachedReadyValue = true;
			if (cacheBundles)
			{
				this.CacheBundles();
			}
			GameState.CurrentState.StartCoroutine(this.LoadTutorialBundle());
			if (Girls.CurrentGirl != null)
			{
				if (GameState.GetGirlScreen().InteractionBox.LevelUp)
				{
					GameState.GetGirlScreen().InteractionBox.LevelUp = false;
				}
				this.LoadBundle(Girls.CurrentGirl.GirlName);
			}
		}
		else
		{
			this.Reset();
			this.UnloadAll();
			this._cachedReadyValue = false;
		}
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00054680 File Offset: 0x00052880
	private void CacheBundle(string bundleName)
	{
		if (!GameState.AssetManager.IsCached(bundleName) && this.HasVolume)
		{
			GameState.AssetManager.GetBundle(bundleName, false, delegate(AssetBundle bundle)
			{
				if (Girls.CurrentGirl == null || bundleName != this.GetBundleName(Girls.CurrentGirl.GirlName))
				{
					GameState.AssetManager.UnloadBundle(bundle);
				}
			});
		}
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x000546E0 File Offset: 0x000528E0
	public void CacheBundles()
	{
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x000546F0 File Offset: 0x000528F0
	private void CacheDLCBundles()
	{
		if (!this.HasVolume)
		{
			return;
		}
		foreach (Balance.GirlName girl in Voiceover.MobileDlcGirlList)
		{
			if (GameState.GetGirlScreen().IsUnlocked(girl))
			{
				this.CacheBundle(this.GetBundleName(girl));
			}
		}
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00054744 File Offset: 0x00052944
	private IEnumerator LoadTutorialBundle()
	{
		if (!this.HasVoiceBundle || this._qpiddyBundle != null)
		{
			yield break;
		}
		string bundleName = this.GetBundleName(Balance.GirlName.QPiddy) + "_intro";
		AssetBundleAsync request = GameState.AssetManager.GetBundleAsync(bundleName, false);
		yield return request;
		this._qpiddyBundle = request.AssetBundle;
		this.PlayQueuedId(Balance.GirlName.QPiddy, Voiceover.BundleType.Tutorial);
		yield break;
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00054760 File Offset: 0x00052960
	public IEnumerator LoadSpecialBundle(Balance.GirlName girlName, bool force = false)
	{
		this._otherQueuedData = default(Voiceover.VoiceQueuedData);
		this.SetDlcGirlActive(girlName);
		if (!this.IsVoiceEnabled && !force)
		{
			yield break;
		}
		string bundleName = this.GetBundleName(girlName);
		if (this._girlBundle != null && this._lastLoadedGirlBundle == this.GetBundleName(girlName))
		{
			this._specialBundle = this._girlBundle;
			this.SetCurrentBundle(Voiceover.BundleType.Special);
		}
		else
		{
			AssetBundleAsync request = GameState.AssetManager.GetBundleAsync(bundleName, false);
			yield return request;
			this._specialBundle = request.AssetBundle;
			this.SetCurrentBundle(Voiceover.BundleType.Special);
			this.PlayQueuedId(girlName, Voiceover.BundleType.Special);
		}
		yield break;
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00054798 File Offset: 0x00052998
	public void LoadBundle(Balance.GirlName girlName)
	{
		if (this.LoadGirlCoroutine != null)
		{
			GameState.CurrentState.StopCoroutine(this.LoadGirlCoroutine);
		}
		this.LoadGirlCoroutine = null;
		this.SetDlcGirlActive(girlName);
		if (!this.IsVoiceEnabled)
		{
			this.Stop();
			this.UnloadBundle(ref this._girlBundle);
			return;
		}
		string bundleName = this.GetBundleName(girlName);
		if (this._lastLoadedGirlBundle == bundleName && this._girlBundle != null)
		{
			return;
		}
		this.UnloadBundle(ref this._girlBundle);
		this.LoadGirlCoroutine = GameState.CurrentState.StartCoroutine(this.LoadBundleInternal(girlName, bundleName));
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0005483C File Offset: 0x00052A3C
	private IEnumerator LoadBundleInternal(Balance.GirlName girlName, string bundleName)
	{
		this._lastPlayedGirl = Balance.GirlName.Unknown;
		this._girlQueuedData = default(Voiceover.VoiceQueuedData);
		AssetBundleAsync request = GameState.AssetManager.GetBundleAsync(bundleName, false);
		yield return request;
		this._girlBundle = request.AssetBundle;
		if (this._girlBundle != null)
		{
			this._lastLoadedGirlBundle = bundleName;
			this.PlayQueuedId(girlName, Voiceover.BundleType.Girl);
		}
		this.LoadGirlCoroutine = null;
		yield break;
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00054874 File Offset: 0x00052A74
	public void UnloadBundle(ref AssetBundle bundle)
	{
		if (bundle != null)
		{
			GameState.AssetManager.UnloadBundle(bundle, true);
			bundle = null;
			this._lastLoadedGirlBundle = string.Empty;
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x000548AC File Offset: 0x00052AAC
	public void UnloadSpecialBundle(Balance.GirlName girlName, bool forceUnload = false)
	{
		if (girlName != Balance.GirlName.Cassie)
		{
			this.Stop();
		}
		if (Girls.CurrentGirl != null)
		{
			this.SetDlcGirlActive(Girls.CurrentGirl.GirlName);
		}
		if (!forceUnload && Girls.CurrentGirl != null && Girls.CurrentGirl.GirlName == girlName && this._lastLoadedGirlBundle == this.GetBundleName(girlName))
		{
			this._girlBundle = this._specialBundle;
			this._specialBundle = null;
		}
		else
		{
			this.UnloadBundle(ref this._specialBundle);
		}
		this._otherQueuedData = default(Voiceover.VoiceQueuedData);
		this.SetCurrentBundle(Voiceover.BundleType.Girl);
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0005495C File Offset: 0x00052B5C
	public void UnloadAll()
	{
		this.UnloadBundle(ref this._qpiddyBundle);
		this.UnloadBundle(ref this._girlBundle);
		this.UnloadBundle(ref this._specialBundle);
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x00054990 File Offset: 0x00052B90
	private void PlayQueuedId()
	{
		if (this._otherQueuedData.IsValid())
		{
			this.PlayQueuedId(this._otherQueuedData.GirlName, this._otherQueuedData.BundleType);
		}
		else if (this._girlQueuedData.IsValid())
		{
			this.PlayQueuedId(this._girlQueuedData.GirlName, this._girlQueuedData.BundleType);
		}
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x000549FC File Offset: 0x00052BFC
	private void PlayQueuedId(Balance.GirlName girlName, Voiceover.BundleType bundleType)
	{
		if (this._currentBundleType != bundleType || (!this._otherQueuedData.IsValid() && !this._girlQueuedData.IsValid()))
		{
			return;
		}
		string text = null;
		if (this._currentBundleType != Voiceover.BundleType.Girl && this._otherQueuedData.IsValid())
		{
			text = this._otherQueuedData.QueuedId;
			this._otherQueuedData = default(Voiceover.VoiceQueuedData);
		}
		else if (this._currentBundleType == Voiceover.BundleType.Girl && this._girlQueuedData.IsValid())
		{
			text = this._girlQueuedData.QueuedId;
		}
		if (!string.IsNullOrEmpty(text))
		{
			this.Play(girlName, bundleType, text, 0.25f);
		}
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00054AB4 File Offset: 0x00052CB4
	private bool SetAudioClip(Balance.GirlName girlName, Voiceover.BundleType bundleType, string id)
	{
		if (this.VoiceoverSource == null || (!this._cellphoneActive && !this.HasVolume))
		{
			return false;
		}
		if (this._lastPlayedGirl == girlName && this._lastPlayedId == id)
		{
			return false;
		}
		string parsedVoiceoverID = this.GetParsedVoiceoverID(id);
		bool flag = bundleType == Voiceover.BundleType.Girl && this._otherQueuedData.IsValid();
		AssetBundle currentBundle = this.GetCurrentBundle();
		if (this.PlaybackBlockers.Count == 0 && !flag && currentBundle != null)
		{
			AudioClip audioClip = currentBundle.LoadAsset<AudioClip>(parsedVoiceoverID);
			if (audioClip != null)
			{
				this._lastPlayedGirl = girlName;
				this._lastPlayedId = id;
				this.VoiceoverSource.clip = audioClip;
				return true;
			}
		}
		else
		{
			this._lastPlayedGirl = Balance.GirlName.Unknown;
			this._lastPlayedId = string.Empty;
		}
		if (flag || bundleType == Voiceover.BundleType.Girl)
		{
			this._girlQueuedData = new Voiceover.VoiceQueuedData(id, girlName, bundleType);
		}
		else if (bundleType != Voiceover.BundleType.Girl)
		{
			this._otherQueuedData = new Voiceover.VoiceQueuedData(id, girlName, this._currentBundleType);
		}
		return false;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x00054BD0 File Offset: 0x00052DD0
	public void PlayWithDelay(float delay)
	{
		if (this.PlayCoroutine != null)
		{
			GameState.CurrentState.StopCoroutine(this.PlayCoroutine);
		}
		this.PlayCoroutine = GameState.CurrentState.StartCoroutine(this.InternalPlayWithDelay(delay));
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x00054C10 File Offset: 0x00052E10
	private IEnumerator InternalPlayWithDelay(float delay)
	{
		this.VoiceoverSource.Stop();
		yield return new WaitForSeconds(delay);
		this.VoiceoverSource.Play();
		this.PlayCoroutine = null;
		if (this._currentBundleType == Voiceover.BundleType.Girl)
		{
			this._girlQueuedData = default(Voiceover.VoiceQueuedData);
		}
		yield break;
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00054C3C File Offset: 0x00052E3C
	public bool Play(Balance.GirlName girlName, Voiceover.BundleType bundleType, string id, float delay = 0f)
	{
		if (this.VoiceoverSource == null || (!this._cellphoneActive && !this._dlcGirlActive && (!this.HasVolume || !this._cachedReadyValue)))
		{
			return false;
		}
		if (this.SetAudioClip(girlName, bundleType, id))
		{
			this.AdjustVolume(girlName);
			if (delay > 0f)
			{
				this.PlayWithDelay(delay);
			}
			else
			{
				this.VoiceoverSource.Play();
				if (bundleType == Voiceover.BundleType.Girl)
				{
					this._girlQueuedData = default(Voiceover.VoiceQueuedData);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00054CDC File Offset: 0x00052EDC
	public void Stop(Voiceover.BundleType bundleType)
	{
		if (bundleType == this._currentBundleType)
		{
			this.Stop();
		}
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00054CF0 File Offset: 0x00052EF0
	public void Stop()
	{
		if (this.PlayCoroutine != null)
		{
			GameState.CurrentState.StopCoroutine(this.PlayCoroutine);
		}
		this.PlayCoroutine = null;
		if (this.VoiceoverSource != null)
		{
			this.VoiceoverSource.Stop();
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x00054D3C File Offset: 0x00052F3C
	private void AdjustVolume(Balance.GirlName girlName)
	{
		float num;
		if (Voiceover.GirlToVolumeDic.TryGetValue(girlName, out num))
		{
			this.VoiceoverSource.volume = Settings.VoiceVolume * num;
		}
		else
		{
			this.VoiceoverSource.volume = Settings.VoiceVolume;
		}
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x00054D84 File Offset: 0x00052F84
	public void AddPlaybackBlocker(GameObject gameObject)
	{
		this.PlaybackBlockers.Add(gameObject);
		this.Stop();
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00054D98 File Offset: 0x00052F98
	public void RemovePlaybackBlocker(GameObject gameObject, bool playQueued = true)
	{
		this.PlaybackBlockers.Remove(gameObject);
		if (this.PlaybackBlockers.Count == 0 && playQueued)
		{
			this.PlayQueuedId();
		}
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00054DC4 File Offset: 0x00052FC4
	private void SetDlcGirlActive(Balance.GirlName girlName)
	{
		this._dlcGirlActive = Voiceover.MobileDlcGirlList.Contains(girlName);
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00054DD8 File Offset: 0x00052FD8
	public void SetCellPhoneActive(bool active)
	{
		this._cellphoneActive = active;
		if (!active && this._specialBundle != null)
		{
			this.UnloadSpecialBundle(Balance.GirlName.NovaPF, true);
		}
		else
		{
			this.Stop();
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00054E10 File Offset: 0x00053010
	public void SetCurrentBundle(Voiceover.BundleType bundleType)
	{
		this._currentBundleType = bundleType;
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00054E1C File Offset: 0x0005301C
	public string GetBundleName(string girlName)
	{
		string text = girlName + "/audio";
		if (girlName == Balance.GirlName.Bearverly.ToLowerFriendlyString())
		{
			text += ((!(Girl.FindGirl(Balance.GirlName.Bearverly) == null) && Girl.FindGirl(Balance.GirlName.Bearverly).Love < 9) ? "_bear" : "_human");
		}
		return text;
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x00054E84 File Offset: 0x00053084
	public string GetBundleName(Balance.GirlName girl)
	{
		return this.GetBundleName(girl.ToLowerFriendlyString());
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x00054E94 File Offset: 0x00053094
	private AssetBundle GetCurrentBundle()
	{
		switch (this._currentBundleType)
		{
		case Voiceover.BundleType.Girl:
			return this._girlBundle;
		case Voiceover.BundleType.Tutorial:
			return this._qpiddyBundle;
		case Voiceover.BundleType.Special:
			return this._specialBundle;
		default:
			return null;
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00054ED8 File Offset: 0x000530D8
	private string GetParsedVoiceoverID(string rawId)
	{
		if (!rawId.Contains("_"))
		{
			return null;
		}
		string text = rawId;
		string text2 = text.Substring(0, text.IndexOf("_"));
		int num;
		if (text == "iro_108" && Girls.CurrentGirl != null && Girls.CurrentGirl.GirlName == Balance.GirlName.Cassie)
		{
			text = text.Replace("iro", "cassie");
		}
		else if (text.StartsWith("darkone"))
		{
			text = text.Replace("darkone", "darkmio");
		}
		else if (text.Contains("bearverly"))
		{
			text2 = ((Girl.FindGirl(Balance.GirlName.Bearverly).Love < 9) ? "bearverly-bear" : "bearverly-human");
		}
		else if (text.Contains("bearverly") && int.TryParse(text.Substring(text2.Length + 1), out num) && num > 100)
		{
			text2 = "bearverly-human";
		}
		if (Voiceover._numberList.Contains(text2) || Voiceover._phoneCallList.Contains(text2))
		{
			text = text.Substring(text2.Length + 1);
		}
		else if (text.StartsWith("wendy_"))
		{
			text = text.Substring(6);
			if (text.Length == 1)
			{
				text = "00" + text;
			}
			else if (text.Length == 2)
			{
				text = "0" + text;
			}
		}
		return text;
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0005506C File Offset: 0x0005326C
	public float GetClipDuration()
	{
		if (this.VoiceoverSource == null || this.VoiceoverSource.clip == null)
		{
			return 0f;
		}
		return this.VoiceoverSource.clip.length;
	}

	// Token: 0x040009DE RID: 2526
	private const float QueuedDelay = 0.25f;

	// Token: 0x040009DF RID: 2527
	public AudioSource VoiceoverSource;

	// Token: 0x040009E0 RID: 2528
	private AssetBundle _qpiddyBundle;

	// Token: 0x040009E1 RID: 2529
	private AssetBundle _girlBundle;

	// Token: 0x040009E2 RID: 2530
	private AssetBundle _specialBundle;

	// Token: 0x040009E3 RID: 2531
	private string _lastLoadedGirlBundle;

	// Token: 0x040009E4 RID: 2532
	private Voiceover.VoiceQueuedData _girlQueuedData;

	// Token: 0x040009E5 RID: 2533
	private Voiceover.VoiceQueuedData _otherQueuedData;

	// Token: 0x040009E6 RID: 2534
	private Voiceover.BundleType _currentBundleType;

	// Token: 0x040009E7 RID: 2535
	private bool _cellphoneActive;

	// Token: 0x040009E8 RID: 2536
	private bool _dlcGirlActive;

	// Token: 0x040009E9 RID: 2537
	private bool _cachedReadyValue;

	// Token: 0x040009EA RID: 2538
	private Coroutine LoadGirlCoroutine;

	// Token: 0x040009EB RID: 2539
	private Coroutine PlayCoroutine;

	// Token: 0x040009EC RID: 2540
	private List<GameObject> PlaybackBlockers = new List<GameObject>();

	// Token: 0x040009ED RID: 2541
	private Balance.GirlName _lastPlayedGirl = Balance.GirlName.Unknown;

	// Token: 0x040009EE RID: 2542
	private string _lastPlayedId = string.Empty;

	// Token: 0x040009EF RID: 2543
	private static Dictionary<Balance.GirlName, float> GirlToVolumeDic = new Dictionary<Balance.GirlName, float>
	{
		{
			Balance.GirlName.Quillzone,
			0.25f
		},
		{
			Balance.GirlName.Bonchovy,
			0.25f
		},
		{
			Balance.GirlName.Ruri,
			0.25f
		},
		{
			Balance.GirlName.Generica,
			0.25f
		},
		{
			Balance.GirlName.Odango,
			0.25f
		},
		{
			Balance.GirlName.Shibuki,
			0.25f
		},
		{
			Balance.GirlName.Sirina,
			0.25f
		},
		{
			Balance.GirlName.Rosa,
			0.25f
		},
		{
			Balance.GirlName.Jelle,
			0.33f
		},
		{
			Balance.GirlName.Spectrum,
			0.33f
		},
		{
			Balance.GirlName.Tessa,
			0.33f
		},
		{
			Balance.GirlName.Juliet,
			0.33f
		},
		{
			Balance.GirlName.Wendy,
			0.33f
		},
		{
			Balance.GirlName.NovaPF,
			0.33f
		},
		{
			Balance.GirlName.ReneePF,
			0.5f
		},
		{
			Balance.GirlName.Suzu,
			0.33f
		},
		{
			Balance.GirlName.Catara,
			0.5f
		},
		{
			Balance.GirlName.Peanut,
			0.5f
		},
		{
			Balance.GirlName.Roxxy,
			0.5f
		},
		{
			Balance.GirlName.Renee,
			1f
		},
		{
			Balance.GirlName.Darya,
			0.5f
		},
		{
			Balance.GirlName.Lustat,
			0.5f
		},
		{
			Balance.GirlName.Sawyer,
			0.5f
		},
		{
			Balance.GirlName.Explora,
			0.5f
		},
		{
			Balance.GirlName.Esper,
			0.5f
		},
		{
			Balance.GirlName.Mallory,
			0.8f
		},
		{
			Balance.GirlName.Lake,
			0.5f
		}
	};

	// Token: 0x040009F0 RID: 2544
	private static string[] _numberList = new string[]
	{
		"catara",
		"claudia",
		"juliet",
		"peanut",
		"rosa",
		"roxxy",
		"ruri",
		"shibuki",
		"sirina",
		"tessa",
		"vellatrix",
		"jelle",
		"quillzone",
		"bonchovy",
		"spectrum",
		"generica",
		"suzu",
		"lustat",
		"sawyer",
		"explora",
		"esper",
		"mallory",
		"lake"
	};

	// Token: 0x040009F1 RID: 2545
	private static string[] _phoneCallList = new string[]
	{
		"renee",
		"nova"
	};

	// Token: 0x040009F2 RID: 2546
	public static Balance.GirlName[] MobileDlcGirlList = new Balance.GirlName[]
	{
		Balance.GirlName.Catara,
		Balance.GirlName.Charlotte,
		Balance.GirlName.Darya,
		Balance.GirlName.Suzu,
		Balance.GirlName.Mallory
	};

	// Token: 0x0200016B RID: 363
	public enum BundleType
	{
		// Token: 0x040009F4 RID: 2548
		Girl,
		// Token: 0x040009F5 RID: 2549
		Tutorial,
		// Token: 0x040009F6 RID: 2550
		Special
	}

	// Token: 0x0200016C RID: 364
	[Serializable]
	public struct VoiceQueuedData
	{
		// Token: 0x06000A92 RID: 2706 RVA: 0x000550B8 File Offset: 0x000532B8
		public VoiceQueuedData(string queuedId, Balance.GirlName girlName, Voiceover.BundleType bundleType)
		{
			this.QueuedId = queuedId;
			this.GirlName = girlName;
			this.BundleType = bundleType;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x000550D0 File Offset: 0x000532D0
		public bool IsValid()
		{
			return !string.IsNullOrEmpty(this.QueuedId);
		}

		// Token: 0x040009F7 RID: 2551
		public string QueuedId;

		// Token: 0x040009F8 RID: 2552
		public Balance.GirlName GirlName;

		// Token: 0x040009F9 RID: 2553
		public Voiceover.BundleType BundleType;
	}
}
