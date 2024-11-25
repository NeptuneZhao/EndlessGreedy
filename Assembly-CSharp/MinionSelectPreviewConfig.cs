﻿using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002E0 RID: 736
public class MinionSelectPreviewConfig : IEntityConfig
{
	// Token: 0x06000F6C RID: 3948 RVA: 0x00059014 File Offset: 0x00057214
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x0005901C File Offset: 0x0005721C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionSelectPreviewConfig.ID, MinionSelectPreviewConfig.ID, true);
		RectTransform rectTransform = gameObject.AddOrGet<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		LayoutElement layoutElement = gameObject.AddOrGet<LayoutElement>();
		layoutElement.preferredHeight = 100f;
		layoutElement.preferredWidth = 100f;
		gameObject.AddOrGet<Effects>();
		gameObject.AddOrGet<Traits>();
		MinionModifiers minionModifiers = gameObject.AddOrGet<MinionModifiers>();
		minionModifiers.initialTraits.Add(BaseMinionConfig.GetMinionBaseTraitIDForModel(MinionConfig.MODEL));
		BaseMinionConfig.AddMinionAttributes(minionModifiers, MinionConfig.GetAttributes());
		BaseMinionConfig.AddMinionAmounts(minionModifiers, MinionConfig.GetAmounts());
		gameObject.AddOrGet<AttributeLevels>();
		gameObject.AddOrGet<AttributeConverters>();
		gameObject.AddOrGet<MinionIdentity>().addToIdentityList = false;
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
			Assets.GetAnim("anim_construction_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_cheer_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		BaseMinionConfig.ConfigureSymbols(gameObject, false);
		return gameObject;
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x000591E0 File Offset: 0x000573E0
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x000591E2 File Offset: 0x000573E2
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400097E RID: 2430
	public static string ID = "MinionSelectPreview";
}
