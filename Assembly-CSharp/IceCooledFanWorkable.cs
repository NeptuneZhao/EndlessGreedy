using System;
using TUNING;
using UnityEngine;

// Token: 0x020006F0 RID: 1776
[AddComponentMenu("KMonoBehaviour/Workable/IceCooledFanWorkable")]
public class IceCooledFanWorkable : Workable
{
	// Token: 0x06002D4F RID: 11599 RVA: 0x000FE52F File Offset: 0x000FC72F
	private IceCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06002D50 RID: 11600 RVA: 0x000FE540 File Offset: 0x000FC740
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.workerStatusItem = null;
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x000FE59F File Offset: 0x000FC79F
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x000FE5DD File Offset: 0x000FC7DD
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x000FE5EC File Offset: 0x000FC7EC
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x000FE5FB File Offset: 0x000FC7FB
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001A41 RID: 6721
	[MyCmpGet]
	private Operational operational;
}
