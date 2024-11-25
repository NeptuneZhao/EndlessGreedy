﻿using System;
using TUNING;

// Token: 0x02000183 RID: 387
public class SpaceTreeSyrupHarvestWorkable : Workable
{
	// Token: 0x060007E3 RID: 2019 RVA: 0x00034B48 File Offset: 0x00032D48
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Harvesting);
		this.workAnims = new HashedString[]
		{
			"syrup_harvest_trunk_pre",
			"syrup_harvest_trunk_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"syrup_harvest_trunk_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"syrup_harvest_trunk_loop"
		};
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_syrup_tree_kanim")
		};
		this.synchronizeAnims = true;
		this.shouldShowSkillPerkStatusItem = false;
		base.SetWorkTime(10f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00034C75 File Offset: 0x00032E75
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00034C7D File Offset: 0x00032E7D
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}
}
