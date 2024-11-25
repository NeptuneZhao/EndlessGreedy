using System;
using UnityEngine;

// Token: 0x0200068B RID: 1675
public abstract class IBuildingConfig
{
	// Token: 0x060029BB RID: 10683
	public abstract BuildingDef CreateBuildingDef();

	// Token: 0x060029BC RID: 10684 RVA: 0x000EB5E5 File Offset: 0x000E97E5
	public virtual void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x060029BD RID: 10685
	public abstract void DoPostConfigureComplete(GameObject go);

	// Token: 0x060029BE RID: 10686 RVA: 0x000EB5E7 File Offset: 0x000E97E7
	public virtual void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x000EB5E9 File Offset: 0x000E97E9
	public virtual void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x000EB5EB File Offset: 0x000E97EB
	public virtual void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x060029C1 RID: 10689 RVA: 0x000EB5ED File Offset: 0x000E97ED
	[Obsolete("Implement GetRequiredDlcIds and/or GetForbiddenDlcIds instead")]
	public virtual string[] GetDlcIds()
	{
		return null;
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x000EB5F0 File Offset: 0x000E97F0
	public virtual string[] GetRequiredDlcIds()
	{
		return null;
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x000EB5F3 File Offset: 0x000E97F3
	public virtual string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060029C4 RID: 10692 RVA: 0x000EB5F6 File Offset: 0x000E97F6
	public virtual bool ForbidFromLoading()
	{
		return false;
	}
}
