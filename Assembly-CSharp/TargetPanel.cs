using System;
using UnityEngine;

// Token: 0x02000DD3 RID: 3539
public abstract class TargetPanel : KMonoBehaviour
{
	// Token: 0x06007078 RID: 28792
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x06007079 RID: 28793 RVA: 0x002A8EFC File Offset: 0x002A70FC
	public virtual void SetTarget(GameObject target)
	{
		if (this.selectedTarget != target)
		{
			if (this.selectedTarget != null)
			{
				this.OnDeselectTarget(this.selectedTarget);
			}
			this.selectedTarget = target;
			if (this.selectedTarget != null)
			{
				this.OnSelectTarget(this.selectedTarget);
			}
		}
	}

	// Token: 0x0600707A RID: 28794 RVA: 0x002A8F52 File Offset: 0x002A7152
	protected virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600707B RID: 28795 RVA: 0x002A8F6C File Offset: 0x002A716C
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600707C RID: 28796 RVA: 0x002A8F85 File Offset: 0x002A7185
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x04004D51 RID: 19793
	protected GameObject selectedTarget;
}
