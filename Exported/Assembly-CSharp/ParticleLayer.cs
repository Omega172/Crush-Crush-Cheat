using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class ParticleLayer : MonoBehaviour
{
	// Token: 0x060005A2 RID: 1442 RVA: 0x0002DB80 File Offset: 0x0002BD80
	private void Start()
	{
		base.GetComponent<ParticleSystemRenderer>().sortingLayerName = ((!this.AlbumParticles) ? "Particle" : "Front UI Particle");
		this.ps = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0002DBC0 File Offset: 0x0002BDC0
	private IEnumerator ParticleWait()
	{
		this.waitCalled = true;
		yield return new WaitForSeconds((float)this.TimeUntilPull);
		this.TimeUntilPull = 0;
		Debug.Log("TimeUntilPull set to 0");
		yield break;
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0002DBDC File Offset: 0x0002BDDC
	private void ParticlePull()
	{
		if (this.MagnetPoint == null)
		{
			return;
		}
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[this.ps.particleCount + 1];
		int particles = this.ps.GetParticles(array);
		Vector3 b = this.MagnetPoint.position - base.transform.position;
		for (int i = 0; i < particles; i++)
		{
			array[i].position = Vector3.Lerp(array[i].position, b, Mathf.SmoothStep(0f, 2f, 1f - array[i].lifetime / array[i].startLifetime));
			if ((array[i].position - b).magnitude <= 10f)
			{
				array[i].lifetime = 0f;
			}
		}
		this.ps.SetParticles(array, particles);
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0002DCD8 File Offset: 0x0002BED8
	private void Update()
	{
		if (this.ps.particleCount == 0)
		{
			return;
		}
		if (this.TimeUntilPull > 0 & !this.waitCalled)
		{
			Debug.Log("Coroutine called");
			base.StartCoroutine(this.ParticleWait());
		}
		else if (this.TimeUntilPull == 0)
		{
			this.ParticlePull();
		}
	}

	// Token: 0x0400057E RID: 1406
	public bool AlbumParticles;

	// Token: 0x0400057F RID: 1407
	private ParticleSystem ps;

	// Token: 0x04000580 RID: 1408
	public float PullDistance = 200f;

	// Token: 0x04000581 RID: 1409
	public Transform MagnetPoint;

	// Token: 0x04000582 RID: 1410
	public int TimeUntilPull;

	// Token: 0x04000583 RID: 1411
	private bool waitCalled;
}
