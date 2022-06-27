using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class ButtonAudioData : EventDataContainerController
{
	// Token: 0x060004BE RID: 1214 RVA: 0x000265E0 File Offset: 0x000247E0
	public override string GetDataContainerType()
	{
		return typeof(AudioData).ToString();
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000265F4 File Offset: 0x000247F4
	public override EventDataContainer GetDataContainer()
	{
		return this.audioData;
	}

	// Token: 0x040004DD RID: 1245
	[SerializeField]
	private AudioData audioData;
}
