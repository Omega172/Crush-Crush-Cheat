using System;
using System.Text;
using BlayFap;
using Steamworks;
using UnityEngine;

// Token: 0x02000100 RID: 256
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0002DFEC File Offset: 0x0002C1EC
	public static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0002E014 File Offset: 0x0002C214
	public static void InitializeSteam(Action<string> OnError)
	{
		try
		{
			SteamManager x = GameObject.Find("Steam").AddComponent<SteamManager>();
			if (!SteamManager.Initialized)
			{
				OnError("Error: Could not connect to Steam API");
			}
			else if (!SteamUser.BLoggedOn())
			{
				OnError("Warning: Playing in offline mode");
				BlayFapClient.Instance.SetupBlayFap();
			}
			else if (x != null && SteamManager.Initialized && !BlayFapClient.LoggedIn)
			{
				Debug.Log("Steam initialized");
				BlayFapClient.Instance.SetupBlayFap();
			}
		}
		catch (Exception ex)
		{
			OnError("Error: " + ex.Message);
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0002E0E0 File Offset: 0x0002C2E0
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002E0EC File Offset: 0x0002C2EC
	public void SaveToCloudSave()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		byte[] bytes = Encoding.ASCII.GetBytes(global::PlayerPrefs.Export());
		Debug.Log(string.Format("Result of SaveToCloudSave: {0}", SteamRemoteStorage.FileWrite(SteamManager.CloudSaveFileName, bytes, bytes.Length).ToString()));
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0002E13C File Offset: 0x0002C33C
	public string ReadFromCloudSave()
	{
		if (!SteamManager.Initialized)
		{
			return null;
		}
		int fileSize = SteamRemoteStorage.GetFileSize(SteamManager.CloudSaveFileName);
		byte[] array = new byte[fileSize];
		SteamRemoteStorage.FileRead(SteamManager.CloudSaveFileName, array, fileSize);
		return Encoding.ASCII.GetString(array);
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0002E180 File Offset: 0x0002C380
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0002E188 File Offset: 0x0002C388
	private void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInialized = true;
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0002E284 File Offset: 0x0002C484
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0002E2DC File Offset: 0x0002C4DC
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0002E314 File Offset: 0x0002C514
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0002E328 File Offset: 0x0002C528
	public ulong GetSteamId()
	{
		return SteamUser.GetSteamID().m_SteamID;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0002E344 File Offset: 0x0002C544
	public void GetSteamToken(Action<string> onSuccess, Action<EResult> onError)
	{
		if (this.getAuthSessionTicketResponseCallback != null)
		{
			this.getAuthSessionTicketResponseCallback.Unregister();
		}
		byte[] ticket = new byte[1024];
		uint ticketLength;
		HAuthTicket authSessionTicket = SteamUser.GetAuthSessionTicket(ticket, 1024, out ticketLength);
		if (authSessionTicket == HAuthTicket.Invalid)
		{
			Debug.LogErrorFormat("Error on GetAuthSessionTicket: [{0}]", new object[]
			{
				EResult.k_EResultUnexpectedError
			});
			onError(EResult.k_EResultUnexpectedError);
		}
		else
		{
			this.getAuthSessionTicketResponseCallback = new Callback<GetAuthSessionTicketResponse_t>(delegate(GetAuthSessionTicketResponse_t result)
			{
				this.getAuthSessionTicketResponseCallback.Unregister();
				if (result.m_eResult == EResult.k_EResultOK)
				{
					onSuccess(BitConverter.ToString(ticket, 0, (int)ticketLength).Replace("-", string.Empty));
				}
				else
				{
					onError(result.m_eResult);
				}
			}, false);
		}
	}

	// Token: 0x040005A7 RID: 1447
	private static SteamManager s_instance;

	// Token: 0x040005A8 RID: 1448
	private static bool s_EverInialized;

	// Token: 0x040005A9 RID: 1449
	private bool m_bInitialized;

	// Token: 0x040005AA RID: 1450
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	// Token: 0x040005AB RID: 1451
	private Callback<GetAuthSessionTicketResponse_t> getAuthSessionTicketResponseCallback;

	// Token: 0x040005AC RID: 1452
	public static readonly string CloudSaveFileName = "crushcrush.sav";
}
