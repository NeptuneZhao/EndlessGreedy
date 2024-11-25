using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class DigPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06001107 RID: 4359 RVA: 0x000600CC File Offset: 0x0005E2CC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(DigPlacerConfig.ID, MISC.PLACERS.DIGPLACER.NAME, Assets.instance.digPlacerAssets.materials[0]);
		Diggable diggable = gameObject.AddOrGet<Diggable>();
		diggable.workTime = 5f;
		diggable.synchronizeAnims = false;
		diggable.workAnims = new HashedString[]
		{
			"place",
			"release"
		};
		diggable.materials = Assets.instance.digPlacerAssets.materials;
		diggable.materialDisplay = gameObject.GetComponentInChildren<MeshRenderer>(true);
		gameObject.AddOrGet<CancellableDig>();
		return gameObject;
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x00060171 File Offset: 0x0005E371
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x00060173 File Offset: 0x0005E373
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A69 RID: 2665
	public static string ID = "DigPlacer";

	// Token: 0x02001134 RID: 4404
	[Serializable]
	public class DigPlacerAssets
	{
		// Token: 0x04005F84 RID: 24452
		public Material[] materials;
	}
}
