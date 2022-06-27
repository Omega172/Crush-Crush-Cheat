using System;
using System.Collections.Generic;
using System.Text;

namespace Spine
{
	// Token: 0x02000184 RID: 388
	public class AnimationState
	{
		// Token: 0x06000B1F RID: 2847 RVA: 0x000582DC File Offset: 0x000564DC
		public AnimationState(AnimationStateData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			this.data = data;
			this.queue = new EventQueue(this, delegate()
			{
				this.animationsChanged = true;
			}, this.trackEntryPool);
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000B21 RID: 2849 RVA: 0x00058384 File Offset: 0x00056584
		// (remove) Token: 0x06000B22 RID: 2850 RVA: 0x000583A0 File Offset: 0x000565A0
		public event AnimationState.TrackEntryDelegate Start;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000B23 RID: 2851 RVA: 0x000583BC File Offset: 0x000565BC
		// (remove) Token: 0x06000B24 RID: 2852 RVA: 0x000583D8 File Offset: 0x000565D8
		public event AnimationState.TrackEntryDelegate Interrupt;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000B25 RID: 2853 RVA: 0x000583F4 File Offset: 0x000565F4
		// (remove) Token: 0x06000B26 RID: 2854 RVA: 0x00058410 File Offset: 0x00056610
		public event AnimationState.TrackEntryDelegate End;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000B27 RID: 2855 RVA: 0x0005842C File Offset: 0x0005662C
		// (remove) Token: 0x06000B28 RID: 2856 RVA: 0x00058448 File Offset: 0x00056648
		public event AnimationState.TrackEntryDelegate Dispose;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000B29 RID: 2857 RVA: 0x00058464 File Offset: 0x00056664
		// (remove) Token: 0x06000B2A RID: 2858 RVA: 0x00058480 File Offset: 0x00056680
		public event AnimationState.TrackEntryDelegate Complete;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000B2B RID: 2859 RVA: 0x0005849C File Offset: 0x0005669C
		// (remove) Token: 0x06000B2C RID: 2860 RVA: 0x000584B8 File Offset: 0x000566B8
		public event AnimationState.TrackEntryEventDelegate Event;

		// Token: 0x06000B2D RID: 2861 RVA: 0x000584D4 File Offset: 0x000566D4
		internal void OnStart(TrackEntry entry)
		{
			if (this.Start != null)
			{
				this.Start(entry);
			}
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000584F0 File Offset: 0x000566F0
		internal void OnInterrupt(TrackEntry entry)
		{
			if (this.Interrupt != null)
			{
				this.Interrupt(entry);
			}
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0005850C File Offset: 0x0005670C
		internal void OnEnd(TrackEntry entry)
		{
			if (this.End != null)
			{
				this.End(entry);
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00058528 File Offset: 0x00056728
		internal void OnDispose(TrackEntry entry)
		{
			if (this.Dispose != null)
			{
				this.Dispose(entry);
			}
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00058544 File Offset: 0x00056744
		internal void OnComplete(TrackEntry entry)
		{
			if (this.Complete != null)
			{
				this.Complete(entry);
			}
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00058560 File Offset: 0x00056760
		internal void OnEvent(TrackEntry entry, Event e)
		{
			if (this.Event != null)
			{
				this.Event(entry, e);
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0005857C File Offset: 0x0005677C
		public void AssignEventSubscribersFrom(AnimationState src)
		{
			this.Event = src.Event;
			this.Start = src.Start;
			this.Interrupt = src.Interrupt;
			this.End = src.End;
			this.Dispose = src.Dispose;
			this.Complete = src.Complete;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x000585D4 File Offset: 0x000567D4
		public void AddEventSubscribersFrom(AnimationState src)
		{
			this.Event = (AnimationState.TrackEntryEventDelegate)Delegate.Combine(this.Event, src.Event);
			this.Start = (AnimationState.TrackEntryDelegate)Delegate.Combine(this.Start, src.Start);
			this.Interrupt = (AnimationState.TrackEntryDelegate)Delegate.Combine(this.Interrupt, src.Interrupt);
			this.End = (AnimationState.TrackEntryDelegate)Delegate.Combine(this.End, src.End);
			this.Dispose = (AnimationState.TrackEntryDelegate)Delegate.Combine(this.Dispose, src.Dispose);
			this.Complete = (AnimationState.TrackEntryDelegate)Delegate.Combine(this.Complete, src.Complete);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0005868C File Offset: 0x0005688C
		public void Update(float delta)
		{
			delta *= this.timeScale;
			TrackEntry[] items = this.tracks.Items;
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				TrackEntry trackEntry = items[i];
				if (trackEntry != null)
				{
					trackEntry.animationLast = trackEntry.nextAnimationLast;
					trackEntry.trackLast = trackEntry.nextTrackLast;
					float num = delta * trackEntry.timeScale;
					if (trackEntry.delay > 0f)
					{
						trackEntry.delay -= num;
						if (trackEntry.delay > 0f)
						{
							goto IL_1FF;
						}
						num = -trackEntry.delay;
						trackEntry.delay = 0f;
					}
					TrackEntry trackEntry2 = trackEntry.next;
					if (trackEntry2 != null)
					{
						float num2 = trackEntry.trackLast - trackEntry2.delay;
						if (num2 >= 0f)
						{
							trackEntry2.delay = 0f;
							trackEntry2.trackTime += ((trackEntry.timeScale != 0f) ? ((num2 / trackEntry.timeScale + delta) * trackEntry2.timeScale) : 0f);
							trackEntry.trackTime += num;
							this.SetCurrent(i, trackEntry2, true);
							while (trackEntry2.mixingFrom != null)
							{
								trackEntry2.mixTime += delta;
								trackEntry2 = trackEntry2.mixingFrom;
							}
							goto IL_1FF;
						}
					}
					else if (trackEntry.trackLast >= trackEntry.trackEnd && trackEntry.mixingFrom == null)
					{
						items[i] = null;
						this.queue.End(trackEntry);
						this.DisposeNext(trackEntry);
						goto IL_1FF;
					}
					if (trackEntry.mixingFrom != null && this.UpdateMixingFrom(trackEntry, delta))
					{
						TrackEntry mixingFrom = trackEntry.mixingFrom;
						trackEntry.mixingFrom = null;
						if (mixingFrom != null)
						{
							mixingFrom.mixingTo = null;
						}
						while (mixingFrom != null)
						{
							this.queue.End(mixingFrom);
							mixingFrom = mixingFrom.mixingFrom;
						}
					}
					trackEntry.trackTime += num;
				}
				IL_1FF:
				i++;
			}
			this.queue.Drain();
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000588B0 File Offset: 0x00056AB0
		private bool UpdateMixingFrom(TrackEntry to, float delta)
		{
			TrackEntry mixingFrom = to.mixingFrom;
			if (mixingFrom == null)
			{
				return true;
			}
			bool result = this.UpdateMixingFrom(mixingFrom, delta);
			mixingFrom.animationLast = mixingFrom.nextAnimationLast;
			mixingFrom.trackLast = mixingFrom.nextTrackLast;
			if (to.mixTime > 0f && to.mixTime >= to.mixDuration)
			{
				if (mixingFrom.totalAlpha == 0f || to.mixDuration == 0f)
				{
					to.mixingFrom = mixingFrom.mixingFrom;
					if (mixingFrom.mixingFrom != null)
					{
						mixingFrom.mixingFrom.mixingTo = to;
					}
					to.interruptAlpha = mixingFrom.interruptAlpha;
					this.queue.End(mixingFrom);
				}
				return result;
			}
			mixingFrom.trackTime += delta * mixingFrom.timeScale;
			to.mixTime += delta;
			return false;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00058990 File Offset: 0x00056B90
		public bool Apply(Skeleton skeleton)
		{
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			}
			if (this.animationsChanged)
			{
				this.AnimationsChanged();
			}
			ExposedList<Event> exposedList = this.events;
			bool result = false;
			TrackEntry[] items = this.tracks.Items;
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				TrackEntry trackEntry = items[i];
				if (trackEntry != null && trackEntry.delay <= 0f)
				{
					result = true;
					MixBlend mixBlend = (i != 0) ? trackEntry.mixBlend : MixBlend.First;
					float num = trackEntry.alpha;
					if (trackEntry.mixingFrom != null)
					{
						num *= this.ApplyMixingFrom(trackEntry, skeleton, mixBlend);
					}
					else if (trackEntry.trackTime >= trackEntry.trackEnd && trackEntry.next == null)
					{
						num = 0f;
					}
					float animationLast = trackEntry.animationLast;
					float animationTime = trackEntry.AnimationTime;
					int count2 = trackEntry.animation.timelines.Count;
					ExposedList<Timeline> timelines = trackEntry.animation.timelines;
					Timeline[] items2 = timelines.Items;
					if ((i == 0 && num == 1f) || mixBlend == MixBlend.Add)
					{
						for (int j = 0; j < count2; j++)
						{
							Timeline timeline = items2[j];
							if (timeline is AttachmentTimeline)
							{
								this.ApplyAttachmentTimeline((AttachmentTimeline)timeline, skeleton, animationTime, mixBlend, true);
							}
							else
							{
								timeline.Apply(skeleton, animationLast, animationTime, exposedList, num, mixBlend, MixDirection.In);
							}
						}
					}
					else
					{
						int[] items3 = trackEntry.timelineMode.Items;
						bool flag = trackEntry.timelinesRotation.Count != count2 << 1;
						if (flag)
						{
							trackEntry.timelinesRotation.Resize(timelines.Count << 1);
						}
						float[] items4 = trackEntry.timelinesRotation.Items;
						for (int k = 0; k < count2; k++)
						{
							Timeline timeline2 = items2[k];
							MixBlend blend = (items3[k] != 0) ? MixBlend.Setup : mixBlend;
							RotateTimeline rotateTimeline = timeline2 as RotateTimeline;
							if (rotateTimeline != null)
							{
								AnimationState.ApplyRotateTimeline(rotateTimeline, skeleton, animationTime, num, blend, items4, k << 1, flag);
							}
							else if (timeline2 is AttachmentTimeline)
							{
								this.ApplyAttachmentTimeline((AttachmentTimeline)timeline2, skeleton, animationTime, mixBlend, true);
							}
							else
							{
								timeline2.Apply(skeleton, animationLast, animationTime, exposedList, num, blend, MixDirection.In);
							}
						}
					}
					this.QueueEvents(trackEntry, animationTime);
					exposedList.Clear(false);
					trackEntry.nextAnimationLast = animationTime;
					trackEntry.nextTrackLast = trackEntry.trackTime;
				}
				i++;
			}
			int num2 = this.unkeyedState + 1;
			Slot[] items5 = skeleton.slots.Items;
			int l = 0;
			int count3 = skeleton.slots.Count;
			while (l < count3)
			{
				Slot slot = items5[l];
				if (slot.attachmentState == num2)
				{
					string attachmentName = slot.data.attachmentName;
					slot.Attachment = ((attachmentName != null) ? skeleton.GetAttachment(slot.data.index, attachmentName) : null);
				}
				l++;
			}
			this.unkeyedState += 2;
			this.queue.Drain();
			return result;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00058CDC File Offset: 0x00056EDC
		private float ApplyMixingFrom(TrackEntry to, Skeleton skeleton, MixBlend blend)
		{
			TrackEntry mixingFrom = to.mixingFrom;
			if (mixingFrom.mixingFrom != null)
			{
				this.ApplyMixingFrom(mixingFrom, skeleton, blend);
			}
			float num;
			if (to.mixDuration == 0f)
			{
				num = 1f;
				if (blend == MixBlend.First)
				{
					blend = MixBlend.Setup;
				}
			}
			else
			{
				num = to.mixTime / to.mixDuration;
				if (num > 1f)
				{
					num = 1f;
				}
				if (blend != MixBlend.First)
				{
					blend = mixingFrom.mixBlend;
				}
			}
			ExposedList<Event> exposedList = (num >= mixingFrom.eventThreshold) ? null : this.events;
			bool attachments = num < mixingFrom.attachmentThreshold;
			bool flag = num < mixingFrom.drawOrderThreshold;
			float animationLast = mixingFrom.animationLast;
			float animationTime = mixingFrom.AnimationTime;
			ExposedList<Timeline> timelines = mixingFrom.animation.timelines;
			int count = timelines.Count;
			Timeline[] items = timelines.Items;
			float num2 = mixingFrom.alpha * to.interruptAlpha;
			float num3 = num2 * (1f - num);
			if (blend == MixBlend.Add)
			{
				for (int i = 0; i < count; i++)
				{
					items[i].Apply(skeleton, animationLast, animationTime, exposedList, num3, blend, MixDirection.Out);
				}
			}
			else
			{
				int[] items2 = mixingFrom.timelineMode.Items;
				TrackEntry[] items3 = mixingFrom.timelineHoldMix.Items;
				bool flag2 = mixingFrom.timelinesRotation.Count != count << 1;
				if (flag2)
				{
					mixingFrom.timelinesRotation.Resize(timelines.Count << 1);
				}
				float[] items4 = mixingFrom.timelinesRotation.Items;
				mixingFrom.totalAlpha = 0f;
				int j = 0;
				while (j < count)
				{
					Timeline timeline = items[j];
					MixDirection direction = MixDirection.Out;
					MixBlend mixBlend;
					float num4;
					switch (items2[j])
					{
					case 0:
						if (flag || !(timeline is DrawOrderTimeline))
						{
							mixBlend = blend;
							num4 = num3;
							goto IL_232;
						}
						break;
					case 1:
						mixBlend = MixBlend.Setup;
						num4 = num3;
						goto IL_232;
					case 2:
						mixBlend = blend;
						num4 = num2;
						goto IL_232;
					case 3:
						mixBlend = MixBlend.Setup;
						num4 = num2;
						goto IL_232;
					default:
					{
						mixBlend = MixBlend.Setup;
						TrackEntry trackEntry = items3[j];
						num4 = num2 * Math.Max(0f, 1f - trackEntry.mixTime / trackEntry.mixDuration);
						goto IL_232;
					}
					}
					IL_2C0:
					j++;
					continue;
					IL_232:
					mixingFrom.totalAlpha += num4;
					RotateTimeline rotateTimeline = timeline as RotateTimeline;
					if (rotateTimeline != null)
					{
						AnimationState.ApplyRotateTimeline(rotateTimeline, skeleton, animationTime, num4, mixBlend, items4, j << 1, flag2);
						goto IL_2C0;
					}
					if (timeline is AttachmentTimeline)
					{
						this.ApplyAttachmentTimeline((AttachmentTimeline)timeline, skeleton, animationTime, mixBlend, attachments);
						goto IL_2C0;
					}
					if (flag && timeline is DrawOrderTimeline && mixBlend == MixBlend.Setup)
					{
						direction = MixDirection.In;
					}
					timeline.Apply(skeleton, animationLast, animationTime, exposedList, num4, mixBlend, direction);
					goto IL_2C0;
				}
			}
			if (to.mixDuration > 0f)
			{
				this.QueueEvents(mixingFrom, animationTime);
			}
			this.events.Clear(false);
			mixingFrom.nextAnimationLast = animationTime;
			mixingFrom.nextTrackLast = mixingFrom.trackTime;
			return num;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00058FF4 File Offset: 0x000571F4
		private void ApplyAttachmentTimeline(AttachmentTimeline timeline, Skeleton skeleton, float time, MixBlend blend, bool attachments)
		{
			Slot slot = skeleton.slots.Items[timeline.slotIndex];
			if (!slot.bone.active)
			{
				return;
			}
			float[] frames = timeline.frames;
			if (time < frames[0])
			{
				if (blend == MixBlend.Setup || blend == MixBlend.First)
				{
					this.SetAttachment(skeleton, slot, slot.data.attachmentName, attachments);
				}
			}
			else
			{
				int num;
				if (time >= frames[frames.Length - 1])
				{
					num = frames.Length - 1;
				}
				else
				{
					num = Animation.BinarySearch(frames, time) - 1;
				}
				this.SetAttachment(skeleton, slot, timeline.attachmentNames[num], attachments);
			}
			if (slot.attachmentState <= this.unkeyedState)
			{
				slot.attachmentState = this.unkeyedState + 1;
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000590B4 File Offset: 0x000572B4
		private void SetAttachment(Skeleton skeleton, Slot slot, string attachmentName, bool attachments)
		{
			slot.Attachment = ((attachmentName != null) ? skeleton.GetAttachment(slot.data.index, attachmentName) : null);
			if (attachments)
			{
				slot.attachmentState = this.unkeyedState + 2;
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000590FC File Offset: 0x000572FC
		private static void ApplyRotateTimeline(RotateTimeline timeline, Skeleton skeleton, float time, float alpha, MixBlend blend, float[] timelinesRotation, int i, bool firstFrame)
		{
			if (firstFrame)
			{
				timelinesRotation[i] = 0f;
			}
			if (alpha == 1f)
			{
				timeline.Apply(skeleton, 0f, time, null, 1f, blend, MixDirection.In);
				return;
			}
			Bone bone = skeleton.bones.Items[timeline.boneIndex];
			if (!bone.active)
			{
				return;
			}
			float[] frames = timeline.frames;
			float num;
			float num2;
			if (time < frames[0])
			{
				if (blend == MixBlend.Setup)
				{
					bone.rotation = bone.data.rotation;
					return;
				}
				if (blend != MixBlend.First)
				{
					return;
				}
				num = bone.rotation;
				num2 = bone.data.rotation;
			}
			else
			{
				num = ((blend != MixBlend.Setup) ? bone.rotation : bone.data.rotation);
				if (time >= frames[frames.Length - 2])
				{
					num2 = bone.data.rotation + frames[frames.Length + -1];
				}
				else
				{
					int num3 = Animation.BinarySearch(frames, time, 2);
					float num4 = frames[num3 + -1];
					float num5 = frames[num3];
					float curvePercent = timeline.GetCurvePercent((num3 >> 1) - 1, 1f - (time - num5) / (frames[num3 + -2] - num5));
					num2 = frames[num3 + 1] - num4;
					num2 -= (float)((16384 - (int)(16384.499999999996 - (double)(num2 / 360f))) * 360);
					num2 = num4 + num2 * curvePercent + bone.data.rotation;
					num2 -= (float)((16384 - (int)(16384.499999999996 - (double)(num2 / 360f))) * 360);
				}
			}
			float num6 = num2 - num;
			num6 -= (float)((16384 - (int)(16384.499999999996 - (double)(num6 / 360f))) * 360);
			float num7;
			if (num6 == 0f)
			{
				num7 = timelinesRotation[i];
			}
			else
			{
				float num8;
				float value;
				if (firstFrame)
				{
					num8 = 0f;
					value = num6;
				}
				else
				{
					num8 = timelinesRotation[i];
					value = timelinesRotation[i + 1];
				}
				bool flag = num6 > 0f;
				bool flag2 = num8 >= 0f;
				if (Math.Sign(value) != Math.Sign(num6) && Math.Abs(value) <= 90f)
				{
					if (Math.Abs(num8) > 180f)
					{
						num8 += (float)(360 * Math.Sign(num8));
					}
					flag2 = flag;
				}
				num7 = num6 + num8 - num8 % 360f;
				if (flag2 != flag)
				{
					num7 += (float)(360 * Math.Sign(num8));
				}
				timelinesRotation[i] = num7;
			}
			timelinesRotation[i + 1] = num6;
			num += num7 * alpha;
			bone.rotation = num - (float)((16384 - (int)(16384.499999999996 - (double)(num / 360f))) * 360);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x000593D0 File Offset: 0x000575D0
		private void QueueEvents(TrackEntry entry, float animationTime)
		{
			float animationStart = entry.animationStart;
			float animationEnd = entry.animationEnd;
			float num = animationEnd - animationStart;
			float num2 = entry.trackLast % num;
			ExposedList<Event> exposedList = this.events;
			Event[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Event @event = items[i];
				if (@event.time < num2)
				{
					break;
				}
				if (@event.time <= animationEnd)
				{
					this.queue.Event(entry, @event);
				}
				i++;
			}
			bool flag;
			if (entry.loop)
			{
				flag = (num == 0f || num2 > entry.trackTime % num);
			}
			else
			{
				flag = (animationTime >= animationEnd && entry.animationLast < animationEnd);
			}
			if (flag)
			{
				this.queue.Complete(entry);
			}
			while (i < count)
			{
				Event event2 = items[i];
				if (event2.time >= animationStart)
				{
					this.queue.Event(entry, items[i]);
				}
				i++;
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x000594F8 File Offset: 0x000576F8
		public void ClearTracks()
		{
			bool drainDisabled = this.queue.drainDisabled;
			this.queue.drainDisabled = true;
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				this.ClearTrack(i);
				i++;
			}
			this.tracks.Clear(true);
			this.queue.drainDisabled = drainDisabled;
			this.queue.Drain();
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00059568 File Offset: 0x00057768
		public void ClearTrack(int trackIndex)
		{
			if (trackIndex >= this.tracks.Count)
			{
				return;
			}
			TrackEntry trackEntry = this.tracks.Items[trackIndex];
			if (trackEntry == null)
			{
				return;
			}
			this.queue.End(trackEntry);
			this.DisposeNext(trackEntry);
			TrackEntry trackEntry2 = trackEntry;
			for (;;)
			{
				TrackEntry mixingFrom = trackEntry2.mixingFrom;
				if (mixingFrom == null)
				{
					break;
				}
				this.queue.End(mixingFrom);
				trackEntry2.mixingFrom = null;
				trackEntry2.mixingTo = null;
				trackEntry2 = mixingFrom;
			}
			this.tracks.Items[trackEntry.trackIndex] = null;
			this.queue.Drain();
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00059604 File Offset: 0x00057804
		private void SetCurrent(int index, TrackEntry current, bool interrupt)
		{
			TrackEntry trackEntry = this.ExpandToIndex(index);
			this.tracks.Items[index] = current;
			if (trackEntry != null)
			{
				if (interrupt)
				{
					this.queue.Interrupt(trackEntry);
				}
				current.mixingFrom = trackEntry;
				trackEntry.mixingTo = current;
				current.mixTime = 0f;
				if (trackEntry.mixingFrom != null && trackEntry.mixDuration > 0f)
				{
					current.interruptAlpha *= Math.Min(1f, trackEntry.mixTime / trackEntry.mixDuration);
				}
				trackEntry.timelinesRotation.Clear(true);
			}
			this.queue.Start(current);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x000596B0 File Offset: 0x000578B0
		public TrackEntry SetAnimation(int trackIndex, string animationName, bool loop)
		{
			Animation animation = this.data.skeletonData.FindAnimation(animationName);
			if (animation == null)
			{
				throw new ArgumentException("Animation not found: " + animationName, "animationName");
			}
			return this.SetAnimation(trackIndex, animation, loop);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x000596F4 File Offset: 0x000578F4
		public TrackEntry SetAnimation(int trackIndex, Animation animation, bool loop)
		{
			if (animation == null)
			{
				throw new ArgumentNullException("animation", "animation cannot be null.");
			}
			bool interrupt = true;
			TrackEntry trackEntry = this.ExpandToIndex(trackIndex);
			if (trackEntry != null)
			{
				if (trackEntry.nextTrackLast == -1f)
				{
					this.tracks.Items[trackIndex] = trackEntry.mixingFrom;
					this.queue.Interrupt(trackEntry);
					this.queue.End(trackEntry);
					this.DisposeNext(trackEntry);
					trackEntry = trackEntry.mixingFrom;
					interrupt = false;
				}
				else
				{
					this.DisposeNext(trackEntry);
				}
			}
			TrackEntry trackEntry2 = this.NewTrackEntry(trackIndex, animation, loop, trackEntry);
			this.SetCurrent(trackIndex, trackEntry2, interrupt);
			this.queue.Drain();
			return trackEntry2;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000597A0 File Offset: 0x000579A0
		public TrackEntry AddAnimation(int trackIndex, string animationName, bool loop, float delay)
		{
			Animation animation = this.data.skeletonData.FindAnimation(animationName);
			if (animation == null)
			{
				throw new ArgumentException("Animation not found: " + animationName, "animationName");
			}
			return this.AddAnimation(trackIndex, animation, loop, delay);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x000597E8 File Offset: 0x000579E8
		public TrackEntry AddAnimation(int trackIndex, Animation animation, bool loop, float delay)
		{
			if (animation == null)
			{
				throw new ArgumentNullException("animation", "animation cannot be null.");
			}
			TrackEntry trackEntry = this.ExpandToIndex(trackIndex);
			if (trackEntry != null)
			{
				while (trackEntry.next != null)
				{
					trackEntry = trackEntry.next;
				}
			}
			TrackEntry trackEntry2 = this.NewTrackEntry(trackIndex, animation, loop, trackEntry);
			if (trackEntry == null)
			{
				this.SetCurrent(trackIndex, trackEntry2, true);
				this.queue.Drain();
			}
			else
			{
				trackEntry.next = trackEntry2;
				if (delay <= 0f)
				{
					float num = trackEntry.animationEnd - trackEntry.animationStart;
					if (num != 0f)
					{
						if (trackEntry.loop)
						{
							delay += num * (float)(1 + (int)(trackEntry.trackTime / num));
						}
						else
						{
							delay += Math.Max(num, trackEntry.trackTime);
						}
						delay -= this.data.GetMix(trackEntry.animation, animation);
					}
					else
					{
						delay = trackEntry.trackTime;
					}
				}
			}
			trackEntry2.delay = delay;
			return trackEntry2;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x000598E8 File Offset: 0x00057AE8
		public TrackEntry SetEmptyAnimation(int trackIndex, float mixDuration)
		{
			TrackEntry trackEntry = this.SetAnimation(trackIndex, AnimationState.EmptyAnimation, false);
			trackEntry.mixDuration = mixDuration;
			trackEntry.trackEnd = mixDuration;
			return trackEntry;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00059914 File Offset: 0x00057B14
		public TrackEntry AddEmptyAnimation(int trackIndex, float mixDuration, float delay)
		{
			if (delay <= 0f)
			{
				delay -= mixDuration;
			}
			TrackEntry trackEntry = this.AddAnimation(trackIndex, AnimationState.EmptyAnimation, false, delay);
			trackEntry.mixDuration = mixDuration;
			trackEntry.trackEnd = mixDuration;
			return trackEntry;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00059950 File Offset: 0x00057B50
		public void SetEmptyAnimations(float mixDuration)
		{
			bool drainDisabled = this.queue.drainDisabled;
			this.queue.drainDisabled = true;
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				TrackEntry trackEntry = this.tracks.Items[i];
				if (trackEntry != null)
				{
					this.SetEmptyAnimation(trackEntry.trackIndex, mixDuration);
				}
				i++;
			}
			this.queue.drainDisabled = drainDisabled;
			this.queue.Drain();
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x000599CC File Offset: 0x00057BCC
		private TrackEntry ExpandToIndex(int index)
		{
			if (index < this.tracks.Count)
			{
				return this.tracks.Items[index];
			}
			this.tracks.Resize(index + 1);
			return null;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00059A00 File Offset: 0x00057C00
		private TrackEntry NewTrackEntry(int trackIndex, Animation animation, bool loop, TrackEntry last)
		{
			TrackEntry trackEntry = this.trackEntryPool.Obtain();
			trackEntry.trackIndex = trackIndex;
			trackEntry.animation = animation;
			trackEntry.loop = loop;
			trackEntry.holdPrevious = false;
			trackEntry.eventThreshold = 0f;
			trackEntry.attachmentThreshold = 0f;
			trackEntry.drawOrderThreshold = 0f;
			trackEntry.animationStart = 0f;
			trackEntry.animationEnd = animation.Duration;
			trackEntry.animationLast = -1f;
			trackEntry.nextAnimationLast = -1f;
			trackEntry.delay = 0f;
			trackEntry.trackTime = 0f;
			trackEntry.trackLast = -1f;
			trackEntry.nextTrackLast = -1f;
			trackEntry.trackEnd = float.MaxValue;
			trackEntry.timeScale = 1f;
			trackEntry.alpha = 1f;
			trackEntry.interruptAlpha = 1f;
			trackEntry.mixTime = 0f;
			trackEntry.mixDuration = ((last != null) ? this.data.GetMix(last.animation, animation) : 0f);
			return trackEntry;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00059B14 File Offset: 0x00057D14
		private void DisposeNext(TrackEntry entry)
		{
			for (TrackEntry next = entry.next; next != null; next = next.next)
			{
				this.queue.Dispose(next);
			}
			entry.next = null;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00059B50 File Offset: 0x00057D50
		private void AnimationsChanged()
		{
			this.animationsChanged = false;
			this.propertyIDs.Clear();
			TrackEntry[] items = this.tracks.Items;
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				TrackEntry trackEntry = items[i];
				if (trackEntry != null)
				{
					while (trackEntry.mixingFrom != null)
					{
						trackEntry = trackEntry.mixingFrom;
					}
					do
					{
						if (trackEntry.mixingTo == null || trackEntry.mixBlend != MixBlend.Add)
						{
							this.ComputeHold(trackEntry);
						}
						trackEntry = trackEntry.mixingTo;
					}
					while (trackEntry != null);
				}
				i++;
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00059BEC File Offset: 0x00057DEC
		private void ComputeHold(TrackEntry entry)
		{
			TrackEntry mixingTo = entry.mixingTo;
			Timeline[] items = entry.animation.timelines.Items;
			int count = entry.animation.timelines.Count;
			int[] items2 = entry.timelineMode.Resize(count).Items;
			entry.timelineHoldMix.Clear(true);
			TrackEntry[] items3 = entry.timelineHoldMix.Resize(count).Items;
			HashSet<int> hashSet = this.propertyIDs;
			if (mixingTo != null && mixingTo.holdPrevious)
			{
				for (int i = 0; i < count; i++)
				{
					items2[i] = ((!hashSet.Add(items[i].PropertyId)) ? 2 : 3);
				}
				return;
			}
			for (int j = 0; j < count; j++)
			{
				Timeline timeline = items[j];
				int propertyId = timeline.PropertyId;
				if (!hashSet.Add(propertyId))
				{
					items2[j] = 0;
				}
				else if (mixingTo == null || timeline is AttachmentTimeline || timeline is DrawOrderTimeline || timeline is EventTimeline || !mixingTo.animation.HasTimeline(propertyId))
				{
					items2[j] = 1;
				}
				else
				{
					TrackEntry mixingTo2 = mixingTo.mixingTo;
					while (mixingTo2 != null)
					{
						if (mixingTo2.animation.HasTimeline(propertyId))
						{
							mixingTo2 = mixingTo2.mixingTo;
						}
						else
						{
							if (mixingTo2.mixDuration > 0f)
							{
								items2[j] = 4;
								items3[j] = mixingTo2;
								goto IL_180;
							}
							break;
						}
					}
					items2[j] = 3;
				}
				IL_180:;
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00059D88 File Offset: 0x00057F88
		public TrackEntry GetCurrent(int trackIndex)
		{
			if (trackIndex >= this.tracks.Count)
			{
				return null;
			}
			return this.tracks.Items[trackIndex];
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00059DB8 File Offset: 0x00057FB8
		public void ClearListenerNotifications()
		{
			this.queue.Clear();
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00059DC8 File Offset: 0x00057FC8
		// (set) Token: 0x06000B4F RID: 2895 RVA: 0x00059DD0 File Offset: 0x00057FD0
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

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00059DDC File Offset: 0x00057FDC
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x00059DE4 File Offset: 0x00057FE4
		public AnimationStateData Data
		{
			get
			{
				return this.data;
			}
			set
			{
				if (this.data == null)
				{
					throw new ArgumentNullException("data", "data cannot be null.");
				}
				this.data = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00059E14 File Offset: 0x00058014
		public ExposedList<TrackEntry> Tracks
		{
			get
			{
				return this.tracks;
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00059E1C File Offset: 0x0005801C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				TrackEntry trackEntry = this.tracks.Items[i];
				if (trackEntry != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(trackEntry.ToString());
				}
				i++;
			}
			if (stringBuilder.Length == 0)
			{
				return "<none>";
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000A7A RID: 2682
		internal const int Subsequent = 0;

		// Token: 0x04000A7B RID: 2683
		internal const int First = 1;

		// Token: 0x04000A7C RID: 2684
		internal const int HoldSubsequent = 2;

		// Token: 0x04000A7D RID: 2685
		internal const int HoldFirst = 3;

		// Token: 0x04000A7E RID: 2686
		internal const int HoldMix = 4;

		// Token: 0x04000A7F RID: 2687
		internal const int Setup = 1;

		// Token: 0x04000A80 RID: 2688
		internal const int Current = 2;

		// Token: 0x04000A81 RID: 2689
		private static readonly Animation EmptyAnimation = new Animation("<empty>", new ExposedList<Timeline>(), 0f);

		// Token: 0x04000A82 RID: 2690
		protected AnimationStateData data;

		// Token: 0x04000A83 RID: 2691
		private readonly ExposedList<TrackEntry> tracks = new ExposedList<TrackEntry>();

		// Token: 0x04000A84 RID: 2692
		private readonly ExposedList<Event> events = new ExposedList<Event>();

		// Token: 0x04000A85 RID: 2693
		private readonly EventQueue queue;

		// Token: 0x04000A86 RID: 2694
		private readonly HashSet<int> propertyIDs = new HashSet<int>();

		// Token: 0x04000A87 RID: 2695
		private bool animationsChanged;

		// Token: 0x04000A88 RID: 2696
		private float timeScale = 1f;

		// Token: 0x04000A89 RID: 2697
		private int unkeyedState;

		// Token: 0x04000A8A RID: 2698
		private readonly Pool<TrackEntry> trackEntryPool = new Pool<TrackEntry>(16, int.MaxValue);

		// Token: 0x0200023C RID: 572
		// (Invoke) Token: 0x060011F4 RID: 4596
		public delegate void TrackEntryDelegate(TrackEntry trackEntry);

		// Token: 0x0200023D RID: 573
		// (Invoke) Token: 0x060011F8 RID: 4600
		public delegate void TrackEntryEventDelegate(TrackEntry trackEntry, Event e);
	}
}
