using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02000F37 RID: 3895
	public class MixingSettingConfig : ListSettingConfig
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060077D1 RID: 30673 RVA: 0x002F7351 File Offset: 0x002F5551
		// (set) Token: 0x060077D2 RID: 30674 RVA: 0x002F7359 File Offset: 0x002F5559
		public string worldgenPath { get; private set; }

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060077D3 RID: 30675 RVA: 0x002F7362 File Offset: 0x002F5562
		public virtual Sprite icon { get; }

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060077D4 RID: 30676 RVA: 0x002F736A File Offset: 0x002F556A
		public virtual List<string> forbiddenClusterTags { get; }

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060077D5 RID: 30677 RVA: 0x002F7372 File Offset: 0x002F5572
		// (set) Token: 0x060077D6 RID: 30678 RVA: 0x002F737A File Offset: 0x002F557A
		public virtual string dlcIdFrom { get; protected set; }

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060077D7 RID: 30679 RVA: 0x002F7383 File Offset: 0x002F5583
		public virtual bool isModded { get; }

		// Token: 0x060077D8 RID: 30680 RVA: 0x002F738C File Offset: 0x002F558C
		protected MixingSettingConfig(string id, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, string worldgenPath, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = false, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false) : base(id, "", "", levels, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
		{
			this.worldgenPath = worldgenPath;
		}
	}
}
