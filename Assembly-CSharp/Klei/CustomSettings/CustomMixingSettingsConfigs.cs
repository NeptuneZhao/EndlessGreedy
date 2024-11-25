using System;
using STRINGS;

namespace Klei.CustomSettings
{
	// Token: 0x02000F3A RID: 3898
	public static class CustomMixingSettingsConfigs
	{
		// Token: 0x040059A4 RID: 22948
		public static SettingConfig DLC2Mixing = new DlcMixingSettingConfig("DLC2_ID", UI.DLC2.NAME, UI.DLC2.MIXING_TOOLTIP, 5L, false, DlcManager.DLC2, "DLC2_ID", "");

		// Token: 0x040059A5 RID: 22949
		public static SettingConfig DLC3Mixing = new DlcMixingSettingConfig("DLC3_ID", UI.DLC3.NAME, UI.DLC3.MIXING_TOOLTIP, 5L, false, DlcManager.DLC3, "DLC3_ID", "");

		// Token: 0x040059A6 RID: 22950
		public static SettingConfig CeresAsteroidMixing = new WorldMixingSettingConfig("CeresAsteroidMixing", "dlc2::worldMixing/CeresMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x040059A7 RID: 22951
		public static SettingConfig IceCavesMixing = new SubworldMixingSettingConfig("IceCavesMixing", "dlc2::subworldMixing/IceCavesMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x040059A8 RID: 22952
		public static SettingConfig CarrotQuarryMixing = new SubworldMixingSettingConfig("CarrotQuarryMixing", "dlc2::subworldMixing/CarrotQuarryMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x040059A9 RID: 22953
		public static SettingConfig SugarWoodsMixing = new SubworldMixingSettingConfig("SugarWoodsMixing", "dlc2::subworldMixing/SugarWoodsMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);
	}
}
