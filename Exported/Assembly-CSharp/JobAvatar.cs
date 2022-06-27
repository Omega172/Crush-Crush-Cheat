using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D0 RID: 208
public class JobAvatar : MonoBehaviour
{
	// Token: 0x060004AC RID: 1196 RVA: 0x00025C80 File Offset: 0x00023E80
	private void OnEnable()
	{
		if (this.ActiveJob != null && this.ActiveJob.Value != null)
		{
			this.experienceProperty.Force(this.ActiveJob.Value.Experience);
			this.levelProperty.Force(this.ActiveJob.Value.Level);
		}
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00025CE4 File Offset: 0x00023EE4
	private void Start()
	{
		this.payoutProperty += delegate(double value)
		{
			this.PayoutText.text = ((value != 0.0) ? string.Format("${0} Per Second", Utilities.ToPrefixedNumber(value, false, false)) : string.Empty);
		};
		this.promotionProperty += delegate(double value)
		{
			this.PromotionPayout.text = ((value != 0.0) ? ((value > 1.0) ? string.Format("+${0} Per Second", Utilities.ToPrefixedNumber(value, false, false)) : "+$1 Per Second") : string.Empty);
		};
		this.experienceProperty += delegate(long value)
		{
			if (this.ActiveJob.Value == null)
			{
				return;
			}
			if (this.ActiveJob.Value.ExperiencePayout == -1L)
			{
				this.PromotionText.text = Translations.GetTranslation("everything_else_19_1", "Max Level!");
			}
			else if (this.ActiveJob.Value.ExperienceToLevel >= 1000000000L)
			{
				this.PromotionText.text = string.Format("{0}% ({1})", (Math.Floor((double)value / (double)this.ActiveJob.Value.ExperienceToLevel * 1000.0) / 10.0).ToString("0.#"), this.ActiveJob.Value.Experience.ToString("n0"));
			}
			else
			{
				this.PromotionText.text = string.Format("{0}/{1}", value.ToString("n0"), this.ActiveJob.Value.ExperienceToLevel.ToString("n0"));
			}
			this.levelProperty.Value = ((!(this.ActiveJob.Value == null)) ? this.ActiveJob.Value.Level : -1);
			float num = (float)value / (float)this.ActiveJob.Value.ExperienceToLevel;
			if (float.IsNaN(num) || num > 1f || num < 0f || this.ActiveJob.Value.ExperiencePayout == -1L)
			{
				num = 1f;
			}
			this.PromotionBar.fillAmount = num;
		};
		this.levelProperty += delegate(int value)
		{
			if (this.ActiveJob.Value == null)
			{
				return;
			}
			if (this.ActiveJob.Value.ExperiencePayout == -1L)
			{
				this.promotionProperty.Value = 0.0;
				this.payoutProperty.Value = this.ActiveJob.Value.IncomePerSecond * (double)GameState.PurchasedAdMultiplier * (double)GameState.CurrentState.TimeMultiplier.Value * (double)((!this.ActiveJob.Value.Gilded) ? 1 : 5);
			}
			else
			{
				this.promotionProperty.Value = (double)this.ActiveJob.Value.IncomeDiff * (double)GameState.PurchasedAdMultiplier * (double)GameState.CurrentState.TimeMultiplier.Value * (double)((!this.ActiveJob.Value.Gilded) ? 1 : 5);
				this.payoutProperty.Value = this.ActiveJob.Value.IncomePerSecond * (double)GameState.PurchasedAdMultiplier * (double)GameState.CurrentState.TimeMultiplier.Value * (double)((!this.ActiveJob.Value.Gilded) ? 1 : 5);
			}
		};
		this.gildProperty += delegate(bool value)
		{
			if (this.ActiveJob.Value != null)
			{
				this.levelProperty.Force(this.ActiveJob.Value.Level);
			}
		};
		this.ActiveJob += delegate(Job2 activeJob)
		{
			this.animationTime = 0.5f;
			if (activeJob == null)
			{
				this.TitleText.text = "Zzzz";
				this.PromotionIndicator.SetActive(false);
				this.PayoutContainer.SetActive(false);
				this.payoutProperty.Value = 0.0;
			}
			else
			{
				this.TitleText.text = Translations.TranslateJob(this.ActiveJob.Value.JobType, 0).ToUpperInvariant();
				this.PromotionIndicator.SetActive(true);
				this.PayoutContainer.SetActive(true);
				this.levelProperty.Force(activeJob.Level);
				this.experienceProperty.Force(this.ActiveJob.Value.Experience);
				this.gildProperty.Force(activeJob.Gilded);
			}
		};
		Translations.CurrentLanguage += delegate(int language)
		{
			if (this.ActiveJob.Value == null)
			{
				this.TitleText.text = "Zzzz";
			}
			else
			{
				this.TitleText.text = Translations.TranslateJob(this.ActiveJob.Value.JobType, 0).ToUpperInvariant();
			}
		};
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00025DBC File Offset: 0x00023FBC
	private void Update()
	{
		this.animationTime += Time.deltaTime;
		if (this.animationTime > 0.5f)
		{
			this.animationTime -= 0.5f;
			this.animationFrame = ((this.animationFrame != 0) ? 0 : 1);
			if (this.ActiveJob.Value != null && this.ActiveJob.Value.JobSprite1 != null)
			{
				if (this.ActiveJob.Value.JobSprite2 != null)
				{
					this.Avatar.sprite = ((this.animationFrame != 0) ? this.ActiveJob.Value.JobSprite2 : this.ActiveJob.Value.JobSprite1);
				}
				else
				{
					this.Avatar.sprite = this.ActiveJob.Value.JobSprite1;
				}
			}
			else if (this.Idle1 != null && this.Idle2 != null)
			{
				this.Avatar.sprite = ((this.animationFrame != 0) ? this.Idle2 : this.Idle1);
			}
		}
		if (this.ActiveJob != null && this.ActiveJob.Value != null)
		{
			if (this.ActiveJob.Value.ExperiencePayout != -1L)
			{
				this.experienceProperty.Value = this.ActiveJob.Value.Experience;
			}
			else
			{
				this.experienceProperty.Value = -1L;
			}
			this.gildProperty.Value = this.ActiveJob.Value.Gilded;
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00025F8C File Offset: 0x0002418C
	public void BackButtonToggleJob()
	{
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00025F90 File Offset: 0x00024190
	private void OnDisable()
	{
		this.LastToggledJob = null;
	}

	// Token: 0x040004C5 RID: 1221
	public Image Avatar;

	// Token: 0x040004C6 RID: 1222
	public Image PromotionBar;

	// Token: 0x040004C7 RID: 1223
	public Text PromotionPayout;

	// Token: 0x040004C8 RID: 1224
	public Text PromotionText;

	// Token: 0x040004C9 RID: 1225
	public Text TitleText;

	// Token: 0x040004CA RID: 1226
	public Text PayoutText;

	// Token: 0x040004CB RID: 1227
	public GameObject PromotionIndicator;

	// Token: 0x040004CC RID: 1228
	public GameObject PayoutContainer;

	// Token: 0x040004CD RID: 1229
	public ReactiveProperty<Job2> ActiveJob = new ReactiveProperty<Job2>(null);

	// Token: 0x040004CE RID: 1230
	public Sprite Idle1;

	// Token: 0x040004CF RID: 1231
	public Sprite Idle2;

	// Token: 0x040004D0 RID: 1232
	private float animationTime;

	// Token: 0x040004D1 RID: 1233
	private int animationFrame;

	// Token: 0x040004D2 RID: 1234
	private ReactiveProperty<double> payoutProperty = new ReactiveProperty<double>(-1.0);

	// Token: 0x040004D3 RID: 1235
	private ReactiveProperty<double> promotionProperty = new ReactiveProperty<double>(-1.0);

	// Token: 0x040004D4 RID: 1236
	private ReactiveProperty<long> experienceProperty = new ReactiveProperty<long>(-1L);

	// Token: 0x040004D5 RID: 1237
	private ReactiveProperty<int> levelProperty = new ReactiveProperty<int>(-1);

	// Token: 0x040004D6 RID: 1238
	private ReactiveProperty<bool> gildProperty = new ReactiveProperty<bool>(false);

	// Token: 0x040004D7 RID: 1239
	public Job2 LastToggledJob;
}
