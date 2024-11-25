using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000AEB RID: 2795
[SerializationConfig(MemberSerialization.OptIn)]
public class TouristModule : StateMachineComponent<TouristModule.StatesInstance>
{
	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06005345 RID: 21317 RVA: 0x001DDC0C File Offset: 0x001DBE0C
	public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

	// Token: 0x06005346 RID: 21318 RVA: 0x001DDC14 File Offset: 0x001DBE14
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005347 RID: 21319 RVA: 0x001DDC1C File Offset: 0x001DBE1C
	public void SetSuspended(bool state)
	{
		this.isSuspended = state;
	}

	// Token: 0x06005348 RID: 21320 RVA: 0x001DDC28 File Offset: 0x001DBE28
	public void ReleaseAstronaut(object data, bool applyBuff = false)
	{
		if (this.releasingAstronaut)
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
			if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1)])
			{
				gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
				if (applyBuff)
				{
					gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("SpaceTourist"), true);
					JoyBehaviourMonitor.Instance smi = gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
					if (smi != null)
					{
						smi.GoToOverjoyed();
					}
				}
			}
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x06005349 RID: 21321 RVA: 0x001DDD18 File Offset: 0x001DBF18
	public void OnSuspend(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.capacityKg = component.MassStored();
			component.allowItemRemoval = false;
		}
		if (base.GetComponent<ManualDeliveryKG>() != null)
		{
			UnityEngine.Object.Destroy(base.GetComponent<ManualDeliveryKG>());
		}
		this.SetSuspended(true);
	}

	// Token: 0x0600534A RID: 21322 RVA: 0x001DDD68 File Offset: 0x001DBF68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.storage = base.GetComponent<Storage>();
		this.assignable = base.GetComponent<Assignable>();
		base.smi.StartSM();
		int cell = Grid.PosToCell(base.gameObject);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("TouristModule.gantryChanged", base.gameObject, cell, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnGantryChanged));
		this.OnGantryChanged(null);
		base.Subscribe<TouristModule>(-1277991738, TouristModule.OnSuspendDelegate);
		base.Subscribe<TouristModule>(684616645, TouristModule.OnAssigneeChangedDelegate);
	}

	// Token: 0x0600534B RID: 21323 RVA: 0x001DDE08 File Offset: 0x001DC008
	private void OnGantryChanged(object data)
	{
		if (base.gameObject != null)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
			int i = Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1);
			if (Grid.FakeFloor[i])
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, null);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, null);
		}
	}

	// Token: 0x0600534C RID: 21324 RVA: 0x001DDEB4 File Offset: 0x001DC0B4
	private Chore CreateWorkChore()
	{
		WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, this.assignable);
		return workChore;
	}

	// Token: 0x0600534D RID: 21325 RVA: 0x001DDF0C File Offset: 0x001DC10C
	private void OnAssigneeChanged(object data)
	{
		if (data == null && base.gameObject.HasTag(GameTags.RocketOnGround) && base.GetComponent<MinionStorage>().GetStoredMinionInfo().Count > 0)
		{
			this.ReleaseAstronaut(null, false);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	// Token: 0x0600534E RID: 21326 RVA: 0x001DDF64 File Offset: 0x001DC164
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry.Clear();
		this.ReleaseAstronaut(null, false);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.smi.StopSM("cleanup");
	}

	// Token: 0x040036E2 RID: 14050
	public Storage storage;

	// Token: 0x040036E3 RID: 14051
	[Serialize]
	private bool isSuspended;

	// Token: 0x040036E4 RID: 14052
	private bool releasingAstronaut;

	// Token: 0x040036E5 RID: 14053
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x040036E6 RID: 14054
	public Assignable assignable;

	// Token: 0x040036E7 RID: 14055
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040036E8 RID: 14056
	private static readonly EventSystem.IntraObjectHandler<TouristModule> OnSuspendDelegate = new EventSystem.IntraObjectHandler<TouristModule>(delegate(TouristModule component, object data)
	{
		component.OnSuspend(data);
	});

	// Token: 0x040036E9 RID: 14057
	private static readonly EventSystem.IntraObjectHandler<TouristModule> OnAssigneeChangedDelegate = new EventSystem.IntraObjectHandler<TouristModule>(delegate(TouristModule component, object data)
	{
		component.OnAssigneeChanged(data);
	});

	// Token: 0x02001B3E RID: 6974
	public class StatesInstance : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.GameInstance
	{
		// Token: 0x0600A2F3 RID: 41715 RVA: 0x00388C68 File Offset: 0x00386E68
		public StatesInstance(TouristModule smi) : base(smi)
		{
			smi.gameObject.Subscribe(-887025858, delegate(object data)
			{
				smi.SetSuspended(false);
				smi.ReleaseAstronaut(null, true);
				smi.assignable.Unassign();
			});
		}
	}

	// Token: 0x02001B3F RID: 6975
	public class States : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule>
	{
		// Token: 0x0600A2F4 RID: 41716 RVA: 0x00388CB0 File Offset: 0x00386EB0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).GoTo(this.awaitingTourist);
			this.awaitingTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).ToggleChore((TouristModule.StatesInstance smi) => smi.master.CreateWorkChore(), this.hasTourist);
			this.hasTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.RocketLanded, this.idle, null).EventTransition(GameHashes.AssigneeChanged, this.idle, null);
		}

		// Token: 0x04007F32 RID: 32562
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State idle;

		// Token: 0x04007F33 RID: 32563
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State awaitingTourist;

		// Token: 0x04007F34 RID: 32564
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State hasTourist;
	}
}
