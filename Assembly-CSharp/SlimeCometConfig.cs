using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002BC RID: 700
public class SlimeCometConfig : IEntityConfig
{
	// Token: 0x06000E9B RID: 3739 RVA: 0x00055CB1 File Offset: 0x00053EB1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x00055CB8 File Offset: 0x00053EB8
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.SlimeMold).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(SlimeCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SLIMECOMET.NAME, "meteor_slime_kanim", SimHashes.SlimeMold, new Vector2(mass * 0.8f * 2f, mass * 1.2f * 2f), new Vector2(310.15f, 323.15f), "Meteor_slimeball_Impact", 7, SimHashes.ContaminatedOxygen, SpawnFXHashes.MeteorImpactSlime, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.addTiles = 2;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		component.diseaseIdx = Db.Get().Diseases.GetIndex("SlimeLung");
		component.addDiseaseCount = (int)(component.EXHAUST_RATE * 100000f);
		return gameObject;
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00055DBB File Offset: 0x00053FBB
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x00055DC0 File Offset: 0x00053FC0
	public void OnSpawn(GameObject go)
	{
		go.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex("SlimeLung"), (int)(UnityEngine.Random.Range(0.9f, 1.2f) * 50f * 100000f), "Meteor");
	}

	// Token: 0x0400090F RID: 2319
	public static string ID = "SlimeComet";

	// Token: 0x04000910 RID: 2320
	public const int ADDED_CELLS = 2;

	// Token: 0x04000911 RID: 2321
	private const SimHashes element = SimHashes.SlimeMold;
}
