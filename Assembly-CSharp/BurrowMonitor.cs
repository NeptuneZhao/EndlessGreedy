using System;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public class BurrowMonitor : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>
{
	// Token: 0x06001EF2 RID: 7922 RVA: 0x000AD3B0 File Offset: 0x000AB5B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.openair;
		this.openair.ToggleBehaviour(GameTags.Creatures.WantsToEnterBurrow, (BurrowMonitor.Instance smi) => smi.ShouldBurrow() && smi.timeinstate > smi.def.minimumAwakeTime, delegate(BurrowMonitor.Instance smi)
		{
			smi.BurrowComplete();
		}).Transition(this.entombed, (BurrowMonitor.Instance smi) => smi.IsEntombed() && !smi.HasTag(GameTags.Creatures.Bagged), UpdateRate.SIM_200ms).Enter("SetCollider", delegate(BurrowMonitor.Instance smi)
		{
			smi.SetCollider(true);
		});
		this.entombed.Enter("SetCollider", delegate(BurrowMonitor.Instance smi)
		{
			smi.SetCollider(false);
		}).Transition(this.openair, (BurrowMonitor.Instance smi) => !smi.IsEntombed(), UpdateRate.SIM_200ms).TagTransition(GameTags.Creatures.Bagged, this.openair, false).ToggleBehaviour(GameTags.Creatures.Burrowed, (BurrowMonitor.Instance smi) => smi.IsEntombed(), delegate(BurrowMonitor.Instance smi)
		{
			smi.GoTo(this.openair);
		}).ToggleBehaviour(GameTags.Creatures.WantsToExitBurrow, (BurrowMonitor.Instance smi) => smi.EmergeIsClear() && GameClock.Instance.IsNighttime(), delegate(BurrowMonitor.Instance smi)
		{
			smi.ExitBurrowComplete();
		});
	}

	// Token: 0x04001175 RID: 4469
	public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State openair;

	// Token: 0x04001176 RID: 4470
	public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State entombed;

	// Token: 0x0200130A RID: 4874
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006559 RID: 25945
		public float burrowHardnessLimit = 20f;

		// Token: 0x0400655A RID: 25946
		public float minimumAwakeTime = 24f;

		// Token: 0x0400655B RID: 25947
		public Vector2 moundColliderSize = new Vector2f(1f, 1.5f);

		// Token: 0x0400655C RID: 25948
		public Vector2 moundColliderOffset = new Vector2(0f, -0.25f);
	}

	// Token: 0x0200130B RID: 4875
	public new class Instance : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.GameInstance
	{
		// Token: 0x0600859A RID: 34202 RVA: 0x0032695C File Offset: 0x00324B5C
		public Instance(IStateMachineTarget master, BurrowMonitor.Def def) : base(master, def)
		{
			KBoxCollider2D component = master.GetComponent<KBoxCollider2D>();
			this.originalColliderSize = component.size;
			this.originalColliderOffset = component.offset;
		}

		// Token: 0x0600859B RID: 34203 RVA: 0x00326990 File Offset: 0x00324B90
		public bool EmergeIsClear()
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(Grid.CellAbove(cell)))
			{
				return false;
			}
			int i = Grid.CellAbove(cell);
			return !Grid.Solid[i] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.9f);
		}

		// Token: 0x0600859C RID: 34204 RVA: 0x003269EB File Offset: 0x00324BEB
		public bool ShouldBurrow()
		{
			return !GameClock.Instance.IsNighttime() && this.CanBurrowInto(Grid.CellBelow(Grid.PosToCell(base.gameObject))) && !base.HasTag(GameTags.Creatures.Bagged);
		}

		// Token: 0x0600859D RID: 34205 RVA: 0x00326A28 File Offset: 0x00324C28
		public bool CanBurrowInto(int cell)
		{
			return Grid.IsValidCell(cell) && Grid.Solid[cell] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f) && !(Grid.Objects[cell, 1] != null) && (float)Grid.Element[cell].hardness <= base.def.burrowHardnessLimit && !Grid.Foundation[cell];
		}

		// Token: 0x0600859E RID: 34206 RVA: 0x00326AA4 File Offset: 0x00324CA4
		public bool IsEntombed()
		{
			int num = Grid.PosToCell(base.smi);
			return Grid.IsValidCell(num) && Grid.Solid[num];
		}

		// Token: 0x0600859F RID: 34207 RVA: 0x00326AD2 File Offset: 0x00324CD2
		public void ExitBurrowComplete()
		{
			base.smi.GetComponent<KBatchedAnimController>().Play("idle_loop", KAnim.PlayMode.Once, 1f, 0f);
			this.GoTo(base.sm.openair);
		}

		// Token: 0x060085A0 RID: 34208 RVA: 0x00326B0C File Offset: 0x00324D0C
		public void BurrowComplete()
		{
			base.smi.transform.SetPosition(Grid.CellToPosCBC(Grid.CellBelow(Grid.PosToCell(base.transform.GetPosition())), Grid.SceneLayer.Creatures));
			base.smi.GetComponent<KBatchedAnimController>().Play("idle_mound", KAnim.PlayMode.Once, 1f, 0f);
			this.GoTo(base.sm.entombed);
		}

		// Token: 0x060085A1 RID: 34209 RVA: 0x00326B7C File Offset: 0x00324D7C
		public void SetCollider(bool original_size)
		{
			KBoxCollider2D component = base.master.GetComponent<KBoxCollider2D>();
			AnimEventHandler component2 = base.master.GetComponent<AnimEventHandler>();
			if (original_size)
			{
				component.size = this.originalColliderSize;
				component.offset = this.originalColliderOffset;
				component2.baseOffset = this.originalColliderOffset;
				return;
			}
			component.size = base.def.moundColliderSize;
			component.offset = base.def.moundColliderOffset;
			component2.baseOffset = base.def.moundColliderOffset;
		}

		// Token: 0x0400655D RID: 25949
		private Vector2 originalColliderSize;

		// Token: 0x0400655E RID: 25950
		private Vector2 originalColliderOffset;
	}
}
