using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000829 RID: 2089
public class WildnessMonitor : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>
{
	// Token: 0x060039C8 RID: 14792 RVA: 0x0013AC40 File Offset: 0x00138E40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.tame;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.wild.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.HideDomesticationSymbol)).Transition(this.tame, (WildnessMonitor.Instance smi) => !WildnessMonitor.IsWild(smi), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.wildEffect).ToggleTag(GameTags.Creatures.Wild);
		this.tame.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.ShowDomesticationSymbol)).Transition(this.wild, new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback(WildnessMonitor.IsWild), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.tameEffect).Enter(delegate(WildnessMonitor.Instance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.LogCritterTamed(smi.PrefabID());
		});
	}

	// Token: 0x060039C9 RID: 14793 RVA: 0x0013AD68 File Offset: 0x00138F68
	private static void HideDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, false);
		}
	}

	// Token: 0x060039CA RID: 14794 RVA: 0x0013ADA0 File Offset: 0x00138FA0
	private static void ShowDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, true);
		}
	}

	// Token: 0x060039CB RID: 14795 RVA: 0x0013ADD6 File Offset: 0x00138FD6
	private static bool IsWild(WildnessMonitor.Instance smi)
	{
		return smi.wildness.value > 0f;
	}

	// Token: 0x060039CC RID: 14796 RVA: 0x0013ADEC File Offset: 0x00138FEC
	private static void RefreshAmounts(WildnessMonitor.Instance smi)
	{
		bool flag = WildnessMonitor.IsWild(smi);
		smi.wildness.hide = !flag;
		AttributeInstance attributeInstance = Db.Get().CritterAttributes.Happiness.Lookup(smi.gameObject);
		if (attributeInstance != null)
		{
			attributeInstance.hide = flag;
		}
		AttributeInstance attributeInstance2 = Db.Get().CritterAttributes.Metabolism.Lookup(smi.gameObject);
		if (attributeInstance2 != null)
		{
			attributeInstance2.hide = flag;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
		if (amountInstance != null)
		{
			amountInstance.hide = flag;
		}
		AmountInstance amountInstance2 = Db.Get().Amounts.Temperature.Lookup(smi.gameObject);
		if (amountInstance2 != null)
		{
			amountInstance2.hide = flag;
		}
		AmountInstance amountInstance3 = Db.Get().Amounts.Fertility.Lookup(smi.gameObject);
		if (amountInstance3 != null)
		{
			amountInstance3.hide = flag;
		}
		AmountInstance amountInstance4 = Db.Get().Amounts.MilkProduction.Lookup(smi.gameObject);
		if (amountInstance4 != null)
		{
			amountInstance4.hide = flag;
		}
		AmountInstance amountInstance5 = Db.Get().Amounts.Beckoning.Lookup(smi.gameObject);
		if (amountInstance5 != null)
		{
			amountInstance5.hide = flag;
		}
	}

	// Token: 0x040022C3 RID: 8899
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild;

	// Token: 0x040022C4 RID: 8900
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State tame;

	// Token: 0x040022C5 RID: 8901
	private static readonly KAnimHashedString[] DOMESTICATION_SYMBOLS = new KAnimHashedString[]
	{
		"tag",
		"snapto_tag"
	};

	// Token: 0x02001747 RID: 5959
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009526 RID: 38182 RVA: 0x0035F11D File Offset: 0x0035D31D
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
		}

		// Token: 0x04007249 RID: 29257
		public Effect wildEffect;

		// Token: 0x0400724A RID: 29258
		public Effect tameEffect;
	}

	// Token: 0x02001748 RID: 5960
	public new class Instance : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.GameInstance
	{
		// Token: 0x06009528 RID: 38184 RVA: 0x0035F14B File Offset: 0x0035D34B
		public Instance(IStateMachineTarget master, WildnessMonitor.Def def) : base(master, def)
		{
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
		}

		// Token: 0x0400724B RID: 29259
		public AmountInstance wildness;
	}
}
