using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002E1 RID: 737
public class MinionUIPortrait : IEntityConfig
{
	// Token: 0x06000F72 RID: 3954 RVA: 0x000591F8 File Offset: 0x000573F8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x00059200 File Offset: 0x00057400
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionUIPortrait.ID, MinionUIPortrait.ID, true);
		RectTransform rectTransform = gameObject.AddOrGet<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		LayoutElement layoutElement = gameObject.AddOrGet<LayoutElement>();
		layoutElement.preferredHeight = 100f;
		layoutElement.preferredWidth = 100f;
		gameObject.AddOrGet<BoxCollider2D>().size = new Vector2(1f, 1f);
		gameObject.AddOrGet<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
		kbatchedAnimController.animScale = 0.5f;
		kbatchedAnimController.setScaleFromAnim = false;
		kbatchedAnimController.animOverrideSize = new Vector2(100f, 120f);
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_idle_healthy_kanim"),
			Assets.GetAnim("anim_cheer_kanim"),
			Assets.GetAnim("inventory_screen_dupe_kanim"),
			Assets.GetAnim("anim_react_wave_shy_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		BaseMinionConfig.ConfigureSymbols(gameObject, false);
		return gameObject;
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x00059389 File Offset: 0x00057589
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x0005938B File Offset: 0x0005758B
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400097F RID: 2431
	public static string ID = "MinionUIPortrait";
}
