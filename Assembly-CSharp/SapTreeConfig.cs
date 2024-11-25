using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class SapTreeConfig : IEntityConfig
{
	// Token: 0x0600077C RID: 1916 RVA: 0x00031C79 File Offset: 0x0002FE79
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00031C80 File Offset: 0x0002FE80
	public GameObject CreatePrefab()
	{
		string id = "SapTree";
		string name = STRINGS.CREATURES.SPECIES.SAPTREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SAPTREE.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = SapTreeConfig.POSITIVE_DECOR_EFFECT;
		KAnimFile anim = Assets.GetAnim("gravitas_sap_tree_kanim");
		string initialAnim = "idle";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 5;
		int height = 5;
		EffectorValues decor = positive_DECOR_EFFECT;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Decoration
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 293f);
		SapTree.Def def = gameObject.AddOrGetDef<SapTree.Def>();
		def.foodSenseArea = new Vector2I(5, 1);
		def.massEatRate = 0.05f;
		def.kcalorieToKGConversionRatio = 0.005f;
		def.stomachSize = 5f;
		def.oozeRate = 2f;
		def.oozeOffsets = new List<Vector3>
		{
			new Vector3(-2f, 2f),
			new Vector3(2f, 1f)
		};
		def.attackSenseArea = new Vector2I(5, 5);
		def.attackCooldown = 5f;
		gameObject.AddOrGet<Storage>();
		FactionAlignment factionAlignment = gameObject.AddOrGet<FactionAlignment>();
		factionAlignment.Alignment = FactionManager.FactionID.Hostile;
		factionAlignment.canBePlayerTargeted = false;
		gameObject.AddOrGet<RangedAttackable>();
		gameObject.AddWeapon(5f, 5f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.AreaOfEffect, 1, 2f);
		gameObject.AddOrGet<WiltCondition>();
		gameObject.AddOrGet<TemperatureVulnerable>().Configure(173.15f, 0f, 373.15f, 1023.15f);
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00031E04 File Offset: 0x00030004
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00031E06 File Offset: 0x00030006
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000551 RID: 1361
	public const string ID = "SapTree";

	// Token: 0x04000552 RID: 1362
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER5;

	// Token: 0x04000553 RID: 1363
	private const int WIDTH = 5;

	// Token: 0x04000554 RID: 1364
	private const int HEIGHT = 5;

	// Token: 0x04000555 RID: 1365
	private const int ATTACK_RADIUS = 2;

	// Token: 0x04000556 RID: 1366
	public const float MASS_EAT_RATE = 0.05f;

	// Token: 0x04000557 RID: 1367
	public const float KCAL_TO_KG_RATIO = 0.005f;
}
