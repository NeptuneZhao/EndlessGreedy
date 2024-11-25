using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020009C2 RID: 2498
[SkipSaveFileSerialization]
public class NightOwl : StateMachineComponent<NightOwl.StatesInstance>
{
	// Token: 0x06004882 RID: 18562 RVA: 0x0019F464 File Offset: 0x0019D664
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x0019F5E0 File Offset: 0x0019D7E0
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x0019F61C File Offset: 0x0019D81C
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x04002F80 RID: 12160
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04002F81 RID: 12161
	private AttributeModifier[] attributeModifiers;

	// Token: 0x020019D1 RID: 6609
	public class StatesInstance : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.GameInstance
	{
		// Token: 0x06009E1B RID: 40475 RVA: 0x00376D9E File Offset: 0x00374F9E
		public StatesInstance(NightOwl master) : base(master)
		{
		}

		// Token: 0x06009E1C RID: 40476 RVA: 0x00376DA7 File Offset: 0x00374FA7
		public bool IsNight()
		{
			return !(GameClock.Instance == null) && !(base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview) && GameClock.Instance.IsNighttime();
		}
	}

	// Token: 0x020019D2 RID: 6610
	public class States : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl>
	{
		// Token: 0x06009E1D RID: 40477 RVA: 0x00376DE0 File Offset: 0x00374FE0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (NightOwl.StatesInstance smi) => smi.IsNight(), UpdateRate.SIM_200ms);
			this.early.Enter("Night", delegate(NightOwl.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotNight", delegate(NightOwl.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.NightTime, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (NightOwl.StatesInstance smi) => !smi.IsNight(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04007AAD RID: 31405
		public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State idle;

		// Token: 0x04007AAE RID: 31406
		public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State early;
	}
}
