using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D1C RID: 3356
public class ReportScreen : KScreen
{
	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x060068D7 RID: 26839 RVA: 0x0027402C File Offset: 0x0027222C
	// (set) Token: 0x060068D8 RID: 26840 RVA: 0x00274033 File Offset: 0x00272233
	public static ReportScreen Instance { get; private set; }

	// Token: 0x060068D9 RID: 26841 RVA: 0x0027403B File Offset: 0x0027223B
	public static void DestroyInstance()
	{
		ReportScreen.Instance = null;
	}

	// Token: 0x060068DA RID: 26842 RVA: 0x00274044 File Offset: 0x00272244
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ReportScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.prevButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day - 1);
		};
		this.nextButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day + 1);
		};
		this.summaryButton.onClick += delegate()
		{
			RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
		};
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060068DB RID: 26843 RVA: 0x002740E6 File Offset: 0x002722E6
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060068DC RID: 26844 RVA: 0x002740EE File Offset: 0x002722EE
	protected override void OnShow(bool bShow)
	{
		base.OnShow(bShow);
		if (ReportManager.Instance != null)
		{
			this.currentReport = ReportManager.Instance.TodaysReport;
		}
	}

	// Token: 0x060068DD RID: 26845 RVA: 0x00274114 File Offset: 0x00272314
	public void SetTitle(string title)
	{
		this.title.text = title;
	}

	// Token: 0x060068DE RID: 26846 RVA: 0x00274122 File Offset: 0x00272322
	public override void ScreenUpdate(bool b)
	{
		base.ScreenUpdate(b);
		this.Refresh();
	}

	// Token: 0x060068DF RID: 26847 RVA: 0x00274134 File Offset: 0x00272334
	private void Refresh()
	{
		global::Debug.Assert(this.currentReport != null);
		if (this.currentReport.day == ReportManager.Instance.TodaysReport.day)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_TODAY, this.currentReport.day));
		}
		else if (this.currentReport.day == ReportManager.Instance.TodaysReport.day - 1)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_YESTERDAY, this.currentReport.day));
		}
		else
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day));
		}
		bool flag = this.currentReport.day < ReportManager.Instance.TodaysReport.day;
		this.nextButton.isInteractable = flag;
		if (flag)
		{
			this.nextButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day + 1);
			this.nextButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.nextButton.GetComponent<ToolTip>().enabled = false;
		}
		flag = (this.currentReport.day > 1);
		this.prevButton.isInteractable = flag;
		if (flag)
		{
			this.prevButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day - 1);
			this.prevButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.prevButton.GetComponent<ToolTip>().enabled = false;
		}
		this.AddSpacer(0);
		int num = 1;
		foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in ReportManager.Instance.ReportGroups)
		{
			ReportManager.ReportEntry entry = this.currentReport.GetEntry(keyValuePair.Key);
			if (num != keyValuePair.Value.group)
			{
				num = keyValuePair.Value.group;
				this.AddSpacer(num);
			}
			bool flag2 = entry.accumulate != 0f || keyValuePair.Value.reportIfZero;
			if (keyValuePair.Value.isHeader)
			{
				this.CreateHeader(keyValuePair.Value);
			}
			else if (flag2)
			{
				this.CreateOrUpdateLine(entry, keyValuePair.Value, flag2);
			}
		}
	}

	// Token: 0x060068E0 RID: 26848 RVA: 0x002743D0 File Offset: 0x002725D0
	public void ShowReport(int day)
	{
		this.currentReport = ReportManager.Instance.FindReport(day);
		global::Debug.Assert(this.currentReport != null, "Can't find report for day: " + day.ToString());
		this.Refresh();
	}

	// Token: 0x060068E1 RID: 26849 RVA: 0x00274408 File Offset: 0x00272608
	private GameObject AddSpacer(int group)
	{
		GameObject gameObject;
		if (this.lineItems.ContainsKey(group.ToString()))
		{
			gameObject = this.lineItems[group.ToString()];
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.lineItemSpacer, this.contentFolder, false);
			gameObject.name = "Spacer" + group.ToString();
			this.lineItems[group.ToString()] = gameObject;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060068E2 RID: 26850 RVA: 0x00274488 File Offset: 0x00272688
	private GameObject CreateHeader(ReportManager.ReportGroup reportGroup)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (gameObject == null)
		{
			gameObject = Util.KInstantiateUI(this.lineItemHeader, this.contentFolder, true);
			gameObject.name = "LineItemHeader" + this.lineItems.Count.ToString();
			this.lineItems[reportGroup.stringKey] = gameObject;
		}
		gameObject.SetActive(true);
		gameObject.GetComponent<ReportScreenHeader>().SetMainEntry(reportGroup);
		return gameObject;
	}

	// Token: 0x060068E3 RID: 26851 RVA: 0x00274510 File Offset: 0x00272710
	private GameObject CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (!is_line_active)
		{
			if (gameObject != null && gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
		else
		{
			if (gameObject == null)
			{
				gameObject = Util.KInstantiateUI(this.lineItem, this.contentFolder, true);
				gameObject.name = "LineItem" + this.lineItems.Count.ToString();
				this.lineItems[reportGroup.stringKey] = gameObject;
			}
			gameObject.SetActive(true);
			gameObject.GetComponent<ReportScreenEntry>().SetMainEntry(entry, reportGroup);
		}
		return gameObject;
	}

	// Token: 0x060068E4 RID: 26852 RVA: 0x002745B6 File Offset: 0x002727B6
	private void OnClickClose()
	{
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Close", false));
		this.Show(false);
	}

	// Token: 0x040046FA RID: 18170
	[SerializeField]
	private LocText title;

	// Token: 0x040046FB RID: 18171
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040046FC RID: 18172
	[SerializeField]
	private KButton prevButton;

	// Token: 0x040046FD RID: 18173
	[SerializeField]
	private KButton nextButton;

	// Token: 0x040046FE RID: 18174
	[SerializeField]
	private KButton summaryButton;

	// Token: 0x040046FF RID: 18175
	[SerializeField]
	private GameObject lineItem;

	// Token: 0x04004700 RID: 18176
	[SerializeField]
	private GameObject lineItemSpacer;

	// Token: 0x04004701 RID: 18177
	[SerializeField]
	private GameObject lineItemHeader;

	// Token: 0x04004702 RID: 18178
	[SerializeField]
	private GameObject contentFolder;

	// Token: 0x04004703 RID: 18179
	private Dictionary<string, GameObject> lineItems = new Dictionary<string, GameObject>();

	// Token: 0x04004704 RID: 18180
	private ReportManager.DailyReport currentReport;
}
