using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D71 RID: 3441
public class HarvestModuleSideScreen : SideScreenContent, ISimEveryTick
{
	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x06006C44 RID: 27716 RVA: 0x0028BBE8 File Offset: 0x00289DE8
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x06006C45 RID: 27717 RVA: 0x0028BBF5 File Offset: 0x00289DF5
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006C46 RID: 27718 RVA: 0x0028BC05 File Offset: 0x00289E05
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06006C47 RID: 27719 RVA: 0x0028BC0C File Offset: 0x00289E0C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetResourceHarvestModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x06006C48 RID: 27720 RVA: 0x0028BC30 File Offset: 0x00289E30
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		this.RefreshModulePanel(resourceHarvestModule);
	}

	// Token: 0x06006C49 RID: 27721 RVA: 0x0028BC64 File Offset: 0x00289E64
	private ResourceHarvestModule.StatesInstance GetResourceHarvestModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			GameObject gameObject = @ref.Get().gameObject;
			if (gameObject.GetDef<ResourceHarvestModule.Def>() != null)
			{
				return gameObject.GetSMI<ResourceHarvestModule.StatesInstance>();
			}
		}
		return null;
	}

	// Token: 0x06006C4A RID: 27722 RVA: 0x0028BCD0 File Offset: 0x00289ED0
	private void RefreshModulePanel(StateMachine.Instance module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x06006C4B RID: 27723 RVA: 0x0028BD24 File Offset: 0x00289F24
	public void SimEveryTick(float dt)
	{
		if (this.targetCraft.IsNullOrDestroyed())
		{
			return;
		}
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		if (resourceHarvestModule == null)
		{
			return;
		}
		GenericUIProgressBar reference = component.GetReference<GenericUIProgressBar>("progressBar");
		float num = 4f;
		float num2 = resourceHarvestModule.timeinstate % num;
		if (resourceHarvestModule.sm.canHarvest.Get(resourceHarvestModule))
		{
			reference.SetFillPercentage(num2 / num);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_IN_PROGRESS);
		}
		else
		{
			reference.SetFillPercentage(0f);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_STOPPED);
		}
		GenericUIProgressBar reference2 = component.GetReference<GenericUIProgressBar>("diamondProgressBar");
		Storage component2 = resourceHarvestModule.GetComponent<Storage>();
		float fillPercentage = component2.MassStored() / component2.Capacity();
		reference2.SetFillPercentage(fillPercentage);
		reference2.label.SetText(ElementLoader.GetElement(SimHashes.Diamond.CreateTag()).name + ": " + GameUtil.GetFormattedMass(component2.MassStored(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x040049D6 RID: 18902
	private Clustercraft targetCraft;

	// Token: 0x040049D7 RID: 18903
	public GameObject moduleContentContainer;

	// Token: 0x040049D8 RID: 18904
	public GameObject modulePanelPrefab;
}
