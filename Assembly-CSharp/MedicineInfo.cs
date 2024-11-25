using System;
using System.Collections.Generic;

// Token: 0x02000959 RID: 2393
[Serializable]
public class MedicineInfo
{
	// Token: 0x060045EA RID: 17898 RVA: 0x0018DBA4 File Offset: 0x0018BDA4
	public MedicineInfo(string id, string effect, MedicineInfo.MedicineType medicineType, string doctorStationId, string[] curedDiseases = null)
	{
		Debug.Assert(!string.IsNullOrEmpty(effect) || (curedDiseases != null && curedDiseases.Length != 0), "Medicine should have an effect or cure diseases");
		this.id = id;
		this.effect = effect;
		this.medicineType = medicineType;
		this.doctorStationId = doctorStationId;
		if (curedDiseases != null)
		{
			this.curedSicknesses = new List<string>(curedDiseases);
			return;
		}
		this.curedSicknesses = new List<string>();
	}

	// Token: 0x060045EB RID: 17899 RVA: 0x0018DC13 File Offset: 0x0018BE13
	public Tag GetSupplyTag()
	{
		return MedicineInfo.GetSupplyTagForStation(this.doctorStationId);
	}

	// Token: 0x060045EC RID: 17900 RVA: 0x0018DC20 File Offset: 0x0018BE20
	public static Tag GetSupplyTagForStation(string stationID)
	{
		Tag tag = TagManager.Create(stationID + GameTags.MedicalSupplies.Name);
		Assets.AddCountableTag(tag);
		return tag;
	}

	// Token: 0x04002D7A RID: 11642
	public string id;

	// Token: 0x04002D7B RID: 11643
	public string effect;

	// Token: 0x04002D7C RID: 11644
	public MedicineInfo.MedicineType medicineType;

	// Token: 0x04002D7D RID: 11645
	public List<string> curedSicknesses;

	// Token: 0x04002D7E RID: 11646
	public string doctorStationId;

	// Token: 0x020018C0 RID: 6336
	public enum MedicineType
	{
		// Token: 0x04007748 RID: 30536
		Booster,
		// Token: 0x04007749 RID: 30537
		CureAny,
		// Token: 0x0400774A RID: 30538
		CureSpecific
	}
}
