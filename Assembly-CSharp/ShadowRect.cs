using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D3C RID: 3388
public class ShadowRect : MonoBehaviour
{
	// Token: 0x06006A83 RID: 27267 RVA: 0x00281DFC File Offset: 0x0027FFFC
	private void OnEnable()
	{
		if (this.RectShadow != null)
		{
			this.RectShadow.name = "Shadow_" + this.RectMain.name;
			this.MatchRect();
			return;
		}
		global::Debug.LogWarning("Shadowrect is missing rectshadow: " + base.gameObject.name);
	}

	// Token: 0x06006A84 RID: 27268 RVA: 0x00281E58 File Offset: 0x00280058
	private void Update()
	{
		this.MatchRect();
	}

	// Token: 0x06006A85 RID: 27269 RVA: 0x00281E60 File Offset: 0x00280060
	protected virtual void MatchRect()
	{
		if (this.RectShadow == null || this.RectMain == null)
		{
			return;
		}
		if (this.shadowLayoutElement == null)
		{
			this.shadowLayoutElement = this.RectShadow.GetComponent<LayoutElement>();
		}
		if (this.shadowLayoutElement != null && !this.shadowLayoutElement.ignoreLayout)
		{
			this.shadowLayoutElement.ignoreLayout = true;
		}
		if (this.RectShadow.transform.parent != this.RectMain.transform.parent)
		{
			this.RectShadow.transform.SetParent(this.RectMain.transform.parent);
		}
		if (this.RectShadow.GetSiblingIndex() >= this.RectMain.GetSiblingIndex())
		{
			this.RectShadow.SetAsFirstSibling();
		}
		this.RectShadow.transform.localScale = Vector3.one;
		if (this.RectShadow.pivot != this.RectMain.pivot)
		{
			this.RectShadow.pivot = this.RectMain.pivot;
		}
		if (this.RectShadow.anchorMax != this.RectMain.anchorMax)
		{
			this.RectShadow.anchorMax = this.RectMain.anchorMax;
		}
		if (this.RectShadow.anchorMin != this.RectMain.anchorMin)
		{
			this.RectShadow.anchorMin = this.RectMain.anchorMin;
		}
		if (this.RectShadow.sizeDelta != this.RectMain.sizeDelta)
		{
			this.RectShadow.sizeDelta = this.RectMain.sizeDelta;
		}
		if (this.RectShadow.anchoredPosition != this.RectMain.anchoredPosition + this.ShadowOffset)
		{
			this.RectShadow.anchoredPosition = this.RectMain.anchoredPosition + this.ShadowOffset;
		}
		if (this.RectMain.gameObject.activeInHierarchy != this.RectShadow.gameObject.activeInHierarchy)
		{
			this.RectShadow.gameObject.SetActive(this.RectMain.gameObject.activeInHierarchy);
		}
	}

	// Token: 0x04004898 RID: 18584
	public RectTransform RectMain;

	// Token: 0x04004899 RID: 18585
	public RectTransform RectShadow;

	// Token: 0x0400489A RID: 18586
	[SerializeField]
	protected Color shadowColor = new Color(0f, 0f, 0f, 0.6f);

	// Token: 0x0400489B RID: 18587
	[SerializeField]
	protected Vector2 ShadowOffset = new Vector2(1.5f, -1.5f);

	// Token: 0x0400489C RID: 18588
	private LayoutElement shadowLayoutElement;
}
