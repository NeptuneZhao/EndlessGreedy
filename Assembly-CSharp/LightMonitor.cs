﻿using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200098C RID: 2444
public class LightMonitor : GameStateMachine<LightMonitor, LightMonitor.Instance>
{
	// Token: 0x0600474F RID: 18255 RVA: 0x001980D4 File Offset: 0x001962D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unburnt;
		this.root.EventTransition(GameHashes.SicknessAdded, this.burnt, (LightMonitor.Instance smi) => smi.gameObject.GetSicknesses().Has(Db.Get().Sicknesses.Sunburn)).Update(new Action<LightMonitor.Instance, float>(LightMonitor.CheckLightLevel), UpdateRate.SIM_1000ms, false);
		this.unburnt.DefaultState(this.unburnt.safe).ParamTransition<float>(this.burnResistance, this.get_burnt, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsLTEZero);
		this.unburnt.safe.DefaultState(this.unburnt.safe.unlit).Update(delegate(LightMonitor.Instance smi, float dt)
		{
			smi.sm.burnResistance.DeltaClamp(dt * 0.25f, 0f, DUPLICANTSTATS.STANDARD.Light.SUNBURN_DELAY_TIME, smi);
		}, UpdateRate.SIM_200ms, false);
		this.unburnt.safe.unlit.ParamTransition<float>(this.lightLevel, this.unburnt.safe.normal_light, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.unburnt.safe.normal_light.ParamTransition<float>(this.lightLevel, this.unburnt.safe.unlit, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ParamTransition<float>(this.lightLevel, this.unburnt.safe.sunlight, (LightMonitor.Instance smi, float p) => p >= (float)DUPLICANTSTATS.STANDARD.Light.LUX_PLEASANT_LIGHT);
		this.unburnt.safe.sunlight.ParamTransition<float>(this.lightLevel, this.unburnt.safe.normal_light, (LightMonitor.Instance smi, float p) => p < (float)DUPLICANTSTATS.STANDARD.Light.LUX_PLEASANT_LIGHT).ParamTransition<float>(this.lightLevel, this.unburnt.burning, (LightMonitor.Instance smi, float p) => p >= (float)DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN).ToggleEffect("Sunlight_Pleasant");
		this.unburnt.burning.ParamTransition<float>(this.lightLevel, this.unburnt.safe.sunlight, (LightMonitor.Instance smi, float p) => p < (float)DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN).Update(delegate(LightMonitor.Instance smi, float dt)
		{
			smi.sm.burnResistance.DeltaClamp(-dt, 0f, DUPLICANTSTATS.STANDARD.Light.SUNBURN_DELAY_TIME, smi);
		}, UpdateRate.SIM_200ms, false).ToggleEffect("Sunlight_Burning");
		this.get_burnt.Enter(delegate(LightMonitor.Instance smi)
		{
			smi.gameObject.GetSicknesses().Infect(new SicknessExposureInfo(Db.Get().Sicknesses.Sunburn.Id, DUPLICANTS.DISEASES.SUNBURNSICKNESS.SUNEXPOSURE));
		}).GoTo(this.burnt);
		this.burnt.EventTransition(GameHashes.SicknessCured, this.unburnt, (LightMonitor.Instance smi) => !smi.gameObject.GetSicknesses().Has(Db.Get().Sicknesses.Sunburn)).Exit(delegate(LightMonitor.Instance smi)
		{
			smi.sm.burnResistance.Set(DUPLICANTSTATS.STANDARD.Light.SUNBURN_DELAY_TIME, smi, false);
		});
	}

	// Token: 0x06004750 RID: 18256 RVA: 0x001983D4 File Offset: 0x001965D4
	private static void CheckLightLevel(LightMonitor.Instance smi, float dt)
	{
		KPrefabID component = smi.GetComponent<KPrefabID>();
		if (component != null && component.HasTag(GameTags.Shaded))
		{
			smi.sm.lightLevel.Set(0f, smi, false);
			return;
		}
		int num = Grid.PosToCell(smi.gameObject);
		if (Grid.IsValidCell(num))
		{
			smi.sm.lightLevel.Set((float)Grid.LightIntensity[num], smi, false);
		}
	}

	// Token: 0x04002E91 RID: 11921
	public const float BURN_RESIST_RECOVERY_FACTOR = 0.25f;

	// Token: 0x04002E92 RID: 11922
	public StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter lightLevel;

	// Token: 0x04002E93 RID: 11923
	public StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter burnResistance = new StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter(DUPLICANTSTATS.STANDARD.Light.SUNBURN_DELAY_TIME);

	// Token: 0x04002E94 RID: 11924
	public LightMonitor.UnburntStates unburnt;

	// Token: 0x04002E95 RID: 11925
	public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State get_burnt;

	// Token: 0x04002E96 RID: 11926
	public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State burnt;

	// Token: 0x0200194E RID: 6478
	public class UnburntStates : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007915 RID: 30997
		public LightMonitor.SafeStates safe;

		// Token: 0x04007916 RID: 30998
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State burning;
	}

	// Token: 0x0200194F RID: 6479
	public class SafeStates : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007917 RID: 30999
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State unlit;

		// Token: 0x04007918 RID: 31000
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State normal_light;

		// Token: 0x04007919 RID: 31001
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State sunlight;
	}

	// Token: 0x02001950 RID: 6480
	public new class Instance : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C16 RID: 39958 RVA: 0x003719BE File Offset: 0x0036FBBE
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x0400791A RID: 31002
		public Effects effects;
	}
}
