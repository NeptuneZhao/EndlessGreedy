using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C03 RID: 3075
public class CodexText : CodexWidget<CodexText>
{
	// Token: 0x170006FF RID: 1791
	// (get) Token: 0x06005E4B RID: 24139 RVA: 0x00231155 File Offset: 0x0022F355
	// (set) Token: 0x06005E4C RID: 24140 RVA: 0x0023115D File Offset: 0x0022F35D
	public string text { get; set; }

	// Token: 0x17000700 RID: 1792
	// (get) Token: 0x06005E4D RID: 24141 RVA: 0x00231166 File Offset: 0x0022F366
	// (set) Token: 0x06005E4E RID: 24142 RVA: 0x0023116E File Offset: 0x0022F36E
	public string messageID { get; set; }

	// Token: 0x17000701 RID: 1793
	// (get) Token: 0x06005E4F RID: 24143 RVA: 0x00231177 File Offset: 0x0022F377
	// (set) Token: 0x06005E50 RID: 24144 RVA: 0x0023117F File Offset: 0x0022F37F
	public CodexTextStyle style { get; set; }

	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x06005E52 RID: 24146 RVA: 0x0023119B File Offset: 0x0022F39B
	// (set) Token: 0x06005E51 RID: 24145 RVA: 0x00231188 File Offset: 0x0022F388
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

	// Token: 0x06005E53 RID: 24147 RVA: 0x002311B6 File Offset: 0x0022F3B6
	public CodexText()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x06005E54 RID: 24148 RVA: 0x002311C5 File Offset: 0x0022F3C5
	public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body, string id = null)
	{
		this.text = text;
		this.style = style;
		if (id != null)
		{
			this.messageID = id;
		}
	}

	// Token: 0x06005E55 RID: 24149 RVA: 0x002311E8 File Offset: 0x0022F3E8
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x06005E56 RID: 24150 RVA: 0x00231234 File Offset: 0x0022F434
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
