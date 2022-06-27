using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001E9 RID: 489
	[RequireComponent(typeof(Animator))]
	public class SkeletonMecanim : SkeletonRenderer, ISkeletonAnimation
	{
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x0600101B RID: 4123 RVA: 0x0007179C File Offset: 0x0006F99C
		// (remove) Token: 0x0600101C RID: 4124 RVA: 0x000717B8 File Offset: 0x0006F9B8
		protected event UpdateBonesDelegate _UpdateLocal;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600101D RID: 4125 RVA: 0x000717D4 File Offset: 0x0006F9D4
		// (remove) Token: 0x0600101E RID: 4126 RVA: 0x000717F0 File Offset: 0x0006F9F0
		protected event UpdateBonesDelegate _UpdateWorld;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600101F RID: 4127 RVA: 0x0007180C File Offset: 0x0006FA0C
		// (remove) Token: 0x06001020 RID: 4128 RVA: 0x00071828 File Offset: 0x0006FA28
		protected event UpdateBonesDelegate _UpdateComplete;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06001021 RID: 4129 RVA: 0x00071844 File Offset: 0x0006FA44
		// (remove) Token: 0x06001022 RID: 4130 RVA: 0x00071860 File Offset: 0x0006FA60
		public event UpdateBonesDelegate UpdateLocal
		{
			add
			{
				this._UpdateLocal = (UpdateBonesDelegate)Delegate.Combine(this._UpdateLocal, value);
			}
			remove
			{
				this._UpdateLocal = (UpdateBonesDelegate)Delegate.Remove(this._UpdateLocal, value);
			}
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06001023 RID: 4131 RVA: 0x0007187C File Offset: 0x0006FA7C
		// (remove) Token: 0x06001024 RID: 4132 RVA: 0x00071898 File Offset: 0x0006FA98
		public event UpdateBonesDelegate UpdateWorld
		{
			add
			{
				this._UpdateWorld = (UpdateBonesDelegate)Delegate.Combine(this._UpdateWorld, value);
			}
			remove
			{
				this._UpdateWorld = (UpdateBonesDelegate)Delegate.Remove(this._UpdateWorld, value);
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001025 RID: 4133 RVA: 0x000718B4 File Offset: 0x0006FAB4
		// (remove) Token: 0x06001026 RID: 4134 RVA: 0x000718D0 File Offset: 0x0006FAD0
		public event UpdateBonesDelegate UpdateComplete
		{
			add
			{
				this._UpdateComplete = (UpdateBonesDelegate)Delegate.Combine(this._UpdateComplete, value);
			}
			remove
			{
				this._UpdateComplete = (UpdateBonesDelegate)Delegate.Remove(this._UpdateComplete, value);
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x000718EC File Offset: 0x0006FAEC
		public SkeletonMecanim.MecanimTranslator Translator
		{
			get
			{
				return this.translator;
			}
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x000718F4 File Offset: 0x0006FAF4
		public override void Initialize(bool overwrite)
		{
			if (this.valid && !overwrite)
			{
				return;
			}
			base.Initialize(overwrite);
			if (!this.valid)
			{
				return;
			}
			if (this.translator == null)
			{
				this.translator = new SkeletonMecanim.MecanimTranslator();
			}
			this.translator.Initialize(base.GetComponent<Animator>(), this.skeletonDataAsset);
			this.wasUpdatedAfterInit = false;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0007195C File Offset: 0x0006FB5C
		public void Update()
		{
			if (!this.valid)
			{
				return;
			}
			this.wasUpdatedAfterInit = true;
			if (this.updateMode <= UpdateMode.OnlyAnimationStatus)
			{
				return;
			}
			this.ApplyAnimation();
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00071990 File Offset: 0x0006FB90
		protected void ApplyAnimation()
		{
			this.translator.Apply(this.skeleton);
			if (this._UpdateLocal != null)
			{
				this._UpdateLocal(this);
			}
			this.skeleton.UpdateWorldTransform();
			if (this._UpdateWorld != null)
			{
				this._UpdateWorld(this);
				this.skeleton.UpdateWorldTransform();
			}
			if (this._UpdateComplete != null)
			{
				this._UpdateComplete(this);
			}
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00071A0C File Offset: 0x0006FC0C
		public override void LateUpdate()
		{
			if (!this.wasUpdatedAfterInit)
			{
				this.Update();
			}
			base.LateUpdate();
		}

		// Token: 0x04000D2D RID: 3373
		[SerializeField]
		protected SkeletonMecanim.MecanimTranslator translator;

		// Token: 0x04000D2E RID: 3374
		private bool wasUpdatedAfterInit = true;

		// Token: 0x020001EA RID: 490
		[Serializable]
		public class MecanimTranslator
		{
			// Token: 0x14000023 RID: 35
			// (add) Token: 0x0600102D RID: 4141 RVA: 0x00071A98 File Offset: 0x0006FC98
			// (remove) Token: 0x0600102E RID: 4142 RVA: 0x00071AB4 File Offset: 0x0006FCB4
			protected event SkeletonMecanim.MecanimTranslator.OnClipAppliedDelegate _OnClipApplied;

			// Token: 0x14000024 RID: 36
			// (add) Token: 0x0600102F RID: 4143 RVA: 0x00071AD0 File Offset: 0x0006FCD0
			// (remove) Token: 0x06001030 RID: 4144 RVA: 0x00071AEC File Offset: 0x0006FCEC
			public event SkeletonMecanim.MecanimTranslator.OnClipAppliedDelegate OnClipApplied
			{
				add
				{
					this._OnClipApplied = (SkeletonMecanim.MecanimTranslator.OnClipAppliedDelegate)Delegate.Combine(this._OnClipApplied, value);
				}
				remove
				{
					this._OnClipApplied = (SkeletonMecanim.MecanimTranslator.OnClipAppliedDelegate)Delegate.Remove(this._OnClipApplied, value);
				}
			}

			// Token: 0x17000301 RID: 769
			// (get) Token: 0x06001031 RID: 4145 RVA: 0x00071B08 File Offset: 0x0006FD08
			public Animator Animator
			{
				get
				{
					return this.animator;
				}
			}

			// Token: 0x17000302 RID: 770
			// (get) Token: 0x06001032 RID: 4146 RVA: 0x00071B10 File Offset: 0x0006FD10
			public int MecanimLayerCount
			{
				get
				{
					if (!this.animator)
					{
						return 0;
					}
					return this.animator.layerCount;
				}
			}

			// Token: 0x17000303 RID: 771
			// (get) Token: 0x06001033 RID: 4147 RVA: 0x00071B30 File Offset: 0x0006FD30
			public string[] MecanimLayerNames
			{
				get
				{
					if (!this.animator)
					{
						return new string[0];
					}
					string[] array = new string[this.animator.layerCount];
					for (int i = 0; i < this.animator.layerCount; i++)
					{
						array[i] = this.animator.GetLayerName(i);
					}
					return array;
				}
			}

			// Token: 0x06001034 RID: 4148 RVA: 0x00071B94 File Offset: 0x0006FD94
			public void Initialize(Animator animator, SkeletonDataAsset skeletonDataAsset)
			{
				this.animator = animator;
				this.previousAnimations.Clear();
				this.animationTable.Clear();
				SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(true);
				foreach (Animation animation in skeletonData.Animations)
				{
					this.animationTable.Add(animation.Name.GetHashCode(), animation);
				}
				this.clipNameHashCodeTable.Clear();
				this.ClearClipInfosForLayers();
			}

			// Token: 0x06001035 RID: 4149 RVA: 0x00071C40 File Offset: 0x0006FE40
			private bool ApplyAnimation(Skeleton skeleton, AnimatorClipInfo info, AnimatorStateInfo stateInfo, int layerIndex, float layerWeight, MixBlend layerBlendMode, bool useClipWeight1 = false)
			{
				float num = info.weight * layerWeight;
				if (num == 0f)
				{
					return false;
				}
				Animation animation = this.GetAnimation(info.clip);
				if (animation == null)
				{
					return false;
				}
				float time = SkeletonMecanim.MecanimTranslator.AnimationTime(stateInfo.normalizedTime, info.clip.length, info.clip.isLooping, stateInfo.speed < 0f);
				num = ((!useClipWeight1) ? num : layerWeight);
				animation.Apply(skeleton, 0f, time, info.clip.isLooping, null, num, layerBlendMode, MixDirection.In);
				if (this._OnClipApplied != null)
				{
					this.OnClipAppliedCallback(animation, stateInfo, layerIndex, time, info.clip.isLooping, num);
				}
				return true;
			}

			// Token: 0x06001036 RID: 4150 RVA: 0x00071D00 File Offset: 0x0006FF00
			private bool ApplyInterruptionAnimation(Skeleton skeleton, bool interpolateWeightTo1, AnimatorClipInfo info, AnimatorStateInfo stateInfo, int layerIndex, float layerWeight, MixBlend layerBlendMode, float interruptingClipTimeAddition, bool useClipWeight1 = false)
			{
				float num = (!interpolateWeightTo1) ? info.weight : ((info.weight + 1f) * 0.5f);
				float num2 = num * layerWeight;
				if (num2 == 0f)
				{
					return false;
				}
				Animation animation = this.GetAnimation(info.clip);
				if (animation == null)
				{
					return false;
				}
				float time = SkeletonMecanim.MecanimTranslator.AnimationTime(stateInfo.normalizedTime + interruptingClipTimeAddition, info.clip.length, stateInfo.speed < 0f);
				num2 = ((!useClipWeight1) ? num2 : layerWeight);
				animation.Apply(skeleton, 0f, time, info.clip.isLooping, null, num2, layerBlendMode, MixDirection.In);
				if (this._OnClipApplied != null)
				{
					this.OnClipAppliedCallback(animation, stateInfo, layerIndex, time, info.clip.isLooping, num2);
				}
				return true;
			}

			// Token: 0x06001037 RID: 4151 RVA: 0x00071DD8 File Offset: 0x0006FFD8
			private void OnClipAppliedCallback(Animation clip, AnimatorStateInfo stateInfo, int layerIndex, float time, bool isLooping, float weight)
			{
				float num = (clip.duration != 0f) ? clip.duration : 1f;
				float num2 = stateInfo.speedMultiplier * stateInfo.speed;
				float num3 = time - Time.deltaTime * num2;
				if (isLooping && clip.duration != 0f)
				{
					time %= clip.duration;
					num3 %= clip.duration;
				}
				this._OnClipApplied(clip, layerIndex, weight, time, num3, num2 < 0f);
			}

			// Token: 0x06001038 RID: 4152 RVA: 0x00071E68 File Offset: 0x00070068
			public void Apply(Skeleton skeleton)
			{
				if (this.layerMixModes.Length < this.animator.layerCount)
				{
					int num = this.layerMixModes.Length;
					Array.Resize<SkeletonMecanim.MecanimTranslator.MixMode>(ref this.layerMixModes, this.animator.layerCount);
					for (int i = num; i < this.animator.layerCount; i++)
					{
						bool flag = false;
						if (i < this.layerBlendModes.Length)
						{
							flag = (this.layerBlendModes[i] == MixBlend.Add);
						}
						this.layerMixModes[i] = ((!flag) ? SkeletonMecanim.MecanimTranslator.MixMode.AlwaysMix : SkeletonMecanim.MecanimTranslator.MixMode.MixNext);
					}
				}
				this.InitClipInfosForLayers();
				int j = 0;
				int layerCount = this.animator.layerCount;
				while (j < layerCount)
				{
					this.GetStateUpdatesFromAnimator(j);
					j++;
				}
				if (this.autoReset)
				{
					List<Animation> list = this.previousAnimations;
					int k = 0;
					int count = list.Count;
					while (k < count)
					{
						list[k].SetKeyedItemsToSetupPose(skeleton);
						k++;
					}
					list.Clear();
					int l = 0;
					int layerCount2 = this.animator.layerCount;
					while (l < layerCount2)
					{
						float num2 = (l != 0) ? this.animator.GetLayerWeight(l) : 1f;
						if (num2 > 0f)
						{
							bool flag2 = this.animator.GetNextAnimatorStateInfo(l).fullPathHash != 0;
							bool flag3;
							int num3;
							int num4;
							int num5;
							IList<AnimatorClipInfo> list2;
							IList<AnimatorClipInfo> list3;
							IList<AnimatorClipInfo> list4;
							bool flag4;
							this.GetAnimatorClipInfos(l, out flag3, out num3, out num4, out num5, out list2, out list3, out list4, out flag4);
							for (int m = 0; m < num3; m++)
							{
								AnimatorClipInfo animatorClipInfo = list2[m];
								float num6 = animatorClipInfo.weight * num2;
								if (num6 != 0f)
								{
									Animation animation = this.GetAnimation(animatorClipInfo.clip);
									if (animation != null)
									{
										list.Add(animation);
									}
								}
							}
							if (flag2)
							{
								for (int n = 0; n < num4; n++)
								{
									AnimatorClipInfo animatorClipInfo2 = list3[n];
									float num7 = animatorClipInfo2.weight * num2;
									if (num7 != 0f)
									{
										Animation animation2 = this.GetAnimation(animatorClipInfo2.clip);
										if (animation2 != null)
										{
											list.Add(animation2);
										}
									}
								}
							}
							if (flag3)
							{
								for (int num8 = 0; num8 < num5; num8++)
								{
									AnimatorClipInfo animatorClipInfo3 = list4[num8];
									float num9 = (!flag4) ? animatorClipInfo3.weight : ((animatorClipInfo3.weight + 1f) * 0.5f);
									float num10 = num9 * num2;
									if (num10 != 0f)
									{
										Animation animation3 = this.GetAnimation(animatorClipInfo3.clip);
										if (animation3 != null)
										{
											list.Add(animation3);
										}
									}
								}
							}
						}
						l++;
					}
				}
				int num11 = 0;
				int layerCount3 = this.animator.layerCount;
				while (num11 < layerCount3)
				{
					float layerWeight = (num11 != 0) ? this.animator.GetLayerWeight(num11) : 1f;
					bool flag5;
					AnimatorStateInfo stateInfo;
					AnimatorStateInfo stateInfo2;
					AnimatorStateInfo stateInfo3;
					float interruptingClipTimeAddition;
					this.GetAnimatorStateInfos(num11, out flag5, out stateInfo, out stateInfo2, out stateInfo3, out interruptingClipTimeAddition);
					bool flag6 = stateInfo2.fullPathHash != 0;
					int num12;
					int num13;
					int num14;
					IList<AnimatorClipInfo> list5;
					IList<AnimatorClipInfo> list6;
					IList<AnimatorClipInfo> list7;
					bool interpolateWeightTo;
					this.GetAnimatorClipInfos(num11, out flag5, out num12, out num13, out num14, out list5, out list6, out list7, out interpolateWeightTo);
					MixBlend layerBlendMode = (num11 >= this.layerBlendModes.Length) ? MixBlend.Replace : this.layerBlendModes[num11];
					SkeletonMecanim.MecanimTranslator.MixMode mixMode = this.GetMixMode(num11, layerBlendMode);
					if (mixMode == SkeletonMecanim.MecanimTranslator.MixMode.AlwaysMix)
					{
						for (int num15 = 0; num15 < num12; num15++)
						{
							this.ApplyAnimation(skeleton, list5[num15], stateInfo, num11, layerWeight, layerBlendMode, false);
						}
						if (flag6)
						{
							for (int num16 = 0; num16 < num13; num16++)
							{
								this.ApplyAnimation(skeleton, list6[num16], stateInfo2, num11, layerWeight, layerBlendMode, false);
							}
						}
						if (flag5)
						{
							for (int num17 = 0; num17 < num14; num17++)
							{
								this.ApplyInterruptionAnimation(skeleton, interpolateWeightTo, list7[num17], stateInfo3, num11, layerWeight, layerBlendMode, interruptingClipTimeAddition, false);
							}
						}
					}
					else
					{
						int num18;
						for (num18 = 0; num18 < num12; num18++)
						{
							if (this.ApplyAnimation(skeleton, list5[num18], stateInfo, num11, layerWeight, layerBlendMode, true))
							{
								num18++;
								break;
							}
						}
						while (num18 < num12)
						{
							this.ApplyAnimation(skeleton, list5[num18], stateInfo, num11, layerWeight, layerBlendMode, false);
							num18++;
						}
						num18 = 0;
						if (flag6)
						{
							if (mixMode == SkeletonMecanim.MecanimTranslator.MixMode.Hard)
							{
								while (num18 < num13)
								{
									if (this.ApplyAnimation(skeleton, list6[num18], stateInfo2, num11, layerWeight, layerBlendMode, true))
									{
										num18++;
										break;
									}
									num18++;
								}
							}
							while (num18 < num13)
							{
								if (!this.ApplyAnimation(skeleton, list6[num18], stateInfo2, num11, layerWeight, layerBlendMode, false))
								{
								}
								num18++;
							}
						}
						num18 = 0;
						if (flag5)
						{
							if (mixMode == SkeletonMecanim.MecanimTranslator.MixMode.Hard)
							{
								while (num18 < num14)
								{
									if (this.ApplyInterruptionAnimation(skeleton, interpolateWeightTo, list7[num18], stateInfo3, num11, layerWeight, layerBlendMode, interruptingClipTimeAddition, true))
									{
										num18++;
										break;
									}
									num18++;
								}
							}
							while (num18 < num14)
							{
								this.ApplyInterruptionAnimation(skeleton, interpolateWeightTo, list7[num18], stateInfo3, num11, layerWeight, layerBlendMode, interruptingClipTimeAddition, false);
								num18++;
							}
						}
					}
					num11++;
				}
			}

			// Token: 0x06001039 RID: 4153 RVA: 0x00072424 File Offset: 0x00070624
			private static float AnimationTime(float normalizedTime, float clipLength, bool loop, bool reversed)
			{
				float num = SkeletonMecanim.MecanimTranslator.AnimationTime(normalizedTime, clipLength, reversed);
				if (loop)
				{
					return num;
				}
				return (clipLength - num >= 0.033333335f) ? num : clipLength;
			}

			// Token: 0x0600103A RID: 4154 RVA: 0x00072458 File Offset: 0x00070658
			private static float AnimationTime(float normalizedTime, float clipLength, bool reversed)
			{
				if (reversed)
				{
					normalizedTime = 1f - normalizedTime + (float)((int)normalizedTime) + (float)((int)normalizedTime);
				}
				return normalizedTime * clipLength;
			}

			// Token: 0x0600103B RID: 4155 RVA: 0x00072474 File Offset: 0x00070674
			private void InitClipInfosForLayers()
			{
				if (this.layerClipInfos.Length < this.animator.layerCount)
				{
					Array.Resize<SkeletonMecanim.MecanimTranslator.ClipInfos>(ref this.layerClipInfos, this.animator.layerCount);
					int i = 0;
					int layerCount = this.animator.layerCount;
					while (i < layerCount)
					{
						if (this.layerClipInfos[i] == null)
						{
							this.layerClipInfos[i] = new SkeletonMecanim.MecanimTranslator.ClipInfos();
						}
						i++;
					}
				}
			}

			// Token: 0x0600103C RID: 4156 RVA: 0x000724E8 File Offset: 0x000706E8
			private void ClearClipInfosForLayers()
			{
				int i = 0;
				int num = this.layerClipInfos.Length;
				while (i < num)
				{
					if (this.layerClipInfos[i] == null)
					{
						this.layerClipInfos[i] = new SkeletonMecanim.MecanimTranslator.ClipInfos();
					}
					else
					{
						this.layerClipInfos[i].isInterruptionActive = false;
						this.layerClipInfos[i].isLastFrameOfInterruption = false;
						this.layerClipInfos[i].clipInfos.Clear();
						this.layerClipInfos[i].nextClipInfos.Clear();
						this.layerClipInfos[i].interruptingClipInfos.Clear();
					}
					i++;
				}
			}

			// Token: 0x0600103D RID: 4157 RVA: 0x00072584 File Offset: 0x00070784
			private SkeletonMecanim.MecanimTranslator.MixMode GetMixMode(int layer, MixBlend layerBlendMode)
			{
				if (this.useCustomMixMode)
				{
					SkeletonMecanim.MecanimTranslator.MixMode mixMode = this.layerMixModes[layer];
					if (layerBlendMode == MixBlend.Add && mixMode == SkeletonMecanim.MecanimTranslator.MixMode.MixNext)
					{
						mixMode = SkeletonMecanim.MecanimTranslator.MixMode.AlwaysMix;
						this.layerMixModes[layer] = mixMode;
					}
					return mixMode;
				}
				return (layerBlendMode != MixBlend.Add) ? SkeletonMecanim.MecanimTranslator.MixMode.MixNext : SkeletonMecanim.MecanimTranslator.MixMode.AlwaysMix;
			}

			// Token: 0x0600103E RID: 4158 RVA: 0x000725D0 File Offset: 0x000707D0
			private void GetStateUpdatesFromAnimator(int layer)
			{
			}

			// Token: 0x0600103F RID: 4159 RVA: 0x000725D4 File Offset: 0x000707D4
			private void GetAnimatorClipInfos(int layer, out bool isInterruptionActive, out int clipInfoCount, out int nextClipInfoCount, out int interruptingClipInfoCount, out IList<AnimatorClipInfo> clipInfo, out IList<AnimatorClipInfo> nextClipInfo, out IList<AnimatorClipInfo> interruptingClipInfo, out bool shallInterpolateWeightTo1)
			{
				SkeletonMecanim.MecanimTranslator.ClipInfos clipInfos = this.layerClipInfos[layer];
				isInterruptionActive = clipInfos.isInterruptionActive;
				clipInfoCount = clipInfos.clipInfoCount;
				nextClipInfoCount = clipInfos.nextClipInfoCount;
				interruptingClipInfoCount = clipInfos.interruptingClipInfoCount;
				clipInfo = clipInfos.clipInfos;
				nextClipInfo = clipInfos.nextClipInfos;
				interruptingClipInfo = ((!isInterruptionActive) ? null : clipInfos.interruptingClipInfos);
				shallInterpolateWeightTo1 = clipInfos.isLastFrameOfInterruption;
			}

			// Token: 0x06001040 RID: 4160 RVA: 0x00072640 File Offset: 0x00070840
			private void GetAnimatorStateInfos(int layer, out bool isInterruptionActive, out AnimatorStateInfo stateInfo, out AnimatorStateInfo nextStateInfo, out AnimatorStateInfo interruptingStateInfo, out float interruptingClipTimeAddition)
			{
				SkeletonMecanim.MecanimTranslator.ClipInfos clipInfos = this.layerClipInfos[layer];
				isInterruptionActive = clipInfos.isInterruptionActive;
				stateInfo = clipInfos.stateInfo;
				nextStateInfo = clipInfos.nextStateInfo;
				interruptingStateInfo = clipInfos.interruptingStateInfo;
				interruptingClipTimeAddition = ((!clipInfos.isLastFrameOfInterruption) ? 0f : clipInfos.interruptingClipTimeAddition);
			}

			// Token: 0x06001041 RID: 4161 RVA: 0x000726A4 File Offset: 0x000708A4
			private Animation GetAnimation(AnimationClip clip)
			{
				int hashCode;
				if (!this.clipNameHashCodeTable.TryGetValue(clip, out hashCode))
				{
					hashCode = clip.name.GetHashCode();
					this.clipNameHashCodeTable.Add(clip, hashCode);
				}
				Animation result;
				this.animationTable.TryGetValue(hashCode, out result);
				return result;
			}

			// Token: 0x04000D32 RID: 3378
			public bool autoReset = true;

			// Token: 0x04000D33 RID: 3379
			public bool useCustomMixMode = true;

			// Token: 0x04000D34 RID: 3380
			public SkeletonMecanim.MecanimTranslator.MixMode[] layerMixModes = new SkeletonMecanim.MecanimTranslator.MixMode[0];

			// Token: 0x04000D35 RID: 3381
			public MixBlend[] layerBlendModes = new MixBlend[0];

			// Token: 0x04000D36 RID: 3382
			private readonly Dictionary<int, Animation> animationTable = new Dictionary<int, Animation>(SkeletonMecanim.MecanimTranslator.IntEqualityComparer.Instance);

			// Token: 0x04000D37 RID: 3383
			private readonly Dictionary<AnimationClip, int> clipNameHashCodeTable = new Dictionary<AnimationClip, int>(SkeletonMecanim.MecanimTranslator.AnimationClipEqualityComparer.Instance);

			// Token: 0x04000D38 RID: 3384
			private readonly List<Animation> previousAnimations = new List<Animation>();

			// Token: 0x04000D39 RID: 3385
			protected SkeletonMecanim.MecanimTranslator.ClipInfos[] layerClipInfos = new SkeletonMecanim.MecanimTranslator.ClipInfos[0];

			// Token: 0x04000D3A RID: 3386
			private Animator animator;

			// Token: 0x020001EB RID: 491
			public enum MixMode
			{
				// Token: 0x04000D3D RID: 3389
				AlwaysMix,
				// Token: 0x04000D3E RID: 3390
				MixNext,
				// Token: 0x04000D3F RID: 3391
				Hard
			}

			// Token: 0x020001EC RID: 492
			protected class ClipInfos
			{
				// Token: 0x04000D40 RID: 3392
				public bool isInterruptionActive;

				// Token: 0x04000D41 RID: 3393
				public bool isLastFrameOfInterruption;

				// Token: 0x04000D42 RID: 3394
				public int clipInfoCount;

				// Token: 0x04000D43 RID: 3395
				public int nextClipInfoCount;

				// Token: 0x04000D44 RID: 3396
				public int interruptingClipInfoCount;

				// Token: 0x04000D45 RID: 3397
				public readonly List<AnimatorClipInfo> clipInfos = new List<AnimatorClipInfo>();

				// Token: 0x04000D46 RID: 3398
				public readonly List<AnimatorClipInfo> nextClipInfos = new List<AnimatorClipInfo>();

				// Token: 0x04000D47 RID: 3399
				public readonly List<AnimatorClipInfo> interruptingClipInfos = new List<AnimatorClipInfo>();

				// Token: 0x04000D48 RID: 3400
				public AnimatorStateInfo stateInfo;

				// Token: 0x04000D49 RID: 3401
				public AnimatorStateInfo nextStateInfo;

				// Token: 0x04000D4A RID: 3402
				public AnimatorStateInfo interruptingStateInfo;

				// Token: 0x04000D4B RID: 3403
				public float interruptingClipTimeAddition;
			}

			// Token: 0x020001ED RID: 493
			private class AnimationClipEqualityComparer : IEqualityComparer<AnimationClip>
			{
				// Token: 0x06001045 RID: 4165 RVA: 0x00072730 File Offset: 0x00070930
				public bool Equals(AnimationClip x, AnimationClip y)
				{
					return x.GetInstanceID() == y.GetInstanceID();
				}

				// Token: 0x06001046 RID: 4166 RVA: 0x00072740 File Offset: 0x00070940
				public int GetHashCode(AnimationClip o)
				{
					return o.GetInstanceID();
				}

				// Token: 0x04000D4C RID: 3404
				internal static readonly IEqualityComparer<AnimationClip> Instance = new SkeletonMecanim.MecanimTranslator.AnimationClipEqualityComparer();
			}

			// Token: 0x020001EE RID: 494
			private class IntEqualityComparer : IEqualityComparer<int>
			{
				// Token: 0x06001049 RID: 4169 RVA: 0x0007275C File Offset: 0x0007095C
				public bool Equals(int x, int y)
				{
					return x == y;
				}

				// Token: 0x0600104A RID: 4170 RVA: 0x00072764 File Offset: 0x00070964
				public int GetHashCode(int o)
				{
					return o;
				}

				// Token: 0x04000D4D RID: 3405
				internal static readonly IEqualityComparer<int> Instance = new SkeletonMecanim.MecanimTranslator.IntEqualityComparer();
			}

			// Token: 0x02000241 RID: 577
			// (Invoke) Token: 0x06001208 RID: 4616
			public delegate void OnClipAppliedDelegate(Animation clip, int layerIndex, float weight, float time, float lastTime, bool playsBackward);
		}
	}
}
