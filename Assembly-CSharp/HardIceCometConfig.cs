using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002BF RID: 703
public class HardIceCometConfig : IEntityConfig
{
	// Token: 0x06000EAD RID: 3757 RVA: 0x00056084 File Offset: 0x00054284
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY.Append("DLC2_ID");
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00056098 File Offset: 0x00054298
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(HardIceCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.HARDICECOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		float mass = ElementLoader.FindElementByHash(SimHashes.CrushedIce).defaultValues.mass;
		comet.massRange = new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f);
		comet.temperatureRange = new Vector2(173.15f, 248.15f);
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.addTiles = 6;
		comet.addTilesMinHeight = 2;
		comet.addTilesMaxHeight = 8;
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_ice_Impact";
		comet.flyingSoundID = 6;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactIce;
		comet.EXHAUST_ELEMENT = SimHashes.Oxygen;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.CrushedIce, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_ice_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x0005620F File Offset: 0x0005440F
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00056211 File Offset: 0x00054411
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000914 RID: 2324
	public static readonly string ID = "HardIceComet";

	// Token: 0x04000915 RID: 2325
	private const SimHashes element = SimHashes.CrushedIce;

	// Token: 0x04000916 RID: 2326
	private const int ADDED_CELLS = 6;
}
