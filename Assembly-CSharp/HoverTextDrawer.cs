using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B98 RID: 2968
public class HoverTextDrawer
{
	// Token: 0x06005989 RID: 22921 RVA: 0x00206258 File Offset: 0x00204458
	public HoverTextDrawer(HoverTextDrawer.Skin skin, RectTransform parent)
	{
		this.shadowBars = new HoverTextDrawer.Pool<Image>(skin.shadowBarWidget.gameObject, parent);
		this.selectBorders = new HoverTextDrawer.Pool<Image>(skin.selectBorderWidget.gameObject, parent);
		this.textWidgets = new HoverTextDrawer.Pool<LocText>(skin.textWidget.gameObject, parent);
		this.iconWidgets = new HoverTextDrawer.Pool<Image>(skin.iconWidget.gameObject, parent);
		this.skin = skin;
	}

	// Token: 0x0600598A RID: 22922 RVA: 0x002062CE File Offset: 0x002044CE
	public void SetEnabled(bool enabled)
	{
		this.shadowBars.SetEnabled(enabled);
		this.textWidgets.SetEnabled(enabled);
		this.iconWidgets.SetEnabled(enabled);
		this.selectBorders.SetEnabled(enabled);
	}

	// Token: 0x0600598B RID: 22923 RVA: 0x00206300 File Offset: 0x00204500
	public void BeginDrawing(Vector2 root_pos)
	{
		this.rootPos = root_pos + this.skin.baseOffset;
		if (this.skin.enableDebugOffset)
		{
			this.rootPos += this.skin.debugOffset;
		}
		this.currentPos = this.rootPos;
		this.textWidgets.BeginDrawing();
		this.iconWidgets.BeginDrawing();
		this.shadowBars.BeginDrawing();
		this.selectBorders.BeginDrawing();
		this.firstShadowBar = true;
		this.minLineHeight = 0;
	}

	// Token: 0x0600598C RID: 22924 RVA: 0x00206393 File Offset: 0x00204593
	public void EndDrawing()
	{
		this.shadowBars.EndDrawing();
		this.iconWidgets.EndDrawing();
		this.textWidgets.EndDrawing();
		this.selectBorders.EndDrawing();
	}

	// Token: 0x0600598D RID: 22925 RVA: 0x002063C4 File Offset: 0x002045C4
	public void DrawText(string text, TextStyleSetting style, Color color, bool override_color = true)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		LocText widget = this.textWidgets.Draw(this.currentPos).widget;
		Color color2 = Color.white;
		if (widget.textStyleSetting != style)
		{
			widget.textStyleSetting = style;
			widget.ApplySettings();
		}
		if (style != null)
		{
			color2 = style.textColor;
		}
		if (override_color)
		{
			color2 = color;
		}
		widget.color = color2;
		if (widget.text != text)
		{
			widget.text = text;
			widget.KForceUpdateDirty();
		}
		this.currentPos.x = this.currentPos.x + widget.renderedWidth;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
		this.minLineHeight = (int)Mathf.Max((float)this.minLineHeight, widget.renderedHeight);
	}

	// Token: 0x0600598E RID: 22926 RVA: 0x00206499 File Offset: 0x00204699
	public void DrawText(string text, TextStyleSetting style)
	{
		this.DrawText(text, style, Color.white, false);
	}

	// Token: 0x0600598F RID: 22927 RVA: 0x002064A9 File Offset: 0x002046A9
	public void AddIndent(int width = 36)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.x = this.currentPos.x + (float)width;
	}

	// Token: 0x06005990 RID: 22928 RVA: 0x002064CC File Offset: 0x002046CC
	public void NewLine(int min_height = 26)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.y = this.currentPos.y - (float)Math.Max(min_height, this.minLineHeight);
		this.currentPos.x = this.rootPos.x;
		this.minLineHeight = 0;
	}

	// Token: 0x06005991 RID: 22929 RVA: 0x00206520 File Offset: 0x00204720
	public void DrawIcon(Sprite icon, int min_width = 18)
	{
		this.DrawIcon(icon, Color.white, min_width, 2);
	}

	// Token: 0x06005992 RID: 22930 RVA: 0x00206530 File Offset: 0x00204730
	public void DrawIcon(Sprite icon, Color color, int image_size = 18, int horizontal_spacing = 2)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.AddIndent(horizontal_spacing);
		HoverTextDrawer.Pool<Image>.Entry entry = this.iconWidgets.Draw(this.currentPos + this.skin.shadowImageOffset);
		entry.widget.sprite = icon;
		entry.widget.color = this.skin.shadowImageColor;
		entry.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		HoverTextDrawer.Pool<Image>.Entry entry2 = this.iconWidgets.Draw(this.currentPos);
		entry2.widget.sprite = icon;
		entry2.widget.color = color;
		entry2.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		this.AddIndent(horizontal_spacing);
		this.currentPos.x = this.currentPos.x + (float)image_size;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
	}

	// Token: 0x06005993 RID: 22931 RVA: 0x0020661C File Offset: 0x0020481C
	public void BeginShadowBar(bool selected = false)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		if (this.firstShadowBar)
		{
			this.firstShadowBar = false;
		}
		else
		{
			this.NewLine(22);
		}
		this.isShadowBarSelected = selected;
		this.shadowStartPos = this.currentPos;
		this.maxShadowX = this.rootPos.x;
	}

	// Token: 0x06005994 RID: 22932 RVA: 0x00206674 File Offset: 0x00204874
	public void EndShadowBar()
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.NewLine(22);
		HoverTextDrawer.Pool<Image>.Entry entry = this.shadowBars.Draw(this.currentPos);
		entry.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x, this.skin.shadowBarBorder.y);
		entry.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f);
		if (this.isShadowBarSelected)
		{
			HoverTextDrawer.Pool<Image>.Entry entry2 = this.selectBorders.Draw(this.currentPos);
			entry2.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x - this.skin.selectBorder.x, this.skin.shadowBarBorder.y + this.skin.selectBorder.y);
			entry2.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f + this.skin.selectBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f + this.skin.selectBorder.y * 2f);
		}
	}

	// Token: 0x06005995 RID: 22933 RVA: 0x00206858 File Offset: 0x00204A58
	public void Cleanup()
	{
		this.shadowBars.Cleanup();
		this.textWidgets.Cleanup();
		this.iconWidgets.Cleanup();
	}

	// Token: 0x04003ADE RID: 15070
	public HoverTextDrawer.Skin skin;

	// Token: 0x04003ADF RID: 15071
	private Vector2 currentPos;

	// Token: 0x04003AE0 RID: 15072
	private Vector2 rootPos;

	// Token: 0x04003AE1 RID: 15073
	private Vector2 shadowStartPos;

	// Token: 0x04003AE2 RID: 15074
	private float maxShadowX;

	// Token: 0x04003AE3 RID: 15075
	private bool firstShadowBar;

	// Token: 0x04003AE4 RID: 15076
	private bool isShadowBarSelected;

	// Token: 0x04003AE5 RID: 15077
	private int minLineHeight;

	// Token: 0x04003AE6 RID: 15078
	private HoverTextDrawer.Pool<LocText> textWidgets;

	// Token: 0x04003AE7 RID: 15079
	private HoverTextDrawer.Pool<Image> iconWidgets;

	// Token: 0x04003AE8 RID: 15080
	private HoverTextDrawer.Pool<Image> shadowBars;

	// Token: 0x04003AE9 RID: 15081
	private HoverTextDrawer.Pool<Image> selectBorders;

	// Token: 0x02001BFC RID: 7164
	[Serializable]
	public class Skin
	{
		// Token: 0x0400815D RID: 33117
		public Vector2 baseOffset;

		// Token: 0x0400815E RID: 33118
		public LocText textWidget;

		// Token: 0x0400815F RID: 33119
		public Image iconWidget;

		// Token: 0x04008160 RID: 33120
		public Vector2 shadowImageOffset;

		// Token: 0x04008161 RID: 33121
		public Color shadowImageColor;

		// Token: 0x04008162 RID: 33122
		public Image shadowBarWidget;

		// Token: 0x04008163 RID: 33123
		public Image selectBorderWidget;

		// Token: 0x04008164 RID: 33124
		public Vector2 shadowBarBorder;

		// Token: 0x04008165 RID: 33125
		public Vector2 selectBorder;

		// Token: 0x04008166 RID: 33126
		public bool drawWidgets;

		// Token: 0x04008167 RID: 33127
		public bool enableProfiling;

		// Token: 0x04008168 RID: 33128
		public bool enableDebugOffset;

		// Token: 0x04008169 RID: 33129
		public bool drawInProgressHoverText;

		// Token: 0x0400816A RID: 33130
		public Vector2 debugOffset;
	}

	// Token: 0x02001BFD RID: 7165
	private class Pool<WidgetType> where WidgetType : MonoBehaviour
	{
		// Token: 0x0600A504 RID: 42244 RVA: 0x0038E834 File Offset: 0x0038CA34
		public Pool(GameObject prefab, RectTransform master_root)
		{
			this.prefab = prefab;
			GameObject gameObject = new GameObject(typeof(WidgetType).Name);
			this.root = gameObject.AddComponent<RectTransform>();
			this.root.SetParent(master_root);
			this.root.anchoredPosition = Vector2.zero;
			this.root.anchorMin = Vector2.zero;
			this.root.anchorMax = Vector2.one;
			this.root.sizeDelta = Vector2.zero;
			gameObject.AddComponent<CanvasGroup>();
		}

		// Token: 0x0600A505 RID: 42245 RVA: 0x0038E8D0 File Offset: 0x0038CAD0
		public HoverTextDrawer.Pool<WidgetType>.Entry Draw(Vector2 pos)
		{
			HoverTextDrawer.Pool<WidgetType>.Entry entry;
			if (this.drawnWidgets < this.entries.Count)
			{
				entry = this.entries[this.drawnWidgets];
				if (!entry.widget.gameObject.activeSelf)
				{
					entry.widget.gameObject.SetActive(true);
				}
			}
			else
			{
				GameObject gameObject = Util.KInstantiateUI(this.prefab, this.root.gameObject, false);
				gameObject.SetActive(true);
				entry.widget = gameObject.GetComponent<WidgetType>();
				entry.rect = gameObject.GetComponent<RectTransform>();
				this.entries.Add(entry);
			}
			entry.rect.anchoredPosition = new Vector2(pos.x, pos.y);
			this.drawnWidgets++;
			return entry;
		}

		// Token: 0x0600A506 RID: 42246 RVA: 0x0038E9A1 File Offset: 0x0038CBA1
		public void BeginDrawing()
		{
			this.drawnWidgets = 0;
		}

		// Token: 0x0600A507 RID: 42247 RVA: 0x0038E9AC File Offset: 0x0038CBAC
		public void EndDrawing()
		{
			for (int i = this.drawnWidgets; i < this.entries.Count; i++)
			{
				if (this.entries[i].widget.gameObject.activeSelf)
				{
					this.entries[i].widget.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600A508 RID: 42248 RVA: 0x0038EA17 File Offset: 0x0038CC17
		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				this.root.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
				return;
			}
			this.root.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		}

		// Token: 0x0600A509 RID: 42249 RVA: 0x0038EA54 File Offset: 0x0038CC54
		public void Cleanup()
		{
			foreach (HoverTextDrawer.Pool<WidgetType>.Entry entry in this.entries)
			{
				UnityEngine.Object.Destroy(entry.widget.gameObject);
			}
			this.entries.Clear();
		}

		// Token: 0x0400816B RID: 33131
		private GameObject prefab;

		// Token: 0x0400816C RID: 33132
		private RectTransform root;

		// Token: 0x0400816D RID: 33133
		private List<HoverTextDrawer.Pool<WidgetType>.Entry> entries = new List<HoverTextDrawer.Pool<WidgetType>.Entry>();

		// Token: 0x0400816E RID: 33134
		private int drawnWidgets;

		// Token: 0x02002633 RID: 9779
		public struct Entry
		{
			// Token: 0x0400A9FE RID: 43518
			public WidgetType widget;

			// Token: 0x0400A9FF RID: 43519
			public RectTransform rect;
		}
	}
}
