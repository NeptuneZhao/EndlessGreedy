using System;
using UnityEngine;

// Token: 0x02000CB8 RID: 3256
public class DividerColumn : TableColumn
{
	// Token: 0x06006486 RID: 25734 RVA: 0x002596C4 File Offset: 0x002578C4
	public DividerColumn(Func<bool> revealed = null, string scrollerID = "") : base(delegate(IAssignableIdentity minion, GameObject widget_go)
	{
		if (revealed != null)
		{
			if (revealed())
			{
				if (!widget_go.activeSelf)
				{
					widget_go.SetActive(true);
					return;
				}
			}
			else if (widget_go.activeSelf)
			{
				widget_go.SetActive(false);
				return;
			}
		}
		else
		{
			widget_go.SetActive(true);
		}
	}, null, null, null, revealed, false, scrollerID)
	{
	}

	// Token: 0x06006487 RID: 25735 RVA: 0x002596FB File Offset: 0x002578FB
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x06006488 RID: 25736 RVA: 0x00259713 File Offset: 0x00257913
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x06006489 RID: 25737 RVA: 0x0025972B File Offset: 0x0025792B
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}
}
