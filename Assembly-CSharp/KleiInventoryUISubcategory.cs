using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C7A RID: 3194
public class KleiInventoryUISubcategory : KMonoBehaviour
{
	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x0600624A RID: 25162 RVA: 0x0024B8A3 File Offset: 0x00249AA3
	public bool IsOpen
	{
		get
		{
			return this.stateExpanded;
		}
	}

	// Token: 0x0600624B RID: 25163 RVA: 0x0024B8AB File Offset: 0x00249AAB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.expandButton.onClick = delegate()
		{
			this.ToggleOpen(!this.stateExpanded);
		};
	}

	// Token: 0x0600624C RID: 25164 RVA: 0x0024B8CA File Offset: 0x00249ACA
	public void SetIdentity(string label, Sprite icon)
	{
		this.label.SetText(label);
		this.icon.sprite = icon;
	}

	// Token: 0x0600624D RID: 25165 RVA: 0x0024B8E4 File Offset: 0x00249AE4
	public void RefreshDisplay()
	{
		foreach (GameObject gameObject in this.dummyItems)
		{
			gameObject.SetActive(false);
		}
		int num = 0;
		for (int i = 0; i < this.gridLayout.transform.childCount; i++)
		{
			if (this.gridLayout.transform.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		base.gameObject.SetActive(num != 0);
		int j = 0;
		int num2 = num % this.gridLayout.constraintCount;
		if (num2 > 0)
		{
			j = this.gridLayout.constraintCount - num2;
		}
		while (j > this.dummyItems.Count)
		{
			this.dummyItems.Add(Util.KInstantiateUI(this.dummyPrefab, this.gridLayout.gameObject, false));
		}
		for (int k = 0; k < j; k++)
		{
			this.dummyItems[k].SetActive(true);
			this.dummyItems[k].transform.SetAsLastSibling();
		}
		this.headerLayout.minWidth = base.transform.parent.rectTransform().rect.width - 8f;
	}

	// Token: 0x0600624E RID: 25166 RVA: 0x0024BA44 File Offset: 0x00249C44
	public void ToggleOpen(bool open)
	{
		this.gridLayout.gameObject.SetActive(open);
		this.stateExpanded = open;
		this.expandButton.ChangeState(this.stateExpanded ? 1 : 0);
	}

	// Token: 0x040042AC RID: 17068
	[SerializeField]
	private GameObject dummyPrefab;

	// Token: 0x040042AD RID: 17069
	public string subcategoryID;

	// Token: 0x040042AE RID: 17070
	public GridLayoutGroup gridLayout;

	// Token: 0x040042AF RID: 17071
	public List<GameObject> dummyItems;

	// Token: 0x040042B0 RID: 17072
	[SerializeField]
	private LayoutElement headerLayout;

	// Token: 0x040042B1 RID: 17073
	[SerializeField]
	private Image icon;

	// Token: 0x040042B2 RID: 17074
	[SerializeField]
	private LocText label;

	// Token: 0x040042B3 RID: 17075
	[SerializeField]
	private MultiToggle expandButton;

	// Token: 0x040042B4 RID: 17076
	private bool stateExpanded = true;
}
