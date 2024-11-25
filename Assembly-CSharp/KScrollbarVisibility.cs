using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C97 RID: 3223
public class KScrollbarVisibility : MonoBehaviour
{
	// Token: 0x0600631C RID: 25372 RVA: 0x0024ED8A File Offset: 0x0024CF8A
	private void Start()
	{
		this.Update();
	}

	// Token: 0x0600631D RID: 25373 RVA: 0x0024ED94 File Offset: 0x0024CF94
	private void Update()
	{
		if (this.content.content == null)
		{
			return;
		}
		bool flag = false;
		Vector2 vector = new Vector2(this.parent.rect.width, this.parent.rect.height);
		Vector2 sizeDelta = this.content.content.GetComponent<RectTransform>().sizeDelta;
		if ((sizeDelta.x >= vector.x && this.checkWidth) || (sizeDelta.y >= vector.y && this.checkHeight))
		{
			flag = true;
		}
		if (this.scrollbar.gameObject.activeSelf != flag)
		{
			this.scrollbar.gameObject.SetActive(flag);
			if (this.others != null)
			{
				GameObject[] array = this.others;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(flag);
				}
			}
		}
	}

	// Token: 0x04004331 RID: 17201
	[SerializeField]
	private ScrollRect content;

	// Token: 0x04004332 RID: 17202
	[SerializeField]
	private RectTransform parent;

	// Token: 0x04004333 RID: 17203
	[SerializeField]
	private bool checkWidth = true;

	// Token: 0x04004334 RID: 17204
	[SerializeField]
	private bool checkHeight = true;

	// Token: 0x04004335 RID: 17205
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x04004336 RID: 17206
	[SerializeField]
	private GameObject[] others;
}
