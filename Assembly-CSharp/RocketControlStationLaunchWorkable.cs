using System;
using TUNING;
using UnityEngine;

// Token: 0x02000761 RID: 1889
[AddComponentMenu("KMonoBehaviour/Workable/RocketControlStationLaunchWorkable")]
public class RocketControlStationLaunchWorkable : Workable
{
	// Token: 0x060032C3 RID: 12995 RVA: 0x001171C8 File Offset: 0x001153C8
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

	// Token: 0x060032C4 RID: 12996 RVA: 0x00117260 File Offset: 0x00115460
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		RocketControlStation.StatesInstance smi = this.GetSMI<RocketControlStation.StatesInstance>();
		if (smi != null)
		{
			smi.SetPilotSpeedMult(worker);
			smi.LaunchRocket();
		}
	}

	// Token: 0x04001DFB RID: 7675
	[MyCmpReq]
	private Operational operational;
}
