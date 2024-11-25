using System;
using TUNING;
using UnityEngine;

// Token: 0x0200085F RID: 2143
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStationDoctorWorkable")]
public class DoctorStationDoctorWorkable : Workable
{
	// Token: 0x06003BB8 RID: 15288 RVA: 0x00148F67 File Offset: 0x00147167
	private DoctorStationDoctorWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x00148F78 File Offset: 0x00147178
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x00148FD0 File Offset: 0x001471D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x00148FD8 File Offset: 0x001471D8
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.station.SetHasDoctor(true);
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x00148FED File Offset: 0x001471ED
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.station.SetHasDoctor(false);
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x00149002 File Offset: 0x00147202
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.station.CompleteDoctoring();
	}

	// Token: 0x04002414 RID: 9236
	[MyCmpReq]
	private DoctorStation station;
}
