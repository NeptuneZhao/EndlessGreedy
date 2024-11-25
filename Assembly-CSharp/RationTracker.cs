using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000A13 RID: 2579
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/RationTracker")]
public class RationTracker : WorldResourceAmountTracker<RationTracker>, ISaveLoadable
{
	// Token: 0x06004AC6 RID: 19142 RVA: 0x001AB6E0 File Offset: 0x001A98E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.itemTag = GameTags.Edible;
	}

	// Token: 0x06004AC7 RID: 19143 RVA: 0x001AB6F4 File Offset: 0x001A98F4
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.caloriesConsumedByFood != null && this.caloriesConsumedByFood.Count > 0)
		{
			foreach (string key in this.caloriesConsumedByFood.Keys)
			{
				float num = this.caloriesConsumedByFood[key];
				float num2 = 0f;
				if (this.amountsConsumedByID.TryGetValue(key, out num2))
				{
					this.amountsConsumedByID[key] = num2 + num;
				}
				else
				{
					this.amountsConsumedByID.Add(key, num);
				}
			}
		}
		this.caloriesConsumedByFood = null;
	}

	// Token: 0x06004AC8 RID: 19144 RVA: 0x001AB7A8 File Offset: 0x001A99A8
	protected override WorldResourceAmountTracker<RationTracker>.ItemData GetItemData(Pickupable item)
	{
		Edible component = item.GetComponent<Edible>();
		return new WorldResourceAmountTracker<RationTracker>.ItemData
		{
			ID = component.FoodID,
			amountValue = component.Calories,
			units = component.Units
		};
	}

	// Token: 0x06004AC9 RID: 19145 RVA: 0x001AB7EC File Offset: 0x001A99EC
	public float GetAmountConsumed()
	{
		float num = 0f;
		foreach (KeyValuePair<string, float> keyValuePair in this.amountsConsumedByID)
		{
			num += keyValuePair.Value;
		}
		return num;
	}

	// Token: 0x06004ACA RID: 19146 RVA: 0x001AB84C File Offset: 0x001A9A4C
	public float GetAmountConsumedForIDs(List<string> itemIDs)
	{
		float num = 0f;
		foreach (string key in itemIDs)
		{
			if (this.amountsConsumedByID.ContainsKey(key))
			{
				num += this.amountsConsumedByID[key];
			}
		}
		return num;
	}

	// Token: 0x06004ACB RID: 19147 RVA: 0x001AB8B8 File Offset: 0x001A9AB8
	public float CountAmountForItemWithID(string ID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					WorldResourceAmountTracker<RationTracker>.ItemData itemData = this.GetItemData(pickupable);
					if (itemData.ID == ID)
					{
						num += itemData.amountValue;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x04003104 RID: 12548
	[Serialize]
	public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();
}
