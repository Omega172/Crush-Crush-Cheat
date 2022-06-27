using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A1 RID: 161
public class HobbyAvatar : MonoBehaviour
{
	// Token: 0x060003D0 RID: 976 RVA: 0x000208C8 File Offset: 0x0001EAC8
	private void Start()
	{
		this.ActiveHobby += delegate(Hobby2 activeHobby)
		{
			this.animationTime = 0.5f;
			if (activeHobby == null)
			{
				this.ActiveHobbyText.text = "Zzzz";
			}
			else
			{
				this.ActiveHobbyText.text = Translations.GetHobbyTitle(activeHobby.Data.Id).ToUpperInvariant();
			}
			if (this.activeHobbyStat != null)
			{
				this.activeHobbyStat.text = ((!(activeHobby == null)) ? ("+ " + Translations.TranslateSkill(activeHobby.Data.Id)) : string.Empty);
			}
		};
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x000208E8 File Offset: 0x0001EAE8
	private void Update()
	{
		this.animationTime += Time.deltaTime;
		if (this.animationTime > 0.5f)
		{
			this.animationTime -= 0.5f;
			this.animationFrame = ((this.animationFrame != 0) ? 0 : 1);
			if (this.ActiveHobby.Value != null && this.ActiveHobby.Value.HobbySprite1 != null)
			{
				if (this.ActiveHobby.Value.HobbySprite2 != null)
				{
					this.Avatar.sprite = ((this.animationFrame != 0) ? this.ActiveHobby.Value.HobbySprite2 : this.ActiveHobby.Value.HobbySprite1);
				}
				else
				{
					this.Avatar.sprite = this.ActiveHobby.Value.HobbySprite1;
				}
			}
			else if (this.Idle1 != null && this.Idle2 != null)
			{
				this.Avatar.sprite = ((this.animationFrame != 0) ? this.Idle2 : this.Idle1);
			}
		}
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00020A38 File Offset: 0x0001EC38
	public void BackButtonToggleHobby()
	{
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00020A3C File Offset: 0x0001EC3C
	private void OnDisable()
	{
		this.LastToggledHobby = null;
	}

	// Token: 0x04000406 RID: 1030
	public Image Avatar;

	// Token: 0x04000407 RID: 1031
	public ReactiveProperty<Hobby2> ActiveHobby = new ReactiveProperty<Hobby2>(null);

	// Token: 0x04000408 RID: 1032
	public Text ActiveHobbyText;

	// Token: 0x04000409 RID: 1033
	private Text activeHobbyStat;

	// Token: 0x0400040A RID: 1034
	public Sprite Idle1;

	// Token: 0x0400040B RID: 1035
	public Sprite Idle2;

	// Token: 0x0400040C RID: 1036
	private float animationTime;

	// Token: 0x0400040D RID: 1037
	private int animationFrame;

	// Token: 0x0400040E RID: 1038
	public Hobby2 LastToggledHobby;
}
