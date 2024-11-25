using System;
using TUNING;

// Token: 0x0200072F RID: 1839
public class EmptyMilkSeparatorWorkable : Workable
{
	// Token: 0x060030D9 RID: 12505 RVA: 0x0010D684 File Offset: 0x0010B884
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workLayer = Grid.SceneLayer.BuildingFront;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_milk_separator_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(15f);
		this.synchronizeAnims = true;
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x0010D724 File Offset: 0x0010B924
	public override void OnPendingCompleteWork(WorkerBase worker)
	{
		System.Action onWork_PST_Begins = this.OnWork_PST_Begins;
		if (onWork_PST_Begins != null)
		{
			onWork_PST_Begins();
		}
		base.OnPendingCompleteWork(worker);
	}

	// Token: 0x04001CA2 RID: 7330
	public System.Action OnWork_PST_Begins;
}
