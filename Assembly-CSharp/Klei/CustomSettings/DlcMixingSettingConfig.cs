using System;
using STRINGS;

namespace Klei.CustomSettings
{
	// Token: 0x02000F36 RID: 3894
	public class DlcMixingSettingConfig : ToggleSettingConfig
	{
		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060077CE RID: 30670 RVA: 0x002F72B9 File Offset: 0x002F54B9
		// (set) Token: 0x060077CF RID: 30671 RVA: 0x002F72C1 File Offset: 0x002F54C1
		public virtual string dlcIdFrom { get; protected set; }

		// Token: 0x060077D0 RID: 30672 RVA: 0x002F72CC File Offset: 0x002F54CC
		public DlcMixingSettingConfig(string id, string label, string tooltip, long coordinate_range = 5L, bool triggers_custom_game = false, string[] required_content = null, string dlcIdFrom = null, string missing_content_default = "") : base(id, label, tooltip, null, null, null, "Disabled", coordinate_range, false, triggers_custom_game, required_content, missing_content_default)
		{
			this.dlcIdFrom = dlcIdFrom;
			SettingLevel off_level = new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.TOOLTIP, 0L, null);
			SettingLevel on_level = new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.TOOLTIP, 1L, null);
			base.StompLevels(off_level, on_level, "Disabled", "Disabled");
		}

		// Token: 0x04005993 RID: 22931
		private const int COORDINATE_RANGE = 5;

		// Token: 0x04005994 RID: 22932
		public const string DisabledLevelId = "Disabled";

		// Token: 0x04005995 RID: 22933
		public const string EnabledLevelId = "Enabled";
	}
}
