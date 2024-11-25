using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009E6 RID: 2534
[SerializationConfig(MemberSerialization.OptIn)]
public class Phonobox : StateMachineComponent<Phonobox.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600497B RID: 18811 RVA: 0x001A4D68 File Offset: 0x001A2F68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new PhonoboxWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("PhonoboxWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			PhonoboxWorkable phonoboxWorkable = go.AddOrGet<PhonoboxWorkable>();
			phonoboxWorkable.owner = this;
			this.workables[i] = phonoboxWorkable;
		}
	}

	// Token: 0x0600497C RID: 18812 RVA: 0x001A4E50 File Offset: 0x001A3050
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

	// Token: 0x0600497D RID: 18813 RVA: 0x001A4EA4 File Offset: 0x001A30A4
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
		WorkChore<PhonoboxWorkable> workChore = new WorkChore<PhonoboxWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	// Token: 0x0600497E RID: 18814 RVA: 0x001A4F0C File Offset: 0x001A310C
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x0600497F RID: 18815 RVA: 0x001A4F28 File Offset: 0x001A3128
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

	// Token: 0x06004980 RID: 18816 RVA: 0x001A4F87 File Offset: 0x001A3187
	public void AddWorker(WorkerBase player)
	{
		this.players.Add(player);
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x06004981 RID: 18817 RVA: 0x001A4FBE File Offset: 0x001A31BE
	public void RemoveWorker(WorkerBase player)
	{
		this.players.Remove(player);
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x06004982 RID: 18818 RVA: 0x001A4FF8 File Offset: 0x001A31F8
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Danced", true);
		return list;
	}

	// Token: 0x04003011 RID: 12305
	public const string SPECIFIC_EFFECT = "Danced";

	// Token: 0x04003012 RID: 12306
	public const string TRACKING_EFFECT = "RecentlyDanced";

	// Token: 0x04003013 RID: 12307
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0),
		new CellOffset(-2, 0),
		new CellOffset(2, 0)
	};

	// Token: 0x04003014 RID: 12308
	private PhonoboxWorkable[] workables;

	// Token: 0x04003015 RID: 12309
	private Chore[] chores;

	// Token: 0x04003016 RID: 12310
	private HashSet<WorkerBase> players = new HashSet<WorkerBase>();

	// Token: 0x04003017 RID: 12311
	private static string[] building_anims = new string[]
	{
		"working_loop",
		"working_loop2",
		"working_loop3"
	};

	// Token: 0x020019E8 RID: 6632
	public class States : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox>
	{
		// Token: 0x06009E69 RID: 40553 RVA: 0x00377BF0 File Offset: 0x00375DF0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(false);
			}).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
			this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", delegate(Phonobox.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(Phonobox.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).DefaultState(this.operational.stopped);
			this.operational.stopped.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(false);
			}).ParamTransition<int>(this.playerCount, this.operational.pre, (Phonobox.StatesInstance smi, int p) => p > 0).PlayAnim("on");
			this.operational.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
			this.operational.playing.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(true);
			}).ScheduleGoTo(25f, this.operational.song_end).ParamTransition<int>(this.playerCount, this.operational.post, (Phonobox.StatesInstance smi, int p) => p == 0).PlayAnim(new Func<Phonobox.StatesInstance, string>(Phonobox.States.GetPlayAnim), KAnim.PlayMode.Loop);
			this.operational.song_end.ParamTransition<int>(this.playerCount, this.operational.bridge, (Phonobox.StatesInstance smi, int p) => p > 0).ParamTransition<int>(this.playerCount, this.operational.post, (Phonobox.StatesInstance smi, int p) => p == 0);
			this.operational.bridge.PlayAnim("working_trans").OnAnimQueueComplete(this.operational.playing);
			this.operational.post.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.stopped);
		}

		// Token: 0x06009E6A RID: 40554 RVA: 0x00377EA8 File Offset: 0x003760A8
		public static string GetPlayAnim(Phonobox.StatesInstance smi)
		{
			int num = UnityEngine.Random.Range(0, Phonobox.building_anims.Length);
			return Phonobox.building_anims[num];
		}

		// Token: 0x04007AD4 RID: 31444
		public StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.IntParameter playerCount;

		// Token: 0x04007AD5 RID: 31445
		public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State unoperational;

		// Token: 0x04007AD6 RID: 31446
		public Phonobox.States.OperationalStates operational;

		// Token: 0x020025CD RID: 9677
		public class OperationalStates : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State
		{
			// Token: 0x0400A842 RID: 43074
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State stopped;

			// Token: 0x0400A843 RID: 43075
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State pre;

			// Token: 0x0400A844 RID: 43076
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State bridge;

			// Token: 0x0400A845 RID: 43077
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State playing;

			// Token: 0x0400A846 RID: 43078
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State song_end;

			// Token: 0x0400A847 RID: 43079
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State post;
		}
	}

	// Token: 0x020019E9 RID: 6633
	public class StatesInstance : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.GameInstance
	{
		// Token: 0x06009E6C RID: 40556 RVA: 0x00377ED2 File Offset: 0x003760D2
		public StatesInstance(Phonobox smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x06009E6D RID: 40557 RVA: 0x00377EEC File Offset: 0x003760EC
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x04007AD7 RID: 31447
		private FetchChore chore;

		// Token: 0x04007AD8 RID: 31448
		private Operational operational;
	}
}
