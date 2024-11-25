using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000AB4 RID: 2740
public class AssignmentGroupController : KMonoBehaviour
{
	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x060050B9 RID: 20665 RVA: 0x001CFF3D File Offset: 0x001CE13D
	// (set) Token: 0x060050BA RID: 20666 RVA: 0x001CFF45 File Offset: 0x001CE145
	public string AssignmentGroupID
	{
		get
		{
			return this._assignmentGroupID;
		}
		private set
		{
			this._assignmentGroupID = value;
		}
	}

	// Token: 0x060050BB RID: 20667 RVA: 0x001CFF4E File Offset: 0x001CE14E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060050BC RID: 20668 RVA: 0x001CFF56 File Offset: 0x001CE156
	[OnDeserialized]
	protected void CreateOrRestoreGroupID()
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
			return;
		}
		Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], base.gameObject.GetProperName());
	}

	// Token: 0x060050BD RID: 20669 RVA: 0x001CFF93 File Offset: 0x001CE193
	public void SetGroupID(string id)
	{
		DebugUtil.DevAssert(!string.IsNullOrEmpty(id), "Trying to set Assignment group on " + base.gameObject.name + " to null or empty.", null);
		if (!string.IsNullOrEmpty(id))
		{
			this.AssignmentGroupID = id;
		}
	}

	// Token: 0x060050BE RID: 20670 RVA: 0x001CFFCD File Offset: 0x001CE1CD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreGroupAssignees();
	}

	// Token: 0x060050BF RID: 20671 RVA: 0x001CFFDC File Offset: 0x001CE1DC
	private void GenerateGroupID()
	{
		if (!this.generateGroupOnStart)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			return;
		}
		this.SetGroupID(base.GetComponent<KPrefabID>().PrefabID().ToString() + "_" + base.GetComponent<KPrefabID>().InstanceID.ToString() + "_assignmentGroup");
		Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], base.gameObject.GetProperName());
	}

	// Token: 0x060050C0 RID: 20672 RVA: 0x001D0068 File Offset: 0x001CE268
	private void RestoreGroupAssignees()
	{
		if (!this.generateGroupOnStart)
		{
			return;
		}
		this.CreateOrRestoreGroupID();
		if (this.minionsInGroupAtLoad == null)
		{
			this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[0];
		}
		for (int i = 0; i < this.minionsInGroupAtLoad.Length; i++)
		{
			Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, this.minionsInGroupAtLoad[i].Get());
		}
		Ownable component = base.GetComponent<Ownable>();
		if (component != null)
		{
			component.Assign(Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID]);
			component.SetCanBeAssigned(false);
		}
	}

	// Token: 0x060050C1 RID: 20673 RVA: 0x001D0104 File Offset: 0x001CE304
	public bool CheckMinionIsMember(MinionAssignablesProxy minion)
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
		}
		return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].HasMember(minion);
	}

	// Token: 0x060050C2 RID: 20674 RVA: 0x001D013C File Offset: 0x001CE33C
	public void SetMember(MinionAssignablesProxy minion, bool isAllowed)
	{
		Debug.Assert(DlcManager.IsExpansion1Active());
		if (!isAllowed)
		{
			Game.Instance.assignmentManager.RemoveFromAssignmentGroup(this.AssignmentGroupID, minion);
			return;
		}
		if (!this.CheckMinionIsMember(minion))
		{
			Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, minion);
		}
	}

	// Token: 0x060050C3 RID: 20675 RVA: 0x001D018C File Offset: 0x001CE38C
	protected override void OnCleanUp()
	{
		if (this.generateGroupOnStart)
		{
			Game.Instance.assignmentManager.RemoveAssignmentGroup(this.AssignmentGroupID);
		}
		base.OnCleanUp();
	}

	// Token: 0x060050C4 RID: 20676 RVA: 0x001D01B4 File Offset: 0x001CE3B4
	[OnSerializing]
	private void OnSerialize()
	{
		Debug.Assert(!string.IsNullOrEmpty(this.AssignmentGroupID), "Assignment group on " + base.gameObject.name + " has null or empty ID");
		ReadOnlyCollection<IAssignableIdentity> members = Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
		this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[members.Count];
		for (int i = 0; i < members.Count; i++)
		{
			this.minionsInGroupAtLoad[i] = new Ref<MinionAssignablesProxy>((MinionAssignablesProxy)members[i]);
		}
	}

	// Token: 0x060050C5 RID: 20677 RVA: 0x001D0249 File Offset: 0x001CE449
	public ReadOnlyCollection<IAssignableIdentity> GetMembers()
	{
		return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
	}

	// Token: 0x040035AE RID: 13742
	public bool generateGroupOnStart;

	// Token: 0x040035AF RID: 13743
	[Serialize]
	private string _assignmentGroupID;

	// Token: 0x040035B0 RID: 13744
	[Serialize]
	private Ref<MinionAssignablesProxy>[] minionsInGroupAtLoad;
}
