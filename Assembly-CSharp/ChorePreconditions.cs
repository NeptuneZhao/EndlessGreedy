using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class ChorePreconditions
{
	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060016E0 RID: 5856 RVA: 0x0007A918 File Offset: 0x00078B18
	public static ChorePreconditions instance
	{
		get
		{
			if (ChorePreconditions._instance == null)
			{
				ChorePreconditions._instance = new ChorePreconditions();
			}
			return ChorePreconditions._instance;
		}
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x0007A930 File Offset: 0x00078B30
	public static void DestroyInstance()
	{
		ChorePreconditions._instance = null;
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x0007A938 File Offset: 0x00078B38
	public ChorePreconditions()
	{
		Chore.Precondition precondition = default(Chore.Precondition);
		precondition.id = "IsPreemptable";
		precondition.sortOrder = 1;
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_PREEMPTABLE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.isAttemptingOverride || context.chore.CanPreempt(context) || context.chore.driver == null;
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsPreemptable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "HasUrge";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_URGE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.chore.choreType.urge == null)
			{
				return true;
			}
			foreach (Urge urge in context.consumerState.consumer.GetUrges())
			{
				if (context.chore.SatisfiesUrge(urge))
				{
					return true;
				}
			}
			return false;
		};
		precondition.canExecuteOnAnyThread = true;
		this.HasUrge = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsValid";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_VALID;
		precondition.sortOrder = -4;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !context.chore.isNull && context.chore.IsValid();
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsValid = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsPermitted";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_PERMITTED;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.consumerState.consumer.IsPermittedOrEnabled(context.choreTypeForPermission, context.chore);
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsPermitted = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsAssignedToMe";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_ASSIGNED_TO_ME;
		precondition.sortOrder = 10;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Assignable assignable = (Assignable)data;
			IAssignableIdentity component = context.consumerState.gameObject.GetComponent<IAssignableIdentity>();
			return component != null && assignable.IsAssignedTo(component);
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsAssignedtoMe = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsInMyRoom";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_IN_MY_ROOM;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			int cell = (int)data;
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			Room room = null;
			if (cavityForCell != null)
			{
				room = cavityForCell.room;
			}
			if (room != null)
			{
				if (context.consumerState.ownable != null)
				{
					using (List<Ownables>.Enumerator enumerator = room.GetOwners().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.gameObject == context.consumerState.gameObject)
							{
								return true;
							}
						}
						return false;
					}
				}
				Room room2 = null;
				FetchChore fetchChore = context.chore as FetchChore;
				if (fetchChore != null && fetchChore.destination != null)
				{
					CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(fetchChore.destination));
					if (cavityForCell2 != null)
					{
						room2 = cavityForCell2.room;
					}
					return room2 != null && room2 == room;
				}
				if (context.chore is WorkChore<Tinkerable>)
				{
					CavityInfo cavityForCell3 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((context.chore as WorkChore<Tinkerable>).gameObject));
					if (cavityForCell3 != null)
					{
						room2 = cavityForCell3.room;
					}
					return room2 != null && room2 == room;
				}
				return false;
			}
			return false;
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsInMyRoom = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsPreferredAssignable";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_PREFERRED_ASSIGNABLE;
		precondition.sortOrder = 10;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Assignable assignable = (Assignable)data;
			return Game.Instance.assignmentManager.GetPreferredAssignables(context.consumerState.assignables, assignable.slot).Contains(assignable);
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsPreferredAssignable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsPreferredAssignableOrUrgent";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_PREFERRED_ASSIGNABLE_OR_URGENT_BLADDER;
		precondition.sortOrder = 10;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Assignable candidate = (Assignable)data;
			if (Game.Instance.assignmentManager.IsPreferredAssignable(context.consumerState.assignables, candidate))
			{
				return true;
			}
			PeeChoreMonitor.Instance smi = context.consumerState.gameObject.GetSMI<PeeChoreMonitor.Instance>();
			if (smi != null)
			{
				return smi.IsInsideState(smi.sm.critical);
			}
			GunkMonitor.Instance smi2 = context.consumerState.gameObject.GetSMI<GunkMonitor.Instance>();
			return smi2 != null && smi2.IsInsideState(smi2.sm.criticalUrge);
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsPreferredAssignableOrUrgentBladder = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsNotTransferArm";
		precondition.description = "";
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !context.consumerState.hasSolidTransferArm;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsNotTransferArm = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "HasSkillPerk";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_SKILL_PERK;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			MinionResume resume = context.consumerState.resume;
			if (!resume)
			{
				return false;
			}
			if (data is SkillPerk)
			{
				SkillPerk perk = data as SkillPerk;
				return resume.HasPerk(perk);
			}
			if (data is HashedString)
			{
				HashedString perkId = (HashedString)data;
				return resume.HasPerk(perkId);
			}
			if (data is string)
			{
				HashedString perkId2 = (string)data;
				return resume.HasPerk(perkId2);
			}
			return false;
		};
		precondition.canExecuteOnAnyThread = true;
		this.HasSkillPerk = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsMinion";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MINION;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.consumerState.resume != null;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsMinion = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsMoreSatisfyingEarly";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MORE_SATISFYING;
		precondition.sortOrder = -2;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.isAttemptingOverride)
			{
				return true;
			}
			if (context.skipMoreSatisfyingEarlyPrecondition)
			{
				return true;
			}
			if (context.consumerState.selectable.IsSelected)
			{
				return true;
			}
			Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
			if (currentChore == null)
			{
				return true;
			}
			if (context.masterPriority.priority_class != currentChore.masterPriority.priority_class)
			{
				return context.masterPriority.priority_class > currentChore.masterPriority.priority_class;
			}
			if (context.consumerState.consumer != null && context.personalPriority != context.consumerState.consumer.GetPersonalPriority(currentChore.choreType))
			{
				return context.personalPriority > context.consumerState.consumer.GetPersonalPriority(currentChore.choreType);
			}
			if (context.masterPriority.priority_value != currentChore.masterPriority.priority_value)
			{
				return context.masterPriority.priority_value > currentChore.masterPriority.priority_value;
			}
			return context.priority > currentChore.choreType.priority;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsMoreSatisfyingEarly = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsMoreSatisfyingLate";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MORE_SATISFYING;
		precondition.sortOrder = 10000;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.isAttemptingOverride)
			{
				return true;
			}
			if (!context.consumerState.selectable.IsSelected)
			{
				return true;
			}
			Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
			if (currentChore == null)
			{
				return true;
			}
			if (context.masterPriority.priority_class != currentChore.masterPriority.priority_class)
			{
				return context.masterPriority.priority_class > currentChore.masterPriority.priority_class;
			}
			if (context.consumerState.consumer != null && context.personalPriority != context.consumerState.consumer.GetPersonalPriority(currentChore.choreType))
			{
				return context.personalPriority > context.consumerState.consumer.GetPersonalPriority(currentChore.choreType);
			}
			if (context.masterPriority.priority_value != currentChore.masterPriority.priority_value)
			{
				return context.masterPriority.priority_value > currentChore.masterPriority.priority_value;
			}
			return context.priority > currentChore.choreType.priority;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsMoreSatisfyingLate = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanChat";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_CHAT;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && !(kmonoBehaviour == null) && context.consumerState.navigator.CanReach(Grid.PosToCell(kmonoBehaviour));
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsChattable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsNotRedAlert";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_RED_ALERT;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority || !context.chore.gameObject.GetMyWorld().IsRedAlert();
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsNotRedAlert = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsScheduledTime";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_SCHEDULED_TIME;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.chore.gameObject.GetMyWorld().IsRedAlert())
			{
				return true;
			}
			ScheduleBlockType type = (ScheduleBlockType)data;
			ScheduleBlock scheduleBlock = context.consumerState.scheduleBlock;
			return scheduleBlock == null || scheduleBlock.IsAllowed(type);
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsScheduledTime = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanMoveTo";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			if (kmonoBehaviour == null)
			{
				return false;
			}
			IApproachable approachable = (IApproachable)kmonoBehaviour;
			int num;
			if (context.consumerState.consumer.GetNavigationCost(approachable, out num))
			{
				context.cost += num;
				return true;
			}
			return false;
		};
		precondition.canExecuteOnAnyThread = false;
		this.CanMoveTo = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanMoveToCell";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			int cell = (int)data;
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			int num;
			if (context.consumerState.consumer.GetNavigationCost(cell, out num))
			{
				context.cost += num;
				return true;
			}
			return false;
		};
		precondition.canExecuteOnAnyThread = true;
		this.CanMoveToCell = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanMoveToDynamicCell";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			Func<int> func = (Func<int>)data;
			if (func == null)
			{
				return false;
			}
			int cell = func();
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			int num;
			if (context.consumerState.consumer.GetNavigationCost(cell, out num))
			{
				context.cost += num;
				return true;
			}
			return false;
		};
		precondition.canExecuteOnAnyThread = false;
		this.CanMoveToDynamicCell = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanMoveToDynamicCellUntilBegun";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			if (context.chore.InProgress())
			{
				return true;
			}
			Func<int> func = (Func<int>)data;
			if (func == null)
			{
				return false;
			}
			int cell = func();
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			int num;
			if (context.consumerState.consumer.GetNavigationCost(cell, out num))
			{
				context.cost += num;
				return true;
			}
			return false;
		};
		precondition.canExecuteOnAnyThread = false;
		this.CanMoveToDynamicCellUntilBegun = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanPickup";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_PICKUP;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Pickupable pickupable = (Pickupable)data;
			return !(pickupable == null) && !(context.consumerState.consumer == null) && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && pickupable.CouldBePickedUpByMinion(context.consumerState.gameObject) && context.consumerState.consumer.CanReach(pickupable);
		};
		precondition.canExecuteOnAnyThread = false;
		this.CanPickup = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsAwake";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_AWAKE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			StaminaMonitor.Instance smi = context.consumerState.consumer.GetSMI<StaminaMonitor.Instance>();
			return smi == null || !smi.IsInsideState(smi.sm.sleepy.sleeping);
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsAwake = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsStanding";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_STANDING;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && context.consumerState.navigator.CurrentNavType == NavType.Floor;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsStanding = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsMoving";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MOVING;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && context.consumerState.navigator.IsMoving();
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsMoving = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsOffLadder";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OFF_LADDER;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && context.consumerState.navigator.CurrentNavType != NavType.Ladder && context.consumerState.navigator.CurrentNavType != NavType.Pole;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsOffLadder = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NotInTube";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NOT_IN_TUBE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && context.consumerState.navigator.CurrentNavType != NavType.Tube;
		};
		precondition.canExecuteOnAnyThread = true;
		this.NotInTube = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "ConsumerHasTrait";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_TRAIT;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			string trait_id = (string)data;
			Traits traits = context.consumerState.traits;
			return !(traits == null) && traits.HasTrait(trait_id);
		};
		precondition.canExecuteOnAnyThread = true;
		this.ConsumerHasTrait = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsOperational";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OPERATIONAL;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return (data as Operational).IsOperational;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsOperational = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsNotMarkedForDeconstruction";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Deconstructable deconstructable = data as Deconstructable;
			return deconstructable == null || !deconstructable.IsMarkedForDeconstruction();
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsNotMarkedForDeconstruction = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsNotMarkedForDisable";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DISABLE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			BuildingEnabledButton buildingEnabledButton = data as BuildingEnabledButton;
			return buildingEnabledButton == null || (buildingEnabledButton.IsEnabled && !buildingEnabledButton.WaitingForDisable);
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsNotMarkedForDisable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsFunctional";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_FUNCTIONAL;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return (data as Operational).IsFunctional;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsFunctional = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsOverrideTargetNullOrMe";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OVERRIDE_TARGET_NULL_OR_ME;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.isAttemptingOverride || context.chore.overrideTarget == null || context.chore.overrideTarget == context.consumerState.consumer;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsOverrideTargetNullOrMe = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NotChoreCreator";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NOT_CHORE_CREATOR;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			GameObject y = (GameObject)data;
			return !(context.consumerState.consumer == null) && !(context.consumerState.gameObject == y);
		};
		precondition.canExecuteOnAnyThread = false;
		this.NotChoreCreator = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsGettingMoreStressed";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_GETTING_MORE_STRESSED;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).GetDelta() > 0f;
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsGettingMoreStressed = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsAllowedByAutomation";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_ALLOWED_BY_AUTOMATION;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((Automatable)data).AllowedByAutomation(context.consumerState.hasSolidTransferArm);
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsAllowedByAutomation = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "HasTag";
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Tag tag = (Tag)data;
			return context.consumerState.prefabid.HasTag(tag);
		};
		precondition.canExecuteOnAnyThread = true;
		this.HasTag = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CheckBehaviourPrecondition";
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Tag tag = (Tag)data;
			return context.consumerState.consumer.RunBehaviourPrecondition(tag);
		};
		precondition.canExecuteOnAnyThread = false;
		this.CheckBehaviourPrecondition = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanDoWorkerPrioritizable";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_DO_RECREATION;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			IWorkerPrioritizable workerPrioritizable = data as IWorkerPrioritizable;
			if (workerPrioritizable == null)
			{
				return false;
			}
			int num = 0;
			if (workerPrioritizable.GetWorkerPriority(context.consumerState.worker, out num))
			{
				context.consumerPriority += num;
				return true;
			}
			return false;
		};
		precondition.canExecuteOnAnyThread = false;
		this.CanDoWorkerPrioritizable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsExclusivelyAvailableWithOtherChores";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.EXCLUSIVELY_AVAILABLE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			foreach (Chore chore in ((List<Chore>)data))
			{
				if (chore != context.chore && chore.driver != null)
				{
					return false;
				}
			}
			return true;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsExclusivelyAvailableWithOtherChores = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsBladderFull";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.BLADDER_FULL;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			BladderMonitor.Instance smi = context.consumerState.gameObject.GetSMI<BladderMonitor.Instance>();
			return smi != null && smi.NeedsToPee();
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsBladderFull = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsBladderNotFull";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.BLADDER_NOT_FULL;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			BladderMonitor.Instance smi = context.consumerState.gameObject.GetSMI<BladderMonitor.Instance>();
			return smi == null || !smi.NeedsToPee();
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsBladderNotFull = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NoDeadBodies";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NO_DEAD_BODIES;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return Components.LiveMinionIdentities.Count == Components.MinionIdentities.Count;
		};
		precondition.canExecuteOnAnyThread = true;
		this.NoDeadBodies = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NoRobots";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NOT_A_ROBOT;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object exempt_robot)
		{
			Tag b = exempt_robot as string;
			return context.consumerState.resume != null || context.consumerState.prefabid.PrefabTag == b;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsNotARobot = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NoBionic";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NOT_A_BIONIC;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.consumerState.prefabid.PrefabTag != BionicMinionConfig.ID;
		};
		precondition.canExecuteOnAnyThread = true;
		this.IsNotABionic = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NotCurrentlyPeeing";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CURRENTLY_PEEING;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			bool result = true;
			Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
			if (currentChore != null)
			{
				string id = currentChore.choreType.Id;
				result = (id != Db.Get().ChoreTypes.BreakPee.Id && id != Db.Get().ChoreTypes.Pee.Id);
			}
			return result;
		};
		precondition.canExecuteOnAnyThread = true;
		this.NotCurrentlyPeeing = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsRocketTravelling";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_ROCKET_TRAVELLING;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Clustercraft component = ClusterManager.Instance.GetWorld(context.chore.gameObject.GetMyWorldId()).GetComponent<Clustercraft>();
			return !(component == null) && component.IsTravellingAndFueled();
		};
		precondition.canExecuteOnAnyThread = false;
		this.IsRocketTravelling = precondition;
		base..ctor();
	}

	// Token: 0x04000CC4 RID: 3268
	private static ChorePreconditions _instance;

	// Token: 0x04000CC5 RID: 3269
	public Chore.Precondition IsPreemptable;

	// Token: 0x04000CC6 RID: 3270
	public Chore.Precondition HasUrge;

	// Token: 0x04000CC7 RID: 3271
	public Chore.Precondition IsValid;

	// Token: 0x04000CC8 RID: 3272
	public Chore.Precondition IsPermitted;

	// Token: 0x04000CC9 RID: 3273
	public Chore.Precondition IsAssignedtoMe;

	// Token: 0x04000CCA RID: 3274
	public Chore.Precondition IsInMyRoom;

	// Token: 0x04000CCB RID: 3275
	public Chore.Precondition IsPreferredAssignable;

	// Token: 0x04000CCC RID: 3276
	public Chore.Precondition IsPreferredAssignableOrUrgentBladder;

	// Token: 0x04000CCD RID: 3277
	public Chore.Precondition IsNotTransferArm;

	// Token: 0x04000CCE RID: 3278
	public Chore.Precondition HasSkillPerk;

	// Token: 0x04000CCF RID: 3279
	public Chore.Precondition IsMinion;

	// Token: 0x04000CD0 RID: 3280
	public Chore.Precondition IsMoreSatisfyingEarly;

	// Token: 0x04000CD1 RID: 3281
	public Chore.Precondition IsMoreSatisfyingLate;

	// Token: 0x04000CD2 RID: 3282
	public Chore.Precondition IsChattable;

	// Token: 0x04000CD3 RID: 3283
	public Chore.Precondition IsNotRedAlert;

	// Token: 0x04000CD4 RID: 3284
	public Chore.Precondition IsScheduledTime;

	// Token: 0x04000CD5 RID: 3285
	public Chore.Precondition CanMoveTo;

	// Token: 0x04000CD6 RID: 3286
	public Chore.Precondition CanMoveToCell;

	// Token: 0x04000CD7 RID: 3287
	public Chore.Precondition CanMoveToDynamicCell;

	// Token: 0x04000CD8 RID: 3288
	public Chore.Precondition CanMoveToDynamicCellUntilBegun;

	// Token: 0x04000CD9 RID: 3289
	public Chore.Precondition CanPickup;

	// Token: 0x04000CDA RID: 3290
	public Chore.Precondition IsAwake;

	// Token: 0x04000CDB RID: 3291
	public Chore.Precondition IsStanding;

	// Token: 0x04000CDC RID: 3292
	public Chore.Precondition IsMoving;

	// Token: 0x04000CDD RID: 3293
	public Chore.Precondition IsOffLadder;

	// Token: 0x04000CDE RID: 3294
	public Chore.Precondition NotInTube;

	// Token: 0x04000CDF RID: 3295
	public Chore.Precondition ConsumerHasTrait;

	// Token: 0x04000CE0 RID: 3296
	public Chore.Precondition IsOperational;

	// Token: 0x04000CE1 RID: 3297
	public Chore.Precondition IsNotMarkedForDeconstruction;

	// Token: 0x04000CE2 RID: 3298
	public Chore.Precondition IsNotMarkedForDisable;

	// Token: 0x04000CE3 RID: 3299
	public Chore.Precondition IsFunctional;

	// Token: 0x04000CE4 RID: 3300
	public Chore.Precondition IsOverrideTargetNullOrMe;

	// Token: 0x04000CE5 RID: 3301
	public Chore.Precondition NotChoreCreator;

	// Token: 0x04000CE6 RID: 3302
	public Chore.Precondition IsGettingMoreStressed;

	// Token: 0x04000CE7 RID: 3303
	public Chore.Precondition IsAllowedByAutomation;

	// Token: 0x04000CE8 RID: 3304
	public Chore.Precondition HasTag;

	// Token: 0x04000CE9 RID: 3305
	public Chore.Precondition CheckBehaviourPrecondition;

	// Token: 0x04000CEA RID: 3306
	public Chore.Precondition CanDoWorkerPrioritizable;

	// Token: 0x04000CEB RID: 3307
	public Chore.Precondition IsExclusivelyAvailableWithOtherChores;

	// Token: 0x04000CEC RID: 3308
	public Chore.Precondition IsBladderFull;

	// Token: 0x04000CED RID: 3309
	public Chore.Precondition IsBladderNotFull;

	// Token: 0x04000CEE RID: 3310
	public Chore.Precondition NoDeadBodies;

	// Token: 0x04000CEF RID: 3311
	public Chore.Precondition IsNotARobot;

	// Token: 0x04000CF0 RID: 3312
	public Chore.Precondition IsNotABionic;

	// Token: 0x04000CF1 RID: 3313
	public Chore.Precondition NotCurrentlyPeeing;

	// Token: 0x04000CF2 RID: 3314
	public Chore.Precondition IsRocketTravelling;
}
