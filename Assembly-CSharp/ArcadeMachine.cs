using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200064B RID: 1611
[SerializationConfig(MemberSerialization.OptIn)]
public class ArcadeMachine : StateMachineComponent<ArcadeMachine.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002764 RID: 10084 RVA: 0x000E04E8 File Offset: 0x000DE6E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new ArcadeMachineWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("ArcadeMachineWorkable", pos);
			ArcadeMachineWorkable arcadeMachineWorkable = go.AddOrGet<ArcadeMachineWorkable>();
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			int player_index = i;
			ArcadeMachineWorkable arcadeMachineWorkable2 = arcadeMachineWorkable;
			arcadeMachineWorkable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(arcadeMachineWorkable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(delegate(Workable workable, Workable.WorkableEvent ev)
			{
				this.OnWorkableEvent(player_index, ev);
			}));
			arcadeMachineWorkable.overrideAnims = this.overrideAnims[i];
			arcadeMachineWorkable.workAnims = this.workAnims[i];
			this.workables[i] = arcadeMachineWorkable;
			this.workables[i].owner = this;
		}
		base.smi.StartSM();
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x000E062C File Offset: 0x000DE82C
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x000E0680 File Offset: 0x000DE880
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<ArcadeMachineWorkable> workChore = new WorkChore<ArcadeMachineWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x000E06E8 File Offset: 0x000DE8E8
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x06002768 RID: 10088 RVA: 0x000E0704 File Offset: 0x000DE904
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06002769 RID: 10089 RVA: 0x000E0764 File Offset: 0x000DE964
	public void OnWorkableEvent(int player, Workable.WorkableEvent ev)
	{
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			this.players.Add(player);
		}
		else
		{
			this.players.Remove(player);
		}
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x0600276A RID: 10090 RVA: 0x000E07BC File Offset: 0x000DE9BC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "PlayedArcade", true);
		return list;
	}

	// Token: 0x040016B6 RID: 5814
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040016B7 RID: 5815
	private ArcadeMachineWorkable[] workables;

	// Token: 0x040016B8 RID: 5816
	private Chore[] chores;

	// Token: 0x040016B9 RID: 5817
	public HashSet<int> players = new HashSet<int>();

	// Token: 0x040016BA RID: 5818
	public KAnimFile[][] overrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_arcade_cabinet_playerone_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_arcade_cabinet_playertwo_kanim")
		}
	};

	// Token: 0x040016BB RID: 5819
	public HashedString[][] workAnims = new HashedString[][]
	{
		new HashedString[]
		{
			"working_pre",
			"working_loop_one_p"
		},
		new HashedString[]
		{
			"working_pre",
			"working_loop_two_p"
		}
	};

	// Token: 0x02001429 RID: 5161
	public class States : GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine>
	{
		// Token: 0x06008997 RID: 35223 RVA: 0x00330EC4 File Offset: 0x0032F0C4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.Enter(delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.SetActive(false);
			}).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
			this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).DefaultState(this.operational.stopped);
			this.operational.stopped.Enter(delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("on").ParamTransition<int>(this.playerCount, this.operational.pre, (ArcadeMachine.StatesInstance smi, int p) => p > 0);
			this.operational.pre.Enter(delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.SetActive(true);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
			this.operational.playing.PlayAnim(new Func<ArcadeMachine.StatesInstance, string>(this.GetPlayingAnim), KAnim.PlayMode.Loop).ParamTransition<int>(this.playerCount, this.operational.post, (ArcadeMachine.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.playerCount, this.operational.playing_coop, (ArcadeMachine.StatesInstance smi, int p) => p > 1);
			this.operational.playing_coop.PlayAnim(new Func<ArcadeMachine.StatesInstance, string>(this.GetPlayingAnim), KAnim.PlayMode.Loop).ParamTransition<int>(this.playerCount, this.operational.post, (ArcadeMachine.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.playerCount, this.operational.playing, (ArcadeMachine.StatesInstance smi, int p) => p == 1);
			this.operational.post.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.stopped);
		}

		// Token: 0x06008998 RID: 35224 RVA: 0x00331188 File Offset: 0x0032F388
		private string GetPlayingAnim(ArcadeMachine.StatesInstance smi)
		{
			bool flag = smi.master.players.Contains(0);
			bool flag2 = smi.master.players.Contains(1);
			if (flag && !flag2)
			{
				return "working_loop_one_p";
			}
			if (flag2 && !flag)
			{
				return "working_loop_two_p";
			}
			return "working_loop_coop_p";
		}

		// Token: 0x040068FB RID: 26875
		public StateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.IntParameter playerCount;

		// Token: 0x040068FC RID: 26876
		public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State unoperational;

		// Token: 0x040068FD RID: 26877
		public ArcadeMachine.States.OperationalStates operational;

		// Token: 0x020024AF RID: 9391
		public class OperationalStates : GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State
		{
			// Token: 0x0400A292 RID: 41618
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State stopped;

			// Token: 0x0400A293 RID: 41619
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State pre;

			// Token: 0x0400A294 RID: 41620
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State playing;

			// Token: 0x0400A295 RID: 41621
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State playing_coop;

			// Token: 0x0400A296 RID: 41622
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State post;
		}
	}

	// Token: 0x0200142A RID: 5162
	public class StatesInstance : GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.GameInstance
	{
		// Token: 0x0600899A RID: 35226 RVA: 0x003311DE File Offset: 0x0032F3DE
		public StatesInstance(ArcadeMachine smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x0600899B RID: 35227 RVA: 0x003311F8 File Offset: 0x0032F3F8
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x040068FE RID: 26878
		private Operational operational;
	}
}
