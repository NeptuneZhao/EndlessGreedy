using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x02000E6B RID: 3691
	public class AssignableSlots : ResourceSet<AssignableSlot>
	{
		// Token: 0x060074A9 RID: 29865 RVA: 0x002D7AD4 File Offset: 0x002D5CD4
		public AssignableSlots()
		{
			this.Bed = base.Add(new OwnableSlot("Bed", MISC.TAGS.BED));
			this.MessStation = base.Add(new OwnableSlot("MessStation", MISC.TAGS.MESSSTATION));
			this.Clinic = base.Add(new OwnableSlot("Clinic", MISC.TAGS.CLINIC));
			this.MedicalBed = base.Add(new OwnableSlot("MedicalBed", MISC.TAGS.CLINIC));
			this.MedicalBed.showInUI = false;
			this.GeneShuffler = base.Add(new OwnableSlot("GeneShuffler", MISC.TAGS.GENE_SHUFFLER));
			this.GeneShuffler.showInUI = false;
			this.Toilet = base.Add(new OwnableSlot("Toilet", MISC.TAGS.TOILET));
			this.MassageTable = base.Add(new OwnableSlot("MassageTable", MISC.TAGS.MASSAGE_TABLE));
			this.RocketCommandModule = base.Add(new OwnableSlot("RocketCommandModule", MISC.TAGS.COMMAND_MODULE));
			this.HabitatModule = base.Add(new OwnableSlot("HabitatModule", MISC.TAGS.HABITAT_MODULE));
			this.ResetSkillsStation = base.Add(new OwnableSlot("ResetSkillsStation", "ResetSkillsStation"));
			this.WarpPortal = base.Add(new OwnableSlot("WarpPortal", MISC.TAGS.WARP_PORTAL));
			this.WarpPortal.showInUI = false;
			this.BionicUpgrade = base.Add(new OwnableSlot("BionicUpgrade", MISC.TAGS.BIONIC_UPGRADE));
			this.Toy = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOYS.SLOT, MISC.TAGS.TOY, false));
			this.Suit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.SUITS.SLOT, MISC.TAGS.SUIT, true));
			this.Tool = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOOLS.TOOLSLOT, MISC.TAGS.MULTITOOL, false));
			this.Outfit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.CLOTHING.SLOT, UI.StripLinkFormatting(MISC.TAGS.CLOTHES), true));
		}

		// Token: 0x0400540E RID: 21518
		public AssignableSlot Bed;

		// Token: 0x0400540F RID: 21519
		public AssignableSlot MessStation;

		// Token: 0x04005410 RID: 21520
		public AssignableSlot Clinic;

		// Token: 0x04005411 RID: 21521
		public AssignableSlot GeneShuffler;

		// Token: 0x04005412 RID: 21522
		public AssignableSlot MedicalBed;

		// Token: 0x04005413 RID: 21523
		public AssignableSlot Toilet;

		// Token: 0x04005414 RID: 21524
		public AssignableSlot MassageTable;

		// Token: 0x04005415 RID: 21525
		public AssignableSlot RocketCommandModule;

		// Token: 0x04005416 RID: 21526
		public AssignableSlot HabitatModule;

		// Token: 0x04005417 RID: 21527
		public AssignableSlot ResetSkillsStation;

		// Token: 0x04005418 RID: 21528
		public AssignableSlot WarpPortal;

		// Token: 0x04005419 RID: 21529
		public AssignableSlot Toy;

		// Token: 0x0400541A RID: 21530
		public AssignableSlot Suit;

		// Token: 0x0400541B RID: 21531
		public AssignableSlot Tool;

		// Token: 0x0400541C RID: 21532
		public AssignableSlot Outfit;

		// Token: 0x0400541D RID: 21533
		public AssignableSlot BionicUpgrade;
	}
}
