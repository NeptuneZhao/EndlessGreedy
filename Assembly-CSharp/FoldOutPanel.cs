using System;
using UnityEngine;

// Token: 0x02000C48 RID: 3144
public class FoldOutPanel : KMonoBehaviour
{
	// Token: 0x0600609C RID: 24732 RVA: 0x0023F368 File Offset: 0x0023D568
	protected override void OnSpawn()
	{
		MultiToggle componentInChildren = base.GetComponentInChildren<MultiToggle>();
		componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(this.OnClick));
		this.ToggleOpen(this.startOpen);
	}

	// Token: 0x0600609D RID: 24733 RVA: 0x0023F39D File Offset: 0x0023D59D
	private void OnClick()
	{
		this.ToggleOpen(!this.panelOpen);
	}

	// Token: 0x0600609E RID: 24734 RVA: 0x0023F3AE File Offset: 0x0023D5AE
	private void ToggleOpen(bool open)
	{
		this.panelOpen = open;
		this.container.SetActive(this.panelOpen);
		base.GetComponentInChildren<MultiToggle>().ChangeState(this.panelOpen ? 1 : 0);
	}

	// Token: 0x04004153 RID: 16723
	private bool panelOpen = true;

	// Token: 0x04004154 RID: 16724
	public GameObject container;

	// Token: 0x04004155 RID: 16725
	public bool startOpen = true;
}
