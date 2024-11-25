using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB1 RID: 3505
public class TelescopeSideScreen : SideScreenContent
{
	// Token: 0x06006EB8 RID: 28344 RVA: 0x002996C8 File Offset: 0x002978C8
	public TelescopeSideScreen()
	{
		this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
	}

	// Token: 0x06006EB9 RID: 28345 RVA: 0x002996E4 File Offset: 0x002978E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectStarmapScreen.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
		SpacecraftManager.instance.Subscribe(532901469, this.refreshDisplayStateDelegate);
		this.RefreshDisplayState(null);
	}

	// Token: 0x06006EBA RID: 28346 RVA: 0x0029973E File Offset: 0x0029793E
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RefreshDisplayState(null);
		this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
	}

	// Token: 0x06006EBB RID: 28347 RVA: 0x00299767 File Offset: 0x00297967
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x06006EBC RID: 28348 RVA: 0x00299783 File Offset: 0x00297983
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x06006EBD RID: 28349 RVA: 0x0029979F File Offset: 0x0029799F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telescope>() != null;
	}

	// Token: 0x06006EBE RID: 28350 RVA: 0x002997B0 File Offset: 0x002979B0
	private void RefreshDisplayState(object data = null)
	{
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		if (SelectTool.Instance.selected.GetComponent<Telescope>() == null)
		{
			return;
		}
		if (!SpacecraftManager.instance.HasAnalysisTarget())
		{
			this.DescriptionText.text = "<b><color=#FF0000>" + UI.UISIDESCREENS.TELESCOPESIDESCREEN.NO_SELECTED_ANALYSIS_TARGET + "</color></b>";
			return;
		}
		string text = UI.UISIDESCREENS.TELESCOPESIDESCREEN.ANALYSIS_TARGET_SELECTED;
		this.DescriptionText.text = text;
	}

	// Token: 0x04004B83 RID: 19331
	public KButton selectStarmapScreen;

	// Token: 0x04004B84 RID: 19332
	public Image researchButtonIcon;

	// Token: 0x04004B85 RID: 19333
	public GameObject content;

	// Token: 0x04004B86 RID: 19334
	private GameObject target;

	// Token: 0x04004B87 RID: 19335
	private Action<object> refreshDisplayStateDelegate;

	// Token: 0x04004B88 RID: 19336
	public LocText DescriptionText;
}
