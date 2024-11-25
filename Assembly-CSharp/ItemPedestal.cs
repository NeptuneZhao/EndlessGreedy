using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020006F4 RID: 1780
[AddComponentMenu("KMonoBehaviour/scripts/ItemPedestal")]
public class ItemPedestal : KMonoBehaviour
{
	// Token: 0x06002D79 RID: 11641 RVA: 0x000FF3B4 File Offset: 0x000FD5B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ItemPedestal>(-731304873, ItemPedestal.OnOccupantChangedDelegate);
		if (this.receptacle.Occupant)
		{
			KBatchedAnimController component = this.receptacle.Occupant.GetComponent<KBatchedAnimController>();
			if (component)
			{
				component.enabled = true;
				component.sceneLayer = Grid.SceneLayer.Move;
			}
			this.OnOccupantChanged(this.receptacle.Occupant);
		}
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x000FF424 File Offset: 0x000FD624
	private void OnOccupantChanged(object data)
	{
		Attributes attributes = this.GetAttributes();
		if (this.decorModifier != null)
		{
			attributes.Remove(this.decorModifier);
			attributes.Remove(this.decorRadiusModifier);
			this.decorModifier = null;
			this.decorRadiusModifier = null;
		}
		if (data != null)
		{
			GameObject gameObject = (GameObject)data;
			UnityEngine.Object component = gameObject.GetComponent<DecorProvider>();
			float value = 5f;
			float value2 = 3f;
			if (component != null)
			{
				value = Mathf.Max(Db.Get().BuildingAttributes.Decor.Lookup(gameObject).GetTotalValue() * 2f, 5f);
				value2 = Db.Get().BuildingAttributes.DecorRadius.Lookup(gameObject).GetTotalValue() + 2f;
			}
			string description = string.Format(BUILDINGS.PREFABS.ITEMPEDESTAL.DISPLAYED_ITEM_FMT, gameObject.GetComponent<KPrefabID>().PrefabTag.ProperName());
			this.decorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, value, description, false, false, true);
			this.decorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, value2, description, false, false, true);
			attributes.Add(this.decorModifier);
			attributes.Add(this.decorRadiusModifier);
		}
	}

	// Token: 0x04001A5E RID: 6750
	[MyCmpReq]
	protected SingleEntityReceptacle receptacle;

	// Token: 0x04001A5F RID: 6751
	[MyCmpReq]
	private DecorProvider decorProvider;

	// Token: 0x04001A60 RID: 6752
	private const float MINIMUM_DECOR = 5f;

	// Token: 0x04001A61 RID: 6753
	private const float STORED_DECOR_MODIFIER = 2f;

	// Token: 0x04001A62 RID: 6754
	private const int RADIUS_BONUS = 2;

	// Token: 0x04001A63 RID: 6755
	private AttributeModifier decorModifier;

	// Token: 0x04001A64 RID: 6756
	private AttributeModifier decorRadiusModifier;

	// Token: 0x04001A65 RID: 6757
	private static readonly EventSystem.IntraObjectHandler<ItemPedestal> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<ItemPedestal>(delegate(ItemPedestal component, object data)
	{
		component.OnOccupantChanged(data);
	});
}
