using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000E68 RID: 3688
	public class MonumentParts : ResourceSet<MonumentPartResource>
	{
		// Token: 0x0600749C RID: 29852 RVA: 0x002D758C File Offset: 0x002D578C
		public MonumentParts(ResourceSet parent) : base("MonumentParts", parent)
		{
			base.Initialize();
			foreach (MonumentPartInfo monumentPartInfo in Blueprints.Get().all.monumentParts)
			{
				this.Add(monumentPartInfo.id, monumentPartInfo.name, monumentPartInfo.desc, monumentPartInfo.rarity, monumentPartInfo.animFile, monumentPartInfo.state, monumentPartInfo.symbolName, monumentPartInfo.part, monumentPartInfo.dlcIds);
			}
		}

		// Token: 0x0600749D RID: 29853 RVA: 0x002D7630 File Offset: 0x002D5830
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] dlcIds)
		{
			MonumentPartResource item = new MonumentPartResource(id, name, desc, rarity, animFilename, state, symbolName, part, dlcIds);
			this.resources.Add(item);
		}

		// Token: 0x0600749E RID: 29854 RVA: 0x002D7660 File Offset: 0x002D5860
		public List<MonumentPartResource> GetParts(MonumentPartResource.Part part)
		{
			return this.resources.FindAll((MonumentPartResource mpr) => mpr.part == part);
		}
	}
}
