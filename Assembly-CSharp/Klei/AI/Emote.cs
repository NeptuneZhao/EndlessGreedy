using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02000F73 RID: 3955
	public class Emote : Resource
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06007957 RID: 31063 RVA: 0x002FF3DD File Offset: 0x002FD5DD
		public int StepCount
		{
			get
			{
				if (this.emoteSteps != null)
				{
					return this.emoteSteps.Count;
				}
				return 0;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06007958 RID: 31064 RVA: 0x002FF3F4 File Offset: 0x002FD5F4
		public KAnimFile AnimSet
		{
			get
			{
				if (this.animSetName != HashedString.Invalid && this.animSet == null)
				{
					this.animSet = Assets.GetAnim(this.animSetName);
				}
				return this.animSet;
			}
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x002FF42D File Offset: 0x002FD62D
		public Emote(ResourceSet parent, string emoteId, EmoteStep[] defaultSteps, string animSetName = null) : base(emoteId, parent, null)
		{
			this.emoteSteps.AddRange(defaultSteps);
			this.animSetName = animSetName;
		}

		// Token: 0x0600795A RID: 31066 RVA: 0x002FF468 File Offset: 0x002FD668
		public bool IsValidForController(KBatchedAnimController animController)
		{
			bool flag = true;
			int num = 0;
			while (flag && num < this.StepCount)
			{
				flag = animController.HasAnimation(this.emoteSteps[num].anim);
				num++;
			}
			KAnimFileData kanimFileData = (this.animSet == null) ? null : this.animSet.GetData();
			int num2 = 0;
			while (kanimFileData != null && flag && num2 < this.StepCount)
			{
				bool flag2 = false;
				int num3 = 0;
				while (!flag2 && num3 < kanimFileData.animCount)
				{
					flag2 = (kanimFileData.GetAnim(num2).id == this.emoteSteps[num2].anim);
					num3++;
				}
				flag = flag2;
				num2++;
			}
			return flag;
		}

		// Token: 0x0600795B RID: 31067 RVA: 0x002FF520 File Offset: 0x002FD720
		public void ApplyAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.AddAnimOverrides(kanimFile, 0f);
		}

		// Token: 0x0600795C RID: 31068 RVA: 0x002FF560 File Offset: 0x002FD760
		public void RemoveAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.RemoveAnimOverrides(kanimFile);
		}

		// Token: 0x0600795D RID: 31069 RVA: 0x002FF59C File Offset: 0x002FD79C
		public void CollectStepAnims(out HashedString[] emoteAnims, int iterations)
		{
			emoteAnims = new HashedString[this.emoteSteps.Count * iterations];
			for (int i = 0; i < emoteAnims.Length; i++)
			{
				emoteAnims[i] = this.emoteSteps[i % this.emoteSteps.Count].anim;
			}
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x002FF5F1 File Offset: 0x002FD7F1
		public bool IsValidStep(int stepIdx)
		{
			return stepIdx >= 0 && stepIdx < this.emoteSteps.Count;
		}

		// Token: 0x170008B8 RID: 2232
		public EmoteStep this[int stepIdx]
		{
			get
			{
				if (!this.IsValidStep(stepIdx))
				{
					return null;
				}
				return this.emoteSteps[stepIdx];
			}
		}

		// Token: 0x06007960 RID: 31072 RVA: 0x002FF620 File Offset: 0x002FD820
		public int GetStepIndex(HashedString animName)
		{
			int i = 0;
			bool condition = false;
			while (i < this.emoteSteps.Count)
			{
				if (this.emoteSteps[i].anim == animName)
				{
					condition = true;
					break;
				}
				i++;
			}
			Debug.Assert(condition, string.Format("Could not find emote step {0} for emote {1}!", animName, this.Id));
			return i;
		}

		// Token: 0x04005A99 RID: 23193
		private HashedString animSetName = null;

		// Token: 0x04005A9A RID: 23194
		private KAnimFile animSet;

		// Token: 0x04005A9B RID: 23195
		private List<EmoteStep> emoteSteps = new List<EmoteStep>();
	}
}
