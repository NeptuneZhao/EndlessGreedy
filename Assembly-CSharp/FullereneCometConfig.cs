using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002B5 RID: 693
public class FullereneCometConfig : IEntityConfig
{
	// Token: 0x06000E71 RID: 3697 RVA: 0x000552A2 File Offset: 0x000534A2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x000552AC File Offset: 0x000534AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(FullereneCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.FULLERENECOMET.NAME, "meteor_fullerene_kanim", SimHashes.Fullerene, new Vector2(3f, 20f), new Vector2(323.15f, 423.15f), "Meteor_Medium_Impact", 1, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactMetal, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.entityDamage = 15;
		component.totalTileDamage = 0.5f;
		component.affectedByDifficulty = false;
		return gameObject;
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x00055336 File Offset: 0x00053536
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x00055338 File Offset: 0x00053538
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000905 RID: 2309
	public static readonly string ID = "FullereneComet";
}
