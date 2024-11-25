using System;
using UnityEngine;

// Token: 0x020005BA RID: 1466
[AddComponentMenu("KMonoBehaviour/scripts/SimpleVent")]
public class SimpleVent : KMonoBehaviour
{
	// Token: 0x060022FA RID: 8954 RVA: 0x000C3266 File Offset: 0x000C1466
	protected override void OnPrefabInit()
	{
		base.Subscribe<SimpleVent>(-592767678, SimpleVent.OnChangedDelegate);
		base.Subscribe<SimpleVent>(-111137758, SimpleVent.OnChangedDelegate);
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000C328A File Offset: 0x000C148A
	protected override void OnSpawn()
	{
		this.OnChanged(null);
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x000C3294 File Offset: 0x000C1494
	private void OnChanged(object data)
	{
		if (this.operational.IsFunctional)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
	}

	// Token: 0x040013DA RID: 5082
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040013DB RID: 5083
	private static readonly EventSystem.IntraObjectHandler<SimpleVent> OnChangedDelegate = new EventSystem.IntraObjectHandler<SimpleVent>(delegate(SimpleVent component, object data)
	{
		component.OnChanged(data);
	});
}
