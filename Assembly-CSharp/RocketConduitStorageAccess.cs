using System;
using UnityEngine;

// Token: 0x02000A67 RID: 2663
public class RocketConduitStorageAccess : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004D49 RID: 19785 RVA: 0x001BB2D4 File Offset: 0x001B94D4
	protected override void OnSpawn()
	{
		WorldContainer myWorld = this.GetMyWorld();
		this.craftModuleInterface = myWorld.GetComponent<CraftModuleInterface>();
	}

	// Token: 0x06004D4A RID: 19786 RVA: 0x001BB2F4 File Offset: 0x001B94F4
	public void Sim200ms(float dt)
	{
		if (this.operational != null && !this.operational.IsOperational)
		{
			return;
		}
		float num = this.storage.MassStored();
		if (num < this.targetLevel - 0.01f || num > this.targetLevel + 0.01f)
		{
			if (this.operational != null)
			{
				this.operational.SetActive(true, false);
			}
			float num2 = this.targetLevel - num;
			foreach (Ref<RocketModuleCluster> @ref in this.craftModuleInterface.ClusterModules)
			{
				CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
				if (component != null && component.storageType == this.cargoType)
				{
					if (num2 > 0f && component.storage.MassStored() > 0f)
					{
						for (int i = component.storage.items.Count - 1; i >= 0; i--)
						{
							GameObject gameObject = component.storage.items[i];
							if (!(this.filterable != null) || !(this.filterable.SelectedTag != GameTags.Void) || !(gameObject.PrefabID() != this.filterable.SelectedTag))
							{
								Pickupable pickupable = gameObject.GetComponent<Pickupable>().Take(num2);
								if (pickupable != null)
								{
									num2 -= pickupable.PrimaryElement.Mass;
									this.storage.Store(pickupable.gameObject, true, false, true, false);
								}
								if (num2 <= 0f)
								{
									break;
								}
							}
						}
						if (num2 <= 0f)
						{
							break;
						}
					}
					if (num2 < 0f && component.storage.RemainingCapacity() > 0f)
					{
						Mathf.Min(-num2, component.storage.RemainingCapacity());
						for (int j = this.storage.items.Count - 1; j >= 0; j--)
						{
							Pickupable pickupable2 = this.storage.items[j].GetComponent<Pickupable>().Take(-num2);
							if (pickupable2 != null)
							{
								num2 += pickupable2.PrimaryElement.Mass;
								component.storage.Store(pickupable2.gameObject, true, false, true, false);
							}
							if (num2 >= 0f)
							{
								break;
							}
						}
						if (num2 >= 0f)
						{
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x04003358 RID: 13144
	[SerializeField]
	public Storage storage;

	// Token: 0x04003359 RID: 13145
	[SerializeField]
	public float targetLevel;

	// Token: 0x0400335A RID: 13146
	[SerializeField]
	public CargoBay.CargoType cargoType;

	// Token: 0x0400335B RID: 13147
	[MyCmpGet]
	private Filterable filterable;

	// Token: 0x0400335C RID: 13148
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400335D RID: 13149
	private const float TOLERANCE = 0.01f;

	// Token: 0x0400335E RID: 13150
	private CraftModuleInterface craftModuleInterface;
}
