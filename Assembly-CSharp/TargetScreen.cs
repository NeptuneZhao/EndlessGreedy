using System;
using UnityEngine;

// Token: 0x02000DD4 RID: 3540
public abstract class TargetScreen : KScreen
{
	// Token: 0x0600707E RID: 28798
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x0600707F RID: 28799 RVA: 0x002A8FA4 File Offset: 0x002A71A4
	public virtual void SetTarget(GameObject target)
	{
		Console.WriteLine(target);
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

	// Token: 0x06007080 RID: 28800 RVA: 0x002A9005 File Offset: 0x002A7205
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		this.SetTarget(null);
	}

	// Token: 0x06007081 RID: 28801 RVA: 0x002A9014 File Offset: 0x002A7214
	public virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x06007082 RID: 28802 RVA: 0x002A902E File Offset: 0x002A722E
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x06007083 RID: 28803 RVA: 0x002A9047 File Offset: 0x002A7247
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x04004D52 RID: 19794
	protected GameObject selectedTarget;
}
