using System;
using TUNING;

// Token: 0x02000676 RID: 1654
public class BuildingInternalConstructorWorkable : Workable
{
	// Token: 0x060028F8 RID: 10488 RVA: 0x000E7E44 File Offset: 0x000E6044
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.resetProgressOnStop = false;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x000E7EE7 File Offset: 0x000E60E7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.constructorInstance = this.GetSMI<BuildingInternalConstructor.Instance>();
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x000E7EFB File Offset: 0x000E60FB
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.constructorInstance.ConstructionComplete(false);
	}

	// Token: 0x04001788 RID: 6024
	private BuildingInternalConstructor.Instance constructorInstance;
}
