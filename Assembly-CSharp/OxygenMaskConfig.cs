using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class OxygenMaskConfig : IEquipmentConfig
{
	// Token: 0x0600029A RID: 666 RVA: 0x00012CF1 File Offset: 0x00010EF1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00012CF8 File Offset: 0x00010EF8
	public EquipmentDef CreateEquipmentDef()
	{
		List<AttributeModifier> list = new List<AttributeModifier>();
		list.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float)TUNING.EQUIPMENT.SUITS.OXYGEN_MASK_ATHLETICS, STRINGS.EQUIPMENT.PREFABS.OXYGEN_MASK.NAME, false, false, true));
		this.expertAthleticsModifier = new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float)(-(float)TUNING.EQUIPMENT.SUITS.OXYGEN_MASK_ATHLETICS), Db.Get().Skills.Suits1.Name, false, false, true);
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Oxygen_Mask", TUNING.EQUIPMENT.SUITS.SLOT, SimHashes.Dirt, 15f, "oxygen_mask_kanim", "mask_oxygen", "", 6, list, null, false, EntityTemplates.CollisionShape.CIRCLE, 0.325f, 0.325f, new Tag[]
		{
			GameTags.Suit,
			GameTags.Clothes
		}, null);
		equipmentDef.wornID = "Worn_Oxygen_Mask";
		equipmentDef.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.OXYGEN_MASK.RECIPE_DESC;
		equipmentDef.OnEquipCallBack = delegate(Equippable eq)
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				Navigator component = targetGameObject.GetComponent<Navigator>();
				if (component != null)
				{
					component.SetFlags(PathFinder.PotentialPath.Flags.HasOxygenMask);
				}
				MinionResume component2 = targetGameObject.GetComponent<MinionResume>();
				if (component2 != null && component2.HasPerk(Db.Get().SkillPerks.ExosuitExpertise.Id))
				{
					targetGameObject.GetAttributes().Get(Db.Get().Attributes.Athletics).Add(this.expertAthleticsModifier);
				}
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
							component.ClearFlags(PathFinder.PotentialPath.Flags.HasOxygenMask);
						}
					}
				}
			}
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Oxygen_Mask");
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Helmet");
		return equipmentDef;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00012E10 File Offset: 0x00011010
	public void DoPostConfigure(GameObject go)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage.showInUI = true;
		SuitTank suitTank = go.AddComponent<SuitTank>();
		suitTank.element = "Oxygen";
		suitTank.capacity = 20f;
		suitTank.elementTag = GameTags.Breathable;
		Durability durability = go.AddComponent<Durability>();
		durability.wornEquipmentPrefabID = "Worn_Oxygen_Mask";
		durability.durabilityLossPerCycle = TUNING.EQUIPMENT.SUITS.OXYGEN_MASK_DECAY;
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Clothes, false);
		component.AddTag(GameTags.PedestalDisplayable, false);
		go.AddComponent<SuitDiseaseHandler>();
	}

	// Token: 0x04000184 RID: 388
	public const string ID = "Oxygen_Mask";

	// Token: 0x04000185 RID: 389
	public const string WORN_ID = "Worn_Oxygen_Mask";

	// Token: 0x04000186 RID: 390
	private const PathFinder.PotentialPath.Flags suit_flags = PathFinder.PotentialPath.Flags.HasOxygenMask;

	// Token: 0x04000187 RID: 391
	private AttributeModifier expertAthleticsModifier;
}
