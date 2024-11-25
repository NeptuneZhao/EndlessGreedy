using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000934 RID: 2356
public class Juicer : StateMachineComponent<Juicer.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600446F RID: 17519 RVA: 0x00185818 File Offset: 0x00183A18
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x06004470 RID: 17520 RVA: 0x0018586C File Offset: 0x00183A6C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004471 RID: 17521 RVA: 0x00185874 File Offset: 0x00183A74
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		string arg2 = (EdiblesManager.GetFoodInfo(tag.Name) != null) ? GameUtil.GetFormattedCaloriesForItem(tag, mass, GameUtil.TimeSlice.None, true) : GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, arg2), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, arg2), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06004472 RID: 17522 RVA: 0x001858EC File Offset: 0x00183AEC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		for (int i = 0; i < this.ingredientTags.Length; i++)
		{
			this.AddRequirementDesc(list, this.ingredientTags[i], this.ingredientMassesPerUse[i]);
		}
		this.AddRequirementDesc(list, GameTags.Water, this.waterMassPerUse);
		return list;
	}

	// Token: 0x04002CC5 RID: 11461
	public string specificEffect;

	// Token: 0x04002CC6 RID: 11462
	public string trackingEffect;

	// Token: 0x04002CC7 RID: 11463
	public Tag[] ingredientTags;

	// Token: 0x04002CC8 RID: 11464
	public float[] ingredientMassesPerUse;

	// Token: 0x04002CC9 RID: 11465
	public float waterMassPerUse;

	// Token: 0x02001892 RID: 6290
	public class States : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer>
	{
		// Token: 0x060098EE RID: 39150 RVA: 0x00369224 File Offset: 0x00367424
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<Juicer.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.Transition(this.operational, GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Not(new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Not(new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady))).PlayAnim("on").WorkableStartTransition((Juicer.StatesInstance smi) => smi.master.GetComponent<JuicerWorkable>(), this.ready.working);
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((Juicer.StatesInstance smi) => smi.master.GetComponent<JuicerWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x060098EF RID: 39151 RVA: 0x003693E8 File Offset: 0x003675E8
		private Chore CreateChore(Juicer.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<JuicerWorkable>();
			WorkChore<JuicerWorkable> workChore = new WorkChore<JuicerWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x060098F0 RID: 39152 RVA: 0x00369448 File Offset: 0x00367648
		private bool IsReady(Juicer.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			if (primaryElement == null)
			{
				return false;
			}
			if (primaryElement.Mass < smi.master.waterMassPerUse)
			{
				return false;
			}
			for (int i = 0; i < smi.master.ingredientTags.Length; i++)
			{
				if (smi.GetComponent<Storage>().GetAmountAvailable(smi.master.ingredientTags[i]) < smi.master.ingredientMassesPerUse[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400768C RID: 30348
		private GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State unoperational;

		// Token: 0x0400768D RID: 30349
		private GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State operational;

		// Token: 0x0400768E RID: 30350
		private Juicer.States.ReadyStates ready;

		// Token: 0x020025AE RID: 9646
		public class ReadyStates : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State
		{
			// Token: 0x0400A7E6 RID: 42982
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State idle;

			// Token: 0x0400A7E7 RID: 42983
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State working;

			// Token: 0x0400A7E8 RID: 42984
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State post;
		}
	}

	// Token: 0x02001893 RID: 6291
	public class StatesInstance : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.GameInstance
	{
		// Token: 0x060098F2 RID: 39154 RVA: 0x003694D4 File Offset: 0x003676D4
		public StatesInstance(Juicer smi) : base(smi)
		{
		}
	}
}
