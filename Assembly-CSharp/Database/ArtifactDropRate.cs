using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000E47 RID: 3655
	public class ArtifactDropRate : Resource
	{
		// Token: 0x06007427 RID: 29735 RVA: 0x002C69C6 File Offset: 0x002C4BC6
		public void AddItem(ArtifactTier tier, float weight)
		{
			this.rates.Add(new global::Tuple<ArtifactTier, float>(tier, weight));
			this.totalWeight += weight;
		}

		// Token: 0x06007428 RID: 29736 RVA: 0x002C69E8 File Offset: 0x002C4BE8
		public float GetTierWeight(ArtifactTier tier)
		{
			float result = 0f;
			foreach (global::Tuple<ArtifactTier, float> tuple in this.rates)
			{
				if (tuple.first == tier)
				{
					result = tuple.second;
				}
			}
			return result;
		}

		// Token: 0x04005001 RID: 20481
		public List<global::Tuple<ArtifactTier, float>> rates = new List<global::Tuple<ArtifactTier, float>>();

		// Token: 0x04005002 RID: 20482
		public float totalWeight;
	}
}
