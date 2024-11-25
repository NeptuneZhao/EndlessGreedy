using System;

// Token: 0x02000764 RID: 1892
public class ScannerModule : GameStateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>
{
	// Token: 0x060032CE RID: 13006 RVA: 0x00117404 File Offset: 0x00115604
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		}).EventHandler(GameHashes.RocketLaunched, delegate(ScannerModule.Instance smi)
		{
			smi.Scan();
		}).EventHandler(GameHashes.ClusterLocationChanged, (ScannerModule.Instance smi) => smi.GetComponent<RocketModuleCluster>().CraftInterface, delegate(ScannerModule.Instance smi)
		{
			smi.Scan();
		}).EventHandler(GameHashes.RocketModuleChanged, (ScannerModule.Instance smi) => smi.GetComponent<RocketModuleCluster>().CraftInterface, delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		}).Exit(delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		});
	}

	// Token: 0x020015ED RID: 5613
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006E33 RID: 28211
		public int scanRadius = 1;
	}

	// Token: 0x020015EE RID: 5614
	public new class Instance : GameStateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.GameInstance
	{
		// Token: 0x0600906A RID: 36970 RVA: 0x0034B81D File Offset: 0x00349A1D
		public Instance(IStateMachineTarget master, ScannerModule.Def def) : base(master, def)
		{
		}

		// Token: 0x0600906B RID: 36971 RVA: 0x0034B828 File Offset: 0x00349A28
		public void Scan()
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (component.Status == Clustercraft.CraftStatus.InFlight)
			{
				ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
				AxialI location = component.Location;
				smi.RevealLocation(location, base.def.scanRadius);
				foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetNotVisibleEntitiesAtAdjacentCell(location))
				{
					smi.RevealLocation(clusterGridEntity.Location, 0);
				}
			}
		}

		// Token: 0x0600906C RID: 36972 RVA: 0x0034B8C8 File Offset: 0x00349AC8
		public void SetFogOfWarAllowed()
		{
			CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
			if (craftInterface.HasClusterDestinationSelector())
			{
				bool flag = false;
				ClusterDestinationSelector clusterDestinationSelector = craftInterface.GetClusterDestinationSelector();
				bool canNavigateFogOfWar = clusterDestinationSelector.canNavigateFogOfWar;
				foreach (Ref<RocketModuleCluster> @ref in craftInterface.ClusterModules)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					if (((rocketModuleCluster != null) ? rocketModuleCluster.GetSMI<ScannerModule.Instance>() : null) != null)
					{
						flag = true;
						break;
					}
				}
				clusterDestinationSelector.canNavigateFogOfWar = flag;
				if (canNavigateFogOfWar && !flag)
				{
					ClusterTraveler component = craftInterface.GetComponent<ClusterTraveler>();
					if (component != null)
					{
						component.RevalidatePath(true);
					}
				}
				craftInterface.GetComponent<Clustercraft>().Trigger(-688990705, null);
			}
		}
	}
}
