using System;
using UnityEngine;

// Token: 0x020009E0 RID: 2528
public class OxygenMask : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004960 RID: 18784 RVA: 0x001A446B File Offset: 0x001A266B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxygenMask>(608245985, OxygenMask.OnSuitTankDeltaDelegate);
	}

	// Token: 0x06004961 RID: 18785 RVA: 0x001A4484 File Offset: 0x001A2684
	private void CheckOxygenLevels(object data)
	{
		if (this.suitTank.IsEmpty())
		{
			Equippable component = base.GetComponent<Equippable>();
			if (component.assignee != null)
			{
				Ownables soleOwner = component.assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					soleOwner.GetComponent<Equipment>().Unequip(component);
				}
			}
		}
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x001A44D0 File Offset: 0x001A26D0
	public void Sim200ms(float dt)
	{
		if (base.GetComponent<Equippable>().assignee == null)
		{
			float num = this.leakRate * dt;
			float massAvailable = this.storage.GetMassAvailable(this.suitTank.elementTag);
			num = Mathf.Min(num, massAvailable);
			this.storage.DropSome(this.suitTank.elementTag, num, true, true, default(Vector3), true, false);
		}
		if (this.suitTank.IsEmpty())
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x04003000 RID: 12288
	private static readonly EventSystem.IntraObjectHandler<OxygenMask> OnSuitTankDeltaDelegate = new EventSystem.IntraObjectHandler<OxygenMask>(delegate(OxygenMask component, object data)
	{
		component.CheckOxygenLevels(data);
	});

	// Token: 0x04003001 RID: 12289
	[MyCmpGet]
	private SuitTank suitTank;

	// Token: 0x04003002 RID: 12290
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003003 RID: 12291
	private float leakRate = 0.1f;
}
