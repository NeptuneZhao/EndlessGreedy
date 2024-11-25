using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C15 RID: 3093
public class CodexElementCategoryList : CodexCollapsibleHeader
{
	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x06005EC3 RID: 24259 RVA: 0x00233462 File Offset: 0x00231662
	// (set) Token: 0x06005EC4 RID: 24260 RVA: 0x0023346A File Offset: 0x0023166A
	public Tag categoryTag { get; set; }

	// Token: 0x06005EC5 RID: 24261 RVA: 0x00233473 File Offset: 0x00231673
	public CodexElementCategoryList() : base(UI.CODEX.CATEGORYNAMES.ELEMENTS, null)
	{
	}

	// Token: 0x06005EC6 RID: 24262 RVA: 0x00233494 File Offset: 0x00231694
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		base.ContentsGameObject = component.GetReference<RectTransform>("ContentContainer").gameObject;
		base.Configure(contentGameObject, displayPane, textStyles);
		Component reference = component.GetReference<RectTransform>("HeaderLabel");
		RectTransform reference2 = component.GetReference<RectTransform>("PrefabLabelWithIcon");
		this.ClearPanel(reference2.transform.parent, reference2);
		reference.GetComponent<LocText>().SetText(UI.CODEX.CATEGORYNAMES.ELEMENTS);
		foreach (Element element in ElementLoader.elements)
		{
			if (element.HasTag(this.categoryTag) && !element.disabled)
			{
				GameObject gameObject = Util.KInstantiateUI(reference2.gameObject, reference2.parent.gameObject, true);
				Image componentInChildren = gameObject.GetComponentInChildren<Image>();
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
				componentInChildren.sprite = uisprite.first;
				componentInChildren.color = uisprite.second;
				gameObject.GetComponentInChildren<LocText>().SetText(element.tag.ProperName());
				this.rows.Add(gameObject);
			}
		}
	}

	// Token: 0x06005EC7 RID: 24263 RVA: 0x002335CC File Offset: 0x002317CC
	private void ClearPanel(Transform containerToClear, Transform skipDestroyingPrefab)
	{
		skipDestroyingPrefab.SetAsFirstSibling();
		for (int i = containerToClear.childCount - 1; i >= 1; i--)
		{
			UnityEngine.Object.Destroy(containerToClear.GetChild(i).gameObject);
		}
		for (int j = this.rows.Count - 1; j >= 0; j--)
		{
			UnityEngine.Object.Destroy(this.rows[j].gameObject);
		}
		this.rows.Clear();
	}

	// Token: 0x04003F59 RID: 16217
	private List<GameObject> rows = new List<GameObject>();
}
