using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class FoodSplatConfig : IEntityConfig
{
	// Token: 0x06000995 RID: 2453 RVA: 0x000392D9 File Offset: 0x000374D9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x000392E0 File Offset: 0x000374E0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateBasicEntity("FoodSplat", STRINGS.ITEMS.FOOD.FOODSPLAT.NAME, STRINGS.ITEMS.FOOD.FOODSPLAT.DESC, 1f, true, Assets.GetAnim("sticker_a_kanim"), "idle_sticker_a", Grid.SceneLayer.Backwall, SimHashes.Creature, null, 293f);
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00039331 File Offset: 0x00037531
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		inst.AddComponent<Modifiers>();
		inst.AddOrGet<KSelectable>();
		inst.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER2);
		inst.AddOrGetDef<Splat.Def>();
		inst.AddOrGet<SplatWorkable>();
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00039370 File Offset: 0x00037570
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000664 RID: 1636
	public const string ID = "FoodSplat";
}
