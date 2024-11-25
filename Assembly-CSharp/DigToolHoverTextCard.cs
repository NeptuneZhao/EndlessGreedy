using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B91 RID: 2961
public class DigToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x0600595E RID: 22878 RVA: 0x00205434 File Offset: 0x00203634
	public override void UpdateHoverElements(List<KSelectable> selected)
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
		if (Grid.IsVisible(num))
		{
			base.DrawTitle(instance, hoverTextDrawer);
			base.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
			Element element = Grid.Element[num];
			bool flag = false;
			if (Grid.Solid[num] && Diggable.IsDiggable(num))
			{
				flag = true;
			}
			if (flag)
			{
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawText(element.nameUpperCase, this.Styles_Title.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				hoverTextDrawer.DrawText(element.GetMaterialCategoryTag().ProperName(), this.Styles_BodyText.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				string[] array = HoverTextHelper.MassStringsReadOnly(num);
				hoverTextDrawer.DrawText(array[0], this.Styles_Values.Property.Standard);
				hoverTextDrawer.DrawText(array[1], this.Styles_Values.Property_Decimal.Standard);
				hoverTextDrawer.DrawText(array[2], this.Styles_Values.Property.Standard);
				hoverTextDrawer.DrawText(array[3], this.Styles_Values.Property.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				hoverTextDrawer.DrawText(GameUtil.GetHardnessString(Grid.Element[num], true), this.Styles_BodyText.Standard);
			}
		}
		else
		{
			hoverTextDrawer.DrawIcon(instance.GetSprite("iconWarning"), 18);
			hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.UNKNOWN, this.Styles_BodyText.Standard);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04003ABC RID: 15036
	private DigToolHoverTextCard.HoverScreenFields hoverScreenElements;

	// Token: 0x02001BF9 RID: 7161
	private struct HoverScreenFields
	{
		// Token: 0x04008151 RID: 33105
		public GameObject UnknownAreaLine;

		// Token: 0x04008152 RID: 33106
		public Image ElementStateIcon;

		// Token: 0x04008153 RID: 33107
		public LocText ElementCategory;

		// Token: 0x04008154 RID: 33108
		public LocText ElementName;

		// Token: 0x04008155 RID: 33109
		public LocText[] ElementMass;

		// Token: 0x04008156 RID: 33110
		public LocText ElementHardness;

		// Token: 0x04008157 RID: 33111
		public LocText ElementHardnessDescription;
	}
}
