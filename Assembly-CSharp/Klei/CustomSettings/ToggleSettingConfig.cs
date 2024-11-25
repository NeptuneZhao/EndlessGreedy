using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02000F34 RID: 3892
	public class ToggleSettingConfig : SettingConfig
	{
		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060077C1 RID: 30657 RVA: 0x002F709C File Offset: 0x002F529C
		// (set) Token: 0x060077C2 RID: 30658 RVA: 0x002F70A4 File Offset: 0x002F52A4
		public SettingLevel on_level { get; private set; }

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060077C3 RID: 30659 RVA: 0x002F70AD File Offset: 0x002F52AD
		// (set) Token: 0x060077C4 RID: 30660 RVA: 0x002F70B5 File Offset: 0x002F52B5
		public SettingLevel off_level { get; private set; }

		// Token: 0x060077C5 RID: 30661 RVA: 0x002F70C0 File Offset: 0x002F52C0
		public ToggleSettingConfig(string id, string label, string tooltip, SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "") : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, false)
		{
			this.off_level = off_level;
			this.on_level = on_level;
		}

		// Token: 0x060077C6 RID: 30662 RVA: 0x002F70F5 File Offset: 0x002F52F5
		public void StompLevels(SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id)
		{
			this.off_level = off_level;
			this.on_level = on_level;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x060077C7 RID: 30663 RVA: 0x002F7114 File Offset: 0x002F5314
		public override SettingLevel GetLevel(string level_id)
		{
			if (this.on_level.id == level_id)
			{
				return this.on_level;
			}
			if (this.off_level.id == level_id)
			{
				return this.off_level;
			}
			if (this.default_level_id == this.on_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.on_level;
			}
			if (this.default_level_id == this.off_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.off_level;
			}
			Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x060077C8 RID: 30664 RVA: 0x002F7219 File Offset: 0x002F5419
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>
			{
				this.off_level,
				this.on_level
			};
		}

		// Token: 0x060077C9 RID: 30665 RVA: 0x002F7238 File Offset: 0x002F5438
		public string ToggleSettingLevelID(string current_id)
		{
			if (this.on_level.id == current_id)
			{
				return this.off_level.id;
			}
			return this.on_level.id;
		}

		// Token: 0x060077CA RID: 30666 RVA: 0x002F7264 File Offset: 0x002F5464
		public bool IsOnLevel(string level_id)
		{
			return level_id == this.on_level.id;
		}
	}
}
