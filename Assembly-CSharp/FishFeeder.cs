using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006D0 RID: 1744
public class FishFeeder : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>
{
	// Token: 0x06002C2F RID: 11311 RVA: 0x000F8140 File Offset: 0x000F6340
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notoperational;
		this.root.Enter(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.SetupFishFeederTopAndBot)).Exit(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.CleanupFishFeederTopAndBot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnStorageChange)).EventHandler(GameHashes.RefreshUserMenu, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnRefreshUserMenu));
		this.notoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.on).TagTransition(GameTags.Operational, this.notoperational, true);
		this.operational.on.DoNothing();
		int num = 19;
		FishFeeder.ballSymbols = new HashedString[num];
		for (int i = 0; i < num; i++)
		{
			FishFeeder.ballSymbols[i] = "ball" + i.ToString();
		}
	}

	// Token: 0x06002C30 RID: 11312 RVA: 0x000F8238 File Offset: 0x000F6438
	private static void SetupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		Storage storage = smi.Get<Storage>();
		smi.fishFeederTop = new FishFeeder.FishFeederTop(smi, FishFeeder.ballSymbols, storage.Capacity());
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot = new FishFeeder.FishFeederBot(smi, 10f, FishFeeder.ballSymbols);
		smi.fishFeederBot.RefreshStorage();
		smi.fishFeederTop.ToggleMutantSeedFetches(smi.ForbidMutantSeeds);
		smi.UpdateMutantSeedStatusItem();
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x000F82A6 File Offset: 0x000F64A6
	private static void CleanupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		smi.fishFeederTop.Cleanup();
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x000F82B4 File Offset: 0x000F64B4
	private static void MoveStoredContentsToConsumeOffset(FishFeeder.Instance smi)
	{
		foreach (GameObject gameObject in smi.GetComponent<Storage>().items)
		{
			if (!(gameObject == null))
			{
				FishFeeder.OnStorageChange(smi, gameObject);
			}
		}
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x000F8318 File Offset: 0x000F6518
	private static void OnStorageChange(FishFeeder.Instance smi, object data)
	{
		if ((GameObject)data == null)
		{
			return;
		}
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot.RefreshStorage();
	}

	// Token: 0x06002C34 RID: 11316 RVA: 0x000F8340 File Offset: 0x000F6540
	private static void OnRefreshUserMenu(FishFeeder.Instance smi, object data)
	{
		if (DlcManager.FeatureRadiationEnabled())
		{
			Game.Instance.userMenu.AddButton(smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", smi.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				smi.ForbidMutantSeeds = !smi.ForbidMutantSeeds;
				FishFeeder.OnRefreshUserMenu(smi, null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.FISH_FEEDER_TOOLTIP, true), 1f);
		}
	}

	// Token: 0x04001977 RID: 6519
	public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State notoperational;

	// Token: 0x04001978 RID: 6520
	public FishFeeder.OperationalState operational;

	// Token: 0x04001979 RID: 6521
	public static HashedString[] ballSymbols;

	// Token: 0x020014D3 RID: 5331
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014D4 RID: 5332
	public class OperationalState : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State
	{
		// Token: 0x04006B08 RID: 27400
		public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State on;
	}

	// Token: 0x020014D5 RID: 5333
	public new class Instance : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameInstance
	{
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06008C32 RID: 35890 RVA: 0x0033942E File Offset: 0x0033762E
		// (set) Token: 0x06008C33 RID: 35891 RVA: 0x00339436 File Offset: 0x00337636
		public bool ForbidMutantSeeds
		{
			get
			{
				return this.forbidMutantSeeds;
			}
			set
			{
				this.forbidMutantSeeds = value;
				this.fishFeederTop.ToggleMutantSeedFetches(this.forbidMutantSeeds);
				this.UpdateMutantSeedStatusItem();
			}
		}

		// Token: 0x06008C34 RID: 35892 RVA: 0x00339458 File Offset: 0x00337658
		public Instance(IStateMachineTarget master, FishFeeder.Def def) : base(master, def)
		{
			this.mutantSeedStatusItem = new StatusItem("FISHFEEDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettingsDelegate));
		}

		// Token: 0x06008C35 RID: 35893 RVA: 0x003394B0 File Offset: 0x003376B0
		private void OnCopySettingsDelegate(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FishFeeder.Instance smi = gameObject.GetSMI<FishFeeder.Instance>();
			if (smi == null)
			{
				return;
			}
			this.ForbidMutantSeeds = smi.ForbidMutantSeeds;
		}

		// Token: 0x06008C36 RID: 35894 RVA: 0x003394E5 File Offset: 0x003376E5
		public void UpdateMutantSeedStatusItem()
		{
			base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(this.mutantSeedStatusItem, SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") && !this.forbidMutantSeeds, null);
		}

		// Token: 0x04006B09 RID: 27401
		private StatusItem mutantSeedStatusItem;

		// Token: 0x04006B0A RID: 27402
		public FishFeeder.FishFeederTop fishFeederTop;

		// Token: 0x04006B0B RID: 27403
		public FishFeeder.FishFeederBot fishFeederBot;

		// Token: 0x04006B0C RID: 27404
		[Serialize]
		private bool forbidMutantSeeds;
	}

	// Token: 0x020014D6 RID: 5334
	public class FishFeederTop : IRenderEveryTick
	{
		// Token: 0x06008C37 RID: 35895 RVA: 0x0033951C File Offset: 0x0033771C
		public FishFeederTop(FishFeeder.Instance smi, HashedString[] ball_symbols, float capacity)
		{
			this.smi = smi;
			this.ballSymbols = ball_symbols;
			this.massPerBall = capacity / (float)ball_symbols.Length;
			this.FillFeeder(this.mass);
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x06008C38 RID: 35896 RVA: 0x00339558 File Offset: 0x00337758
		private void FillFeeder(float mass)
		{
			KBatchedAnimController component = this.smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < this.ballSymbols.Length; i++)
			{
				bool is_visible = mass > (float)(i + 1) * this.massPerBall;
				component.SetSymbolVisiblity(this.ballSymbols[i], is_visible);
			}
		}

		// Token: 0x06008C39 RID: 35897 RVA: 0x003395AC File Offset: 0x003377AC
		public void RefreshStorage()
		{
			float num = 0f;
			foreach (GameObject gameObject in this.smi.GetComponent<Storage>().items)
			{
				if (!(gameObject == null))
				{
					num += gameObject.GetComponent<PrimaryElement>().Mass;
				}
			}
			this.targetMass = num;
		}

		// Token: 0x06008C3A RID: 35898 RVA: 0x00339628 File Offset: 0x00337828
		public void RenderEveryTick(float dt)
		{
			this.timeSinceLastBallAppeared += dt;
			if (this.targetMass > this.mass && this.timeSinceLastBallAppeared > 0.025f)
			{
				float num = Mathf.Min(this.massPerBall, this.targetMass - this.mass);
				this.mass += num;
				this.FillFeeder(this.mass);
				this.timeSinceLastBallAppeared = 0f;
			}
		}

		// Token: 0x06008C3B RID: 35899 RVA: 0x0033969C File Offset: 0x0033789C
		public void Cleanup()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x06008C3C RID: 35900 RVA: 0x003396AC File Offset: 0x003378AC
		public void ToggleMutantSeedFetches(bool allow)
		{
			StorageLocker component = this.smi.GetComponent<StorageLocker>();
			if (component != null)
			{
				component.UpdateForbiddenTag(GameTags.MutatedSeed, !allow);
			}
		}

		// Token: 0x04006B0D RID: 27405
		private FishFeeder.Instance smi;

		// Token: 0x04006B0E RID: 27406
		private float mass;

		// Token: 0x04006B0F RID: 27407
		private float targetMass;

		// Token: 0x04006B10 RID: 27408
		private HashedString[] ballSymbols;

		// Token: 0x04006B11 RID: 27409
		private float massPerBall;

		// Token: 0x04006B12 RID: 27410
		private float timeSinceLastBallAppeared;
	}

	// Token: 0x020014D7 RID: 5335
	public class FishFeederBot
	{
		// Token: 0x06008C3D RID: 35901 RVA: 0x003396E0 File Offset: 0x003378E0
		public FishFeederBot(FishFeeder.Instance smi, float mass_per_ball, HashedString[] ball_symbols)
		{
			this.smi = smi;
			this.massPerBall = mass_per_ball;
			this.anim = GameUtil.KInstantiate(Assets.GetPrefab("FishFeederBot"), smi.transform.GetPosition(), Grid.SceneLayer.Front, null, 0).GetComponent<KBatchedAnimController>();
			this.anim.transform.SetParent(smi.transform);
			this.anim.gameObject.SetActive(true);
			this.anim.SetSceneLayer(Grid.SceneLayer.Building);
			this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			this.anim.Stop();
			foreach (HashedString hash in ball_symbols)
			{
				this.anim.SetSymbolVisiblity(hash, false);
			}
			Storage[] components = smi.gameObject.GetComponents<Storage>();
			this.topStorage = components[0];
			this.botStorage = components[1];
			if (!this.botStorage.IsEmpty())
			{
				this.SetBallSymbol(this.botStorage.items[0].gameObject);
				this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06008C3E RID: 35902 RVA: 0x00339820 File Offset: 0x00337A20
		public void RefreshStorage()
		{
			if (this.refreshingStorage)
			{
				return;
			}
			this.refreshingStorage = true;
			foreach (GameObject gameObject in this.botStorage.items)
			{
				if (!(gameObject == null))
				{
					int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
					gameObject.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore));
				}
			}
			if (this.botStorage.IsEmpty())
			{
				float num = 0f;
				foreach (GameObject gameObject2 in this.topStorage.items)
				{
					if (!(gameObject2 == null))
					{
						num += gameObject2.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num > 0f)
				{
					Pickupable pickupable = this.topStorage.items[0].GetComponent<Pickupable>().Take(this.massPerBall);
					this.SetBallSymbol(pickupable.gameObject);
					this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
					this.botStorage.Store(pickupable.gameObject, false, false, true, false);
				}
				else
				{
					this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, false);
				}
			}
			this.refreshingStorage = false;
		}

		// Token: 0x06008C3F RID: 35903 RVA: 0x003399BC File Offset: 0x00337BBC
		private void SetBallSymbol(GameObject stored_go)
		{
			if (stored_go == null)
			{
				return;
			}
			this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, true);
			KAnim.Build build = stored_go.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
			KAnim.Build.Symbol symbol = stored_go.HasTag(GameTags.Seed) ? build.GetSymbol("object") : build.GetSymbol("algae");
			if (symbol != null)
			{
				this.anim.GetComponent<SymbolOverrideController>().AddSymbolOverride(FishFeeder.FishFeederBot.HASH_FEEDBALL, symbol, 0);
			}
			HashedString batchGroupOverride = new HashedString("FishFeeder" + stored_go.GetComponent<KPrefabID>().PrefabTag.Name);
			this.anim.SetBatchGroupOverride(batchGroupOverride);
			int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
			stored_go.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingUse));
		}

		// Token: 0x04006B13 RID: 27411
		private KBatchedAnimController anim;

		// Token: 0x04006B14 RID: 27412
		private Storage topStorage;

		// Token: 0x04006B15 RID: 27413
		private Storage botStorage;

		// Token: 0x04006B16 RID: 27414
		private bool refreshingStorage;

		// Token: 0x04006B17 RID: 27415
		private FishFeeder.Instance smi;

		// Token: 0x04006B18 RID: 27416
		private float massPerBall;

		// Token: 0x04006B19 RID: 27417
		private static readonly HashedString HASH_FEEDBALL = "feedball";
	}
}
