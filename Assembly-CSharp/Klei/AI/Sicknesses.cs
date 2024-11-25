using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F50 RID: 3920
	public class Sicknesses : Modifications<Sickness, SicknessInstance>
	{
		// Token: 0x0600789B RID: 30875 RVA: 0x002FB463 File Offset: 0x002F9663
		public Sicknesses(GameObject go) : base(go, Db.Get().Sicknesses)
		{
		}

		// Token: 0x0600789C RID: 30876 RVA: 0x002FB478 File Offset: 0x002F9678
		public void Infect(SicknessExposureInfo exposure_info)
		{
			Sickness modifier = Db.Get().Sicknesses.Get(exposure_info.sicknessID);
			if (!base.Has(modifier))
			{
				this.CreateInstance(modifier).ExposureInfo = exposure_info;
			}
		}

		// Token: 0x0600789D RID: 30877 RVA: 0x002FB4B4 File Offset: 0x002F96B4
		public override SicknessInstance CreateInstance(Sickness sickness)
		{
			SicknessInstance sicknessInstance = new SicknessInstance(base.gameObject, sickness);
			this.Add(sicknessInstance);
			base.Trigger(GameHashes.SicknessAdded, sicknessInstance);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, 1f, base.gameObject.GetProperName(), null);
			return sicknessInstance;
		}

		// Token: 0x0600789E RID: 30878 RVA: 0x002FB4FF File Offset: 0x002F96FF
		public bool IsInfected()
		{
			return base.Count > 0;
		}

		// Token: 0x0600789F RID: 30879 RVA: 0x002FB50A File Offset: 0x002F970A
		public bool Cure(Sickness sickness)
		{
			return this.Cure(sickness.Id);
		}

		// Token: 0x060078A0 RID: 30880 RVA: 0x002FB518 File Offset: 0x002F9718
		public bool Cure(string sickness_id)
		{
			SicknessInstance sicknessInstance = null;
			foreach (SicknessInstance sicknessInstance2 in this)
			{
				if (sicknessInstance2.modifier.Id == sickness_id)
				{
					sicknessInstance = sicknessInstance2;
					break;
				}
			}
			if (sicknessInstance != null)
			{
				this.Remove(sicknessInstance);
				base.Trigger(GameHashes.SicknessCured, sicknessInstance);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, -1f, base.gameObject.GetProperName(), null);
				return true;
			}
			return false;
		}
	}
}
