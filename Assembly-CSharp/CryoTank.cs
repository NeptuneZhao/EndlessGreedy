using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200082C RID: 2092
public class CryoTank : StateMachineComponent<CryoTank.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x060039E1 RID: 14817 RVA: 0x0013B905 File Offset: 0x00139B05
	public string SidescreenButtonText
	{
		get
		{
			return BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTON;
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x060039E2 RID: 14818 RVA: 0x0013B911 File Offset: 0x00139B11
	public string SidescreenButtonTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTONTOOLTIP;
		}
	}

	// Token: 0x060039E3 RID: 14819 RVA: 0x0013B91D File Offset: 0x00139B1D
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x0013B920 File Offset: 0x00139B20
	public void OnSidescreenButtonPressed()
	{
		this.OnClickOpen();
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x0013B928 File Offset: 0x00139B28
	public bool SidescreenButtonInteractable()
	{
		return this.HasDefrostedFriend();
	}

	// Token: 0x060039E6 RID: 14822 RVA: 0x0013B930 File Offset: 0x00139B30
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x060039E7 RID: 14823 RVA: 0x0013B934 File Offset: 0x00139B34
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x0013B93B File Offset: 0x00139B3B
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x0013B940 File Offset: 0x00139B40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Demolishable component = base.GetComponent<Demolishable>();
		if (component != null)
		{
			component.allowDemolition = !this.HasDefrostedFriend();
		}
	}

	// Token: 0x060039EA RID: 14826 RVA: 0x0013B97D File Offset: 0x00139B7D
	public bool HasDefrostedFriend()
	{
		return base.smi.IsInsideState(base.smi.sm.closed) && this.chore == null;
	}

	// Token: 0x060039EB RID: 14827 RVA: 0x0013B9A8 File Offset: 0x00139BA8
	public void DropContents()
	{
		MinionStartingStats minionStartingStats = new MinionStartingStats(GameTags.Minions.Models.Standard, false, null, "AncientKnowledge", false);
		GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.transform.position), this.dropOffset), Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		minionStartingStats.Apply(gameObject);
		gameObject.GetComponent<MinionIdentity>().arrivalTime = (float)UnityEngine.Random.Range(-2000, -1000);
		MinionResume component = gameObject.GetComponent<MinionResume>();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			component.ForceAddSkillPoint();
		}
		base.smi.sm.defrostedDuplicant.Set(gameObject, base.smi, false);
		gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
		ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
		if (component2 != null)
		{
			base.smi.defrostAnimChore = new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_cryo_chamber_kanim", new HashedString[]
			{
				"defrost",
				"defrost_exit"
			}, KAnim.PlayMode.Once, false);
			Vector3 position2 = gameObject.transform.GetPosition();
			position2.z = Grid.GetLayerZ(Grid.SceneLayer.Gas);
			gameObject.transform.SetPosition(position2);
			gameObject.GetMyWorld().SetDupeVisited();
		}
		SaveGame.Instance.ColonyAchievementTracker.defrostedDuplicant = true;
	}

	// Token: 0x060039EC RID: 14828 RVA: 0x0013BB54 File Offset: 0x00139D54
	public void ShowEventPopup()
	{
		GameObject gameObject = base.smi.sm.defrostedDuplicant.Get(base.smi);
		if (this.opener != null && gameObject != null)
		{
			SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.CryoFriend, -1, null).smi as SimpleEvent.StatesInstance;
			statesInstance.minions = new GameObject[]
			{
				gameObject,
				this.opener
			};
			statesInstance.SetTextParameter("dupe", this.opener.GetProperName());
			statesInstance.SetTextParameter("friend", gameObject.GetProperName());
			statesInstance.ShowEventPopup();
		}
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x0013BC00 File Offset: 0x00139E00
	public void Cheer()
	{
		GameObject gameObject = base.smi.sm.defrostedDuplicant.Get(base.smi);
		if (this.opener != null && gameObject != null)
		{
			Db db = Db.Get();
			this.opener.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
			new EmoteChore(this.opener.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
			gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
			new EmoteChore(gameObject.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
		}
	}

	// Token: 0x060039EE RID: 14830 RVA: 0x0013BCEA File Offset: 0x00139EEA
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x060039EF RID: 14831 RVA: 0x0013BCF3 File Offset: 0x00139EF3
	private void OnClickCancel()
	{
		this.CancelActivateChore(null);
	}

	// Token: 0x060039F0 RID: 14832 RVA: 0x0013BCFC File Offset: 0x00139EFC
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteActivateChore();
		}, null, null, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x060039F1 RID: 14833 RVA: 0x0013BD68 File Offset: 0x00139F68
	public void CancelActivateChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x060039F2 RID: 14834 RVA: 0x0013BD8C File Offset: 0x00139F8C
	private void CompleteActivateChore()
	{
		this.opener = this.chore.driver.gameObject;
		base.smi.GoTo(base.smi.sm.open);
		this.chore = null;
		Demolishable component = base.smi.GetComponent<Demolishable>();
		if (component != null)
		{
			component.allowDemolition = true;
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x040022E4 RID: 8932
	public string[][] possible_contents_ids;

	// Token: 0x040022E5 RID: 8933
	public string machineSound;

	// Token: 0x040022E6 RID: 8934
	public string overrideAnim;

	// Token: 0x040022E7 RID: 8935
	public CellOffset dropOffset = CellOffset.none;

	// Token: 0x040022E8 RID: 8936
	private GameObject opener;

	// Token: 0x040022E9 RID: 8937
	private Chore chore;

	// Token: 0x0200174C RID: 5964
	public class StatesInstance : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.GameInstance
	{
		// Token: 0x06009547 RID: 38215 RVA: 0x0035F2DE File Offset: 0x0035D4DE
		public StatesInstance(CryoTank master) : base(master)
		{
		}

		// Token: 0x04007261 RID: 29281
		public Chore defrostAnimChore;
	}

	// Token: 0x0200174D RID: 5965
	public class States : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank>
	{
		// Token: 0x06009548 RID: 38216 RVA: 0x0035F2E8 File Offset: 0x0035D4E8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(CryoTank.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.open.GoTo(this.defrost).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.defrost.PlayAnim("defrost").OnAnimQueueComplete(this.defrostExit).Update(delegate(CryoTank.StatesInstance smi, float dt)
			{
				smi.sm.defrostedDuplicant.Get(smi).GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.master.ShowEventPopup();
			});
			this.defrostExit.PlayAnim("defrost_exit").Update(delegate(CryoTank.StatesInstance smi, float dt)
			{
				if (smi.defrostAnimChore == null || smi.defrostAnimChore.isComplete)
				{
					smi.GoTo(this.off);
				}
			}, UpdateRate.SIM_200ms, false).Exit(delegate(CryoTank.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.defrostedDuplicant.Get(smi);
				if (gameObject != null)
				{
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Move);
					smi.master.Cheer();
				}
			});
			this.off.PlayAnim("off").Enter(delegate(CryoTank.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x04007262 RID: 29282
		public StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.TargetParameter defrostedDuplicant;

		// Token: 0x04007263 RID: 29283
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State closed;

		// Token: 0x04007264 RID: 29284
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State open;

		// Token: 0x04007265 RID: 29285
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrost;

		// Token: 0x04007266 RID: 29286
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrostExit;

		// Token: 0x04007267 RID: 29287
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State off;
	}
}
