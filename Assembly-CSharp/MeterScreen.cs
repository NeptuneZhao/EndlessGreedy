using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x02000CD9 RID: 3289
public class MeterScreen : KScreen, IRender1000ms
{
	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x060065AC RID: 26028 RVA: 0x0025E9CD File Offset: 0x0025CBCD
	// (set) Token: 0x060065AD RID: 26029 RVA: 0x0025E9D4 File Offset: 0x0025CBD4
	public static MeterScreen Instance { get; private set; }

	// Token: 0x060065AE RID: 26030 RVA: 0x0025E9DC File Offset: 0x0025CBDC
	public static void DestroyInstance()
	{
		MeterScreen.Instance = null;
	}

	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x060065AF RID: 26031 RVA: 0x0025E9E4 File Offset: 0x0025CBE4
	public bool StartValuesSet
	{
		get
		{
			return this.startValuesSet;
		}
	}

	// Token: 0x060065B0 RID: 26032 RVA: 0x0025E9EC File Offset: 0x0025CBEC
	protected override void OnPrefabInit()
	{
		MeterScreen.Instance = this;
	}

	// Token: 0x060065B1 RID: 26033 RVA: 0x0025E9F4 File Offset: 0x0025CBF4
	protected override void OnSpawn()
	{
		this.RedAlertTooltip.OnToolTip = new Func<string>(this.OnRedAlertTooltip);
		MultiToggle redAlertButton = this.RedAlertButton;
		redAlertButton.onClick = (System.Action)Delegate.Combine(redAlertButton.onClick, new System.Action(delegate()
		{
			this.OnRedAlertClick();
		}));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.Refresh();
		});
		Game.Instance.Subscribe(1585324898, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
		Game.Instance.Subscribe(-1393151672, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
	}

	// Token: 0x060065B2 RID: 26034 RVA: 0x0025EA94 File Offset: 0x0025CC94
	private void OnRedAlertClick()
	{
		bool flag = !ClusterManager.Instance.activeWorld.AlertManager.IsRedAlertToggledOn();
		ClusterManager.Instance.activeWorld.AlertManager.ToggleRedAlert(flag);
		if (flag)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
	}

	// Token: 0x060065B3 RID: 26035 RVA: 0x0025EAF2 File Offset: 0x0025CCF2
	private void RefreshRedAlertButtonState()
	{
		this.RedAlertButton.ChangeState(ClusterManager.Instance.activeWorld.IsRedAlert() ? 1 : 0);
	}

	// Token: 0x060065B4 RID: 26036 RVA: 0x0025EB14 File Offset: 0x0025CD14
	public void Render1000ms(float dt)
	{
		this.Refresh();
	}

	// Token: 0x060065B5 RID: 26037 RVA: 0x0025EB1C File Offset: 0x0025CD1C
	public void InitializeValues()
	{
		if (this.startValuesSet)
		{
			return;
		}
		this.startValuesSet = true;
		this.Refresh();
	}

	// Token: 0x060065B6 RID: 26038 RVA: 0x0025EB34 File Offset: 0x0025CD34
	private void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.RefreshMinions();
		for (int i = 0; i < this.valueDisplayers.Length; i++)
		{
			this.valueDisplayers[i].Refresh();
		}
		this.RefreshRedAlertButtonState();
	}

	// Token: 0x060065B7 RID: 26039 RVA: 0x0025EB74 File Offset: 0x0025CD74
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x060065B8 RID: 26040 RVA: 0x0025EBC5 File Offset: 0x0025CDC5
	private List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x060065B9 RID: 26041 RVA: 0x0025EBDC File Offset: 0x0025CDDC
	private void RefreshMinions()
	{
		int count = Components.LiveMinionIdentities.Count;
		int count2 = this.GetWorldMinionIdentities().Count;
		if (count2 == this.cachedMinionCount)
		{
			return;
		}
		this.cachedMinionCount = count2;
		string newString;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			ClusterGridEntity component = ClusterManager.Instance.activeWorld.GetComponent<ClusterGridEntity>();
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION_CLUSTER, component.Name, count2, count);
			this.currentMinions.text = string.Format("{0}/{1}", count2, count);
		}
		else
		{
			this.currentMinions.text = string.Format("{0}", count);
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION, count.ToString("0"));
		}
		this.MinionsTooltip.ClearMultiStringTooltip();
		this.MinionsTooltip.AddMultiStringTooltip(newString, this.ToolTipStyle_Header);
	}

	// Token: 0x060065BA RID: 26042 RVA: 0x0025ECC8 File Offset: 0x0025CEC8
	private string OnRedAlertTooltip()
	{
		this.RedAlertTooltip.ClearMultiStringTooltip();
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_TITLE, this.ToolTipStyle_Header);
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_CONTENT, this.ToolTipStyle_Property);
		return "";
	}

	// Token: 0x040044A8 RID: 17576
	[SerializeField]
	private LocText currentMinions;

	// Token: 0x040044AA RID: 17578
	public ToolTip MinionsTooltip;

	// Token: 0x040044AB RID: 17579
	public MeterScreen_ValueTrackerDisplayer[] valueDisplayers;

	// Token: 0x040044AC RID: 17580
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x040044AD RID: 17581
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x040044AE RID: 17582
	private bool startValuesSet;

	// Token: 0x040044AF RID: 17583
	public MultiToggle RedAlertButton;

	// Token: 0x040044B0 RID: 17584
	public ToolTip RedAlertTooltip;

	// Token: 0x040044B1 RID: 17585
	private MeterScreen.DisplayInfo immunityDisplayInfo = new MeterScreen.DisplayInfo
	{
		selectedIndex = -1
	};

	// Token: 0x040044B2 RID: 17586
	private List<MinionIdentity> worldLiveMinionIdentities;

	// Token: 0x040044B3 RID: 17587
	private int cachedMinionCount = -1;

	// Token: 0x02001DD0 RID: 7632
	private struct DisplayInfo
	{
		// Token: 0x04008851 RID: 34897
		public int selectedIndex;
	}
}
