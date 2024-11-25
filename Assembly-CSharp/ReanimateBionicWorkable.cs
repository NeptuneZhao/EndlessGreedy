using System;
using TUNING;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class ReanimateBionicWorkable : Workable
{
	// Token: 0x06001272 RID: 4722 RVA: 0x00065220 File Offset: 0x00063420
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workAnims = new HashedString[]
		{
			"offline_battery_change_pre",
			"offline_battery_change_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"offline_battery_change_pst"
		};
		base.SetWorkTime(30f);
		this.readyForSkillWorkStatusItem = Db.Get().DuplicantStatusItems.BionicRequiresSkillPerk;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.InstallingElectrobank);
		this.workingStatusItem = Db.Get().DuplicantStatusItems.BionicBeingRebooted;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_bionic_kanim")
		};
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		this.resetProgressOnStop = true;
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x00065314 File Offset: 0x00063514
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Vector3 position = worker.transform.GetPosition();
		position.x = base.transform.GetPosition().x;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
		worker.transform.SetPosition(position);
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x00065368 File Offset: 0x00063568
	protected override void OnStopWork(WorkerBase worker)
	{
		Vector3 position = worker.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		worker.transform.SetPosition(position);
		base.OnStopWork(worker);
	}
}
