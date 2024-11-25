using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000982 RID: 2434
public class GasLiquidExposureMonitor : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>
{
	// Token: 0x0600471C RID: 18204 RVA: 0x00196C64 File Offset: 0x00194E64
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		this.root.Update(new Action<GasLiquidExposureMonitor.Instance, float>(this.UpdateExposure), UpdateRate.SIM_33ms, false);
		this.normal.ParamTransition<bool>(this.isIrritated, this.irritated, (GasLiquidExposureMonitor.Instance smi, bool p) => this.isIrritated.Get(smi));
		this.irritated.ParamTransition<bool>(this.isIrritated, this.normal, (GasLiquidExposureMonitor.Instance smi, bool p) => !this.isIrritated.Get(smi)).ToggleStatusItem(Db.Get().DuplicantStatusItems.GasLiquidIrritation, (GasLiquidExposureMonitor.Instance smi) => smi).DefaultState(this.irritated.irritated);
		this.irritated.irritated.Transition(this.irritated.rubbingEyes, new StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Transition.ConditionCallback(GasLiquidExposureMonitor.CanReact), UpdateRate.SIM_200ms);
		this.irritated.rubbingEyes.Exit(delegate(GasLiquidExposureMonitor.Instance smi)
		{
			smi.lastReactTime = GameClock.Instance.GetTime();
		}).ToggleReactable((GasLiquidExposureMonitor.Instance smi) => smi.GetReactable()).OnSignal(this.reactFinished, this.irritated.irritated);
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x00196DB1 File Offset: 0x00194FB1
	private static bool CanReact(GasLiquidExposureMonitor.Instance smi)
	{
		return GameClock.Instance.GetTime() > smi.lastReactTime + 60f;
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x00196DCC File Offset: 0x00194FCC
	private static void InitializeCustomRates()
	{
		if (GasLiquidExposureMonitor.customExposureRates != null)
		{
			return;
		}
		GasLiquidExposureMonitor.minorIrritationEffect = Db.Get().effects.Get("MinorIrritation");
		GasLiquidExposureMonitor.majorIrritationEffect = Db.Get().effects.Get("MajorIrritation");
		GasLiquidExposureMonitor.customExposureRates = new Dictionary<SimHashes, float>();
		float value = -1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Water] = value;
		float value2 = -0.25f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CarbonDioxide] = value2;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen] = value2;
		float value3 = 0f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ContaminatedOxygen] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.DirtyWater] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ViscoGel] = value3;
		float value4 = 0.5f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Hydrogen] = value4;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SaltWater] = value4;
		float value5 = 1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ChlorineGas] = value5;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.EthanolGas] = value5;
		float value6 = 3f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Chlorine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SourGas] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Brine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Ethanol] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SuperCoolant] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CrudeOil] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Naphtha] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Petroleum] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Mercury] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.MercuryGas] = value6;
	}

	// Token: 0x0600471F RID: 18207 RVA: 0x00196F90 File Offset: 0x00195190
	public float GetCurrentExposure(GasLiquidExposureMonitor.Instance smi)
	{
		float result;
		if (GasLiquidExposureMonitor.customExposureRates.TryGetValue(smi.CurrentlyExposedToElement().id, out result))
		{
			return result;
		}
		return 0f;
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x00196FC0 File Offset: 0x001951C0
	private void UpdateExposure(GasLiquidExposureMonitor.Instance smi, float dt)
	{
		GasLiquidExposureMonitor.InitializeCustomRates();
		float exposureRate = 0f;
		smi.isInAirtightEnvironment = false;
		smi.isImmuneToIrritability = false;
		int num = Grid.CellAbove(Grid.PosToCell(smi.gameObject));
		if (Grid.IsValidCell(num))
		{
			Element element = Grid.Element[num];
			float num2;
			if (!GasLiquidExposureMonitor.customExposureRates.TryGetValue(element.id, out num2))
			{
				if (Grid.Temperature[num] >= -13657.5f && Grid.Temperature[num] <= 27315f)
				{
					num2 = 1f;
				}
				else
				{
					num2 = 2f;
				}
			}
			if (smi.effects.HasImmunityTo(GasLiquidExposureMonitor.minorIrritationEffect) || smi.effects.HasImmunityTo(GasLiquidExposureMonitor.majorIrritationEffect))
			{
				smi.isImmuneToIrritability = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if ((smi.master.gameObject.HasTag(GameTags.HasSuitTank) && smi.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit()) || smi.master.gameObject.HasTag(GameTags.InTransitTube))
			{
				smi.isInAirtightEnvironment = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if (!smi.isInAirtightEnvironment && !smi.isImmuneToIrritability)
			{
				if (element.IsGas)
				{
					exposureRate = num2 * Grid.Mass[num] / 1f;
				}
				else if (element.IsLiquid)
				{
					exposureRate = num2 * Grid.Mass[num] / 1000f;
				}
			}
		}
		smi.exposureRate = exposureRate;
		smi.exposure += smi.exposureRate * dt;
		smi.exposure = MathUtil.Clamp(0f, 30f, smi.exposure);
		this.ApplyEffects(smi);
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x00197170 File Offset: 0x00195370
	private void ApplyEffects(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.minorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else if (smi.IsMajorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.majorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else
		{
			smi.effects.Remove(GasLiquidExposureMonitor.minorIrritationEffect);
			smi.effects.Remove(GasLiquidExposureMonitor.majorIrritationEffect);
			this.isIrritated.Set(false, smi, false);
		}
	}

	// Token: 0x06004722 RID: 18210 RVA: 0x00197202 File Offset: 0x00195402
	public Effect GetAppliedEffect(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			return GasLiquidExposureMonitor.minorIrritationEffect;
		}
		if (smi.IsMajorIrritation())
		{
			return GasLiquidExposureMonitor.majorIrritationEffect;
		}
		return null;
	}

	// Token: 0x04002E5C RID: 11868
	public const float MIN_REACT_INTERVAL = 60f;

	// Token: 0x04002E5D RID: 11869
	private static Dictionary<SimHashes, float> customExposureRates;

	// Token: 0x04002E5E RID: 11870
	private static Effect minorIrritationEffect;

	// Token: 0x04002E5F RID: 11871
	private static Effect majorIrritationEffect;

	// Token: 0x04002E60 RID: 11872
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.BoolParameter isIrritated;

	// Token: 0x04002E61 RID: 11873
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Signal reactFinished;

	// Token: 0x04002E62 RID: 11874
	public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State normal;

	// Token: 0x04002E63 RID: 11875
	public GasLiquidExposureMonitor.IrritatedStates irritated;

	// Token: 0x02001933 RID: 6451
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001934 RID: 6452
	public class TUNING
	{
		// Token: 0x040078B0 RID: 30896
		public const float MINOR_IRRITATION_THRESHOLD = 8f;

		// Token: 0x040078B1 RID: 30897
		public const float MAJOR_IRRITATION_THRESHOLD = 15f;

		// Token: 0x040078B2 RID: 30898
		public const float MAX_EXPOSURE = 30f;

		// Token: 0x040078B3 RID: 30899
		public const float GAS_UNITS = 1f;

		// Token: 0x040078B4 RID: 30900
		public const float LIQUID_UNITS = 1000f;

		// Token: 0x040078B5 RID: 30901
		public const float REDUCE_EXPOSURE_RATE_FAST = -1f;

		// Token: 0x040078B6 RID: 30902
		public const float REDUCE_EXPOSURE_RATE_SLOW = -0.25f;

		// Token: 0x040078B7 RID: 30903
		public const float NO_CHANGE = 0f;

		// Token: 0x040078B8 RID: 30904
		public const float SLOW_EXPOSURE_RATE = 0.5f;

		// Token: 0x040078B9 RID: 30905
		public const float NORMAL_EXPOSURE_RATE = 1f;

		// Token: 0x040078BA RID: 30906
		public const float QUICK_EXPOSURE_RATE = 3f;

		// Token: 0x040078BB RID: 30907
		public const float DEFAULT_MIN_TEMPERATURE = -13657.5f;

		// Token: 0x040078BC RID: 30908
		public const float DEFAULT_MAX_TEMPERATURE = 27315f;

		// Token: 0x040078BD RID: 30909
		public const float DEFAULT_LOW_RATE = 1f;

		// Token: 0x040078BE RID: 30910
		public const float DEFAULT_HIGH_RATE = 2f;
	}

	// Token: 0x02001935 RID: 6453
	public class IrritatedStates : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State
	{
		// Token: 0x040078BF RID: 30911
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State irritated;

		// Token: 0x040078C0 RID: 30912
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State rubbingEyes;
	}

	// Token: 0x02001936 RID: 6454
	public new class Instance : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.GameInstance
	{
		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06009B99 RID: 39833 RVA: 0x0036FF4B File Offset: 0x0036E14B
		public float minorIrritationThreshold
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06009B9A RID: 39834 RVA: 0x0036FF52 File Offset: 0x0036E152
		public Instance(IStateMachineTarget master, GasLiquidExposureMonitor.Def def) : base(master, def)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x06009B9B RID: 39835 RVA: 0x0036FF68 File Offset: 0x0036E168
		public Reactable GetReactable()
		{
			Emote iritatedEyes = Db.Get().Emotes.Minion.IritatedEyes;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "IrritatedEyes", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(iritatedEyes);
			selfEmoteReactable.preventChoreInterruption = true;
			selfEmoteReactable.RegisterEmoteStepCallbacks("irritated_eyes", null, delegate(GameObject go)
			{
				base.sm.reactFinished.Trigger(this);
			});
			return selfEmoteReactable;
		}

		// Token: 0x06009B9C RID: 39836 RVA: 0x0036FFF4 File Offset: 0x0036E1F4
		public bool IsMinorIrritation()
		{
			return this.exposure >= 8f && this.exposure < 15f;
		}

		// Token: 0x06009B9D RID: 39837 RVA: 0x00370012 File Offset: 0x0036E212
		public bool IsMajorIrritation()
		{
			return this.exposure >= 15f;
		}

		// Token: 0x06009B9E RID: 39838 RVA: 0x00370024 File Offset: 0x0036E224
		public Element CurrentlyExposedToElement()
		{
			if (this.isInAirtightEnvironment)
			{
				return ElementLoader.GetElement(SimHashes.Oxygen.CreateTag());
			}
			int num = Grid.CellAbove(Grid.PosToCell(base.smi.gameObject));
			return Grid.Element[num];
		}

		// Token: 0x06009B9F RID: 39839 RVA: 0x00370066 File Offset: 0x0036E266
		public void ResetExposure()
		{
			this.exposure = 0f;
		}

		// Token: 0x040078C1 RID: 30913
		[Serialize]
		public float exposure;

		// Token: 0x040078C2 RID: 30914
		[Serialize]
		public float lastReactTime;

		// Token: 0x040078C3 RID: 30915
		[Serialize]
		public float exposureRate;

		// Token: 0x040078C4 RID: 30916
		public Effects effects;

		// Token: 0x040078C5 RID: 30917
		public bool isInAirtightEnvironment;

		// Token: 0x040078C6 RID: 30918
		public bool isImmuneToIrritability;
	}
}
