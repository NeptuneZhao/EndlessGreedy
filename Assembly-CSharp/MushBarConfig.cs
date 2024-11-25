using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class MushBarConfig : IEntityConfig
{
	// Token: 0x060008D0 RID: 2256 RVA: 0x000376C6 File Offset: 0x000358C6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x000376D0 File Offset: 0x000358D0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MushBar", STRINGS.ITEMS.FOOD.MUSHBAR.NAME, STRINGS.ITEMS.FOOD.MUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbar_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.MUSHBAR);
		ComplexRecipeManager.Get().GetRecipe(MushBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00037757 File Offset: 0x00035957
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x00037759 File Offset: 0x00035959
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0003775C File Offset: 0x0003595C
	public static GameObject CreateFabricationVisualizer(GameObject result)
	{
		KBatchedAnimController component = result.GetComponent<KBatchedAnimController>();
		GameObject gameObject = new GameObject();
		gameObject.name = result.name + "Visualizer";
		gameObject.SetActive(false);
		gameObject.transform.SetLocalPosition(Vector3.zero);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = component.AnimFiles;
		kbatchedAnimController.initialAnim = "fabricating";
		kbatchedAnimController.isMovable = true;
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = new HashedString("meter_ration");
		kbatchedAnimTracker.offset = Vector3.zero;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		return gameObject;
	}

	// Token: 0x04000600 RID: 1536
	public const string ID = "MushBar";

	// Token: 0x04000601 RID: 1537
	public static ComplexRecipe recipe;
}
