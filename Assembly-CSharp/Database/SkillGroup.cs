using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000ED0 RID: 3792
	public class SkillGroup : Resource, IListableOption
	{
		// Token: 0x0600763A RID: 30266 RVA: 0x002E5787 File Offset: 0x002E3987
		string IListableOption.GetProperName()
		{
			return Strings.Get("STRINGS.DUPLICANTS.SKILLGROUPS." + this.Id.ToUpper() + ".NAME");
		}

		// Token: 0x0600763B RID: 30267 RVA: 0x002E57AD File Offset: 0x002E39AD
		public SkillGroup(string id, string choreGroupID, string name, string icon, string archetype_icon) : base(id, name)
		{
			this.choreGroupID = choreGroupID;
			this.choreGroupIcon = icon;
			this.archetypeIcon = archetype_icon;
		}

		// Token: 0x0400561A RID: 22042
		public string choreGroupID;

		// Token: 0x0400561B RID: 22043
		public List<Klei.AI.Attribute> relevantAttributes;

		// Token: 0x0400561C RID: 22044
		public List<string> requiredChoreGroups;

		// Token: 0x0400561D RID: 22045
		public string choreGroupIcon;

		// Token: 0x0400561E RID: 22046
		public string archetypeIcon;
	}
}
