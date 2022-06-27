using System;

namespace Spine
{
	// Token: 0x02000185 RID: 389
	public class TrackEntry : Pool<TrackEntry>.IPoolable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000B56 RID: 2902 RVA: 0x00059EEC File Offset: 0x000580EC
		// (remove) Token: 0x06000B57 RID: 2903 RVA: 0x00059F08 File Offset: 0x00058108
		public event AnimationState.TrackEntryDelegate Start;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000B58 RID: 2904 RVA: 0x00059F24 File Offset: 0x00058124
		// (remove) Token: 0x06000B59 RID: 2905 RVA: 0x00059F40 File Offset: 0x00058140
		public event AnimationState.TrackEntryDelegate Interrupt;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000B5A RID: 2906 RVA: 0x00059F5C File Offset: 0x0005815C
		// (remove) Token: 0x06000B5B RID: 2907 RVA: 0x00059F78 File Offset: 0x00058178
		public event AnimationState.TrackEntryDelegate End;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000B5C RID: 2908 RVA: 0x00059F94 File Offset: 0x00058194
		// (remove) Token: 0x06000B5D RID: 2909 RVA: 0x00059FB0 File Offset: 0x000581B0
		public event AnimationState.TrackEntryDelegate Dispose;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000B5E RID: 2910 RVA: 0x00059FCC File Offset: 0x000581CC
		// (remove) Token: 0x06000B5F RID: 2911 RVA: 0x00059FE8 File Offset: 0x000581E8
		public event AnimationState.TrackEntryDelegate Complete;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000B60 RID: 2912 RVA: 0x0005A004 File Offset: 0x00058204
		// (remove) Token: 0x06000B61 RID: 2913 RVA: 0x0005A020 File Offset: 0x00058220
		public event AnimationState.TrackEntryEventDelegate Event;

		// Token: 0x06000B62 RID: 2914 RVA: 0x0005A03C File Offset: 0x0005823C
		internal void OnStart()
		{
			if (this.Start != null)
			{
				this.Start(this);
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0005A058 File Offset: 0x00058258
		internal void OnInterrupt()
		{
			if (this.Interrupt != null)
			{
				this.Interrupt(this);
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0005A074 File Offset: 0x00058274
		internal void OnEnd()
		{
			if (this.End != null)
			{
				this.End(this);
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0005A090 File Offset: 0x00058290
		internal void OnDispose()
		{
			if (this.Dispose != null)
			{
				this.Dispose(this);
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0005A0AC File Offset: 0x000582AC
		internal void OnComplete()
		{
			if (this.Complete != null)
			{
				this.Complete(this);
			}
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0005A0C8 File Offset: 0x000582C8
		internal void OnEvent(Event e)
		{
			if (this.Event != null)
			{
				this.Event(this, e);
			}
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0005A0E4 File Offset: 0x000582E4
		public void Reset()
		{
			this.next = null;
			this.mixingFrom = null;
			this.mixingTo = null;
			this.animation = null;
			this.Start = null;
			this.Interrupt = null;
			this.End = null;
			this.Dispose = null;
			this.Complete = null;
			this.Event = null;
			this.timelineMode.Clear(true);
			this.timelineHoldMix.Clear(true);
			this.timelinesRotation.Clear(true);
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0005A15C File Offset: 0x0005835C
		public int TrackIndex
		{
			get
			{
				return this.trackIndex;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0005A164 File Offset: 0x00058364
		public Animation Animation
		{
			get
			{
				return this.animation;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0005A16C File Offset: 0x0005836C
		// (set) Token: 0x06000B6C RID: 2924 RVA: 0x0005A174 File Offset: 0x00058374
		public bool Loop
		{
			get
			{
				return this.loop;
			}
			set
			{
				this.loop = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0005A180 File Offset: 0x00058380
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0005A188 File Offset: 0x00058388
		public float Delay
		{
			get
			{
				return this.delay;
			}
			set
			{
				this.delay = value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x0005A194 File Offset: 0x00058394
		// (set) Token: 0x06000B70 RID: 2928 RVA: 0x0005A19C File Offset: 0x0005839C
		public float TrackTime
		{
			get
			{
				return this.trackTime;
			}
			set
			{
				this.trackTime = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0005A1A8 File Offset: 0x000583A8
		// (set) Token: 0x06000B72 RID: 2930 RVA: 0x0005A1B0 File Offset: 0x000583B0
		public float TrackEnd
		{
			get
			{
				return this.trackEnd;
			}
			set
			{
				this.trackEnd = value;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0005A1BC File Offset: 0x000583BC
		// (set) Token: 0x06000B74 RID: 2932 RVA: 0x0005A1C4 File Offset: 0x000583C4
		public float AnimationStart
		{
			get
			{
				return this.animationStart;
			}
			set
			{
				this.animationStart = value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0005A1D0 File Offset: 0x000583D0
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x0005A1D8 File Offset: 0x000583D8
		public float AnimationEnd
		{
			get
			{
				return this.animationEnd;
			}
			set
			{
				this.animationEnd = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x0005A1E4 File Offset: 0x000583E4
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x0005A1EC File Offset: 0x000583EC
		public float AnimationLast
		{
			get
			{
				return this.animationLast;
			}
			set
			{
				this.animationLast = value;
				this.nextAnimationLast = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0005A1FC File Offset: 0x000583FC
		public float AnimationTime
		{
			get
			{
				if (!this.loop)
				{
					return Math.Min(this.trackTime + this.animationStart, this.animationEnd);
				}
				float num = this.animationEnd - this.animationStart;
				if (num == 0f)
				{
					return this.animationStart;
				}
				return this.trackTime % num + this.animationStart;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0005A25C File Offset: 0x0005845C
		// (set) Token: 0x06000B7B RID: 2939 RVA: 0x0005A264 File Offset: 0x00058464
		public float TimeScale
		{
			get
			{
				return this.timeScale;
			}
			set
			{
				this.timeScale = value;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0005A270 File Offset: 0x00058470
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0005A278 File Offset: 0x00058478
		public float Alpha
		{
			get
			{
				return this.alpha;
			}
			set
			{
				this.alpha = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0005A284 File Offset: 0x00058484
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0005A28C File Offset: 0x0005848C
		public float EventThreshold
		{
			get
			{
				return this.eventThreshold;
			}
			set
			{
				this.eventThreshold = value;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0005A298 File Offset: 0x00058498
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x0005A2A0 File Offset: 0x000584A0
		public float AttachmentThreshold
		{
			get
			{
				return this.attachmentThreshold;
			}
			set
			{
				this.attachmentThreshold = value;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0005A2AC File Offset: 0x000584AC
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0005A2B4 File Offset: 0x000584B4
		public float DrawOrderThreshold
		{
			get
			{
				return this.drawOrderThreshold;
			}
			set
			{
				this.drawOrderThreshold = value;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0005A2C0 File Offset: 0x000584C0
		public TrackEntry Next
		{
			get
			{
				return this.next;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0005A2C8 File Offset: 0x000584C8
		public bool IsComplete
		{
			get
			{
				return this.trackTime >= this.animationEnd - this.animationStart;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0005A2E4 File Offset: 0x000584E4
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x0005A2EC File Offset: 0x000584EC
		public float MixTime
		{
			get
			{
				return this.mixTime;
			}
			set
			{
				this.mixTime = value;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0005A2F8 File Offset: 0x000584F8
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x0005A300 File Offset: 0x00058500
		public float MixDuration
		{
			get
			{
				return this.mixDuration;
			}
			set
			{
				this.mixDuration = value;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0005A30C File Offset: 0x0005850C
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x0005A314 File Offset: 0x00058514
		public MixBlend MixBlend
		{
			get
			{
				return this.mixBlend;
			}
			set
			{
				this.mixBlend = value;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0005A320 File Offset: 0x00058520
		public TrackEntry MixingFrom
		{
			get
			{
				return this.mixingFrom;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0005A328 File Offset: 0x00058528
		public TrackEntry MixingTo
		{
			get
			{
				return this.mixingTo;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0005A330 File Offset: 0x00058530
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x0005A338 File Offset: 0x00058538
		public bool HoldPrevious
		{
			get
			{
				return this.holdPrevious;
			}
			set
			{
				this.holdPrevious = value;
			}
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0005A344 File Offset: 0x00058544
		public void ResetRotationDirections()
		{
			this.timelinesRotation.Clear(true);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0005A354 File Offset: 0x00058554
		public override string ToString()
		{
			return (this.animation != null) ? this.animation.name : "<none>";
		}

		// Token: 0x04000A91 RID: 2705
		internal Animation animation;

		// Token: 0x04000A92 RID: 2706
		internal TrackEntry next;

		// Token: 0x04000A93 RID: 2707
		internal TrackEntry mixingFrom;

		// Token: 0x04000A94 RID: 2708
		internal TrackEntry mixingTo;

		// Token: 0x04000A95 RID: 2709
		internal int trackIndex;

		// Token: 0x04000A96 RID: 2710
		internal bool loop;

		// Token: 0x04000A97 RID: 2711
		internal bool holdPrevious;

		// Token: 0x04000A98 RID: 2712
		internal float eventThreshold;

		// Token: 0x04000A99 RID: 2713
		internal float attachmentThreshold;

		// Token: 0x04000A9A RID: 2714
		internal float drawOrderThreshold;

		// Token: 0x04000A9B RID: 2715
		internal float animationStart;

		// Token: 0x04000A9C RID: 2716
		internal float animationEnd;

		// Token: 0x04000A9D RID: 2717
		internal float animationLast;

		// Token: 0x04000A9E RID: 2718
		internal float nextAnimationLast;

		// Token: 0x04000A9F RID: 2719
		internal float delay;

		// Token: 0x04000AA0 RID: 2720
		internal float trackTime;

		// Token: 0x04000AA1 RID: 2721
		internal float trackLast;

		// Token: 0x04000AA2 RID: 2722
		internal float nextTrackLast;

		// Token: 0x04000AA3 RID: 2723
		internal float trackEnd;

		// Token: 0x04000AA4 RID: 2724
		internal float timeScale = 1f;

		// Token: 0x04000AA5 RID: 2725
		internal float alpha;

		// Token: 0x04000AA6 RID: 2726
		internal float mixTime;

		// Token: 0x04000AA7 RID: 2727
		internal float mixDuration;

		// Token: 0x04000AA8 RID: 2728
		internal float interruptAlpha;

		// Token: 0x04000AA9 RID: 2729
		internal float totalAlpha;

		// Token: 0x04000AAA RID: 2730
		internal MixBlend mixBlend = MixBlend.Replace;

		// Token: 0x04000AAB RID: 2731
		internal readonly ExposedList<int> timelineMode = new ExposedList<int>();

		// Token: 0x04000AAC RID: 2732
		internal readonly ExposedList<TrackEntry> timelineHoldMix = new ExposedList<TrackEntry>();

		// Token: 0x04000AAD RID: 2733
		internal readonly ExposedList<float> timelinesRotation = new ExposedList<float>();
	}
}
