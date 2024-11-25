using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000AC2 RID: 2754
[SerializationConfig(MemberSerialization.OptIn)]
public class CommandModule : StateMachineComponent<CommandModule.StatesInstance>
{
	// Token: 0x06005197 RID: 20887 RVA: 0x001D4193 File Offset: 0x001D2393
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rocketStats = new RocketStats(this);
		this.conditions = base.GetComponent<CommandConditions>();
	}

	// Token: 0x06005198 RID: 20888 RVA: 0x001D41B4 File Offset: 0x001D23B4
	public void ReleaseAstronaut(bool fill_bladder)
	{
		if (this.releasingAstronaut || this.robotPilotControlled)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			GameObject gameObject = component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
			if (!(gameObject == null))
			{
				if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1)])
				{
					gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
				}
				if (fill_bladder)
				{
					AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(gameObject);
					if (amountInstance != null)
					{
						amountInstance.value = amountInstance.GetMax();
					}
				}
			}
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x06005199 RID: 20889 RVA: 0x001D42A8 File Offset: 0x001D24A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.storage = base.GetComponent<Storage>();
		if (!this.robotPilotControlled)
		{
			this.assignable = base.GetComponent<Assignable>();
			this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAssignTo));
			int cell = Grid.PosToCell(base.gameObject);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("CommandModule.gantryChanged", base.gameObject, cell, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnGantryChanged));
			this.OnGantryChanged(null);
		}
		base.smi.StartSM();
	}

	// Token: 0x0600519A RID: 20890 RVA: 0x001D4344 File Offset: 0x001D2544
	private bool CanAssignTo(MinionAssignablesProxy worker)
	{
		if (worker.target is MinionIdentity)
		{
			return (worker.target as KMonoBehaviour).GetComponent<MinionResume>().HasPerk(Db.Get().SkillPerks.CanUseRockets);
		}
		return worker.target is StoredMinionIdentity && (worker.target as StoredMinionIdentity).HasPerk(Db.Get().SkillPerks.CanUseRockets);
	}

	// Token: 0x0600519B RID: 20891 RVA: 0x001D43B4 File Offset: 0x001D25B4
	private static bool HasValidGantry(GameObject go)
	{
		int num = Grid.OffsetCell(Grid.PosToCell(go), 0, -1);
		return Grid.IsValidCell(num) && Grid.FakeFloor[num];
	}

	// Token: 0x0600519C RID: 20892 RVA: 0x001D43E4 File Offset: 0x001D25E4
	private void OnGantryChanged(object data)
	{
		if (base.gameObject != null)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
			if (CommandModule.HasValidGantry(base.smi.master.gameObject))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, null);
			}
			else
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, null);
			}
			base.smi.sm.gantryChanged.Trigger(base.smi);
		}
	}

	// Token: 0x0600519D RID: 20893 RVA: 0x001D449C File Offset: 0x001D269C
	private Chore CreateWorkChore()
	{
		WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRockets);
		workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, this.assignable);
		return workChore;
	}

	// Token: 0x0600519E RID: 20894 RVA: 0x001D4512 File Offset: 0x001D2712
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry.Clear();
		this.ReleaseAstronaut(false);
		base.smi.StopSM("cleanup");
	}

	// Token: 0x04003602 RID: 13826
	public Storage storage;

	// Token: 0x04003603 RID: 13827
	public RocketStats rocketStats;

	// Token: 0x04003604 RID: 13828
	public CommandConditions conditions;

	// Token: 0x04003605 RID: 13829
	private bool releasingAstronaut;

	// Token: 0x04003606 RID: 13830
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x04003607 RID: 13831
	public Assignable assignable;

	// Token: 0x04003608 RID: 13832
	public bool robotPilotControlled;

	// Token: 0x04003609 RID: 13833
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x02001B07 RID: 6919
	public class StatesInstance : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.GameInstance
	{
		// Token: 0x0600A207 RID: 41479 RVA: 0x0038474B File Offset: 0x0038294B
		public StatesInstance(CommandModule master) : base(master)
		{
		}

		// Token: 0x0600A208 RID: 41480 RVA: 0x00384754 File Offset: 0x00382954
		public void SetSuspended(bool suspended)
		{
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				component.allowItemRemoval = !suspended;
			}
			ManualDeliveryKG component2 = base.GetComponent<ManualDeliveryKG>();
			if (component2 != null)
			{
				component2.Pause(suspended, "Rocket is suspended");
			}
		}

		// Token: 0x0600A209 RID: 41481 RVA: 0x00384798 File Offset: 0x00382998
		public bool CheckStoredMinionIsAssignee()
		{
			if (base.smi.master.robotPilotControlled)
			{
				return true;
			}
			foreach (MinionStorage.Info info in base.GetComponent<MinionStorage>().GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					KPrefabID kprefabID = info.serializedMinion.Get();
					if (!(kprefabID == null))
					{
						StoredMinionIdentity component = kprefabID.GetComponent<StoredMinionIdentity>();
						if (base.GetComponent<Assignable>().assignee == component.assignableProxy.Get())
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	// Token: 0x02001B08 RID: 6920
	public class States : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule>
	{
		// Token: 0x0600A20A RID: 41482 RVA: 0x00384844 File Offset: 0x00382A44
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grounded;
			this.grounded.PlayAnim("grounded", KAnim.PlayMode.Loop).DefaultState(this.grounded.awaitingAstronaut).TagTransition(GameTags.RocketNotOnGround, this.spaceborne, false);
			this.grounded.refreshChore.GoTo(this.grounded.awaitingAstronaut);
			this.grounded.awaitingAstronaut.Enter(delegate(CommandModule.StatesInstance smi)
			{
				if (smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.hasAstronaut);
				}
				Game.Instance.userMenu.Refresh(smi.gameObject);
			}).EventHandler(GameHashes.AssigneeChanged, delegate(CommandModule.StatesInstance smi)
			{
				if (smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.hasAstronaut);
				}
				else
				{
					smi.GoTo(this.grounded.refreshChore);
				}
				Game.Instance.userMenu.Refresh(smi.gameObject);
			}).ToggleChore((CommandModule.StatesInstance smi) => smi.master.CreateWorkChore(), this.grounded.hasAstronaut);
			this.grounded.hasAstronaut.EventHandler(GameHashes.AssigneeChanged, delegate(CommandModule.StatesInstance smi)
			{
				if (!smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.waitingToRelease);
				}
			});
			this.grounded.waitingToRelease.ToggleStatusItem(Db.Get().BuildingStatusItems.DisembarkingDuplicant, null).OnSignal(this.gantryChanged, this.grounded.awaitingAstronaut, delegate(CommandModule.StatesInstance smi)
			{
				if (CommandModule.HasValidGantry(smi.gameObject))
				{
					smi.master.ReleaseAstronaut(this.accumulatedPee.Get(smi));
					this.accumulatedPee.Set(false, smi, false);
					Game.Instance.userMenu.Refresh(smi.gameObject);
					return true;
				}
				return false;
			});
			this.spaceborne.DefaultState(this.spaceborne.launch);
			this.spaceborne.launch.Enter(delegate(CommandModule.StatesInstance smi)
			{
				smi.SetSuspended(true);
			}).GoTo(this.spaceborne.idle);
			this.spaceborne.idle.TagTransition(GameTags.RocketNotOnGround, this.spaceborne.land, true);
			this.spaceborne.land.Enter(delegate(CommandModule.StatesInstance smi)
			{
				smi.SetSuspended(false);
				Game.Instance.userMenu.Refresh(smi.gameObject);
				this.accumulatedPee.Set(true, smi, false);
			}).GoTo(this.grounded.waitingToRelease);
		}

		// Token: 0x04007E87 RID: 32391
		public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.Signal gantryChanged;

		// Token: 0x04007E88 RID: 32392
		public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.BoolParameter accumulatedPee;

		// Token: 0x04007E89 RID: 32393
		public CommandModule.States.GroundedStates grounded;

		// Token: 0x04007E8A RID: 32394
		public CommandModule.States.SpaceborneStates spaceborne;

		// Token: 0x0200260A RID: 9738
		public class GroundedStates : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
		{
			// Token: 0x0400A948 RID: 43336
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State refreshChore;

			// Token: 0x0400A949 RID: 43337
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State awaitingAstronaut;

			// Token: 0x0400A94A RID: 43338
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State hasAstronaut;

			// Token: 0x0400A94B RID: 43339
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State waitingToRelease;
		}

		// Token: 0x0200260B RID: 9739
		public class SpaceborneStates : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
		{
			// Token: 0x0400A94C RID: 43340
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State launch;

			// Token: 0x0400A94D RID: 43341
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State idle;

			// Token: 0x0400A94E RID: 43342
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State land;
		}
	}
}
