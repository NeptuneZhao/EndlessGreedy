using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000657 RID: 1623
[AddComponentMenu("KMonoBehaviour/scripts/AssignmentManager")]
public class AssignmentManager : KMonoBehaviour
{
	// Token: 0x060027EC RID: 10220 RVA: 0x000E2CD0 File Offset: 0x000E0ED0
	public IEnumerator<Assignable> GetEnumerator()
	{
		return this.assignables.GetEnumerator();
	}

	// Token: 0x060027ED RID: 10221 RVA: 0x000E2CE2 File Offset: 0x000E0EE2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe<AssignmentManager>(586301400, AssignmentManager.MinionMigrationDelegate);
	}

	// Token: 0x060027EE RID: 10222 RVA: 0x000E2D00 File Offset: 0x000E0F00
	protected void MinionMigration(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject() == minionMigrationEventArgs.minionId.gameObject)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x000E2DAC File Offset: 0x000E0FAC
	public void Add(Assignable assignable)
	{
		this.assignables.Add(assignable);
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x000E2DBA File Offset: 0x000E0FBA
	public void Remove(Assignable assignable)
	{
		this.assignables.Remove(assignable);
	}

	// Token: 0x060027F1 RID: 10225 RVA: 0x000E2DC9 File Offset: 0x000E0FC9
	public AssignmentGroup TryCreateAssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		if (this.assignment_groups.ContainsKey(id))
		{
			return this.assignment_groups[id];
		}
		return new AssignmentGroup(id, members, name);
	}

	// Token: 0x060027F2 RID: 10226 RVA: 0x000E2DEE File Offset: 0x000E0FEE
	public void RemoveAssignmentGroup(string id)
	{
		if (!this.assignment_groups.ContainsKey(id))
		{
			global::Debug.LogError("Assignment group with id " + id + " doesn't exists");
			return;
		}
		this.assignment_groups.Remove(id);
	}

	// Token: 0x060027F3 RID: 10227 RVA: 0x000E2E21 File Offset: 0x000E1021
	public void AddToAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].AddMember(member);
	}

	// Token: 0x060027F4 RID: 10228 RVA: 0x000E2E46 File Offset: 0x000E1046
	public void RemoveFromAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].RemoveMember(member);
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x000E2E6C File Offset: 0x000E106C
	public void RemoveFromAllGroups(IAssignableIdentity member)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee == member)
			{
				assignable.Unassign();
			}
		}
		foreach (KeyValuePair<string, AssignmentGroup> keyValuePair in this.assignment_groups)
		{
			if (keyValuePair.Value.HasMember(member))
			{
				keyValuePair.Value.RemoveMember(member);
			}
		}
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x000E2F20 File Offset: 0x000E1120
	public void RemoveFromWorld(IAssignableIdentity minionIdentity, int world_id)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null && assignable.assignee.GetOwners().Count == 1)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee == minionIdentity && assignable.GetMyWorldId() == world_id)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x060027F7 RID: 10231 RVA: 0x000E2FC4 File Offset: 0x000E11C4
	public List<Assignable> GetPreferredAssignables(Assignables owner, AssignableSlot slot)
	{
		List<Assignable> preferredAssignableResults = this.PreferredAssignableResults;
		List<Assignable> preferredAssignableResults2;
		lock (preferredAssignableResults)
		{
			this.PreferredAssignableResults.Clear();
			int num = int.MaxValue;
			foreach (Assignable assignable in this.assignables)
			{
				if (assignable.slot == slot && assignable.assignee != null && assignable.assignee.HasOwner(owner))
				{
					Room room = assignable.assignee as Room;
					if (room != null && room.roomType.priority_building_use)
					{
						this.PreferredAssignableResults.Clear();
						this.PreferredAssignableResults.Add(assignable);
						return this.PreferredAssignableResults;
					}
					int num2 = assignable.assignee.NumOwners();
					if (num2 == num)
					{
						this.PreferredAssignableResults.Add(assignable);
					}
					else if (num2 < num)
					{
						num = num2;
						this.PreferredAssignableResults.Clear();
						this.PreferredAssignableResults.Add(assignable);
					}
				}
			}
			preferredAssignableResults2 = this.PreferredAssignableResults;
		}
		return preferredAssignableResults2;
	}

	// Token: 0x060027F8 RID: 10232 RVA: 0x000E3124 File Offset: 0x000E1324
	public bool IsPreferredAssignable(Assignables owner, Assignable candidate)
	{
		IAssignableIdentity assignee = candidate.assignee;
		if (assignee == null || !assignee.HasOwner(owner))
		{
			return false;
		}
		int num = assignee.NumOwners();
		Room room = assignee as Room;
		if (room != null && room.roomType.priority_building_use)
		{
			return true;
		}
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.slot == candidate.slot && assignable.assignee != assignee)
			{
				Room room2 = assignable.assignee as Room;
				if (room2 != null && room2.roomType.priority_building_use && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
				if (assignable.assignee.NumOwners() < num && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x04001713 RID: 5907
	private List<Assignable> assignables = new List<Assignable>();

	// Token: 0x04001714 RID: 5908
	public const string PUBLIC_GROUP_ID = "public";

	// Token: 0x04001715 RID: 5909
	public Dictionary<string, AssignmentGroup> assignment_groups = new Dictionary<string, AssignmentGroup>
	{
		{
			"public",
			new AssignmentGroup("public", new IAssignableIdentity[0], UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.PUBLIC)
		}
	};

	// Token: 0x04001716 RID: 5910
	private static readonly EventSystem.IntraObjectHandler<AssignmentManager> MinionMigrationDelegate = new EventSystem.IntraObjectHandler<AssignmentManager>(delegate(AssignmentManager component, object data)
	{
		component.MinionMigration(data);
	});

	// Token: 0x04001717 RID: 5911
	private List<Assignable> PreferredAssignableResults = new List<Assignable>();
}
