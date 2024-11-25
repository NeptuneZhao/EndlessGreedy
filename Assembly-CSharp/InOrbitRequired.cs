using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000570 RID: 1392
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InOrbitRequired")]
public class InOrbitRequired : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x0600203C RID: 8252 RVA: 0x000B4E5C File Offset: 0x000B305C
	protected override void OnSpawn()
	{
		WorldContainer myWorld = this.GetMyWorld();
		this.craftModuleInterface = myWorld.GetComponent<CraftModuleInterface>();
		base.OnSpawn();
		bool newInOrbit = this.craftModuleInterface.HasTag(GameTags.RocketNotOnGround);
		this.UpdateFlag(newInOrbit);
		this.craftModuleInterface.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x000B4EB7 File Offset: 0x000B30B7
	protected override void OnCleanUp()
	{
		if (this.craftModuleInterface != null)
		{
			this.craftModuleInterface.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x000B4EE4 File Offset: 0x000B30E4
	private void OnTagsChanged(object data)
	{
		TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
		if (tagChangedEventData.tag == GameTags.RocketNotOnGround)
		{
			this.UpdateFlag(tagChangedEventData.added);
		}
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x000B4F16 File Offset: 0x000B3116
	private void UpdateFlag(bool newInOrbit)
	{
		this.operational.SetFlag(InOrbitRequired.inOrbitFlag, newInOrbit);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InOrbitRequired, !newInOrbit, this);
	}

	// Token: 0x06002040 RID: 8256 RVA: 0x000B4F49 File Offset: 0x000B3149
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.IN_ORBIT_REQUIRED, UI.BUILDINGEFFECTS.TOOLTIPS.IN_ORBIT_REQUIRED, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04001231 RID: 4657
	[MyCmpReq]
	private Building building;

	// Token: 0x04001232 RID: 4658
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001233 RID: 4659
	public static readonly Operational.Flag inOrbitFlag = new Operational.Flag("in_orbit", Operational.Flag.Type.Requirement);

	// Token: 0x04001234 RID: 4660
	private CraftModuleInterface craftModuleInterface;
}
