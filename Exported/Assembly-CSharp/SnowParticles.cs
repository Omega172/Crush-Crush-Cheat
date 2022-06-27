using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class SnowParticles : MonoBehaviour
{
	// Token: 0x06000024 RID: 36 RVA: 0x00003614 File Offset: 0x00001814
	private void Start()
	{
		if (this.ParticleRenderer != null)
		{
			this.ParticleRenderer.sortingLayerName = "Particle";
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003638 File Offset: 0x00001838
	private void ParticlePull()
	{
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[this.ParticleSystem.particleCount + 1];
		int particles = this.ParticleSystem.GetParticles(array);
		float num = Time.deltaTime * 20f;
		float time = Time.time;
		for (int i = 0; i < particles; i++)
		{
			array[i].position = new Vector3(array[i].position.x - Mathf.Sin(time + (float)i) * num, array[i].position.y, array[i].position.z);
			if (array[i].position.y < -650f)
			{
				array[i].lifetime = 0f;
			}
		}
		this.ParticleSystem.SetParticles(array, particles);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003730 File Offset: 0x00001930
	private void Update()
	{
		if (this.ParticleSystem == null)
		{
			return;
		}
		if (this.ParticleSystem.gameObject.activeInHierarchy != !Settings.ParticlesDisabled)
		{
			this.ParticleSystem.gameObject.SetActive(!Settings.ParticlesDisabled);
		}
		if (Settings.ParticlesDisabled)
		{
			return;
		}
		this.ParticlePull();
	}

	// Token: 0x04000012 RID: 18
	public ParticleSystemRenderer ParticleRenderer;

	// Token: 0x04000013 RID: 19
	public ParticleSystem ParticleSystem;
}
