using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000224 RID: 548
public class MachinePartsConfig : IEntityConfig
{
	// Token: 0x06000B54 RID: 2900 RVA: 0x00043161 File Offset: 0x00041361
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00043168 File Offset: 0x00041368
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("MachineParts", ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.DESC, 5f, true, Assets.GetAnim("buildingrelocate_kanim"), "idle", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, null);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x000431C2 File Offset: 0x000413C2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x000431C4 File Offset: 0x000413C4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000786 RID: 1926
	public const string ID = "MachineParts";

	// Token: 0x04000787 RID: 1927
	public static readonly Tag TAG = TagManager.Create("MachineParts");

	// Token: 0x04000788 RID: 1928
	public const float MASS = 5f;
}
