using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020007C0 RID: 1984
public class DirectlyEdiblePlant_TreeBranches : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x060036BE RID: 14014 RVA: 0x00129EBC File Offset: 0x001280BC
	protected override void OnSpawn()
	{
		this.trunk = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		base.OnSpawn();
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x00129ED5 File Offset: 0x001280D5
	public bool CanPlantBeEaten()
	{
		return this.GetMaxBranchMaturity() >= this.MinimumEdibleMaturity;
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x00129EE8 File Offset: 0x001280E8
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		float maxBranchMaturity = this.GetMaxBranchMaturity();
		float num = Mathf.Min(desiredUnitsToConsume, maxBranchMaturity);
		GameObject mostMatureBranch = this.GetMostMatureBranch();
		if (!mostMatureBranch)
		{
			return 0f;
		}
		Growing component = mostMatureBranch.GetComponent<Growing>();
		if (component)
		{
			Harvestable component2 = mostMatureBranch.GetComponent<Harvestable>();
			if (component2 != null)
			{
				component2.Trigger(2127324410, true);
			}
			component.ConsumeMass(num);
			return num;
		}
		mostMatureBranch.GetAmounts().Get(Db.Get().Amounts.Maturity.Id).ApplyDelta(-desiredUnitsToConsume);
		base.gameObject.Trigger(-1793167409, null);
		mostMatureBranch.Trigger(-1793167409, null);
		return desiredUnitsToConsume;
	}

	// Token: 0x060036C1 RID: 14017 RVA: 0x00129FA0 File Offset: 0x001281A0
	public float PlantProductGrowthPerCycle()
	{
		Crop component = base.GetComponent<Crop>();
		string cropID = component.cropId;
		if (this.overrideCropID != null)
		{
			cropID = this.overrideCropID;
		}
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == cropID).cropDuration / 600f;
		return 1f / num;
	}

	// Token: 0x060036C2 RID: 14018 RVA: 0x0012A004 File Offset: 0x00128204
	public float GetMaxBranchMaturity()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		this.trunk.ActionPerBranch(delegate(GameObject branch)
		{
			if (branch != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						max_branch = branch;
					}
				}
			}
		});
		return max_maturity;
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x0012A048 File Offset: 0x00128248
	private GameObject GetMostMatureBranch()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		this.trunk.ActionPerBranch(delegate(GameObject branch)
		{
			if (branch != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						max_branch = branch;
					}
				}
			}
		});
		return max_branch;
	}

	// Token: 0x060036C4 RID: 14020 RVA: 0x0012A08C File Offset: 0x0012828C
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = this.PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x0012A0AF File Offset: 0x001282AF
	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x0012A0B2 File Offset: 0x001282B2
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantDirectly;
	}

	// Token: 0x0400205C RID: 8284
	private PlantBranchGrower.Instance trunk;

	// Token: 0x0400205D RID: 8285
	public float MinimumEdibleMaturity = 0.25f;

	// Token: 0x0400205E RID: 8286
	public string overrideCropID;
}
