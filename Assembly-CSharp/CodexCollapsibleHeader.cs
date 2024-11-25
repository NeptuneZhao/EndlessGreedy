using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C12 RID: 3090
public class CodexCollapsibleHeader : CodexWidget<CodexCollapsibleHeader>
{
	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x0023322B File Offset: 0x0023142B
	// (set) Token: 0x06005EB2 RID: 24242 RVA: 0x00233252 File Offset: 0x00231452
	protected GameObject ContentsGameObject
	{
		get
		{
			if (this.contentsGameObject == null)
			{
				this.contentsGameObject = this.contents.go;
			}
			return this.contentsGameObject;
		}
		set
		{
			this.contentsGameObject = value;
		}
	}

	// Token: 0x06005EB3 RID: 24243 RVA: 0x0023325B File Offset: 0x0023145B
	public CodexCollapsibleHeader(string label, ContentContainer contents)
	{
		this.label = label;
		this.contents = contents;
	}

	// Token: 0x06005EB4 RID: 24244 RVA: 0x00233274 File Offset: 0x00231474
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("Label");
		reference.text = this.label;
		reference.textStyleSetting = textStyles[CodexTextStyle.Subtitle];
		reference.ApplySettings();
		MultiToggle reference2 = component.GetReference<MultiToggle>("ExpandToggle");
		reference2.ChangeState(1);
		reference2.onClick = delegate()
		{
			this.ToggleCategoryOpen(contentGameObject, !this.ContentsGameObject.activeSelf);
		};
	}

	// Token: 0x06005EB5 RID: 24245 RVA: 0x002332EB File Offset: 0x002314EB
	private void ToggleCategoryOpen(GameObject header, bool open)
	{
		header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
		this.ContentsGameObject.SetActive(open);
	}

	// Token: 0x04003F52 RID: 16210
	protected ContentContainer contents;

	// Token: 0x04003F53 RID: 16211
	private string label;

	// Token: 0x04003F54 RID: 16212
	private GameObject contentsGameObject;
}
