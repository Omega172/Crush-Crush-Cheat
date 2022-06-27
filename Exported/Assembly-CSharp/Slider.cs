using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200013A RID: 314
public class Slider : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x06000818 RID: 2072 RVA: 0x0004B524 File Offset: 0x00049724
	private void Start()
	{
		if (base.transform.parent.gameObject.name.Contains("Music"))
		{
			this.adjustMusic = true;
		}
		if (base.transform.parent.gameObject.name.Contains("SFX"))
		{
			this.adjustSound = true;
		}
		this.sliderFill = base.transform.parent.Find("Slider Fill").GetComponent<RectTransform>();
		this.sliderFill.GetComponent<Image>().fillAmount = this.Value;
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0004B5C0 File Offset: 0x000497C0
	public void OnPointerDown(PointerEventData eventData)
	{
		Vector2 a;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.sliderFill, eventData.position, Camera.main, out a);
		this.pointerDeltaX = (a - new Vector2(base.transform.localPosition.x, base.transform.localPosition.y)).x;
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0004B628 File Offset: 0x00049828
	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.adjustSound)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayWithVolume(this.Value);
		}
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0004B658 File Offset: 0x00049858
	public void OnDrag(PointerEventData eventData)
	{
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.sliderFill, eventData.position, Camera.main, out vector);
		float x = Mathf.Min(global::Slider.width - this._deviationX, Mathf.Max(-this._deviationX, vector.x - this.pointerDeltaX));
		base.transform.localPosition = new Vector3(x, base.transform.localPosition.y, 0f);
		this.sliderFill.GetComponent<Image>().fillAmount = this.Value;
		if (this.adjustMusic)
		{
			AudioSource music = GameObject.Find("Canvas").GetComponent<Audio>().Music;
			music.volume = this.Value;
			if (this.Value == 0f && music.isPlaying)
			{
				music.Pause();
			}
			else if (this.Value != 0f && !music.isPlaying)
			{
				music.Play();
			}
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600081C RID: 2076 RVA: 0x0004B75C File Offset: 0x0004995C
	// (set) Token: 0x0600081D RID: 2077 RVA: 0x0004B78C File Offset: 0x0004998C
	public float Value
	{
		get
		{
			return (base.transform.localPosition.x + this._deviationX) / global::Slider.width;
		}
		set
		{
			float num = Mathf.Max(0f, Mathf.Min(1f, value));
			base.transform.localPosition = new Vector3(num * global::Slider.width - this._deviationX, 0f, 0f);
			base.transform.parent.Find("Slider Fill").GetComponent<Image>().fillAmount = num;
		}
	}

	// Token: 0x04000887 RID: 2183
	private bool adjustMusic;

	// Token: 0x04000888 RID: 2184
	private bool adjustSound;

	// Token: 0x04000889 RID: 2185
	[SerializeField]
	[Tooltip("Half the difference between this GO and an optional child containing the graphic, centered.")]
	private float _deviationX;

	// Token: 0x0400088A RID: 2186
	private float pointerDeltaX;

	// Token: 0x0400088B RID: 2187
	private RectTransform sliderFill;

	// Token: 0x0400088C RID: 2188
	private static float width = 100f;
}
