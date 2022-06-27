using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000C RID: 12
public class ExploraSpinner : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600002C RID: 44 RVA: 0x00003A6C File Offset: 0x00001C6C
	// (set) Token: 0x0600002D RID: 45 RVA: 0x00003A74 File Offset: 0x00001C74
	public Sprite Spinner { get; set; }

	// Token: 0x0600002E RID: 46 RVA: 0x00003A80 File Offset: 0x00001C80
	private void Update()
	{
		if (this.Spinner == null || Girls.CurrentGirl == null)
		{
			return;
		}
		if (Girls.CurrentGirl.GirlName != Balance.GirlName.Explora && this._rotateTime < 0f)
		{
			return;
		}
		if (this._image == null)
		{
			GameObject gameObject = new GameObject();
			this._image = gameObject.AddComponent<Image>();
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			this._image.sprite = this.Spinner;
			this._image.rectTransform.sizeDelta = new Vector2(50f, 50f);
			this._image.raycastTarget = false;
			gameObject.SetActive(false);
			this._unlocked = base.transform.Find("Unlocked").GetComponent<Button>();
		}
		if (!this._unlocked.gameObject.activeInHierarchy)
		{
			return;
		}
		this._currentTime -= Time.deltaTime;
		if (this._currentTime < 0f)
		{
			this._currentTime = (float)UnityEngine.Random.RandomRange(20, 60);
			this._rotateTime = (float)UnityEngine.Random.RandomRange(3, 10);
			this._image.gameObject.SetActive(true);
			this._unlocked.colors = new ColorBlock
			{
				normalColor = new Color(1f, 1f, 1f, 0.5f),
				disabledColor = new Color(0.784f, 0.784f, 0.784f, 0.5f),
				highlightedColor = new Color(0.96f, 0.96f, 0.96f, 0.5f),
				pressedColor = new Color(0.784f, 0.784f, 0.784f, 0.5f),
				colorMultiplier = 1f
			};
		}
		if (this._rotateTime > 0f)
		{
			if (Girls.CurrentGirl.GirlName != Balance.GirlName.Explora)
			{
				this._rotateTime = -1f;
			}
			this._rotateTime -= Time.deltaTime;
			if (this._rotateTime < 0f)
			{
				this._image.gameObject.SetActive(false);
				this._unlocked.colors = new ColorBlock
				{
					normalColor = new Color(1f, 1f, 1f, 1f),
					disabledColor = new Color(0.784f, 0.784f, 0.784f, 0.5f),
					highlightedColor = new Color(0.96f, 0.96f, 0.96f, 1f),
					pressedColor = new Color(0.784f, 0.784f, 0.784f, 1f),
					colorMultiplier = 1f
				};
			}
			else
			{
				this._image.rectTransform.localEulerAngles = new Vector3(0f, 0f, Mathf.Round(this._rotateTime * 120f / 30f) * 30f);
			}
		}
	}

	// Token: 0x04000018 RID: 24
	private Image _image;

	// Token: 0x04000019 RID: 25
	private float _currentTime;

	// Token: 0x0400001A RID: 26
	private float _rotateTime;

	// Token: 0x0400001B RID: 27
	private Button _unlocked;
}
