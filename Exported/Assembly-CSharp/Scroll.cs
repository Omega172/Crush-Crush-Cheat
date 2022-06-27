using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000139 RID: 313
public class Scroll : MonoBehaviour
{
	// Token: 0x06000814 RID: 2068 RVA: 0x0004B4CC File Offset: 0x000496CC
	private void Start()
	{
		base.GetComponent<Scrollbar>().size = 0f;
		base.GetComponent<Scrollbar>().value = 1f;
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0004B4FC File Offset: 0x000496FC
	private void Update()
	{
		base.GetComponent<Scrollbar>().size = 0f;
	}
}
