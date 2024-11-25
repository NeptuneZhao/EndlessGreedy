using System;

namespace Database
{
	// Token: 0x02000E61 RID: 3681
	public class EquippableFacades : ResourceSet<EquippableFacadeResource>
	{
		// Token: 0x0600747D RID: 29821 RVA: 0x002D4640 File Offset: 0x002D2840
		public EquippableFacades(ResourceSet parent) : base("EquippableFacades", parent)
		{
			base.Initialize();
			foreach (EquippableFacadeInfo equippableFacadeInfo in Blueprints.Get().all.equippableFacades)
			{
				this.Add(equippableFacadeInfo.id, equippableFacadeInfo.name, equippableFacadeInfo.desc, equippableFacadeInfo.rarity, equippableFacadeInfo.defID, equippableFacadeInfo.buildOverride, equippableFacadeInfo.animFile, equippableFacadeInfo.dlcIds);
			}
		}

		// Token: 0x0600747E RID: 29822 RVA: 0x002D46E0 File Offset: 0x002D28E0
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile)
		{
			this.Add(id, name, desc, rarity, defID, buildOverride, animFile, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x0600747F RID: 29823 RVA: 0x002D4704 File Offset: 0x002D2904
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] dlcIds)
		{
			EquippableFacadeResource item = new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, dlcIds);
			this.resources.Add(item);
		}
	}
}
