using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C29 RID: 3113
public class DLCToggle : KMonoBehaviour
{
	// Token: 0x06005F6B RID: 24427 RVA: 0x00236B8C File Offset: 0x00234D8C
	protected override void OnPrefabInit()
	{
		this.expansion1Active = DlcManager.IsExpansion1Active();
	}

	// Token: 0x06005F6C RID: 24428 RVA: 0x00236B9C File Offset: 0x00234D9C
	public void ToggleExpansion1Cicked()
	{
		Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.GetComponentInParent<Canvas>().gameObject, true).AddDefaultCancel().SetHeader(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1 : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1).AddSprite(this.expansion1Active ? GlobalResources.Instance().baseGameLogoSmall : GlobalResources.Instance().expansion1LogoSmall).AddPlainText(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_DESC : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_DESC).AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen screen)
		{
			DlcManager.ToggleDLC("EXPANSION1_ID");
		}, true);
	}

	// Token: 0x0400403C RID: 16444
	private bool expansion1Active;
}
