using System;
using UnityEngine;

// Token: 0x02000C2C RID: 3116
public class DebugElementMenu : KButtonMenu
{
	// Token: 0x06005F94 RID: 24468 RVA: 0x002380EF File Offset: 0x002362EF
	protected override void OnPrefabInit()
	{
		DebugElementMenu.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06005F95 RID: 24469 RVA: 0x00238104 File Offset: 0x00236304
	protected override void OnForcedCleanUp()
	{
		DebugElementMenu.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06005F96 RID: 24470 RVA: 0x00238112 File Offset: 0x00236312
	public void Turnoff()
	{
		this.root.gameObject.SetActive(false);
	}

	// Token: 0x0400405A RID: 16474
	public static DebugElementMenu Instance;

	// Token: 0x0400405B RID: 16475
	public GameObject root;
}
