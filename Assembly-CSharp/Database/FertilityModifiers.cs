using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E3F RID: 3647
	public class FertilityModifiers : ResourceSet<FertilityModifier>
	{
		// Token: 0x06007414 RID: 29716 RVA: 0x002C54F0 File Offset: 0x002C36F0
		public List<FertilityModifier> GetForTag(Tag searchTag)
		{
			List<FertilityModifier> list = new List<FertilityModifier>();
			foreach (FertilityModifier fertilityModifier in this.resources)
			{
				if (fertilityModifier.TargetTag == searchTag)
				{
					list.Add(fertilityModifier);
				}
			}
			return list;
		}
	}
}
