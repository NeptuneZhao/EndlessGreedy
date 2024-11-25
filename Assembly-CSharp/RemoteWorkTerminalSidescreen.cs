using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D97 RID: 3479
public class RemoteWorkTerminalSidescreen : SideScreenContent
{
	// Token: 0x06006DB1 RID: 28081 RVA: 0x00293F5A File Offset: 0x0029215A
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.rowPrefab.SetActive(false);
		if (show)
		{
			this.RefreshOptions(null);
		}
	}

	// Token: 0x06006DB2 RID: 28082 RVA: 0x00293F79 File Offset: 0x00292179
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RemoteWorkTerminal>() != null;
	}

	// Token: 0x06006DB3 RID: 28083 RVA: 0x00293F87 File Offset: 0x00292187
	public override void SetTarget(GameObject target)
	{
		this.targetTerminal = target.GetComponent<RemoteWorkTerminal>();
		this.RefreshOptions(null);
		this.uiRefreshSubHandle = target.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
	}

	// Token: 0x06006DB4 RID: 28084 RVA: 0x00293FB9 File Offset: 0x002921B9
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.targetTerminal != null)
		{
			this.targetTerminal.gameObject.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
	}

	// Token: 0x06006DB5 RID: 28085 RVA: 0x00293FF0 File Offset: 0x002921F0
	private void RefreshOptions(object data = null)
	{
		int num = 0;
		this.SetRow(num++, UI.UISIDESCREENS.GEOTUNERSIDESCREEN.NOTHING, Assets.GetSprite("action_building_disabled"), null);
		foreach (RemoteWorkerDock remoteWorkerDock in Components.RemoteWorkerDocks.GetItems(this.targetTerminal.GetMyWorldId()))
		{
			remoteWorkerDock.GetProperName();
			Sprite first = Def.GetUISprite(remoteWorkerDock.gameObject, "ui", false).first;
			int idx = num++;
			string name = UI.StripLinkFormatting(remoteWorkerDock.GetProperName());
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(remoteWorkerDock.gameObject, "ui", false);
			this.SetRow(idx, name, (uisprite != null) ? uisprite.first : null, remoteWorkerDock);
		}
		for (int i = num; i < this.rowContainer.childCount; i++)
		{
			this.rowContainer.GetChild(i).gameObject.SetActive(false);
		}
	}

	// Token: 0x06006DB6 RID: 28086 RVA: 0x002940F4 File Offset: 0x002922F4
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x06006DB7 RID: 28087 RVA: 0x00294138 File Offset: 0x00292338
	private void SetRow(int idx, string name, Sprite icon, RemoteWorkerDock dock)
	{
		dock == null;
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("label");
		reference.text = name;
		reference.ApplySettings();
		Image reference2 = component.GetReference<Image>("icon");
		reference2.sprite = icon;
		reference2.color = Color.white;
		ToolTip toolTip = gameObject.GetComponentsInChildren<ToolTip>().First<ToolTip>();
		toolTip.SetSimpleTooltip(UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.DOCK_TOOLTIP);
		toolTip.enabled = (dock != null);
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.targetTerminal.FutureDock == dock) ? 1 : 0);
		component2.onClick = delegate()
		{
			this.targetTerminal.FutureDock = dock;
			this.RefreshOptions(null);
		};
		component2.onDoubleClick = delegate()
		{
			CameraController.Instance.CameraGoTo((dock == null) ? this.targetTerminal.transform.GetPosition() : dock.transform.GetPosition(), 2f, true);
			return true;
		};
	}

	// Token: 0x04004ADB RID: 19163
	private RemoteWorkTerminal targetTerminal;

	// Token: 0x04004ADC RID: 19164
	public GameObject rowPrefab;

	// Token: 0x04004ADD RID: 19165
	public RectTransform rowContainer;

	// Token: 0x04004ADE RID: 19166
	public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();

	// Token: 0x04004ADF RID: 19167
	private int uiRefreshSubHandle = -1;
}
