using System;
using Spine;
using Spine.Unity;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class PeanutLofi : MonoBehaviour
{
	// Token: 0x0600003C RID: 60 RVA: 0x000041D8 File Offset: 0x000023D8
	private void Start()
	{
		this._animation = base.GetComponent<SkeletonGraphic>();
		this._currentAnimationIndex = UnityEngine.Random.Range(0, this._animationNames.Length);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00004208 File Offset: 0x00002408
	private void OnEnable()
	{
		this._animationLength = 0f;
		if (this._animation != null)
		{
			this._animation.AnimationState.ClearTrack(2);
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00004238 File Offset: 0x00002438
	private void Update()
	{
		if (this._animationLength <= 0f)
		{
			this._animationLength = (float)UnityEngine.Random.Range(15, 40);
			this._animation.AnimationState.AddAnimation(2, this._animationNames[this._currentAnimationIndex], this._currentAnimationIndex == 2, 0f);
			this._animation.AnimationState.AddAnimation(2, "Breathe 2", false, (this._currentAnimationIndex != 2) ? 0f : (this._animationLength - 5f));
			this._currentAnimationIndex = (this._currentAnimationIndex + 1) % this._animationNames.Length;
		}
		else
		{
			TrackEntry current = this._animation.AnimationState.GetCurrent(2);
			if (current != null && current.TrackTime >= current.Animation.Duration && current.Animation.Name == "Breathe 2")
			{
				current.Delay = 3f;
				current.TrackTime = 0f;
			}
			this._animationLength -= Time.deltaTime;
		}
	}

	// Token: 0x0400002A RID: 42
	private SkeletonGraphic _animation;

	// Token: 0x0400002B RID: 43
	private string[] _animationNames = new string[]
	{
		"Arm Writes 1 Slowed",
		"Arm Writes 2 Slowed",
		"Head Bobs",
		"Reading Page"
	};

	// Token: 0x0400002C RID: 44
	private float _animationLength;

	// Token: 0x0400002D RID: 45
	private int _currentAnimationIndex;
}
