using System;
using STRINGS;
using UnityEngine;

// Token: 0x020006AD RID: 1709
public class ContactConductivePipeBridge : GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>
{
	// Token: 0x06002B06 RID: 11014 RVA: 0x000F1B10 File Offset: 0x000EFD10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.noLiquid;
		this.noLiquid.PlayAnim("off", KAnim.PlayMode.Once).ParamTransition<float>(this.noLiquidTimer, this.withLiquid, GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.IsGTZero);
		this.withLiquid.Update(new Action<ContactConductivePipeBridge.Instance, float>(ContactConductivePipeBridge.ExpirationTimerUpdate), UpdateRate.SIM_200ms, false).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<float>(this.noLiquidTimer, this.noLiquid, GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.IsLTEZero);
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x000F1B88 File Offset: 0x000EFD88
	private static void ExpirationTimerUpdate(ContactConductivePipeBridge.Instance smi, float dt)
	{
		float num = smi.sm.noLiquidTimer.Get(smi);
		num -= dt;
		smi.sm.noLiquidTimer.Set(num, smi, false);
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x000F1BC0 File Offset: 0x000EFDC0
	private static float CalculateMaxWattsTransfered(float buildingTemperature, float building_thermal_conductivity, float content_temperature, float content_thermal_conductivity)
	{
		float num = 1f;
		float num2 = 1f;
		float num3 = 50f;
		float num4 = content_temperature - buildingTemperature;
		float num5 = (content_thermal_conductivity + building_thermal_conductivity) * 0.5f;
		return num4 * num5 * num * num3 / num2;
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x000F1BF4 File Offset: 0x000EFDF4
	private static float GetKilloJoulesTransfered(float maxWattsTransfered, float dt, float building_Temperature, float building_heat_capacity, float content_temperature, float content_heat_capacity)
	{
		float num = maxWattsTransfered * dt / 1000f;
		float min = Mathf.Min(content_temperature, building_Temperature);
		float max = Mathf.Max(content_temperature, building_Temperature);
		float value = content_temperature - num / content_heat_capacity;
		float num2 = building_Temperature + num / building_heat_capacity;
		float num3 = Mathf.Clamp(value, min, max);
		num2 = Mathf.Clamp(num2, min, max);
		float num4 = Mathf.Abs(num3 - content_temperature);
		float num5 = Mathf.Abs(num2 - building_Temperature);
		float a = num4 * content_heat_capacity;
		float b = num5 * building_heat_capacity;
		return Mathf.Min(a, b) * Mathf.Sign(maxWattsTransfered);
	}

	// Token: 0x06002B0A RID: 11018 RVA: 0x000F1C68 File Offset: 0x000EFE68
	private static float GetFinalContentTemperature(float KJT, float building_Temperature, float building_heat_capacity, float content_temperature, float content_heat_capacity)
	{
		float num = -KJT;
		float num2 = Mathf.Max(0f, content_temperature + num / content_heat_capacity);
		float num3 = Mathf.Max(0f, building_Temperature - num / building_heat_capacity);
		if ((content_temperature - building_Temperature) * (num2 - num3) < 0f)
		{
			return content_temperature * content_heat_capacity / (content_heat_capacity + building_heat_capacity) + building_Temperature * building_heat_capacity / (content_heat_capacity + building_heat_capacity);
		}
		return num2;
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x000F1CBC File Offset: 0x000EFEBC
	private static float GetFinalBuildingTemperature(float content_temperature, float content_final_temperature, float content_heat_capacity, float building_temperature, float building_heat_capacity)
	{
		float num = (content_temperature - content_final_temperature) * content_heat_capacity;
		float min = Mathf.Min(content_temperature, building_temperature);
		float max = Mathf.Max(content_temperature, building_temperature);
		float num2 = num / building_heat_capacity;
		return Mathf.Clamp(building_temperature + num2, min, max);
	}

	// Token: 0x040018B2 RID: 6322
	private const string loopAnimName = "on";

	// Token: 0x040018B3 RID: 6323
	private const string loopAnim_noWater = "off";

	// Token: 0x040018B4 RID: 6324
	private GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.State withLiquid;

	// Token: 0x040018B5 RID: 6325
	private GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.State noLiquid;

	// Token: 0x040018B6 RID: 6326
	private StateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.FloatParameter noLiquidTimer;

	// Token: 0x0200149F RID: 5279
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006A7B RID: 27259
		public ConduitType type = ConduitType.Liquid;

		// Token: 0x04006A7C RID: 27260
		public float pumpKGRate;
	}

	// Token: 0x020014A0 RID: 5280
	public new class Instance : GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.GameInstance
	{
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06008B93 RID: 35731 RVA: 0x003370A6 File Offset: 0x003352A6
		public Tag tag
		{
			get
			{
				if (this.type != ConduitType.Liquid)
				{
					return GameTags.Gas;
				}
				return GameTags.Liquid;
			}
		}

		// Token: 0x06008B94 RID: 35732 RVA: 0x003370BC File Offset: 0x003352BC
		public Instance(IStateMachineTarget master, ContactConductivePipeBridge.Def def) : base(master, def)
		{
		}

		// Token: 0x06008B95 RID: 35733 RVA: 0x003370DC File Offset: 0x003352DC
		public override void StartSM()
		{
			base.StartSM();
			this.inputCell = this.building.GetUtilityInputCell();
			this.outputCell = this.building.GetUtilityOutputCell();
			this.structureHandle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
			Conduit.GetFlowManager(this.type).AddConduitUpdater(new Action<float>(this.Flow), ConduitFlowPriority.Default);
		}

		// Token: 0x06008B96 RID: 35734 RVA: 0x00337144 File Offset: 0x00335344
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new Action<float>(this.Flow));
		}

		// Token: 0x06008B97 RID: 35735 RVA: 0x00337168 File Offset: 0x00335368
		private void Flow(float dt)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
			if (flowManager.HasConduit(this.inputCell) && flowManager.HasConduit(this.outputCell))
			{
				ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
				ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.outputCell);
				float num = Mathf.Min(contents.mass, base.def.pumpKGRate * dt);
				if (flowManager.CanMergeContents(contents, contents2, num))
				{
					base.smi.sm.noLiquidTimer.Set(1.5f, base.smi, false);
					float amountAllowedForMerging = flowManager.GetAmountAllowedForMerging(contents, contents2, num);
					if (amountAllowedForMerging > 0f)
					{
						float temperature = this.ExchangeStorageTemperatureWithBuilding(contents, amountAllowedForMerging, dt);
						float num2 = ((base.def.type == ConduitType.Liquid) ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow).AddElement(this.outputCell, contents.element, amountAllowedForMerging, temperature, contents.diseaseIdx, contents.diseaseCount);
						if (amountAllowedForMerging != num2)
						{
							global::Debug.Log("Mass Differs By: " + (amountAllowedForMerging - num2).ToString());
						}
						flowManager.RemoveElement(this.inputCell, num2);
					}
				}
			}
		}

		// Token: 0x06008B98 RID: 35736 RVA: 0x003372A4 File Offset: 0x003354A4
		private float ExchangeStorageTemperatureWithBuilding(ConduitFlow.ConduitContents content, float mass, float dt)
		{
			PrimaryElement component = this.building.GetComponent<PrimaryElement>();
			float building_thermal_conductivity = component.Element.thermalConductivity * this.building.Def.ThermalConductivity;
			if (mass > 0f)
			{
				Element element = ElementLoader.FindElementByHash(content.element);
				float content_heat_capacity = mass * element.specificHeatCapacity;
				float num = this.building.Def.MassForTemperatureModification * component.Element.specificHeatCapacity;
				float temperature = component.Temperature;
				float temperature2 = content.temperature;
				float num2 = ContactConductivePipeBridge.CalculateMaxWattsTransfered(temperature, building_thermal_conductivity, temperature2, element.thermalConductivity);
				float finalContentTemperature = ContactConductivePipeBridge.GetFinalContentTemperature(ContactConductivePipeBridge.GetKilloJoulesTransfered(num2, dt, temperature, num, temperature2, content_heat_capacity), temperature, num, temperature2, content_heat_capacity);
				float finalBuildingTemperature = ContactConductivePipeBridge.GetFinalBuildingTemperature(temperature2, finalContentTemperature, content_heat_capacity, temperature, num);
				float delta_kilojoules = Mathf.Sign(num2) * Mathf.Abs(finalBuildingTemperature - temperature) * num;
				if ((finalBuildingTemperature >= 0f && finalBuildingTemperature <= 10000f) & (finalContentTemperature >= 0f && finalContentTemperature <= 10000f))
				{
					GameComps.StructureTemperatures.ProduceEnergy(base.smi.structureHandle, delta_kilojoules, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time);
					return finalContentTemperature;
				}
			}
			return 0f;
		}

		// Token: 0x04006A7D RID: 27261
		public ConduitType type = ConduitType.Liquid;

		// Token: 0x04006A7E RID: 27262
		public HandleVector<int>.Handle structureHandle;

		// Token: 0x04006A7F RID: 27263
		public int inputCell = -1;

		// Token: 0x04006A80 RID: 27264
		public int outputCell = -1;

		// Token: 0x04006A81 RID: 27265
		[MyCmpGet]
		public Building building;
	}
}
