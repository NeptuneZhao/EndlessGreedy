using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public class NuclearWasteCometConfig : IEntityConfig
{
	// Token: 0x06000E7D RID: 3709 RVA: 0x000554BC File Offset: 0x000536BC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x000554C4 File Offset: 0x000536C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(NuclearWasteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.NUCLEAR_WASTE.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(NuclearWasteCometConfig.MASS, NuclearWasteCometConfig.MASS);
		comet.EXHAUST_ELEMENT = SimHashes.Fallout;
		comet.EXHAUST_RATE = NuclearWasteCometConfig.MASS * 0.2f;
		comet.temperatureRange = new Vector2(473.15f, 573.15f);
		comet.entityDamage = 2;
		comet.totalTileDamage = 0.45f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_Nuclear_Impact";
		comet.flyingSoundID = 3;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		comet.addTiles = 1;
		comet.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id);
		comet.addDiseaseCount = 1000000;
		comet.affectedByDifficulty = false;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Corium, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("nuclear_metldown_comet_fx_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00055673 File Offset: 0x00053873
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x00055675 File Offset: 0x00053875
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000907 RID: 2311
	public static string ID = "NuclearWasteComet";

	// Token: 0x04000908 RID: 2312
	public static float MASS = 1f;
}
