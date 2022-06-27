using System;
using UnityEngine;

// Token: 0x020000D2 RID: 210
[Serializable]
public class AudioData : EventDataContainer
{
	// Token: 0x060004BC RID: 1212 RVA: 0x00026538 File Offset: 0x00024738
	public override void Deserialize(Transform rootTransform, Transform targetTransform, EventDataContainer dataContainer)
	{
		if (!(dataContainer is AudioData))
		{
			Debug.LogError("Not an actual AudioDataContainer");
			return;
		}
		AudioData audioData = (AudioData)dataContainer;
		if (audioData == null)
		{
			Debug.LogError("AudioData is null");
			return;
		}
		ButtonAudio buttonAudio = targetTransform.gameObject.GetComponent<ButtonAudio>();
		if (buttonAudio == null)
		{
			buttonAudio = targetTransform.gameObject.AddComponent<ButtonAudio>();
		}
		Audio component = GameState.CurrentState.GetComponent<Audio>();
		buttonAudio.OnMouseClick = component.GetAudioClip(audioData.OnMouseClick);
		buttonAudio.OnMouseDown = component.GetAudioClip(audioData.OnMouseDown);
		buttonAudio.OnMouseOver = component.GetAudioClip(audioData.OnMouseOver);
	}

	// Token: 0x040004DA RID: 1242
	public Audio.AudioFile OnMouseClick;

	// Token: 0x040004DB RID: 1243
	public Audio.AudioFile OnMouseOver;

	// Token: 0x040004DC RID: 1244
	public Audio.AudioFile OnMouseDown;
}
