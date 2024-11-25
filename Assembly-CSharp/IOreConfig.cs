using System;
using UnityEngine;

// Token: 0x02000315 RID: 789
public interface IOreConfig
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600108F RID: 4239
	SimHashes ElementID { get; }

	// Token: 0x06001090 RID: 4240
	GameObject CreatePrefab();
}
