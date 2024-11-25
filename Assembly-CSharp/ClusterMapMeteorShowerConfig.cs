using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000ABC RID: 2748
public class ClusterMapMeteorShowerConfig : IMultiEntityConfig
{
	// Token: 0x06005101 RID: 20737 RVA: 0x001D145A File Offset: 0x001CF65A
	public static string GetFullID(string id)
	{
		return "ClusterMapMeteorShower_" + id;
	}

	// Token: 0x06005102 RID: 20738 RVA: 0x001D1467 File Offset: 0x001CF667
	public static string GetReverseFullID(string fullID)
	{
		return fullID.Replace("ClusterMapMeteorShower_", "");
	}

	// Token: 0x06005103 RID: 20739 RVA: 0x001D147C File Offset: 0x001CF67C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Biological", "ClusterBiologicalShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.SLIME.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.SLIME.DESCRIPTION, "shower_cluster_biological_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Snow", "ClusterSnowShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.SNOW.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.SNOW.DESCRIPTION, "shower_cluster_snow_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Ice", "ClusterIceShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.ICE.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.ICE.DESCRIPTION, "shower_cluster_ice_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Copper", "ClusterCopperShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.COPPER.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.COPPER.DESCRIPTION, "shower_cluster_copper_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Iron", "ClusterIronShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.IRON.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.IRON.DESCRIPTION, "shower_cluster_iron_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Gold", "ClusterGoldShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.GOLD.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.GOLD.DESCRIPTION, "shower_cluster_gold_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Uranium", "ClusterUraniumShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.URANIUM.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.URANIUM.DESCRIPTION, "shower_cluster_uranium_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("HeavyDust", "ClusterRegolithShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.HEAVYDUST.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.HEAVYDUST.DESCRIPTION, "shower_cluster_regolith_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("LightDust", "ClusterLightRegolithShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.LIGHTDUST.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.LIGHTDUST.DESCRIPTION, "shower_cluster_light_regolith_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Moo", "GassyMooteorEvent", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.MOO.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.MOO.DESCRIPTION, "shower_mooteor_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Regolith", "MeteorShowerDustEvent", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.REGOLITH.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.REGOLITH.DESCRIPTION, "shower_cluster_regolith_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Oxylite", "ClusterOxyliteShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.OXYLITE.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.OXYLITE.DESCRIPTION, "shower_cluster_oxylite_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("BleachStone", "ClusterBleachStoneShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BLEACHSTONE.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BLEACHSTONE.DESCRIPTION, "shower_cluster_biological_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_EXPANSION1_ONLY, SimHashes.Unobtanium));
		if (DlcManager.IsExpansion1Active())
		{
			list.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("IceAndTrees", "ClusterIceAndTreesShower", UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.ICEANDTREES.NAME, UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.ICEANDTREES.DESCRIPTION, "shower_cluster_ice_kanim", "idle_loop", "ui", DlcManager.AVAILABLE_DLC_2, SimHashes.Unobtanium));
		}
		list.RemoveAll((GameObject x) => x == null);
		return list;
	}

	// Token: 0x06005104 RID: 20740 RVA: 0x001D185C File Offset: 0x001CFA5C
	public static GameObject CreateClusterMeteor(string id, string meteorEventID, string name, string desc, string animFile, string initial_anim = "idle_loop", string ui_anim = "ui", string[] dlcIDs = null, SimHashes element = SimHashes.Unobtanium)
	{
		if (dlcIDs == null)
		{
			dlcIDs = DlcManager.AVAILABLE_EXPANSION1_ONLY;
		}
		if (!DlcManager.IsDlcListValidForCurrentContent(dlcIDs))
		{
			return null;
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity(ClusterMapMeteorShowerConfig.GetFullID(id), name, desc, 25f, true, Assets.GetAnim(animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, false, SORTORDER.KEEPSAKES, element, new List<Tag>());
		gameObject.AddOrGet<KSelectable>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.requireLaunchPadOnAsteroidDestination = false;
		clusterDestinationSelector.dodgesHiddenAsteroids = true;
		ClusterMapMeteorShowerVisualizer clusterMapMeteorShowerVisualizer = gameObject.AddOrGet<ClusterMapMeteorShowerVisualizer>();
		clusterMapMeteorShowerVisualizer.p_name = name;
		clusterMapMeteorShowerVisualizer.clusterAnimName = animFile;
		ClusterTraveler clusterTraveler = gameObject.AddOrGet<ClusterTraveler>();
		clusterTraveler.revealsFogOfWarAsItTravels = false;
		clusterTraveler.quickTravelToAsteroidIfInOrbit = false;
		ClusterMapMeteorShower.Def def = gameObject.AddOrGetDef<ClusterMapMeteorShower.Def>();
		def.name = name;
		def.description = desc;
		def.name_Hidden = UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.UNIDENTIFIED.NAME;
		def.description_Hidden = UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.UNIDENTIFIED.DESCRIPTION;
		def.eventID = meteorEventID;
		return gameObject;
	}

	// Token: 0x06005105 RID: 20741 RVA: 0x001D195D File Offset: 0x001CFB5D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06005106 RID: 20742 RVA: 0x001D195F File Offset: 0x001CFB5F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040035D7 RID: 13783
	public const string IDENTIFY_AUDIO_NAME = "ClusterMapMeteor_Reveal";

	// Token: 0x040035D8 RID: 13784
	public const string ID_SIGNATURE = "ClusterMapMeteorShower_";
}
