using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200069B RID: 1691
public class Compost : StateMachineComponent<Compost.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002A7C RID: 10876 RVA: 0x000F0345 File Offset: 0x000EE545
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Compost>(-1697596308, Compost.OnStorageChangedDelegate);
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x000F0360 File Offset: 0x000EE560
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().ShowStatusItem = false;
		this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, base.GetComponent<Storage>());
		base.smi.StartSM();
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x000F03AD File Offset: 0x000EE5AD
	protected override void OnCleanUp()
	{
		this.temperatureAdjuster.CleanUp();
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x000F03BA File Offset: 0x000EE5BA
	private void OnStorageChanged(object data)
	{
		(GameObject)data == null;
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x000F03C9 File Offset: 0x000EE5C9
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);
	}

	// Token: 0x0400187C RID: 6268
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400187D RID: 6269
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400187E RID: 6270
	[SerializeField]
	public float flipInterval = 600f;

	// Token: 0x0400187F RID: 6271
	[SerializeField]
	public float simulatedInternalTemperature = 323.15f;

	// Token: 0x04001880 RID: 6272
	[SerializeField]
	public float simulatedInternalHeatCapacity = 400f;

	// Token: 0x04001881 RID: 6273
	[SerializeField]
	public float simulatedThermalConductivity = 1000f;

	// Token: 0x04001882 RID: 6274
	private SimulatedTemperatureAdjuster temperatureAdjuster;

	// Token: 0x04001883 RID: 6275
	private static readonly EventSystem.IntraObjectHandler<Compost> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Compost>(delegate(Compost component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02001497 RID: 5271
	public class StatesInstance : GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.GameInstance
	{
		// Token: 0x06008B6E RID: 35694 RVA: 0x00336B3A File Offset: 0x00334D3A
		public StatesInstance(Compost master) : base(master)
		{
		}

		// Token: 0x06008B6F RID: 35695 RVA: 0x00336B43 File Offset: 0x00334D43
		public bool CanStartConverting()
		{
			return base.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x06008B70 RID: 35696 RVA: 0x00336B56 File Offset: 0x00334D56
		public bool CanContinueConverting()
		{
			return base.master.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x06008B71 RID: 35697 RVA: 0x00336B68 File Offset: 0x00334D68
		public bool IsEmpty()
		{
			return base.master.storage.IsEmpty();
		}

		// Token: 0x06008B72 RID: 35698 RVA: 0x00336B7A File Offset: 0x00334D7A
		public void ResetWorkable()
		{
			CompostWorkable component = base.master.GetComponent<CompostWorkable>();
			component.ShowProgressBar(false);
			component.WorkTimeRemaining = component.GetWorkTime();
		}
	}

	// Token: 0x02001498 RID: 5272
	public class States : GameStateMachine<Compost.States, Compost.StatesInstance, Compost>
	{
		// Token: 0x06008B73 RID: 35699 RVA: 0x00336B9C File Offset: 0x00334D9C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.insufficientMass, (Compost.StatesInstance smi) => !smi.IsEmpty()).EventTransition(GameHashes.OperationalChanged, this.disabledEmpty, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("off");
			this.insufficientMass.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => smi.IsEmpty()).EventTransition(GameHashes.OnStorageChange, this.inert, (Compost.StatesInstance smi) => smi.CanStartConverting()).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("idle_half");
			this.inert.EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("on").ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingCompostFlip, null).ToggleChore(new Func<Compost.StatesInstance, Chore>(this.CreateFlipChore), this.composting);
			this.composting.Enter("Composting", delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => !smi.CanContinueConverting()).EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ScheduleGoTo((Compost.StatesInstance smi) => smi.master.flipInterval, this.inert).Exit(delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.disabled.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.inert, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.disabledEmpty.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.empty, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
		}

		// Token: 0x06008B74 RID: 35700 RVA: 0x00336F2C File Offset: 0x0033512C
		private Chore CreateFlipChore(Compost.StatesInstance smi)
		{
			return new WorkChore<CompostWorkable>(Db.Get().ChoreTypes.FlipCompost, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04006A6F RID: 27247
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State empty;

		// Token: 0x04006A70 RID: 27248
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State insufficientMass;

		// Token: 0x04006A71 RID: 27249
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabled;

		// Token: 0x04006A72 RID: 27250
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabledEmpty;

		// Token: 0x04006A73 RID: 27251
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State inert;

		// Token: 0x04006A74 RID: 27252
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State composting;
	}
}
