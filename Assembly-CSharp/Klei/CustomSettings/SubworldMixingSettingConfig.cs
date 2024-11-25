using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02000F39 RID: 3897
	public class SubworldMixingSettingConfig : MixingSettingConfig
	{
		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x060077DF RID: 30687 RVA: 0x002F7574 File Offset: 0x002F5774
		public override string label
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedSubworldMixingSetting.name, out entry))
				{
					return cachedSubworldMixingSetting.name;
				}
				return entry;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x060077E0 RID: 30688 RVA: 0x002F75AC File Offset: 0x002F57AC
		public override string tooltip
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedSubworldMixingSetting.description, out entry))
				{
					return cachedSubworldMixingSetting.description;
				}
				return entry;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x060077E1 RID: 30689 RVA: 0x002F75E4 File Offset: 0x002F57E4
		public override Sprite icon
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				Sprite sprite = (cachedSubworldMixingSetting.icon != null) ? Assets.GetSprite(cachedSubworldMixingSetting.icon) : null;
				if (sprite == null)
				{
					sprite = Assets.GetSprite("unknown");
				}
				return sprite;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060077E2 RID: 30690 RVA: 0x002F7633 File Offset: 0x002F5833
		public override List<string> forbiddenClusterTags
		{
			get
			{
				return SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath).forbiddenClusterTags;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060077E3 RID: 30691 RVA: 0x002F7645 File Offset: 0x002F5845
		public override bool isModded
		{
			get
			{
				return SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath).isModded;
			}
		}

		// Token: 0x060077E4 RID: 30692 RVA: 0x002F7658 File Offset: 0x002F5858
		public SubworldMixingSettingConfig(string id, string worldgenPath, string[] required_content = null, string dlcIdFrom = null, bool triggers_custom_game = true, long coordinate_range = 5L) : base(id, null, null, null, worldgenPath, coordinate_range, false, triggers_custom_game, required_content, "", false)
		{
			this.dlcIdFrom = dlcIdFrom;
			List<SettingLevel> levels = new List<SettingLevel>
			{
				new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP_BASEGAME, 0L, null),
				new SettingLevel("TryMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP_BASEGAME, 1L, null),
				new SettingLevel("GuranteeMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP_BASEGAME, 2L, null)
			};
			base.StompLevels(levels, "Disabled", "Disabled");
		}

		// Token: 0x040059A0 RID: 22944
		private const int COORDINATE_RANGE = 5;

		// Token: 0x040059A1 RID: 22945
		public const string DisabledLevelId = "Disabled";

		// Token: 0x040059A2 RID: 22946
		public const string TryMixingLevelId = "TryMixing";

		// Token: 0x040059A3 RID: 22947
		public const string GuaranteeMixingLevelId = "GuranteeMixing";
	}
}
