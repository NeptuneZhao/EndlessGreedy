using System;
using UnityEngine;

// Token: 0x02000D10 RID: 3344
public class PlanSubCategoryToggle : KMonoBehaviour
{
	// Token: 0x06006860 RID: 26720 RVA: 0x0027166E File Offset: 0x0026F86E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.open = !this.open;
			this.gridContainer.SetActive(this.open);
			this.toggle.ChangeState(this.open ? 0 : 1);
		}));
	}

	// Token: 0x04004687 RID: 18055
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x04004688 RID: 18056
	[SerializeField]
	private GameObject gridContainer;

	// Token: 0x04004689 RID: 18057
	private bool open = true;
}
