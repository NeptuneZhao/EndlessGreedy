using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CE8 RID: 3304
public class MotdBox : KMonoBehaviour
{
	// Token: 0x06006665 RID: 26213 RVA: 0x0026401C File Offset: 0x0026221C
	public void Config(MotdBox.PageData[] data)
	{
		this.pageDatas = data;
		if (this.pageButtons != null)
		{
			for (int i = this.pageButtons.Length - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.pageButtons[i]);
			}
			this.pageButtons = null;
		}
		this.pageButtons = new GameObject[data.Length];
		for (int j = 0; j < this.pageButtons.Length; j++)
		{
			int idx = j;
			GameObject gameObject = Util.KInstantiateUI(this.pageCarouselButtonPrefab, this.pageCarouselContainer, false);
			gameObject.SetActive(true);
			this.pageButtons[j] = gameObject;
			MultiToggle component = gameObject.GetComponent<MultiToggle>();
			component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
			{
				this.SwitchPage(idx);
			}));
		}
		this.SwitchPage(0);
	}

	// Token: 0x06006666 RID: 26214 RVA: 0x002640E8 File Offset: 0x002622E8
	private void SwitchPage(int newPage)
	{
		this.selectedPage = newPage;
		for (int i = 0; i < this.pageButtons.Length; i++)
		{
			this.pageButtons[i].GetComponent<MultiToggle>().ChangeState((i == this.selectedPage) ? 1 : 0);
		}
		this.image.texture = this.pageDatas[newPage].Texture;
		this.headerLabel.SetText(this.pageDatas[newPage].HeaderText);
		this.urlOpener.SetURL(this.pageDatas[newPage].URL);
		if (string.IsNullOrEmpty(this.pageDatas[newPage].ImageText))
		{
			this.imageLabel.gameObject.SetActive(false);
			this.imageLabel.SetText("");
			return;
		}
		this.imageLabel.gameObject.SetActive(true);
		this.imageLabel.SetText(this.pageDatas[newPage].ImageText);
	}

	// Token: 0x0400450F RID: 17679
	[SerializeField]
	private GameObject pageCarouselContainer;

	// Token: 0x04004510 RID: 17680
	[SerializeField]
	private GameObject pageCarouselButtonPrefab;

	// Token: 0x04004511 RID: 17681
	[SerializeField]
	private RawImage image;

	// Token: 0x04004512 RID: 17682
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04004513 RID: 17683
	[SerializeField]
	private LocText imageLabel;

	// Token: 0x04004514 RID: 17684
	[SerializeField]
	private URLOpenFunction urlOpener;

	// Token: 0x04004515 RID: 17685
	private int selectedPage;

	// Token: 0x04004516 RID: 17686
	private GameObject[] pageButtons;

	// Token: 0x04004517 RID: 17687
	private MotdBox.PageData[] pageDatas;

	// Token: 0x02001DF1 RID: 7665
	public class PageData
	{
		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x0600AA1E RID: 43550 RVA: 0x003A0E6B File Offset: 0x0039F06B
		// (set) Token: 0x0600AA1F RID: 43551 RVA: 0x003A0E73 File Offset: 0x0039F073
		public Texture2D Texture { get; set; }

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x0600AA20 RID: 43552 RVA: 0x003A0E7C File Offset: 0x0039F07C
		// (set) Token: 0x0600AA21 RID: 43553 RVA: 0x003A0E84 File Offset: 0x0039F084
		public string HeaderText { get; set; }

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600AA22 RID: 43554 RVA: 0x003A0E8D File Offset: 0x0039F08D
		// (set) Token: 0x0600AA23 RID: 43555 RVA: 0x003A0E95 File Offset: 0x0039F095
		public string ImageText { get; set; }

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600AA24 RID: 43556 RVA: 0x003A0E9E File Offset: 0x0039F09E
		// (set) Token: 0x0600AA25 RID: 43557 RVA: 0x003A0EA6 File Offset: 0x0039F0A6
		public string URL { get; set; }
	}
}
