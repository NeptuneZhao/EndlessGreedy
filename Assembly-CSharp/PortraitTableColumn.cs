using System;
using UnityEngine;

// Token: 0x02000CAE RID: 3246
public class PortraitTableColumn : TableColumn
{
	// Token: 0x060063FB RID: 25595 RVA: 0x00254C6C File Offset: 0x00252E6C
	public PortraitTableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, bool double_click_to_target = true) : base(on_load_action, sort_comparison, null, null, null, false, "")
	{
		this.double_click_to_target = double_click_to_target;
	}

	// Token: 0x060063FC RID: 25596 RVA: 0x00254C9B File Offset: 0x00252E9B
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
		gameObject.GetComponent<CrewPortrait>().targetImage.enabled = true;
		return gameObject;
	}

	// Token: 0x060063FD RID: 25597 RVA: 0x00254CBB File Offset: 0x00252EBB
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(this.prefab_portrait, parent, true);
	}

	// Token: 0x060063FE RID: 25598 RVA: 0x00254CCC File Offset: 0x00252ECC
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
		if (this.double_click_to_target)
		{
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				parent.GetComponent<TableRow>().SelectMinion();
			};
			gameObject.GetComponent<KButton>().onDoubleClick += delegate()
			{
				parent.GetComponent<TableRow>().SelectAndFocusMinion();
			};
		}
		return gameObject;
	}

	// Token: 0x040043EE RID: 17390
	public GameObject prefab_portrait = Assets.UIPrefabs.TableScreenWidgets.MinionPortrait;

	// Token: 0x040043EF RID: 17391
	private bool double_click_to_target;
}
