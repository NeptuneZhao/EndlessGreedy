using System;
using STRINGS;
using UnityEngine;

// Token: 0x020007BF RID: 1983
public class DirectlyEdiblePlant_StorageElement : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x060036B4 RID: 14004 RVA: 0x00129D79 File Offset: 0x00127F79
	public float MassGeneratedPerCycle
	{
		get
		{
			return this.rateProducedPerCycle * this.storageCapacity;
		}
	}

	// Token: 0x060036B5 RID: 14005 RVA: 0x00129D88 File Offset: 0x00127F88
	protected override void OnPrefabInit()
	{
		this.storageCapacity = this.storage.capacityKg;
		base.OnPrefabInit();
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x00129DA4 File Offset: 0x00127FA4
	public bool CanPlantBeEaten()
	{
		Tag tag = this.GetTagToConsume();
		return this.storage.GetMassAvailable(tag) / this.storage.capacityKg >= this.minimum_mass_percentageRequiredToEat;
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x00129DDC File Offset: 0x00127FDC
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		if (this.storage.MassStored() <= 0f)
		{
			return 0f;
		}
		Tag tag = this.GetTagToConsume();
		float massAvailable = this.storage.GetMassAvailable(tag);
		float num = Mathf.Min(desiredUnitsToConsume, massAvailable);
		this.storage.ConsumeIgnoringDisease(tag, num);
		return num;
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x00129E2B File Offset: 0x0012802B
	public float PlantProductGrowthPerCycle()
	{
		return this.MassGeneratedPerCycle;
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x00129E33 File Offset: 0x00128033
	private Tag GetTagToConsume()
	{
		if (!(this.tagToConsume != Tag.Invalid))
		{
			return this.storage.items[0].GetComponent<KPrefabID>().PrefabTag;
		}
		return this.tagToConsume;
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x00129E69 File Offset: 0x00128069
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		return string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.EDIBLE_PLANT_INTERNAL_STORAGE, GameUtil.GetFormattedMass(consumer_KGWorthOfCaloriesLostPerSecond, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), this.tagToConsume.ProperName());
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x00129E93 File Offset: 0x00128093
	public CellOffset[] GetAllowedOffsets()
	{
		return this.edibleCellOffsets;
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x00129E9B File Offset: 0x0012809B
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantStorage;
	}

	// Token: 0x04002055 RID: 8277
	public CellOffset[] edibleCellOffsets;

	// Token: 0x04002056 RID: 8278
	public Tag tagToConsume = Tag.Invalid;

	// Token: 0x04002057 RID: 8279
	public float rateProducedPerCycle;

	// Token: 0x04002058 RID: 8280
	public float storageCapacity;

	// Token: 0x04002059 RID: 8281
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400205A RID: 8282
	[MyCmpGet]
	private KPrefabID prefabID;

	// Token: 0x0400205B RID: 8283
	public float minimum_mass_percentageRequiredToEat = 0.25f;
}
