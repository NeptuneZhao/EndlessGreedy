using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02000F33 RID: 3891
	public class ListSettingConfig : SettingConfig
	{
		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x060077B8 RID: 30648 RVA: 0x002F6E8C File Offset: 0x002F508C
		// (set) Token: 0x060077B9 RID: 30649 RVA: 0x002F6E94 File Offset: 0x002F5094
		public List<SettingLevel> levels { get; private set; }

		// Token: 0x060077BA RID: 30650 RVA: 0x002F6EA0 File Offset: 0x002F50A0
		public ListSettingConfig(string id, string label, string tooltip, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false) : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
		{
			this.levels = levels;
		}

		// Token: 0x060077BB RID: 30651 RVA: 0x002F6ECE File Offset: 0x002F50CE
		public void StompLevels(List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id)
		{
			this.levels = levels;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x060077BC RID: 30652 RVA: 0x002F6EE8 File Offset: 0x002F50E8
		public override SettingLevel GetLevel(string level_id)
		{
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == level_id)
				{
					return this.levels[i];
				}
			}
			for (int j = 0; j < this.levels.Count; j++)
			{
				if (this.levels[j].id == this.default_level_id)
				{
					return this.levels[j];
				}
			}
			global::Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x060077BD RID: 30653 RVA: 0x002F6F8E File Offset: 0x002F518E
		public override List<SettingLevel> GetLevels()
		{
			return this.levels;
		}

		// Token: 0x060077BE RID: 30654 RVA: 0x002F6F98 File Offset: 0x002F5198
		public string CycleSettingLevelID(string current_id, int direction)
		{
			string result = "";
			if (current_id == "")
			{
				current_id = this.levels[0].id;
			}
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == current_id)
				{
					int index = Mathf.Clamp(i + direction, 0, this.levels.Count - 1);
					result = this.levels[index].id;
					break;
				}
			}
			return result;
		}

		// Token: 0x060077BF RID: 30655 RVA: 0x002F7028 File Offset: 0x002F5228
		public bool IsFirstLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == 0;
		}

		// Token: 0x060077C0 RID: 30656 RVA: 0x002F705C File Offset: 0x002F525C
		public bool IsLastLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == this.levels.Count - 1;
		}
	}
}
