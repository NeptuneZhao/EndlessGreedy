using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020007AF RID: 1967
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreGroupManager")]
public class ChoreGroupManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060035C8 RID: 13768 RVA: 0x0012499F File Offset: 0x00122B9F
	public static void DestroyInstance()
	{
		ChoreGroupManager.instance = null;
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x060035C9 RID: 13769 RVA: 0x001249A7 File Offset: 0x00122BA7
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x060035CA RID: 13770 RVA: 0x001249AF File Offset: 0x00122BAF
	public Dictionary<Tag, int> DefaultChorePermission
	{
		get
		{
			return this.defaultChorePermissions;
		}
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x001249B8 File Offset: 0x00122BB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ChoreGroupManager.instance = this;
		this.ConvertOldVersion();
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			if (!this.defaultChorePermissions.ContainsKey(choreGroup.Id.ToTag()))
			{
				this.defaultChorePermissions.Add(choreGroup.Id.ToTag(), 2);
			}
		}
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x00124A50 File Offset: 0x00122C50
	private void ConvertOldVersion()
	{
		foreach (Tag key in this.defaultForbiddenTagsList)
		{
			if (!this.defaultChorePermissions.ContainsKey(key))
			{
				this.defaultChorePermissions.Add(key, -1);
			}
			this.defaultChorePermissions[key] = 0;
		}
		this.defaultForbiddenTagsList.Clear();
	}

	// Token: 0x0400200E RID: 8206
	public static ChoreGroupManager instance;

	// Token: 0x0400200F RID: 8207
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();

	// Token: 0x04002010 RID: 8208
	[Serialize]
	private Dictionary<Tag, int> defaultChorePermissions = new Dictionary<Tag, int>();
}
