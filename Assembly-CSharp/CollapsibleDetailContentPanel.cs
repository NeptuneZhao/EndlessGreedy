using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C34 RID: 3124
[AddComponentMenu("KMonoBehaviour/scripts/CollapsibleDetailContentPanel")]
public class CollapsibleDetailContentPanel : KMonoBehaviour
{
	// Token: 0x06005FE6 RID: 24550 RVA: 0x0023A2F8 File Offset: 0x002384F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.collapseButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ToggleOpen));
		this.ArrowIcon.SetActive();
		this.log = new LoggerFSS("detailpanel", 35);
		this.labels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>>();
		this.buttonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>>();
		this.Commit();
	}

	// Token: 0x06005FE7 RID: 24551 RVA: 0x0023A36B File Offset: 0x0023856B
	public void SetTitle(string title)
	{
		this.HeaderLabel.text = title;
	}

	// Token: 0x06005FE8 RID: 24552 RVA: 0x0023A37C File Offset: 0x0023857C
	public void Commit()
	{
		int num = 0;
		foreach (CollapsibleDetailContentPanel.Label<DetailLabel> label in this.labels.Values)
		{
			if (label.used)
			{
				num++;
				if (!label.obj.gameObject.activeSelf)
				{
					label.obj.gameObject.SetActive(true);
				}
			}
			else if (!label.used && label.obj.gameObject.activeSelf)
			{
				label.obj.gameObject.SetActive(false);
			}
			label.used = false;
		}
		foreach (CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label2 in this.buttonLabels.Values)
		{
			if (label2.used)
			{
				num++;
				if (!label2.obj.gameObject.activeSelf)
				{
					label2.obj.gameObject.SetActive(true);
				}
			}
			else if (!label2.used && label2.obj.gameObject.activeSelf)
			{
				label2.obj.gameObject.SetActive(false);
			}
			label2.used = false;
		}
		if (base.gameObject.activeSelf && num == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!base.gameObject.activeSelf && num > 0)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005FE9 RID: 24553 RVA: 0x0023A518 File Offset: 0x00238718
	public void SetLabel(string id, string text, string tooltip)
	{
		CollapsibleDetailContentPanel.Label<DetailLabel> label;
		if (!this.labels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabel>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabel>()
			};
			label.obj.gameObject.name = id;
			this.labels[id] = label;
		}
		label.obj.label.AllowLinks = true;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.used = true;
	}

	// Token: 0x06005FEA RID: 24554 RVA: 0x0023A5C4 File Offset: 0x002387C4
	public void SetLabelWithButton(string id, string text, string tooltip, System.Action buttonCb)
	{
		CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label;
		if (!this.buttonLabels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabelWithButton>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelWithActionButtonTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabelWithButton>()
			};
			label.obj.gameObject.name = id;
			this.buttonLabels[id] = label;
		}
		label.obj.label.AllowLinks = false;
		label.obj.label.raycastTarget = false;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.obj.button.ClearOnClick();
		label.obj.button.onClick += buttonCb;
		label.used = true;
	}

	// Token: 0x06005FEB RID: 24555 RVA: 0x0023A6A0 File Offset: 0x002388A0
	private void ToggleOpen()
	{
		bool flag = this.scalerMask.gameObject.activeSelf;
		flag = !flag;
		this.scalerMask.gameObject.SetActive(flag);
		if (flag)
		{
			this.ArrowIcon.SetActive();
			this.ForceLocTextsMeshRebuild();
			return;
		}
		this.ArrowIcon.SetInactive();
	}

	// Token: 0x06005FEC RID: 24556 RVA: 0x0023A6F4 File Offset: 0x002388F4
	public void ForceLocTextsMeshRebuild()
	{
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ForceMeshUpdate();
		}
	}

	// Token: 0x06005FED RID: 24557 RVA: 0x0023A71E File Offset: 0x0023891E
	public void SetActive(bool active)
	{
		if (base.gameObject.activeSelf != active)
		{
			base.gameObject.SetActive(active);
		}
	}

	// Token: 0x040040AB RID: 16555
	public ImageToggleState ArrowIcon;

	// Token: 0x040040AC RID: 16556
	public LocText HeaderLabel;

	// Token: 0x040040AD RID: 16557
	public MultiToggle collapseButton;

	// Token: 0x040040AE RID: 16558
	public Transform Content;

	// Token: 0x040040AF RID: 16559
	public ScalerMask scalerMask;

	// Token: 0x040040B0 RID: 16560
	[Space(10f)]
	public DetailLabel labelTemplate;

	// Token: 0x040040B1 RID: 16561
	public DetailLabelWithButton labelWithActionButtonTemplate;

	// Token: 0x040040B2 RID: 16562
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>> labels;

	// Token: 0x040040B3 RID: 16563
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>> buttonLabels;

	// Token: 0x040040B4 RID: 16564
	private LoggerFSS log;

	// Token: 0x02001D16 RID: 7446
	private class Label<T>
	{
		// Token: 0x040085F0 RID: 34288
		public T obj;

		// Token: 0x040085F1 RID: 34289
		public bool used;
	}
}
