using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class ActiveParticleConsumer : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>
{
	// Token: 0x06000185 RID: 389 RVA: 0x0000A4D4 File Offset: 0x000086D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.root.Enter(delegate(ActiveParticleConsumer.Instance smi)
		{
			smi.GetComponent<Operational>().SetFlag(ActiveParticleConsumer.canConsumeParticlesFlag, false);
		});
		this.inoperational.EventTransition(GameHashes.OnParticleStorageChanged, this.operational, new StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback(this.IsReady)).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForHighEnergyParticles, null);
		this.operational.DefaultState(this.operational.waiting).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational, GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Not(new StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback(this.IsReady))).ToggleOperationalFlag(ActiveParticleConsumer.canConsumeParticlesFlag);
		this.operational.waiting.EventTransition(GameHashes.ActiveChanged, this.operational.consuming, (ActiveParticleConsumer.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.operational.consuming.EventTransition(GameHashes.ActiveChanged, this.operational.waiting, (ActiveParticleConsumer.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(ActiveParticleConsumer.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000A634 File Offset: 0x00008834
	public bool IsReady(ActiveParticleConsumer.Instance smi)
	{
		return smi.storage.Particles >= smi.def.minParticlesForOperational;
	}

	// Token: 0x040000EB RID: 235
	public static readonly Operational.Flag canConsumeParticlesFlag = new Operational.Flag("canConsumeParticles", Operational.Flag.Type.Requirement);

	// Token: 0x040000EC RID: 236
	public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State inoperational;

	// Token: 0x040000ED RID: 237
	public ActiveParticleConsumer.OperationalStates operational;

	// Token: 0x02000F9E RID: 3998
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06007A14 RID: 31252 RVA: 0x00301AF0 File Offset: 0x002FFCF0
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.ACTIVE_PARTICLE_CONSUMPTION.Replace("{Rate}", GameUtil.GetFormattedHighEnergyParticles(this.activeConsumptionRate, GameUtil.TimeSlice.PerSecond, true)), UI.BUILDINGEFFECTS.TOOLTIPS.ACTIVE_PARTICLE_CONSUMPTION.Replace("{Rate}", GameUtil.GetFormattedHighEnergyParticles(this.activeConsumptionRate, GameUtil.TimeSlice.PerSecond, true)), Descriptor.DescriptorType.Requirement, false)
			};
		}

		// Token: 0x04005B2A RID: 23338
		public float activeConsumptionRate = 1f;

		// Token: 0x04005B2B RID: 23339
		public float minParticlesForOperational = 1f;

		// Token: 0x04005B2C RID: 23340
		public string meterSymbolName;
	}

	// Token: 0x02000F9F RID: 3999
	public class OperationalStates : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State
	{
		// Token: 0x04005B2D RID: 23341
		public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State waiting;

		// Token: 0x04005B2E RID: 23342
		public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State consuming;
	}

	// Token: 0x02000FA0 RID: 4000
	public new class Instance : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.GameInstance
	{
		// Token: 0x06007A17 RID: 31255 RVA: 0x00301B6D File Offset: 0x002FFD6D
		public Instance(IStateMachineTarget master, ActiveParticleConsumer.Def def) : base(master, def)
		{
			this.storage = master.GetComponent<HighEnergyParticleStorage>();
		}

		// Token: 0x06007A18 RID: 31256 RVA: 0x00301B83 File Offset: 0x002FFD83
		public void Update(float dt)
		{
			this.storage.ConsumeAndGet(dt * base.def.activeConsumptionRate);
		}

		// Token: 0x04005B2F RID: 23343
		public bool ShowWorkingStatus;

		// Token: 0x04005B30 RID: 23344
		public HighEnergyParticleStorage storage;
	}
}
