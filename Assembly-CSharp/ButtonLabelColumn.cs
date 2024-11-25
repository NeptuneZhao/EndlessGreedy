using System;
using UnityEngine;

// Token: 0x02000CAC RID: 3244
public class ButtonLabelColumn : LabelTableColumn
{
	// Token: 0x060063F2 RID: 25586 RVA: 0x00254843 File Offset: 0x00252A43
	public ButtonLabelColumn(Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Action<GameObject> on_click_action, Action<GameObject> on_double_click_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, bool whiteText = false) : base(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, 128, false)
	{
		this.on_click_action = on_click_action;
		this.on_double_click_action = on_double_click_action;
		this.whiteText = whiteText;
	}

	// Token: 0x060063F3 RID: 25587 RVA: 0x00254870 File Offset: 0x00252A70
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite : Assets.UIPrefabs.TableScreenWidgets.ButtonLabel, parent, true);
		if (this.on_click_action != null)
		{
			widget_go.GetComponent<KButton>().onClick += delegate()
			{
				this.on_click_action(widget_go);
			};
		}
		if (this.on_double_click_action != null)
		{
			widget_go.GetComponent<KButton>().onDoubleClick += delegate()
			{
				this.on_double_click_action(widget_go);
			};
		}
		return widget_go;
	}

	// Token: 0x060063F4 RID: 25588 RVA: 0x0025490D File Offset: 0x00252B0D
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return base.GetHeaderWidget(parent);
	}

	// Token: 0x060063F5 RID: 25589 RVA: 0x00254918 File Offset: 0x00252B18
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite : Assets.UIPrefabs.TableScreenWidgets.ButtonLabel, parent, true);
		ToolTip tt = widget_go.GetComponent<ToolTip>();
		tt.OnToolTip = (() => this.GetTooltip(tt));
		if (this.on_click_action != null)
		{
			widget_go.GetComponent<KButton>().onClick += delegate()
			{
				this.on_click_action(widget_go);
			};
		}
		if (this.on_double_click_action != null)
		{
			widget_go.GetComponent<KButton>().onDoubleClick += delegate()
			{
				this.on_double_click_action(widget_go);
			};
		}
		return widget_go;
	}

	// Token: 0x040043E7 RID: 17383
	private Action<GameObject> on_click_action;

	// Token: 0x040043E8 RID: 17384
	private Action<GameObject> on_double_click_action;

	// Token: 0x040043E9 RID: 17385
	private bool whiteText;
}
