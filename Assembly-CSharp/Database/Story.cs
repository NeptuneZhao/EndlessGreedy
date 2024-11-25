using System;
using ProcGen;

namespace Database
{
	// Token: 0x02000E8D RID: 3725
	public class Story : Resource, IComparable<Story>
	{
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x0600751E RID: 29982 RVA: 0x002DF845 File Offset: 0x002DDA45
		// (set) Token: 0x0600751F RID: 29983 RVA: 0x002DF84D File Offset: 0x002DDA4D
		public int HashId { get; private set; }

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06007520 RID: 29984 RVA: 0x002DF856 File Offset: 0x002DDA56
		public WorldTrait StoryTrait
		{
			get
			{
				if (this._cachedStoryTrait == null)
				{
					this._cachedStoryTrait = SettingsCache.GetCachedStoryTrait(this.worldgenStoryTraitKey, false);
				}
				return this._cachedStoryTrait;
			}
		}

		// Token: 0x06007521 RID: 29985 RVA: 0x002DF878 File Offset: 0x002DDA78
		public Story(string id, string worldgenStoryTraitKey, int displayOrder)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.kleiUseOnlyCoordinateOrder = -1;
			this.updateNumber = -1;
			this.sandboxStampTemplateId = null;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x06007522 RID: 29986 RVA: 0x002DF8B8 File Offset: 0x002DDAB8
		public Story(string id, string worldgenStoryTraitKey, int displayOrder, int kleiUseOnlyCoordinateOrder, int updateNumber, string sandboxStampTemplateId)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.updateNumber = updateNumber;
			this.sandboxStampTemplateId = sandboxStampTemplateId;
			this.kleiUseOnlyCoordinateOrder = kleiUseOnlyCoordinateOrder;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x06007523 RID: 29987 RVA: 0x002DF904 File Offset: 0x002DDB04
		public int CompareTo(Story other)
		{
			return this.displayOrder.CompareTo(other.displayOrder);
		}

		// Token: 0x06007524 RID: 29988 RVA: 0x002DF925 File Offset: 0x002DDB25
		public bool IsNew()
		{
			return this.updateNumber == LaunchInitializer.UpdateNumber();
		}

		// Token: 0x06007525 RID: 29989 RVA: 0x002DF934 File Offset: 0x002DDB34
		public Story AutoStart()
		{
			this.autoStart = true;
			return this;
		}

		// Token: 0x06007526 RID: 29990 RVA: 0x002DF93E File Offset: 0x002DDB3E
		public Story SetKeepsake(string prefabId)
		{
			this.keepsakePrefabId = prefabId;
			return this;
		}

		// Token: 0x04005546 RID: 21830
		public const int MODDED_STORY = -1;

		// Token: 0x04005547 RID: 21831
		public int kleiUseOnlyCoordinateOrder;

		// Token: 0x04005549 RID: 21833
		public bool autoStart;

		// Token: 0x0400554A RID: 21834
		public string keepsakePrefabId;

		// Token: 0x0400554B RID: 21835
		public readonly string worldgenStoryTraitKey;

		// Token: 0x0400554C RID: 21836
		private readonly int displayOrder;

		// Token: 0x0400554D RID: 21837
		private readonly int updateNumber;

		// Token: 0x0400554E RID: 21838
		public string sandboxStampTemplateId;

		// Token: 0x0400554F RID: 21839
		private WorldTrait _cachedStoryTrait;
	}
}
