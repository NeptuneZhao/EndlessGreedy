using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class EscapePodConfig : IEntityConfig
{
	// Token: 0x060002A9 RID: 681 RVA: 0x0001335A File Offset: 0x0001155A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00013364 File Offset: 0x00011564
	public GameObject CreatePrefab()
	{
		string id = "EscapePod";
		string name = STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("escape_pod_kanim"), "grounded", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.RoomProberBuilding
		}, 293f);
		gameObject.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.BuildingFront;
		TravellingCargoLander.Def def = gameObject.AddOrGetDef<TravellingCargoLander.Def>();
		def.landerWidth = 1;
		def.landingSpeed = 15f;
		def.deployOnLanding = true;
		CargoDropperMinion.Def def2 = gameObject.AddOrGetDef<CargoDropperMinion.Def>();
		def2.kAnimName = "anim_interacts_escape_pod_kanim";
		def2.animName = "deploying";
		def2.animLayer = Grid.SceneLayer.BuildingUse;
		def2.notifyOnJettison = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "escape_pod01_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.NAME");
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		gameObject.AddOrGet<ClusterTraveler>();
		gameObject.AddOrGet<MinionStorage>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		return gameObject;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x000134A3 File Offset: 0x000116A3
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060002AC RID: 684 RVA: 0x000134C1 File Offset: 0x000116C1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400018C RID: 396
	public const string ID = "EscapePod";

	// Token: 0x0400018D RID: 397
	public const float MASS = 100f;
}
