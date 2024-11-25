using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E5D RID: 3677
	public class Diseases : ResourceSet<Disease>
	{
		// Token: 0x06007472 RID: 29810 RVA: 0x002D25E4 File Offset: 0x002D07E4
		public Diseases(ResourceSet parent, bool statsOnly = false) : base("Diseases", parent)
		{
			this.FoodGerms = base.Add(new FoodGerms(statsOnly));
			this.SlimeGerms = base.Add(new SlimeGerms(statsOnly));
			this.PollenGerms = base.Add(new PollenGerms(statsOnly));
			this.ZombieSpores = base.Add(new ZombieSpores(statsOnly));
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationPoisoning = base.Add(new RadiationPoisoning(statsOnly));
			}
		}

		// Token: 0x06007473 RID: 29811 RVA: 0x002D2660 File Offset: 0x002D0860
		public bool IsValidID(string id)
		{
			bool result = false;
			using (List<Disease>.Enumerator enumerator = this.resources.GetEnumerator())
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

		// Token: 0x06007474 RID: 29812 RVA: 0x002D26C0 File Offset: 0x002D08C0
		public byte GetIndex(int hash)
		{
			byte b = 0;
			while ((int)b < this.resources.Count)
			{
				Disease disease = this.resources[(int)b];
				if (hash == disease.id.GetHashCode())
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		// Token: 0x06007475 RID: 29813 RVA: 0x002D270C File Offset: 0x002D090C
		public byte GetIndex(HashedString id)
		{
			return this.GetIndex(id.GetHashCode());
		}

		// Token: 0x040052A7 RID: 21159
		public Disease FoodGerms;

		// Token: 0x040052A8 RID: 21160
		public Disease SlimeGerms;

		// Token: 0x040052A9 RID: 21161
		public Disease PollenGerms;

		// Token: 0x040052AA RID: 21162
		public Disease ZombieSpores;

		// Token: 0x040052AB RID: 21163
		public Disease RadiationPoisoning;
	}
}
