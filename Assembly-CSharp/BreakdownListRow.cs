using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF1 RID: 3569
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownListRow")]
public class BreakdownListRow : KMonoBehaviour
{
	// Token: 0x0600714A RID: 29002 RVA: 0x002ADA88 File Offset: 0x002ABC88
	public void ShowData(string name, string value)
	{
		base.gameObject.transform.localScale = Vector3.one;
		this.nameLabel.text = name;
		this.valueLabel.text = value;
		this.dotOutlineImage.gameObject.SetActive(true);
		Vector2 vector = Vector2.one * 0.6f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.dotInsideImage.gameObject.SetActive(true);
		this.dotInsideImage.color = BreakdownListRow.statusColour[0];
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetHighlighted(false);
		this.SetImportant(false);
	}

	// Token: 0x0600714B RID: 29003 RVA: 0x002ADB64 File Offset: 0x002ABD64
	public void ShowStatusData(string name, string value, BreakdownListRow.Status dotColor)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetStatusColor(dotColor);
	}

	// Token: 0x0600714C RID: 29004 RVA: 0x002ADBC4 File Offset: 0x002ABDC4
	public void SetStatusColor(BreakdownListRow.Status dotColor)
	{
		this.checkmarkImage.gameObject.SetActive(dotColor > BreakdownListRow.Status.Default);
		this.checkmarkImage.color = BreakdownListRow.statusColour[(int)dotColor];
		switch (dotColor)
		{
		case BreakdownListRow.Status.Red:
			this.checkmarkImage.sprite = this.statusFailureIcon;
			return;
		case BreakdownListRow.Status.Green:
			this.checkmarkImage.sprite = this.statusSuccessIcon;
			return;
		case BreakdownListRow.Status.Yellow:
			this.checkmarkImage.sprite = this.statusWarningIcon;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600714D RID: 29005 RVA: 0x002ADC48 File Offset: 0x002ABE48
	public void ShowCheckmarkData(string name, string value, BreakdownListRow.Status status)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.SetStatusColor(status);
	}

	// Token: 0x0600714E RID: 29006 RVA: 0x002ADCAC File Offset: 0x002ABEAC
	public void ShowIconData(string name, string value, Sprite sprite)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(true);
		this.checkmarkImage.gameObject.SetActive(false);
		this.iconImage.sprite = sprite;
		this.iconImage.color = Color.white;
	}

	// Token: 0x0600714F RID: 29007 RVA: 0x002ADD21 File Offset: 0x002ABF21
	public void ShowIconData(string name, string value, Sprite sprite, Color spriteColor)
	{
		this.ShowIconData(name, value, sprite);
		this.iconImage.color = spriteColor;
	}

	// Token: 0x06007150 RID: 29008 RVA: 0x002ADD3C File Offset: 0x002ABF3C
	public void SetHighlighted(bool highlighted)
	{
		this.isHighlighted = highlighted;
		Vector2 vector = Vector2.one * 0.8f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.nameLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
		this.valueLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
	}

	// Token: 0x06007151 RID: 29009 RVA: 0x002ADDC8 File Offset: 0x002ABFC8
	public void SetDisabled(bool disabled)
	{
		this.isDisabled = disabled;
		this.nameLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
		this.valueLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
	}

	// Token: 0x06007152 RID: 29010 RVA: 0x002ADE1C File Offset: 0x002AC01C
	public void SetImportant(bool important)
	{
		this.isImportant = important;
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.nameLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.valueLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.nameLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
		this.valueLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
	}

	// Token: 0x06007153 RID: 29011 RVA: 0x002ADEB4 File Offset: 0x002AC0B4
	public void HideIcon()
	{
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
	}

	// Token: 0x06007154 RID: 29012 RVA: 0x002ADF05 File Offset: 0x002AC105
	public void AddTooltip(string tooltipText)
	{
		if (this.tooltip == null)
		{
			this.tooltip = base.gameObject.AddComponent<ToolTip>();
		}
		this.tooltip.SetSimpleTooltip(tooltipText);
	}

	// Token: 0x06007155 RID: 29013 RVA: 0x002ADF32 File Offset: 0x002AC132
	public void ClearTooltip()
	{
		if (this.tooltip != null)
		{
			this.tooltip.ClearMultiStringTooltip();
		}
	}

	// Token: 0x06007156 RID: 29014 RVA: 0x002ADF4D File Offset: 0x002AC14D
	public void SetValue(string value)
	{
		this.valueLabel.text = value;
	}

	// Token: 0x04004DF7 RID: 19959
	private static Color[] statusColour = new Color[]
	{
		new Color(0.34117648f, 0.36862746f, 0.45882353f, 1f),
		new Color(0.72156864f, 0.38431373f, 0f, 1f),
		new Color(0.38431373f, 0.72156864f, 0f, 1f),
		new Color(0.72156864f, 0.72156864f, 0f, 1f)
	};

	// Token: 0x04004DF8 RID: 19960
	public Image dotOutlineImage;

	// Token: 0x04004DF9 RID: 19961
	public Image dotInsideImage;

	// Token: 0x04004DFA RID: 19962
	public Image iconImage;

	// Token: 0x04004DFB RID: 19963
	public Image checkmarkImage;

	// Token: 0x04004DFC RID: 19964
	public LocText nameLabel;

	// Token: 0x04004DFD RID: 19965
	public LocText valueLabel;

	// Token: 0x04004DFE RID: 19966
	private bool isHighlighted;

	// Token: 0x04004DFF RID: 19967
	private bool isDisabled;

	// Token: 0x04004E00 RID: 19968
	private bool isImportant;

	// Token: 0x04004E01 RID: 19969
	private ToolTip tooltip;

	// Token: 0x04004E02 RID: 19970
	[SerializeField]
	private Sprite statusSuccessIcon;

	// Token: 0x04004E03 RID: 19971
	[SerializeField]
	private Sprite statusWarningIcon;

	// Token: 0x04004E04 RID: 19972
	[SerializeField]
	private Sprite statusFailureIcon;

	// Token: 0x02001EF3 RID: 7923
	public enum Status
	{
		// Token: 0x04008C1C RID: 35868
		Default,
		// Token: 0x04008C1D RID: 35869
		Red,
		// Token: 0x04008C1E RID: 35870
		Green,
		// Token: 0x04008C1F RID: 35871
		Yellow
	}
}
