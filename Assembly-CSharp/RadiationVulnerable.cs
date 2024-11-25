using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200081B RID: 2075
public class RadiationVulnerable : GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>
{
	// Token: 0x0600396A RID: 14698 RVA: 0x00139258 File Offset: 0x00137458
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.comfortable.Transition(this.too_dark, (RadiationVulnerable.StatesInstance smi) => smi.GetRadiationThresholdCrossed() == -1, UpdateRate.SIM_1000ms).Transition(this.too_bright, (RadiationVulnerable.StatesInstance smi) => smi.GetRadiationThresholdCrossed() == 1, UpdateRate.SIM_1000ms).TriggerOnEnter(GameHashes.RadiationComfort, null);
		this.too_dark.Transition(this.comfortable, (RadiationVulnerable.StatesInstance smi) => smi.GetRadiationThresholdCrossed() != -1, UpdateRate.SIM_1000ms).TriggerOnEnter(GameHashes.RadiationDiscomfort, null);
		this.too_bright.Transition(this.comfortable, (RadiationVulnerable.StatesInstance smi) => smi.GetRadiationThresholdCrossed() != 1, UpdateRate.SIM_1000ms).TriggerOnEnter(GameHashes.RadiationDiscomfort, null);
	}

	// Token: 0x04002291 RID: 8849
	public GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>.State comfortable;

	// Token: 0x04002292 RID: 8850
	public GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>.State too_dark;

	// Token: 0x04002293 RID: 8851
	public GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>.State too_bright;

	// Token: 0x02001728 RID: 5928
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060094D4 RID: 38100 RVA: 0x0035E160 File Offset: 0x0035C360
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			float preModifiedAttributeValue = component.GetPreModifiedAttributeValue(Db.Get().PlantAttributes.MinRadiationThreshold);
			string preModifiedAttributeFormattedValue = component.GetPreModifiedAttributeFormattedValue(Db.Get().PlantAttributes.MinRadiationThreshold);
			string preModifiedAttributeFormattedValue2 = component.GetPreModifiedAttributeFormattedValue(Db.Get().PlantAttributes.MaxRadiationThreshold);
			MutantPlant component2 = go.GetComponent<MutantPlant>();
			bool flag = component2 != null && component2.IsOriginal;
			if (preModifiedAttributeValue <= 0f)
			{
				return new List<Descriptor>
				{
					new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_NO_MIN_RADIATION.Replace("{MaxRads}", preModifiedAttributeFormattedValue2), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_NO_MIN_RADIATION.Replace("{MaxRads}", preModifiedAttributeFormattedValue2) + (flag ? UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP.ToString() : ""), Descriptor.DescriptorType.Requirement, false)
				};
			}
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_RADIATION.Replace("{MinRads}", preModifiedAttributeFormattedValue).Replace("{MaxRads}", preModifiedAttributeFormattedValue2), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RADIATION.Replace("{MinRads}", preModifiedAttributeFormattedValue).Replace("{MaxRads}", preModifiedAttributeFormattedValue2) + (flag ? UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP.ToString() : ""), Descriptor.DescriptorType.Requirement, false)
			};
		}
	}

	// Token: 0x02001729 RID: 5929
	public class StatesInstance : GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>.GameInstance, IWiltCause
	{
		// Token: 0x060094D6 RID: 38102 RVA: 0x0035E290 File Offset: 0x0035C490
		public StatesInstance(IStateMachineTarget master, RadiationVulnerable.Def def) : base(master, def)
		{
			this.minRadiationAttributeInstance = Db.Get().PlantAttributes.MinRadiationThreshold.Lookup(base.gameObject);
			this.maxRadiationAttributeInstance = Db.Get().PlantAttributes.MaxRadiationThreshold.Lookup(base.gameObject);
		}

		// Token: 0x060094D7 RID: 38103 RVA: 0x0035E2E8 File Offset: 0x0035C4E8
		public int GetRadiationThresholdCrossed()
		{
			int num = Grid.PosToCell(base.master.gameObject);
			if (!Grid.IsValidCell(num))
			{
				return 0;
			}
			if (Grid.Radiation[num] < this.minRadiationAttributeInstance.GetTotalValue())
			{
				return -1;
			}
			if (Grid.Radiation[num] <= this.maxRadiationAttributeInstance.GetTotalValue())
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060094D8 RID: 38104 RVA: 0x0035E345 File Offset: 0x0035C545
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Radiation
				};
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060094D9 RID: 38105 RVA: 0x0035E354 File Offset: 0x0035C554
		public string WiltStateString
		{
			get
			{
				if (base.smi.IsInsideState(base.smi.sm.too_dark))
				{
					return Db.Get().CreatureStatusItems.Crop_Too_NonRadiated.GetName(this);
				}
				if (base.smi.IsInsideState(base.smi.sm.too_bright))
				{
					return Db.Get().CreatureStatusItems.Crop_Too_Radiated.GetName(this);
				}
				return "";
			}
		}

		// Token: 0x040071F2 RID: 29170
		private AttributeInstance minRadiationAttributeInstance;

		// Token: 0x040071F3 RID: 29171
		private AttributeInstance maxRadiationAttributeInstance;
	}
}
