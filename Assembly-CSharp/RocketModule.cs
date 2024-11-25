﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

// Token: 0x02000AE1 RID: 2785
[AddComponentMenu("KMonoBehaviour/scripts/RocketModule")]
public class RocketModule : KMonoBehaviour
{
	// Token: 0x060052B5 RID: 21173 RVA: 0x001DA650 File Offset: 0x001D8850
	public ProcessCondition AddModuleCondition(ProcessCondition.ProcessConditionType conditionType, ProcessCondition condition)
	{
		if (!this.moduleConditions.ContainsKey(conditionType))
		{
			this.moduleConditions.Add(conditionType, new List<ProcessCondition>());
		}
		if (!this.moduleConditions[conditionType].Contains(condition))
		{
			this.moduleConditions[conditionType].Add(condition);
		}
		return condition;
	}

	// Token: 0x060052B6 RID: 21174 RVA: 0x001DA6A4 File Offset: 0x001D88A4
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		List<ProcessCondition> list = new List<ProcessCondition>();
		if (conditionType == ProcessCondition.ProcessConditionType.All)
		{
			using (Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>.Enumerator enumerator = this.moduleConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ProcessCondition.ProcessConditionType, List<ProcessCondition>> keyValuePair = enumerator.Current;
					list.AddRange(keyValuePair.Value);
				}
				return list;
			}
		}
		if (this.moduleConditions.ContainsKey(conditionType))
		{
			list = this.moduleConditions[conditionType];
		}
		return list;
	}

	// Token: 0x060052B7 RID: 21175 RVA: 0x001DA724 File Offset: 0x001D8924
	public void SetBGKAnim(KAnimFile anim_file)
	{
		this.bgAnimFile = anim_file;
	}

	// Token: 0x060052B8 RID: 21176 RVA: 0x001DA72D File Offset: 0x001D892D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketOnGroundTagDelegate, false);
		GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketNotOnGroundTagDelegate, false);
	}

	// Token: 0x060052B9 RID: 21177 RVA: 0x001DA750 File Offset: 0x001D8950
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.conditionManager = this.FindLaunchConditionManager();
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
			if (spacecraftFromLaunchConditionManager != null)
			{
				this.SetParentRocketName(spacecraftFromLaunchConditionManager.GetRocketName());
			}
			this.RegisterWithConditionManager();
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.RocketName, this);
		}
		base.Subscribe<RocketModule>(1502190696, RocketModule.DEBUG_OnDestroyDelegate);
		this.FixSorting();
		AttachableBuilding component2 = base.GetComponent<AttachableBuilding>();
		component2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(component2.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentNetworkChanged));
		if (this.bgAnimFile != null)
		{
			this.AddBGGantry();
		}
	}

	// Token: 0x060052BA RID: 21178 RVA: 0x001DA818 File Offset: 0x001D8A18
	public void FixSorting()
	{
		int num = 0;
		AttachableBuilding component = base.GetComponent<AttachableBuilding>();
		while (component != null)
		{
			BuildingAttachPoint attachedTo = component.GetAttachedTo();
			if (!(attachedTo != null))
			{
				break;
			}
			component = attachedTo.GetComponent<AttachableBuilding>();
			num++;
		}
		Vector3 localPosition = base.transform.GetLocalPosition();
		localPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Building) - (float)num * 0.01f;
		base.transform.SetLocalPosition(localPosition);
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (component2.enabled)
		{
			component2.enabled = false;
			component2.enabled = true;
		}
	}

	// Token: 0x060052BB RID: 21179 RVA: 0x001DA8A4 File Offset: 0x001D8AA4
	private void OnAttachmentNetworkChanged(object ab)
	{
		this.FixSorting();
	}

	// Token: 0x060052BC RID: 21180 RVA: 0x001DA8AC File Offset: 0x001D8AAC
	private void AddBGGantry()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		GameObject gameObject = new GameObject();
		gameObject.name = string.Format(this.rocket_module_bg_base_string, base.name, this.rocket_module_bg_affix);
		gameObject.SetActive(false);
		Vector3 position = component.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.InteriorWall);
		gameObject.transform.SetPosition(position);
		gameObject.transform.parent = base.transform;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			this.bgAnimFile
		};
		kbatchedAnimController.initialAnim = this.rocket_module_bg_anim;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kbatchedAnimController.initialMode = KAnim.PlayMode.Paused;
		kbatchedAnimController.FlipX = component.FlipX;
		kbatchedAnimController.FlipY = component.FlipY;
		gameObject.SetActive(true);
	}

	// Token: 0x060052BD RID: 21181 RVA: 0x001DA978 File Offset: 0x001D8B78
	private void DEBUG_OnDestroy(object data)
	{
		if (this.conditionManager != null && !App.IsExiting && !KMonoBehaviour.isLoadingScene)
		{
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
			this.conditionManager.DEBUG_TraceModuleDestruction(base.name, (spacecraftFromLaunchConditionManager == null) ? "null spacecraft" : spacecraftFromLaunchConditionManager.state.ToString(), new StackTrace(true).ToString());
		}
	}

	// Token: 0x060052BE RID: 21182 RVA: 0x001DA9EC File Offset: 0x001D8BEC
	private void OnRocketOnGroundTag(object data)
	{
		this.RegisterComponents();
		Operational component = base.GetComponent<Operational>();
		if (this.operationalLandedRequired && component != null)
		{
			component.SetFlag(RocketModule.landedFlag, true);
		}
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x001DAA24 File Offset: 0x001D8C24
	private void OnRocketNotOnGroundTag(object data)
	{
		this.DeregisterComponents();
		Operational component = base.GetComponent<Operational>();
		if (this.operationalLandedRequired && component != null)
		{
			component.SetFlag(RocketModule.landedFlag, false);
		}
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x001DAA5C File Offset: 0x001D8C5C
	public void DeregisterComponents()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		component.IsSelectable = false;
		BuildingComplete component2 = base.GetComponent<BuildingComplete>();
		if (component2 != null)
		{
			component2.UpdatePosition();
		}
		if (SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(null, false);
		}
		Deconstructable component3 = base.GetComponent<Deconstructable>();
		if (component3 != null)
		{
			component3.SetAllowDeconstruction(false);
		}
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			GameComps.StructureTemperatures.Disable(handle);
		}
		FakeFloorAdder component4 = base.GetComponent<FakeFloorAdder>();
		if (component4 != null)
		{
			component4.SetFloor(false);
		}
		AccessControl component5 = base.GetComponent<AccessControl>();
		if (component5 != null)
		{
			component5.SetRegistered(false);
		}
		foreach (ManualDeliveryKG manualDeliveryKG in base.GetComponents<ManualDeliveryKG>())
		{
			DebugUtil.DevAssert(!manualDeliveryKG.IsPaused, "RocketModule ManualDeliver chore was already paused, when this rocket lands it will re-enable it.", null);
			manualDeliveryKG.Pause(true, "Rocket heading to space");
		}
		BuildingConduitEndpoints[] components2 = base.GetComponents<BuildingConduitEndpoints>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].RemoveEndPoint();
		}
		ReorderableBuilding component6 = base.GetComponent<ReorderableBuilding>();
		if (component6 != null)
		{
			component6.ShowReorderArm(false);
		}
		Workable component7 = base.GetComponent<Workable>();
		if (component7 != null)
		{
			component7.RefreshReachability();
		}
		Structure component8 = base.GetComponent<Structure>();
		if (component8 != null)
		{
			component8.UpdatePosition();
		}
		WireUtilitySemiVirtualNetworkLink component9 = base.GetComponent<WireUtilitySemiVirtualNetworkLink>();
		if (component9 != null)
		{
			component9.SetLinkConnected(false);
		}
		PartialLightBlocking component10 = base.GetComponent<PartialLightBlocking>();
		if (component10 != null)
		{
			component10.ClearLightBlocking();
		}
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x001DAC00 File Offset: 0x001D8E00
	public void RegisterComponents()
	{
		base.GetComponent<KSelectable>().IsSelectable = true;
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component != null)
		{
			component.UpdatePosition();
		}
		Deconstructable component2 = base.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.SetAllowDeconstruction(true);
		}
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			GameComps.StructureTemperatures.Enable(handle);
		}
		Storage[] components = base.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].UpdateStoredItemCachedCells();
		}
		FakeFloorAdder component3 = base.GetComponent<FakeFloorAdder>();
		if (component3 != null)
		{
			component3.SetFloor(true);
		}
		AccessControl component4 = base.GetComponent<AccessControl>();
		if (component4 != null)
		{
			component4.SetRegistered(true);
		}
		ManualDeliveryKG[] components2 = base.GetComponents<ManualDeliveryKG>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].Pause(false, "Landing on world");
		}
		BuildingConduitEndpoints[] components3 = base.GetComponents<BuildingConduitEndpoints>();
		for (int i = 0; i < components3.Length; i++)
		{
			components3[i].AddEndpoint();
		}
		ReorderableBuilding component5 = base.GetComponent<ReorderableBuilding>();
		if (component5 != null)
		{
			component5.ShowReorderArm(true);
		}
		Workable component6 = base.GetComponent<Workable>();
		if (component6 != null)
		{
			component6.RefreshReachability();
		}
		Structure component7 = base.GetComponent<Structure>();
		if (component7 != null)
		{
			component7.UpdatePosition();
		}
		WireUtilitySemiVirtualNetworkLink component8 = base.GetComponent<WireUtilitySemiVirtualNetworkLink>();
		if (component8 != null)
		{
			component8.SetLinkConnected(true);
		}
		PartialLightBlocking component9 = base.GetComponent<PartialLightBlocking>();
		if (component9 != null)
		{
			component9.SetLightBlocking();
		}
	}

	// Token: 0x060052C2 RID: 21186 RVA: 0x001DAD90 File Offset: 0x001D8F90
	private void ToggleComponent(Type cmpType, bool enabled)
	{
		MonoBehaviour monoBehaviour = (MonoBehaviour)base.GetComponent(cmpType);
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = enabled;
		}
	}

	// Token: 0x060052C3 RID: 21187 RVA: 0x001DADBA File Offset: 0x001D8FBA
	public void RegisterWithConditionManager()
	{
		global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		if (this.conditionManager != null)
		{
			this.conditionManager.RegisterRocketModule(this);
		}
	}

	// Token: 0x060052C4 RID: 21188 RVA: 0x001DADE3 File Offset: 0x001D8FE3
	protected override void OnCleanUp()
	{
		if (this.conditionManager != null)
		{
			this.conditionManager.UnregisterRocketModule(this);
		}
		base.OnCleanUp();
	}

	// Token: 0x060052C5 RID: 21189 RVA: 0x001DAE08 File Offset: 0x001D9008
	public virtual LaunchConditionManager FindLaunchConditionManager()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				LaunchConditionManager component = gameObject.GetComponent<LaunchConditionManager>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x060052C6 RID: 21190 RVA: 0x001DAE78 File Offset: 0x001D9078
	public void SetParentRocketName(string newName)
	{
		this.parentRocketName = newName;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x060052C7 RID: 21191 RVA: 0x001DAE91 File Offset: 0x001D9091
	public virtual string GetParentRocketName()
	{
		return this.parentRocketName;
	}

	// Token: 0x060052C8 RID: 21192 RVA: 0x001DAE9C File Offset: 0x001D909C
	public void MoveToSpace()
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null && component.GetMyWorld() != null)
		{
			component.GetMyWorld().RemoveTopPriorityPrioritizable(component);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Building component2 = base.GetComponent<Building>();
		component2.Def.UnmarkArea(cell, component2.Orientation, component2.Def.ObjectLayer, base.gameObject);
		Vector3 position = new Vector3(-1f, -1f, 0f);
		base.gameObject.transform.SetPosition(position);
		LogicPorts component3 = base.GetComponent<LogicPorts>();
		if (component3 != null)
		{
			component3.OnMove();
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, false, this);
	}

	// Token: 0x060052C9 RID: 21193 RVA: 0x001DAF6C File Offset: 0x001D916C
	public void MoveToPad(int newCell)
	{
		base.gameObject.transform.SetPosition(Grid.CellToPos(newCell, CellAlignment.Bottom, Grid.SceneLayer.Building));
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Building component = base.GetComponent<Building>();
		component.RefreshCells();
		component.Def.MarkArea(cell, component.Orientation, component.Def.ObjectLayer, base.gameObject);
		LogicPorts component2 = base.GetComponent<LogicPorts>();
		if (component2 != null)
		{
			component2.OnMove();
		}
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		if (component3 != null && component3.IsTopPriority())
		{
			component3.GetMyWorld().AddTopPriorityPrioritizable(component3);
		}
	}

	// Token: 0x040036A6 RID: 13990
	public LaunchConditionManager conditionManager;

	// Token: 0x040036A7 RID: 13991
	public Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>> moduleConditions = new Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>();

	// Token: 0x040036A8 RID: 13992
	public static readonly Operational.Flag landedFlag = new Operational.Flag("landed", Operational.Flag.Type.Requirement);

	// Token: 0x040036A9 RID: 13993
	public bool operationalLandedRequired = true;

	// Token: 0x040036AA RID: 13994
	private string rocket_module_bg_base_string = "{0}{1}";

	// Token: 0x040036AB RID: 13995
	private string rocket_module_bg_affix = "BG";

	// Token: 0x040036AC RID: 13996
	private string rocket_module_bg_anim = "on";

	// Token: 0x040036AD RID: 13997
	[SerializeField]
	private KAnimFile bgAnimFile;

	// Token: 0x040036AE RID: 13998
	protected string parentRocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x040036AF RID: 13999
	private static readonly EventSystem.IntraObjectHandler<RocketModule> DEBUG_OnDestroyDelegate = new EventSystem.IntraObjectHandler<RocketModule>(delegate(RocketModule component, object data)
	{
		component.DEBUG_OnDestroy(data);
	});

	// Token: 0x040036B0 RID: 14000
	private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketOnGround, delegate(RocketModule component, object data)
	{
		component.OnRocketOnGroundTag(data);
	});

	// Token: 0x040036B1 RID: 14001
	private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketNotOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketNotOnGround, delegate(RocketModule component, object data)
	{
		component.OnRocketNotOnGroundTag(data);
	});
}
