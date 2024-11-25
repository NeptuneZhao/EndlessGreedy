using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E45 RID: 3653
	public class ArtableStage : PermitResource
	{
		// Token: 0x06007421 RID: 29729 RVA: 0x002C6794 File Offset: 0x002C4994
		public ArtableStage(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, ArtableStatusItem status_item, string prefabId, string symbolName, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
		{
			this.id = id;
			this.animFile = animFile;
			this.anim = anim;
			this.symbolName = symbolName;
			this.decor = decor_value;
			this.cheerOnComplete = cheer_on_complete;
			this.statusItem = status_item;
			this.prefabId = prefabId;
		}

		// Token: 0x06007422 RID: 29730 RVA: 0x002C67F0 File Offset: 0x002C49F0
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.animFile), "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.ARTABLE_ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(this.prefabId).GetProperName()).Replace("{ArtableQuality}", this.statusItem.GetName(null)));
			return result;
		}

		// Token: 0x04004FF9 RID: 20473
		public string id;

		// Token: 0x04004FFA RID: 20474
		public string anim;

		// Token: 0x04004FFB RID: 20475
		public string animFile;

		// Token: 0x04004FFC RID: 20476
		public string prefabId;

		// Token: 0x04004FFD RID: 20477
		public string symbolName;

		// Token: 0x04004FFE RID: 20478
		public int decor;

		// Token: 0x04004FFF RID: 20479
		public bool cheerOnComplete;

		// Token: 0x04005000 RID: 20480
		public ArtableStatusItem statusItem;
	}
}
