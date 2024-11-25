using System;
using UnityEngine;

// Token: 0x02000D9C RID: 3484
public class RoleStationSideScreen : SideScreenContent
{
	// Token: 0x06006DE9 RID: 28137 RVA: 0x002952B9 File Offset: 0x002934B9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006DEA RID: 28138 RVA: 0x002952C1 File Offset: 0x002934C1
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x04004B00 RID: 19200
	public GameObject content;

	// Token: 0x04004B01 RID: 19201
	private GameObject target;

	// Token: 0x04004B02 RID: 19202
	public LocText DescriptionText;
}
