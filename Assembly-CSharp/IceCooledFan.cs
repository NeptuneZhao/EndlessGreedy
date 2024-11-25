using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006F1 RID: 1777
[SerializationConfig(MemberSerialization.OptIn)]
public class IceCooledFan : StateMachineComponent<IceCooledFan.StatesInstance>
{
	// Token: 0x06002D55 RID: 11605 RVA: 0x000FE60A File Offset: 0x000FC80A
	public bool HasMaterial()
	{
		this.UpdateMeter();
		return this.iceStorage.MassStored() > 0f;
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x000FE624 File Offset: 0x000FC824
	public void CheckWorking()
	{
		if (base.smi.master.workable.worker == null)
		{
			base.smi.GoTo(base.smi.sm.unworkable);
		}
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x000FE660 File Offset: 0x000FC860
	private void UpdateUnworkableStatusItems()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (!base.smi.EnvironmentNeedsCooling())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, this.minCooledTemperature);
			}
		}
		else if (component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, false);
		}
		if (!base.smi.EnvironmentHighEnoughPressure())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.UnderPressure, this.minEnvironmentMass);
				return;
			}
		}
		else if (component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.UnderPressure, false);
		}
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x000FE760 File Offset: 0x000FC960
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_waterbody",
			"meter_waterlevel"
		});
		base.smi.StartSM();
		base.GetComponent<ManualDeliveryKG>().SetStorage(this.iceStorage);
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x000FE7CC File Offset: 0x000FC9CC
	private void UpdateMeter()
	{
		float num = 0f;
		foreach (GameObject gameObject in this.iceStorage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			num += component.Temperature;
		}
		num /= (float)this.iceStorage.items.Count;
		float num2 = Mathf.Clamp01((num - this.LOW_ICE_TEMP) / (this.targetTemperature - this.LOW_ICE_TEMP));
		this.meter.SetPositionPercent(1f - num2);
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x000FE874 File Offset: 0x000FCA74
	private void DoCooling(float dt)
	{
		float kilowatts = this.coolingRate * dt;
		foreach (GameObject gameObject in this.iceStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), kilowatts, this.targetTemperature);
		}
		for (int i = this.iceStorage.items.Count; i > 0; i--)
		{
			GameObject gameObject2 = this.iceStorage.items[i - 1];
			if (gameObject2 != null && gameObject2.GetComponent<PrimaryElement>().Temperature > gameObject2.GetComponent<PrimaryElement>().Element.highTemp && gameObject2.GetComponent<PrimaryElement>().Element.HasTransitionUp)
			{
				PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
				this.iceStorage.AddLiquid(component.Element.highTempTransitionTarget, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
				this.iceStorage.ConsumeIgnoringDisease(gameObject2);
			}
		}
		for (int j = this.iceStorage.items.Count; j > 0; j--)
		{
			GameObject gameObject3 = this.iceStorage.items[j - 1];
			if (gameObject3 != null && gameObject3.GetComponent<PrimaryElement>().Temperature >= this.targetTemperature)
			{
				this.iceStorage.Transfer(gameObject3, this.liquidStorage, true, true);
			}
		}
		if (!this.liquidStorage.IsEmpty())
		{
			this.liquidStorage.DropAll(false, false, new Vector3(1f, 0f, 0f), true, null);
		}
		this.UpdateMeter();
	}

	// Token: 0x04001A42 RID: 6722
	[SerializeField]
	public float minCooledTemperature;

	// Token: 0x04001A43 RID: 6723
	[SerializeField]
	public float minEnvironmentMass;

	// Token: 0x04001A44 RID: 6724
	[SerializeField]
	public float coolingRate;

	// Token: 0x04001A45 RID: 6725
	[SerializeField]
	public float targetTemperature;

	// Token: 0x04001A46 RID: 6726
	[SerializeField]
	public Vector2I minCoolingRange;

	// Token: 0x04001A47 RID: 6727
	[SerializeField]
	public Vector2I maxCoolingRange;

	// Token: 0x04001A48 RID: 6728
	[SerializeField]
	public Storage iceStorage;

	// Token: 0x04001A49 RID: 6729
	[SerializeField]
	public Storage liquidStorage;

	// Token: 0x04001A4A RID: 6730
	[SerializeField]
	public Tag consumptionTag;

	// Token: 0x04001A4B RID: 6731
	private float LOW_ICE_TEMP = 173.15f;

	// Token: 0x04001A4C RID: 6732
	[MyCmpAdd]
	private IceCooledFanWorkable workable;

	// Token: 0x04001A4D RID: 6733
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001A4E RID: 6734
	private MeterController meter;

	// Token: 0x02001520 RID: 5408
	public class StatesInstance : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.GameInstance
	{
		// Token: 0x06008D4E RID: 36174 RVA: 0x0033EDE5 File Offset: 0x0033CFE5
		public StatesInstance(IceCooledFan smi) : base(smi)
		{
		}

		// Token: 0x06008D4F RID: 36175 RVA: 0x0033EDF0 File Offset: 0x0033CFF0
		public bool IsWorkable()
		{
			bool result = false;
			if (base.master.operational.IsOperational && this.EnvironmentNeedsCooling() && base.smi.master.HasMaterial() && base.smi.EnvironmentHighEnoughPressure())
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06008D50 RID: 36176 RVA: 0x0033EE3C File Offset: 0x0033D03C
		public bool EnvironmentNeedsCooling()
		{
			bool result = false;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			for (int i = base.master.minCoolingRange.y; i < base.master.maxCoolingRange.y; i++)
			{
				for (int j = base.master.minCoolingRange.x; j < base.master.maxCoolingRange.x; j++)
				{
					CellOffset offset = new CellOffset(j, i);
					int i2 = Grid.OffsetCell(cell, offset);
					if (Grid.Temperature[i2] > base.master.minCooledTemperature)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06008D51 RID: 36177 RVA: 0x0033EEE4 File Offset: 0x0033D0E4
		public bool EnvironmentHighEnoughPressure()
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			for (int i = base.master.minCoolingRange.y; i < base.master.maxCoolingRange.y; i++)
			{
				for (int j = base.master.minCoolingRange.x; j < base.master.maxCoolingRange.x; j++)
				{
					CellOffset offset = new CellOffset(j, i);
					int i2 = Grid.OffsetCell(cell, offset);
					if (Grid.Mass[i2] >= base.master.minEnvironmentMass)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x02001521 RID: 5409
	public class States : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan>
	{
		// Token: 0x06008D52 RID: 36178 RVA: 0x0033EF84 File Offset: 0x0033D184
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unworkable;
			this.root.Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.workable.SetWorkTime(float.PositiveInfinity);
			});
			this.workable.ToggleChore(new Func<IceCooledFan.StatesInstance, Chore>(this.CreateUseChore), this.work_pst).EventTransition(GameHashes.ActiveChanged, this.workable.cooling, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker != null).EventTransition(GameHashes.OperationalChanged, this.workable.cooling, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker != null).Transition(this.unworkable, (IceCooledFan.StatesInstance smi) => !smi.IsWorkable(), UpdateRate.SIM_200ms);
			this.workable.cooling.EventTransition(GameHashes.OperationalChanged, this.unworkable, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker == null).EventHandler(GameHashes.ActiveChanged, delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.CheckWorking();
			}).Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
				if (!smi.EnvironmentNeedsCooling() || !smi.master.HasMaterial() || !smi.EnvironmentHighEnoughPressure())
				{
					smi.GoTo(this.unworkable);
				}
			}).Update("IceCooledFanCooling", delegate(IceCooledFan.StatesInstance smi, float dt)
			{
				smi.master.DoCooling(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(IceCooledFan.StatesInstance smi)
			{
				if (!smi.master.HasMaterial())
				{
					smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Working");
				}
				smi.master.liquidStorage.DropAll(false, false, default(Vector3), true, null);
			});
			this.work_pst.ScheduleGoTo(2f, this.unworkable);
			this.unworkable.Update("IceFanUnworkableStatusItems", delegate(IceCooledFan.StatesInstance smi, float dt)
			{
				smi.master.UpdateUnworkableStatusItems();
			}, UpdateRate.SIM_200ms, false).Transition(this.workable.waiting, (IceCooledFan.StatesInstance smi) => smi.IsWorkable(), UpdateRate.SIM_200ms).Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.UpdateUnworkableStatusItems();
			}).Exit(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.UpdateUnworkableStatusItems();
			});
		}

		// Token: 0x06008D53 RID: 36179 RVA: 0x0033F1FC File Offset: 0x0033D3FC
		private Chore CreateUseChore(IceCooledFan.StatesInstance smi)
		{
			return new WorkChore<IceCooledFanWorkable>(Db.Get().ChoreTypes.IceCooledFan, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04006BFD RID: 27645
		public IceCooledFan.States.Workable workable;

		// Token: 0x04006BFE RID: 27646
		public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State unworkable;

		// Token: 0x04006BFF RID: 27647
		public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State work_pst;

		// Token: 0x020024F4 RID: 9460
		public class Workable : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State
		{
			// Token: 0x0400A457 RID: 42071
			public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State waiting;

			// Token: 0x0400A458 RID: 42072
			public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State cooling;
		}
	}
}
