using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020006AE RID: 1710
public class CookingStation : ComplexFabricator, IGameObjectEffectDescriptor
{
	// Token: 0x06002B0D RID: 11021 RVA: 0x000F1CF4 File Offset: 0x000EFEF4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x000F1D2C File Offset: 0x000EFF2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanElectricGrill.Id;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_cookstation_kanim")
		};
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable workable = this.workable;
		workable.OnWorkTickActions = (Action<WorkerBase, float>)Delegate.Combine(workable.OnWorkTickActions, new Action<WorkerBase, float>(delegate(WorkerBase worker, float dt)
		{
			global::Debug.Assert(worker != null, "How did we get a null worker?");
			if (this.diseaseCountKillRate > 0)
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				int num = Math.Max(1, (int)((float)this.diseaseCountKillRate * dt));
				component.ModifyDiseaseCount(-num, "CookingStation");
			}
		}));
		base.GetComponent<ComplexFabricator>().workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorCooking;
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x000F1E38 File Offset: 0x000F0038
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ModifyDiseaseCount(-component.DiseaseCount, "CookingStation.CompleteOrder");
		}
		base.GetComponent<Operational>().SetActive(false, false);
		return list;
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x000F1EAC File Offset: 0x000F00AC
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.REMOVES_DISEASE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE, Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x040018B7 RID: 6327
	[SerializeField]
	private int diseaseCountKillRate = 100;
}
