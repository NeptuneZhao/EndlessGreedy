using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C85 RID: 3205
public class KleiPermitDioramaVis : KMonoBehaviour
{
	// Token: 0x0600629A RID: 25242 RVA: 0x0024CD03 File Offset: 0x0024AF03
	protected override void OnPrefabInit()
	{
		this.Init();
	}

	// Token: 0x0600629B RID: 25243 RVA: 0x0024CD0C File Offset: 0x0024AF0C
	private void Init()
	{
		if (this.initComplete)
		{
			return;
		}
		this.allVisList = ReflectionUtil.For<KleiPermitDioramaVis>(this).CollectValuesForFieldsThatInheritOrImplement<IKleiPermitDioramaVisTarget>(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.ConfigureSetup();
		}
		this.initComplete = true;
	}

	// Token: 0x0600629C RID: 25244 RVA: 0x0024CD7C File Offset: 0x0024AF7C
	public void ConfigureWith(PermitResource permit)
	{
		if (!this.initComplete)
		{
			this.Init();
		}
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.GetGameObject().SetActive(false);
		}
		KleiPermitVisUtil.ClearAnimation();
		IKleiPermitDioramaVisTarget permitVisTarget = this.GetPermitVisTarget(permit);
		permitVisTarget.GetGameObject().SetActive(true);
		permitVisTarget.ConfigureWith(permit);
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			this.dlcImage.gameObject.SetActive(true);
			this.dlcImage.sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlcIdFrom));
			return;
		}
		this.dlcImage.gameObject.SetActive(false);
	}

	// Token: 0x0600629D RID: 25245 RVA: 0x0024CE48 File Offset: 0x0024B048
	private IKleiPermitDioramaVisTarget GetPermitVisTarget(PermitResource permit)
	{
		KleiPermitDioramaVis.lastRenderedPermit = permit;
		if (permit == null)
		{
			return this.fallbackVis.WithError(string.Format("Given invalid permit: {0}", permit));
		}
		if (permit.Category == PermitCategory.Equipment || permit.Category == PermitCategory.DupeTops || permit.Category == PermitCategory.DupeBottoms || permit.Category == PermitCategory.DupeGloves || permit.Category == PermitCategory.DupeShoes || permit.Category == PermitCategory.DupeHats || permit.Category == PermitCategory.DupeAccessories || permit.Category == PermitCategory.AtmoSuitHelmet || permit.Category == PermitCategory.AtmoSuitBody || permit.Category == PermitCategory.AtmoSuitGloves || permit.Category == PermitCategory.AtmoSuitBelt || permit.Category == PermitCategory.AtmoSuitShoes)
		{
			return this.equipmentVis;
		}
		if (permit.Category == PermitCategory.Building)
		{
			BuildLocationRule? buildLocationRule = KleiPermitVisUtil.GetBuildLocationRule(permit);
			if (buildLocationRule == null)
			{
				if (permit.DlcIds.SequenceEqual(DlcManager.AVAILABLE_EXPANSION1_ONLY))
				{
					return this.buildingOnFloorVis;
				}
				return this.fallbackVis.WithError("Couldn't get BuildLocationRule on permit with id \"" + permit.Id + "\"");
			}
			else
			{
				BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
				if (!buildingDef.BuildingComplete.GetComponent<Bed>().IsNullOrDestroyed())
				{
					return this.buildingOnFloorVis;
				}
				if (buildingDef.PrefabID == "RockCrusher" || buildingDef.PrefabID == "GasReservoir" || buildingDef.PrefabID == "ArcadeMachine" || buildingDef.PrefabID == "MicrobeMusher" || buildingDef.PrefabID == "FlushToilet" || buildingDef.PrefabID == "WashSink" || buildingDef.PrefabID == "Headquarters" || buildingDef.PrefabID == "GourmetCookingStation")
				{
					return this.buildingOnFloorBigVis;
				}
				if (!buildingDef.BuildingComplete.GetComponent<RocketModule>().IsNullOrDestroyed() || !buildingDef.BuildingComplete.GetComponent<RocketEngine>().IsNullOrDestroyed())
				{
					return this.buildingRocketVis;
				}
				if (buildingDef.PrefabID == "PlanterBox" || buildingDef.PrefabID == "FlowerVase")
				{
					return this.buildingOnFloorBotanicalVis;
				}
				if (buildingDef.PrefabID == "ExteriorWall")
				{
					return this.wallpaperVis;
				}
				if (buildingDef.PrefabID == "FlowerVaseHanging" || buildingDef.PrefabID == "FlowerVaseHangingFancy")
				{
					return this.buildingHangingHookBotanicalVis;
				}
				if (buildLocationRule != null)
				{
					BuildLocationRule valueOrDefault = buildLocationRule.GetValueOrDefault();
					switch (valueOrDefault)
					{
					case BuildLocationRule.OnFloor:
						break;
					case BuildLocationRule.OnFloorOverSpace:
						goto IL_2AE;
					case BuildLocationRule.OnCeiling:
						return this.buildingOnCeilingVis.WithAlignment(Alignment.Top());
					case BuildLocationRule.OnWall:
						return this.buildingOnWallVis.WithAlignment(Alignment.Left());
					case BuildLocationRule.InCorner:
						return this.buildingInCeilingCornerVis.WithAlignment(Alignment.TopLeft());
					default:
						if (valueOrDefault != BuildLocationRule.OnFoundationRotatable)
						{
							goto IL_2AE;
						}
						break;
					}
					return this.buildingOnFloorVis;
				}
				IL_2AE:
				return this.fallbackVis.WithError(string.Format("No visualization available for building with BuildLocationRule of {0}", buildLocationRule));
			}
		}
		else if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef2.IsNullOrDestroyed())
			{
				return this.fallbackVis.WithError("Couldn't find building def for Artable " + permit.Id);
			}
			ArtableStage artableStage = (ArtableStage)permit;
			if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|21_0<Sculpture>(buildingDef2))
			{
				if (buildingDef2.PrefabID == "WoodSculpture")
				{
					return this.artablePaintingVis;
				}
				return this.artableSculptureVis;
			}
			else
			{
				if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|21_0<Painting>(buildingDef2))
				{
					return this.artablePaintingVis;
				}
				return this.fallbackVis.WithError("No visualization available for Artable " + permit.Id);
			}
		}
		else
		{
			if (permit.Category != PermitCategory.JoyResponse)
			{
				return this.fallbackVis.WithError("No visualization has been defined for permit with id \"" + permit.Id + "\"");
			}
			if (permit is BalloonArtistFacadeResource)
			{
				return this.joyResponseBalloonVis;
			}
			return this.fallbackVis.WithError("No visualization available for JoyResponse " + permit.Id);
		}
	}

	// Token: 0x0600629E RID: 25246 RVA: 0x0024D208 File Offset: 0x0024B408
	public static Sprite GetDioramaBackground(PermitCategory permitCategory)
	{
		switch (permitCategory)
		{
		case PermitCategory.DupeTops:
		case PermitCategory.DupeBottoms:
		case PermitCategory.DupeGloves:
		case PermitCategory.DupeShoes:
		case PermitCategory.DupeHats:
		case PermitCategory.DupeAccessories:
			return Assets.GetSprite("screen_bg_clothing");
		case PermitCategory.AtmoSuitHelmet:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitGloves:
		case PermitCategory.AtmoSuitBelt:
		case PermitCategory.AtmoSuitShoes:
			return Assets.GetSprite("screen_bg_atmosuit");
		case PermitCategory.Building:
			return Assets.GetSprite("screen_bg_buildings");
		case PermitCategory.Artwork:
			return Assets.GetSprite("screen_bg_art");
		case PermitCategory.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		}
		return null;
	}

	// Token: 0x0600629F RID: 25247 RVA: 0x0024D2B4 File Offset: 0x0024B4B4
	public static Sprite GetDioramaBackground(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return Assets.GetSprite("screen_bg_clothing");
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return Assets.GetSprite("screen_bg_atmosuit");
		default:
			return null;
		}
	}

	// Token: 0x060062A1 RID: 25249 RVA: 0x0024D30E File Offset: 0x0024B50E
	[CompilerGenerated]
	internal static bool <GetPermitVisTarget>g__Has|21_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x040042DF RID: 17119
	[SerializeField]
	private Image dlcImage;

	// Token: 0x040042E0 RID: 17120
	[SerializeField]
	private KleiPermitDioramaVis_Fallback fallbackVis;

	// Token: 0x040042E1 RID: 17121
	[SerializeField]
	private KleiPermitDioramaVis_DupeEquipment equipmentVis;

	// Token: 0x040042E2 RID: 17122
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorVis;

	// Token: 0x040042E3 RID: 17123
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloorBig buildingOnFloorBigVis;

	// Token: 0x040042E4 RID: 17124
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnWallVis;

	// Token: 0x040042E5 RID: 17125
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnCeilingVis;

	// Token: 0x040042E6 RID: 17126
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingInCeilingCornerVis;

	// Token: 0x040042E7 RID: 17127
	[SerializeField]
	private KleiPermitDioramaVis_BuildingRocket buildingRocketVis;

	// Token: 0x040042E8 RID: 17128
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorBotanicalVis;

	// Token: 0x040042E9 RID: 17129
	[SerializeField]
	private KleiPermitDioramaVis_BuildingHangingHook buildingHangingHookBotanicalVis;

	// Token: 0x040042EA RID: 17130
	[SerializeField]
	private KleiPermitDioramaVis_Wallpaper wallpaperVis;

	// Token: 0x040042EB RID: 17131
	[SerializeField]
	private KleiPermitDioramaVis_ArtablePainting artablePaintingVis;

	// Token: 0x040042EC RID: 17132
	[SerializeField]
	private KleiPermitDioramaVis_ArtableSculpture artableSculptureVis;

	// Token: 0x040042ED RID: 17133
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon joyResponseBalloonVis;

	// Token: 0x040042EE RID: 17134
	private bool initComplete;

	// Token: 0x040042EF RID: 17135
	private IReadOnlyList<IKleiPermitDioramaVisTarget> allVisList;

	// Token: 0x040042F0 RID: 17136
	public static PermitResource lastRenderedPermit;
}
