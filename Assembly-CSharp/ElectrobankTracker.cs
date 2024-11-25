using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000871 RID: 2161
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElectrobankTracker")]
public class ElectrobankTracker : WorldResourceAmountTracker<ElectrobankTracker>, ISaveLoadable
{
	// Token: 0x06003C44 RID: 15428 RVA: 0x0014E532 File Offset: 0x0014C732
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.itemTag = GameTags.ChargedPortableBattery;
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x0014E548 File Offset: 0x0014C748
	protected override WorldResourceAmountTracker<ElectrobankTracker>.ItemData GetItemData(Pickupable item)
	{
		Electrobank component = item.GetComponent<Electrobank>();
		return new WorldResourceAmountTracker<ElectrobankTracker>.ItemData
		{
			ID = component.ID,
			amountValue = component.Charge * item.PrimaryElement.Units,
			units = item.PrimaryElement.Units
		};
	}
}
