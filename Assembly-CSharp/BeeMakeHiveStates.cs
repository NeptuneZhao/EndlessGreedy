using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class BeeMakeHiveStates : GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>
{
	// Token: 0x06000368 RID: 872 RVA: 0x0001C7E8 File Offset: 0x0001A9E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findBuildLocation;
		this.root.DoNothing();
		this.findBuildLocation.Enter(delegate(BeeMakeHiveStates.Instance smi)
		{
			this.FindBuildLocation(smi);
			if (smi.targetBuildCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToBuildLocation);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToBuildLocation.MoveTo((BeeMakeHiveStates.Instance smi) => smi.targetBuildCell, this.doBuild, this.behaviourcomplete, false);
		this.doBuild.PlayAnim("hive_grow_pre").EventHandler(GameHashes.AnimQueueComplete, delegate(BeeMakeHiveStates.Instance smi)
		{
			if (smi.gameObject.GetComponent<Bee>().FindHiveInRoom() == null)
			{
				smi.builtHome = true;
				smi.BuildHome();
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToMakeHome, false).Exit(delegate(BeeMakeHiveStates.Instance smi)
		{
			if (smi.builtHome)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			}
		});
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0001C8B8 File Offset: 0x0001AAB8
	private void FindBuildLocation(BeeMakeHiveStates.Instance smi)
	{
		smi.targetBuildCell = Grid.InvalidCell;
		GameObject prefab = Assets.GetPrefab("BeeHive".ToTag());
		BuildingPlacementQuery buildingPlacementQuery = PathFinderQueries.buildingPlacementQuery.Reset(1, prefab);
		smi.GetComponent<Navigator>().RunQuery(buildingPlacementQuery);
		if (buildingPlacementQuery.result_cells.Count > 0)
		{
			smi.targetBuildCell = buildingPlacementQuery.result_cells[UnityEngine.Random.Range(0, buildingPlacementQuery.result_cells.Count)];
		}
	}

	// Token: 0x04000262 RID: 610
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State findBuildLocation;

	// Token: 0x04000263 RID: 611
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State moveToBuildLocation;

	// Token: 0x04000264 RID: 612
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State doBuild;

	// Token: 0x04000265 RID: 613
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State behaviourcomplete;

	// Token: 0x02000FF5 RID: 4085
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FF6 RID: 4086
	public new class Instance : GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.GameInstance
	{
		// Token: 0x06007AE2 RID: 31458 RVA: 0x00302C9A File Offset: 0x00300E9A
		public Instance(Chore<BeeMakeHiveStates.Instance> chore, BeeMakeHiveStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToMakeHome);
		}

		// Token: 0x06007AE3 RID: 31459 RVA: 0x00302CC0 File Offset: 0x00300EC0
		public void BuildHome()
		{
			Vector3 position = Grid.CellToPos(this.targetBuildCell, CellAlignment.Bottom, Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("BeeHive".ToTag()), position, Quaternion.identity, null, null, true, 0);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ElementID = SimHashes.Creature;
			component.Temperature = base.gameObject.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			gameObject.GetSMI<BeeHive.StatesInstance>().SetUpNewHive();
		}

		// Token: 0x04005BDE RID: 23518
		public int targetBuildCell;

		// Token: 0x04005BDF RID: 23519
		public bool builtHome;
	}
}
