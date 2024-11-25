using System;
using UnityEngine;

// Token: 0x02000CB0 RID: 3248
public class PrioritizeRowTableColumn : TableColumn
{
	// Token: 0x06006404 RID: 25604 RVA: 0x00254F74 File Offset: 0x00253174
	public PrioritizeRowTableColumn(object user_data, Action<object, int> on_change_priority, Func<object, int, string> on_hover_widget) : base(null, null, null, null, null, false, "")
	{
		this.userData = user_data;
		this.onChangePriority = on_change_priority;
		this.onHoverWidget = on_hover_widget;
	}

	// Token: 0x06006405 RID: 25605 RVA: 0x00254F9C File Offset: 0x0025319C
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06006406 RID: 25606 RVA: 0x00254FA5 File Offset: 0x002531A5
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06006407 RID: 25607 RVA: 0x00254FAE File Offset: 0x002531AE
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowHeaderWidget, parent, true);
	}

	// Token: 0x06006408 RID: 25608 RVA: 0x00254FC8 File Offset: 0x002531C8
	private GameObject GetWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowWidget, parent, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		this.ConfigureButton(component, "UpButton", 1, gameObject);
		this.ConfigureButton(component, "DownButton", -1, gameObject);
		return gameObject;
	}

	// Token: 0x06006409 RID: 25609 RVA: 0x00255010 File Offset: 0x00253210
	private void ConfigureButton(HierarchyReferences refs, string ref_id, int delta, GameObject widget_go)
	{
		KButton kbutton = refs.GetReference(ref_id) as KButton;
		kbutton.onClick += delegate()
		{
			this.onChangePriority(widget_go, delta);
		};
		ToolTip component = kbutton.GetComponent<ToolTip>();
		if (component != null)
		{
			component.OnToolTip = (() => this.onHoverWidget(widget_go, delta));
		}
	}

	// Token: 0x040043F6 RID: 17398
	public object userData;

	// Token: 0x040043F7 RID: 17399
	private Action<object, int> onChangePriority;

	// Token: 0x040043F8 RID: 17400
	private Func<object, int, string> onHoverWidget;
}
