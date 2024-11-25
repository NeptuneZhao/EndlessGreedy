using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02000F35 RID: 3893
	public class SeedSettingConfig : SettingConfig
	{
		// Token: 0x060077CB RID: 30667 RVA: 0x002F7278 File Offset: 0x002F5478
		public SeedSettingConfig(string id, string label, string tooltip, bool debug_only = false, bool triggers_custom_game = true) : base(id, label, tooltip, "", "", -1L, debug_only, triggers_custom_game, null, "", false)
		{
		}

		// Token: 0x060077CC RID: 30668 RVA: 0x002F72A5 File Offset: 0x002F54A5
		public override SettingLevel GetLevel(string level_id)
		{
			return new SettingLevel(level_id, level_id, level_id, 0L, null);
		}

		// Token: 0x060077CD RID: 30669 RVA: 0x002F72B2 File Offset: 0x002F54B2
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>();
		}
	}
}
