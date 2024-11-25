using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class RockCometConfig : IEntityConfig
{
	// Token: 0x06000E59 RID: 3673 RVA: 0x00054CB1 File Offset: 0x00052EB1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x00054CB8 File Offset: 0x00052EB8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RockCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ROCKCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		float mass = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues.mass;
		comet.massRange = new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f);
		comet.temperatureRange = new Vector2(323.15f, 423.15f);
		comet.addTiles = 6;
		comet.addTilesMinHeight = 2;
		comet.addTilesMaxHeight = 8;
		comet.entityDamage = 20;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_Large_Impact";
		comet.flyingSoundID = 2;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDirt;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_rock_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x00054E19 File Offset: 0x00053019
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x00054E1B File Offset: 0x0005301B
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040008FF RID: 2303
	public static readonly string ID = "RockComet";

	// Token: 0x04000900 RID: 2304
	private const SimHashes element = SimHashes.Regolith;

	// Token: 0x04000901 RID: 2305
	private const int ADDED_CELLS = 6;
}
