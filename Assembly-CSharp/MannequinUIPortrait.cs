﻿using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002DB RID: 731
public class MannequinUIPortrait : IEntityConfig
{
	// Token: 0x06000F4B RID: 3915 RVA: 0x00058853 File Offset: 0x00056A53
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0005885C File Offset: 0x00056A5C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MannequinUIPortrait.ID, MannequinUIPortrait.ID, true);
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
			Assets.GetAnim("mannequin_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		BaseMinionConfig.ConfigureSymbols(gameObject, false);
		return gameObject;
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x0005898B File Offset: 0x00056B8B
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x0005898D File Offset: 0x00056B8D
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000974 RID: 2420
	public static string ID = "MannequinUIPortrait";
}
