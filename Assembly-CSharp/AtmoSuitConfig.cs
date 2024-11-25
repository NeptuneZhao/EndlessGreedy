﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class AtmoSuitConfig : IEquipmentConfig
{
	// Token: 0x06000271 RID: 625 RVA: 0x0001149A File Offset: 0x0000F69A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x000114A4 File Offset: 0x0000F6A4
	public EquipmentDef CreateEquipmentDef()
	{
		List<AttributeModifier> list = new List<AttributeModifier>();
		list.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS, STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.THERMAL_CONDUCTIVITY_BARRIER, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(Db.Get().Attributes.Digging.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_DIGGING, STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
		list.Add(new AttributeModifier(Db.Get().Attributes.ScoldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCOLDING, STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
		this.expertAthleticsModifier = new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float)(-(float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS), Db.Get().Skills.Suits1.Name, false, false, true);
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Atmo_Suit", TUNING.EQUIPMENT.SUITS.SLOT, SimHashes.Dirt, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_MASS, "suit_oxygen_kanim", "", "body_oxygen_kanim", 6, list, null, true, EntityTemplates.CollisionShape.CIRCLE, 0.325f, 0.325f, new Tag[]
		{
			GameTags.Suit,
			GameTags.Clothes,
			GameTags.PedestalDisplayable,
			GameTags.AirtightSuit
		}, null);
		equipmentDef.wornID = "Worn_Atmo_Suit";
		equipmentDef.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC;
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("SoakingWet"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WetFeet"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("ColdAir"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WarmAir"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("PoppedEarDrums"));
		equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("Slipped"));
		equipmentDef.OnEquipCallBack = delegate(Equippable eq)
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				Navigator component = targetGameObject.GetComponent<Navigator>();
				if (component != null)
				{
					component.SetFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit);
				}
				MinionResume component2 = targetGameObject.GetComponent<MinionResume>();
				if (component2 != null && component2.HasPerk(Db.Get().SkillPerks.ExosuitExpertise.Id))
				{
					targetGameObject.GetAttributes().Get(Db.Get().Attributes.Athletics).Add(this.expertAthleticsModifier);
				}
				targetGameObject.AddTag(GameTags.HasAirtightSuit);
			}
		};
		equipmentDef.OnUnequipCallBack = delegate(Equippable eq)
		{
			if (eq.assignee != null)
			{
				Ownables soleOwner = eq.assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
					if (targetGameObject)
					{
						Attributes attributes = targetGameObject.GetAttributes();
						if (attributes != null)
						{
							attributes.Get(Db.Get().Attributes.Athletics).Remove(this.expertAthleticsModifier);
						}
						Navigator component = targetGameObject.GetComponent<Navigator>();
						if (component != null)
						{
							component.ClearFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit);
						}
						Effects component2 = targetGameObject.GetComponent<Effects>();
						if (component2 != null)
						{
							component2.RemoveImmunity(Db.Get().effects.Get("ColdAir"), "Atmo_Suit");
							component2.RemoveImmunity(Db.Get().effects.Get("WarmAir"), "Atmo_Suit");
							if (component2.HasEffect("SoiledSuit"))
							{
								component2.Remove("SoiledSuit");
							}
						}
						targetGameObject.RemoveTag(GameTags.HasAirtightSuit);
					}
					Tag elementTag = eq.GetComponent<SuitTank>().elementTag;
					eq.GetComponent<Storage>().DropUnlessHasTag(elementTag);
				}
			}
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Atmo_Suit");
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Helmet");
		return equipmentDef;
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0001176C File Offset: 0x0000F96C
	public void DoPostConfigure(GameObject go)
	{
		SuitTank suitTank = go.AddComponent<SuitTank>();
		suitTank.element = "Oxygen";
		suitTank.capacity = DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * 600f * 1.25f;
		suitTank.elementTag = GameTags.Breathable;
		go.AddComponent<HelmetController>();
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Clothes, false);
		component.AddTag(GameTags.PedestalDisplayable, false);
		component.AddTag(GameTags.AirtightSuit, false);
		Durability durability = go.AddComponent<Durability>();
		durability.wornEquipmentPrefabID = "Worn_Atmo_Suit";
		durability.durabilityLossPerCycle = TUNING.EQUIPMENT.SUITS.ATMOSUIT_DECAY;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage.showInUI = true;
		go.AddOrGet<AtmoSuit>();
		go.AddComponent<SuitDiseaseHandler>();
	}

	// Token: 0x04000170 RID: 368
	public const string ID = "Atmo_Suit";

	// Token: 0x04000171 RID: 369
	public const string WORN_ID = "Worn_Atmo_Suit";

	// Token: 0x04000172 RID: 370
	public static ComplexRecipe recipe;

	// Token: 0x04000173 RID: 371
	private const PathFinder.PotentialPath.Flags suit_flags = PathFinder.PotentialPath.Flags.HasAtmoSuit;

	// Token: 0x04000174 RID: 372
	private AttributeModifier expertAthleticsModifier;
}
