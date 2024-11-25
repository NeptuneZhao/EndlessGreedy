using System;
using UnityEngine;

// Token: 0x02000D08 RID: 3336
[AddComponentMenu("KMonoBehaviour/scripts/PageView")]
public class PageView : KMonoBehaviour
{
	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x060067B9 RID: 26553 RVA: 0x0026BF9E File Offset: 0x0026A19E
	public int ChildrenPerPage
	{
		get
		{
			return this.childrenPerPage;
		}
	}

	// Token: 0x060067BA RID: 26554 RVA: 0x0026BFA6 File Offset: 0x0026A1A6
	private void Update()
	{
		if (this.oldChildCount != base.transform.childCount)
		{
			this.oldChildCount = base.transform.childCount;
			this.RefreshPage();
		}
	}

	// Token: 0x060067BB RID: 26555 RVA: 0x0026BFD4 File Offset: 0x0026A1D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.nextButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.currentPage = (this.currentPage + 1) % this.pageCount;
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
		MultiToggle multiToggle2 = this.prevButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.currentPage--;
			if (this.currentPage < 0)
			{
				this.currentPage += this.pageCount;
			}
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
	}

	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x060067BC RID: 26556 RVA: 0x0026C038 File Offset: 0x0026A238
	private int pageCount
	{
		get
		{
			int num = base.transform.childCount / this.childrenPerPage;
			if (base.transform.childCount % this.childrenPerPage != 0)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x060067BD RID: 26557 RVA: 0x0026C074 File Offset: 0x0026A274
	private void RefreshPage()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (i < this.currentPage * this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else if (i >= this.currentPage * this.childrenPerPage + this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else
			{
				base.transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		this.pageLabel.SetText((this.currentPage % this.pageCount + 1).ToString() + "/" + this.pageCount.ToString());
	}

	// Token: 0x0400460B RID: 17931
	[SerializeField]
	private MultiToggle nextButton;

	// Token: 0x0400460C RID: 17932
	[SerializeField]
	private MultiToggle prevButton;

	// Token: 0x0400460D RID: 17933
	[SerializeField]
	private LocText pageLabel;

	// Token: 0x0400460E RID: 17934
	[SerializeField]
	private int childrenPerPage = 8;

	// Token: 0x0400460F RID: 17935
	private int currentPage;

	// Token: 0x04004610 RID: 17936
	private int oldChildCount;

	// Token: 0x04004611 RID: 17937
	public Action<int> OnChangePage;
}
