using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002C2 RID: 706
public class PhosphoricCometConfig : IEntityConfig
{
	// Token: 0x06000EBF RID: 3775 RVA: 0x00056483 File Offset: 0x00054683
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x0005648C File Offset: 0x0005468C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(PhosphoricCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.PHOSPHORICCOMET.NAME, "meteor_phosphoric_kanim", SimHashes.Phosphorite, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Void, SpawnFXHashes.MeteorImpactPhosphoric, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x00056523 File Offset: 0x00054723
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x00056525 File Offset: 0x00054725
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000919 RID: 2329
	public static string ID = "PhosphoricComet";
}
