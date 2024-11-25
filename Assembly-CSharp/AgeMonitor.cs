using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007ED RID: 2029
public class AgeMonitor : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>
{
	// Token: 0x06003824 RID: 14372 RVA: 0x00132E2C File Offset: 0x0013102C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		this.alive.ToggleAttributeModifier("Aging", (AgeMonitor.Instance smi) => this.aging, null).Transition(this.time_to_die, new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.Transition.ConditionCallback(AgeMonitor.TimeToDie), UpdateRate.SIM_1000ms).Update(new Action<AgeMonitor.Instance, float>(AgeMonitor.UpdateOldStatusItem), UpdateRate.SIM_1000ms, false);
		this.time_to_die.Enter(new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State.Callback(AgeMonitor.Die));
		this.aging = new AttributeModifier(Db.Get().Amounts.Age.deltaAttribute.Id, 0.0016666667f, CREATURES.MODIFIERS.AGE.NAME, false, false, true);
	}

	// Token: 0x06003825 RID: 14373 RVA: 0x00132ED8 File Offset: 0x001310D8
	private static void Die(AgeMonitor.Instance smi)
	{
		smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Generic);
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x00132EF4 File Offset: 0x001310F4
	private static bool TimeToDie(AgeMonitor.Instance smi)
	{
		return smi.age.value >= smi.age.GetMax();
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x00132F14 File Offset: 0x00131114
	private static void UpdateOldStatusItem(AgeMonitor.Instance smi, float dt)
	{
		bool show = smi.age.value > smi.age.GetMax() * 0.9f;
		smi.oldStatusGuid = smi.kselectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Old, smi.oldStatusGuid, show, smi);
	}

	// Token: 0x040021BC RID: 8636
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State alive;

	// Token: 0x040021BD RID: 8637
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State time_to_die;

	// Token: 0x040021BE RID: 8638
	private AttributeModifier aging;

	// Token: 0x020016BD RID: 5821
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600936A RID: 37738 RVA: 0x0035927D File Offset: 0x0035747D
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Age.Id);
		}

		// Token: 0x040070BB RID: 28859
		public float maxAgePercentOnSpawn = 0.75f;
	}

	// Token: 0x020016BE RID: 5822
	public new class Instance : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.GameInstance
	{
		// Token: 0x0600936C RID: 37740 RVA: 0x003592B8 File Offset: 0x003574B8
		public Instance(IStateMachineTarget master, AgeMonitor.Def def) : base(master, def)
		{
			this.age = Db.Get().Amounts.Age.Lookup(base.gameObject);
			base.Subscribe(1119167081, delegate(object data)
			{
				this.RandomizeAge();
			});
		}

		// Token: 0x0600936D RID: 37741 RVA: 0x00359304 File Offset: 0x00357504
		public void RandomizeAge()
		{
			this.age.value = UnityEngine.Random.value * this.age.GetMax() * base.def.maxAgePercentOnSpawn;
			AmountInstance amountInstance = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (amountInstance != null)
			{
				amountInstance.value = this.age.value / this.age.GetMax() * amountInstance.GetMax() * 1.75f;
				amountInstance.value = Mathf.Min(amountInstance.value, amountInstance.GetMax() * 0.9f);
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x0600936E RID: 37742 RVA: 0x0035939E File Offset: 0x0035759E
		public float CyclesUntilDeath
		{
			get
			{
				return this.age.GetMax() - this.age.value;
			}
		}

		// Token: 0x040070BC RID: 28860
		public AmountInstance age;

		// Token: 0x040070BD RID: 28861
		public Guid oldStatusGuid;

		// Token: 0x040070BE RID: 28862
		[MyCmpReq]
		public KSelectable kselectable;
	}
}
