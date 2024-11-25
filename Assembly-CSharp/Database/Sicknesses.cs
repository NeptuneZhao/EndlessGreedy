using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E7D RID: 3709
	public class Sicknesses : ResourceSet<Sickness>
	{
		// Token: 0x060074DC RID: 29916 RVA: 0x002DAC3C File Offset: 0x002D8E3C
		public Sicknesses(ResourceSet parent) : base("Sicknesses", parent)
		{
			this.FoodSickness = base.Add(new FoodSickness());
			this.SlimeSickness = base.Add(new SlimeSickness());
			this.ZombieSickness = base.Add(new ZombieSickness());
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationSickness = base.Add(new RadiationSickness());
			}
			this.Allergies = base.Add(new Allergies());
			this.Sunburn = base.Add(new Sunburn());
		}

		// Token: 0x060074DD RID: 29917 RVA: 0x002DACC4 File Offset: 0x002D8EC4
		public static bool IsValidID(string id)
		{
			bool result = false;
			using (List<Sickness>.Enumerator enumerator = Db.Get().Sicknesses.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == id)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x04005498 RID: 21656
		public Sickness FoodSickness;

		// Token: 0x04005499 RID: 21657
		public Sickness SlimeSickness;

		// Token: 0x0400549A RID: 21658
		public Sickness ZombieSickness;

		// Token: 0x0400549B RID: 21659
		public Sickness Allergies;

		// Token: 0x0400549C RID: 21660
		public Sickness RadiationSickness;

		// Token: 0x0400549D RID: 21661
		public Sickness Sunburn;
	}
}
