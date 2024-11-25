using System;
using System.Collections.Generic;

// Token: 0x02000E04 RID: 3588
public static class WorldGenProgressStages
{
	// Token: 0x04004E99 RID: 20121
	public static KeyValuePair<WorldGenProgressStages.Stages, float>[] StageWeights = new KeyValuePair<WorldGenProgressStages.Stages, float>[]
	{
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Failure, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SetupNoise, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateNoise, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateSolarSystem, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.WorldLayout, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.CompleteLayout, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NoiseMapBuilder, 9f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ClearingLevel, 0.5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Processing, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Borders, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ProcessRivers, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ConvertCellsToEdges, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DrawWorldBorder, 0.2f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlaceTemplates, 5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SettleSim, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DetectNaturalCavities, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlacingCreatures, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Complete, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NumberOfStages, 0f)
	};

	// Token: 0x02001F0A RID: 7946
	public enum Stages
	{
		// Token: 0x04008C6F RID: 35951
		Failure,
		// Token: 0x04008C70 RID: 35952
		SetupNoise,
		// Token: 0x04008C71 RID: 35953
		GenerateNoise,
		// Token: 0x04008C72 RID: 35954
		GenerateSolarSystem,
		// Token: 0x04008C73 RID: 35955
		WorldLayout,
		// Token: 0x04008C74 RID: 35956
		CompleteLayout,
		// Token: 0x04008C75 RID: 35957
		NoiseMapBuilder,
		// Token: 0x04008C76 RID: 35958
		ClearingLevel,
		// Token: 0x04008C77 RID: 35959
		Processing,
		// Token: 0x04008C78 RID: 35960
		Borders,
		// Token: 0x04008C79 RID: 35961
		ProcessRivers,
		// Token: 0x04008C7A RID: 35962
		ConvertCellsToEdges,
		// Token: 0x04008C7B RID: 35963
		DrawWorldBorder,
		// Token: 0x04008C7C RID: 35964
		PlaceTemplates,
		// Token: 0x04008C7D RID: 35965
		SettleSim,
		// Token: 0x04008C7E RID: 35966
		DetectNaturalCavities,
		// Token: 0x04008C7F RID: 35967
		PlacingCreatures,
		// Token: 0x04008C80 RID: 35968
		Complete,
		// Token: 0x04008C81 RID: 35969
		NumberOfStages
	}
}
