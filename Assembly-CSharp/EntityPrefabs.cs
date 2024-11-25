using System;
using UnityEngine;

// Token: 0x02000888 RID: 2184
[AddComponentMenu("KMonoBehaviour/scripts/EntityPrefabs")]
public class EntityPrefabs : KMonoBehaviour
{
	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x06003D42 RID: 15682 RVA: 0x00152CB5 File Offset: 0x00150EB5
	// (set) Token: 0x06003D43 RID: 15683 RVA: 0x00152CBC File Offset: 0x00150EBC
	public static EntityPrefabs Instance { get; private set; }

	// Token: 0x06003D44 RID: 15684 RVA: 0x00152CC4 File Offset: 0x00150EC4
	public static void DestroyInstance()
	{
		EntityPrefabs.Instance = null;
	}

	// Token: 0x06003D45 RID: 15685 RVA: 0x00152CCC File Offset: 0x00150ECC
	protected override void OnPrefabInit()
	{
		EntityPrefabs.Instance = this;
	}

	// Token: 0x0400255F RID: 9567
	public GameObject SelectMarker;

	// Token: 0x04002560 RID: 9568
	public GameObject ForegroundLayer;
}
