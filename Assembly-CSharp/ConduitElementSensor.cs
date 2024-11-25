using System;
using KSerialization;

// Token: 0x020006AC RID: 1708
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitElementSensor : ConduitSensor
{
	// Token: 0x06002B01 RID: 11009 RVA: 0x000F197A File Offset: 0x000EFB7A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filterable.onFilterChanged += this.OnFilterChanged;
		this.OnFilterChanged(this.filterable.SelectedTag);
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x000F19AC File Offset: 0x000EFBAC
	private void OnFilterChanged(Tag tag)
	{
		if (!tag.IsValid)
		{
			return;
		}
		bool on = tag == GameTags.Void;
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x000F19EC File Offset: 0x000EFBEC
	protected override void ConduitUpdate(float dt)
	{
		Tag a;
		bool flag;
		this.GetContentsElement(out a, out flag);
		if (!base.IsSwitchedOn)
		{
			if (a == this.filterable.SelectedTag && flag)
			{
				this.Toggle();
				return;
			}
		}
		else if (a != this.filterable.SelectedTag || !flag)
		{
			this.Toggle();
		}
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x000F1A44 File Offset: 0x000EFC44
	private void GetContentsElement(out Tag element, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			element = contents.element.CreateTag();
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		KPrefabID kprefabID = (pickupable != null) ? pickupable.GetComponent<KPrefabID>() : null;
		if (kprefabID != null && pickupable.PrimaryElement.Mass > 0f)
		{
			element = kprefabID.PrefabTag;
			hasMass = true;
			return;
		}
		element = GameTags.Void;
		hasMass = false;
	}

	// Token: 0x040018B1 RID: 6321
	[MyCmpGet]
	private Filterable filterable;
}
