using System;
using UnityEngine;

// Token: 0x02000CEA RID: 3306
public class MotdData_Box
{
	// Token: 0x0600666C RID: 26220 RVA: 0x002642DC File Offset: 0x002624DC
	public bool ShouldDisplay()
	{
		long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		return num >= this.startTime && this.finishTime >= num;
	}

	// Token: 0x0400451A RID: 17690
	public string category;

	// Token: 0x0400451B RID: 17691
	public string guid;

	// Token: 0x0400451C RID: 17692
	public long startTime;

	// Token: 0x0400451D RID: 17693
	public long finishTime;

	// Token: 0x0400451E RID: 17694
	public string title;

	// Token: 0x0400451F RID: 17695
	public string text;

	// Token: 0x04004520 RID: 17696
	public string image;

	// Token: 0x04004521 RID: 17697
	public string href;

	// Token: 0x04004522 RID: 17698
	public Texture2D resolvedImage;

	// Token: 0x04004523 RID: 17699
	public bool resolvedImageIsFromDisk;
}
