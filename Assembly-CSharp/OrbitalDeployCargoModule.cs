using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000AD4 RID: 2772
public class OrbitalDeployCargoModule : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>
{
	// Token: 0x06005255 RID: 21077 RVA: 0x001D837C File Offset: 0x001D657C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.ClusterDestinationReached, delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			if (smi.AutoDeploy && smi.IsValidDropLocation())
			{
				smi.DeployCargoPods();
			}
		});
		this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.grounded.loading.PlayAnim((OrbitalDeployCargoModule.StatesInstance smi) => smi.GetLoadingAnimName(), KAnim.PlayMode.Once).ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).OnAnimQueueComplete(this.grounded.loaded);
		this.grounded.loaded.ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).EventTransition(GameHashes.OnStorageChange, this.grounded.loading, (OrbitalDeployCargoModule.StatesInstance smi) => smi.NeedsVisualUpdate());
		this.grounded.empty.Enter(delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			this.numVisualCapsules.Set(0, smi, false);
		}).PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsTrue);
		this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.not_grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
		this.not_grounded.emptying.PlayAnim("deploying").GoTo(this.not_grounded.empty);
		this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.not_grounded.loaded, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsTrue);
	}

	// Token: 0x0400364B RID: 13899
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.BoolParameter hasCargo;

	// Token: 0x0400364C RID: 13900
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.Signal emptyCargo;

	// Token: 0x0400364D RID: 13901
	public OrbitalDeployCargoModule.GroundedStates grounded;

	// Token: 0x0400364E RID: 13902
	public OrbitalDeployCargoModule.NotGroundedStates not_grounded;

	// Token: 0x0400364F RID: 13903
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IntParameter numVisualCapsules;

	// Token: 0x02001B1F RID: 6943
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007EDC RID: 32476
		public float numCapsules;
	}

	// Token: 0x02001B20 RID: 6944
	public class GroundedStates : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State
	{
		// Token: 0x04007EDD RID: 32477
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loading;

		// Token: 0x04007EDE RID: 32478
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loaded;

		// Token: 0x04007EDF RID: 32479
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State empty;
	}

	// Token: 0x02001B21 RID: 6945
	public class NotGroundedStates : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State
	{
		// Token: 0x04007EE0 RID: 32480
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loaded;

		// Token: 0x04007EE1 RID: 32481
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State emptying;

		// Token: 0x04007EE2 RID: 32482
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State empty;
	}

	// Token: 0x02001B22 RID: 6946
	public class StatesInstance : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.GameInstance, IEmptyableCargo
	{
		// Token: 0x0600A27E RID: 41598 RVA: 0x00387348 File Offset: 0x00385548
		public StatesInstance(IStateMachineTarget master, OrbitalDeployCargoModule.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new LoadingCompleteCondition(this.storage));
			base.gameObject.Subscribe(-1683615038, new Action<object>(this.SetupMeter));
		}

		// Token: 0x0600A27F RID: 41599 RVA: 0x0038739E File Offset: 0x0038559E
		private void SetupMeter(object obj)
		{
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
		}

		// Token: 0x0600A280 RID: 41600 RVA: 0x003873B8 File Offset: 0x003855B8
		protected override void OnCleanUp()
		{
			base.gameObject.Unsubscribe(-1683615038, new Action<object>(this.SetupMeter));
			base.OnCleanUp();
		}

		// Token: 0x0600A281 RID: 41601 RVA: 0x003873DC File Offset: 0x003855DC
		public bool NeedsVisualUpdate()
		{
			int num = base.sm.numVisualCapsules.Get(this);
			int num2 = Mathf.FloorToInt(this.storage.MassStored() / 200f);
			if (num < num2)
			{
				base.sm.numVisualCapsules.Delta(1, this);
				return true;
			}
			return false;
		}

		// Token: 0x0600A282 RID: 41602 RVA: 0x0038742C File Offset: 0x0038562C
		public string GetLoadingAnimName()
		{
			int num = base.sm.numVisualCapsules.Get(this);
			int num2 = Mathf.RoundToInt(this.storage.capacityKg / 200f);
			if (num == num2)
			{
				return "loading6_full";
			}
			if (num == num2 - 1)
			{
				return "loading5";
			}
			if (num == num2 - 2)
			{
				return "loading4";
			}
			if (num == num2 - 3 || num > 2)
			{
				return "loading3_repeat";
			}
			if (num == 2)
			{
				return "loading2";
			}
			if (num == 1)
			{
				return "loading1";
			}
			return "deployed";
		}

		// Token: 0x0600A283 RID: 41603 RVA: 0x003874B0 File Offset: 0x003856B0
		public void DeployCargoPods()
		{
			Clustercraft component = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			ClusterGridEntity orbitAsteroid = component.GetOrbitAsteroid();
			if (orbitAsteroid != null)
			{
				WorldContainer component2 = orbitAsteroid.GetComponent<WorldContainer>();
				int id = component2.id;
				Vector3 position = new Vector3(component2.minimumBounds.x + 1f, component2.maximumBounds.y, Grid.GetLayerZ(Grid.SceneLayer.Front));
				while (this.storage.MassStored() > 0f)
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), position);
					gameObject.GetComponent<Pickupable>().deleteOffGrid = false;
					float num = 0f;
					while (num < 200f && this.storage.MassStored() > 0f)
					{
						num += this.storage.Transfer(gameObject.GetComponent<Storage>(), GameTags.Stored, 200f - num, false, true);
					}
					gameObject.SetActive(true);
					gameObject.GetSMI<RailGunPayload.StatesInstance>().Travel(component.Location, component2.GetMyWorldLocation());
				}
			}
			this.CheckIfLoaded();
		}

		// Token: 0x0600A284 RID: 41604 RVA: 0x003875D0 File Offset: 0x003857D0
		public bool CheckIfLoaded()
		{
			bool flag = this.storage.MassStored() > 0f;
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x0600A285 RID: 41605 RVA: 0x00387619 File Offset: 0x00385819
		public bool IsValidDropLocation()
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetOrbitAsteroid() != null;
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600A286 RID: 41606 RVA: 0x00387636 File Offset: 0x00385836
		// (set) Token: 0x0600A287 RID: 41607 RVA: 0x0038763E File Offset: 0x0038583E
		public bool AutoDeploy
		{
			get
			{
				return this.autoDeploy;
			}
			set
			{
				this.autoDeploy = value;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x0600A288 RID: 41608 RVA: 0x00387647 File Offset: 0x00385847
		public bool CanAutoDeploy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A289 RID: 41609 RVA: 0x0038764A File Offset: 0x0038584A
		public void EmptyCargo()
		{
			this.DeployCargoPods();
		}

		// Token: 0x0600A28A RID: 41610 RVA: 0x00387652 File Offset: 0x00385852
		public bool CanEmptyCargo()
		{
			return base.sm.hasCargo.Get(base.smi) && this.IsValidDropLocation();
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x0600A28B RID: 41611 RVA: 0x00387674 File Offset: 0x00385874
		public bool ChooseDuplicant
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x0600A28C RID: 41612 RVA: 0x00387677 File Offset: 0x00385877
		// (set) Token: 0x0600A28D RID: 41613 RVA: 0x0038767A File Offset: 0x0038587A
		public MinionIdentity ChosenDuplicant
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x0600A28E RID: 41614 RVA: 0x0038767C File Offset: 0x0038587C
		public bool ModuleDeployed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04007EE3 RID: 32483
		private Storage storage;

		// Token: 0x04007EE4 RID: 32484
		[Serialize]
		private bool autoDeploy;
	}
}
