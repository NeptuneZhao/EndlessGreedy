using System;
using System.Collections.Generic;

// Token: 0x02000C44 RID: 3140
public class FabricatorListScreen : KToggleMenu
{
	// Token: 0x0600607C RID: 24700 RVA: 0x0023E764 File Offset: 0x0023C964
	private void Refresh()
	{
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Fabricator fabricator in Components.Fabricators.Items)
		{
			KSelectable component = fabricator.GetComponent<KSelectable>();
			list.Add(new KToggleMenu.ToggleInfo(component.GetName(), fabricator, global::Action.NumActions));
		}
		base.Setup(list);
	}

	// Token: 0x0600607D RID: 24701 RVA: 0x0023E7E0 File Offset: 0x0023C9E0
	protected override void OnSpawn()
	{
		base.onSelect += this.OnClickFabricator;
	}

	// Token: 0x0600607E RID: 24702 RVA: 0x0023E7F4 File Offset: 0x0023C9F4
	protected override void OnActivate()
	{
		base.OnActivate();
		this.Refresh();
	}

	// Token: 0x0600607F RID: 24703 RVA: 0x0023E804 File Offset: 0x0023CA04
	private void OnClickFabricator(KToggleMenu.ToggleInfo toggle_info)
	{
		Fabricator fabricator = (Fabricator)toggle_info.userData;
		SelectTool.Instance.Select(fabricator.GetComponent<KSelectable>(), false);
	}
}
