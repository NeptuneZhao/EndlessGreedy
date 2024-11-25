using System;
using UnityEngine;

// Token: 0x02000816 RID: 2070
[AddComponentMenu("KMonoBehaviour/scripts/NotCapturable")]
public class NotCapturable : KMonoBehaviour
{
	// Token: 0x0600393F RID: 14655 RVA: 0x00138364 File Offset: 0x00136564
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (base.GetComponent<Capturable>() != null)
		{
			DebugUtil.LogErrorArgs(this, new object[]
			{
				"Entity has both Capturable and NotCapturable!"
			});
		}
		Components.NotCapturables.Add(this);
	}

	// Token: 0x06003940 RID: 14656 RVA: 0x00138399 File Offset: 0x00136599
	protected override void OnCleanUp()
	{
		Components.NotCapturables.Remove(this);
		base.OnCleanUp();
	}
}
