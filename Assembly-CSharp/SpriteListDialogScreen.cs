using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DCC RID: 3532
public class SpriteListDialogScreen : KModalScreen
{
	// Token: 0x06007014 RID: 28692 RVA: 0x002A4377 File Offset: 0x002A2577
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<SpriteListDialogScreen.Button>();
	}

	// Token: 0x06007015 RID: 28693 RVA: 0x002A4396 File Offset: 0x002A2596
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06007016 RID: 28694 RVA: 0x002A4399 File Offset: 0x002A2599
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06007017 RID: 28695 RVA: 0x002A43B4 File Offset: 0x002A25B4
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new SpriteListDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x06007018 RID: 28696 RVA: 0x002A4400 File Offset: 0x002A2600
	public void AddListRow(Sprite sprite, string text, float width = -1f, float height = -1f)
	{
		GameObject gameObject = Util.KInstantiateUI(this.listPrefab, this.listPanel, true);
		gameObject.GetComponentInChildren<LocText>().text = text;
		Image componentInChildren = gameObject.GetComponentInChildren<Image>();
		componentInChildren.sprite = sprite;
		if (sprite == null)
		{
			Color color = componentInChildren.color;
			color.a = 0f;
			componentInChildren.color = color;
		}
		if (width >= 0f || height >= 0f)
		{
			componentInChildren.GetComponent<AspectRatioFitter>().enabled = false;
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = width;
			component.preferredWidth = width;
			component.minHeight = height;
			component.preferredHeight = height;
			return;
		}
		AspectRatioFitter component2 = componentInChildren.GetComponent<AspectRatioFitter>();
		float aspectRatio = (sprite == null) ? 1f : (sprite.rect.width / sprite.rect.height);
		component2.aspectRatio = aspectRatio;
	}

	// Token: 0x06007019 RID: 28697 RVA: 0x002A44D8 File Offset: 0x002A26D8
	public void PopupConfirmDialog(string text, string title_text = null)
	{
		foreach (SpriteListDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x0600701A RID: 28698 RVA: 0x002A456C File Offset: 0x002A276C
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x04004CCF RID: 19663
	public System.Action onDeactivateCB;

	// Token: 0x04004CD0 RID: 19664
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04004CD1 RID: 19665
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x04004CD2 RID: 19666
	[SerializeField]
	private LocText titleText;

	// Token: 0x04004CD3 RID: 19667
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x04004CD4 RID: 19668
	[SerializeField]
	private GameObject listPanel;

	// Token: 0x04004CD5 RID: 19669
	[SerializeField]
	private GameObject listPrefab;

	// Token: 0x04004CD6 RID: 19670
	private List<SpriteListDialogScreen.Button> buttons;

	// Token: 0x02001EDC RID: 7900
	private struct Button
	{
		// Token: 0x04008BBD RID: 35773
		public System.Action action;

		// Token: 0x04008BBE RID: 35774
		public GameObject gameObject;

		// Token: 0x04008BBF RID: 35775
		public string label;
	}
}
