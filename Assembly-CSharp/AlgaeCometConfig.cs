using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class AlgaeCometConfig : IEntityConfig
{
	// Token: 0x06000EB9 RID: 3769 RVA: 0x000563CD File Offset: 0x000545CD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x000563D4 File Offset: 0x000545D4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(AlgaeCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ALGAECOMET.NAME, "meteor_algae_kanim", SimHashes.Algae, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_algae_Impact", 7, SimHashes.Void, SpawnFXHashes.MeteorImpactAlgae, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0005646B File Offset: 0x0005466B
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0005646D File Offset: 0x0005466D
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000918 RID: 2328
	public static string ID = "AlgaeComet";
}
