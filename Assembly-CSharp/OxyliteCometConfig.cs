using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public class OxyliteCometConfig : IEntityConfig
{
	// Token: 0x06000EC5 RID: 3781 RVA: 0x0005653B File Offset: 0x0005473B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x00056544 File Offset: 0x00054744
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(OxyliteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.OXYLITECOMET.NAME, "meteor_oxylite_kanim", SimHashes.OxyRock, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Oxygen, SpawnFXHashes.MeteorImpactIce, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 2;
		component.addTilesMaxHeight = 8;
		return gameObject;
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x000565F3 File Offset: 0x000547F3
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x000565F5 File Offset: 0x000547F5
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400091A RID: 2330
	public static string ID = "OxyliteComet";

	// Token: 0x0400091B RID: 2331
	private const int ADDED_CELLS = 6;
}
