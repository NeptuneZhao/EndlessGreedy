using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007FD RID: 2045
public class CropSleepingMonitor : GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>
{
	// Token: 0x0600388D RID: 14477 RVA: 0x00134B08 File Offset: 0x00132D08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.awake;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.Update("CropSleepingMonitor.root", delegate(CropSleepingMonitor.Instance smi, float dt)
		{
			int cell = Grid.PosToCell(smi.master.gameObject);
			GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State state = smi.IsCellSafe(cell) ? this.awake : this.sleeping;
			smi.GoTo(state);
		}, UpdateRate.SIM_1000ms, false);
		this.sleeping.TriggerOnEnter(GameHashes.CropSleep, null).ToggleStatusItem(Db.Get().CreatureStatusItems.CropSleeping, (CropSleepingMonitor.Instance smi) => smi);
		this.awake.TriggerOnEnter(GameHashes.CropWakeUp, null);
	}

	// Token: 0x040021F4 RID: 8692
	public GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State sleeping;

	// Token: 0x040021F5 RID: 8693
	public GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State awake;

	// Token: 0x020016E1 RID: 5857
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060093D3 RID: 37843 RVA: 0x0035A270 File Offset: 0x00358470
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			if (this.prefersDarkness)
			{
				return new List<Descriptor>
				{
					new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_DARKNESS, Descriptor.DescriptorType.Requirement, false)
				};
			}
			Klei.AI.Attribute minLightLux = Db.Get().PlantAttributes.MinLightLux;
			AttributeInstance attributeInstance = minLightLux.Lookup(obj);
			int lux = Mathf.RoundToInt((attributeInstance != null) ? attributeInstance.GetTotalValue() : obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(minLightLux));
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(lux)), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(lux)), Descriptor.DescriptorType.Requirement, false)
			};
		}

		// Token: 0x04007117 RID: 28951
		public bool prefersDarkness;
	}

	// Token: 0x020016E2 RID: 5858
	public new class Instance : GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.GameInstance
	{
		// Token: 0x060093D5 RID: 37845 RVA: 0x0035A325 File Offset: 0x00358525
		public Instance(IStateMachineTarget master, CropSleepingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060093D6 RID: 37846 RVA: 0x0035A32F File Offset: 0x0035852F
		public bool IsSleeping()
		{
			return this.GetCurrentState() == base.smi.sm.sleeping;
		}

		// Token: 0x060093D7 RID: 37847 RVA: 0x0035A34C File Offset: 0x0035854C
		public bool IsCellSafe(int cell)
		{
			AttributeInstance attributeInstance = Db.Get().PlantAttributes.MinLightLux.Lookup(base.gameObject);
			int num = Grid.LightIntensity[cell];
			if (!base.def.prefersDarkness)
			{
				return (float)num >= attributeInstance.GetTotalValue();
			}
			return num == 0;
		}
	}
}
