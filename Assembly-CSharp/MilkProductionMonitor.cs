using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200098D RID: 2445
public class MilkProductionMonitor : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>
{
	// Token: 0x06004752 RID: 18258 RVA: 0x0019846C File Offset: 0x0019666C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.producing;
		this.producing.DefaultState(this.producing.paused).EventHandler(GameHashes.CaloriesConsumed, delegate(MilkProductionMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.producing.paused.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.producing, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing), UpdateRate.SIM_1000ms);
		this.producing.producing.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing)), UpdateRate.SIM_1000ms);
		this.producing.full.ToggleStatusItem(Db.Get().CreatureStatusItems.MilkFull, null).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull)), UpdateRate.SIM_1000ms).Enter(delegate(MilkProductionMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Creatures.RequiresMilking);
		});
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x001985C0 File Offset: 0x001967C0
	private static bool IsProducing(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull && smi.IsUnderProductionEffect;
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x001985D2 File Offset: 0x001967D2
	private static bool IsFull(MilkProductionMonitor.Instance smi)
	{
		return smi.IsFull;
	}

	// Token: 0x06004755 RID: 18261 RVA: 0x001985DA File Offset: 0x001967DA
	private static bool HasCapacity(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull;
	}

	// Token: 0x04002E97 RID: 11927
	public MilkProductionMonitor.ProducingStates producing;

	// Token: 0x02001952 RID: 6482
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009C23 RID: 39971 RVA: 0x00371B38 File Offset: 0x0036FD38
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.MilkProduction.Id);
		}

		// Token: 0x04007926 RID: 31014
		public const SimHashes element = SimHashes.Milk;

		// Token: 0x04007927 RID: 31015
		public string effectId;

		// Token: 0x04007928 RID: 31016
		public float Capacity = 200f;

		// Token: 0x04007929 RID: 31017
		public float CaloriesPerCycle = 1000f;

		// Token: 0x0400792A RID: 31018
		public float HappinessRequired;
	}

	// Token: 0x02001953 RID: 6483
	public class ProducingStates : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State
	{
		// Token: 0x0400792B RID: 31019
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State paused;

		// Token: 0x0400792C RID: 31020
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State producing;

		// Token: 0x0400792D RID: 31021
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State full;
	}

	// Token: 0x02001954 RID: 6484
	public new class Instance : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.GameInstance
	{
		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06009C26 RID: 39974 RVA: 0x00371B84 File Offset: 0x0036FD84
		public float MilkAmount
		{
			get
			{
				return this.MilkPercentage / 100f * base.def.Capacity;
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06009C27 RID: 39975 RVA: 0x00371B9E File Offset: 0x0036FD9E
		public float MilkPercentage
		{
			get
			{
				return this.milkAmountInstance.value;
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06009C28 RID: 39976 RVA: 0x00371BAB File Offset: 0x0036FDAB
		public bool IsFull
		{
			get
			{
				return this.MilkPercentage >= this.milkAmountInstance.GetMax();
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06009C29 RID: 39977 RVA: 0x00371BC3 File Offset: 0x0036FDC3
		public bool IsUnderProductionEffect
		{
			get
			{
				return this.milkAmountInstance.GetDelta() > 0f;
			}
		}

		// Token: 0x06009C2A RID: 39978 RVA: 0x00371BD7 File Offset: 0x0036FDD7
		public Instance(IStateMachineTarget master, MilkProductionMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009C2B RID: 39979 RVA: 0x00371BE4 File Offset: 0x0036FDE4
		public override void StartSM()
		{
			this.milkAmountInstance = Db.Get().Amounts.MilkProduction.Lookup(base.gameObject);
			if (base.def.effectId != null)
			{
				this.effectInstance = this.effects.Get(base.smi.def.effectId);
			}
			base.StartSM();
		}

		// Token: 0x06009C2C RID: 39980 RVA: 0x00371C48 File Offset: 0x0036FE48
		public void OnCaloriesConsumed(object data)
		{
			if (base.def.effectId == null)
			{
				return;
			}
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.effectInstance = this.effects.Get(base.smi.def.effectId);
			if (this.effectInstance == null)
			{
				this.effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			this.effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.CaloriesPerCycle * 600f;
		}

		// Token: 0x06009C2D RID: 39981 RVA: 0x00371CE4 File Offset: 0x0036FEE4
		private void RemoveMilk(float amount)
		{
			if (this.milkAmountInstance != null)
			{
				float value = Mathf.Min(this.milkAmountInstance.GetMin(), this.MilkPercentage - amount);
				this.milkAmountInstance.SetValue(value);
			}
		}

		// Token: 0x06009C2E RID: 39982 RVA: 0x00371D20 File Offset: 0x0036FF20
		public PrimaryElement ExtractMilk(float desiredAmount)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (num <= 0f)
			{
				return null;
			}
			this.RemoveMilk(num);
			PrimaryElement component = LiquidSourceManager.Instance.CreateChunk(SimHashes.Milk, num, temperature, 0, 0, base.transform.GetPosition()).GetComponent<PrimaryElement>();
			component.KeepZeroMassObject = false;
			return component;
		}

		// Token: 0x06009C2F RID: 39983 RVA: 0x00371D84 File Offset: 0x0036FF84
		public PrimaryElement ExtractMilkIntoElementChunk(float desiredAmount, PrimaryElement elementChunk)
		{
			if (elementChunk == null || elementChunk.ElementID != SimHashes.Milk)
			{
				return null;
			}
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			float mass = elementChunk.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(elementChunk.Temperature, mass, temperature, num);
			elementChunk.SetMassTemperature(mass + num, finalTemperature);
			return elementChunk;
		}

		// Token: 0x06009C30 RID: 39984 RVA: 0x00371DEC File Offset: 0x0036FFEC
		public PrimaryElement ExtractMilkIntoStorage(float desiredAmount, Storage storage)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			return storage.AddLiquid(SimHashes.Milk, num, temperature, 0, 0, false, true);
		}

		// Token: 0x0400792E RID: 31022
		public Action<float> OnMilkAmountChanged;

		// Token: 0x0400792F RID: 31023
		public AmountInstance milkAmountInstance;

		// Token: 0x04007930 RID: 31024
		public EffectInstance effectInstance;

		// Token: 0x04007931 RID: 31025
		[MyCmpGet]
		private Effects effects;
	}
}
