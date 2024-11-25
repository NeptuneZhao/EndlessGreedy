using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class RotPileConfig : IEntityConfig
{
	// Token: 0x06000918 RID: 2328 RVA: 0x00038055 File Offset: 0x00036255
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0003805C File Offset: 0x0003625C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(RotPileConfig.ID, STRINGS.ITEMS.FOOD.ROTPILE.NAME, STRINGS.ITEMS.FOOD.ROTPILE.DESC, 1f, false, Assets.GetAnim("rotfood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Organics, false);
		component.AddTag(GameTags.Compostable, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<OccupyArea>();
		gameObject.AddOrGet<Modifiers>();
		gameObject.AddOrGet<RotPile>();
		gameObject.AddComponent<DecorProvider>().SetValues(DECOR.PENALTY.TIER2);
		return gameObject;
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x000380FF File Offset: 0x000362FF
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<DecorProvider>().overrideName = STRINGS.ITEMS.FOOD.ROTPILE.NAME;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00038116 File Offset: 0x00036316
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400061D RID: 1565
	public static string ID = "RotPile";
}
