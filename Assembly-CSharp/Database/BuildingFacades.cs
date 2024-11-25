using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000E51 RID: 3665
	public class BuildingFacades : ResourceSet<BuildingFacadeResource>
	{
		// Token: 0x06007445 RID: 29765 RVA: 0x002C8240 File Offset: 0x002C6440
		public BuildingFacades(ResourceSet parent) : base("BuildingFacades", parent)
		{
			base.Initialize();
			foreach (BuildingFacadeInfo buildingFacadeInfo in Blueprints.Get().all.buildingFacades)
			{
				this.Add(buildingFacadeInfo.id, buildingFacadeInfo.name, buildingFacadeInfo.desc, buildingFacadeInfo.rarity, buildingFacadeInfo.prefabId, buildingFacadeInfo.animFile, buildingFacadeInfo.dlcIds, buildingFacadeInfo.workables);
			}
		}

		// Token: 0x06007446 RID: 29766 RVA: 0x002C82E8 File Offset: 0x002C64E8
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		public void Add(string id, LocString Name, LocString Desc, PermitRarity rarity, string prefabId, string animFile, Dictionary<string, string> workables = null)
		{
			this.Add(id, Name, Desc, rarity, prefabId, animFile, DlcManager.AVAILABLE_ALL_VERSIONS, workables);
		}

		// Token: 0x06007447 RID: 29767 RVA: 0x002C830C File Offset: 0x002C650C
		public void Add(string id, LocString Name, LocString Desc, PermitRarity rarity, string prefabId, string animFile, string[] dlcIds, Dictionary<string, string> workables = null)
		{
			BuildingFacadeResource item = new BuildingFacadeResource(id, Name, Desc, rarity, prefabId, animFile, dlcIds, workables);
			this.resources.Add(item);
		}

		// Token: 0x06007448 RID: 29768 RVA: 0x002C8344 File Offset: 0x002C6544
		public void PostProcess()
		{
			foreach (BuildingFacadeResource buildingFacadeResource in this.resources)
			{
				buildingFacadeResource.Init();
			}
		}
	}
}
