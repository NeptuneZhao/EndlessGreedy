using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200053B RID: 1339
public class ColdImmunityProvider : EffectImmunityProviderStation<ColdImmunityProvider.Instance>
{
	// Token: 0x0400113B RID: 4411
	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "WarmTouch";

	// Token: 0x020012F5 RID: 4853
	public new class Def : EffectImmunityProviderStation<ColdImmunityProvider.Instance>.Def, IGameObjectEffectDescriptor
	{
		// Token: 0x06008555 RID: 34133 RVA: 0x00325E30 File Offset: 0x00324030
		public override string[] DefaultAnims()
		{
			return new string[]
			{
				"warmup_pre",
				"warmup_loop",
				"warmup_pst"
			};
		}

		// Token: 0x06008556 RID: 34134 RVA: 0x00325E50 File Offset: 0x00324050
		public override string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		// Token: 0x06008557 RID: 34135 RVA: 0x00325E58 File Offset: 0x00324058
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false)
			};
		}
	}

	// Token: 0x020012F6 RID: 4854
	public new class Instance : EffectImmunityProviderStation<ColdImmunityProvider.Instance>.BaseInstance
	{
		// Token: 0x06008559 RID: 34137 RVA: 0x00325EC5 File Offset: 0x003240C5
		public Instance(IStateMachineTarget master, ColdImmunityProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x0600855A RID: 34138 RVA: 0x00325ECF File Offset: 0x003240CF
		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("WarmTouch", true);
		}
	}
}
