using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02000F32 RID: 3890
	public abstract class SettingConfig
	{
		// Token: 0x0600779F RID: 30623 RVA: 0x002F6CF8 File Offset: 0x002F4EF8
		public SettingConfig(string id, string label, string tooltip, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
			this.coordinate_range = coordinate_range;
			this.debug_only = debug_only;
			this.triggers_custom_game = triggers_custom_game;
			this.required_content = required_content;
			this.missing_content_default = missing_content_default;
			this.hide_in_ui = hide_in_ui;
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x060077A0 RID: 30624 RVA: 0x002F6D60 File Offset: 0x002F4F60
		// (set) Token: 0x060077A1 RID: 30625 RVA: 0x002F6D68 File Offset: 0x002F4F68
		public string id { get; private set; }

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x060077A2 RID: 30626 RVA: 0x002F6D71 File Offset: 0x002F4F71
		// (set) Token: 0x060077A3 RID: 30627 RVA: 0x002F6D79 File Offset: 0x002F4F79
		public virtual string label { get; private set; }

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x060077A4 RID: 30628 RVA: 0x002F6D82 File Offset: 0x002F4F82
		// (set) Token: 0x060077A5 RID: 30629 RVA: 0x002F6D8A File Offset: 0x002F4F8A
		public virtual string tooltip { get; private set; }

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x060077A6 RID: 30630 RVA: 0x002F6D93 File Offset: 0x002F4F93
		// (set) Token: 0x060077A7 RID: 30631 RVA: 0x002F6D9B File Offset: 0x002F4F9B
		public long coordinate_range { get; protected set; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060077A8 RID: 30632 RVA: 0x002F6DA4 File Offset: 0x002F4FA4
		// (set) Token: 0x060077A9 RID: 30633 RVA: 0x002F6DAC File Offset: 0x002F4FAC
		public string[] required_content { get; private set; }

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x060077AA RID: 30634 RVA: 0x002F6DB5 File Offset: 0x002F4FB5
		// (set) Token: 0x060077AB RID: 30635 RVA: 0x002F6DBD File Offset: 0x002F4FBD
		public string missing_content_default { get; private set; }

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x060077AC RID: 30636 RVA: 0x002F6DC6 File Offset: 0x002F4FC6
		// (set) Token: 0x060077AD RID: 30637 RVA: 0x002F6DCE File Offset: 0x002F4FCE
		public bool triggers_custom_game { get; protected set; }

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x060077AE RID: 30638 RVA: 0x002F6DD7 File Offset: 0x002F4FD7
		// (set) Token: 0x060077AF RID: 30639 RVA: 0x002F6DDF File Offset: 0x002F4FDF
		public bool debug_only { get; protected set; }

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060077B0 RID: 30640 RVA: 0x002F6DE8 File Offset: 0x002F4FE8
		// (set) Token: 0x060077B1 RID: 30641 RVA: 0x002F6DF0 File Offset: 0x002F4FF0
		public bool hide_in_ui { get; protected set; }

		// Token: 0x060077B2 RID: 30642
		public abstract SettingLevel GetLevel(string level_id);

		// Token: 0x060077B3 RID: 30643
		public abstract List<SettingLevel> GetLevels();

		// Token: 0x060077B4 RID: 30644 RVA: 0x002F6DF9 File Offset: 0x002F4FF9
		public bool IsDefaultLevel(string level_id)
		{
			return level_id == this.default_level_id;
		}

		// Token: 0x060077B5 RID: 30645 RVA: 0x002F6E07 File Offset: 0x002F5007
		public bool ShowInUI()
		{
			return !this.deprecated && !this.hide_in_ui && (!this.debug_only || DebugHandler.enabled) && DlcManager.IsAllContentSubscribed(this.required_content);
		}

		// Token: 0x060077B6 RID: 30646 RVA: 0x002F6E3A File Offset: 0x002F503A
		public string GetDefaultLevelId()
		{
			if (!DlcManager.IsAllContentSubscribed(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.default_level_id;
		}

		// Token: 0x060077B7 RID: 30647 RVA: 0x002F6E63 File Offset: 0x002F5063
		public string GetNoSweatDefaultLevelId()
		{
			if (!DlcManager.IsAllContentSubscribed(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.nosweat_default_level_id;
		}

		// Token: 0x04005987 RID: 22919
		protected string default_level_id;

		// Token: 0x04005988 RID: 22920
		protected string nosweat_default_level_id;

		// Token: 0x0400598F RID: 22927
		public bool deprecated;
	}
}
