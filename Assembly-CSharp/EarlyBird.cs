using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000867 RID: 2151
[SkipSaveFileSerialization]
public class EarlyBird : StateMachineComponent<EarlyBird.StatesInstance>
{
	// Token: 0x06003BFC RID: 15356 RVA: 0x0014A51C File Offset: 0x0014871C
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x0014A698 File Offset: 0x00148898
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x0014A6D4 File Offset: 0x001488D4
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x04002447 RID: 9287
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04002448 RID: 9288
	private AttributeModifier[] attributeModifiers;

	// Token: 0x02001769 RID: 5993
	public class StatesInstance : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.GameInstance
	{
		// Token: 0x06009592 RID: 38290 RVA: 0x0035FEB0 File Offset: 0x0035E0B0
		public StatesInstance(EarlyBird master) : base(master)
		{
		}

		// Token: 0x06009593 RID: 38291 RVA: 0x0035FEBC File Offset: 0x0035E0BC
		public bool IsMorning()
		{
			return !(ScheduleManager.Instance == null) && !(base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview) && Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23) < TRAITS.EARLYBIRD_SCHEDULEBLOCK;
		}
	}

	// Token: 0x0200176A RID: 5994
	public class States : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird>
	{
		// Token: 0x06009594 RID: 38292 RVA: 0x0035FF14 File Offset: 0x0035E114
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (EarlyBird.StatesInstance smi) => smi.IsMorning(), UpdateRate.SIM_200ms);
			this.early.Enter("Morning", delegate(EarlyBird.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMorning", delegate(EarlyBird.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.EarlyMorning, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (EarlyBird.StatesInstance smi) => !smi.IsMorning(), UpdateRate.SIM_200ms);
		}

		// Token: 0x040072A4 RID: 29348
		public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State idle;

		// Token: 0x040072A5 RID: 29349
		public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State early;
	}
}
