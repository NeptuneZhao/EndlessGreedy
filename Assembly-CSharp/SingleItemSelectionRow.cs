using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DA6 RID: 3494
public class SingleItemSelectionRow : KMonoBehaviour
{
	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06006E4B RID: 28235 RVA: 0x00297A48 File Offset: 0x00295C48
	public virtual string InvalidTagTitle
	{
		get
		{
			return UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.NO_SELECTION;
		}
	}

	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x06006E4C RID: 28236 RVA: 0x00297A54 File Offset: 0x00295C54
	// (set) Token: 0x06006E4D RID: 28237 RVA: 0x00297A5C File Offset: 0x00295C5C
	public Tag InvalidTag { get; protected set; } = GameTags.Void;

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x06006E4E RID: 28238 RVA: 0x00297A65 File Offset: 0x00295C65
	// (set) Token: 0x06006E4F RID: 28239 RVA: 0x00297A6D File Offset: 0x00295C6D
	public new Tag tag { get; protected set; }

	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x06006E50 RID: 28240 RVA: 0x00297A76 File Offset: 0x00295C76
	public bool IsVisible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x06006E51 RID: 28241 RVA: 0x00297A83 File Offset: 0x00295C83
	// (set) Token: 0x06006E52 RID: 28242 RVA: 0x00297A8B File Offset: 0x00295C8B
	public bool IsSelected { get; protected set; }

	// Token: 0x06006E53 RID: 28243 RVA: 0x00297A94 File Offset: 0x00295C94
	protected override void OnPrefabInit()
	{
		this.regularColor = this.outline.color;
		base.OnPrefabInit();
	}

	// Token: 0x06006E54 RID: 28244 RVA: 0x00297AB0 File Offset: 0x00295CB0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.button != null)
		{
			this.button.onPointerEnter += delegate()
			{
				if (!this.IsSelected)
				{
					this.outline.color = this.outlineHighLightColor;
				}
			};
			this.button.onPointerExit += delegate()
			{
				if (!this.IsSelected)
				{
					this.outline.color = this.regularColor;
				}
			};
			this.button.onClick += this.OnItemClicked;
		}
	}

	// Token: 0x06006E55 RID: 28245 RVA: 0x00297B17 File Offset: 0x00295D17
	public virtual void SetVisibleState(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x06006E56 RID: 28246 RVA: 0x00297B25 File Offset: 0x00295D25
	protected virtual void OnItemClicked()
	{
		Action<SingleItemSelectionRow> clicked = this.Clicked;
		if (clicked == null)
		{
			return;
		}
		clicked(this);
	}

	// Token: 0x06006E57 RID: 28247 RVA: 0x00297B38 File Offset: 0x00295D38
	public virtual void SetTag(Tag tag)
	{
		this.tag = tag;
		this.SetText((tag == this.InvalidTag) ? this.InvalidTagTitle : tag.ProperName());
		if (tag != this.InvalidTag)
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
			this.SetIcon(uisprite.first, uisprite.second);
			return;
		}
		this.SetIcon(null, Color.white);
	}

	// Token: 0x06006E58 RID: 28248 RVA: 0x00297BAD File Offset: 0x00295DAD
	protected virtual void SetText(string assignmentStr)
	{
		this.labelText.text = ((!string.IsNullOrEmpty(assignmentStr)) ? assignmentStr : "-");
	}

	// Token: 0x06006E59 RID: 28249 RVA: 0x00297BCA File Offset: 0x00295DCA
	public virtual void SetSelected(bool selected)
	{
		this.IsSelected = selected;
		this.outline.color = (selected ? this.outlineHighLightColor : this.outlineDefaultColor);
		this.BG.color = (selected ? this.BGHighLightColor : Color.white);
	}

	// Token: 0x06006E5A RID: 28250 RVA: 0x00297C0A File Offset: 0x00295E0A
	protected virtual void SetIcon(Sprite sprite, Color color)
	{
		this.icon.sprite = sprite;
		this.icon.color = color;
		this.icon.gameObject.SetActive(sprite != null);
	}

	// Token: 0x04004B45 RID: 19269
	[SerializeField]
	protected Image icon;

	// Token: 0x04004B46 RID: 19270
	[SerializeField]
	protected LocText labelText;

	// Token: 0x04004B47 RID: 19271
	[SerializeField]
	protected Image BG;

	// Token: 0x04004B48 RID: 19272
	[SerializeField]
	protected Image outline;

	// Token: 0x04004B49 RID: 19273
	[SerializeField]
	protected Color outlineHighLightColor = new Color32(168, 74, 121, byte.MaxValue);

	// Token: 0x04004B4A RID: 19274
	[SerializeField]
	protected Color BGHighLightColor = new Color32(168, 74, 121, 80);

	// Token: 0x04004B4B RID: 19275
	[SerializeField]
	protected Color outlineDefaultColor = new Color32(204, 204, 204, byte.MaxValue);

	// Token: 0x04004B4C RID: 19276
	protected Color regularColor = Color.white;

	// Token: 0x04004B4D RID: 19277
	[SerializeField]
	public KButton button;

	// Token: 0x04004B51 RID: 19281
	public Action<SingleItemSelectionRow> Clicked;
}
