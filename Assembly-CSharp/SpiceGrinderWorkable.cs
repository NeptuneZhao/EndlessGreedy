using System;
using System.Linq;
using TUNING;
using UnityEngine;

// Token: 0x02000770 RID: 1904
public class SpiceGrinderWorkable : Workable, IConfigurableConsumer
{
	// Token: 0x06003364 RID: 13156 RVA: 0x00119B50 File Offset: 0x00117D50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanSpiceGrinder.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Spicing;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_spice_grinder_kanim")
		};
		base.SetWorkTime(5f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x00119C10 File Offset: 0x00117E10
	protected override void OnStartWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood != null)
		{
			float num = this.Grinder.CurrentFood.Calories * 0.001f / 1000f;
			base.SetWorkTime(num * 5f);
		}
		else
		{
			global::Debug.LogWarning("SpiceGrider attempted to start spicing with no food");
			base.StopWork(worker, true);
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x00119C79 File Offset: 0x00117E79
	protected override void OnAbortWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x00119C9A File Offset: 0x00117E9A
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.SpiceFood();
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x00119CBC File Offset: 0x00117EBC
	public IConfigurableConsumerOption[] GetSettingOptions()
	{
		return SpiceGrinder.SettingOptions.Values.ToArray<SpiceGrinder.Option>();
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x00119CDA File Offset: 0x00117EDA
	public IConfigurableConsumerOption GetSelectedOption()
	{
		return this.Grinder.SelectedOption;
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x00119CE7 File Offset: 0x00117EE7
	public void SetSelectedOption(IConfigurableConsumerOption option)
	{
		this.Grinder.OnOptionSelected(option as SpiceGrinder.Option);
	}

	// Token: 0x04001E5F RID: 7775
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001E60 RID: 7776
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x04001E61 RID: 7777
	public SpiceGrinder.StatesInstance Grinder;
}
