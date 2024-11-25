using System;

// Token: 0x02000545 RID: 1349
public interface IPlantConsumptionInstructions
{
	// Token: 0x06001EFA RID: 7930
	CellOffset[] GetAllowedOffsets();

	// Token: 0x06001EFB RID: 7931
	float ConsumePlant(float desiredUnitsToConsume);

	// Token: 0x06001EFC RID: 7932
	float PlantProductGrowthPerCycle();

	// Token: 0x06001EFD RID: 7933
	bool CanPlantBeEaten();

	// Token: 0x06001EFE RID: 7934
	string GetFormattedConsumptionPerCycle(float consumer_caloriesLossPerCaloriesPerKG);

	// Token: 0x06001EFF RID: 7935
	Diet.Info.FoodType GetDietFoodType();
}
