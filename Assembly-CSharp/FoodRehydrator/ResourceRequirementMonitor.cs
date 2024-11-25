using System;

namespace FoodRehydrator
{
	// Token: 0x02000E2B RID: 3627
	public class ResourceRequirementMonitor : KMonoBehaviour
	{
		// Token: 0x060073BD RID: 29629 RVA: 0x002C47DC File Offset: 0x002C29DC
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Storage[] components = base.GetComponents<Storage>();
			DebugUtil.DevAssert(components.Length == 2, "Incorrect number of storages on foodrehydrator", null);
			this.packages = components[0];
			this.water = components[1];
			base.Subscribe<ResourceRequirementMonitor>(-1697596308, ResourceRequirementMonitor.OnStorageChangedDelegate);
		}

		// Token: 0x060073BE RID: 29630 RVA: 0x002C482A File Offset: 0x002C2A2A
		protected float GetAvailableWater()
		{
			return this.water.GetMassAvailable(GameTags.Water);
		}

		// Token: 0x060073BF RID: 29631 RVA: 0x002C483C File Offset: 0x002C2A3C
		protected bool HasSufficientResources()
		{
			return this.packages.items.Count > 0 && this.GetAvailableWater() > 1f;
		}

		// Token: 0x060073C0 RID: 29632 RVA: 0x002C4860 File Offset: 0x002C2A60
		protected void OnStorageChanged(object _)
		{
			this.operational.SetFlag(ResourceRequirementMonitor.flag, this.HasSufficientResources());
		}

		// Token: 0x04004FAC RID: 20396
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04004FAD RID: 20397
		private Storage packages;

		// Token: 0x04004FAE RID: 20398
		private Storage water;

		// Token: 0x04004FAF RID: 20399
		private static readonly Operational.Flag flag = new Operational.Flag("HasSufficientResources", Operational.Flag.Type.Requirement);

		// Token: 0x04004FB0 RID: 20400
		private static readonly EventSystem.IntraObjectHandler<ResourceRequirementMonitor> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ResourceRequirementMonitor>(delegate(ResourceRequirementMonitor component, object data)
		{
			component.OnStorageChanged(data);
		});
	}
}
