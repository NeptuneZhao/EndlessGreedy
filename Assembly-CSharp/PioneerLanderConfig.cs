using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class PioneerLanderConfig : IEntityConfig
{
	// Token: 0x060010F5 RID: 4341 RVA: 0x0005F9BB File Offset: 0x0005DBBB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x0005F9C4 File Offset: 0x0005DBC4
	public GameObject CreatePrefab()
	{
		string id = "PioneerLander";
		string name = STRINGS.BUILDINGS.PREFABS.PIONEERLANDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PIONEERLANDER.DESC;
		float mass = 400f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_pioneer_cargo_lander_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.RoomProberBuilding
		}, 293f);
		gameObject.AddOrGetDef<CargoLander.Def>().previewTag = "PioneerLander_Preview".ToTag();
		CargoDropperMinion.Def def = gameObject.AddOrGetDef<CargoDropperMinion.Def>();
		def.kAnimName = "anim_interacts_pioneer_cargo_lander_kanim";
		def.animName = "enter";
		gameObject.AddOrGet<MinionStorage>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		gameObject.AddOrGet<Storable>();
		Placeable placeable = gameObject.AddOrGet<Placeable>();
		placeable.kAnimName = "rocket_pioneer_cargo_lander_kanim";
		placeable.animName = "place";
		placeable.placementRules = new List<Placeable.PlacementRules>
		{
			Placeable.PlacementRules.OnFoundation,
			Placeable.PlacementRules.VisibleToSpace,
			Placeable.PlacementRules.RestrictToWorld
		};
		placeable.checkRootCellOnly = true;
		EntityTemplates.CreateAndRegisterPreview("PioneerLander_Preview", Assets.GetAnim("rocket_pioneer_cargo_lander_kanim"), "place", ObjectLayer.Building, 3, 3);
		return gameObject;
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x0005FAFA File Offset: 0x0005DCFA
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x0005FB18 File Offset: 0x0005DD18
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A64 RID: 2660
	public const string ID = "PioneerLander";

	// Token: 0x04000A65 RID: 2661
	public const string PREVIEW_ID = "PioneerLander_Preview";

	// Token: 0x04000A66 RID: 2662
	public const float MASS = 400f;
}
