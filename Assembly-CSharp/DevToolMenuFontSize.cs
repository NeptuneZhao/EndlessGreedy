using System;
using ImGuiNET;

// Token: 0x02000617 RID: 1559
public class DevToolMenuFontSize
{
	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06002665 RID: 9829 RVA: 0x000D64B1 File Offset: 0x000D46B1
	// (set) Token: 0x06002664 RID: 9828 RVA: 0x000D64A8 File Offset: 0x000D46A8
	public bool initialized { get; private set; }

	// Token: 0x06002666 RID: 9830 RVA: 0x000D64BC File Offset: 0x000D46BC
	public void RefreshFontSize()
	{
		DevToolMenuFontSize.FontSizeCategory @int = (DevToolMenuFontSize.FontSizeCategory)KPlayerPrefs.GetInt("Imgui_font_size_category", 2);
		this.SetFontSizeCategory(@int);
	}

	// Token: 0x06002667 RID: 9831 RVA: 0x000D64DC File Offset: 0x000D46DC
	public void InitializeIfNeeded()
	{
		if (!this.initialized)
		{
			this.initialized = true;
			this.RefreshFontSize();
		}
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x000D64F4 File Offset: 0x000D46F4
	public void DrawMenu()
	{
		if (ImGui.BeginMenu("Settings"))
		{
			bool flag = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Fabric;
			bool flag2 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Small;
			bool flag3 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Regular;
			bool flag4 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Large;
			if (ImGui.BeginMenu("Size"))
			{
				if (ImGui.Checkbox("Original Font", ref flag) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Fabric)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Fabric);
				}
				if (ImGui.Checkbox("Small Text", ref flag2) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Small)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Small);
				}
				if (ImGui.Checkbox("Regular Text", ref flag3) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Regular)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Regular);
				}
				if (ImGui.Checkbox("Large Text", ref flag4) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Large)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Large);
				}
				ImGui.EndMenu();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x06002669 RID: 9833 RVA: 0x000D65C8 File Offset: 0x000D47C8
	public unsafe void SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory size)
	{
		this.fontSizeCategory = size;
		KPlayerPrefs.SetInt("Imgui_font_size_category", (int)size);
		ImGuiIOPtr io = ImGui.GetIO();
		if (size < (DevToolMenuFontSize.FontSizeCategory)io.Fonts.Fonts.Size)
		{
			ImFontPtr wrappedPtr = *io.Fonts.Fonts[(int)size];
			io.NativePtr->FontDefault = wrappedPtr;
		}
	}

	// Token: 0x040015DA RID: 5594
	public const string SETTINGS_KEY_FONT_SIZE_CATEGORY = "Imgui_font_size_category";

	// Token: 0x040015DB RID: 5595
	private DevToolMenuFontSize.FontSizeCategory fontSizeCategory;

	// Token: 0x020013FB RID: 5115
	public enum FontSizeCategory
	{
		// Token: 0x04006885 RID: 26757
		Fabric,
		// Token: 0x04006886 RID: 26758
		Small,
		// Token: 0x04006887 RID: 26759
		Regular,
		// Token: 0x04006888 RID: 26760
		Large
	}
}
