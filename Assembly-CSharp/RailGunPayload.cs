using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000753 RID: 1875
[SerializationConfig(MemberSerialization.OptIn)]
public class RailGunPayload : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>
{
	// Token: 0x0600321C RID: 12828 RVA: 0x00113470 File Offset: 0x00111670
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded.idle;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.grounded.DefaultState(this.grounded.idle).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(true, smi, false);
		}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.RailgunpayloadNeedsEmptying, null).ToggleTag(GameTags.RailGunPayloadEmptyable).ToggleTag(GameTags.ClusterEntityGrounded).EventHandler(GameHashes.DroppedAll, delegate(RailGunPayload.StatesInstance smi)
		{
			smi.OnDroppedAll();
		}).OnSignal(this.launch, this.takeoff);
		this.grounded.idle.PlayAnim("idle");
		this.grounded.crater.Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.animController.randomiseLoopedOffset = true;
		}).Exit(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.animController.randomiseLoopedOffset = false;
		}).PlayAnim("landed", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.grounded.idle, null);
		this.takeoff.DefaultState(this.takeoff.launch).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(false, smi, false);
		}).PlayAnim("launching").OnSignal(this.beginTravelling, this.travel);
		this.takeoff.launch.Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.StartTakeoff();
		}).GoTo(this.takeoff.airborne);
		this.takeoff.airborne.Update("Launch", delegate(RailGunPayload.StatesInstance smi, float dt)
		{
			smi.UpdateLaunch(dt);
		}, UpdateRate.SIM_EVERY_TICK, false);
		this.travel.DefaultState(this.travel.travelling).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(false, smi, false);
		}).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.MoveToSpace();
		}).PlayAnim("idle").ToggleTag(GameTags.EntityInSpace).ToggleMainStatusItem(Db.Get().BuildingStatusItems.InFlight, (RailGunPayload.StatesInstance smi) => smi.GetComponent<ClusterTraveler>());
		this.travel.travelling.EventTransition(GameHashes.ClusterDestinationReached, this.travel.transferWorlds, null);
		this.travel.transferWorlds.Exit(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.StartLand();
		}).GoTo(this.landing.landing);
		this.landing.DefaultState(this.landing.landing).ParamTransition<bool>(this.onSurface, this.grounded.crater, GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IsTrue).ParamTransition<int>(this.destinationWorld, this.takeoff, (RailGunPayload.StatesInstance smi, int p) => p != -1).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.MoveToWorld();
		});
		this.landing.landing.PlayAnim("falling", KAnim.PlayMode.Loop).UpdateTransition(this.landing.impact, (RailGunPayload.StatesInstance smi, float dt) => smi.UpdateLanding(dt), UpdateRate.SIM_200ms, false).ToggleGravity(this.landing.impact);
		this.landing.impact.PlayAnim("land").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.crater);
	}

	// Token: 0x04001D97 RID: 7575
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IntParameter destinationWorld = new StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IntParameter(-1);

	// Token: 0x04001D98 RID: 7576
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.BoolParameter onSurface = new StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.BoolParameter(false);

	// Token: 0x04001D99 RID: 7577
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.Signal beginTravelling;

	// Token: 0x04001D9A RID: 7578
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.Signal launch;

	// Token: 0x04001D9B RID: 7579
	public RailGunPayload.TakeoffStates takeoff;

	// Token: 0x04001D9C RID: 7580
	public RailGunPayload.TravelStates travel;

	// Token: 0x04001D9D RID: 7581
	public RailGunPayload.LandingStates landing;

	// Token: 0x04001D9E RID: 7582
	public RailGunPayload.GroundedStates grounded;

	// Token: 0x020015C5 RID: 5573
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006DB1 RID: 28081
		public bool attractToBeacons;

		// Token: 0x04006DB2 RID: 28082
		public string clusterAnimSymbolSwapTarget;

		// Token: 0x04006DB3 RID: 28083
		public List<string> randomClusterSymbolSwaps;

		// Token: 0x04006DB4 RID: 28084
		public string worldAnimSymbolSwapTarget;

		// Token: 0x04006DB5 RID: 28085
		public List<string> randomWorldSymbolSwaps;
	}

	// Token: 0x020015C6 RID: 5574
	public class TakeoffStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04006DB6 RID: 28086
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State launch;

		// Token: 0x04006DB7 RID: 28087
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State airborne;
	}

	// Token: 0x020015C7 RID: 5575
	public class TravelStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04006DB8 RID: 28088
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State travelling;

		// Token: 0x04006DB9 RID: 28089
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State transferWorlds;
	}

	// Token: 0x020015C8 RID: 5576
	public class LandingStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04006DBA RID: 28090
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State landing;

		// Token: 0x04006DBB RID: 28091
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State impact;
	}

	// Token: 0x020015C9 RID: 5577
	public class GroundedStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04006DBC RID: 28092
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State crater;

		// Token: 0x04006DBD RID: 28093
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State idle;
	}

	// Token: 0x020015CA RID: 5578
	public class StatesInstance : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.GameInstance
	{
		// Token: 0x06008FC0 RID: 36800 RVA: 0x00348D84 File Offset: 0x00346F84
		public StatesInstance(IStateMachineTarget master, RailGunPayload.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			DebugUtil.Assert(def.clusterAnimSymbolSwapTarget == null == (def.worldAnimSymbolSwapTarget == null), "Must specify both or neither symbol swap targets!");
			DebugUtil.Assert((def.randomClusterSymbolSwaps == null && def.randomWorldSymbolSwaps == null) || def.randomClusterSymbolSwaps.Count == def.randomWorldSymbolSwaps.Count, "Must specify the same number of swaps for both world and cluster!");
			if (def.clusterAnimSymbolSwapTarget != null && def.worldAnimSymbolSwapTarget != null)
			{
				if (this.randomSymbolSwapIndex == -1)
				{
					this.randomSymbolSwapIndex = UnityEngine.Random.Range(0, def.randomClusterSymbolSwaps.Count);
					global::Debug.Log(string.Format("Rolling a random symbol: {0}", this.randomSymbolSwapIndex), base.gameObject);
				}
				base.GetComponent<BallisticClusterGridEntity>().SwapSymbolFromSameAnim(def.clusterAnimSymbolSwapTarget, def.randomClusterSymbolSwaps[this.randomSymbolSwapIndex]);
				KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol(def.randomWorldSymbolSwaps[this.randomSymbolSwapIndex]);
				this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(def.worldAnimSymbolSwapTarget, symbol, 0);
			}
		}

		// Token: 0x06008FC1 RID: 36801 RVA: 0x00348EC8 File Offset: 0x003470C8
		public void Launch(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.takeoff);
		}

		// Token: 0x06008FC2 RID: 36802 RVA: 0x00348F10 File Offset: 0x00347110
		public void Travel(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.travel);
		}

		// Token: 0x06008FC3 RID: 36803 RVA: 0x00348F56 File Offset: 0x00347156
		public void StartTakeoff()
		{
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		// Token: 0x06008FC4 RID: 36804 RVA: 0x00348F7C File Offset: 0x0034717C
		public void StartLand()
		{
			WorldContainer worldContainer = ClusterManager.Instance.GetWorld(base.sm.destinationWorld.Get(this));
			if (worldContainer == null)
			{
				worldContainer = ClusterManager.Instance.GetStartWorld();
			}
			int num = Grid.InvalidCell;
			if (base.def.attractToBeacons)
			{
				num = ClusterManager.Instance.GetLandingBeaconLocation(worldContainer.id);
			}
			int num4;
			if (num != Grid.InvalidCell)
			{
				int num2;
				int num3;
				Grid.CellToXY(num, out num2, out num3);
				int minInclusive = Mathf.Max(num2 - 3, (int)worldContainer.minimumBounds.x);
				int maxExclusive = Mathf.Min(num2 + 3, (int)worldContainer.maximumBounds.x);
				num4 = Mathf.RoundToInt((float)UnityEngine.Random.Range(minInclusive, maxExclusive));
			}
			else
			{
				num4 = Mathf.RoundToInt(UnityEngine.Random.Range(worldContainer.minimumBounds.x + 3f, worldContainer.maximumBounds.x - 3f));
			}
			Vector3 position = new Vector3((float)num4 + 0.5f, worldContainer.maximumBounds.y - 1f, Grid.GetLayerZ(Grid.SceneLayer.Front));
			base.transform.SetPosition(position);
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
			GameComps.Fallers.Add(base.gameObject, new Vector2(0f, -10f));
			base.sm.destinationWorld.Set(-1, this, false);
		}

		// Token: 0x06008FC5 RID: 36805 RVA: 0x003490E4 File Offset: 0x003472E4
		public void UpdateLaunch(float dt)
		{
			if (base.gameObject.GetMyWorld() != null)
			{
				Vector3 position = base.transform.GetPosition() + new Vector3(0f, this.takeoffVelocity * dt, 0f);
				base.transform.SetPosition(position);
				return;
			}
			base.sm.beginTravelling.Trigger(this);
			ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
			if (ClusterGrid.Instance.GetAsteroidAtCell(component.Location) != null)
			{
				base.GetComponent<ClusterTraveler>().AdvancePathOneStep();
			}
		}

		// Token: 0x06008FC6 RID: 36806 RVA: 0x00349178 File Offset: 0x00347378
		public bool UpdateLanding(float dt)
		{
			if (base.gameObject.GetMyWorld() != null)
			{
				Vector3 position = base.transform.GetPosition();
				position.y -= 0.5f;
				int cell = Grid.PosToCell(position);
				if (Grid.IsWorldValidCell(cell) && Grid.IsSolidCell(cell))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008FC7 RID: 36807 RVA: 0x003491CE File Offset: 0x003473CE
		public void OnDroppedAll()
		{
			base.gameObject.DeleteObject();
		}

		// Token: 0x06008FC8 RID: 36808 RVA: 0x003491DB File Offset: 0x003473DB
		public bool IsTraveling()
		{
			return base.IsInsideState(base.sm.travel.travelling);
		}

		// Token: 0x06008FC9 RID: 36809 RVA: 0x003491F4 File Offset: 0x003473F4
		public void MoveToSpace()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = false;
			}
			base.gameObject.transform.SetPosition(new Vector3(-1f, -1f, 0f));
		}

		// Token: 0x06008FCA RID: 36810 RVA: 0x00349240 File Offset: 0x00347440
		public void MoveToWorld()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = true;
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(true);
			}
		}

		// Token: 0x04006DBE RID: 28094
		[Serialize]
		public float takeoffVelocity;

		// Token: 0x04006DBF RID: 28095
		[Serialize]
		private int randomSymbolSwapIndex = -1;

		// Token: 0x04006DC0 RID: 28096
		public KBatchedAnimController animController;
	}
}
