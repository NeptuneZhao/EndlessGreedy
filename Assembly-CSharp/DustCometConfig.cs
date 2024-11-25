using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002B6 RID: 694
public class DustCometConfig : IEntityConfig
{
	// Token: 0x06000E77 RID: 3703 RVA: 0x0005534E File Offset: 0x0005354E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x00055358 File Offset: 0x00053558
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(DustCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.DUSTCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(0.2f, 0.5f);
		comet.temperatureRange = new Vector2(223.15f, 253.15f);
		comet.entityDamage = 2;
		comet.totalTileDamage = 0.15f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_dust_heavy_Impact";
		comet.flyingSoundID = 0;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_sand_kanim")
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

	// Token: 0x06000E79 RID: 3705 RVA: 0x000554A4 File Offset: 0x000536A4
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x000554A6 File Offset: 0x000536A6
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000906 RID: 2310
	public static string ID = "DustComet";
}
