using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002C0 RID: 704
public class LightDustCometConfig : IEntityConfig
{
	// Token: 0x06000EB3 RID: 3763 RVA: 0x00056227 File Offset: 0x00054427
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x00056230 File Offset: 0x00054430
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(LightDustCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.LIGHTDUSTCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(10f, 14f);
		comet.temperatureRange = new Vector2(223.15f, 253.15f);
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.explosionOreCount = new Vector2I(1, 2);
		comet.explosionSpeedRange = new Vector2(4f, 7f);
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_dust_light_Impact";
		comet.flyingSoundID = 0;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactLightDust;
		comet.EXHAUST_ELEMENT = SimHashes.Void;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_dust_kanim")
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

	// Token: 0x06000EB5 RID: 3765 RVA: 0x000563B5 File Offset: 0x000545B5
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x000563B7 File Offset: 0x000545B7
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000917 RID: 2327
	public static string ID = "LightDustComet";
}
