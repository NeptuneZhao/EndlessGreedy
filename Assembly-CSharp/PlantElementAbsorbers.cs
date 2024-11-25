using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009E9 RID: 2537
public class PlantElementAbsorbers : KCompactedVector<PlantElementAbsorber>
{
	// Token: 0x0600498D RID: 18829 RVA: 0x001A52D8 File Offset: 0x001A34D8
	public HandleVector<int>.Handle Add(Storage storage, PlantElementAbsorber.ConsumeInfo[] consumed_elements)
	{
		if (consumed_elements == null || consumed_elements.Length == 0)
		{
			return HandleVector<int>.InvalidHandle;
		}
		HandleVector<int>.Handle[] array = new HandleVector<int>.Handle[consumed_elements.Length];
		for (int i = 0; i < consumed_elements.Length; i++)
		{
			array[i] = Game.Instance.accumulators.Add("ElementsConsumed", storage);
		}
		HandleVector<int>.Handle result = HandleVector<int>.InvalidHandle;
		if (consumed_elements.Length == 1)
		{
			result = base.Allocate(new PlantElementAbsorber
			{
				storage = storage,
				consumedElements = null,
				accumulators = array,
				localInfo = new PlantElementAbsorber.LocalInfo
				{
					tag = consumed_elements[0].tag,
					massConsumptionRate = consumed_elements[0].massConsumptionRate
				}
			});
		}
		else
		{
			result = base.Allocate(new PlantElementAbsorber
			{
				storage = storage,
				consumedElements = consumed_elements,
				accumulators = array,
				localInfo = new PlantElementAbsorber.LocalInfo
				{
					tag = Tag.Invalid,
					massConsumptionRate = 0f
				}
			});
		}
		return result;
	}

	// Token: 0x0600498E RID: 18830 RVA: 0x001A53E4 File Offset: 0x001A35E4
	public HandleVector<int>.Handle Remove(HandleVector<int>.Handle h)
	{
		if (this.updating)
		{
			this.queuedRemoves.Add(h);
		}
		else
		{
			base.Free(h);
		}
		return HandleVector<int>.InvalidHandle;
	}

	// Token: 0x0600498F RID: 18831 RVA: 0x001A540C File Offset: 0x001A360C
	public void Sim200ms(float dt)
	{
		int count = this.data.Count;
		this.updating = true;
		for (int i = 0; i < count; i++)
		{
			PlantElementAbsorber plantElementAbsorber = this.data[i];
			if (!(plantElementAbsorber.storage == null))
			{
				if (plantElementAbsorber.consumedElements == null)
				{
					float num = plantElementAbsorber.localInfo.massConsumptionRate * dt;
					PrimaryElement primaryElement = plantElementAbsorber.storage.FindFirstWithMass(plantElementAbsorber.localInfo.tag, 0f);
					if (primaryElement != null)
					{
						float num2 = Mathf.Min(num, primaryElement.Mass);
						primaryElement.Mass -= num2;
						num -= num2;
						Game.Instance.accumulators.Accumulate(plantElementAbsorber.accumulators[0], num2);
						plantElementAbsorber.storage.Trigger(-1697596308, primaryElement.gameObject);
					}
				}
				else
				{
					for (int j = 0; j < plantElementAbsorber.consumedElements.Length; j++)
					{
						float num3 = plantElementAbsorber.consumedElements[j].massConsumptionRate * dt;
						PrimaryElement primaryElement2 = plantElementAbsorber.storage.FindFirstWithMass(plantElementAbsorber.consumedElements[j].tag, 0f);
						while (primaryElement2 != null)
						{
							float num4 = Mathf.Min(num3, primaryElement2.Mass);
							primaryElement2.Mass -= num4;
							num3 -= num4;
							Game.Instance.accumulators.Accumulate(plantElementAbsorber.accumulators[j], num4);
							plantElementAbsorber.storage.Trigger(-1697596308, primaryElement2.gameObject);
							if (num3 <= 0f)
							{
								break;
							}
							primaryElement2 = plantElementAbsorber.storage.FindFirstWithMass(plantElementAbsorber.consumedElements[j].tag, 0f);
						}
					}
				}
				this.data[i] = plantElementAbsorber;
			}
		}
		this.updating = false;
		for (int k = 0; k < this.queuedRemoves.Count; k++)
		{
			HandleVector<int>.Handle h = this.queuedRemoves[k];
			this.Remove(h);
		}
		this.queuedRemoves.Clear();
	}

	// Token: 0x06004990 RID: 18832 RVA: 0x001A5640 File Offset: 0x001A3840
	public override void Clear()
	{
		base.Clear();
		for (int i = 0; i < this.data.Count; i++)
		{
			this.data[i].Clear();
		}
		this.data.Clear();
		this.handles.Clear();
	}

	// Token: 0x06004991 RID: 18833 RVA: 0x001A5693 File Offset: 0x001A3893
	public PlantElementAbsorbers() : base(0)
	{
	}

	// Token: 0x04003021 RID: 12321
	private bool updating;

	// Token: 0x04003022 RID: 12322
	private List<HandleVector<int>.Handle> queuedRemoves = new List<HandleVector<int>.Handle>();
}
