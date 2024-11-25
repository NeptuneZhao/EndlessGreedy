using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002C4 RID: 708
public class BleachStoneCometConfig : IEntityConfig
{
	// Token: 0x06000ECB RID: 3787 RVA: 0x0005660B File Offset: 0x0005480B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x00056614 File Offset: 0x00054814
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(BleachStoneCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.BLEACHSTONECOMET.NAME, "meteor_bleachstone_kanim", SimHashes.BleachStone, new Vector2(mass * 0.8f * 1f, mass * 1.2f * 1f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 1, SimHashes.ChlorineGas, SpawnFXHashes.MeteorImpactIce, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.addTiles = 1;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x000566E5 File Offset: 0x000548E5
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x000566E7 File Offset: 0x000548E7
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400091C RID: 2332
	public static string ID = "BleachStoneComet";

	// Token: 0x0400091D RID: 2333
	private const int ADDED_CELLS = 1;
}
