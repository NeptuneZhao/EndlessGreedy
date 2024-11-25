using System;
using UnityEngine;

// Token: 0x02000DA3 RID: 3491
public abstract class SideScreenContent : KScreen
{
	// Token: 0x06006E39 RID: 28217 RVA: 0x002978E6 File Offset: 0x00295AE6
	public virtual void SetTarget(GameObject target)
	{
	}

	// Token: 0x06006E3A RID: 28218 RVA: 0x002978E8 File Offset: 0x00295AE8
	public virtual void ClearTarget()
	{
	}

	// Token: 0x06006E3B RID: 28219
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x06006E3C RID: 28220 RVA: 0x002978EA File Offset: 0x00295AEA
	public virtual int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x06006E3D RID: 28221 RVA: 0x002978ED File Offset: 0x00295AED
	public virtual string GetTitle()
	{
		return Strings.Get(this.titleKey);
	}

	// Token: 0x04004B3F RID: 19263
	[SerializeField]
	protected string titleKey;

	// Token: 0x04004B40 RID: 19264
	public GameObject ContentContainer;
}
