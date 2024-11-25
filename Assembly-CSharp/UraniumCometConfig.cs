using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class UraniumCometConfig : IEntityConfig
{
	// Token: 0x06000E95 RID: 3733 RVA: 0x00055BD2 File Offset: 0x00053DD2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x00055BDC File Offset: 0x00053DDC
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.UraniumOre).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(UraniumCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.URANIUMORECOMET.NAME, "meteor_uranium_kanim", SimHashes.UraniumOre, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(323.15f, 403.15f), "Meteor_Nuclear_Impact", 3, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactUranium, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.entityDamage = 15;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x00055C99 File Offset: 0x00053E99
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x00055C9B File Offset: 0x00053E9B
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400090C RID: 2316
	public static readonly string ID = "UraniumComet";

	// Token: 0x0400090D RID: 2317
	private const SimHashes element = SimHashes.UraniumOre;

	// Token: 0x0400090E RID: 2318
	private const int ADDED_CELLS = 6;
}
