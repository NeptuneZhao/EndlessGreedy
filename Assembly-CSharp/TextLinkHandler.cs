using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DD6 RID: 3542
public class TextLinkHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x06007094 RID: 28820 RVA: 0x002A99D8 File Offset: 0x002A7BD8
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (!this.text.AllowLinks)
		{
			return;
		}
		int num = TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null);
		if (num != -1)
		{
			string text = CodexCache.FormatLinkID(this.text.textInfo.linkInfo[num].GetLinkID());
			if (this.overrideLinkAction == null || this.overrideLinkAction(text))
			{
				if (!CodexCache.entries.ContainsKey(text))
				{
					SubEntry subEntry = CodexCache.FindSubEntry(text);
					if (subEntry == null || subEntry.disabled)
					{
						text = "PAGENOTFOUND";
					}
				}
				else if (CodexCache.entries[text].disabled)
				{
					text = "PAGENOTFOUND";
				}
				if (!ManagementMenu.Instance.codexScreen.gameObject.activeInHierarchy)
				{
					ManagementMenu.Instance.ToggleCodex();
				}
				ManagementMenu.Instance.codexScreen.ChangeArticle(text, true, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			}
		}
	}

	// Token: 0x06007095 RID: 28821 RVA: 0x002A9AC6 File Offset: 0x002A7CC6
	private void Update()
	{
		this.CheckMouseOver();
		if (TextLinkHandler.hoveredText == this && this.text.AllowLinks)
		{
			PlayerController.Instance.ActiveTool.SetLinkCursor(this.hoverLink);
		}
	}

	// Token: 0x06007096 RID: 28822 RVA: 0x002A9AFD File Offset: 0x002A7CFD
	private void OnEnable()
	{
		this.CheckMouseOver();
	}

	// Token: 0x06007097 RID: 28823 RVA: 0x002A9B05 File Offset: 0x002A7D05
	private void OnDisable()
	{
		this.ClearState();
	}

	// Token: 0x06007098 RID: 28824 RVA: 0x002A9B0D File Offset: 0x002A7D0D
	private void Awake()
	{
		this.text = base.GetComponent<LocText>();
		if (this.text.AllowLinks && !this.text.raycastTarget)
		{
			this.text.raycastTarget = true;
		}
	}

	// Token: 0x06007099 RID: 28825 RVA: 0x002A9B41 File Offset: 0x002A7D41
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetMouseOver();
	}

	// Token: 0x0600709A RID: 28826 RVA: 0x002A9B49 File Offset: 0x002A7D49
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ClearState();
	}

	// Token: 0x0600709B RID: 28827 RVA: 0x002A9B54 File Offset: 0x002A7D54
	private void ClearState()
	{
		if (this == null || this.Equals(null))
		{
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			if (this.hoverLink && PlayerController.Instance != null && PlayerController.Instance.ActiveTool != null)
			{
				PlayerController.Instance.ActiveTool.SetLinkCursor(false);
			}
			TextLinkHandler.hoveredText = null;
			this.hoverLink = false;
		}
	}

	// Token: 0x0600709C RID: 28828 RVA: 0x002A9BC8 File Offset: 0x002A7DC8
	public void CheckMouseOver()
	{
		if (this.text == null)
		{
			return;
		}
		if (TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null) != -1)
		{
			this.SetMouseOver();
			this.hoverLink = true;
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			this.hoverLink = false;
		}
	}

	// Token: 0x0600709D RID: 28829 RVA: 0x002A9C1A File Offset: 0x002A7E1A
	private void SetMouseOver()
	{
		if (TextLinkHandler.hoveredText != null && TextLinkHandler.hoveredText != this)
		{
			TextLinkHandler.hoveredText.hoverLink = false;
		}
		TextLinkHandler.hoveredText = this;
	}

	// Token: 0x04004D59 RID: 19801
	private static TextLinkHandler hoveredText;

	// Token: 0x04004D5A RID: 19802
	[MyCmpGet]
	private LocText text;

	// Token: 0x04004D5B RID: 19803
	private bool hoverLink;

	// Token: 0x04004D5C RID: 19804
	public Func<string, bool> overrideLinkAction;
}
