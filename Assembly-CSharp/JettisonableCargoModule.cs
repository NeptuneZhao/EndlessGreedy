using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ACA RID: 2762
public class JettisonableCargoModule : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>
{
	// Token: 0x06005206 RID: 20998 RVA: 0x001D6BDC File Offset: 0x001D4DDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
		this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.not_grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
		this.not_grounded.emptying.PlayAnim("deploying").Update(delegate(JettisonableCargoModule.StatesInstance smi, float dt)
		{
			if (smi.CheckReadyForFinalDeploy())
			{
				smi.FinalDeploy();
				smi.GoTo(smi.sm.not_grounded.empty);
			}
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ClusterLocationChanged, (JettisonableCargoModule.StatesInstance smi) => Game.Instance, this.not_grounded, null).Exit(delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CancelPendingDeploy();
		});
		this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.not_grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
	}

	// Token: 0x04003626 RID: 13862
	public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.BoolParameter hasCargo;

	// Token: 0x04003627 RID: 13863
	public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Signal emptyCargo;

	// Token: 0x04003628 RID: 13864
	public JettisonableCargoModule.GroundedStates grounded;

	// Token: 0x04003629 RID: 13865
	public JettisonableCargoModule.NotGroundedStates not_grounded;

	// Token: 0x02001B11 RID: 6929
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007EA8 RID: 32424
		public DefComponent<Storage> landerContainer;

		// Token: 0x04007EA9 RID: 32425
		public Tag landerPrefabID;

		// Token: 0x04007EAA RID: 32426
		public Vector3 cargoDropOffset;

		// Token: 0x04007EAB RID: 32427
		public string clusterMapFXPrefabID;
	}

	// Token: 0x02001B12 RID: 6930
	public class GroundedStates : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
	{
		// Token: 0x04007EAC RID: 32428
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;

		// Token: 0x04007EAD RID: 32429
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
	}

	// Token: 0x02001B13 RID: 6931
	public class NotGroundedStates : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
	{
		// Token: 0x04007EAE RID: 32430
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;

		// Token: 0x04007EAF RID: 32431
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State emptying;

		// Token: 0x04007EB0 RID: 32432
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
	}

	// Token: 0x02001B14 RID: 6932
	public class StatesInstance : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.GameInstance, IEmptyableCargo
	{
		// Token: 0x0600A235 RID: 41525 RVA: 0x00384F93 File Offset: 0x00383193
		public StatesInstance(IStateMachineTarget master, JettisonableCargoModule.Def def) : base(master, def)
		{
			this.landerContainer = def.landerContainer.Get(this);
		}

		// Token: 0x0600A236 RID: 41526 RVA: 0x00384FB0 File Offset: 0x003831B0
		private void ChooseLanderLocation()
		{
			ClusterGridEntity stableOrbitAsteroid = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid();
			if (stableOrbitAsteroid != null)
			{
				WorldContainer component = stableOrbitAsteroid.GetComponent<WorldContainer>();
				Placeable component2 = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<Placeable>();
				component2.restrictWorldId = component.id;
				component.LookAtSurface();
				ClusterManager.Instance.SetActiveWorld(component.id);
				ManagementMenu.Instance.CloseAll();
				PlaceTool.Instance.Activate(component2, new Action<Placeable, int>(this.OnLanderPlaced));
			}
		}

		// Token: 0x0600A237 RID: 41527 RVA: 0x00385048 File Offset: 0x00383248
		private void OnLanderPlaced(Placeable lander, int cell)
		{
			this.landerPlaced = true;
			this.landerPlacementCell = cell;
			if (lander.GetComponent<MinionStorage>() != null)
			{
				this.OpenMoveChoreForChosenDuplicant();
			}
			ManagementMenu.Instance.ToggleClusterMap();
			base.sm.emptyCargo.Trigger(base.smi);
			ClusterMapScreen.Instance.SelectEntity(base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<ClusterGridEntity>(), true);
		}

		// Token: 0x0600A238 RID: 41528 RVA: 0x003850B4 File Offset: 0x003832B4
		private void OpenMoveChoreForChosenDuplicant()
		{
			RocketModuleCluster component = base.master.GetComponent<RocketModuleCluster>();
			Clustercraft craft = component.CraftInterface.GetComponent<Clustercraft>();
			MinionStorage storage = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<MinionStorage>();
			this.EnableTeleport(true);
			this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>().SetModuleDeployChore(this.landerPlacementCell, delegate(Chore obj)
			{
				Game.Instance.assignmentManager.RemoveFromWorld(this.ChosenDuplicant.assignableProxy.Get(), craft.ModuleInterface.GetInteriorWorld().id);
				storage.SerializeMinion(this.ChosenDuplicant.gameObject);
				this.EnableTeleport(false);
			});
		}

		// Token: 0x0600A239 RID: 41529 RVA: 0x00385138 File Offset: 0x00383338
		private void EnableTeleport(bool enable)
		{
			ClustercraftExteriorDoor component = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponent<ClustercraftExteriorDoor>();
			ClustercraftInteriorDoor interiorDoor = component.GetInteriorDoor();
			AccessControl component2 = component.GetInteriorDoor().GetComponent<AccessControl>();
			NavTeleporter component3 = base.GetComponent<NavTeleporter>();
			if (enable)
			{
				component3.SetOverrideCell(this.landerPlacementCell);
				interiorDoor.GetComponent<NavTeleporter>().SetTarget(component3);
				component3.SetTarget(interiorDoor.GetComponent<NavTeleporter>());
				using (List<MinionIdentity>.Enumerator enumerator = Components.MinionIdentities.GetWorldItems(interiorDoor.GetMyWorldId(), false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MinionIdentity minionIdentity = enumerator.Current;
						component2.SetPermission(minionIdentity.assignableProxy.Get(), (minionIdentity == this.ChosenDuplicant) ? AccessControl.Permission.Both : AccessControl.Permission.Neither);
					}
					return;
				}
			}
			component3.SetOverrideCell(-1);
			interiorDoor.GetComponent<NavTeleporter>().SetTarget(null);
			component3.SetTarget(null);
			component2.SetPermission(this.ChosenDuplicant.assignableProxy.Get(), AccessControl.Permission.Neither);
		}

		// Token: 0x0600A23A RID: 41530 RVA: 0x00385250 File Offset: 0x00383450
		public void FinalDeploy()
		{
			this.landerPlaced = false;
			Placeable component = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<Placeable>();
			this.landerContainer.FindFirst(base.def.landerPrefabID);
			this.landerContainer.Drop(component.gameObject, true);
			TreeFilterable component2 = base.GetComponent<TreeFilterable>();
			TreeFilterable component3 = component.GetComponent<TreeFilterable>();
			if (component3 != null)
			{
				component3.UpdateFilters(component2.AcceptedTags);
			}
			Storage component4 = component.GetComponent<Storage>();
			if (component4 != null)
			{
				Storage[] components = base.gameObject.GetComponents<Storage>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].Transfer(component4, false, true);
				}
			}
			Vector3 position = Grid.CellToPosCBC(this.landerPlacementCell, Grid.SceneLayer.Building);
			component.transform.SetPosition(position);
			component.gameObject.SetActive(true);
			base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().gameObject.Trigger(1792516731, component);
			component.Trigger(1792516731, base.gameObject);
			GameObject gameObject = Assets.TryGetPrefab(base.smi.def.clusterMapFXPrefabID);
			if (gameObject != null)
			{
				this.clusterMapFX = GameUtil.KInstantiate(gameObject, Grid.SceneLayer.Background, null, 0);
				this.clusterMapFX.SetActive(true);
				this.clusterMapFX.GetComponent<ClusterFXEntity>().Init(component.GetMyWorldLocation(), Vector3.zero);
				component.Subscribe(1969584890, delegate(object data)
				{
					if (!this.clusterMapFX.IsNullOrDestroyed())
					{
						Util.KDestroyGameObject(this.clusterMapFX);
					}
				});
				component.Subscribe(1591811118, delegate(object data)
				{
					if (!this.clusterMapFX.IsNullOrDestroyed())
					{
						Util.KDestroyGameObject(this.clusterMapFX);
					}
				});
			}
		}

		// Token: 0x0600A23B RID: 41531 RVA: 0x003853F8 File Offset: 0x003835F8
		public bool CheckReadyForFinalDeploy()
		{
			MinionStorage component = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<MinionStorage>();
			return !(component != null) || component.GetStoredMinionInfo().Count > 0;
		}

		// Token: 0x0600A23C RID: 41532 RVA: 0x0038543A File Offset: 0x0038363A
		public void CancelPendingDeploy()
		{
			this.landerPlaced = false;
			if (this.ChosenDuplicant != null && this.CheckIfLoaded())
			{
				this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>().CancelModuleDeployChore();
			}
		}

		// Token: 0x0600A23D RID: 41533 RVA: 0x0038546C File Offset: 0x0038366C
		public bool CheckIfLoaded()
		{
			bool flag = false;
			using (List<GameObject>.Enumerator enumerator = this.landerContainer.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.PrefabID() == base.def.landerPrefabID)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x0600A23E RID: 41534 RVA: 0x00385504 File Offset: 0x00383704
		public bool IsValidDropLocation()
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid() != null;
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x0600A23F RID: 41535 RVA: 0x00385521 File Offset: 0x00383721
		// (set) Token: 0x0600A240 RID: 41536 RVA: 0x00385529 File Offset: 0x00383729
		public bool AutoDeploy { get; set; }

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x0600A241 RID: 41537 RVA: 0x00385532 File Offset: 0x00383732
		public bool CanAutoDeploy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600A242 RID: 41538 RVA: 0x00385535 File Offset: 0x00383735
		public void EmptyCargo()
		{
			this.ChooseLanderLocation();
		}

		// Token: 0x0600A243 RID: 41539 RVA: 0x00385540 File Offset: 0x00383740
		public bool CanEmptyCargo()
		{
			return base.sm.hasCargo.Get(base.smi) && this.IsValidDropLocation() && (!this.ChooseDuplicant || this.ChosenDuplicant != null) && !this.landerPlaced;
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x0600A244 RID: 41540 RVA: 0x00385590 File Offset: 0x00383790
		public bool ChooseDuplicant
		{
			get
			{
				GameObject gameObject = this.landerContainer.FindFirst(base.def.landerPrefabID);
				return !(gameObject == null) && gameObject.GetComponent<MinionStorage>() != null;
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x0600A245 RID: 41541 RVA: 0x003855CB File Offset: 0x003837CB
		public bool ModuleDeployed
		{
			get
			{
				return this.landerPlaced;
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x0600A246 RID: 41542 RVA: 0x003855D3 File Offset: 0x003837D3
		// (set) Token: 0x0600A247 RID: 41543 RVA: 0x003855DB File Offset: 0x003837DB
		public MinionIdentity ChosenDuplicant
		{
			get
			{
				return this.chosenDuplicant;
			}
			set
			{
				this.chosenDuplicant = value;
			}
		}

		// Token: 0x04007EB1 RID: 32433
		private Storage landerContainer;

		// Token: 0x04007EB2 RID: 32434
		private bool landerPlaced;

		// Token: 0x04007EB3 RID: 32435
		private MinionIdentity chosenDuplicant;

		// Token: 0x04007EB4 RID: 32436
		private int landerPlacementCell;

		// Token: 0x04007EB5 RID: 32437
		public GameObject clusterMapFX;
	}
}
