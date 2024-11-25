using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C04 RID: 3076
public class CodexTextWithTooltip : CodexWidget<CodexTextWithTooltip>
{
	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x06005E57 RID: 24151 RVA: 0x0023124A File Offset: 0x0022F44A
	// (set) Token: 0x06005E58 RID: 24152 RVA: 0x00231252 File Offset: 0x0022F452
	public string text { get; set; }

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x06005E59 RID: 24153 RVA: 0x0023125B File Offset: 0x0022F45B
	// (set) Token: 0x06005E5A RID: 24154 RVA: 0x00231263 File Offset: 0x0022F463
	public string tooltip { get; set; }

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x06005E5B RID: 24155 RVA: 0x0023126C File Offset: 0x0022F46C
	// (set) Token: 0x06005E5C RID: 24156 RVA: 0x00231274 File Offset: 0x0022F474
	public CodexTextStyle style { get; set; }

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x06005E5E RID: 24158 RVA: 0x00231290 File Offset: 0x0022F490
	// (set) Token: 0x06005E5D RID: 24157 RVA: 0x0023127D File Offset: 0x0022F47D
	public string stringKey
	{
		get
		{
			return "--> " + (this.text ?? "NULL");
		}
		set
		{
			this.text = Strings.Get(value);
		}
	}

	// Token: 0x06005E5F RID: 24159 RVA: 0x002312AB File Offset: 0x0022F4AB
	public CodexTextWithTooltip()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x06005E60 RID: 24160 RVA: 0x002312BA File Offset: 0x0022F4BA
	public CodexTextWithTooltip(string text, string tooltip, CodexTextStyle style = CodexTextStyle.Body)
	{
		this.text = text;
		this.style = style;
		this.tooltip = tooltip;
	}

	// Token: 0x06005E61 RID: 24161 RVA: 0x002312D8 File Offset: 0x0022F4D8
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x06005E62 RID: 24162 RVA: 0x00231324 File Offset: 0x0022F524
	public void ConfigureTooltip(ToolTip tooltip)
	{
		tooltip.SetSimpleTooltip(this.tooltip);
	}

	// Token: 0x06005E63 RID: 24163 RVA: 0x00231332 File Offset: 0x0022F532
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		this.ConfigureTooltip(contentGameObject.GetComponent<ToolTip>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
