using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class BuzzStates : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>
{
	// Token: 0x0600037B RID: 891 RVA: 0x0001CE00 File Offset: 0x0001B000
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State state = this.root.Exit("StopNavigator", delegate(BuzzStates.Instance smi)
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
		this.idle.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(this.PlayIdle)).ToggleScheduleCallback("DoBuzz", (BuzzStates.Instance smi) => (float)UnityEngine.Random.Range(3, 10), delegate(BuzzStates.Instance smi)
		{
			this.numMoves.Set(UnityEngine.Random.Range(4, 6), smi, false);
			smi.GoTo(this.buzz.move);
		});
		this.buzz.ParamTransition<int>(this.numMoves, this.idle, (BuzzStates.Instance smi, int p) => p <= 0);
		this.buzz.move.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(this.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.buzz.pause, null).EventTransition(GameHashes.NavigationFailed, this.buzz.pause, null);
		this.buzz.pause.Enter(delegate(BuzzStates.Instance smi)
		{
			this.numMoves.Set(this.numMoves.Get(smi) - 1, smi, false);
			smi.GoTo(this.buzz.move);
		});
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0001CF7C File Offset: 0x0001B17C
	public void MoveToNewCell(BuzzStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		BuzzStates.MoveCellQuery moveCellQuery = new BuzzStates.MoveCellQuery(component.CurrentNavType);
		moveCellQuery.allowLiquid = smi.gameObject.HasTag(GameTags.Amphibious);
		component.RunQuery(moveCellQuery);
		component.GoTo(moveCellQuery.GetResultCell(), null);
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0001CFC8 File Offset: 0x0001B1C8
	public void PlayIdle(BuzzStates.Instance smi)
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

	// Token: 0x0400026C RID: 620
	private StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.IntParameter numMoves;

	// Token: 0x0400026D RID: 621
	private BuzzStates.BuzzingStates buzz;

	// Token: 0x0400026E RID: 622
	public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State idle;

	// Token: 0x0400026F RID: 623
	public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State move;

	// Token: 0x02001002 RID: 4098
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BF4 RID: 23540
		public BuzzStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02002381 RID: 9089
		// (Invoke) Token: 0x0600B6C2 RID: 46786
		public delegate HashedString IdleAnimCallback(BuzzStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x02001003 RID: 4099
	public new class Instance : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.GameInstance
	{
		// Token: 0x06007AFE RID: 31486 RVA: 0x00302ECB File Offset: 0x003010CB
		public Instance(Chore<BuzzStates.Instance> chore, BuzzStates.Def def) : base(chore, def)
		{
		}
	}

	// Token: 0x02001004 RID: 4100
	public class BuzzingStates : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State
	{
		// Token: 0x04005BF5 RID: 23541
		public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State move;

		// Token: 0x04005BF6 RID: 23542
		public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State pause;
	}

	// Token: 0x02001005 RID: 4101
	public class MoveCellQuery : PathFinderQuery
	{
		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06007B00 RID: 31488 RVA: 0x00302EDD File Offset: 0x003010DD
		// (set) Token: 0x06007B01 RID: 31489 RVA: 0x00302EE5 File Offset: 0x003010E5
		public bool allowLiquid { get; set; }

		// Token: 0x06007B02 RID: 31490 RVA: 0x00302EEE File Offset: 0x003010EE
		public MoveCellQuery(NavType navType)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
		}

		// Token: 0x06007B03 RID: 31491 RVA: 0x00302F18 File Offset: 0x00301118
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			bool flag = this.navType != NavType.Swim;
			bool flag2 = this.navType == NavType.Swim || this.allowLiquid;
			bool flag3 = Grid.IsSubstantialLiquid(cell, 0.35f);
			if (flag3 && !flag2)
			{
				return false;
			}
			if (!flag3 && !flag)
			{
				return false;
			}
			this.targetCell = cell;
			int num = this.maxIterations - 1;
			this.maxIterations = num;
			return num <= 0;
		}

		// Token: 0x06007B04 RID: 31492 RVA: 0x00302F89 File Offset: 0x00301189
		public override int GetResultCell()
		{
			return this.targetCell;
		}

		// Token: 0x04005BF7 RID: 23543
		private NavType navType;

		// Token: 0x04005BF8 RID: 23544
		private int targetCell = Grid.InvalidCell;

		// Token: 0x04005BF9 RID: 23545
		private int maxIterations;
	}
}
