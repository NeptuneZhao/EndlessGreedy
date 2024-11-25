using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000AA7 RID: 2727
public class SodaFountain : StateMachineComponent<SodaFountain.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06005042 RID: 20546 RVA: 0x001CD618 File Offset: 0x001CB818
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x06005043 RID: 20547 RVA: 0x001CD66C File Offset: 0x001CB86C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005044 RID: 20548 RVA: 0x001CD674 File Offset: 0x001CB874
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06005045 RID: 20549 RVA: 0x001CD6DC File Offset: 0x001CB8DC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		this.AddRequirementDesc(list, this.ingredientTag, this.ingredientMassPerUse);
		this.AddRequirementDesc(list, GameTags.Water, this.waterMassPerUse);
		return list;
	}

	// Token: 0x04003553 RID: 13651
	public string specificEffect;

	// Token: 0x04003554 RID: 13652
	public string trackingEffect;

	// Token: 0x04003555 RID: 13653
	public Tag ingredientTag;

	// Token: 0x04003556 RID: 13654
	public float ingredientMassPerUse;

	// Token: 0x04003557 RID: 13655
	public float waterMassPerUse;

	// Token: 0x02001AD9 RID: 6873
	public class States : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain>
	{
		// Token: 0x0600A149 RID: 41289 RVA: 0x00382A14 File Offset: 0x00380C14
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<SodaFountain.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.Transition(this.operational, GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Not(new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Not(new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady))).WorkableStartTransition((SodaFountain.StatesInstance smi) => smi.master.GetComponent<SodaFountainWorkable>(), this.ready.working);
			this.ready.working.PlayAnim("working_pre").WorkableStopTransition((SodaFountain.StatesInstance smi) => smi.master.GetComponent<SodaFountainWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0600A14A RID: 41290 RVA: 0x00382BC0 File Offset: 0x00380DC0
		private Chore CreateChore(SodaFountain.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<SodaFountainWorkable>();
			WorkChore<SodaFountainWorkable> workChore = new WorkChore<SodaFountainWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600A14B RID: 41291 RVA: 0x00382C20 File Offset: 0x00380E20
		private bool IsReady(SodaFountain.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= smi.master.waterMassPerUse && smi.GetComponent<Storage>().GetAmountAvailable(smi.master.ingredientTag) >= smi.master.ingredientMassPerUse;
		}

		// Token: 0x04007DE9 RID: 32233
		private GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State unoperational;

		// Token: 0x04007DEA RID: 32234
		private GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State operational;

		// Token: 0x04007DEB RID: 32235
		private SodaFountain.States.ReadyStates ready;

		// Token: 0x02002608 RID: 9736
		public class ReadyStates : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State
		{
			// Token: 0x0400A942 RID: 43330
			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State idle;

			// Token: 0x0400A943 RID: 43331
			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State working;

			// Token: 0x0400A944 RID: 43332
			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State post;
		}
	}

	// Token: 0x02001ADA RID: 6874
	public class StatesInstance : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.GameInstance
	{
		// Token: 0x0600A14D RID: 41293 RVA: 0x00382C8C File Offset: 0x00380E8C
		public StatesInstance(SodaFountain smi) : base(smi)
		{
		}
	}
}
