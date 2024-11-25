using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003F7 RID: 1015
public class WarpReceiverConfig : IEntityConfig
{
	// Token: 0x06001562 RID: 5474 RVA: 0x00075727 File Offset: 0x00073927
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x00075730 File Offset: 0x00073930
	public GameObject CreatePrefab()
	{
		string id = WarpReceiverConfig.ID;
		string name = STRINGS.BUILDINGS.PREFABS.WARPRECEIVER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.WARPRECEIVER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("warp_portal_receiver_kanim"), "idle", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		gameObject.AddTag(GameTags.WarpTech);
		gameObject.AddTag(GameTags.Gravitas);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<WarpReceiver>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Prioritizable>();
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("notes_AI", UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_RECEIVER));
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		return gameObject;
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x00075820 File Offset: 0x00073A20
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<WarpReceiver>().workLayer = Grid.SceneLayer.Building;
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		inst.GetComponent<Deconstructable>();
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x0007584B File Offset: 0x00073A4B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000C1C RID: 3100
	public static string ID = "WarpReceiver";
}
