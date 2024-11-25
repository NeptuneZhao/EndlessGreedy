using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000885 RID: 2181
public interface IMultiEntityConfig
{
	// Token: 0x06003D36 RID: 15670
	List<GameObject> CreatePrefabs();

	// Token: 0x06003D37 RID: 15671
	void OnPrefabInit(GameObject inst);

	// Token: 0x06003D38 RID: 15672
	void OnSpawn(GameObject inst);
}
