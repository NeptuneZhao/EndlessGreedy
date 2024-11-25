using System;

namespace Database
{
	// Token: 0x02000E71 RID: 3697
	public abstract class PermitResource : Resource
	{
		// Token: 0x060074B7 RID: 29879 RVA: 0x002D8170 File Offset: 0x002D6370
		public PermitResource(string id, string Name, string Desc, PermitCategory permitCategory, PermitRarity rarity, string[] DLCIds) : base(id, Name)
		{
			DebugUtil.DevAssert(Name != null, "Name must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			DebugUtil.DevAssert(Desc != null, "Description must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			this.Description = Desc;
			this.Category = permitCategory;
			this.Rarity = rarity;
			this.DlcIds = DLCIds;
		}

		// Token: 0x060074B8 RID: 29880
		public abstract PermitPresentationInfo GetPermitPresentationInfo();

		// Token: 0x060074B9 RID: 29881 RVA: 0x002D81EE File Offset: 0x002D63EE
		public bool IsOwnableOnServer()
		{
			return this.Rarity != PermitRarity.Universal && this.Rarity != PermitRarity.UniversalLocked;
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x002D8207 File Offset: 0x002D6407
		public bool IsUnlocked()
		{
			return this.Rarity == PermitRarity.Universal || PermitItems.IsPermitUnlocked(this);
		}

		// Token: 0x060074BB RID: 29883 RVA: 0x002D821A File Offset: 0x002D641A
		public string GetDlcIdFrom()
		{
			if (this.DlcIds == DlcManager.AVAILABLE_ALL_VERSIONS || this.DlcIds == DlcManager.AVAILABLE_VANILLA_ONLY)
			{
				return null;
			}
			if (this.DlcIds.Length == 0)
			{
				return null;
			}
			return this.DlcIds[0];
		}

		// Token: 0x0400543D RID: 21565
		public string Description;

		// Token: 0x0400543E RID: 21566
		public PermitCategory Category;

		// Token: 0x0400543F RID: 21567
		public PermitRarity Rarity;

		// Token: 0x04005440 RID: 21568
		public string[] DlcIds;
	}
}
