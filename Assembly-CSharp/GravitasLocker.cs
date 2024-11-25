using System;
using UnityEngine;

// Token: 0x020008D8 RID: 2264
public class GravitasLocker : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>
{
	// Token: 0x06004090 RID: 16528 RVA: 0x0016F920 File Offset: 0x0016DB20
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.close;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.close.ParamTransition<bool>(this.IsOpen, this.open, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue).DefaultState(this.close.idle);
		this.close.idle.PlayAnim("on").ParamTransition<bool>(this.WorkOrderGiven, this.close.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
		this.close.work.DefaultState(this.close.work.waitingForDupe);
		this.close.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartlWorkChore_OpenLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((GravitasLocker.Instance smi) => smi.GetWorkable(), this.close.work.complete).ParamTransition<bool>(this.WorkOrderGiven, this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
		this.close.work.complete.Enter(delegate(GravitasLocker.Instance smi)
		{
			this.WorkOrderGiven.Set(false, smi, false);
		}).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Open)).TriggerOnEnter(GameHashes.UIRefresh, null);
		this.open.ParamTransition<bool>(this.IsOpen, this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse).DefaultState(this.open.opening);
		this.open.opening.PlayAnim("working").OnAnimQueueComplete(this.open.idle);
		this.open.idle.PlayAnim("empty").Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.SpawnLoot)).ParamTransition<bool>(this.WorkOrderGiven, this.open.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
		this.open.work.DefaultState(this.open.work.waitingForDupe);
		this.open.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartWorkChore_CloseLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((GravitasLocker.Instance smi) => smi.GetWorkable(), this.open.work.complete).ParamTransition<bool>(this.WorkOrderGiven, this.open.idle, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
		this.open.work.complete.Enter(delegate(GravitasLocker.Instance smi)
		{
			this.WorkOrderGiven.Set(false, smi, false);
		}).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Close)).TriggerOnEnter(GameHashes.UIRefresh, null);
	}

	// Token: 0x06004091 RID: 16529 RVA: 0x0016FBEC File Offset: 0x0016DDEC
	public static void Open(GravitasLocker.Instance smi)
	{
		smi.Open();
	}

	// Token: 0x06004092 RID: 16530 RVA: 0x0016FBF4 File Offset: 0x0016DDF4
	public static void Close(GravitasLocker.Instance smi)
	{
		smi.Close();
	}

	// Token: 0x06004093 RID: 16531 RVA: 0x0016FBFC File Offset: 0x0016DDFC
	public static void SpawnLoot(GravitasLocker.Instance smi)
	{
		smi.SpawnLoot();
	}

	// Token: 0x06004094 RID: 16532 RVA: 0x0016FC04 File Offset: 0x0016DE04
	public static void StartWorkChore_CloseLocker(GravitasLocker.Instance smi)
	{
		smi.CreateWorkChore_CloseLocker();
	}

	// Token: 0x06004095 RID: 16533 RVA: 0x0016FC0C File Offset: 0x0016DE0C
	public static void StartlWorkChore_OpenLocker(GravitasLocker.Instance smi)
	{
		smi.CreateWorkChore_OpenLocker();
	}

	// Token: 0x06004096 RID: 16534 RVA: 0x0016FC14 File Offset: 0x0016DE14
	public static void StopWorkChore(GravitasLocker.Instance smi)
	{
		smi.StopWorkChore();
	}

	// Token: 0x04002A94 RID: 10900
	public const float CLOSE_WORKTIME = 1f;

	// Token: 0x04002A95 RID: 10901
	public const float OPEN_WORKTIME = 1.5f;

	// Token: 0x04002A96 RID: 10902
	public const string CLOSED_ANIM_NAME = "on";

	// Token: 0x04002A97 RID: 10903
	public const string OPENING_ANIM_NAME = "working";

	// Token: 0x04002A98 RID: 10904
	public const string OPENED = "empty";

	// Token: 0x04002A99 RID: 10905
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter IsOpen;

	// Token: 0x04002A9A RID: 10906
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WasEmptied;

	// Token: 0x04002A9B RID: 10907
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WorkOrderGiven;

	// Token: 0x04002A9C RID: 10908
	public GravitasLocker.CloseStates close;

	// Token: 0x04002A9D RID: 10909
	public GravitasLocker.OpenStates open;

	// Token: 0x02001819 RID: 6169
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400750E RID: 29966
		public bool CanBeClosed;

		// Token: 0x0400750F RID: 29967
		public string SideScreen_OpenButtonText;

		// Token: 0x04007510 RID: 29968
		public string SideScreen_OpenButtonTooltip;

		// Token: 0x04007511 RID: 29969
		public string SideScreen_CancelOpenButtonText;

		// Token: 0x04007512 RID: 29970
		public string SideScreen_CancelOpenButtonTooltip;

		// Token: 0x04007513 RID: 29971
		public string SideScreen_CloseButtonText;

		// Token: 0x04007514 RID: 29972
		public string SideScreen_CloseButtonTooltip;

		// Token: 0x04007515 RID: 29973
		public string SideScreen_CancelCloseButtonText;

		// Token: 0x04007516 RID: 29974
		public string SideScreen_CancelCloseButtonTooltip;

		// Token: 0x04007517 RID: 29975
		public string OPEN_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		// Token: 0x04007518 RID: 29976
		public string CLOSE_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		// Token: 0x04007519 RID: 29977
		public string[] ObjectsToSpawn = new string[0];

		// Token: 0x0400751A RID: 29978
		public string[] LootSymbols = new string[0];
	}

	// Token: 0x0200181A RID: 6170
	public class WorkStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x0400751B RID: 29979
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State waitingForDupe;

		// Token: 0x0400751C RID: 29980
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State complete;
	}

	// Token: 0x0200181B RID: 6171
	public class CloseStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x0400751D RID: 29981
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;

		// Token: 0x0400751E RID: 29982
		public GravitasLocker.WorkStates work;
	}

	// Token: 0x0200181C RID: 6172
	public class OpenStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x0400751F RID: 29983
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State opening;

		// Token: 0x04007520 RID: 29984
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;

		// Token: 0x04007521 RID: 29985
		public GravitasLocker.WorkStates work;
	}

	// Token: 0x0200181D RID: 6173
	public new class Instance : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06009777 RID: 38775 RVA: 0x003658D0 File Offset: 0x00363AD0
		public bool WorkOrderGiven
		{
			get
			{
				return base.smi.sm.WorkOrderGiven.Get(base.smi);
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06009778 RID: 38776 RVA: 0x003658ED File Offset: 0x00363AED
		public bool IsOpen
		{
			get
			{
				return base.smi.sm.IsOpen.Get(base.smi);
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06009779 RID: 38777 RVA: 0x0036590A File Offset: 0x00363B0A
		public bool HasContents
		{
			get
			{
				return !base.smi.sm.WasEmptied.Get(base.smi) && base.def.ObjectsToSpawn.Length != 0;
			}
		}

		// Token: 0x0600977A RID: 38778 RVA: 0x0036593A File Offset: 0x00363B3A
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x0600977B RID: 38779 RVA: 0x00365942 File Offset: 0x00363B42
		public void Open()
		{
			base.smi.sm.IsOpen.Set(true, base.smi, false);
		}

		// Token: 0x0600977C RID: 38780 RVA: 0x00365962 File Offset: 0x00363B62
		public void Close()
		{
			base.smi.sm.IsOpen.Set(false, base.smi, false);
		}

		// Token: 0x0600977D RID: 38781 RVA: 0x00365982 File Offset: 0x00363B82
		public Instance(IStateMachineTarget master, GravitasLocker.Def def) : base(master, def)
		{
		}

		// Token: 0x0600977E RID: 38782 RVA: 0x0036598C File Offset: 0x00363B8C
		public override void StartSM()
		{
			this.DefineDropSpawnPositions();
			base.StartSM();
			this.UpdateContentPreviewSymbols();
		}

		// Token: 0x0600977F RID: 38783 RVA: 0x003659A0 File Offset: 0x00363BA0
		public void DefineDropSpawnPositions()
		{
			if (this.dropSpawnPositions == null && base.def.LootSymbols.Length != 0)
			{
				this.dropSpawnPositions = new Vector3[base.def.LootSymbols.Length];
				for (int i = 0; i < this.dropSpawnPositions.Length; i++)
				{
					bool flag;
					Vector3 vector = this.animController.GetSymbolTransform(base.def.LootSymbols[i], out flag).GetColumn(3);
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					this.dropSpawnPositions[i] = (flag ? vector : base.gameObject.transform.GetPosition());
				}
			}
		}

		// Token: 0x06009780 RID: 38784 RVA: 0x00365A54 File Offset: 0x00363C54
		public void CreateWorkChore_CloseLocker()
		{
			if (this.chore == null)
			{
				this.workable.SetWorkTime(1f);
				this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Repair, this.workable, null, true, null, null, null, true, null, false, true, Assets.GetAnim(base.def.CLOSE_INTERACT_ANIM_NAME), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
			}
		}

		// Token: 0x06009781 RID: 38785 RVA: 0x00365AC0 File Offset: 0x00363CC0
		public void CreateWorkChore_OpenLocker()
		{
			if (this.chore == null)
			{
				this.workable.SetWorkTime(1.5f);
				this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this.workable, null, true, null, null, null, true, null, false, true, Assets.GetAnim(base.def.OPEN_INTERACT_ANIM_NAME), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
			}
		}

		// Token: 0x06009782 RID: 38786 RVA: 0x00365B2A File Offset: 0x00363D2A
		public void StopWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Canceled by user");
				this.chore = null;
			}
		}

		// Token: 0x06009783 RID: 38787 RVA: 0x00365B4C File Offset: 0x00363D4C
		public void SpawnLoot()
		{
			if (this.HasContents)
			{
				for (int i = 0; i < base.def.ObjectsToSpawn.Length; i++)
				{
					string name = base.def.ObjectsToSpawn[i];
					GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 0, name, Grid.SceneLayer.Ore);
					gameObject.SetActive(true);
					if (this.dropSpawnPositions != null && i < this.dropSpawnPositions.Length)
					{
						gameObject.transform.position = this.dropSpawnPositions[i];
					}
				}
				base.smi.sm.WasEmptied.Set(true, base.smi, false);
				this.UpdateContentPreviewSymbols();
			}
		}

		// Token: 0x06009784 RID: 38788 RVA: 0x00365BF8 File Offset: 0x00363DF8
		public void UpdateContentPreviewSymbols()
		{
			for (int i = 0; i < base.def.LootSymbols.Length; i++)
			{
				this.animController.SetSymbolVisiblity(base.def.LootSymbols[i], false);
			}
			if (this.HasContents)
			{
				for (int j = 0; j < Mathf.Min(base.def.LootSymbols.Length, base.def.ObjectsToSpawn.Length); j++)
				{
					KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(base.def.ObjectsToSpawn[j]).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
					SymbolOverrideController component = base.gameObject.GetComponent<SymbolOverrideController>();
					string text = base.def.LootSymbols[j];
					component.AddSymbolOverride(text, symbolByIndex, 0);
					this.animController.SetSymbolVisiblity(text, true);
				}
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06009785 RID: 38789 RVA: 0x00365CE0 File Offset: 0x00363EE0
		public string SidescreenButtonText
		{
			get
			{
				if (!this.IsOpen)
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonText;
					}
					return base.def.SideScreen_CancelOpenButtonText;
				}
				else
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_CloseButtonText;
					}
					return base.def.SideScreen_CancelCloseButtonText;
				}
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06009786 RID: 38790 RVA: 0x00365D34 File Offset: 0x00363F34
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!this.IsOpen)
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonTooltip;
					}
					return base.def.SideScreen_CancelOpenButtonTooltip;
				}
				else
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_CloseButtonTooltip;
					}
					return base.def.SideScreen_CancelCloseButtonTooltip;
				}
			}
		}

		// Token: 0x06009787 RID: 38791 RVA: 0x00365D88 File Offset: 0x00363F88
		public bool SidescreenEnabled()
		{
			return !this.IsOpen || base.def.CanBeClosed;
		}

		// Token: 0x06009788 RID: 38792 RVA: 0x00365D9F File Offset: 0x00363F9F
		public bool SidescreenButtonInteractable()
		{
			return !this.IsOpen || base.def.CanBeClosed;
		}

		// Token: 0x06009789 RID: 38793 RVA: 0x00365DB6 File Offset: 0x00363FB6
		public int HorizontalGroupID()
		{
			return 0;
		}

		// Token: 0x0600978A RID: 38794 RVA: 0x00365DB9 File Offset: 0x00363FB9
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x0600978B RID: 38795 RVA: 0x00365DBD File Offset: 0x00363FBD
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600978C RID: 38796 RVA: 0x00365DC4 File Offset: 0x00363FC4
		public void OnSidescreenButtonPressed()
		{
			base.smi.sm.WorkOrderGiven.Set(!base.smi.sm.WorkOrderGiven.Get(base.smi), base.smi, false);
		}

		// Token: 0x04007522 RID: 29986
		[MyCmpGet]
		private Workable workable;

		// Token: 0x04007523 RID: 29987
		[MyCmpGet]
		private KBatchedAnimController animController;

		// Token: 0x04007524 RID: 29988
		private Chore chore;

		// Token: 0x04007525 RID: 29989
		private Vector3[] dropSpawnPositions;
	}
}
