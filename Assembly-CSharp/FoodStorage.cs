using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000569 RID: 1385
public class FoodStorage : KMonoBehaviour
{
	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06002014 RID: 8212 RVA: 0x000B4A5F File Offset: 0x000B2C5F
	// (set) Token: 0x06002015 RID: 8213 RVA: 0x000B4A67 File Offset: 0x000B2C67
	public FilteredStorage FilteredStorage { get; set; }

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06002016 RID: 8214 RVA: 0x000B4A70 File Offset: 0x000B2C70
	// (set) Token: 0x06002017 RID: 8215 RVA: 0x000B4A78 File Offset: 0x000B2C78
	public bool SpicedFoodOnly
	{
		get
		{
			return this.onlyStoreSpicedFood;
		}
		set
		{
			this.onlyStoreSpicedFood = value;
			base.Trigger(1163645216, this.onlyStoreSpicedFood);
			if (this.onlyStoreSpicedFood)
			{
				this.FilteredStorage.AddForbiddenTag(GameTags.UnspicedFood);
				this.storage.DropHasTags(new Tag[]
				{
					GameTags.Edible,
					GameTags.UnspicedFood
				});
				return;
			}
			this.FilteredStorage.RemoveForbiddenTag(GameTags.UnspicedFood);
		}
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x000B4AF5 File Offset: 0x000B2CF5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FoodStorage>(-905833192, FoodStorage.OnCopySettingsDelegate);
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000B4B0E File Offset: 0x000B2D0E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000B4B18 File Offset: 0x000B2D18
	private void OnCopySettings(object data)
	{
		FoodStorage component = ((GameObject)data).GetComponent<FoodStorage>();
		if (component != null)
		{
			this.SpicedFoodOnly = component.SpicedFoodOnly;
		}
	}

	// Token: 0x04001227 RID: 4647
	[Serialize]
	private bool onlyStoreSpicedFood;

	// Token: 0x04001228 RID: 4648
	[MyCmpReq]
	public Storage storage;

	// Token: 0x0400122A RID: 4650
	private static readonly EventSystem.IntraObjectHandler<FoodStorage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FoodStorage>(delegate(FoodStorage component, object data)
	{
		component.OnCopySettings(data);
	});
}
