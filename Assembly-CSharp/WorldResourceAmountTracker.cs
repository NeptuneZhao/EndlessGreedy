using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000B67 RID: 2919
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class WorldResourceAmountTracker<T> : KMonoBehaviour where T : KMonoBehaviour
{
	// Token: 0x060057B0 RID: 22448 RVA: 0x001F575A File Offset: 0x001F395A
	public static void DestroyInstance()
	{
		WorldResourceAmountTracker<T>.instance = default(T);
	}

	// Token: 0x060057B1 RID: 22449 RVA: 0x001F5767 File Offset: 0x001F3967
	public static T Get()
	{
		return WorldResourceAmountTracker<T>.instance;
	}

	// Token: 0x060057B2 RID: 22450 RVA: 0x001F5770 File Offset: 0x001F3970
	protected override void OnPrefabInit()
	{
		Debug.Assert(WorldResourceAmountTracker<T>.instance == null, "Error, WorldResourceAmountTracker of type T has already been initialize and another instance is attempting to initialize. this isn't allowed because T is meant to be a singleton, ensure only one instance exist. existing instance GameObject: " + ((WorldResourceAmountTracker<T>.instance == null) ? "" : WorldResourceAmountTracker<T>.instance.gameObject.name) + ". Error triggered by instance of T in GameObject: " + base.gameObject.name);
		WorldResourceAmountTracker<T>.instance = (this as T);
		this.itemTag = GameTags.Edible;
	}

	// Token: 0x060057B3 RID: 22451 RVA: 0x001F57F4 File Offset: 0x001F39F4
	protected override void OnSpawn()
	{
		base.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x060057B4 RID: 22452 RVA: 0x001F580E File Offset: 0x001F3A0E
	private void OnNewDay(object data)
	{
		this.previousFrame = this.currentFrame;
		this.currentFrame = default(WorldResourceAmountTracker<T>.Frame);
	}

	// Token: 0x060057B5 RID: 22453
	protected abstract WorldResourceAmountTracker<T>.ItemData GetItemData(Pickupable item);

	// Token: 0x060057B6 RID: 22454 RVA: 0x001F5828 File Offset: 0x001F3A28
	public float CountAmount(Dictionary<string, float> unitCountByID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					WorldResourceAmountTracker<T>.ItemData itemData = this.GetItemData(pickupable);
					num += itemData.amountValue;
					if (unitCountByID != null)
					{
						if (!unitCountByID.ContainsKey(itemData.ID))
						{
							unitCountByID[itemData.ID] = 0f;
						}
						string id = itemData.ID;
						unitCountByID[id] += itemData.units;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x060057B7 RID: 22455 RVA: 0x001F58F4 File Offset: 0x001F3AF4
	public void RegisterAmountProduced(float val)
	{
		this.currentFrame.amountProduced = this.currentFrame.amountProduced + val;
	}

	// Token: 0x060057B8 RID: 22456 RVA: 0x001F5908 File Offset: 0x001F3B08
	public void RegisterAmountConsumed(string ID, float valueConsumed)
	{
		this.currentFrame.amountConsumed = this.currentFrame.amountConsumed + valueConsumed;
		if (!this.amountsConsumedByID.ContainsKey(ID))
		{
			this.amountsConsumedByID.Add(ID, valueConsumed);
			return;
		}
		Dictionary<string, float> dictionary = this.amountsConsumedByID;
		dictionary[ID] += valueConsumed;
	}

	// Token: 0x0400394F RID: 14671
	private static T instance;

	// Token: 0x04003950 RID: 14672
	[Serialize]
	public WorldResourceAmountTracker<T>.Frame currentFrame;

	// Token: 0x04003951 RID: 14673
	[Serialize]
	public WorldResourceAmountTracker<T>.Frame previousFrame;

	// Token: 0x04003952 RID: 14674
	[Serialize]
	public Dictionary<string, float> amountsConsumedByID = new Dictionary<string, float>();

	// Token: 0x04003953 RID: 14675
	protected Tag itemTag;

	// Token: 0x02001BC1 RID: 7105
	protected struct ItemData
	{
		// Token: 0x040080A0 RID: 32928
		public string ID;

		// Token: 0x040080A1 RID: 32929
		public float amountValue;

		// Token: 0x040080A2 RID: 32930
		public float units;
	}

	// Token: 0x02001BC2 RID: 7106
	public struct Frame
	{
		// Token: 0x040080A3 RID: 32931
		public float amountProduced;

		// Token: 0x040080A4 RID: 32932
		public float amountConsumed;
	}
}
