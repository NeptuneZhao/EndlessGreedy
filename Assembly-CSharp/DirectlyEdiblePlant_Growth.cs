using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020007BE RID: 1982
public class DirectlyEdiblePlant_Growth : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x060036AC RID: 13996 RVA: 0x00129BD0 File Offset: 0x00127DD0
	public bool CanPlantBeEaten()
	{
		float num = 0.25f;
		float num2 = 0f;
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(base.gameObject);
		if (amountInstance != null)
		{
			num2 = amountInstance.value / amountInstance.GetMax();
		}
		return num2 >= num;
	}

	// Token: 0x060036AD RID: 13997 RVA: 0x00129C1C File Offset: 0x00127E1C
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(this.growing.gameObject);
		float growthUnitToMaturityRatio = this.GetGrowthUnitToMaturityRatio(amountInstance.GetMax(), base.GetComponent<KPrefabID>());
		float b = amountInstance.value * growthUnitToMaturityRatio;
		float num = Mathf.Min(desiredUnitsToConsume, b);
		this.growing.ConsumeGrowthUnits(num, growthUnitToMaturityRatio);
		return num;
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x00129C84 File Offset: 0x00127E84
	public float PlantProductGrowthPerCycle()
	{
		Crop crop = base.GetComponent<Crop>();
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == crop.cropId).cropDuration / 600f;
		return 1f / num;
	}

	// Token: 0x060036AF RID: 13999 RVA: 0x00129CCC File Offset: 0x00127ECC
	private float GetGrowthUnitToMaturityRatio(float maturityMax, KPrefabID prefab_id)
	{
		ResourceSet<Trait> traits = Db.Get().traits;
		Tag prefabTag = prefab_id.PrefabTag;
		Trait trait = traits.Get(prefabTag.ToString() + "Original");
		if (trait != null)
		{
			AttributeModifier attributeModifier = trait.SelfModifiers.Find((AttributeModifier match) => match.AttributeId == "MaturityMax");
			if (attributeModifier != null)
			{
				return attributeModifier.Value / maturityMax;
			}
		}
		return 1f;
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x00129D48 File Offset: 0x00127F48
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = this.PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x00129D6B File Offset: 0x00127F6B
	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}

	// Token: 0x060036B2 RID: 14002 RVA: 0x00129D6E File Offset: 0x00127F6E
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantDirectly;
	}

	// Token: 0x04002054 RID: 8276
	[MyCmpGet]
	private Growing growing;
}
