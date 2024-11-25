using System;
using Klei.AI;

// Token: 0x0200056C RID: 1388
public class HeatImmunityProvider : EffectImmunityProviderStation<HeatImmunityProvider.Instance>
{
	// Token: 0x0400122C RID: 4652
	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "RefreshingTouch";

	// Token: 0x0200136E RID: 4974
	public new class Def : EffectImmunityProviderStation<HeatImmunityProvider.Instance>.Def
	{
	}

	// Token: 0x0200136F RID: 4975
	public new class Instance : EffectImmunityProviderStation<HeatImmunityProvider.Instance>.BaseInstance
	{
		// Token: 0x06008727 RID: 34599 RVA: 0x0032ADE4 File Offset: 0x00328FE4
		public Instance(IStateMachineTarget master, HeatImmunityProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06008728 RID: 34600 RVA: 0x0032ADEE File Offset: 0x00328FEE
		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("RefreshingTouch", true);
		}
	}
}
