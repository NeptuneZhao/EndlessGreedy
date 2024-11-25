using System;
using TUNING;

// Token: 0x02000A5F RID: 2655
public class ArmTrapWorkable : Workable
{
	// Token: 0x06004D22 RID: 19746 RVA: 0x001B9D0C File Offset: 0x001B7F0C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.CanBeArmedAtLongDistance)
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			this.faceTargetWhenWorking = true;
			this.multitoolContext = "build";
			this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		}
		if (this.initialOffsets != null && this.initialOffsets.Length != 0)
		{
			base.SetOffsets(this.initialOffsets);
		}
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.ArmingTrap);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(5f);
		this.synchronizeAnims = true;
		this.resetProgressOnStop = true;
	}

	// Token: 0x06004D23 RID: 19747 RVA: 0x001B9DE9 File Offset: 0x001B7FE9
	public override void OnPendingCompleteWork(WorkerBase worker)
	{
		base.OnPendingCompleteWork(worker);
		this.WorkInPstAnimation = true;
		base.gameObject.Trigger(-2025798095, null);
	}

	// Token: 0x06004D24 RID: 19748 RVA: 0x001B9E0A File Offset: 0x001B800A
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.WorkInPstAnimation = false;
	}

	// Token: 0x04003338 RID: 13112
	public bool WorkInPstAnimation;

	// Token: 0x04003339 RID: 13113
	public bool CanBeArmedAtLongDistance;

	// Token: 0x0400333A RID: 13114
	public CellOffset[] initialOffsets;
}
