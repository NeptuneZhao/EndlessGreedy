using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000082 RID: 130
public class FunkyVestConfig : IEquipmentConfig
{
	// Token: 0x06000287 RID: 647 RVA: 0x00011F3D File Offset: 0x0001013D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00011F44 File Offset: 0x00010144
	public EquipmentDef CreateEquipmentDef()
	{
		new Dictionary<string, float>().Add("BasicFabric", (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS);
		ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.FANCY_CLOTHING;
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Funky_Vest", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, "shirt_decor01_kanim", TUNING.EQUIPMENT.VESTS.SNAPON0, "body_shirt_decor01_kanim", 4, attributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
		Descriptor item = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)), Descriptor.DescriptorType.Effect, false);
		Descriptor item2 = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, ClothingWearer.ClothingInfo.FANCY_CLOTHING.decorMod), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, ClothingWearer.ClothingInfo.FANCY_CLOTHING.decorMod), Descriptor.DescriptorType.Effect, false);
		equipmentDef.additionalDescriptors.Add(item);
		equipmentDef.additionalDescriptors.Add(item2);
		equipmentDef.OnEquipCallBack = delegate(Equippable eq)
		{
			ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo);
		};
		equipmentDef.OnUnequipCallBack = new Action<Equippable>(ClothingWearer.ClothingInfo.OnUnequipVest);
		equipmentDef.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.RECIPE_DESC;
		return equipmentDef;
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00012094 File Offset: 0x00010294
	public static void SetupVest(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
		Equippable equippable = go.GetComponent<Equippable>();
		if (equippable == null)
		{
			equippable = go.AddComponent<Equippable>();
		}
		equippable.SetQuality(global::QualityLevel.Poor);
		go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
	}

	// Token: 0x0600028A RID: 650 RVA: 0x000120DD File Offset: 0x000102DD
	public void DoPostConfigure(GameObject go)
	{
		FunkyVestConfig.SetupVest(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.PedestalDisplayable, false);
	}

	// Token: 0x04000178 RID: 376
	public const string ID = "Funky_Vest";

	// Token: 0x04000179 RID: 377
	public static ComplexRecipe recipe;
}
