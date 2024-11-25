using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E88 RID: 3720
	public class TechItems : ResourceSet<TechItem>
	{
		// Token: 0x06007507 RID: 29959 RVA: 0x002DC78F File Offset: 0x002DA98F
		public TechItems(ResourceSet parent) : base("TechItems", parent)
		{
		}

		// Token: 0x06007508 RID: 29960 RVA: 0x002DC7A0 File Offset: 0x002DA9A0
		public void Init()
		{
			this.automationOverlay = this.AddTechItem("AutomationOverlay", RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_logic"), null, null, false);
			this.suitsOverlay = this.AddTechItem("SuitsOverlay", RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_suit"), null, null, false);
			this.betaResearchPoint = this.AddTechItem("BetaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_beta_icon"), null, null, false);
			this.gammaResearchPoint = this.AddTechItem("GammaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_gamma_icon"), null, null, false);
			this.orbitalResearchPoint = this.AddTechItem("OrbitalResearchPoint", RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_orbital_icon"), null, null, false);
			this.conveyorOverlay = this.AddTechItem("ConveyorOverlay", RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_conveyor"), null, null, false);
			this.jetSuit = this.AddTechItem("JetSuit", RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Jet_Suit".ToTag()), null, null, false);
			this.atmoSuit = this.AddTechItem("AtmoSuit", RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Atmo_Suit".ToTag()), null, null, false);
			this.oxygenMask = this.AddTechItem("OxygenMask", RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.NAME, RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.DESC, this.GetPrefabSpriteFnBuilder("Oxygen_Mask".ToTag()), null, null, false);
			this.deltaResearchPoint = this.AddTechItem("DeltaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_delta_icon"), DlcManager.EXPANSION1, null, false);
			this.leadSuit = this.AddTechItem("LeadSuit", RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Lead_Suit".ToTag()), DlcManager.EXPANSION1, null, false);
			this.disposableElectrobankOrganic = this.AddTechItem("DisposableElectrobank_BasicSingleHarvestPlant", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_ORGANIC.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_ORGANIC.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_BasicSingleHarvestPlant".ToTag()), DlcManager.DLC3, null, false);
			this.disposableElectrobankUraniumOre = this.AddTechItem("DisposableElectrobank_UraniumOre", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_UraniumOre".ToTag()), new string[]
			{
				"EXPANSION1_ID",
				"DLC3_ID"
			}, null, false);
			this.electrobank = this.AddTechItem("Electrobank", RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.NAME, RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("Electrobank".ToTag()), DlcManager.DLC3, null, false);
		}

		// Token: 0x06007509 RID: 29961 RVA: 0x002DCABF File Offset: 0x002DACBF
		private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName)
		{
			return (string anim, bool centered) => Assets.GetSprite(spriteName);
		}

		// Token: 0x0600750A RID: 29962 RVA: 0x002DCAD8 File Offset: 0x002DACD8
		private Func<string, bool, Sprite> GetPrefabSpriteFnBuilder(Tag prefabTag)
		{
			return (string anim, bool centered) => Def.GetUISprite(prefabTag, "ui", false).first;
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x002DCAF4 File Offset: 0x002DACF4
		[Obsolete("Used AddTechItem with requiredDlcIds and forbiddenDlcIds instead.")]
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] DLCIds, bool poi_unlock = false)
		{
			string[] requiredDlcIds;
			string[] forbiddenDlcIds;
			DlcManager.ConvertAvailableToRequireAndForbidden(DLCIds, out requiredDlcIds, out forbiddenDlcIds);
			return this.AddTechItem(id, name, description, getUISprite, requiredDlcIds, forbiddenDlcIds, poi_unlock);
		}

		// Token: 0x0600750C RID: 29964 RVA: 0x002DCB1C File Offset: 0x002DAD1C
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, bool poi_unlock = false)
		{
			if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
			{
				return null;
			}
			if (base.TryGet(id) != null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Tried adding a tech item called",
					id,
					name,
					"but it was already added!"
				});
				return base.Get(id);
			}
			Tech techFromItemID = this.GetTechFromItemID(id);
			if (techFromItemID == null)
			{
				return null;
			}
			TechItem techItem = new TechItem(id, this, name, description, getUISprite, techFromItemID.Id, requiredDlcIds, forbiddenDlcIds, poi_unlock);
			techFromItemID.unlockedItems.Add(techItem);
			return techItem;
		}

		// Token: 0x0600750D RID: 29965 RVA: 0x002DCB9C File Offset: 0x002DAD9C
		public bool IsTechItemComplete(string id)
		{
			bool result = true;
			foreach (TechItem techItem in this.resources)
			{
				if (techItem.Id == id)
				{
					result = techItem.IsComplete();
					break;
				}
			}
			return result;
		}

		// Token: 0x0600750E RID: 29966 RVA: 0x002DCC04 File Offset: 0x002DAE04
		public Tech GetTechFromItemID(string itemId)
		{
			if (Db.Get().Techs == null)
			{
				return null;
			}
			return Db.Get().Techs.TryGetTechForTechItem(itemId);
		}

		// Token: 0x0600750F RID: 29967 RVA: 0x002DCC24 File Offset: 0x002DAE24
		public int GetTechTierForItem(string itemId)
		{
			Tech techFromItemID = this.GetTechFromItemID(itemId);
			if (techFromItemID != null)
			{
				return Techs.GetTier(techFromItemID);
			}
			return 0;
		}

		// Token: 0x040054F2 RID: 21746
		public const string AUTOMATION_OVERLAY_ID = "AutomationOverlay";

		// Token: 0x040054F3 RID: 21747
		public TechItem automationOverlay;

		// Token: 0x040054F4 RID: 21748
		public const string SUITS_OVERLAY_ID = "SuitsOverlay";

		// Token: 0x040054F5 RID: 21749
		public TechItem suitsOverlay;

		// Token: 0x040054F6 RID: 21750
		public const string JET_SUIT_ID = "JetSuit";

		// Token: 0x040054F7 RID: 21751
		public TechItem jetSuit;

		// Token: 0x040054F8 RID: 21752
		public const string ATMO_SUIT_ID = "AtmoSuit";

		// Token: 0x040054F9 RID: 21753
		public TechItem atmoSuit;

		// Token: 0x040054FA RID: 21754
		public const string OXYGEN_MASK_ID = "OxygenMask";

		// Token: 0x040054FB RID: 21755
		public TechItem oxygenMask;

		// Token: 0x040054FC RID: 21756
		public const string LEAD_SUIT_ID = "LeadSuit";

		// Token: 0x040054FD RID: 21757
		public TechItem leadSuit;

		// Token: 0x040054FE RID: 21758
		public TechItem disposableElectrobankOrganic;

		// Token: 0x040054FF RID: 21759
		public TechItem disposableElectrobankUraniumOre;

		// Token: 0x04005500 RID: 21760
		public TechItem electrobank;

		// Token: 0x04005501 RID: 21761
		public const string BETA_RESEARCH_POINT_ID = "BetaResearchPoint";

		// Token: 0x04005502 RID: 21762
		public TechItem betaResearchPoint;

		// Token: 0x04005503 RID: 21763
		public const string GAMMA_RESEARCH_POINT_ID = "GammaResearchPoint";

		// Token: 0x04005504 RID: 21764
		public TechItem gammaResearchPoint;

		// Token: 0x04005505 RID: 21765
		public const string DELTA_RESEARCH_POINT_ID = "DeltaResearchPoint";

		// Token: 0x04005506 RID: 21766
		public TechItem deltaResearchPoint;

		// Token: 0x04005507 RID: 21767
		public const string ORBITAL_RESEARCH_POINT_ID = "OrbitalResearchPoint";

		// Token: 0x04005508 RID: 21768
		public TechItem orbitalResearchPoint;

		// Token: 0x04005509 RID: 21769
		public const string CONVEYOR_OVERLAY_ID = "ConveyorOverlay";

		// Token: 0x0400550A RID: 21770
		public TechItem conveyorOverlay;
	}
}
