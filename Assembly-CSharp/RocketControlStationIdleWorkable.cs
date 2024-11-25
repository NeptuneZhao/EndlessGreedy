using System;
using TUNING;
using UnityEngine;

// Token: 0x02000762 RID: 1890
[AddComponentMenu("KMonoBehaviour/Workable/RocketControlStationIdleWorkable")]
public class RocketControlStationIdleWorkable : Workable
{
	// Token: 0x060032C6 RID: 12998 RVA: 0x00117294 File Offset: 0x00115494
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_rocket_control_station_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		this.attributeConverter = Db.Get().AttributeConverters.PilotingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Rocketry.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
		base.SetWorkTime(30f);
	}

	// Token: 0x060032C7 RID: 12999 RVA: 0x0011732C File Offset: 0x0011552C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		RocketControlStation.StatesInstance smi = this.GetSMI<RocketControlStation.StatesInstance>();
		if (smi != null)
		{
			smi.SetPilotSpeedMult(worker);
		}
	}

	// Token: 0x04001DFC RID: 7676
	[MyCmpReq]
	private Operational operational;
}
