using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B96 RID: 2966
[AddComponentMenu("KMonoBehaviour/scripts/HoverTextConfiguration")]
public class HoverTextConfiguration : KMonoBehaviour
{
	// Token: 0x0600597E RID: 22910 RVA: 0x00205E5D File Offset: 0x0020405D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ConfigureHoverScreen();
	}

	// Token: 0x0600597F RID: 22911 RVA: 0x00205E6B File Offset: 0x0020406B
	protected virtual void ConfigureTitle(HoverTextScreen screen)
	{
		if (string.IsNullOrEmpty(this.ToolName))
		{
			this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
		}
	}

	// Token: 0x06005980 RID: 22912 RVA: 0x00205E95 File Offset: 0x00204095
	protected void DrawTitle(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		drawer.DrawText(this.ToolName, this.ToolTitleTextStyle);
	}

	// Token: 0x06005981 RID: 22913 RVA: 0x00205EAC File Offset: 0x002040AC
	protected void DrawInstructions(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		TextStyleSetting standard = this.Styles_Instruction.Standard;
		drawer.NewLine(26);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseLeft, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_left"), 20);
		}
		drawer.DrawText(this.ActionName, standard);
		drawer.AddIndent(8);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseRight, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_right"), 20);
		}
		drawer.DrawText(this.backStr, standard);
	}

	// Token: 0x06005982 RID: 22914 RVA: 0x00205F50 File Offset: 0x00204150
	public virtual void ConfigureHoverScreen()
	{
		if (!string.IsNullOrEmpty(this.ActionStringKey))
		{
			this.ActionName = Strings.Get(this.ActionStringKey);
		}
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.ConfigureTitle(instance);
		this.backStr = UI.TOOLS.GENERIC.BACK.ToString().ToUpper();
	}

	// Token: 0x06005983 RID: 22915 RVA: 0x00205FA4 File Offset: 0x002041A4
	public virtual void UpdateHoverElements(List<KSelectable> hover_objects)
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
		this.DrawTitle(instance, hoverTextDrawer);
		this.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04003ACE RID: 15054
	public TextStyleSetting[] HoverTextStyleSettings;

	// Token: 0x04003ACF RID: 15055
	public string ToolNameStringKey = "";

	// Token: 0x04003AD0 RID: 15056
	public string ActionStringKey = "";

	// Token: 0x04003AD1 RID: 15057
	[HideInInspector]
	public string ActionName = "";

	// Token: 0x04003AD2 RID: 15058
	[HideInInspector]
	public string ToolName;

	// Token: 0x04003AD3 RID: 15059
	protected string backStr;

	// Token: 0x04003AD4 RID: 15060
	public TextStyleSetting ToolTitleTextStyle;

	// Token: 0x04003AD5 RID: 15061
	public HoverTextConfiguration.TextStylePair Styles_Title;

	// Token: 0x04003AD6 RID: 15062
	public HoverTextConfiguration.TextStylePair Styles_BodyText;

	// Token: 0x04003AD7 RID: 15063
	public HoverTextConfiguration.TextStylePair Styles_Instruction;

	// Token: 0x04003AD8 RID: 15064
	public HoverTextConfiguration.TextStylePair Styles_Warning;

	// Token: 0x04003AD9 RID: 15065
	public HoverTextConfiguration.ValuePropertyTextStyles Styles_Values;

	// Token: 0x02001BFA RID: 7162
	[Serializable]
	public struct TextStylePair
	{
		// Token: 0x04008158 RID: 33112
		public TextStyleSetting Standard;

		// Token: 0x04008159 RID: 33113
		public TextStyleSetting Selected;
	}

	// Token: 0x02001BFB RID: 7163
	[Serializable]
	public struct ValuePropertyTextStyles
	{
		// Token: 0x0400815A RID: 33114
		public HoverTextConfiguration.TextStylePair Property;

		// Token: 0x0400815B RID: 33115
		public HoverTextConfiguration.TextStylePair Property_Decimal;

		// Token: 0x0400815C RID: 33116
		public HoverTextConfiguration.TextStylePair Property_Unit;
	}
}
