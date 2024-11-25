using System;
using UnityEngine;

// Token: 0x02000884 RID: 2180
public interface IEntityConfig
{
	// Token: 0x06003D32 RID: 15666
	GameObject CreatePrefab();

	// Token: 0x06003D33 RID: 15667
	void OnPrefabInit(GameObject inst);

	// Token: 0x06003D34 RID: 15668
	void OnSpawn(GameObject inst);

	// Token: 0x06003D35 RID: 15669
	string[] GetDlcIds();
}
