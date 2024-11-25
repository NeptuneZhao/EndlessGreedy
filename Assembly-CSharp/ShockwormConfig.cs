﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class ShockwormConfig : IEntityConfig
{
	// Token: 0x06000635 RID: 1589 RVA: 0x0002AB52 File Offset: 0x00028D52
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0002AB5C File Offset: 0x00028D5C
	public GameObject CreatePrefab()
	{
		string id = "ShockWorm";
		string name = STRINGS.CREATURES.SPECIES.SHOCKWORM.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SHOCKWORM.DESC;
		float mass = 50f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("shockworm_kanim"), "idle", Grid.SceneLayer.Creatures, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		FactionManager.FactionID faction = FactionManager.FactionID.Hostile;
		string initialTraitID = null;
		string navGridName = "FlyerNavGrid1x2";
		NavType navType = NavType.Hover;
		int max_probing_radius = 32;
		float moveSpeed = 2f;
		string onDeathDropID = "Meat";
		int onDeathDropCount = 3;
		bool drownVulnerable = true;
		bool entombVulnerable = true;
		float freezing_ = TUNING.CREATURES.TEMPERATURE.FREEZING_2;
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, faction, initialTraitID, navGridName, navType, max_probing_radius, moveSpeed, onDeathDropID, onDeathDropCount, drownVulnerable, entombVulnerable, TUNING.CREATURES.TEMPERATURE.FREEZING_1, TUNING.CREATURES.TEMPERATURE.HOT_1, freezing_, TUNING.CREATURES.TEMPERATURE.HOT_2);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddWeapon(3f, 6f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.AreaOfEffect, 10, 4f).AddEffect("WasAttacked", 1f);
		SoundEventVolumeCache.instance.AddVolume("shockworm_kanim", "Shockworm_attack_arc", NOISE_POLLUTION.CREATURES.TIER6);
		return gameObject;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0002AC3B File Offset: 0x00028E3B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0002AC3D File Offset: 0x00028E3D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400045D RID: 1117
	public const string ID = "ShockWorm";
}
