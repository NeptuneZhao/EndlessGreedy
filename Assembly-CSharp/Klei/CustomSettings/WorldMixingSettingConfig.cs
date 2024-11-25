using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02000F38 RID: 3896
	public class WorldMixingSettingConfig : MixingSettingConfig
	{
		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060077D9 RID: 30681 RVA: 0x002F73C4 File Offset: 0x002F55C4
		public override string label
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedWorldMixingSetting.name, out entry))
				{
					return cachedWorldMixingSetting.name;
				}
				return entry;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060077DA RID: 30682 RVA: 0x002F73FC File Offset: 0x002F55FC
		public override string tooltip
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedWorldMixingSetting.description, out entry))
				{
					return cachedWorldMixingSetting.description;
				}
				return entry;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060077DB RID: 30683 RVA: 0x002F7434 File Offset: 0x002F5634
		public override Sprite icon
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				Sprite sprite = (cachedWorldMixingSetting.icon != null) ? ColonyDestinationAsteroidBeltData.GetUISprite(cachedWorldMixingSetting.icon) : null;
				if (sprite == null)
				{
					sprite = Assets.GetSprite(cachedWorldMixingSetting.icon);
				}
				if (sprite == null)
				{
					sprite = Assets.GetSprite("unknown");
				}
				return sprite;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060077DC RID: 30684 RVA: 0x002F7498 File Offset: 0x002F5698
		public override List<string> forbiddenClusterTags
		{
			get
			{
				return SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath).forbiddenClusterTags;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x060077DD RID: 30685 RVA: 0x002F74AA File Offset: 0x002F56AA
		public override bool isModded
		{
			get
			{
				return SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath).isModded;
			}
		}

		// Token: 0x060077DE RID: 30686 RVA: 0x002F74BC File Offset: 0x002F56BC
		public WorldMixingSettingConfig(string id, string worldgenPath, string[] required_content = null, string dlcIdFrom = null, bool triggers_custom_game = true, long coordinate_range = 5L) : base(id, null, null, null, worldgenPath, coordinate_range, false, triggers_custom_game, required_content, "", false)
		{
			this.dlcIdFrom = dlcIdFrom;
			List<SettingLevel> levels = new List<SettingLevel>
			{
				new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.TOOLTIP, 0L, null),
				new SettingLevel("TryMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP, 1L, null),
				new SettingLevel("GuranteeMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP, 2L, null)
			};
			base.StompLevels(levels, "Disabled", "Disabled");
		}

		// Token: 0x0400599C RID: 22940
		private const int COORDINATE_RANGE = 5;

		// Token: 0x0400599D RID: 22941
		public const string DisabledLevelId = "Disabled";

		// Token: 0x0400599E RID: 22942
		public const string TryMixingLevelId = "TryMixing";

		// Token: 0x0400599F RID: 22943
		public const string GuaranteeMixingLevelId = "GuranteeMixing";
	}
}
