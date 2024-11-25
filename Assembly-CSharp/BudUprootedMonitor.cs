using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
[AddComponentMenu("KMonoBehaviour/scripts/BudUprootedMonitor")]
public class BudUprootedMonitor : KMonoBehaviour
{
	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06003840 RID: 14400 RVA: 0x0013344F File Offset: 0x0013164F
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x0013346B File Offset: 0x0013166B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<BudUprootedMonitor>(-216549700, BudUprootedMonitor.OnUprootedDelegate);
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x00133484 File Offset: 0x00131684
	public void SetParentObject(KPrefabID id)
	{
		this.parentObject = new Ref<KPrefabID>(id);
		base.Subscribe(id.gameObject, 1969584890, new Action<object>(this.OnLoseParent));
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x001334B0 File Offset: 0x001316B0
	private void OnLoseParent(object obj)
	{
		if (!this.uprooted && !base.isNull)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			this.uprooted = true;
			base.Trigger(-216549700, null);
			if (this.destroyOnParentLost)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x00133504 File Offset: 0x00131704
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x0013350C File Offset: 0x0013170C
	public static bool IsObjectUprooted(GameObject plant)
	{
		BudUprootedMonitor component = plant.GetComponent<BudUprootedMonitor>();
		return !(component == null) && component.IsUprooted;
	}

	// Token: 0x040021CA RID: 8650
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x040021CB RID: 8651
	[Serialize]
	private bool uprooted;

	// Token: 0x040021CC RID: 8652
	public bool destroyOnParentLost;

	// Token: 0x040021CD RID: 8653
	public Ref<KPrefabID> parentObject = new Ref<KPrefabID>();

	// Token: 0x040021CE RID: 8654
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040021CF RID: 8655
	private static readonly EventSystem.IntraObjectHandler<BudUprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BudUprootedMonitor>(delegate(BudUprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
