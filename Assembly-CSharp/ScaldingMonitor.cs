using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200099B RID: 2459
public class ScaldingMonitor : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>
{
	// Token: 0x060047A9 RID: 18345 RVA: 0x0019A37C File Offset: 0x0019857C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State.Callback(ScaldingMonitor.SetInitialAverageExternalTemperature)).EventHandler(GameHashes.OnUnequip, new GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameEvent.Callback(ScaldingMonitor.OnSuitUnequipped)).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.AverageExternalTemperatureUpdate), UpdateRate.SIM_200ms, false);
		this.idle.Transition(this.transitionToScalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding), UpdateRate.SIM_200ms).Transition(this.transitionToScolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding), UpdateRate.SIM_200ms);
		this.transitionToScalding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding)), UpdateRate.SIM_200ms).Transition(this.scalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScaldingTimed), UpdateRate.SIM_200ms);
		this.transitionToScolding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding)), UpdateRate.SIM_200ms).Transition(this.scolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScoldingTimed), UpdateRate.SIM_200ms);
		this.scalding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScalding), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scalding, (ScaldingMonitor.Instance smi) => smi).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeScaldDamage), UpdateRate.SIM_1000ms, false);
		this.scolding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScolding), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scolding, (ScaldingMonitor.Instance smi) => smi).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeColdDamage), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x0019A5A6 File Offset: 0x001987A6
	public static void OnSuitUnequipped(ScaldingMonitor.Instance smi, object obj)
	{
		if (obj != null && ((Equippable)obj).HasTag(GameTags.AirtightSuit))
		{
			smi.ResetExternalTemperatureAverage();
		}
	}

	// Token: 0x060047AB RID: 18347 RVA: 0x0019A5C3 File Offset: 0x001987C3
	public static void SetInitialAverageExternalTemperature(ScaldingMonitor.Instance smi)
	{
		smi.AverageExternalTemperature = smi.GetCurrentExternalTemperature();
	}

	// Token: 0x060047AC RID: 18348 RVA: 0x0019A5D1 File Offset: 0x001987D1
	public static bool CanEscapeScalding(ScaldingMonitor.Instance smi)
	{
		return !smi.IsScalding() && smi.timeinstate > 1f;
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x0019A5EA File Offset: 0x001987EA
	public static bool CanEscapeScolding(ScaldingMonitor.Instance smi)
	{
		return !smi.IsScolding() && smi.timeinstate > 1f;
	}

	// Token: 0x060047AE RID: 18350 RVA: 0x0019A603 File Offset: 0x00198803
	public static bool IsScaldingTimed(ScaldingMonitor.Instance smi)
	{
		return smi.IsScalding() && smi.timeinstate > 1f;
	}

	// Token: 0x060047AF RID: 18351 RVA: 0x0019A61C File Offset: 0x0019881C
	public static bool IsScalding(ScaldingMonitor.Instance smi)
	{
		return smi.IsScalding();
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x0019A624 File Offset: 0x00198824
	public static bool IsScolding(ScaldingMonitor.Instance smi)
	{
		return smi.IsScolding();
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x0019A62C File Offset: 0x0019882C
	public static bool IsScoldingTimed(ScaldingMonitor.Instance smi)
	{
		return smi.IsScolding() && smi.timeinstate > 1f;
	}

	// Token: 0x060047B2 RID: 18354 RVA: 0x0019A645 File Offset: 0x00198845
	public static void TakeScaldDamage(ScaldingMonitor.Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x0019A64E File Offset: 0x0019884E
	public static void TakeColdDamage(ScaldingMonitor.Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x0019A658 File Offset: 0x00198858
	public static void AverageExternalTemperatureUpdate(ScaldingMonitor.Instance smi, float dt)
	{
		smi.AverageExternalTemperature *= Mathf.Max(0f, 1f - dt / 6f);
		smi.AverageExternalTemperature += smi.GetCurrentExternalTemperature() * (dt / 6f);
	}

	// Token: 0x04002ED4 RID: 11988
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x04002ED5 RID: 11989
	private const float TEMPERATURE_AVERAGING_RANGE = 6f;

	// Token: 0x04002ED6 RID: 11990
	private const float MIN_SCALD_INTERVAL = 5f;

	// Token: 0x04002ED7 RID: 11991
	private const float SCALDING_DAMAGE_AMOUNT = 10f;

	// Token: 0x04002ED8 RID: 11992
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State idle;

	// Token: 0x04002ED9 RID: 11993
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScalding;

	// Token: 0x04002EDA RID: 11994
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScolding;

	// Token: 0x04002EDB RID: 11995
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scalding;

	// Token: 0x04002EDC RID: 11996
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scolding;

	// Token: 0x02001975 RID: 6517
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400797E RID: 31102
		public float defaultScaldingTreshold = 345f;

		// Token: 0x0400797F RID: 31103
		public float defaultScoldingTreshold = 183f;
	}

	// Token: 0x02001976 RID: 6518
	public new class Instance : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameInstance
	{
		// Token: 0x06009CA7 RID: 40103 RVA: 0x00372CF4 File Offset: 0x00370EF4
		public Instance(IStateMachineTarget master, ScaldingMonitor.Def def) : base(master, def)
		{
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.baseScalindingThreshold = new AttributeModifier("ScaldingThreshold", def.defaultScaldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);
			this.baseScoldingThreshold = new AttributeModifier("ScoldingThreshold", def.defaultScoldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);
			this.attributes = base.gameObject.GetAttributes();
		}

		// Token: 0x06009CA8 RID: 40104 RVA: 0x00372D80 File Offset: 0x00370F80
		public override void StartSM()
		{
			base.smi.attributes.Get(Db.Get().Attributes.ScaldingThreshold).Add(this.baseScalindingThreshold);
			base.smi.attributes.Get(Db.Get().Attributes.ScoldingThreshold).Add(this.baseScoldingThreshold);
			base.StartSM();
		}

		// Token: 0x06009CA9 RID: 40105 RVA: 0x00372DE8 File Offset: 0x00370FE8
		public bool IsScalding()
		{
			int num = Grid.PosToCell(base.gameObject);
			return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void && this.AverageExternalTemperature > this.GetScaldingThreshold();
		}

		// Token: 0x06009CAA RID: 40106 RVA: 0x00372E3F File Offset: 0x0037103F
		public float GetScaldingThreshold()
		{
			return base.smi.attributes.GetValue("ScaldingThreshold");
		}

		// Token: 0x06009CAB RID: 40107 RVA: 0x00372E58 File Offset: 0x00371058
		public bool IsScolding()
		{
			int num = Grid.PosToCell(base.gameObject);
			return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void && this.AverageExternalTemperature < this.GetScoldingThreshold();
		}

		// Token: 0x06009CAC RID: 40108 RVA: 0x00372EAF File Offset: 0x003710AF
		public float GetScoldingThreshold()
		{
			return base.smi.attributes.GetValue("ScoldingThreshold");
		}

		// Token: 0x06009CAD RID: 40109 RVA: 0x00372EC6 File Offset: 0x003710C6
		public void TemperatureDamage(float dt)
		{
			if (this.health != null && Time.time - this.lastScaldTime > 5f)
			{
				this.lastScaldTime = Time.time;
				this.health.Damage(dt * 10f);
			}
		}

		// Token: 0x06009CAE RID: 40110 RVA: 0x00372F06 File Offset: 0x00371106
		public void ResetExternalTemperatureAverage()
		{
			base.smi.AverageExternalTemperature = this.internalTemperature.value;
		}

		// Token: 0x06009CAF RID: 40111 RVA: 0x00372F20 File Offset: 0x00371120
		public float GetCurrentExternalTemperature()
		{
			int num = Grid.PosToCell(base.gameObject);
			if (this.occupyArea != null)
			{
				float num2 = 0f;
				int num3 = 0;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					int num4 = Grid.OffsetCell(num, this.occupyArea.OccupiedCellsOffsets[i]);
					if (Grid.IsValidCell(num4))
					{
						bool flag = Grid.Element[num4].id == SimHashes.Vacuum || Grid.Element[num4].id == SimHashes.Void;
						num3++;
						num2 += (flag ? this.internalTemperature.value : Grid.Temperature[num4]);
					}
				}
				return num2 / (float)Mathf.Max(1, num3);
			}
			if (Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void)
			{
				return Grid.Temperature[num];
			}
			return this.internalTemperature.value;
		}

		// Token: 0x04007980 RID: 31104
		public float AverageExternalTemperature;

		// Token: 0x04007981 RID: 31105
		private float lastScaldTime;

		// Token: 0x04007982 RID: 31106
		private Attributes attributes;

		// Token: 0x04007983 RID: 31107
		[MyCmpGet]
		private Health health;

		// Token: 0x04007984 RID: 31108
		[MyCmpGet]
		private OccupyArea occupyArea;

		// Token: 0x04007985 RID: 31109
		private AttributeModifier baseScalindingThreshold;

		// Token: 0x04007986 RID: 31110
		private AttributeModifier baseScoldingThreshold;

		// Token: 0x04007987 RID: 31111
		public AmountInstance internalTemperature;
	}
}
