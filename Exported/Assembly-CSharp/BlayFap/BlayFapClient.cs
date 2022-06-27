using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AssetBundles;
using BlayFapShared;
using SadPanda.Platforms;
using SimpleJSON;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;

namespace BlayFap
{
	// Token: 0x02000034 RID: 52
	public class BlayFapClient : MonoBehaviour
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00009F44 File Offset: 0x00008144
		public static BlayFapClient Instance
		{
			get
			{
				if (BlayFapClient.instance == null)
				{
					BlayFapClient.instance = BlayFapClient.CreateInstance();
				}
				return BlayFapClient.instance;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00009F68 File Offset: 0x00008168
		private static BlayFapClient CreateInstance()
		{
			GameObject gameObject = new GameObject("BlayFapClient");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<BlayFapClient>();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00009F8C File Offset: 0x0000818C
		private void Awake()
		{
			if (BlayFapClient.instance != null)
			{
				UnityEngine.Object.DestroyImmediate(base.gameObject);
				return;
			}
			Application.runInBackground = true;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00009FBC File Offset: 0x000081BC
		private void OnDestroy()
		{
			if (BlayFapClient.instance == this)
			{
				BlayFapClient.instance = null;
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00009FD4 File Offset: 0x000081D4
		public void LoginWithCustomId(LoginWithCustomIDRequest request, Action<LoginResponse> responseCallback)
		{
			this.Login("login/LoginWithCustomId", JsonUtility.ToJson(request), responseCallback);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009FE8 File Offset: 0x000081E8
		public void LoginWithSteam(LoginWithSteamRequest request, Action<LoginResponse> responseCallback)
		{
			this.Login("login/LoginWithSteam", JsonUtility.ToJson(request), responseCallback);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009FFC File Offset: 0x000081FC
		public void LoginWithAndroid(LoginWithAndroidRequest request, Action<LoginResponse> responseCallback)
		{
			this.Login("login/LoginWithAndroid", JsonUtility.ToJson(request), responseCallback);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000A010 File Offset: 0x00008210
		public void LoginWithGooglePlay(LoginWithGooglePlayRequest request, Action<LoginResponse> responseCallback)
		{
			this.Login("login/LoginWithGooglePlay", JsonUtility.ToJson(request), responseCallback);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000A024 File Offset: 0x00008224
		public void LoginWithIos(LoginWithIosRequest request, Action<LoginResponse> responseCallback)
		{
			this.Login("login/LoginWithIos", JsonUtility.ToJson(request), responseCallback);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000A038 File Offset: 0x00008238
		public void LoginWithGameCenter(LoginWithGameCenterRequest request, Action<LoginResponse> responseCallback)
		{
			this.Login("login/LoginWithGameCenter", JsonUtility.ToJson(request), responseCallback);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000A04C File Offset: 0x0000824C
		public void LoginWithKongregate(LoginWithCustomIDRequest request, Action<LoginResponse> responseCallback)
		{
			this.Login("login/LoginWithKongregate", JsonUtility.ToJson(request), responseCallback);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000A060 File Offset: 0x00008260
		public void LoginWithNutaku(LoginWithCustomIDRequest request, Action<LoginResponse> responseCallback)
		{
			this.Login("login/LoginWithNutaku", JsonUtility.ToJson(request), responseCallback);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000A074 File Offset: 0x00008274
		private void Login(string path, string request, Action<LoginResponse> responseCallback)
		{
			BlayFapClient.LoginTime = DateTime.UtcNow;
			base.StartCoroutine(this.PostAsync<LoginResponse>(this.baseUrl + path, request, delegate(LoginResponse response)
			{
				if (response.Error == null)
				{
					BlayFapClient.LoggedIn = true;
					BlayFapClient.BlayFapId = response.BlayFapId;
					BlayFapClient.AuthToken = response.AuthToken;
					BlayFapClient.CreationDate = response.CreationDate;
				}
				else
				{
					BlayFapClient.LoggedIn = false;
					BlayFapClient.BlayFapId = 0UL;
					BlayFapClient.AuthToken = string.Empty;
					BlayFapClient.CreationDate = default(DateTime);
				}
				if (responseCallback != null)
				{
					responseCallback(response);
				}
			}));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000A0C0 File Offset: 0x000082C0
		public void AddApiCount(string apiName)
		{
			AddApiCountRequest obj = new AddApiCountRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				ApiCounterName = apiName
			};
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + "client/ApiCount", JsonUtility.ToJson(obj), delegate(BlayFapResponse response)
			{
			}));
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000A128 File Offset: 0x00008328
		public void GetTime(Action<GetTimeResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<GetTimeResponse>(responseCallback))
			{
				return;
			}
			GetTimeRequest obj = new GetTimeRequest
			{
				BlayFapId = BlayFapClient.BlayFapId
			};
			base.StartCoroutine(this.PostAsync<GetTimeResponse>(this.baseUrl + "client/GetTime", JsonUtility.ToJson(obj), responseCallback));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000A180 File Offset: 0x00008380
		public void GetTitleData(Action<GetTitleDataResponse> responseCallback)
		{
			GetTitleDataRequest request = new GetTitleDataRequest
			{
				BlayFapId = BlayFapClient.BlayFapId
			};
			this.GetTitleData(request, responseCallback);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000A1A8 File Offset: 0x000083A8
		public void GetTitleData(GetTitleDataRequest request, Action<GetTitleDataResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<GetTitleDataResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<GetTitleDataResponse>(this.baseUrl + "client/GetTitleData", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000A1F8 File Offset: 0x000083F8
		public void GetUserInfo(Action<GetUserInfoResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<GetUserInfoResponse>(responseCallback))
			{
				return;
			}
			GetUserInfoRequest obj = new GetUserInfoRequest
			{
				BlayFapId = BlayFapClient.BlayFapId
			};
			base.StartCoroutine(this.PostAsync<GetUserInfoResponse>(this.baseUrl + "client/GetUserInfo", JsonUtility.ToJson(obj), responseCallback));
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000A250 File Offset: 0x00008450
		public void GetUserInventory(Action<GetUserInventoryResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<GetUserInventoryResponse>(responseCallback))
			{
				return;
			}
			GetUserInventoryRequest obj = new GetUserInventoryRequest
			{
				BlayFapId = BlayFapClient.BlayFapId
			};
			base.StartCoroutine(this.PostAsync<GetUserInventoryResponse>(this.baseUrl + "client/GetUserInventory", JsonUtility.ToJson(obj), responseCallback));
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000A2A8 File Offset: 0x000084A8
		public void ConsumeUserItem(ConsumeUserItemRequest request, Action<ConsumeUserItemResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<ConsumeUserItemResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<ConsumeUserItemResponse>(this.baseUrl + "client/ConsumeUserItem", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000A2F8 File Offset: 0x000084F8
		public void MigrateUser(Action<MigrateUserResponse> responseCallback)
		{
			if (!this.IsLoggedIn<MigrateUserResponse>(responseCallback))
			{
				return;
			}
			AuthenticatedBlayFapRequest obj = new AuthenticatedBlayFapRequest
			{
				BlayFapId = BlayFapClient.BlayFapId
			};
			base.StartCoroutine(this.PostAsync<MigrateUserResponse>(this.baseUrl + "client/MigrateUser", JsonUtility.ToJson(obj), responseCallback));
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000A34C File Offset: 0x0000854C
		public void SetUserData(SetUserDataRequest request, Action<SetUserDataResponse> responseCallback)
		{
			if (!this.IsLoggedIn<SetUserDataResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<SetUserDataResponse>(this.baseUrl + "client/SetUserData", SimpleJson.SerializeObject(request, null), responseCallback));
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000A398 File Offset: 0x00008598
		public void GetUserData(GetUserDataRequest request, Action<GetUserDataResponse> responseCallback)
		{
			if (!this.IsLoggedIn<GetUserDataResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<GetUserDataResponse>(this.baseUrl + "client/GetUserData", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000A3E4 File Offset: 0x000085E4
		public void GetCombinedUserData(GetCombinedUserDataRequest request, Action<GetCombinedUserDataResponse> responseCallback)
		{
			if (!this.IsLoggedIn<GetCombinedUserDataResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<GetCombinedUserDataResponse>(this.baseUrl + "client/GetCombinedUserData", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000A430 File Offset: 0x00008630
		public void ConsumeUserVirtualCurrency(ConsumeUserVirtualCurrencyRequest request, Action<ConsumeUserVirtualCurrencyResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<ConsumeUserVirtualCurrencyResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<BlayFapClient.UserVirtualCurrencyInternalResponse>(this.baseUrl + "client/ConsumeUserVirtualCurrency", JsonUtility.ToJson(request), delegate(BlayFapClient.UserVirtualCurrencyInternalResponse internalResponse)
			{
				ConsumeUserVirtualCurrencyResponse obj;
				if (internalResponse.Error != null)
				{
					obj = new ConsumeUserVirtualCurrencyResponse
					{
						Error = internalResponse.Error
					};
				}
				else
				{
					Dictionary<CurrencyType, int> dictionary = new Dictionary<CurrencyType, int>();
					if (internalResponse.ConsumedCurrency != null)
					{
						int value;
						if (internalResponse.ConsumedCurrency.TryGetValue("CurrencyA", out value))
						{
							dictionary.Add(CurrencyType.DiamondRefunds, value);
						}
						if (internalResponse.ConsumedCurrency.TryGetValue("CurrencyB", out value))
						{
							dictionary.Add(CurrencyType.EventTokenRefunds, value);
						}
						if (internalResponse.ConsumedCurrency.TryGetValue("CurrencyC", out value))
						{
							dictionary.Add(CurrencyType.CurrencyC, value);
						}
						if (internalResponse.ConsumedCurrency.TryGetValue("CurrencyD", out value))
						{
							dictionary.Add(CurrencyType.CurrencyD, value);
						}
					}
					obj = new ConsumeUserVirtualCurrencyResponse
					{
						ConsumedCurrency = dictionary
					};
				}
				responseCallback(obj);
			}));
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000A4A4 File Offset: 0x000086A4
		public void StartPurchase(StartPurchaseRequest request, Action<StartPurchaseResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<StartPurchaseResponse>(responseCallback))
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<StartPurchaseResponse>(this.baseUrl + "purchase/StartPurchase", JsonUtility.ToJson(request), delegate(StartPurchaseResponse internalResponse)
			{
				StartPurchaseResponse obj;
				if (internalResponse.Error != null)
				{
					obj = new StartPurchaseResponse
					{
						Error = internalResponse.Error
					};
				}
				else
				{
					obj = internalResponse;
				}
				responseCallback(obj);
			}));
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A50C File Offset: 0x0000870C
		public void FinishPurchase(FinishPurchaseRequest request, Action<FinishPurchaseResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<FinishPurchaseResponse>(responseCallback))
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<FinishPurchaseResponse>(this.baseUrl + "purchase/FinishPurchase", JsonUtility.ToJson(request), delegate(FinishPurchaseResponse internalResponse)
			{
				FinishPurchaseResponse obj;
				if (internalResponse.Error != null)
				{
					obj = new FinishPurchaseResponse
					{
						Error = internalResponse.Error
					};
				}
				else
				{
					obj = internalResponse;
				}
				responseCallback(obj);
			}));
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A574 File Offset: 0x00008774
		public void GetFilteredCatalog(GetFilteredCatalogRequest request, Action<GetCatalogResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<GetCatalogResponse>(this.baseUrl + "purchase/GetFilteredCatalog", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A5A4 File Offset: 0x000087A4
		public void GetCatalog(GetCatalogRequest request, Action<GetCatalogResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			base.StartCoroutine(this.GetAsync<GetCatalogResponse>(this.baseUrl + "purchase/GetCatalog", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A5D4 File Offset: 0x000087D4
		public void ValidateGooglePlayReceipt(ValidateGooglePlayReceiptRequest request, Action<ValidateGooglePlayReceiptResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<ValidateGooglePlayReceiptResponse>(responseCallback))
			{
				GameState.CurrentState.transform.Find("Popups/Purchase Error Popup").gameObject.SetActive(true);
				return;
			}
			base.StartCoroutine(this.PostAsync<ValidateGooglePlayReceiptResponse>(this.baseUrl + "purchase/ValidateGooglePlayReceipt", JsonUtility.ToJson(request), delegate(ValidateGooglePlayReceiptResponse internalResponse)
			{
				ValidateGooglePlayReceiptResponse obj;
				if (internalResponse.Error != null)
				{
					obj = new ValidateGooglePlayReceiptResponse
					{
						Error = internalResponse.Error
					};
				}
				else
				{
					obj = internalResponse;
				}
				responseCallback(obj);
			}));
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A65C File Offset: 0x0000885C
		public void ValidateItunesReceipt(ValidateItunesReceiptRequest request, Action<ValidateItunesReceiptResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<ValidateItunesReceiptResponse>(responseCallback))
			{
				GameState.CurrentState.transform.Find("Popups/Purchase Error Popup").gameObject.SetActive(true);
				return;
			}
			base.StartCoroutine(this.PostAsync<ValidateItunesReceiptResponse>(this.baseUrl + "purchase/ValidateItunesReceipt", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000A6C0 File Offset: 0x000088C0
		public void UpdateUserSession(UpdateUserSessionRequest request, Action<UpdateUserSessionResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<UpdateUserSessionResponse>(responseCallback))
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<UpdateUserSessionResponse>(this.baseUrl + "client/UpdateUserSession", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A708 File Offset: 0x00008908
		public void GetUserSession(GetUserSessionRequest request, Action<GetUserSessionResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<GetUserSessionResponse>(responseCallback))
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<GetUserSessionResponse>(this.baseUrl + "client/GetUserSession", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000A750 File Offset: 0x00008950
		public void UpdateDynamoUserSession(UpdateUserSessionRequest request, Action<UpdateUserSessionResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<UpdateUserSessionResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			if (this.SessionId == 0UL)
			{
				Debug.LogError("UpdateDynamoUserSession was called before GetDynamoUserSession");
			}
			else if (this.SessionId != request.SessionID)
			{
				Debug.LogError("Session Ids did not match");
			}
			base.StartCoroutine(this.PostAsync<UpdateUserSessionResponse>(this.baseUrl + "client/UpdateDynamoUserSession", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A7D8 File Offset: 0x000089D8
		public void GetDynamoUserSession(GetUserSessionRequest request, Action<GetDynamoUserSessionResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<GetDynamoUserSessionResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<GetDynamoUserSessionResponse>(this.baseUrl + "client/GetDynamoUserSession", JsonUtility.ToJson(request), delegate(GetDynamoUserSessionResponse response)
			{
				if (response.Error == null)
				{
					this.SessionId = response.SessionID;
				}
				responseCallback(response);
			}));
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A854 File Offset: 0x00008A54
		public void GetNewestSessionId(AuthenticatedBlayFapRequest request, Action<GetNewestSessionIdResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<GetNewestSessionIdResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<GetNewestSessionIdResponse>(this.baseUrl + "client/GetNewestSessionId", JsonUtility.ToJson(request), delegate(GetNewestSessionIdResponse response)
			{
				if (response.Error != null || this.SessionId == 0UL || this.SessionId != response.SessionID)
				{
				}
				responseCallback(response);
			}));
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		public void RedeemCoupon(RedeemCouponRequest request, Action<BlayFapResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<BlayFapResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + "client/RedeemCoupon", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000A920 File Offset: 0x00008B20
		public void GetPoll(GetPollRequest request, Action<GetPollResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<GetPollResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<GetPollResponse>(this.baseUrl + "poll/GetPoll", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000A970 File Offset: 0x00008B70
		public void VoteForPoll(VoteForPollRequest request, Action<BlayFapResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<BlayFapResponse>(responseCallback))
			{
				return;
			}
			request.BlayFapId = BlayFapClient.BlayFapId;
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + "poll/VoteForPoll", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000A9C0 File Offset: 0x00008BC0
		public void AwardPurchasedItem(Store2.PurchaseType purchaseType, Action<BlayFapResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<BlayFapResponse>(responseCallback))
			{
				return;
			}
			string text;
			switch (purchaseType)
			{
			case Store2.PurchaseType.JelleQuillzone:
				text = "crushcrush/PurchaseJelleQuillzone";
				goto IL_149;
			case Store2.PurchaseType.BonchovySpectrum:
				text = "crushcrush/PurchaseBonchovySpectrum";
				goto IL_149;
			case Store2.PurchaseType.Odango:
				text = "crushcrush/PurchaseOdango";
				goto IL_149;
			case Store2.PurchaseType.Shibuki:
				text = "crushcrush/PurchaseShibuki";
				goto IL_149;
			case Store2.PurchaseType.Sirina:
				text = "crushcrush/PurchaseSirina";
				goto IL_149;
			case Store2.PurchaseType.Vellatrix:
				text = "crushcrush/PurchaseVellatrix";
				goto IL_149;
			case Store2.PurchaseType.Roxxy:
				text = "crushcrush/PurchaseRoxxy";
				goto IL_149;
			case Store2.PurchaseType.Tessa:
				text = "crushcrush/PurchaseTessa";
				goto IL_149;
			case Store2.PurchaseType.NinaUnique:
				text = "crushcrush/AwardNinaUnique";
				goto IL_149;
			case Store2.PurchaseType.Juliet:
				text = "crushcrush/PurchaseJuliet";
				goto IL_149;
			case Store2.PurchaseType.Rosa:
				text = "crushcrush/PurchaseRosa";
				goto IL_149;
			case Store2.PurchaseType.FullVoices:
				text = "crushcrush/PurchaseFullVoices";
				goto IL_149;
			case Store2.PurchaseType.OutfitsBikini:
				text = "crushcrush/PurchaseBikinis";
				goto IL_149;
			case Store2.PurchaseType.OutfitsSchool:
				text = "crushcrush/PurchaseSchool";
				goto IL_149;
			case Store2.PurchaseType.OutfitsWedding:
				text = "crushcrush/PurchaseWedding";
				goto IL_149;
			case Store2.PurchaseType.OutfitsChristmas:
				text = "crushcrush/PurchaseChristmas";
				goto IL_149;
			}
			text = "crushcrush/Purchase" + purchaseType.ToString();
			IL_149:
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			AuthenticatedBlayFapRequest obj = new AuthenticatedBlayFapRequest
			{
				BlayFapId = BlayFapClient.BlayFapId
			};
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + text, JsonUtility.ToJson(obj), responseCallback));
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000AB58 File Offset: 0x00008D58
		public void AwardPlayfabItem(Playfab.PlayfabItems itemType, Action<BlayFapResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<BlayFapResponse>(responseCallback))
			{
				return;
			}
			string text = null;
			if (itemType != Playfab.PlayfabItems.Anniversary2022)
			{
				if (itemType != Playfab.PlayfabItems.Winter2018)
				{
					if (itemType != Playfab.PlayfabItems.Nutaku2019)
					{
						if (itemType == (Playfab.PlayfabItems)((ulong)-2147483648))
						{
							text = "crushcrush/AwardAnniversary2021";
						}
					}
					else
					{
						text = "crushcrush/AwardNutaku2019";
					}
				}
				else
				{
					text = "crushcrush/AwardWinter2018";
				}
			}
			else
			{
				text = "crushcrush/AwardAnniversary2022";
			}
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			AuthenticatedBlayFapRequest obj = new AuthenticatedBlayFapRequest
			{
				BlayFapId = BlayFapClient.BlayFapId
			};
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + text, JsonUtility.ToJson(obj), responseCallback));
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000AC20 File Offset: 0x00008E20
		public void RequestAccountDeletion(AuthenticatedBlayFapRequest request, Action<BlayFapResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<BlayFapResponse>(responseCallback))
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + "client/RequestAccountDeletion", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000AC68 File Offset: 0x00008E68
		public void CancelAccountDeletion(AuthenticatedBlayFapRequest request, Action<BlayFapResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<BlayFapResponse>(responseCallback))
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + "client/CancelAccountDeletion", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000ACB0 File Offset: 0x00008EB0
		public void LinkSupportAccount(LinkSupportAccountRequest request, Action<BlayFapResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<BlayFapResponse>(responseCallback))
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + "client/LinkSupportAccount", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000ACF8 File Offset: 0x00008EF8
		public void SubmitSupportTicket(SubmitSupportTicketRequest request, Action<BlayFapResponse> responseCallback)
		{
			if (responseCallback == null)
			{
				return;
			}
			if (!this.IsLoggedIn<BlayFapResponse>(responseCallback))
			{
				return;
			}
			base.StartCoroutine(this.PostAsync<BlayFapResponse>(this.baseUrl + "client/SubmitSupportTicket", JsonUtility.ToJson(request), responseCallback));
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000AD40 File Offset: 0x00008F40
		private IEnumerator PostAsync<T>(string uri, string body, Action<T> responseCallback) where T : BlayFapResponse, new()
		{
			using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
			{
				www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body))
				{
					contentType = "application/json"
				};
				www.SetRequestHeader("Content-Type", "application/json");
				if (!string.IsNullOrEmpty(BlayFapClient.AuthToken))
				{
					www.SetRequestHeader("X-Authorization", BlayFapClient.AuthToken);
				}
				this.Log(BlayFapClient.ELoggingLevel.Debug, "Sending post request to [{0}]", new string[]
				{
					uri
				});
				this.Log(BlayFapClient.ELoggingLevel.Trace, ">> [{0}]", new string[]
				{
					body
				});
				yield return www.Send();
				if (www.isError)
				{
					this.Log(BlayFapClient.ELoggingLevel.Error, "Error while sending BlayFap data: [{0}]", new string[]
					{
						www.error
					});
					if (uri.Contains("Login"))
					{
						if (responseCallback != null)
						{
							T response3 = Activator.CreateInstance<T>();
							response3.Error = new BlayFapResponseError(BlayFapResponseError.BlayFapError.ServiceUnavailable);
							responseCallback(response3);
						}
					}
					else if (www.responseCode == 401L)
					{
						Debug.Log("Lost authentication - trying to login and then attempting to call PostAsync again.");
						this.queuedCalls.Add(delegate(LoginResponse response)
						{
							base.StartCoroutine(this.PostAsync<T>(uri, body, responseCallback));
						});
						this.blayfapInitialized = false;
						this.OnBlayFapLogin.AddLateListener(new ReactiveProperty<LoginResponse>.Changed(this.OnRelogin));
						this.SetupBlayFap();
					}
					else if (responseCallback != null)
					{
						T response2 = Activator.CreateInstance<T>();
						response2.Error = new BlayFapResponseError(BlayFapResponseError.BlayFapError.UnknownError);
						responseCallback(response2);
					}
				}
				else
				{
					string contents = www.downloadHandler.text;
					this.Log(BlayFapClient.ELoggingLevel.Trace, "<< [{0}]", new string[]
					{
						contents
					});
					if (contents != string.Empty)
					{
						T result = SimpleJson.DeserializeObject<T>(contents, null);
						if (result.Error != null && result.Error.ErrorType == BlayFapResponseError.BlayFapError.InMaintenance)
						{
							BlayFapClient.InMaintenance.Value = true;
							T maintenance = Activator.CreateInstance<T>();
							maintenance.Error = new BlayFapResponseError(BlayFapResponseError.BlayFapError.InMaintenance);
							responseCallback(maintenance);
						}
						else
						{
							BlayFapClient.InMaintenance.Value = false;
							if (responseCallback != null)
							{
								responseCallback(result);
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000AD88 File Offset: 0x00008F88
		private void OnRelogin(LoginResponse response)
		{
			if (response.Error == null)
			{
				this.OnBlayFapLogin.RemoveLateListener(new ReactiveProperty<LoginResponse>.Changed(this.OnRelogin));
				foreach (ReactiveProperty<LoginResponse>.Changed changed in this.queuedCalls)
				{
					changed(response);
				}
				this.queuedCalls.Clear();
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000AE1C File Offset: 0x0000901C
		private IEnumerator GetAsync<T>(string uri, string body, Action<T> responseCallback) where T : BlayFapResponse, new()
		{
			using (UnityWebRequest www = UnityWebRequest.Get(uri))
			{
				www.SetRequestHeader("Content-Type", "application/json");
				if (!string.IsNullOrEmpty(BlayFapClient.AuthToken))
				{
					www.SetRequestHeader("X-Authorization", BlayFapClient.AuthToken);
				}
				this.Log(BlayFapClient.ELoggingLevel.Debug, "Sending request to [{0}]", new string[]
				{
					uri
				});
				this.Log(BlayFapClient.ELoggingLevel.Trace, ">> [{0}]", new string[]
				{
					body
				});
				yield return www.Send();
				if (www.isError)
				{
					this.Log(BlayFapClient.ELoggingLevel.Error, "Error while sending BlayFap data: [{0}]", new string[]
					{
						www.error
					});
				}
				else
				{
					string contents = www.downloadHandler.text;
					this.Log(BlayFapClient.ELoggingLevel.Trace, "<< [{0}]", new string[]
					{
						contents
					});
					if (contents != string.Empty)
					{
						T result = SimpleJson.DeserializeObject<T>(contents, null);
						if (responseCallback != null)
						{
							responseCallback(result);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000AE64 File Offset: 0x00009064
		private T TryDeserializeObject<T>(string contents) where T : BlayFapResponse, new()
		{
			T result;
			try
			{
				T t = SimpleJson.DeserializeObject<T>(contents, null);
				result = t;
			}
			catch (Exception)
			{
				Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "Failed parsing json.  Type: " + typeof(T).ToString() + " data: " + contents);
				T t2 = Activator.CreateInstance<T>();
				t2.Error = new BlayFapResponseError(BlayFapResponseError.BlayFapError.UnknownError);
				result = t2;
			}
			return result;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000AEEC File Offset: 0x000090EC
		private bool IsLoggedIn<T>(Action<T> responseCallback) where T : BlayFapResponse, new()
		{
			if (BlayFapClient.LoggedIn)
			{
				return true;
			}
			T t = Activator.CreateInstance<T>();
			t.Error = new BlayFapResponseError(BlayFapResponseError.BlayFapError.NotAuthenticated);
			T obj = t;
			responseCallback(obj);
			return false;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000AF28 File Offset: 0x00009128
		private void Log(BlayFapClient.ELoggingLevel level, string message, params string[] args)
		{
			if (level < this.LoggingLevel)
			{
				return;
			}
			message = string.Concat(new object[]
			{
				"[BlayFap] [",
				level,
				"]: ",
				message
			});
			switch (level)
			{
			case BlayFapClient.ELoggingLevel.Trace:
			case BlayFapClient.ELoggingLevel.Debug:
			case BlayFapClient.ELoggingLevel.Info:
				Debug.LogFormat(message, args);
				break;
			case BlayFapClient.ELoggingLevel.Warning:
				Debug.LogWarningFormat(message, args);
				break;
			case BlayFapClient.ELoggingLevel.Error:
				Debug.LogErrorFormat(message, args);
				break;
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000AFBC File Offset: 0x000091BC
		private void OnLogin(LoginResponse response, bool relogin)
		{
			if (relogin)
			{
				return;
			}
			this.OnBlayFapLogin.Value = response;
			if (!Nutaku.Connected && !Johren.Connected)
			{
				this.AllowGameLoad.Value = true;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000B004 File Offset: 0x00009204
		public void SetupBlayFap()
		{
			if (this.blayfapInitialized)
			{
				return;
			}
			if (this.lastSignInAttempt > DateTime.UtcNow - TimeSpan.FromMinutes(5.0))
			{
				return;
			}
			this.lastSignInAttempt = DateTime.UtcNow;
			this.blayfapInitialized = true;
			if (SteamManager.Initialized)
			{
				if (SteamUser.BLoggedOn())
				{
					SteamManager.Instance.GetSteamToken(delegate(string token)
					{
						LoginWithSteamRequest request = new LoginWithSteamRequest
						{
							CreateAccount = true,
							SteamAuthToken = token,
							SteamId = SteamManager.Instance.GetSteamId(),
							VersionIdentifier = AssetBundleSettings.Manifest.Substring(1, 6)
						};
						BlayFapClient.Instance.LoginWithSteam(request, delegate(LoginResponse response)
						{
							this.OnLogin(response, false);
						});
					}, delegate(EResult error)
					{
					});
				}
				else
				{
					this.OnLogin(new LoginResponse
					{
						Error = new BlayFapResponseError(BlayFapResponseError.BlayFapError.SteamApiOffline)
					}, false);
				}
				return;
			}
			this.AllowGameLoad.Value = true;
		}

		// Token: 0x040001AD RID: 429
		private static BlayFapClient instance;

		// Token: 0x040001AE RID: 430
		private readonly string baseUrl = "https://blayfap.sadpandastudios.com/";

		// Token: 0x040001AF RID: 431
		private readonly BlayFapClient.ELoggingLevel LoggingLevel = BlayFapClient.ELoggingLevel.Warning;

		// Token: 0x040001B0 RID: 432
		public static bool UsingBlayfap = false;

		// Token: 0x040001B1 RID: 433
		public static DateTime LoginTime = new DateTime(2000, 1, 1);

		// Token: 0x040001B2 RID: 434
		public static ReactiveProperty<bool> InMaintenance = new ReactiveProperty<bool>();

		// Token: 0x040001B3 RID: 435
		public static ulong BlayFapId = 0UL;

		// Token: 0x040001B4 RID: 436
		public static bool LoggedIn = false;

		// Token: 0x040001B5 RID: 437
		public static DateTime CreationDate;

		// Token: 0x040001B6 RID: 438
		private static string AuthToken = string.Empty;

		// Token: 0x040001B7 RID: 439
		public ulong SessionId;

		// Token: 0x040001B8 RID: 440
		private List<ReactiveProperty<LoginResponse>.Changed> queuedCalls = new List<ReactiveProperty<LoginResponse>.Changed>();

		// Token: 0x040001B9 RID: 441
		private DateTime lastSignInAttempt = default(DateTime);

		// Token: 0x040001BA RID: 442
		private bool blayfapInitialized;

		// Token: 0x040001BB RID: 443
		public ReactiveProperty<LoginResponse> OnBlayFapLogin = new ReactiveProperty<LoginResponse>(null);

		// Token: 0x040001BC RID: 444
		public ReactiveProperty<bool> AllowGameLoad = new ReactiveProperty<bool>(false);

		// Token: 0x02000035 RID: 53
		private enum ELoggingLevel
		{
			// Token: 0x040001C0 RID: 448
			None = 6,
			// Token: 0x040001C1 RID: 449
			Error = 5,
			// Token: 0x040001C2 RID: 450
			Warning = 4,
			// Token: 0x040001C3 RID: 451
			Info = 3,
			// Token: 0x040001C4 RID: 452
			Debug = 2,
			// Token: 0x040001C5 RID: 453
			Trace = 1
		}

		// Token: 0x02000036 RID: 54
		private class UserVirtualCurrencyInternalResponse : BlayFapResponse
		{
			// Token: 0x040001C6 RID: 454
			public Dictionary<string, int> ConsumedCurrency;
		}
	}
}
