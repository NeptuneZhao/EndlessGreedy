using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BAD RID: 2989
public class PrioritizeToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005A83 RID: 23171 RVA: 0x0020D440 File Offset: 0x0020B640
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		if (ToolMenu.Instance.PriorityScreen == null)
		{
			return;
		}
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		base.DrawTitle(instance, hoverTextDrawer);
		base.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		hoverTextDrawer.NewLine(26);
		hoverTextDrawer.DrawText(string.Format(UI.TOOLS.PRIORITIZE.SPECIFIC_PRIORITY, ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority().priority_value.ToString()), this.Styles_Title.Standard);
		string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
		if (lastEnabledFilter != null && lastEnabledFilter != "ALL")
		{
			this.ConfigureTitle(instance);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x06005A84 RID: 23172 RVA: 0x0020D538 File Offset: 0x0020B738
	protected override void ConfigureTitle(HoverTextScreen screen)
	{
		string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
		if (string.IsNullOrEmpty(this.ToolName) || lastEnabledFilter == "ALL")
		{
			this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
		}
		if (lastEnabledFilter != null && lastEnabledFilter != "ALL")
		{
			this.ToolName = string.Format(UI.TOOLS.CAPITALS, Strings.Get(this.ToolNameStringKey) + string.Format(UI.TOOLS.FILTER_HOVERCARD_HEADER, Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + lastEnabledFilter + ".TOOLTIP")));
		}
	}
}
