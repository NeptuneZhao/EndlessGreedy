using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BA3 RID: 2979
public class MopToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005A47 RID: 23111 RVA: 0x0020BDEC File Offset: 0x00209FEC
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
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
			if (element.IsLiquid)
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
			}
		}
		else
		{
			hoverTextDrawer.DrawIcon(instance.GetSprite("iconWarning"), 18);
			hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.UNKNOWN.ToString().ToUpper(), this.Styles_BodyText.Standard);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04003B58 RID: 15192
	private MopToolHoverTextCard.HoverScreenFields hoverScreenElements;

	// Token: 0x02001C17 RID: 7191
	private struct HoverScreenFields
	{
		// Token: 0x040081CF RID: 33231
		public GameObject UnknownAreaLine;

		// Token: 0x040081D0 RID: 33232
		public Image ElementStateIcon;

		// Token: 0x040081D1 RID: 33233
		public LocText ElementCategory;

		// Token: 0x040081D2 RID: 33234
		public LocText ElementName;

		// Token: 0x040081D3 RID: 33235
		public LocText[] ElementMass;

		// Token: 0x040081D4 RID: 33236
		public LocText ElementHardness;

		// Token: 0x040081D5 RID: 33237
		public LocText ElementHardnessDescription;
	}
}
