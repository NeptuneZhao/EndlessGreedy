using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002CD RID: 717
public class FullMinionUIPortrait : IEntityConfig
{
	// Token: 0x06000EF9 RID: 3833 RVA: 0x00056D9E File Offset: 0x00054F9E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x00056DA8 File Offset: 0x00054FA8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(FullMinionUIPortrait.ID, FullMinionUIPortrait.ID, true);
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
		gameObject.AddOrGet<FaceGraph>();
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
			Assets.GetAnim("anim_cheer_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		BaseMinionConfig.ConfigureSymbols(gameObject, true);
		return gameObject;
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x00056F14 File Offset: 0x00055114
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x00056F16 File Offset: 0x00055116
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000930 RID: 2352
	public static string ID = "FullMinionUIPortrait";
}
