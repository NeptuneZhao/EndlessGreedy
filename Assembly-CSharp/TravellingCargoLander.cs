using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200078E RID: 1934
[SerializationConfig(MemberSerialization.OptIn)]
public class TravellingCargoLander : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>
{
	// Token: 0x060034CF RID: 13519 RVA: 0x0011F910 File Offset: 0x0011DB10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.init;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.InitializeOperationalFlag(RocketModule.landedFlag, false).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.init.ParamTransition<bool>(this.isLanding, this.landing.landing, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue).ParamTransition<bool>(this.isLanded, this.grounded, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue).GoTo(this.travel);
		this.travel.DefaultState(this.travel.travelling).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.MoveToSpace();
		}).PlayAnim("idle").ToggleTag(GameTags.EntityInSpace).ToggleMainStatusItem(Db.Get().BuildingStatusItems.InFlight, (TravellingCargoLander.StatesInstance smi) => smi.GetComponent<ClusterTraveler>());
		this.travel.travelling.EventTransition(GameHashes.ClusterDestinationReached, this.travel.transferWorlds, null);
		this.travel.transferWorlds.Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.StartLand();
		}).GoTo(this.landing.landing);
		this.landing.Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanding.Set(true, smi, false);
		}).Exit(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanding.Set(false, smi, false);
		});
		this.landing.landing.PlayAnim("landing", KAnim.PlayMode.Loop).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.ResetAnimPosition();
		}).Update(delegate(TravellingCargoLander.StatesInstance smi, float dt)
		{
			smi.LandingUpdate(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).Transition(this.landing.impact, (TravellingCargoLander.StatesInstance smi) => smi.flightAnimOffset <= 0f, UpdateRate.SIM_200ms).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.MoveToWorld();
		});
		this.landing.impact.PlayAnim("grounded_pre").OnAnimQueueComplete(this.grounded);
		this.grounded.DefaultState(this.grounded.loaded).ToggleTag(GameTags.ClusterEntityGrounded).ToggleOperationalFlag(RocketModule.landedFlag).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanded.Set(true, smi, false);
		});
		this.grounded.loaded.PlayAnim("grounded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsFalse).OnSignal(this.emptyCargo, this.grounded.emptying).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.DoLand();
		});
		this.grounded.emptying.PlayAnim("deploying").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.empty);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue);
	}

	// Token: 0x04001F32 RID: 7986
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IntParameter destinationWorld = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IntParameter(-1);

	// Token: 0x04001F33 RID: 7987
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter isLanding = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04001F34 RID: 7988
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter isLanded = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04001F35 RID: 7989
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter hasCargo = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04001F36 RID: 7990
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.Signal emptyCargo;

	// Token: 0x04001F37 RID: 7991
	public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State init;

	// Token: 0x04001F38 RID: 7992
	public TravellingCargoLander.TravelStates travel;

	// Token: 0x04001F39 RID: 7993
	public TravellingCargoLander.LandingStates landing;

	// Token: 0x04001F3A RID: 7994
	public TravellingCargoLander.GroundedStates grounded;

	// Token: 0x0200163D RID: 5693
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006F0E RID: 28430
		public int landerWidth = 1;

		// Token: 0x04006F0F RID: 28431
		public float landingSpeed = 5f;

		// Token: 0x04006F10 RID: 28432
		public bool deployOnLanding;
	}

	// Token: 0x0200163E RID: 5694
	public class TravelStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x04006F11 RID: 28433
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State travelling;

		// Token: 0x04006F12 RID: 28434
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State transferWorlds;
	}

	// Token: 0x0200163F RID: 5695
	public class LandingStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x04006F13 RID: 28435
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State landing;

		// Token: 0x04006F14 RID: 28436
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State impact;
	}

	// Token: 0x02001640 RID: 5696
	public class GroundedStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x04006F15 RID: 28437
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State loaded;

		// Token: 0x04006F16 RID: 28438
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State emptying;

		// Token: 0x04006F17 RID: 28439
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State empty;
	}

	// Token: 0x02001641 RID: 5697
	public class StatesInstance : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.GameInstance
	{
		// Token: 0x06009191 RID: 37265 RVA: 0x00350B5F File Offset: 0x0034ED5F
		public StatesInstance(IStateMachineTarget master, TravellingCargoLander.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
		}

		// Token: 0x06009192 RID: 37266 RVA: 0x00350B80 File Offset: 0x0034ED80
		public void Travel(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.travel);
		}

		// Token: 0x06009193 RID: 37267 RVA: 0x00350BC8 File Offset: 0x0034EDC8
		public void StartLand()
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(base.sm.destinationWorld.Get(this));
			Vector3 position = Grid.CellToPosCBC(ClusterManager.Instance.GetRandomSurfaceCell(world.id, base.def.landerWidth, true), this.animController.sceneLayer);
			base.transform.SetPosition(position);
		}

		// Token: 0x06009194 RID: 37268 RVA: 0x00350C2C File Offset: 0x0034EE2C
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

		// Token: 0x06009195 RID: 37269 RVA: 0x00350C84 File Offset: 0x0034EE84
		public void MoveToSpace()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = false;
			}
			base.gameObject.transform.SetPosition(new Vector3(-1f, -1f, Grid.GetLayerZ(this.animController.sceneLayer)));
		}

		// Token: 0x06009196 RID: 37270 RVA: 0x00350CD8 File Offset: 0x0034EED8
		public void MoveToWorld()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = true;
			}
		}

		// Token: 0x06009197 RID: 37271 RVA: 0x00350CFC File Offset: 0x0034EEFC
		public void ResetAnimPosition()
		{
			this.animController.Offset = Vector3.up * this.flightAnimOffset;
		}

		// Token: 0x06009198 RID: 37272 RVA: 0x00350D19 File Offset: 0x0034EF19
		public void LandingUpdate(float dt)
		{
			this.flightAnimOffset = Mathf.Max(this.flightAnimOffset - dt * base.def.landingSpeed, 0f);
			this.ResetAnimPosition();
		}

		// Token: 0x06009199 RID: 37273 RVA: 0x00350D48 File Offset: 0x0034EF48
		public void DoLand()
		{
			this.animController.Offset = Vector3.zero;
			OccupyArea component = base.smi.GetComponent<OccupyArea>();
			if (component != null)
			{
				component.ApplyToCells = true;
			}
			if (base.def.deployOnLanding && this.CheckIfLoaded())
			{
				base.sm.emptyCargo.Trigger(this);
			}
		}

		// Token: 0x0600919A RID: 37274 RVA: 0x00350DA8 File Offset: 0x0034EFA8
		public bool CheckIfLoaded()
		{
			bool flag = false;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				flag |= (component.GetStoredMinionInfo().Count > 0);
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null && !component2.IsEmpty())
			{
				flag = true;
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x04006F18 RID: 28440
		[Serialize]
		public float flightAnimOffset = 50f;

		// Token: 0x04006F19 RID: 28441
		public KBatchedAnimController animController;
	}
}
