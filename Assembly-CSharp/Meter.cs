using System;
using UnityEngine;

// Token: 0x0200095E RID: 2398
[AddComponentMenu("KMonoBehaviour/scripts/Meter")]
public class Meter : KMonoBehaviour
{
	// Token: 0x020018CB RID: 6347
	public enum Offset
	{
		// Token: 0x0400776A RID: 30570
		Infront,
		// Token: 0x0400776B RID: 30571
		Behind,
		// Token: 0x0400776C RID: 30572
		UserSpecified,
		// Token: 0x0400776D RID: 30573
		NoChange
	}
}
