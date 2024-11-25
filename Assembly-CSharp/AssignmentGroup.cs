using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x0200056E RID: 1390
public class AssignmentGroup : IAssignableIdentity
{
	// Token: 0x17000151 RID: 337
	// (get) Token: 0x0600202C RID: 8236 RVA: 0x000B4BC6 File Offset: 0x000B2DC6
	// (set) Token: 0x0600202D RID: 8237 RVA: 0x000B4BCE File Offset: 0x000B2DCE
	public string id { get; private set; }

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x0600202E RID: 8238 RVA: 0x000B4BD7 File Offset: 0x000B2DD7
	// (set) Token: 0x0600202F RID: 8239 RVA: 0x000B4BDF File Offset: 0x000B2DDF
	public string name { get; private set; }

	// Token: 0x06002030 RID: 8240 RVA: 0x000B4BE8 File Offset: 0x000B2DE8
	public AssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		this.id = id;
		this.name = name;
		foreach (IAssignableIdentity item in members)
		{
			this.members.Add(item);
		}
		if (Game.Instance != null)
		{
			Game.Instance.assignmentManager.assignment_groups.Add(id, this);
			Game.Instance.Trigger(-1123234494, this);
		}
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000B4C72 File Offset: 0x000B2E72
	public void AddMember(IAssignableIdentity member)
	{
		if (!this.members.Contains(member))
		{
			this.members.Add(member);
		}
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000B4C9E File Offset: 0x000B2E9E
	public void RemoveMember(IAssignableIdentity member)
	{
		this.members.Remove(member);
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000B4CBD File Offset: 0x000B2EBD
	public string GetProperName()
	{
		return this.name;
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x000B4CC5 File Offset: 0x000B2EC5
	public bool HasMember(IAssignableIdentity member)
	{
		return this.members.Contains(member);
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x000B4CD3 File Offset: 0x000B2ED3
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x000B4CD6 File Offset: 0x000B2ED6
	public ReadOnlyCollection<IAssignableIdentity> GetMembers()
	{
		return this.members.AsReadOnly();
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x000B4CE4 File Offset: 0x000B2EE4
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			this.current_owners.AddRange(assignableIdentity.GetOwners());
		}
		return this.current_owners;
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000B4D54 File Offset: 0x000B2F54
	public Ownables GetSoleOwner()
	{
		if (this.members.Count == 1)
		{
			return this.members[0] as Ownables;
		}
		Debug.LogWarningFormat("GetSoleOwner called on AssignmentGroup with {0} members", new object[]
		{
			this.members.Count
		});
		return null;
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x000B4DA8 File Offset: 0x000B2FA8
	public bool HasOwner(Assignables owner)
	{
		using (List<IAssignableIdentity>.Enumerator enumerator = this.members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasOwner(owner))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x000B4E04 File Offset: 0x000B3004
	public int NumOwners()
	{
		int num = 0;
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			num += assignableIdentity.NumOwners();
		}
		return num;
	}

	// Token: 0x0400122F RID: 4655
	private List<IAssignableIdentity> members = new List<IAssignableIdentity>();

	// Token: 0x04001230 RID: 4656
	public List<Ownables> current_owners = new List<Ownables>();
}
