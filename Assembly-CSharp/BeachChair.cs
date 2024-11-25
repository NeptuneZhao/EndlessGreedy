using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200065E RID: 1630
public class BeachChair : StateMachineComponent<BeachChair.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600282E RID: 10286 RVA: 0x000E413D File Offset: 0x000E233D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600282F RID: 10287 RVA: 0x000E4150 File Offset: 0x000E2350
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002830 RID: 10288 RVA: 0x000E4158 File Offset: 0x000E2358
	public static void AddModifierDescriptions(List<Descriptor> descs, string effect_id, bool high_lux)
	{
		Klei.AI.Modifier modifier = Db.Get().effects.Get(effect_id);
		LocString locString = high_lux ? BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_HIGH : BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_LOW;
		LocString locString2 = high_lux ? BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_HIGH_TOOLTIP : BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_LOW_TOOLTIP;
		foreach (AttributeModifier attributeModifier in modifier.SelfModifiers)
		{
			Descriptor item = new Descriptor(locString.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()).Replace("{lux}", GameUtil.GetFormattedLux(BeachChairConfig.TAN_LUX)), locString2.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()).Replace("{lux}", GameUtil.GetFormattedLux(BeachChairConfig.TAN_LUX)), Descriptor.DescriptorType.Effect, false);
			item.IncreaseIndent();
			descs.Add(item);
		}
	}

	// Token: 0x06002831 RID: 10289 RVA: 0x000E4298 File Offset: 0x000E2498
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		BeachChair.AddModifierDescriptions(list, this.specificEffectLit, true);
		BeachChair.AddModifierDescriptions(list, this.specificEffectUnlit, false);
		return list;
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x000E42E5 File Offset: 0x000E24E5
	public void SetLit(bool v)
	{
		base.smi.sm.lit.Set(v, base.smi, false);
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x000E4305 File Offset: 0x000E2505
	public void SetWorker(WorkerBase worker)
	{
		base.smi.sm.worker.Set(worker, base.smi);
	}

	// Token: 0x04001728 RID: 5928
	public string specificEffectUnlit;

	// Token: 0x04001729 RID: 5929
	public string specificEffectLit;

	// Token: 0x0400172A RID: 5930
	public string trackingEffect;

	// Token: 0x0400172B RID: 5931
	public const float LIT_RATIO_FOR_POSITIVE_EFFECT = 0.75f;

	// Token: 0x02001438 RID: 5176
	public class States : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair>
	{
		// Token: 0x060089C5 RID: 35269 RVA: 0x003313E0 File Offset: 0x0032F5E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<BeachChair.StatesInstance, Chore>(this.CreateChore), this.inoperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.working_pre);
			this.ready.working_pre.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Target(this.worker).PlayAnim("working_pre").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_lit);
					return;
				}
				smi.GoTo(this.ready.working_unlit);
			});
			this.ready.working_unlit.DefaultState(this.ready.working_unlit.working).Enter(delegate(BeachChair.StatesInstance smi)
			{
				BeachChairWorkable component = smi.master.GetComponent<BeachChairWorkable>();
				component.workingPstComplete = (component.workingPstFailed = this.UNLIT_PST_ANIMS);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.TanningLightInsufficient, null).WorkableStopTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.post).Target(this.worker).PlayAnim("working_unlit_pre");
			this.ready.working_unlit.working.ParamTransition<bool>(this.lit, this.ready.working_unlit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsTrue).Target(this.worker).QueueAnim("working_unlit_loop", true, null);
			this.ready.working_unlit.post.Target(this.worker).PlayAnim("working_unlit_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_lit);
					return;
				}
				smi.GoTo(this.ready.working_unlit.working);
			});
			this.ready.working_lit.DefaultState(this.ready.working_lit.working).Enter(delegate(BeachChair.StatesInstance smi)
			{
				BeachChairWorkable component = smi.master.GetComponent<BeachChairWorkable>();
				component.workingPstComplete = (component.workingPstFailed = this.LIT_PST_ANIMS);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.TanningLightSufficient, null).WorkableStopTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.post).Target(this.worker).PlayAnim("working_lit_pre");
			this.ready.working_lit.working.ParamTransition<bool>(this.lit, this.ready.working_lit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsFalse).Target(this.worker).QueueAnim("working_lit_loop", true, null).ScheduleGoTo((BeachChair.StatesInstance smi) => UnityEngine.Random.Range(5f, 15f), this.ready.working_lit.silly);
			this.ready.working_lit.silly.ParamTransition<bool>(this.lit, this.ready.working_lit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsFalse).Target(this.worker).PlayAnim((BeachChair.StatesInstance smi) => this.SILLY_ANIMS[UnityEngine.Random.Range(0, this.SILLY_ANIMS.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.ready.working_lit.working);
			this.ready.working_lit.post.Target(this.worker).PlayAnim("working_lit_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (!this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_unlit);
					return;
				}
				smi.GoTo(this.ready.working_lit.working);
			});
			this.ready.post.PlayAnim("working_pst").Exit(delegate(BeachChair.StatesInstance smi)
			{
				this.worker.Set(null, smi);
			}).OnAnimQueueComplete(this.ready);
		}

		// Token: 0x060089C6 RID: 35270 RVA: 0x003317FC File Offset: 0x0032F9FC
		private Chore CreateChore(BeachChair.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<BeachChairWorkable>();
			WorkChore<BeachChairWorkable> workChore = new WorkChore<BeachChairWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x04006933 RID: 26931
		public StateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.BoolParameter lit;

		// Token: 0x04006934 RID: 26932
		public StateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.TargetParameter worker;

		// Token: 0x04006935 RID: 26933
		private GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State inoperational;

		// Token: 0x04006936 RID: 26934
		private BeachChair.States.ReadyStates ready;

		// Token: 0x04006937 RID: 26935
		private HashedString[] UNLIT_PST_ANIMS = new HashedString[]
		{
			"working_unlit_pst",
			"working_pst"
		};

		// Token: 0x04006938 RID: 26936
		private HashedString[] LIT_PST_ANIMS = new HashedString[]
		{
			"working_lit_pst",
			"working_pst"
		};

		// Token: 0x04006939 RID: 26937
		private string[] SILLY_ANIMS = new string[]
		{
			"working_lit_loop1",
			"working_lit_loop2",
			"working_lit_loop3"
		};

		// Token: 0x020024B1 RID: 9393
		public class LitWorkingStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x0400A2A2 RID: 41634
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working;

			// Token: 0x0400A2A3 RID: 41635
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State silly;

			// Token: 0x0400A2A4 RID: 41636
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}

		// Token: 0x020024B2 RID: 9394
		public class WorkingStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x0400A2A5 RID: 41637
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working;

			// Token: 0x0400A2A6 RID: 41638
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}

		// Token: 0x020024B3 RID: 9395
		public class ReadyStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x0400A2A7 RID: 41639
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State idle;

			// Token: 0x0400A2A8 RID: 41640
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working_pre;

			// Token: 0x0400A2A9 RID: 41641
			public BeachChair.States.WorkingStates working_unlit;

			// Token: 0x0400A2AA RID: 41642
			public BeachChair.States.LitWorkingStates working_lit;

			// Token: 0x0400A2AB RID: 41643
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}
	}

	// Token: 0x02001439 RID: 5177
	public class StatesInstance : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.GameInstance
	{
		// Token: 0x060089CF RID: 35279 RVA: 0x00331A12 File Offset: 0x0032FC12
		public StatesInstance(BeachChair smi) : base(smi)
		{
		}
	}
}
