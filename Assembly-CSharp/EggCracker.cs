using System;
using TUNING;
using UnityEngine;

// Token: 0x020006C1 RID: 1729
[AddComponentMenu("KMonoBehaviour/scripts/EggCracker")]
public class EggCracker : KMonoBehaviour
{
	// Token: 0x06002BB3 RID: 11187 RVA: 0x000F5754 File Offset: 0x000F3954
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.refinery.choreType = Db.Get().ChoreTypes.Cook;
		this.refinery.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable complexFabricatorWorkable = this.workable;
		complexFabricatorWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(complexFabricatorWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x000F583A File Offset: 0x000F3A3A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x000F5854 File Offset: 0x000F3A54
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent e)
	{
		if (e == Workable.WorkableEvent.WorkStarted)
		{
			ComplexRecipe currentWorkingOrder = this.refinery.CurrentWorkingOrder;
			if (currentWorkingOrder != null)
			{
				ComplexRecipe.RecipeElement[] ingredients = currentWorkingOrder.ingredients;
				if (ingredients.Length != 0)
				{
					ComplexRecipe.RecipeElement recipeElement = ingredients[0];
					this.display_egg = this.refinery.buildStorage.FindFirst(recipeElement.material);
					this.PositionActiveEgg();
					return;
				}
			}
		}
		else if (e == Workable.WorkableEvent.WorkCompleted)
		{
			if (this.display_egg)
			{
				this.display_egg.GetComponent<KBatchedAnimController>().Play("hatching_pst", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
		}
		else if (e == Workable.WorkableEvent.WorkStopped)
		{
			UnityEngine.Object.Destroy(this.tracker);
			this.tracker = null;
			this.display_egg = null;
		}
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x000F58FC File Offset: 0x000F3AFC
	private void PositionActiveEgg()
	{
		if (!this.display_egg)
		{
			return;
		}
		KBatchedAnimController component = this.display_egg.GetComponent<KBatchedAnimController>();
		component.enabled = true;
		component.SetSceneLayer(Grid.SceneLayer.BuildingUse);
		KSelectable component2 = this.display_egg.GetComponent<KSelectable>();
		if (component2 != null)
		{
			component2.enabled = true;
		}
		this.tracker = this.display_egg.AddComponent<KBatchedAnimTracker>();
		this.tracker.symbol = "snapto_egg";
	}

	// Token: 0x04001921 RID: 6433
	[MyCmpReq]
	private ComplexFabricator refinery;

	// Token: 0x04001922 RID: 6434
	[MyCmpReq]
	private ComplexFabricatorWorkable workable;

	// Token: 0x04001923 RID: 6435
	private KBatchedAnimTracker tracker;

	// Token: 0x04001924 RID: 6436
	private GameObject display_egg;
}
