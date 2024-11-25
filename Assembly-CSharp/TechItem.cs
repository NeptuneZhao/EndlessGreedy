using System;
using UnityEngine;

// Token: 0x02000A54 RID: 2644
public class TechItem : Resource
{
	// Token: 0x06004CB8 RID: 19640 RVA: 0x001B6790 File Offset: 0x001B4990
	[Obsolete("Use constructor with requiredDlcIds and forbiddenDlcIds")]
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] dlcIds, bool isPOIUnlock = false) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.isPOIUnlock = isPOIUnlock;
		DlcManager.ConvertAvailableToRequireAndForbidden(dlcIds, out this.requiredDlcIds, out this.forbiddenDlcIds);
	}

	// Token: 0x06004CB9 RID: 19641 RVA: 0x001B67CE File Offset: 0x001B49CE
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, bool isPOIUnlock = false) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.isPOIUnlock = isPOIUnlock;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06004CBA RID: 19642 RVA: 0x001B6809 File Offset: 0x001B4A09
	public Tech ParentTech
	{
		get
		{
			return Db.Get().Techs.Get(this.parentTechId);
		}
	}

	// Token: 0x06004CBB RID: 19643 RVA: 0x001B6820 File Offset: 0x001B4A20
	public Sprite UISprite()
	{
		return this.getUISprite("ui", false);
	}

	// Token: 0x06004CBC RID: 19644 RVA: 0x001B6833 File Offset: 0x001B4A33
	public bool IsComplete()
	{
		return this.ParentTech.IsComplete() || this.IsPOIUnlocked();
	}

	// Token: 0x06004CBD RID: 19645 RVA: 0x001B684C File Offset: 0x001B4A4C
	private bool IsPOIUnlocked()
	{
		if (this.isPOIUnlock)
		{
			TechInstance techInstance = Research.Instance.Get(this.ParentTech);
			if (techInstance != null)
			{
				return techInstance.UnlockedPOITechIds.Contains(this.Id);
			}
		}
		return false;
	}

	// Token: 0x06004CBE RID: 19646 RVA: 0x001B6888 File Offset: 0x001B4A88
	public void POIUnlocked()
	{
		DebugUtil.DevAssert(this.isPOIUnlock, "Trying to unlock tech item " + this.Id + " via POI and it's not marked as POI unlockable.", null);
		if (this.isPOIUnlock && !this.IsComplete())
		{
			Research.Instance.Get(this.ParentTech).UnlockPOITech(this.Id);
		}
	}

	// Token: 0x04003302 RID: 13058
	public string description;

	// Token: 0x04003303 RID: 13059
	public Func<string, bool, Sprite> getUISprite;

	// Token: 0x04003304 RID: 13060
	public string parentTechId;

	// Token: 0x04003305 RID: 13061
	public bool isPOIUnlock;

	// Token: 0x04003306 RID: 13062
	[Obsolete("Use required/forbidden instead")]
	public string[] dlcIds;

	// Token: 0x04003307 RID: 13063
	public string[] requiredDlcIds;

	// Token: 0x04003308 RID: 13064
	public string[] forbiddenDlcIds;
}
