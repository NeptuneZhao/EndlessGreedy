using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A68 RID: 2664
public class RocketUsageRestriction : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>
{
	// Token: 0x06004D4C RID: 19788 RVA: 0x001BB590 File Offset: 0x001B9790
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			if (DlcManager.FeatureClusterSpaceEnabled() && smi.master.gameObject.GetMyWorld().IsModuleInterior)
			{
				smi.Subscribe(493375141, new Action<object>(smi.OnRefreshUserMenu));
				smi.GoToRestrictionState();
				return;
			}
			smi.StopSM("Not inside rocket or no cluster space");
		});
		this.restriction.Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.AquireRocketControlStation)).Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			Components.RocketControlStations.OnAdd += new Action<RocketControlStation>(smi.ControlStationBuilt);
		}).Exit(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			Components.RocketControlStations.OnAdd -= new Action<RocketControlStation>(smi.ControlStationBuilt);
		});
		this.restriction.uncontrolled.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketRestriction, null).Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			this.RestrictUsage(smi, false);
		});
		this.restriction.controlled.DefaultState(this.restriction.controlled.nostation);
		this.restriction.controlled.nostation.Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).ParamTransition<GameObject>(this.rocketControlStation, this.restriction.controlled.controlled, GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.IsNotNull);
		this.restriction.controlled.controlled.OnTargetLost(this.rocketControlStation, this.restriction.controlled.nostation).Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).Target(this.rocketControlStation).EventHandler(GameHashes.RocketRestrictionChanged, new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).Target(this.masterTarget);
	}

	// Token: 0x06004D4D RID: 19789 RVA: 0x001BB735 File Offset: 0x001B9935
	private void OnRocketRestrictionChanged(RocketUsageRestriction.StatesInstance smi)
	{
		this.RestrictUsage(smi, smi.BuildingRestrictionsActive());
	}

	// Token: 0x06004D4E RID: 19790 RVA: 0x001BB744 File Offset: 0x001B9944
	private void RestrictUsage(RocketUsageRestriction.StatesInstance smi, bool restrict)
	{
		smi.master.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.RocketRestrictionInactive, !restrict && smi.isControlled, null);
		if (smi.isRestrictionApplied == restrict)
		{
			return;
		}
		smi.isRestrictionApplied = restrict;
		smi.operational.SetFlag(RocketUsageRestriction.rocketUsageAllowed, !smi.def.restrictOperational || !restrict);
		smi.master.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.RocketRestrictionActive, restrict, null);
		Storage[] components = smi.master.gameObject.GetComponents<Storage>();
		if (components != null && components.Length != 0)
		{
			for (int i = 0; i < components.Length; i++)
			{
				if (restrict)
				{
					smi.previousStorageAllowItemRemovalStates = new bool[components.Length];
					smi.previousStorageAllowItemRemovalStates[i] = components[i].allowItemRemoval;
					components[i].allowItemRemoval = false;
				}
				else if (smi.previousStorageAllowItemRemovalStates != null && i < smi.previousStorageAllowItemRemovalStates.Length)
				{
					components[i].allowItemRemoval = smi.previousStorageAllowItemRemovalStates[i];
				}
				foreach (GameObject go in components[i].items)
				{
					go.Trigger(-778359855, components[i]);
				}
			}
		}
		Ownable component = smi.master.GetComponent<Ownable>();
		if (restrict && component != null && component.IsAssigned())
		{
			component.Unassign();
		}
	}

	// Token: 0x06004D4F RID: 19791 RVA: 0x001BB8CC File Offset: 0x001B9ACC
	private void AquireRocketControlStation(RocketUsageRestriction.StatesInstance smi)
	{
		if (!this.rocketControlStation.IsNull(smi))
		{
			return;
		}
		foreach (object obj in Components.RocketControlStations)
		{
			RocketControlStation rocketControlStation = (RocketControlStation)obj;
			if (rocketControlStation.GetMyWorldId() == smi.GetMyWorldId())
			{
				this.rocketControlStation.Set(rocketControlStation, smi);
			}
		}
	}

	// Token: 0x0400335F RID: 13151
	public static readonly Operational.Flag rocketUsageAllowed = new Operational.Flag("rocketUsageAllowed", Operational.Flag.Type.Requirement);

	// Token: 0x04003360 RID: 13152
	private StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.TargetParameter rocketControlStation;

	// Token: 0x04003361 RID: 13153
	public RocketUsageRestriction.RestrictionStates restriction;

	// Token: 0x02001A7A RID: 6778
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600A011 RID: 40977 RVA: 0x0037E713 File Offset: 0x0037C913
		public override void Configure(GameObject prefab)
		{
			RocketControlStation.CONTROLLED_BUILDINGS.Add(prefab.PrefabID());
		}

		// Token: 0x04007C96 RID: 31894
		public bool initialControlledStateWhenBuilt = true;

		// Token: 0x04007C97 RID: 31895
		public bool restrictOperational = true;
	}

	// Token: 0x02001A7B RID: 6779
	public class ControlledStates : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State
	{
		// Token: 0x04007C98 RID: 31896
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State nostation;

		// Token: 0x04007C99 RID: 31897
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State controlled;
	}

	// Token: 0x02001A7C RID: 6780
	public class RestrictionStates : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State
	{
		// Token: 0x04007C9A RID: 31898
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State uncontrolled;

		// Token: 0x04007C9B RID: 31899
		public RocketUsageRestriction.ControlledStates controlled;
	}

	// Token: 0x02001A7D RID: 6781
	public class StatesInstance : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.GameInstance
	{
		// Token: 0x0600A015 RID: 40981 RVA: 0x0037E74B File Offset: 0x0037C94B
		public StatesInstance(IStateMachineTarget master, RocketUsageRestriction.Def def) : base(master, def)
		{
			this.isControlled = def.initialControlledStateWhenBuilt;
		}

		// Token: 0x0600A016 RID: 40982 RVA: 0x0037E768 File Offset: 0x0037C968
		public void OnRefreshUserMenu(object data)
		{
			KIconButtonMenu.ButtonInfo button;
			if (this.isControlled)
			{
				button = new KIconButtonMenu.ButtonInfo("action_rocket_restriction_uncontrolled", UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.NAME_UNCONTROLLED, new System.Action(this.OnChange), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.TOOLTIP_UNCONTROLLED, true);
			}
			else
			{
				button = new KIconButtonMenu.ButtonInfo("action_rocket_restriction_controlled", UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.NAME_CONTROLLED, new System.Action(this.OnChange), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.TOOLTIP_CONTROLLED, true);
			}
			Game.Instance.userMenu.AddButton(base.gameObject, button, 11f);
		}

		// Token: 0x0600A017 RID: 40983 RVA: 0x0037E804 File Offset: 0x0037CA04
		public void ControlStationBuilt(object o)
		{
			base.sm.AquireRocketControlStation(base.smi);
		}

		// Token: 0x0600A018 RID: 40984 RVA: 0x0037E817 File Offset: 0x0037CA17
		private void OnChange()
		{
			this.isControlled = !this.isControlled;
			this.GoToRestrictionState();
		}

		// Token: 0x0600A019 RID: 40985 RVA: 0x0037E830 File Offset: 0x0037CA30
		public void GoToRestrictionState()
		{
			if (base.smi.isControlled)
			{
				base.smi.GoTo(base.sm.restriction.controlled);
				return;
			}
			base.smi.GoTo(base.sm.restriction.uncontrolled);
		}

		// Token: 0x0600A01A RID: 40986 RVA: 0x0037E881 File Offset: 0x0037CA81
		public bool BuildingRestrictionsActive()
		{
			return this.isControlled && !base.sm.rocketControlStation.IsNull(base.smi) && base.sm.rocketControlStation.Get<RocketControlStation>(base.smi).BuildingRestrictionsActive;
		}

		// Token: 0x04007C9C RID: 31900
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04007C9D RID: 31901
		public bool[] previousStorageAllowItemRemovalStates;

		// Token: 0x04007C9E RID: 31902
		[Serialize]
		public bool isControlled = true;

		// Token: 0x04007C9F RID: 31903
		public bool isRestrictionApplied;
	}
}
