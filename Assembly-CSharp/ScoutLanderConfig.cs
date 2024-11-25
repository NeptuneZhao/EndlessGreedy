using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200039C RID: 924
public class ScoutLanderConfig : IEntityConfig
{
	// Token: 0x0600133A RID: 4922 RVA: 0x0006A9C9 File Offset: 0x00068BC9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x0006A9D0 File Offset: 0x00068BD0
	public GameObject CreatePrefab()
	{
		string id = "ScoutLander";
		string name = STRINGS.BUILDINGS.PREFABS.SCOUTLANDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.SCOUTLANDER.DESC;
		float mass = 400f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_scout_cargo_lander_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.RoomProberBuilding
		}, 293f);
		gameObject.AddOrGetDef<CargoLander.Def>().previewTag = "ScoutLander_Preview".ToTag();
		gameObject.AddOrGetDef<CargoDropperStorage.Def>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showInUI = true;
		storage.allowItemRemoval = false;
		storage.capacityKg = 2000f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		gameObject.AddOrGet<Storable>();
		Placeable placeable = gameObject.AddOrGet<Placeable>();
		placeable.kAnimName = "rocket_scout_cargo_lander_kanim";
		placeable.animName = "place";
		placeable.placementRules = new List<Placeable.PlacementRules>
		{
			Placeable.PlacementRules.OnFoundation,
			Placeable.PlacementRules.VisibleToSpace,
			Placeable.PlacementRules.RestrictToWorld
		};
		placeable.checkRootCellOnly = true;
		EntityTemplates.CreateAndRegisterPreview("ScoutLander_Preview", Assets.GetAnim("rocket_scout_cargo_lander_kanim"), "place", ObjectLayer.Building, 3, 3);
		return gameObject;
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x0006AB14 File Offset: 0x00068D14
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x0006AB32 File Offset: 0x00068D32
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000B32 RID: 2866
	public const string ID = "ScoutLander";

	// Token: 0x04000B33 RID: 2867
	public const string PREVIEW_ID = "ScoutLander_Preview";

	// Token: 0x04000B34 RID: 2868
	public const float MASS = 400f;
}
