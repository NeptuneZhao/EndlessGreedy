using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002B4 RID: 692
public class GoldCometConfig : IEntityConfig
{
	// Token: 0x06000E6B RID: 3691 RVA: 0x00055126 File Offset: 0x00053326
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x00055130 File Offset: 0x00053330
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(GoldCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.GOLDCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(3f, 20f);
		comet.temperatureRange = new Vector2(323.15f, 423.15f);
		comet.explosionOreCount = new Vector2I(2, 4);
		comet.entityDamage = 15;
		comet.totalTileDamage = 0.5f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_Medium_Impact";
		comet.flyingSoundID = 1;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactMetal;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.GoldAmalgam, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_gold_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x0005528A File Offset: 0x0005348A
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x0005528C File Offset: 0x0005348C
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000904 RID: 2308
	public static string ID = "GoldComet";
}
