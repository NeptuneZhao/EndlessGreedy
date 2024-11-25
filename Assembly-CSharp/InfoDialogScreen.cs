using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C67 RID: 3175
public class InfoDialogScreen : KModalScreen
{
	// Token: 0x0600615B RID: 24923 RVA: 0x00244332 File Offset: 0x00242532
	public InfoScreenPlainText GetSubHeaderPrefab()
	{
		return this.subHeaderTemplate;
	}

	// Token: 0x0600615C RID: 24924 RVA: 0x0024433A File Offset: 0x0024253A
	public InfoScreenPlainText GetPlainTextPrefab()
	{
		return this.plainTextTemplate;
	}

	// Token: 0x0600615D RID: 24925 RVA: 0x00244342 File Offset: 0x00242542
	public InfoScreenLineItem GetLineItemPrefab()
	{
		return this.lineItemTemplate;
	}

	// Token: 0x0600615E RID: 24926 RVA: 0x0024434A File Offset: 0x0024254A
	public GameObject GetPrimaryButtonPrefab()
	{
		return this.leftButtonPrefab;
	}

	// Token: 0x0600615F RID: 24927 RVA: 0x00244352 File Offset: 0x00242552
	public GameObject GetSecondaryButtonPrefab()
	{
		return this.rightButtonPrefab;
	}

	// Token: 0x06006160 RID: 24928 RVA: 0x0024435A File Offset: 0x0024255A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06006161 RID: 24929 RVA: 0x0024436E File Offset: 0x0024256E
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06006162 RID: 24930 RVA: 0x00244374 File Offset: 0x00242574
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!this.escapeCloses)
		{
			e.TryConsume(global::Action.Escape);
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		if (PlayerController.Instance != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006163 RID: 24931 RVA: 0x002443CB File Offset: 0x002425CB
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show && this.onDeactivateFn != null)
		{
			this.onDeactivateFn();
		}
	}

	// Token: 0x06006164 RID: 24932 RVA: 0x002443EA File Offset: 0x002425EA
	public InfoDialogScreen AddDefaultOK(bool escapeCloses = false)
	{
		this.AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, true);
		this.escapeCloses = escapeCloses;
		return this;
	}

	// Token: 0x06006165 RID: 24933 RVA: 0x00244425 File Offset: 0x00242625
	public InfoDialogScreen AddDefaultCancel()
	{
		this.AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, false);
		this.escapeCloses = true;
		return this;
	}

	// Token: 0x06006166 RID: 24934 RVA: 0x00244460 File Offset: 0x00242660
	public InfoDialogScreen AddOption(string text, Action<InfoDialogScreen> action, bool rightSide = false)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		gameObject.gameObject.GetComponentInChildren<LocText>().text = text;
		gameObject.gameObject.GetComponent<KButton>().onClick += delegate()
		{
			action(this);
		};
		return this;
	}

	// Token: 0x06006167 RID: 24935 RVA: 0x002444D8 File Offset: 0x002426D8
	public InfoDialogScreen AddOption(bool rightSide, out KButton button, out LocText buttonText)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		button = gameObject.GetComponent<KButton>();
		buttonText = gameObject.GetComponentInChildren<LocText>();
		return this;
	}

	// Token: 0x06006168 RID: 24936 RVA: 0x0024451F File Offset: 0x0024271F
	public InfoDialogScreen SetHeader(string header)
	{
		this.header.text = header;
		return this;
	}

	// Token: 0x06006169 RID: 24937 RVA: 0x0024452E File Offset: 0x0024272E
	public InfoDialogScreen AddSprite(Sprite sprite)
	{
		Util.KInstantiateUI<InfoScreenSpriteItem>(this.spriteItemTemplate.gameObject, this.contentContainer, false).SetSprite(sprite);
		return this;
	}

	// Token: 0x0600616A RID: 24938 RVA: 0x0024454E File Offset: 0x0024274E
	public InfoDialogScreen AddPlainText(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.plainTextTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x0600616B RID: 24939 RVA: 0x0024456E File Offset: 0x0024276E
	public InfoDialogScreen AddLineItem(string text, string tooltip)
	{
		InfoScreenLineItem infoScreenLineItem = Util.KInstantiateUI<InfoScreenLineItem>(this.lineItemTemplate.gameObject, this.contentContainer, false);
		infoScreenLineItem.SetText(text);
		infoScreenLineItem.SetTooltip(tooltip);
		return this;
	}

	// Token: 0x0600616C RID: 24940 RVA: 0x00244595 File Offset: 0x00242795
	public InfoDialogScreen AddSubHeader(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.subHeaderTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x0600616D RID: 24941 RVA: 0x002445B8 File Offset: 0x002427B8
	public InfoDialogScreen AddSpacer(float height)
	{
		GameObject gameObject = new GameObject("spacer");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(this.contentContainer.transform, false);
		LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
		layoutElement.minHeight = height;
		layoutElement.preferredHeight = height;
		layoutElement.flexibleHeight = 0f;
		gameObject.SetActive(true);
		return this;
	}

	// Token: 0x0600616E RID: 24942 RVA: 0x00244612 File Offset: 0x00242812
	public InfoDialogScreen AddUI<T>(T prefab, out T spawn) where T : MonoBehaviour
	{
		spawn = Util.KInstantiateUI<T>(prefab.gameObject, this.contentContainer, true);
		return this;
	}

	// Token: 0x0600616F RID: 24943 RVA: 0x00244634 File Offset: 0x00242834
	public InfoDialogScreen AddDescriptors(List<Descriptor> descriptors)
	{
		for (int i = 0; i < descriptors.Count; i++)
		{
			this.AddLineItem(descriptors[i].IndentedText(), descriptors[i].tooltipText);
		}
		return this;
	}

	// Token: 0x04004200 RID: 16896
	[SerializeField]
	private InfoScreenPlainText subHeaderTemplate;

	// Token: 0x04004201 RID: 16897
	[SerializeField]
	private InfoScreenPlainText plainTextTemplate;

	// Token: 0x04004202 RID: 16898
	[SerializeField]
	private InfoScreenLineItem lineItemTemplate;

	// Token: 0x04004203 RID: 16899
	[SerializeField]
	private InfoScreenSpriteItem spriteItemTemplate;

	// Token: 0x04004204 RID: 16900
	[Space(10f)]
	[SerializeField]
	private LocText header;

	// Token: 0x04004205 RID: 16901
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x04004206 RID: 16902
	[SerializeField]
	private GameObject leftButtonPrefab;

	// Token: 0x04004207 RID: 16903
	[SerializeField]
	private GameObject rightButtonPrefab;

	// Token: 0x04004208 RID: 16904
	[SerializeField]
	private GameObject leftButtonPanel;

	// Token: 0x04004209 RID: 16905
	[SerializeField]
	private GameObject rightButtonPanel;

	// Token: 0x0400420A RID: 16906
	private bool escapeCloses;

	// Token: 0x0400420B RID: 16907
	public System.Action onDeactivateFn;
}
