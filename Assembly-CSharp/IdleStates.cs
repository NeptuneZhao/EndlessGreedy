using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class IdleStates : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>
{
	// Token: 0x0600041D RID: 1053 RVA: 0x000211F8 File Offset: 0x0001F3F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State state = this.root.Exit("StopNavigator", delegate(IdleStates.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		});
		string name = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.Idle);
		this.loop.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(this.PlayIdle)).ToggleScheduleCallback("IdleMove", (IdleStates.Instance smi) => (float)UnityEngine.Random.Range(3, 10), delegate(IdleStates.Instance smi)
		{
			smi.GoTo(this.move);
		});
		this.move.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(this.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.loop, null).EventTransition(GameHashes.NavigationFailed, this.loop, null);
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00021310 File Offset: 0x0001F510
	public void MoveToNewCell(IdleStates.Instance smi)
	{
		if (smi.HasTag(GameTags.StationaryIdling))
		{
			smi.GoTo(smi.sm.loop);
			return;
		}
		Navigator component = smi.GetComponent<Navigator>();
		IdleStates.MoveCellQuery moveCellQuery = new IdleStates.MoveCellQuery(component.CurrentNavType);
		moveCellQuery.allowLiquid = smi.gameObject.HasTag(GameTags.Amphibious);
		moveCellQuery.submerged = smi.gameObject.HasTag(GameTags.Creatures.Submerged);
		int num = Grid.PosToCell(component);
		if (component.CurrentNavType == NavType.Hover && CellSelectionObject.IsExposedToSpace(num))
		{
			int num2 = 0;
			int cell = num;
			for (int i = 0; i < 10; i++)
			{
				cell = Grid.CellBelow(cell);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || !CellSelectionObject.IsExposedToSpace(cell))
				{
					break;
				}
				num2++;
			}
			moveCellQuery.lowerCellBias = (num2 == 10);
		}
		component.RunQuery(moveCellQuery);
		component.GoTo(moveCellQuery.GetResultCell(), null);
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x000213F4 File Offset: 0x0001F5F4
	public void PlayIdle(IdleStates.Instance smi)
	{
		KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
		Navigator component2 = smi.GetComponent<Navigator>();
		NavType nav_type = component2.CurrentNavType;
		if (smi.GetComponent<Facing>().GetFacing())
		{
			nav_type = NavGrid.MirrorNavType(nav_type);
		}
		if (smi.def.customIdleAnim != null)
		{
			HashedString invalid = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref invalid);
			if (hashedString != HashedString.Invalid)
			{
				if (invalid != HashedString.Invalid)
				{
					component.Play(invalid, KAnim.PlayMode.Once, 1f, 0f);
				}
				component.Queue(hashedString, KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		HashedString idleAnim = component2.NavGrid.GetIdleAnim(nav_type);
		component.Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x040002CA RID: 714
	private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State loop;

	// Token: 0x040002CB RID: 715
	private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State move;

	// Token: 0x0200106D RID: 4205
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005CC7 RID: 23751
		public IdleStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02002387 RID: 9095
		// (Invoke) Token: 0x0600B6D3 RID: 46803
		public delegate HashedString IdleAnimCallback(IdleStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x0200106E RID: 4206
	public new class Instance : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.GameInstance
	{
		// Token: 0x06007BF4 RID: 31732 RVA: 0x003046A4 File Offset: 0x003028A4
		public Instance(Chore<IdleStates.Instance> chore, IdleStates.Def def) : base(chore, def)
		{
		}
	}

	// Token: 0x0200106F RID: 4207
	public class MoveCellQuery : PathFinderQuery
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06007BF5 RID: 31733 RVA: 0x003046AE File Offset: 0x003028AE
		// (set) Token: 0x06007BF6 RID: 31734 RVA: 0x003046B6 File Offset: 0x003028B6
		public bool allowLiquid { get; set; }

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06007BF7 RID: 31735 RVA: 0x003046BF File Offset: 0x003028BF
		// (set) Token: 0x06007BF8 RID: 31736 RVA: 0x003046C7 File Offset: 0x003028C7
		public bool submerged { get; set; }

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06007BF9 RID: 31737 RVA: 0x003046D0 File Offset: 0x003028D0
		// (set) Token: 0x06007BFA RID: 31738 RVA: 0x003046D8 File Offset: 0x003028D8
		public bool lowerCellBias { get; set; }

		// Token: 0x06007BFB RID: 31739 RVA: 0x003046E1 File Offset: 0x003028E1
		public MoveCellQuery(NavType navType)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
		}

		// Token: 0x06007BFC RID: 31740 RVA: 0x0030470C File Offset: 0x0030290C
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			GameObject gameObject;
			Grid.ObjectLayers[1].TryGetValue(cell, out gameObject);
			if (gameObject != null)
			{
				BuildingUnderConstruction component = gameObject.GetComponent<BuildingUnderConstruction>();
				if (component != null && (component.Def.IsFoundation || component.HasTag(GameTags.NoCreatureIdling)))
				{
					return false;
				}
			}
			bool flag = this.submerged || Grid.IsNavigatableLiquid(cell);
			bool flag2 = this.navType != NavType.Swim;
			bool flag3 = this.navType == NavType.Swim || this.allowLiquid;
			if (flag && !flag3)
			{
				return false;
			}
			if (!flag && !flag2)
			{
				return false;
			}
			if (this.targetCell == Grid.InvalidCell || !this.lowerCellBias)
			{
				this.targetCell = cell;
			}
			else
			{
				int num = Grid.CellRow(this.targetCell);
				if (Grid.CellRow(cell) < num)
				{
					this.targetCell = cell;
				}
			}
			int num2 = this.maxIterations - 1;
			this.maxIterations = num2;
			return num2 <= 0;
		}

		// Token: 0x06007BFD RID: 31741 RVA: 0x00304804 File Offset: 0x00302A04
		public override int GetResultCell()
		{
			return this.targetCell;
		}

		// Token: 0x04005CC8 RID: 23752
		private NavType navType;

		// Token: 0x04005CC9 RID: 23753
		private int targetCell = Grid.InvalidCell;

		// Token: 0x04005CCA RID: 23754
		private int maxIterations;
	}
}
