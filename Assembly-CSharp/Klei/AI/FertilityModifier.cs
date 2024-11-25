using System;

namespace Klei.AI
{
	// Token: 0x02000F6B RID: 3947
	public class FertilityModifier : Resource
	{
		// Token: 0x0600791A RID: 31002 RVA: 0x002FE860 File Offset: 0x002FCA60
		public FertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction) : base(id, name)
		{
			this.Description = description;
			this.TargetTag = targetTag;
			this.TooltipCB = tooltipCB;
			this.ApplyFunction = applyFunction;
		}

		// Token: 0x0600791B RID: 31003 RVA: 0x002FE889 File Offset: 0x002FCA89
		public string GetTooltip()
		{
			if (this.TooltipCB != null)
			{
				return this.TooltipCB(this.Description);
			}
			return this.Description;
		}

		// Token: 0x04005A86 RID: 23174
		public string Description;

		// Token: 0x04005A87 RID: 23175
		public Tag TargetTag;

		// Token: 0x04005A88 RID: 23176
		public Func<string, string> TooltipCB;

		// Token: 0x04005A89 RID: 23177
		public FertilityModifier.FertilityModFn ApplyFunction;

		// Token: 0x02002360 RID: 9056
		// (Invoke) Token: 0x0600B68B RID: 46731
		public delegate void FertilityModFn(FertilityMonitor.Instance inst, Tag eggTag);
	}
}
