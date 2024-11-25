using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000957 RID: 2391
public class MechanicalSurfboard : StateMachineComponent<MechanicalSurfboard.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060045DE RID: 17886 RVA: 0x0018D778 File Offset: 0x0018B978
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060045DF RID: 17887 RVA: 0x0018D78B File Offset: 0x0018B98B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060045E0 RID: 17888 RVA: 0x0018D794 File Offset: 0x0018B994
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		list.Add(new Descriptor(BUILDINGS.PREFABS.MECHANICALSURFBOARD.WATER_REQUIREMENT.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.minOperationalWaterKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.MECHANICALSURFBOARD.WATER_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.minOperationalWaterKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(BUILDINGS.PREFABS.MECHANICALSURFBOARD.LEAK_REQUIREMENT.Replace("{amount}", GameUtil.GetFormattedMass(this.waterSpillRateKG, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.MECHANICALSURFBOARD.LEAK_REQUIREMENT_TOOLTIP.Replace("{amount}", GameUtil.GetFormattedMass(this.waterSpillRateKG, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false).IncreaseIndent());
		return list;
	}

	// Token: 0x04002D72 RID: 11634
	public string specificEffect;

	// Token: 0x04002D73 RID: 11635
	public string trackingEffect;

	// Token: 0x04002D74 RID: 11636
	public float waterSpillRateKG;

	// Token: 0x04002D75 RID: 11637
	public float minOperationalWaterKG;

	// Token: 0x04002D76 RID: 11638
	public string[] interactAnims = new string[]
	{
		"anim_interacts_mechanical_surfboard_kanim",
		"anim_interacts_mechanical_surfboard2_kanim",
		"anim_interacts_mechanical_surfboard3_kanim"
	};

	// Token: 0x020018BE RID: 6334
	public class States : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard>
	{
		// Token: 0x060099BE RID: 39358 RVA: 0x0036AF78 File Offset: 0x00369178
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.inoperational, true).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Transition.ConditionCallback(this.IsReady)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GettingReady, null);
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<MechanicalSurfboard.StatesInstance, Chore>(this.CreateChore), this.operational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((MechanicalSurfboard.StatesInstance smi) => smi.master.GetComponent<MechanicalSurfboardWorkable>(), this.ready.working).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Not(new StateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((MechanicalSurfboard.StatesInstance smi) => smi.master.GetComponent<MechanicalSurfboardWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x060099BF RID: 39359 RVA: 0x0036B144 File Offset: 0x00369344
		private Chore CreateChore(MechanicalSurfboard.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<MechanicalSurfboardWorkable>();
			WorkChore<MechanicalSurfboardWorkable> workChore = new WorkChore<MechanicalSurfboardWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x060099C0 RID: 39360 RVA: 0x0036B1A4 File Offset: 0x003693A4
		private bool IsReady(MechanicalSurfboard.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= smi.master.minOperationalWaterKG;
		}

		// Token: 0x04007744 RID: 30532
		private GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State inoperational;

		// Token: 0x04007745 RID: 30533
		private GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State operational;

		// Token: 0x04007746 RID: 30534
		private MechanicalSurfboard.States.ReadyStates ready;

		// Token: 0x020025B3 RID: 9651
		public class ReadyStates : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State
		{
			// Token: 0x0400A7F9 RID: 43001
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State idle;

			// Token: 0x0400A7FA RID: 43002
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State working;

			// Token: 0x0400A7FB RID: 43003
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State post;
		}
	}

	// Token: 0x020018BF RID: 6335
	public class StatesInstance : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.GameInstance
	{
		// Token: 0x060099C2 RID: 39362 RVA: 0x0036B1EB File Offset: 0x003693EB
		public StatesInstance(MechanicalSurfboard smi) : base(smi)
		{
		}
	}
}
