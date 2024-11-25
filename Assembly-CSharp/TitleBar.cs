using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DD9 RID: 3545
[AddComponentMenu("KMonoBehaviour/scripts/TitleBar")]
public class TitleBar : KMonoBehaviour
{
	// Token: 0x060070A7 RID: 28839 RVA: 0x002AA763 File Offset: 0x002A8963
	public void SetTitle(string Name)
	{
		this.titleText.text = Name;
	}

	// Token: 0x060070A8 RID: 28840 RVA: 0x002AA771 File Offset: 0x002A8971
	public void SetSubText(string subtext, string tooltip = "")
	{
		this.subtextText.text = subtext;
		this.subtextText.GetComponent<ToolTip>().toolTip = tooltip;
	}

	// Token: 0x060070A9 RID: 28841 RVA: 0x002AA790 File Offset: 0x002A8990
	public void SetWarningActve(bool state)
	{
		this.WarningNotification.SetActive(state);
	}

	// Token: 0x060070AA RID: 28842 RVA: 0x002AA79E File Offset: 0x002A899E
	public void SetWarning(Sprite icon, string label)
	{
		this.SetWarningActve(true);
		this.NotificationIcon.sprite = icon;
		this.NotificationText.text = label;
	}

	// Token: 0x060070AB RID: 28843 RVA: 0x002AA7BF File Offset: 0x002A89BF
	public void SetPortrait(GameObject target)
	{
		this.portrait.SetPortrait(target);
	}

	// Token: 0x04004D6D RID: 19821
	public LocText titleText;

	// Token: 0x04004D6E RID: 19822
	public LocText subtextText;

	// Token: 0x04004D6F RID: 19823
	public GameObject WarningNotification;

	// Token: 0x04004D70 RID: 19824
	public Text NotificationText;

	// Token: 0x04004D71 RID: 19825
	public Image NotificationIcon;

	// Token: 0x04004D72 RID: 19826
	public Sprite techIcon;

	// Token: 0x04004D73 RID: 19827
	public Sprite materialIcon;

	// Token: 0x04004D74 RID: 19828
	public TitleBarPortrait portrait;

	// Token: 0x04004D75 RID: 19829
	public bool userEditable;

	// Token: 0x04004D76 RID: 19830
	public bool setCameraControllerState = true;
}
