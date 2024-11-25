using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
public class WarpPortalConfig : IEntityConfig
{
	// Token: 0x0600155D RID: 5469 RVA: 0x000755C4 File Offset: 0x000737C4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x000755CC File Offset: 0x000737CC
	public GameObject CreatePrefab()
	{
		string id = "WarpPortal";
		string name = STRINGS.BUILDINGS.PREFABS.WARPPORTAL.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.WARPPORTAL.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("warp_portal_sender_kanim"), "idle", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		gameObject.AddTag(GameTags.WarpTech);
		gameObject.AddTag(GameTags.Gravitas);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<WarpPortal>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Ownable>().tintWhenUnassigned = false;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("notes_teleportation", UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_SENDER));
		gameObject.AddOrGet<Prioritizable>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		return gameObject;
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x000756C8 File Offset: 0x000738C8
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<WarpPortal>().workLayer = Grid.SceneLayer.Building;
		inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.WarpPortal.Id;
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		inst.GetComponent<Deconstructable>();
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x0007571D File Offset: 0x0007391D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000C1B RID: 3099
	public const string ID = "WarpPortal";
}
