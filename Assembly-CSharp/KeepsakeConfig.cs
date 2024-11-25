using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002D5 RID: 725
public class KeepsakeConfig : IMultiEntityConfig
{
	// Token: 0x06000F25 RID: 3877 RVA: 0x0005810C File Offset: 0x0005630C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(KeepsakeConfig.CreateKeepsake("MegaBrain", UI.KEEPSAKES.MEGA_BRAIN.NAME, UI.KEEPSAKES.MEGA_BRAIN.DESCRIPTION, "keepsake_mega_brain_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("CritterManipulator", UI.KEEPSAKES.CRITTER_MANIPULATOR.NAME, UI.KEEPSAKES.CRITTER_MANIPULATOR.DESCRIPTION, "keepsake_critter_manipulator_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("LonelyMinion", UI.KEEPSAKES.LONELY_MINION.NAME, UI.KEEPSAKES.LONELY_MINION.DESCRIPTION, "keepsake_lonelyminion_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("FossilHunt", UI.KEEPSAKES.FOSSIL_HUNT.NAME, UI.KEEPSAKES.FOSSIL_HUNT.DESCRIPTION, "keepsake_fossil_dig_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("GeothermalPlant", UI.KEEPSAKES.GEOTHERMAL_PLANT.NAME, UI.KEEPSAKES.GEOTHERMAL_PLANT.DESCRIPTION, "keepsake_geothermal_vent_kanim", "idle", "ui", DlcManager.AVAILABLE_DLC_2, null, SimHashes.Creature));
		GameObject gameObject = KeepsakeConfig.CreateKeepsake("MorbRoverMaker", UI.KEEPSAKES.MORB_ROVER_MAKER.NAME, UI.KEEPSAKES.MORB_ROVER_MAKER.DESCRIPTION, "keepsake_morb_tank_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature);
		gameObject.AddOrGetDef<MorbRoverMakerKeepsake.Def>();
		list.Add(gameObject);
		list.RemoveAll((GameObject x) => x == null);
		return list;
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x000582C4 File Offset: 0x000564C4
	public static GameObject CreateKeepsake(string id, string name, string desc, string animFile, string initial_anim = "idle", string ui_anim = "ui", string[] dlcIDs = null, KeepsakeConfig.PostInitFn postInitFn = null, SimHashes element = SimHashes.Creature)
	{
		if (dlcIDs == null)
		{
			dlcIDs = DlcManager.AVAILABLE_ALL_VERSIONS;
		}
		if (!DlcManager.IsDlcListValidForCurrentContent(dlcIDs))
		{
			return null;
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity("keepsake_" + id.ToLower(), name, desc, 25f, true, Assets.GetAnim(animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, SORTORDER.KEEPSAKES, element, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.BONUS.TIER1);
		decorProvider.overrideName = gameObject.GetProperName();
		gameObject.AddOrGet<KSelectable>();
		gameObject.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
		if (postInitFn != null)
		{
			postInitFn(gameObject);
		}
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.PedestalDisplayable, false);
		component.AddTag(GameTags.Keepsake, false);
		return gameObject;
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x000583A2 File Offset: 0x000565A2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x000583A4 File Offset: 0x000565A4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x02001115 RID: 4373
	// (Invoke) Token: 0x06007E62 RID: 32354
	public delegate void PostInitFn(GameObject gameObject);
}
