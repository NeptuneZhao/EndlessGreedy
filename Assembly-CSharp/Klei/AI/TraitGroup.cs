using System;

namespace Klei.AI
{
	// Token: 0x02000F78 RID: 3960
	public class TraitGroup : ModifierGroup<Trait>
	{
		// Token: 0x0600797E RID: 31102 RVA: 0x002FFE11 File Offset: 0x002FE011
		public TraitGroup(string id, string name, bool is_spawn_trait) : base(id, name)
		{
			this.IsSpawnTrait = is_spawn_trait;
		}

		// Token: 0x04005AAA RID: 23210
		public bool IsSpawnTrait;
	}
}
