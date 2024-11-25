using System;
using KSerialization;
using UnityEngine;

// Token: 0x020008F8 RID: 2296
[SerializationConfig(MemberSerialization.OptIn)]
public class IceMachine : StateMachineComponent<IceMachine.StatesInstance>, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x060041F3 RID: 16883 RVA: 0x0017593F File Offset: 0x00173B3F
	public void SetStorages(Storage waterStorage, Storage iceStorage)
	{
		this.waterStorage = waterStorage;
		this.iceStorage = iceStorage;
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x00175950 File Offset: 0x00173B50
	private bool CanMakeIce()
	{
		bool flag = this.waterStorage != null && this.waterStorage.GetMassAvailable(SimHashes.Water) >= 0.1f;
		bool flag2 = this.iceStorage != null && this.iceStorage.IsFull();
		return flag && !flag2;
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x001759B0 File Offset: 0x00173BB0
	private void MakeIce(IceMachine.StatesInstance smi, float dt)
	{
		float num = this.heatRemovalRate * dt / (float)this.waterStorage.items.Count;
		foreach (GameObject gameObject in this.waterStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, smi.master.targetTemperature);
		}
		for (int i = this.waterStorage.items.Count; i > 0; i--)
		{
			GameObject gameObject2 = this.waterStorage.items[i - 1];
			if (gameObject2 && gameObject2.GetComponent<PrimaryElement>().Temperature < gameObject2.GetComponent<PrimaryElement>().Element.lowTemp)
			{
				PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
				this.waterStorage.AddOre(this.targetProductionElement, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
				this.waterStorage.ConsumeIgnoringDisease(gameObject2);
			}
		}
		smi.UpdateIceState();
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x00175AD8 File Offset: 0x00173CD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x00175AEC File Offset: 0x00173CEC
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[IceMachineConfig.ELEMENT_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string tooltipText = Strings.Get("STRINGS.BUILDINGS.PREFABS.ICEMACHINE.OPTION_TOOLTIPS." + IceMachineConfig.ELEMENT_OPTIONS[i].ToString().ToUpper());
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(IceMachineConfig.ELEMENT_OPTIONS[i], ElementLoader.GetElement(IceMachineConfig.ELEMENT_OPTIONS[i]).name, Def.GetUISprite(IceMachineConfig.ELEMENT_OPTIONS[i], "ui", false), tooltipText);
		}
		return array;
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x00175B90 File Offset: 0x00173D90
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.targetProductionElement = ElementLoader.GetElementID(option.tag);
	}

	// Token: 0x060041F9 RID: 16889 RVA: 0x00175BA3 File Offset: 0x00173DA3
	public Tag GetSelectedOption()
	{
		return this.targetProductionElement.CreateTag();
	}

	// Token: 0x04002BB1 RID: 11185
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002BB2 RID: 11186
	public Storage waterStorage;

	// Token: 0x04002BB3 RID: 11187
	public Storage iceStorage;

	// Token: 0x04002BB4 RID: 11188
	public float targetTemperature;

	// Token: 0x04002BB5 RID: 11189
	public float heatRemovalRate;

	// Token: 0x04002BB6 RID: 11190
	private static StatusItem iceStorageFullStatusItem;

	// Token: 0x04002BB7 RID: 11191
	[Serialize]
	public SimHashes targetProductionElement = SimHashes.Ice;

	// Token: 0x02001862 RID: 6242
	public class StatesInstance : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.GameInstance
	{
		// Token: 0x06009823 RID: 38947 RVA: 0x003676A8 File Offset: 0x003658A8
		public StatesInstance(IceMachine smi) : base(smi)
		{
			this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_OL",
				"meter_frame",
				"meter_fill"
			});
			this.UpdateMeter();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x06009824 RID: 38948 RVA: 0x0036771A File Offset: 0x0036591A
		private void OnStorageChange(object data)
		{
			this.UpdateMeter();
		}

		// Token: 0x06009825 RID: 38949 RVA: 0x00367722 File Offset: 0x00365922
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(base.smi.master.iceStorage.MassStored() / base.smi.master.iceStorage.Capacity()));
		}

		// Token: 0x06009826 RID: 38950 RVA: 0x00367760 File Offset: 0x00365960
		public void UpdateIceState()
		{
			bool value = false;
			for (int i = base.smi.master.waterStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject = base.smi.master.waterStorage.items[i - 1];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= base.smi.master.targetTemperature)
				{
					value = true;
				}
			}
			base.sm.doneFreezingIce.Set(value, this, false);
		}

		// Token: 0x040075BB RID: 30139
		private MeterController meter;

		// Token: 0x040075BC RID: 30140
		public Chore emptyChore;
	}

	// Token: 0x02001863 RID: 6243
	public class States : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine>
	{
		// Token: 0x06009827 RID: 38951 RVA: 0x003677F0 File Offset: 0x003659F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (IceMachine.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (IceMachine.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (IceMachine.StatesInstance smi) => smi.master.CanMakeIce());
			this.on.working_pre.Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.UpdateIceState();
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.QueueAnim("working_loop", true, null).Update("UpdateWorking", delegate(IceMachine.StatesInstance smi, float dt)
			{
				smi.master.MakeIce(smi, dt);
			}, UpdateRate.SIM_200ms, false).ParamTransition<bool>(this.doneFreezingIce, this.on.working_pst, GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.IsTrue).Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
			}).Exit(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Done Working");
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.CoolingWater, null);
			this.on.working_pst.Exit(new StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback(this.DoTransfer)).PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x06009828 RID: 38952 RVA: 0x00367A14 File Offset: 0x00365C14
		private void DoTransfer(IceMachine.StatesInstance smi)
		{
			for (int i = smi.master.waterStorage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = smi.master.waterStorage.items[i];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= smi.master.targetTemperature)
				{
					smi.master.waterStorage.Transfer(gameObject, smi.master.iceStorage, false, true);
				}
			}
			smi.UpdateMeter();
		}

		// Token: 0x040075BD RID: 30141
		public StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.BoolParameter doneFreezingIce;

		// Token: 0x040075BE RID: 30142
		public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State off;

		// Token: 0x040075BF RID: 30143
		public IceMachine.States.OnStates on;

		// Token: 0x020025AC RID: 9644
		public class OnStates : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State
		{
			// Token: 0x0400A7DA RID: 42970
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State waiting;

			// Token: 0x0400A7DB RID: 42971
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pre;

			// Token: 0x0400A7DC RID: 42972
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working;

			// Token: 0x0400A7DD RID: 42973
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pst;
		}
	}
}
