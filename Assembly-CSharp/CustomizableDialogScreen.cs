using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C27 RID: 3111
public class CustomizableDialogScreen : KModalScreen
{
	// Token: 0x06005F60 RID: 24416 RVA: 0x00236933 File Offset: 0x00234B33
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<CustomizableDialogScreen.Button>();
	}

	// Token: 0x06005F61 RID: 24417 RVA: 0x00236952 File Offset: 0x00234B52
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06005F62 RID: 24418 RVA: 0x00236958 File Offset: 0x00234B58
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new CustomizableDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x06005F63 RID: 24419 RVA: 0x002369A4 File Offset: 0x00234BA4
	public void PopupConfirmDialog(string text, string title_text = null, Sprite image_sprite = null)
	{
		foreach (CustomizableDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x06005F64 RID: 24420 RVA: 0x00236A60 File Offset: 0x00234C60
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x0400402E RID: 16430
	public System.Action onDeactivateCB;

	// Token: 0x0400402F RID: 16431
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04004030 RID: 16432
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x04004031 RID: 16433
	[SerializeField]
	private LocText titleText;

	// Token: 0x04004032 RID: 16434
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x04004033 RID: 16435
	[SerializeField]
	private Image image;

	// Token: 0x04004034 RID: 16436
	private List<CustomizableDialogScreen.Button> buttons;

	// Token: 0x02001D0E RID: 7438
	private struct Button
	{
		// Token: 0x040085DE RID: 34270
		public System.Action action;

		// Token: 0x040085DF RID: 34271
		public GameObject gameObject;

		// Token: 0x040085E0 RID: 34272
		public string label;
	}
}
