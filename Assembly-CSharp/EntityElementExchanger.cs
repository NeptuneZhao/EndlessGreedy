using System;
using UnityEngine;

// Token: 0x020009B3 RID: 2483
public class EntityElementExchanger : StateMachineComponent<EntityElementExchanger.StatesInstance>
{
	// Token: 0x06004822 RID: 18466 RVA: 0x0019D4D8 File Offset: 0x0019B6D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x0019D4E0 File Offset: 0x0019B6E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004824 RID: 18468 RVA: 0x0019D4F3 File Offset: 0x0019B6F3
	public void SetConsumptionRate(float consumptionRate)
	{
		this.consumeRate = consumptionRate;
	}

	// Token: 0x06004825 RID: 18469 RVA: 0x0019D4FC File Offset: 0x0019B6FC
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		EntityElementExchanger entityElementExchanger = (EntityElementExchanger)data;
		if (entityElementExchanger != null)
		{
			entityElementExchanger.OnSimConsume(mass_cb_info);
		}
	}

	// Token: 0x06004826 RID: 18470 RVA: 0x0019D520 File Offset: 0x0019B720
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		float num = mass_cb_info.mass * base.smi.master.exchangeRatio;
		if (this.reportExchange && base.smi.master.emittedElement == SimHashes.Oxygen)
		{
			string text = base.gameObject.GetProperName();
			ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
			if (component != null && component.GetReceptacle() != null)
			{
				text = text + " (" + component.GetReceptacle().gameObject.GetProperName() + ")";
			}
			ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num, text, null);
		}
		SimMessages.EmitMass(Grid.PosToCell(base.smi.master.transform.GetPosition() + this.outputOffset), ElementLoader.FindElementByHash(base.smi.master.emittedElement).idx, num, ElementLoader.FindElementByHash(base.smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
	}

	// Token: 0x04002F50 RID: 12112
	public Vector3 outputOffset = Vector3.zero;

	// Token: 0x04002F51 RID: 12113
	public bool reportExchange;

	// Token: 0x04002F52 RID: 12114
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002F53 RID: 12115
	public SimHashes consumedElement;

	// Token: 0x04002F54 RID: 12116
	public SimHashes emittedElement;

	// Token: 0x04002F55 RID: 12117
	public float consumeRate;

	// Token: 0x04002F56 RID: 12118
	public float exchangeRatio;

	// Token: 0x020019B2 RID: 6578
	public class StatesInstance : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.GameInstance
	{
		// Token: 0x06009DD3 RID: 40403 RVA: 0x003760D6 File Offset: 0x003742D6
		public StatesInstance(EntityElementExchanger master) : base(master)
		{
		}
	}

	// Token: 0x020019B3 RID: 6579
	public class States : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger>
	{
		// Token: 0x06009DD4 RID: 40404 RVA: 0x003760E0 File Offset: 0x003742E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.exchanging;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.exchanging.Enter(delegate(EntityElementExchanger.StatesInstance smi)
			{
				WiltCondition component = smi.master.gameObject.GetComponent<WiltCondition>();
				if (component != null && component.IsWilting())
				{
					smi.GoTo(smi.sm.paused);
				}
			}).EventTransition(GameHashes.Wilt, this.paused, null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementConsume, null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementOutput, null).Update("EntityElementExchanger", delegate(EntityElementExchanger.StatesInstance smi, float dt)
			{
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(EntityElementExchanger.OnSimConsumeCallback), smi.master, "EntityElementExchanger");
				SimMessages.ConsumeMass(Grid.PosToCell(smi.master.gameObject), smi.master.consumedElement, smi.master.consumeRate * dt, 3, handle.index);
			}, UpdateRate.SIM_1000ms, false);
			this.paused.EventTransition(GameHashes.WiltRecover, this.exchanging, null);
		}

		// Token: 0x04007A69 RID: 31337
		public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State exchanging;

		// Token: 0x04007A6A RID: 31338
		public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State paused;
	}
}
