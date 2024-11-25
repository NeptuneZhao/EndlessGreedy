using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000898 RID: 2200
public class EspressoMachine : StateMachineComponent<EspressoMachine.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003DB4 RID: 15796 RVA: 0x00155058 File Offset: 0x00153258
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x001550AC File Offset: 0x001532AC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x001550B4 File Offset: 0x001532B4
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x0015511C File Offset: 0x0015331C
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Espresso", true);
		this.AddRequirementDesc(list, EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE);
		this.AddRequirementDesc(list, GameTags.Water, EspressoMachine.WATER_MASS_PER_USE);
		return list;
	}

	// Token: 0x040025A6 RID: 9638
	public const string SPECIFIC_EFFECT = "Espresso";

	// Token: 0x040025A7 RID: 9639
	public const string TRACKING_EFFECT = "RecentlyRecDrink";

	// Token: 0x040025A8 RID: 9640
	public static Tag INGREDIENT_TAG = new Tag("SpiceNut");

	// Token: 0x040025A9 RID: 9641
	public static float INGREDIENT_MASS_PER_USE = 1f;

	// Token: 0x040025AA RID: 9642
	public static float WATER_MASS_PER_USE = 1f;

	// Token: 0x02001797 RID: 6039
	public class States : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine>
	{
		// Token: 0x0600962A RID: 38442 RVA: 0x003610C8 File Offset: 0x0035F2C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<EspressoMachine.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((EspressoMachine.StatesInstance smi) => smi.master.GetComponent<EspressoMachineWorkable>(), this.ready.working).Transition(this.operational, GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Not(new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Not(new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((EspressoMachine.StatesInstance smi) => smi.master.GetComponent<EspressoMachineWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0600962B RID: 38443 RVA: 0x0036128C File Offset: 0x0035F48C
		private Chore CreateChore(EspressoMachine.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<EspressoMachineWorkable>();
			WorkChore<EspressoMachineWorkable> workChore = new WorkChore<EspressoMachineWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600962C RID: 38444 RVA: 0x003612EC File Offset: 0x0035F4EC
		private bool IsReady(EspressoMachine.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= EspressoMachine.WATER_MASS_PER_USE && smi.GetComponent<Storage>().GetAmountAvailable(EspressoMachine.INGREDIENT_TAG) >= EspressoMachine.INGREDIENT_MASS_PER_USE;
		}

		// Token: 0x04007327 RID: 29479
		private GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State unoperational;

		// Token: 0x04007328 RID: 29480
		private GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State operational;

		// Token: 0x04007329 RID: 29481
		private EspressoMachine.States.ReadyStates ready;

		// Token: 0x0200258D RID: 9613
		public class ReadyStates : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State
		{
			// Token: 0x0400A73B RID: 42811
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State idle;

			// Token: 0x0400A73C RID: 42812
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State working;

			// Token: 0x0400A73D RID: 42813
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State post;
		}
	}

	// Token: 0x02001798 RID: 6040
	public class StatesInstance : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.GameInstance
	{
		// Token: 0x0600962E RID: 38446 RVA: 0x00361346 File Offset: 0x0035F546
		public StatesInstance(EspressoMachine smi) : base(smi)
		{
		}
	}
}
