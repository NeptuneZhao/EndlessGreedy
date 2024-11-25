using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C13 RID: 3091
public class CodexVideo : CodexWidget<CodexVideo>
{
	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06005EB6 RID: 24246 RVA: 0x00233315 File Offset: 0x00231515
	// (set) Token: 0x06005EB7 RID: 24247 RVA: 0x0023331D File Offset: 0x0023151D
	public string name { get; set; }

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06005EB9 RID: 24249 RVA: 0x0023332F File Offset: 0x0023152F
	// (set) Token: 0x06005EB8 RID: 24248 RVA: 0x00233326 File Offset: 0x00231526
	public string videoName
	{
		get
		{
			return "--> " + (this.name ?? "NULL");
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06005EBA RID: 24250 RVA: 0x0023334A File Offset: 0x0023154A
	// (set) Token: 0x06005EBB RID: 24251 RVA: 0x00233352 File Offset: 0x00231552
	public string overlayName { get; set; }

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06005EBC RID: 24252 RVA: 0x0023335B File Offset: 0x0023155B
	// (set) Token: 0x06005EBD RID: 24253 RVA: 0x00233363 File Offset: 0x00231563
	public List<string> overlayTexts { get; set; }

	// Token: 0x06005EBE RID: 24254 RVA: 0x0023336C File Offset: 0x0023156C
	public void ConfigureVideo(VideoWidget videoWidget, string clipName, string overlayName = null, List<string> overlayTexts = null)
	{
		videoWidget.SetClip(Assets.GetVideo(clipName), overlayName, overlayTexts);
	}

	// Token: 0x06005EBF RID: 24255 RVA: 0x0023337D File Offset: 0x0023157D
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureVideo(contentGameObject.GetComponent<VideoWidget>(), this.name, this.overlayName, this.overlayTexts);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
