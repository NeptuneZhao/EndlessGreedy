using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002BE RID: 702
public class SpaceTreeSeedCometConfig : IEntityConfig
{
	// Token: 0x06000EA7 RID: 3751 RVA: 0x00055EDD File Offset: 0x000540DD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x00055EE4 File Offset: 0x000540E4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SpaceTreeSeedCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SPACETREESEEDCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		SpaceTreeSeededComet spaceTreeSeededComet = gameObject.AddOrGet<SpaceTreeSeededComet>();
		spaceTreeSeededComet.massRange = new Vector2(50f, 100f);
		spaceTreeSeededComet.temperatureRange = new Vector2(253.15f, 263.15f);
		spaceTreeSeededComet.explosionTemperatureRange = spaceTreeSeededComet.temperatureRange;
		spaceTreeSeededComet.impactSound = "Meteor_copper_Impact";
		spaceTreeSeededComet.flyingSoundID = 5;
		spaceTreeSeededComet.EXHAUST_ELEMENT = SimHashes.Void;
		spaceTreeSeededComet.explosionEffectHash = SpawnFXHashes.None;
		spaceTreeSeededComet.entityDamage = 0;
		spaceTreeSeededComet.totalTileDamage = 0f;
		spaceTreeSeededComet.splashRadius = 1;
		spaceTreeSeededComet.addTiles = 3;
		spaceTreeSeededComet.addTilesMinHeight = 1;
		spaceTreeSeededComet.addTilesMaxHeight = 2;
		spaceTreeSeededComet.lootOnDestroyedByMissile = new string[]
		{
			"SpaceTreeSeed"
		};
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Snow, true);
		primaryElement.Temperature = (spaceTreeSeededComet.temperatureRange.x + spaceTreeSeededComet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_bonbon_snow_kanim")
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

	// Token: 0x06000EA9 RID: 3753 RVA: 0x0005606C File Offset: 0x0005426C
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0005606E File Offset: 0x0005426E
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000913 RID: 2323
	public static string ID = "SpaceTreeSeedComet";
}
