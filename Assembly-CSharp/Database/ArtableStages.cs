using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000E46 RID: 3654
	public class ArtableStages : ResourceSet<ArtableStage>
	{
		// Token: 0x06007423 RID: 29731 RVA: 0x002C6870 File Offset: 0x002C4A70
		public ArtableStage Add(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname, string[] dlcIds)
		{
			ArtableStatusItem status_item = Db.Get().ArtableStatuses.Get(status_id);
			ArtableStage artableStage = new ArtableStage(id, name, desc, rarity, animFile, anim, decor_value, cheer_on_complete, status_item, prefabId, symbolname, dlcIds);
			this.resources.Add(artableStage);
			return artableStage;
		}

		// Token: 0x06007424 RID: 29732 RVA: 0x002C68B8 File Offset: 0x002C4AB8
		public ArtableStages(ResourceSet parent) : base("ArtableStages", parent)
		{
			foreach (ArtableInfo artableInfo in Blueprints.Get().all.artables)
			{
				this.Add(artableInfo.id, artableInfo.name, artableInfo.desc, artableInfo.rarity, artableInfo.animFile, artableInfo.anim, artableInfo.decor_value, artableInfo.cheer_on_complete, artableInfo.status_id, artableInfo.prefabId, artableInfo.symbolname, artableInfo.dlcIds);
			}
		}

		// Token: 0x06007425 RID: 29733 RVA: 0x002C6968 File Offset: 0x002C4B68
		public List<ArtableStage> GetPrefabStages(Tag prefab_id)
		{
			return this.resources.FindAll((ArtableStage stage) => stage.prefabId == prefab_id);
		}

		// Token: 0x06007426 RID: 29734 RVA: 0x002C6999 File Offset: 0x002C4B99
		public ArtableStage DefaultPrefabStage(Tag prefab_id)
		{
			return this.GetPrefabStages(prefab_id).Find((ArtableStage stage) => stage.statusItem == Db.Get().ArtableStatuses.AwaitingArting);
		}
	}
}
