using System;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class CreatureFallMonitor : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>
{
	// Token: 0x06001F1B RID: 7963 RVA: 0x000AE43C File Offset: 0x000AC63C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.grounded.ToggleBehaviour(GameTags.Creatures.Falling, (CreatureFallMonitor.Instance smi) => smi.ShouldFall(), null);
	}

	// Token: 0x04001186 RID: 4486
	public static float FLOOR_DISTANCE = -0.065f;

	// Token: 0x04001187 RID: 4487
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State grounded;

	// Token: 0x04001188 RID: 4488
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State falling;

	// Token: 0x02001322 RID: 4898
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065B3 RID: 26035
		public bool canSwim;

		// Token: 0x040065B4 RID: 26036
		public bool checkHead = true;
	}

	// Token: 0x02001323 RID: 4899
	public new class Instance : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.GameInstance
	{
		// Token: 0x0600860F RID: 34319 RVA: 0x00328324 File Offset: 0x00326524
		public Instance(IStateMachineTarget master, CreatureFallMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008610 RID: 34320 RVA: 0x0032833C File Offset: 0x0032653C
		public void SnapToGround()
		{
			Vector3 position = base.smi.transform.GetPosition();
			Vector3 position2 = Grid.CellToPosCBC(Grid.PosToCell(position), Grid.SceneLayer.Creatures);
			position2.x = position.x;
			base.smi.transform.SetPosition(position2);
			if (this.navigator.IsValidNavType(NavType.Floor))
			{
				this.navigator.SetCurrentNavType(NavType.Floor);
				return;
			}
			if (this.navigator.IsValidNavType(NavType.Hover))
			{
				this.navigator.SetCurrentNavType(NavType.Hover);
			}
		}

		// Token: 0x06008611 RID: 34321 RVA: 0x003283BC File Offset: 0x003265BC
		public bool ShouldFall()
		{
			if (this.kprefabId.HasTag(GameTags.Stored))
			{
				return false;
			}
			Vector3 position = base.smi.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (Grid.IsValidCell(num) && Grid.Solid[num])
			{
				return false;
			}
			if (this.navigator.IsMoving())
			{
				return false;
			}
			if (this.CanSwimAtCurrentLocation())
			{
				return false;
			}
			if (this.navigator.CurrentNavType != NavType.Swim)
			{
				if (this.navigator.NavGrid.NavTable.IsValid(num, this.navigator.CurrentNavType))
				{
					return false;
				}
				if (this.navigator.CurrentNavType == NavType.Ceiling)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.LeftWall)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.RightWall)
				{
					return true;
				}
			}
			Vector3 vector = position;
			vector.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int num2 = Grid.PosToCell(vector);
			return !Grid.IsValidCell(num2) || !Grid.Solid[num2];
		}

		// Token: 0x06008612 RID: 34322 RVA: 0x003284C4 File Offset: 0x003266C4
		public bool CanSwimAtCurrentLocation()
		{
			if (base.def.canSwim)
			{
				Vector3 position = base.transform.GetPosition();
				float num = 1f;
				if (!base.def.checkHead)
				{
					num = 0.5f;
				}
				position.y += this.collider.size.y * num;
				if (Grid.IsSubstantialLiquid(Grid.PosToCell(position), 0.35f))
				{
					if (!GameComps.Gravities.Has(base.gameObject))
					{
						return true;
					}
					if (GameComps.Gravities.GetData(GameComps.Gravities.GetHandle(base.gameObject)).velocity.magnitude < 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040065B5 RID: 26037
		public string anim = "fall";

		// Token: 0x040065B6 RID: 26038
		[MyCmpReq]
		private KPrefabID kprefabId;

		// Token: 0x040065B7 RID: 26039
		[MyCmpReq]
		private Navigator navigator;

		// Token: 0x040065B8 RID: 26040
		[MyCmpReq]
		private KBoxCollider2D collider;
	}
}
