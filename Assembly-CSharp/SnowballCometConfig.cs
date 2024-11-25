using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002BD RID: 701
public class SnowballCometConfig : IEntityConfig
{
	// Token: 0x06000EA1 RID: 3745 RVA: 0x00055E26 File Offset: 0x00054026
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY.Append("DLC2_ID");
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x00055E38 File Offset: 0x00054038
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(SnowballCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SNOWBALLCOMET.NAME, "meteor_snow_kanim", SimHashes.Snow, new Vector2(3f, 20f), new Vector2(253.15f, 263.15f), "Meteor_snowball_Impact", 5, SimHashes.Void, SpawnFXHashes.None, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.splashRadius = 1;
		component.addTiles = 3;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		return gameObject;
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x00055EC5 File Offset: 0x000540C5
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x00055EC7 File Offset: 0x000540C7
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000912 RID: 2322
	public static string ID = "SnowballComet";
}
