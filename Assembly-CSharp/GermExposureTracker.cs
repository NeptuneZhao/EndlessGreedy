using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GermExposureTracker")]
public class GermExposureTracker : KMonoBehaviour
{
	// Token: 0x06004067 RID: 16487 RVA: 0x0016CCD0 File Offset: 0x0016AED0
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GermExposureTracker.Instance == null);
		GermExposureTracker.Instance = this;
	}

	// Token: 0x06004068 RID: 16488 RVA: 0x0016CCE8 File Offset: 0x0016AEE8
	protected override void OnSpawn()
	{
		this.rng = new SeededRandom(GameClock.Instance.GetCycle());
	}

	// Token: 0x06004069 RID: 16489 RVA: 0x0016CCFF File Offset: 0x0016AEFF
	protected override void OnForcedCleanUp()
	{
		GermExposureTracker.Instance = null;
	}

	// Token: 0x0600406A RID: 16490 RVA: 0x0016CD08 File Offset: 0x0016AF08
	public void AddExposure(ExposureType exposure_type, float amount)
	{
		float num;
		this.accumulation.TryGetValue(exposure_type.germ_id, out num);
		float num2 = num + amount;
		if (num2 > 1f)
		{
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity cmp = enumerator.Current;
					GermExposureMonitor.Instance smi = cmp.GetSMI<GermExposureMonitor.Instance>();
					if (smi.GetExposureState(exposure_type.germ_id) == GermExposureMonitor.ExposureState.Exposed)
					{
						float exposureWeight = cmp.GetSMI<GermExposureMonitor.Instance>().GetExposureWeight(exposure_type.germ_id);
						if (exposureWeight > 0f)
						{
							this.exposure_candidates.Add(new GermExposureTracker.WeightedExposure
							{
								weight = exposureWeight,
								monitor = smi
							});
						}
					}
				}
				goto IL_F8;
			}
			IL_AF:
			num2 -= 1f;
			if (this.exposure_candidates.Count > 0)
			{
				GermExposureTracker.WeightedExposure weightedExposure = WeightedRandom.Choose<GermExposureTracker.WeightedExposure>(this.exposure_candidates, this.rng);
				this.exposure_candidates.Remove(weightedExposure);
				weightedExposure.monitor.ContractGerms(exposure_type.germ_id);
			}
			IL_F8:
			if (num2 > 1f)
			{
				goto IL_AF;
			}
		}
		this.accumulation[exposure_type.germ_id] = num2;
		this.exposure_candidates.Clear();
	}

	// Token: 0x04002A7E RID: 10878
	public static GermExposureTracker Instance;

	// Token: 0x04002A7F RID: 10879
	[Serialize]
	private Dictionary<HashedString, float> accumulation = new Dictionary<HashedString, float>();

	// Token: 0x04002A80 RID: 10880
	private SeededRandom rng;

	// Token: 0x04002A81 RID: 10881
	private List<GermExposureTracker.WeightedExposure> exposure_candidates = new List<GermExposureTracker.WeightedExposure>();

	// Token: 0x02001813 RID: 6163
	private class WeightedExposure : IWeighted
	{
		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06009764 RID: 38756 RVA: 0x003656C1 File Offset: 0x003638C1
		// (set) Token: 0x06009765 RID: 38757 RVA: 0x003656C9 File Offset: 0x003638C9
		public float weight { get; set; }

		// Token: 0x04007504 RID: 29956
		public GermExposureMonitor.Instance monitor;
	}
}
