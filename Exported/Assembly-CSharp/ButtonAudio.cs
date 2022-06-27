using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200012F RID: 303
public class ButtonAudio : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler, IPointerClickHandler
{
	// Token: 0x060007EC RID: 2028 RVA: 0x0004AC88 File Offset: 0x00048E88
	public void Start()
	{
		this.buttonComponent = base.GetComponent<Button>();
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0004AC98 File Offset: 0x00048E98
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.buttonComponent != null && !this.buttonComponent.interactable)
		{
			return;
		}
		if (this.OnMouseOver != null)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(this.OnMouseOver);
		}
		if (this.OnMouseOverAlt != Audio.AudioFile.None)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(this.OnMouseOverAlt);
		}
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x0004AD10 File Offset: 0x00048F10
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.buttonComponent != null && !this.buttonComponent.interactable)
		{
			return;
		}
		if (this.OnMouseClick != null)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(this.OnMouseClick);
		}
		if (this.OnMouseClickAlt != Audio.AudioFile.None)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(this.OnMouseClickAlt);
		}
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0004AD88 File Offset: 0x00048F88
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.buttonComponent != null && !this.buttonComponent.interactable)
		{
			return;
		}
		if (this.OnMouseDown != null)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(this.OnMouseDown);
		}
		if (this.OnMouseDownAlt != Audio.AudioFile.None)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(this.OnMouseDownAlt);
		}
	}

	// Token: 0x04000871 RID: 2161
	public AudioClip OnMouseClick;

	// Token: 0x04000872 RID: 2162
	public AudioClip OnMouseOver;

	// Token: 0x04000873 RID: 2163
	public AudioClip OnMouseDown;

	// Token: 0x04000874 RID: 2164
	[Header("Optional")]
	public Audio.AudioFile OnMouseClickAlt;

	// Token: 0x04000875 RID: 2165
	public Audio.AudioFile OnMouseOverAlt;

	// Token: 0x04000876 RID: 2166
	public Audio.AudioFile OnMouseDownAlt;

	// Token: 0x04000877 RID: 2167
	private Button buttonComponent;
}
