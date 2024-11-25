using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000DA9 RID: 3497
public class SingleItemSelectionSideScreen_SelectedItemSection : KMonoBehaviour
{
	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x06006E7C RID: 28284 RVA: 0x002985C4 File Offset: 0x002967C4
	// (set) Token: 0x06006E7B RID: 28283 RVA: 0x002985BB File Offset: 0x002967BB
	public Tag Item { get; private set; }

	// Token: 0x06006E7D RID: 28285 RVA: 0x002985CC File Offset: 0x002967CC
	public void Clear()
	{
		this.SetItem(null);
	}

	// Token: 0x06006E7E RID: 28286 RVA: 0x002985DC File Offset: 0x002967DC
	public void SetItem(Tag item)
	{
		this.Item = item;
		if (this.Item != GameTags.Void)
		{
			this.SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.TITLE);
			this.SetContentText(this.Item.ProperName());
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(this.Item, "ui", false);
			this.SetImage(uisprite.first, uisprite.second);
			return;
		}
		this.SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_TITLE);
		this.SetContentText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_MESSAGE);
		this.SetImage(null, Color.white);
	}

	// Token: 0x06006E7F RID: 28287 RVA: 0x00298679 File Offset: 0x00296879
	private void SetTitleText(string text)
	{
		this.title.text = text;
	}

	// Token: 0x06006E80 RID: 28288 RVA: 0x00298687 File Offset: 0x00296887
	private void SetContentText(string text)
	{
		this.contentText.text = text;
	}

	// Token: 0x06006E81 RID: 28289 RVA: 0x00298695 File Offset: 0x00296895
	private void SetImage(Sprite sprite, Color color)
	{
		this.image.sprite = sprite;
		this.image.color = color;
		this.image.gameObject.SetActive(sprite != null);
	}

	// Token: 0x04004B5E RID: 19294
	[Header("References")]
	[SerializeField]
	private LocText title;

	// Token: 0x04004B5F RID: 19295
	[SerializeField]
	private LocText contentText;

	// Token: 0x04004B60 RID: 19296
	[SerializeField]
	private KImage image;
}
