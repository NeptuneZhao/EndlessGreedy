﻿using System;
using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
	// Token: 0x02000F49 RID: 3913
	public class FoodGerms : Disease
	{
		// Token: 0x06007877 RID: 30839 RVA: 0x002F9F0C File Offset: 0x002F810C
		public FoodGerms(bool statsOnly) : base("FoodPoisoning", 10f, new Disease.RangeInfo(248.15f, 278.15f, 313.15f, 348.15f), new Disease.RangeInfo(10f, 1200f, 1200f, 10f), new Disease.RangeInfo(0f, 0f, 1000f, 1000f), Disease.RangeInfo.Idempotent(), 2.5f, statsOnly)
		{
		}

		// Token: 0x06007878 RID: 30840 RVA: 0x002F9F80 File Offset: 0x002F8180
		protected override void PopulateElemGrowthInfo()
		{
			base.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
			base.AddGrowthRule(new GrowthRule
			{
				underPopulationDeathRate = new float?(2.6666667f),
				minCountPerKG = new float?(0.4f),
				populationHalfLife = new float?(12000f),
				maxCountPerKG = new float?((float)1000),
				overPopulationHalfLife = new float?(3000f),
				minDiffusionCount = new int?(1000),
				diffusionScale = new float?(0.001f),
				minDiffusionInfestationTickCount = new byte?((byte)1)
			});
			base.AddGrowthRule(new StateGrowthRule(Element.State.Solid)
			{
				minCountPerKG = new float?(0.4f),
				populationHalfLife = new float?(300f),
				overPopulationHalfLife = new float?(10f),
				minDiffusionCount = new int?(1000000)
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.ToxicSand)
			{
				populationHalfLife = new float?(float.PositiveInfinity),
				overPopulationHalfLife = new float?(12000f)
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.Creature)
			{
				populationHalfLife = new float?(float.PositiveInfinity),
				maxCountPerKG = new float?((float)4000),
				overPopulationHalfLife = new float?(3000f)
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.BleachStone)
			{
				populationHalfLife = new float?(10f),
				overPopulationHalfLife = new float?(10f),
				diffusionScale = new float?(0.001f)
			});
			base.AddGrowthRule(new StateGrowthRule(Element.State.Gas)
			{
				minCountPerKG = new float?(250f),
				populationHalfLife = new float?(1200f),
				overPopulationHalfLife = new float?(300f),
				diffusionScale = new float?(0.01f)
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.ContaminatedOxygen)
			{
				populationHalfLife = new float?(12000f),
				maxCountPerKG = new float?((float)10000),
				overPopulationHalfLife = new float?(3000f),
				diffusionScale = new float?(0.05f)
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.ChlorineGas)
			{
				populationHalfLife = new float?(10f),
				overPopulationHalfLife = new float?(10f),
				minDiffusionCount = new int?(1000000)
			});
			base.AddGrowthRule(new StateGrowthRule(Element.State.Liquid)
			{
				minCountPerKG = new float?(0.4f),
				populationHalfLife = new float?(12000f),
				maxCountPerKG = new float?((float)5000),
				diffusionScale = new float?(0.2f)
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.DirtyWater)
			{
				populationHalfLife = new float?(-12000f),
				overPopulationHalfLife = new float?(12000f)
			});
			base.AddGrowthRule(new TagGrowthRule(GameTags.Edible)
			{
				populationHalfLife = new float?(-12000f),
				overPopulationHalfLife = new float?(float.PositiveInfinity)
			});
			base.AddGrowthRule(new TagGrowthRule(GameTags.Pickled)
			{
				populationHalfLife = new float?(10f),
				overPopulationHalfLife = new float?(10f)
			});
			base.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
			base.AddExposureRule(new ExposureRule
			{
				populationHalfLife = new float?(float.PositiveInfinity)
			});
			base.AddExposureRule(new ElementExposureRule(SimHashes.DirtyWater)
			{
				populationHalfLife = new float?(-12000f)
			});
			base.AddExposureRule(new ElementExposureRule(SimHashes.ContaminatedOxygen)
			{
				populationHalfLife = new float?(-12000f)
			});
			base.AddExposureRule(new ElementExposureRule(SimHashes.ChlorineGas)
			{
				populationHalfLife = new float?(10f)
			});
		}

		// Token: 0x04005A00 RID: 23040
		public const string ID = "FoodPoisoning";

		// Token: 0x04005A01 RID: 23041
		private const float VOMIT_FREQUENCY = 200f;
	}
}
