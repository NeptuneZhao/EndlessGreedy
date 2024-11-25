using System;
using TUNING;

// Token: 0x020001DB RID: 475
public abstract class FossilExcavationWorkable : Workable
{
	// Token: 0x060009BD RID: 2493
	protected abstract bool IsMarkedForExcavation();

	// Token: 0x060009BE RID: 2494 RVA: 0x00039B08 File Offset: 0x00037D08
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.FossilHuntExcavationInProgress;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.FossilHunt_WorkerExcavating);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fossils_small_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = false;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00039BC0 File Offset: 0x00037DC0
	protected override void UpdateStatusItem(object data = null)
	{
		base.UpdateStatusItem(data);
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.waitingWorkStatusItemHandle != default(Guid))
		{
			component.RemoveStatusItem(this.waitingWorkStatusItemHandle, false);
		}
		if (base.worker == null && this.IsMarkedForExcavation())
		{
			this.waitingWorkStatusItemHandle = component.AddStatusItem(this.waitingForExcavationWorkStatusItem, null);
		}
	}

	// Token: 0x0400066E RID: 1646
	protected Guid waitingWorkStatusItemHandle;

	// Token: 0x0400066F RID: 1647
	protected StatusItem waitingForExcavationWorkStatusItem = Db.Get().BuildingStatusItems.FossilHuntExcavationOrdered;
}
