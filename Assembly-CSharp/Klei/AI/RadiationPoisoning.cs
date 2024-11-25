using System;
using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
	// Token: 0x02000F4C RID: 3916
	public class RadiationPoisoning : Disease
	{
		// Token: 0x0600787C RID: 30844 RVA: 0x002FA73C File Offset: 0x002F893C
		public RadiationPoisoning(bool statsOnly) : base("RadiationSickness", 100f, Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), 0f, statsOnly)
		{
		}

		// Token: 0x0600787D RID: 30845 RVA: 0x002FA774 File Offset: 0x002F8974
		protected override void PopulateElemGrowthInfo()
		{
			base.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
			base.AddGrowthRule(new GrowthRule
			{
				underPopulationDeathRate = new float?(0f),
				minCountPerKG = new float?(0f),
				populationHalfLife = new float?(600f),
				maxCountPerKG = new float?(float.PositiveInfinity),
				overPopulationHalfLife = new float?(600f),
				minDiffusionCount = new int?(10000),
				diffusionScale = new float?(0f),
				minDiffusionInfestationTickCount = new byte?((byte)1)
			});
			base.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
		}

		// Token: 0x04005A06 RID: 23046
		public const string ID = "RadiationSickness";
	}
}
