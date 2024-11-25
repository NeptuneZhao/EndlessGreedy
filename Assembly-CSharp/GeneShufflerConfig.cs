using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F6 RID: 502
public class GeneShufflerConfig : IEntityConfig
{
	// Token: 0x06000A41 RID: 2625 RVA: 0x0003C46E File Offset: 0x0003A66E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x0003C478 File Offset: 0x0003A678
	public GameObject CreatePrefab()
	{
		string id = "GeneShuffler";
		string name = STRINGS.BUILDINGS.PREFABS.GENESHUFFLER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GENESHUFFLER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geneshuffler_kanim"), "on", Grid.SceneLayer.Building, 4, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<GeneShuffler>();
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.NerualVacillator));
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Ownable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Demolishable>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.dropOnLoad = true;
		ManualDeliveryKG manualDeliveryKG = gameObject.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = new Tag("GeneShufflerRecharge");
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.capacity = 1f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		return gameObject;
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0003C5CC File Offset: 0x0003A7CC
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<GeneShuffler>().workLayer = Grid.SceneLayer.Building;
		inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.GeneShuffler.Id;
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		inst.GetComponent<Deconstructable>();
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0003C621 File Offset: 0x0003A821
	public void OnSpawn(GameObject inst)
	{
	}
}
