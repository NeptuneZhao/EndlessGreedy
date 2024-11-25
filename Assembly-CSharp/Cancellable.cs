using System;
using UnityEngine;

// Token: 0x02000535 RID: 1333
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Cancellable")]
public class Cancellable : KMonoBehaviour
{
	// Token: 0x06001E5F RID: 7775 RVA: 0x000A9460 File Offset: 0x000A7660
	protected override void OnPrefabInit()
	{
		base.Subscribe<Cancellable>(2127324410, Cancellable.OnCancelDelegate);
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x000A9473 File Offset: 0x000A7673
	protected virtual void OnCancel(object data)
	{
		this.DeleteObject();
	}

	// Token: 0x04001122 RID: 4386
	private static readonly EventSystem.IntraObjectHandler<Cancellable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Cancellable>(delegate(Cancellable component, object data)
	{
		component.OnCancel(data);
	});
}
