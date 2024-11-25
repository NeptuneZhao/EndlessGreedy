using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D70 RID: 3440
public class HabitatModuleSideScreen : SideScreenContent
{
	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x06006C3C RID: 27708 RVA: 0x0028BA6F File Offset: 0x00289C6F
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x06006C3D RID: 27709 RVA: 0x0028BA7C File Offset: 0x00289C7C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006C3E RID: 27710 RVA: 0x0028BA8C File Offset: 0x00289C8C
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06006C3F RID: 27711 RVA: 0x0028BA93 File Offset: 0x00289C93
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetPassengerModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x06006C40 RID: 27712 RVA: 0x0028BAB8 File Offset: 0x00289CB8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		PassengerRocketModule passengerModule = this.GetPassengerModule(this.targetCraft);
		this.RefreshModulePanel(passengerModule);
	}

	// Token: 0x06006C41 RID: 27713 RVA: 0x0028BAEC File Offset: 0x00289CEC
	private PassengerRocketModule GetPassengerModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06006C42 RID: 27714 RVA: 0x0028BB54 File Offset: 0x00289D54
	private void RefreshModulePanel(PassengerRocketModule module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		KButton reference = component.GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			AudioMixer.instance.Start(module.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			ClusterManager.Instance.SetActiveWorld(module.GetComponent<ClustercraftExteriorDoor>().GetTargetWorld().id);
			ManagementMenu.Instance.CloseAll();
		};
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x040049D3 RID: 18899
	private Clustercraft targetCraft;

	// Token: 0x040049D4 RID: 18900
	public GameObject moduleContentContainer;

	// Token: 0x040049D5 RID: 18901
	public GameObject modulePanelPrefab;
}
