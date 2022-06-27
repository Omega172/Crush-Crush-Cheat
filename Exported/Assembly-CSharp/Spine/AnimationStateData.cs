using System;
using System.Collections.Generic;

namespace Spine
{
	// Token: 0x0200018B RID: 395
	public class AnimationStateData
	{
		// Token: 0x06000BA7 RID: 2983 RVA: 0x0005A718 File Offset: 0x00058918
		public AnimationStateData(SkeletonData skeletonData)
		{
			if (skeletonData == null)
			{
				throw new ArgumentException("skeletonData cannot be null.", "skeletonData");
			}
			this.skeletonData = skeletonData;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0005A750 File Offset: 0x00058950
		public SkeletonData SkeletonData
		{
			get
			{
				return this.skeletonData;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0005A758 File Offset: 0x00058958
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x0005A760 File Offset: 0x00058960
		public float DefaultMix
		{
			get
			{
				return this.defaultMix;
			}
			set
			{
				this.defaultMix = value;
			}
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0005A76C File Offset: 0x0005896C
		public void SetMix(string fromName, string toName, float duration)
		{
			Animation animation = this.skeletonData.FindAnimation(fromName);
			if (animation == null)
			{
				throw new ArgumentException("Animation not found: " + fromName, "fromName");
			}
			Animation animation2 = this.skeletonData.FindAnimation(toName);
			if (animation2 == null)
			{
				throw new ArgumentException("Animation not found: " + toName, "toName");
			}
			this.SetMix(animation, animation2, duration);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0005A7D4 File Offset: 0x000589D4
		public void SetMix(Animation from, Animation to, float duration)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from", "from cannot be null.");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to", "to cannot be null.");
			}
			AnimationStateData.AnimationPair key = new AnimationStateData.AnimationPair(from, to);
			this.animationToMixTime.Remove(key);
			this.animationToMixTime.Add(key, duration);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0005A830 File Offset: 0x00058A30
		public float GetMix(Animation from, Animation to)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from", "from cannot be null.");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to", "to cannot be null.");
			}
			AnimationStateData.AnimationPair key = new AnimationStateData.AnimationPair(from, to);
			float result;
			if (this.animationToMixTime.TryGetValue(key, out result))
			{
				return result;
			}
			return this.defaultMix;
		}

		// Token: 0x04000AC6 RID: 2758
		internal SkeletonData skeletonData;

		// Token: 0x04000AC7 RID: 2759
		private readonly Dictionary<AnimationStateData.AnimationPair, float> animationToMixTime = new Dictionary<AnimationStateData.AnimationPair, float>(AnimationStateData.AnimationPairComparer.Instance);

		// Token: 0x04000AC8 RID: 2760
		internal float defaultMix;

		// Token: 0x0200018C RID: 396
		public struct AnimationPair
		{
			// Token: 0x06000BAE RID: 2990 RVA: 0x0005A890 File Offset: 0x00058A90
			public AnimationPair(Animation a1, Animation a2)
			{
				this.a1 = a1;
				this.a2 = a2;
			}

			// Token: 0x06000BAF RID: 2991 RVA: 0x0005A8A0 File Offset: 0x00058AA0
			public override string ToString()
			{
				return this.a1.name + "->" + this.a2.name;
			}

			// Token: 0x04000AC9 RID: 2761
			public readonly Animation a1;

			// Token: 0x04000ACA RID: 2762
			public readonly Animation a2;
		}

		// Token: 0x0200018D RID: 397
		public class AnimationPairComparer : IEqualityComparer<AnimationStateData.AnimationPair>
		{
			// Token: 0x06000BB2 RID: 2994 RVA: 0x0005A8E4 File Offset: 0x00058AE4
			bool IEqualityComparer<AnimationStateData.AnimationPair>.Equals(AnimationStateData.AnimationPair x, AnimationStateData.AnimationPair y)
			{
				return object.ReferenceEquals(x.a1, y.a1) && object.ReferenceEquals(x.a2, y.a2);
			}

			// Token: 0x06000BB3 RID: 2995 RVA: 0x0005A920 File Offset: 0x00058B20
			int IEqualityComparer<AnimationStateData.AnimationPair>.GetHashCode(AnimationStateData.AnimationPair obj)
			{
				int hashCode = obj.a1.GetHashCode();
				return (hashCode << 5) + hashCode ^ obj.a2.GetHashCode();
			}

			// Token: 0x04000ACB RID: 2763
			public static readonly AnimationStateData.AnimationPairComparer Instance = new AnimationStateData.AnimationPairComparer();
		}
	}
}
