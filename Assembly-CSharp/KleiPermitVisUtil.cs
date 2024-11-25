using System;
using Database;
using UnityEngine;

// Token: 0x02000C95 RID: 3221
public static class KleiPermitVisUtil
{
	// Token: 0x060062F0 RID: 25328 RVA: 0x0024DEEC File Offset: 0x0024C0EC
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingFacadeResource buildingPermit)
	{
		KAnimFile anim = Assets.GetAnim(buildingPermit.AnimFile);
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			anim
		});
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(anim), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060062F1 RID: 25329 RVA: 0x0024DF54 File Offset: 0x0024C154
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingDef buildingDef)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(buildingDef.AnimFiles);
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(buildingDef.AnimFiles[0]), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060062F2 RID: 25330 RVA: 0x0024DFAC File Offset: 0x0024C1AC
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, ArtableStage artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(artablePermit.animFile)
		});
		buildingKAnim.Play(artablePermit.anim, KAnim.PlayMode.Once, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060062F3 RID: 25331 RVA: 0x0024E014 File Offset: 0x0024C214
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, DbStickerBomb artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			artablePermit.animFile
		});
		HashedString defaultStickerAnimHash = KleiPermitVisUtil.GetDefaultStickerAnimHash(artablePermit.animFile);
		if (defaultStickerAnimHash != null)
		{
			buildingKAnim.Play(defaultStickerAnimHash, KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			global::Debug.Assert(false, "Couldn't find default sticker for sticker " + artablePermit.Id);
			buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(artablePermit.animFile), KAnim.PlayMode.Once, 1f, 0f);
		}
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060062F4 RID: 25332 RVA: 0x0024E0B8 File Offset: 0x0024C2B8
	public static void ConfigureBuildingPosition(RectTransform transform, PrefabDefinedUIPosition anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		anchorPosition.SetOn(transform);
		transform.anchoredPosition += new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x060062F5 RID: 25333 RVA: 0x0024E114 File Offset: 0x0024C314
	public static void ConfigureBuildingPosition(RectTransform transform, Vector2 anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		transform.anchoredPosition = anchorPosition + new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x060062F6 RID: 25334 RVA: 0x0024E162 File Offset: 0x0024C362
	public static void ClearAnimation()
	{
		if (!KleiPermitVisUtil.buildingAnimateIn.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(KleiPermitVisUtil.buildingAnimateIn.gameObject);
		}
	}

	// Token: 0x060062F7 RID: 25335 RVA: 0x0024E17F File Offset: 0x0024C37F
	public static void AnimateIn(KBatchedAnimController buildingKAnim, Updater extraUpdater = default(Updater))
	{
		KleiPermitVisUtil.ClearAnimation();
		KleiPermitVisUtil.buildingAnimateIn = KleiPermitBuildingAnimateIn.MakeFor(buildingKAnim, extraUpdater);
	}

	// Token: 0x060062F8 RID: 25336 RVA: 0x0024E192 File Offset: 0x0024C392
	public static HashedString GetFirstAnimHash(KAnimFile animFile)
	{
		return animFile.GetData().GetAnim(0).hash;
	}

	// Token: 0x060062F9 RID: 25337 RVA: 0x0024E1A8 File Offset: 0x0024C3A8
	public static HashedString GetDefaultStickerAnimHash(KAnimFile stickerAnimFile)
	{
		KAnimFileData data = stickerAnimFile.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.name.StartsWith("idle_sticker"))
			{
				return anim.hash;
			}
		}
		return null;
	}

	// Token: 0x060062FA RID: 25338 RVA: 0x0024E1F4 File Offset: 0x0024C3F4
	public static BuildLocationRule? GetBuildLocationRule(PermitResource permit)
	{
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		if (buildingDef == null)
		{
			return null;
		}
		return new BuildLocationRule?(buildingDef.BuildLocationRule);
	}

	// Token: 0x060062FB RID: 25339 RVA: 0x0024E228 File Offset: 0x0024C428
	public static BuildingDef GetBuildingDef(PermitResource permit)
	{
		BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
		if (buildingFacadeResource != null)
		{
			GameObject gameObject = Assets.TryGetPrefab(buildingFacadeResource.PrefabID);
			if (gameObject == null)
			{
				return null;
			}
			BuildingComplete component = gameObject.GetComponent<BuildingComplete>();
			if (component == null || !component)
			{
				return null;
			}
			return component.Def;
		}
		else
		{
			ArtableStage artableStage = permit as ArtableStage;
			if (artableStage == null)
			{
				return null;
			}
			BuildingComplete component2 = Assets.GetPrefab(artableStage.prefabId).GetComponent<BuildingComplete>();
			if (component2 == null || !component2)
			{
				return null;
			}
			return component2.Def;
		}
	}

	// Token: 0x0400431F RID: 17183
	public const float TILE_SIZE_UI = 176f;

	// Token: 0x04004320 RID: 17184
	public static KleiPermitBuildingAnimateIn buildingAnimateIn;
}
