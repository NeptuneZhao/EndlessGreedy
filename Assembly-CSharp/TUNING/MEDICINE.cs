using System;

namespace TUNING
{
	// Token: 0x02000EF8 RID: 3832
	public class MEDICINE
	{
		// Token: 0x0400578E RID: 22414
		public const float DEFAULT_MASS = 1f;

		// Token: 0x0400578F RID: 22415
		public const float RECUPERATION_DISEASE_MULTIPLIER = 1.1f;

		// Token: 0x04005790 RID: 22416
		public const float RECUPERATION_DOCTORED_DISEASE_MULTIPLIER = 1.2f;

		// Token: 0x04005791 RID: 22417
		public const float WORK_TIME = 10f;

		// Token: 0x04005792 RID: 22418
		public static readonly MedicineInfo BASICBOOSTER = new MedicineInfo("BasicBooster", "Medicine_BasicBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04005793 RID: 22419
		public static readonly MedicineInfo INTERMEDIATEBOOSTER = new MedicineInfo("IntermediateBooster", "Medicine_IntermediateBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04005794 RID: 22420
		public static readonly MedicineInfo BASICCURE = new MedicineInfo("BasicCure", null, MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"FoodSickness"
		});

		// Token: 0x04005795 RID: 22421
		public static readonly MedicineInfo ANTIHISTAMINE = new MedicineInfo("Antihistamine", "HistamineSuppression", MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"Allergies"
		});

		// Token: 0x04005796 RID: 22422
		public static readonly MedicineInfo INTERMEDIATECURE = new MedicineInfo("IntermediateCure", null, MedicineInfo.MedicineType.CureSpecific, "DoctorStation", new string[]
		{
			"SlimeSickness"
		});

		// Token: 0x04005797 RID: 22423
		public static readonly MedicineInfo ADVANCEDCURE = new MedicineInfo("AdvancedCure", null, MedicineInfo.MedicineType.CureSpecific, "AdvancedDoctorStation", new string[]
		{
			"ZombieSickness"
		});

		// Token: 0x04005798 RID: 22424
		public static readonly MedicineInfo BASICRADPILL = new MedicineInfo("BasicRadPill", "Medicine_BasicRadPill", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04005799 RID: 22425
		public static readonly MedicineInfo INTERMEDIATERADPILL = new MedicineInfo("IntermediateRadPill", "Medicine_IntermediateRadPill", MedicineInfo.MedicineType.Booster, "AdvancedDoctorStation", null);
	}
}
