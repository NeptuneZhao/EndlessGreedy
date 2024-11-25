using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class DisposableElectrobankConfig : IMultiEntityConfig
{
	// Token: 0x06000EDB RID: 3803 RVA: 0x0005678C File Offset: 0x0005498C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_RawMetal", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.DESC, 20f, SimHashes.Cuprite, "electrobank_popcan_kanim", DlcManager.DLC3, null, "object"));
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_BasicSingleHarvestPlant", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_MUCKROOT.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_MUCKROOT.DESC, 20f, SimHashes.Creature, "electrobank_muckroot_kanim", DlcManager.DLC3, null, "object"));
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_LightBugEgg", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_LIGHTBUGEGG.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_LIGHTBUGEGG.DESC, 20f, SimHashes.Creature, "electrobank_shinebug_egg_kanim", DlcManager.DLC3, null, "object"));
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_Sucrose", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SUCROSE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SUCROSE.DESC, 20f, SimHashes.Sucrose, "electrobank_sucrose_kanim", DlcManager.DLC3, null, "object"));
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_UraniumOre", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.DESC, 20f, SimHashes.UraniumOre, "electrobank_uranium_kanim", DlcManager.DLC3.Append(DlcManager.EXPANSION1), null, "object"));
		list.RemoveAll((GameObject t) => t == null);
		return list;
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x000568D8 File Offset: 0x00054AD8
	private GameObject CreateDisposableElectrobank(string id, LocString name, LocString description, float mass, SimHashes element, string animName, string[] requiredDlcIDs = null, string[] forbiddenDlcIds = null, string initialAnim = "object")
	{
		if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIDs, forbiddenDlcIds))
		{
			return null;
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, description, mass, true, Assets.GetAnim(animName), initialAnim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddComponent<Electrobank>();
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x00056974 File Offset: 0x00054B74
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00056976 File Offset: 0x00054B76
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000920 RID: 2336
	public const string ID = "DisposableElectrobank_";

	// Token: 0x04000921 RID: 2337
	public const float MASS = 20f;

	// Token: 0x04000922 RID: 2338
	public static Dictionary<Tag, ComplexRecipe> recipes = new Dictionary<Tag, ComplexRecipe>();

	// Token: 0x04000923 RID: 2339
	public const string ID_METAL_ORE = "DisposableElectrobank_RawMetal";

	// Token: 0x04000924 RID: 2340
	public const string ID_MUCKROOT = "DisposableElectrobank_BasicSingleHarvestPlant";

	// Token: 0x04000925 RID: 2341
	public const string ID_LIGHTBUG_EGG = "DisposableElectrobank_LightBugEgg";

	// Token: 0x04000926 RID: 2342
	public const string ID_SUCROSE = "DisposableElectrobank_Sucrose";

	// Token: 0x04000927 RID: 2343
	public const string ID_URANIUM_ORE = "DisposableElectrobank_UraniumOre";
}
