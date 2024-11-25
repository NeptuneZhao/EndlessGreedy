using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000952 RID: 2386
[AddComponentMenu("KMonoBehaviour/scripts/LoreBearer")]
public class LoreBearer : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x060045A0 RID: 17824 RVA: 0x0018C9C9 File Offset: 0x0018ABC9
	public string content
	{
		get
		{
			return Strings.Get("STRINGS.LORE.BUILDINGS." + base.gameObject.name + ".ENTRY");
		}
	}

	// Token: 0x060045A1 RID: 17825 RVA: 0x0018C9EF File Offset: 0x0018ABEF
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060045A2 RID: 17826 RVA: 0x0018C9F7 File Offset: 0x0018ABF7
	public LoreBearer Internal_SetContent(LoreBearerAction action)
	{
		this.displayContentAction = action;
		return this;
	}

	// Token: 0x060045A3 RID: 17827 RVA: 0x0018CA01 File Offset: 0x0018AC01
	public LoreBearer Internal_SetContent(LoreBearerAction action, string[] collectionsToUnlockFrom)
	{
		this.displayContentAction = action;
		this.collectionsToUnlockFrom = collectionsToUnlockFrom;
		return this;
	}

	// Token: 0x060045A4 RID: 17828 RVA: 0x0018CA12 File Offset: 0x0018AC12
	public static InfoDialogScreen ShowPopupDialog()
	{
		return (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
	}

	// Token: 0x060045A5 RID: 17829 RVA: 0x0018CA44 File Offset: 0x0018AC44
	private void OnClickRead()
	{
		InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(base.gameObject.GetComponent<KSelectable>().GetProperName()).AddDefaultOK(true);
		if (this.BeenClicked)
		{
			infoDialogScreen.AddPlainText(this.BeenSearched);
			return;
		}
		this.BeenClicked = true;
		if (DlcManager.IsExpansion1Active())
		{
			Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 1, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.gameObject.transform, 1.5f, false);
		}
		if (this.displayContentAction != null)
		{
			this.displayContentAction(infoDialogScreen);
			return;
		}
		LoreBearerUtil.UnlockNextJournalEntry(infoDialogScreen);
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x060045A6 RID: 17830 RVA: 0x0018CB0A File Offset: 0x0018AD0A
	public string SidescreenButtonText
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.NAME;
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x060045A7 RID: 17831 RVA: 0x0018CB25 File Offset: 0x0018AD25
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.TOOLTIP_ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.TOOLTIP;
		}
	}

	// Token: 0x060045A8 RID: 17832 RVA: 0x0018CB40 File Offset: 0x0018AD40
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x060045A9 RID: 17833 RVA: 0x0018CB43 File Offset: 0x0018AD43
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060045AA RID: 17834 RVA: 0x0018CB46 File Offset: 0x0018AD46
	public void OnSidescreenButtonPressed()
	{
		this.OnClickRead();
	}

	// Token: 0x060045AB RID: 17835 RVA: 0x0018CB4E File Offset: 0x0018AD4E
	public bool SidescreenButtonInteractable()
	{
		return !this.BeenClicked;
	}

	// Token: 0x060045AC RID: 17836 RVA: 0x0018CB59 File Offset: 0x0018AD59
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x060045AD RID: 17837 RVA: 0x0018CB5D File Offset: 0x0018AD5D
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04002D59 RID: 11609
	[Serialize]
	private bool BeenClicked;

	// Token: 0x04002D5A RID: 11610
	public string BeenSearched = UI.USERMENUACTIONS.READLORE.ALREADY_SEARCHED;

	// Token: 0x04002D5B RID: 11611
	private string[] collectionsToUnlockFrom;

	// Token: 0x04002D5C RID: 11612
	private LoreBearerAction displayContentAction;
}
