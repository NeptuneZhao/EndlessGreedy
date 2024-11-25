using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000E49 RID: 3657
	public class AttributeConverters : ResourceSet<AttributeConverter>
	{
		// Token: 0x0600742C RID: 29740 RVA: 0x002C6D24 File Offset: 0x002C4F24
		public AttributeConverter Create(string id, string name, string description, Klei.AI.Attribute attribute, float multiplier, float base_value, IAttributeFormatter formatter, string[] available_dlcs)
		{
			AttributeConverter attributeConverter = new AttributeConverter(id, name, description, multiplier, base_value, attribute, formatter);
			if (DlcManager.IsDlcListValidForCurrentContent(available_dlcs))
			{
				base.Add(attributeConverter);
				attribute.converters.Add(attributeConverter);
			}
			return attributeConverter;
		}

		// Token: 0x0600742D RID: 29741 RVA: 0x002C6D64 File Offset: 0x002C4F64
		public AttributeConverters()
		{
			ToPercentAttributeFormatter formatter = new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None);
			StandardAttributeFormatter formatter2 = new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.None);
			this.MovementSpeed = this.Create("MovementSpeed", "Movement Speed", DUPLICANTS.ATTRIBUTES.ATHLETICS.SPEEDMODIFIER, Db.Get().Attributes.Athletics, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.ConstructionSpeed = this.Create("ConstructionSpeed", "Construction Speed", DUPLICANTS.ATTRIBUTES.CONSTRUCTION.SPEEDMODIFIER, Db.Get().Attributes.Construction, 0.25f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.DiggingSpeed = this.Create("DiggingSpeed", "Digging Speed", DUPLICANTS.ATTRIBUTES.DIGGING.SPEEDMODIFIER, Db.Get().Attributes.Digging, 0.25f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.MachinerySpeed = this.Create("MachinerySpeed", "Machinery Speed", DUPLICANTS.ATTRIBUTES.MACHINERY.SPEEDMODIFIER, Db.Get().Attributes.Machinery, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.HarvestSpeed = this.Create("HarvestSpeed", "Harvest Speed", DUPLICANTS.ATTRIBUTES.BOTANIST.HARVEST_SPEED_MODIFIER, Db.Get().Attributes.Botanist, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.PlantTendSpeed = this.Create("PlantTendSpeed", "Plant Tend Speed", DUPLICANTS.ATTRIBUTES.BOTANIST.TINKER_MODIFIER, Db.Get().Attributes.Botanist, 0.025f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.CompoundingSpeed = this.Create("CompoundingSpeed", "Compounding Speed", DUPLICANTS.ATTRIBUTES.CARING.FABRICATE_SPEEDMODIFIER, Db.Get().Attributes.Caring, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.ResearchSpeed = this.Create("ResearchSpeed", "Research Speed", DUPLICANTS.ATTRIBUTES.LEARNING.RESEARCHSPEED, Db.Get().Attributes.Learning, 0.4f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.TrainingSpeed = this.Create("TrainingSpeed", "Training Speed", DUPLICANTS.ATTRIBUTES.LEARNING.SPEEDMODIFIER, Db.Get().Attributes.Learning, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.CookingSpeed = this.Create("CookingSpeed", "Cooking Speed", DUPLICANTS.ATTRIBUTES.COOKING.SPEEDMODIFIER, Db.Get().Attributes.Cooking, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.ArtSpeed = this.Create("ArtSpeed", "Art Speed", DUPLICANTS.ATTRIBUTES.ART.SPEEDMODIFIER, Db.Get().Attributes.Art, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.DoctorSpeed = this.Create("DoctorSpeed", "Doctor Speed", DUPLICANTS.ATTRIBUTES.CARING.SPEEDMODIFIER, Db.Get().Attributes.Caring, 0.2f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.TidyingSpeed = this.Create("TidyingSpeed", "Tidying Speed", DUPLICANTS.ATTRIBUTES.STRENGTH.SPEEDMODIFIER, Db.Get().Attributes.Strength, 0.25f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.AttackDamage = this.Create("AttackDamage", "Attack Damage", DUPLICANTS.ATTRIBUTES.DIGGING.ATTACK_MODIFIER, Db.Get().Attributes.Digging, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.PilotingSpeed = this.Create("PilotingSpeed", "Piloting Speed", DUPLICANTS.ATTRIBUTES.SPACENAVIGATION.SPEED_MODIFIER, Db.Get().Attributes.SpaceNavigation, 0.025f, 0f, formatter, DlcManager.AVAILABLE_EXPANSION1_ONLY);
			this.ImmuneLevelBoost = this.Create("ImmuneLevelBoost", "Immune Level Boost", DUPLICANTS.ATTRIBUTES.IMMUNITY.BOOST_MODIFIER, Db.Get().Attributes.Immunity, 0.0016666667f, 0f, new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.PerCycle), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.ToiletSpeed = this.Create("ToiletSpeed", "Toilet Speed", "", Db.Get().Attributes.ToiletEfficiency, 1f, -1f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.CarryAmountFromStrength = this.Create("CarryAmountFromStrength", "Carry Amount", DUPLICANTS.ATTRIBUTES.STRENGTH.CARRYMODIFIER, Db.Get().Attributes.Strength, 40f, 0f, formatter2, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.TemperatureInsulation = this.Create("TemperatureInsulation", "Temperature Insulation", DUPLICANTS.ATTRIBUTES.INSULATION.SPEEDMODIFIER, Db.Get().Attributes.Insulation, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.SeedHarvestChance = this.Create("SeedHarvestChance", "Seed Harvest Chance", DUPLICANTS.ATTRIBUTES.BOTANIST.BONUS_SEEDS, Db.Get().Attributes.Botanist, 0.033f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.CapturableSpeed = this.Create("CapturableSpeed", "Capturable Speed", DUPLICANTS.ATTRIBUTES.RANCHING.CAPTURABLESPEED, Db.Get().Attributes.Ranching, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.GeotuningSpeed = this.Create("GeotuningSpeed", "Geotuning Speed", DUPLICANTS.ATTRIBUTES.LEARNING.GEOTUNER_SPEED_MODIFIER, Db.Get().Attributes.Learning, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.RanchingEffectDuration = this.Create("RanchingEffectDuration", "Ranching Effect Duration", DUPLICANTS.ATTRIBUTES.RANCHING.EFFECTMODIFIER, Db.Get().Attributes.Ranching, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.FarmedEffectDuration = this.Create("FarmedEffectDuration", "Farmer's Touch Duration", DUPLICANTS.ATTRIBUTES.BOTANIST.TINKER_EFFECT_MODIFIER, Db.Get().Attributes.Botanist, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.PowerTinkerEffectDuration = this.Create("PowerTinkerEffectDuration", "Engie's Tune-Up Effect Duration", DUPLICANTS.ATTRIBUTES.MACHINERY.TINKER_EFFECT_MODIFIER, Db.Get().Attributes.Machinery, 0.025f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x0600742E RID: 29742 RVA: 0x002C73B8 File Offset: 0x002C55B8
		public List<AttributeConverter> GetConvertersForAttribute(Klei.AI.Attribute attrib)
		{
			List<AttributeConverter> list = new List<AttributeConverter>();
			foreach (AttributeConverter attributeConverter in this.resources)
			{
				if (attributeConverter.attribute == attrib)
				{
					list.Add(attributeConverter);
				}
			}
			return list;
		}

		// Token: 0x0400500A RID: 20490
		public AttributeConverter MovementSpeed;

		// Token: 0x0400500B RID: 20491
		public AttributeConverter ConstructionSpeed;

		// Token: 0x0400500C RID: 20492
		public AttributeConverter DiggingSpeed;

		// Token: 0x0400500D RID: 20493
		public AttributeConverter MachinerySpeed;

		// Token: 0x0400500E RID: 20494
		public AttributeConverter HarvestSpeed;

		// Token: 0x0400500F RID: 20495
		public AttributeConverter PlantTendSpeed;

		// Token: 0x04005010 RID: 20496
		public AttributeConverter CompoundingSpeed;

		// Token: 0x04005011 RID: 20497
		public AttributeConverter ResearchSpeed;

		// Token: 0x04005012 RID: 20498
		public AttributeConverter TrainingSpeed;

		// Token: 0x04005013 RID: 20499
		public AttributeConverter CookingSpeed;

		// Token: 0x04005014 RID: 20500
		public AttributeConverter ArtSpeed;

		// Token: 0x04005015 RID: 20501
		public AttributeConverter DoctorSpeed;

		// Token: 0x04005016 RID: 20502
		public AttributeConverter TidyingSpeed;

		// Token: 0x04005017 RID: 20503
		public AttributeConverter AttackDamage;

		// Token: 0x04005018 RID: 20504
		public AttributeConverter PilotingSpeed;

		// Token: 0x04005019 RID: 20505
		public AttributeConverter ImmuneLevelBoost;

		// Token: 0x0400501A RID: 20506
		public AttributeConverter ToiletSpeed;

		// Token: 0x0400501B RID: 20507
		public AttributeConverter CarryAmountFromStrength;

		// Token: 0x0400501C RID: 20508
		public AttributeConverter TemperatureInsulation;

		// Token: 0x0400501D RID: 20509
		public AttributeConverter SeedHarvestChance;

		// Token: 0x0400501E RID: 20510
		public AttributeConverter RanchingEffectDuration;

		// Token: 0x0400501F RID: 20511
		public AttributeConverter FarmedEffectDuration;

		// Token: 0x04005020 RID: 20512
		public AttributeConverter PowerTinkerEffectDuration;

		// Token: 0x04005021 RID: 20513
		public AttributeConverter CapturableSpeed;

		// Token: 0x04005022 RID: 20514
		public AttributeConverter GeotuningSpeed;
	}
}
