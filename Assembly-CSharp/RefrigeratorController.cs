using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000759 RID: 1881
public class RefrigeratorController : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>
{
	// Token: 0x06003262 RID: 12898 RVA: 0x00114AD4 File Offset: 0x00112CD4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.DefaultState(this.operational.steady).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Not(new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational))).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(false, false);
		});
		this.operational.cooling.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplyCoolingExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.steady, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AllFoodCool), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeCooling, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main);
		this.operational.steady.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplySteadyExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.cooling, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AnyWarmFood), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeSteady, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(true);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(false);
		});
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x00114D04 File Offset: 0x00112F04
	private bool AllFoodCool(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStopBuffer)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003264 RID: 12900 RVA: 0x00114DA4 File Offset: 0x00112FA4
	private bool AnyWarmFood(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStartBuffer)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003265 RID: 12901 RVA: 0x00114E44 File Offset: 0x00113044
	private bool IsOperational(RefrigeratorController.StatesInstance smi)
	{
		return smi.operational.IsOperational;
	}

	// Token: 0x04001DCF RID: 7631
	public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State inoperational;

	// Token: 0x04001DD0 RID: 7632
	public RefrigeratorController.OperationalStates operational;

	// Token: 0x020015D7 RID: 5591
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600900F RID: 36879 RVA: 0x0034A608 File Offset: 0x00348808
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.AddRange(SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature));
			Descriptor item = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.coolingHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item);
			return list;
		}

		// Token: 0x04006DF5 RID: 28149
		public float activeCoolingStartBuffer = 2f;

		// Token: 0x04006DF6 RID: 28150
		public float activeCoolingStopBuffer = 0.1f;

		// Token: 0x04006DF7 RID: 28151
		public float simulatedInternalTemperature = 274.15f;

		// Token: 0x04006DF8 RID: 28152
		public float simulatedInternalHeatCapacity = 400f;

		// Token: 0x04006DF9 RID: 28153
		public float simulatedThermalConductivity = 1000f;

		// Token: 0x04006DFA RID: 28154
		public float powerSaverEnergyUsage;

		// Token: 0x04006DFB RID: 28155
		public float coolingHeatKW;

		// Token: 0x04006DFC RID: 28156
		public float steadyHeatKW;
	}

	// Token: 0x020015D8 RID: 5592
	public class OperationalStates : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State
	{
		// Token: 0x04006DFD RID: 28157
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State cooling;

		// Token: 0x04006DFE RID: 28158
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State steady;
	}

	// Token: 0x020015D9 RID: 5593
	public class StatesInstance : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.GameInstance
	{
		// Token: 0x06009012 RID: 36882 RVA: 0x0034A6BC File Offset: 0x003488BC
		public StatesInstance(IStateMachineTarget master, RefrigeratorController.Def def) : base(master, def)
		{
			this.temperatureAdjuster = new SimulatedTemperatureAdjuster(def.simulatedInternalTemperature, def.simulatedInternalHeatCapacity, def.simulatedThermalConductivity, this.storage);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		}

		// Token: 0x06009013 RID: 36883 RVA: 0x0034A70A File Offset: 0x0034890A
		protected override void OnCleanUp()
		{
			this.temperatureAdjuster.CleanUp();
			base.OnCleanUp();
		}

		// Token: 0x06009014 RID: 36884 RVA: 0x0034A71D File Offset: 0x0034891D
		public float GetSaverPower()
		{
			return base.def.powerSaverEnergyUsage;
		}

		// Token: 0x06009015 RID: 36885 RVA: 0x0034A72A File Offset: 0x0034892A
		public float GetNormalPower()
		{
			return base.GetComponent<EnergyConsumer>().WattsNeededWhenActive;
		}

		// Token: 0x06009016 RID: 36886 RVA: 0x0034A738 File Offset: 0x00348938
		public void SetEnergySaver(bool energySaving)
		{
			EnergyConsumer component = base.GetComponent<EnergyConsumer>();
			if (energySaving)
			{
				component.BaseWattageRating = this.GetSaverPower();
				return;
			}
			component.BaseWattageRating = this.GetNormalPower();
		}

		// Token: 0x06009017 RID: 36887 RVA: 0x0034A768 File Offset: 0x00348968
		public void ApplyCoolingExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.coolingHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x06009018 RID: 36888 RVA: 0x0034A792 File Offset: 0x00348992
		public void ApplySteadyExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.steadyHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x04006DFF RID: 28159
		[MyCmpReq]
		public Operational operational;

		// Token: 0x04006E00 RID: 28160
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04006E01 RID: 28161
		private HandleVector<int>.Handle structureTemperature;

		// Token: 0x04006E02 RID: 28162
		private SimulatedTemperatureAdjuster temperatureAdjuster;
	}
}
