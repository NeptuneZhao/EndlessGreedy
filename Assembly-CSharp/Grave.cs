using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006E7 RID: 1767
public class Grave : StateMachineComponent<Grave.StatesInstance>
{
	// Token: 0x06002CFB RID: 11515 RVA: 0x000FCACE File Offset: 0x000FACCE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Grave>(-1697596308, Grave.OnStorageChangedDelegate);
		this.epitaphIdx = UnityEngine.Random.Range(0, int.MaxValue);
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x000FCAF8 File Offset: 0x000FACF8
	protected override void OnSpawn()
	{
		base.GetComponent<Storage>().SetOffsets(Grave.DELIVERY_OFFSETS);
		Storage component = base.GetComponent<Storage>();
		Storage storage = component;
		storage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(storage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		KAnimFile anim = Assets.GetAnim("anim_bury_dupe_kanim");
		int num = 0;
		KAnim.Anim anim2;
		for (;;)
		{
			anim2 = anim.GetData().GetAnim(num);
			if (anim2 == null)
			{
				goto IL_8F;
			}
			if (anim2.name == "working_pre")
			{
				break;
			}
			num++;
		}
		float workTime = (float)(anim2.numFrames - 3) / anim2.frameRate;
		component.SetWorkTime(workTime);
		IL_8F:
		base.OnSpawn();
		base.smi.StartSM();
		Components.Graves.Add(this);
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x000FCBB0 File Offset: 0x000FADB0
	protected override void OnCleanUp()
	{
		Components.Graves.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002CFE RID: 11518 RVA: 0x000FCBC4 File Offset: 0x000FADC4
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.graveName = gameObject.name;
			MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
			if (component != null)
			{
				Personality personality = Db.Get().Personalities.TryGet(component.personalityResourceId);
				KAnimFile anim = Assets.GetAnim("gravestone_kanim");
				if (personality != null && anim.GetData().GetAnim(personality.graveStone) != null)
				{
					this.graveAnim = personality.graveStone;
				}
			}
			Util.KDestroyGameObject(gameObject);
		}
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x000FCC4B File Offset: 0x000FAE4B
	private void OnWorkEvent(Workable workable, Workable.WorkableEvent evt)
	{
	}

	// Token: 0x040019F5 RID: 6645
	[Serialize]
	public string graveName;

	// Token: 0x040019F6 RID: 6646
	[Serialize]
	public string graveAnim = "closed";

	// Token: 0x040019F7 RID: 6647
	[Serialize]
	public int epitaphIdx;

	// Token: 0x040019F8 RID: 6648
	[Serialize]
	public float burialTime = -1f;

	// Token: 0x040019F9 RID: 6649
	private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1];

	// Token: 0x040019FA RID: 6650
	private static readonly EventSystem.IntraObjectHandler<Grave> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Grave>(delegate(Grave component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02001506 RID: 5382
	public class StatesInstance : GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.GameInstance
	{
		// Token: 0x06008CEA RID: 36074 RVA: 0x0033D2C5 File Offset: 0x0033B4C5
		public StatesInstance(Grave master) : base(master)
		{
		}

		// Token: 0x06008CEB RID: 36075 RVA: 0x0033D2D0 File Offset: 0x0033B4D0
		public void CreateFetchTask()
		{
			this.chore = new FetchChore(Db.Get().ChoreTypes.FetchCritical, base.GetComponent<Storage>(), 1f, new HashSet<Tag>
			{
				GameTags.BaseMinion
			}, FetchChore.MatchCriteria.MatchTags, GameTags.Corpse, null, null, true, null, null, null, Operational.State.Operational, 0);
			this.chore.allowMultifetch = false;
		}

		// Token: 0x06008CEC RID: 36076 RVA: 0x0033D32D File Offset: 0x0033B52D
		public void CancelFetchTask()
		{
			this.chore.Cancel("Exit State");
			this.chore = null;
		}

		// Token: 0x04006BB9 RID: 27577
		private FetchChore chore;
	}

	// Token: 0x02001507 RID: 5383
	public class States : GameStateMachine<Grave.States, Grave.StatesInstance, Grave>
	{
		// Token: 0x06008CED RID: 36077 RVA: 0x0033D348 File Offset: 0x0033B548
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.PlayAnim("open").Enter("CreateFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CreateFetchTask();
			}).Exit("CancelFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CancelFetchTask();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GraveEmpty, null).EventTransition(GameHashes.OnStorageChange, this.full, null);
			this.full.PlayAnim((Grave.StatesInstance smi) => smi.master.graveAnim, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Grave, null).Enter(delegate(Grave.StatesInstance smi)
			{
				if (smi.master.burialTime < 0f)
				{
					smi.master.burialTime = GameClock.Instance.GetTime();
				}
			});
		}

		// Token: 0x04006BBA RID: 27578
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State empty;

		// Token: 0x04006BBB RID: 27579
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State full;
	}
}
