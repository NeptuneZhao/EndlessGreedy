﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BAB RID: 2987
public class PlaceToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005A7F RID: 23167 RVA: 0x0020D218 File Offset: 0x0020B418
	public override void UpdateHoverElements(List<KSelectable> hoverObjects)
	{
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		this.ActionName = UI.TOOLS.PLACE.TOOLACTION;
		if (this.currentPlaceable != null && this.currentPlaceable.GetProperName() != null)
		{
			this.ToolName = string.Format(UI.TOOLS.PLACE.NAME, this.currentPlaceable.GetProperName());
		}
		base.DrawTitle(instance, hoverTextDrawer);
		base.DrawInstructions(instance, hoverTextDrawer);
		int min_height = 26;
		int width = 8;
		string text;
		if (this.currentPlaceable != null && !this.currentPlaceable.IsValidPlaceLocation(num, out text))
		{
			hoverTextDrawer.NewLine(min_height);
			hoverTextDrawer.AddIndent(width);
			hoverTextDrawer.DrawText(text, this.HoverTextStyleSettings[1]);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04003B9E RID: 15262
	public Placeable currentPlaceable;
}
