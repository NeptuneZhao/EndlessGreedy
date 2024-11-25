using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000ACB RID: 2763
[AddComponentMenu("KMonoBehaviour/scripts/LaunchConditionManager")]
public class LaunchConditionManager : KMonoBehaviour, ISim4000ms, ISim1000ms
{
	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x06005208 RID: 21000 RVA: 0x001D6E07 File Offset: 0x001D5007
	// (set) Token: 0x06005209 RID: 21001 RVA: 0x001D6E0F File Offset: 0x001D500F
	public List<RocketModule> rocketModules { get; private set; }

	// Token: 0x0600520A RID: 21002 RVA: 0x001D6E18 File Offset: 0x001D5018
	public void DEBUG_TraceModuleDestruction(string moduleName, string state, string stackTrace)
	{
		if (this.DEBUG_ModuleDestructions == null)
		{
			this.DEBUG_ModuleDestructions = new List<global::Tuple<string, string, string>>();
		}
		this.DEBUG_ModuleDestructions.Add(new global::Tuple<string, string, string>(moduleName, state, stackTrace));
	}

	// Token: 0x0600520B RID: 21003 RVA: 0x001D6E40 File Offset: 0x001D5040
	[ContextMenu("Dump Module Destructions")]
	private void DEBUG_DumpModuleDestructions()
	{
		if (this.DEBUG_ModuleDestructions == null || this.DEBUG_ModuleDestructions.Count == 0)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Sorry, no logged module destructions. :("
			});
			return;
		}
		foreach (global::Tuple<string, string, string> tuple in this.DEBUG_ModuleDestructions)
		{
			DebugUtil.LogArgs(new object[]
			{
				tuple.first,
				">",
				tuple.second,
				"\n",
				tuple.third,
				"\nEND MODULE DUMP\n\n"
			});
		}
	}

	// Token: 0x0600520C RID: 21004 RVA: 0x001D6EF4 File Offset: 0x001D50F4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rocketModules = new List<RocketModule>();
	}

	// Token: 0x0600520D RID: 21005 RVA: 0x001D6F07 File Offset: 0x001D5107
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.launchable = base.GetComponent<ILaunchableRocket>();
		this.FindModules();
		base.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged = delegate(object data)
		{
			this.FindModules();
		};
	}

	// Token: 0x0600520E RID: 21006 RVA: 0x001D6F38 File Offset: 0x001D5138
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600520F RID: 21007 RVA: 0x001D6F40 File Offset: 0x001D5140
	public void Sim1000ms(float dt)
	{
		Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
		if (spacecraftFromLaunchConditionManager == null)
		{
			return;
		}
		global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id);
		if (base.gameObject.GetComponent<LogicPorts>().GetInputValue(this.triggerPort) == 1 && spacecraftDestination != null && spacecraftDestination.id != -1)
		{
			this.Launch(spacecraftDestination);
		}
	}

	// Token: 0x06005210 RID: 21008 RVA: 0x001D6FA8 File Offset: 0x001D51A8
	public void FindModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null && component.conditionManager == null)
			{
				component.conditionManager = this;
				component.RegisterWithConditionManager();
			}
		}
	}

	// Token: 0x06005211 RID: 21009 RVA: 0x001D7024 File Offset: 0x001D5224
	public void RegisterRocketModule(RocketModule module)
	{
		if (!this.rocketModules.Contains(module))
		{
			this.rocketModules.Add(module);
		}
	}

	// Token: 0x06005212 RID: 21010 RVA: 0x001D7040 File Offset: 0x001D5240
	public void UnregisterRocketModule(RocketModule module)
	{
		this.rocketModules.Remove(module);
	}

	// Token: 0x06005213 RID: 21011 RVA: 0x001D7050 File Offset: 0x001D5250
	public List<ProcessCondition> GetLaunchConditionList()
	{
		List<ProcessCondition> list = new List<ProcessCondition>();
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			foreach (ProcessCondition item in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketPrep))
			{
				list.Add(item);
			}
			foreach (ProcessCondition item2 in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketStorage))
			{
				list.Add(item2);
			}
		}
		return list;
	}

	// Token: 0x06005214 RID: 21012 RVA: 0x001D7130 File Offset: 0x001D5330
	public void Launch(SpaceDestination destination)
	{
		if (destination == null)
		{
			global::Debug.LogError("Null destination passed to launch");
		}
		if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).state != Spacecraft.MissionState.Grounded)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode || (this.CheckReadyToLaunch() && this.CheckAbleToFly()))
		{
			this.launchable.LaunchableGameObject.Trigger(705820818, null);
			SpacecraftManager.instance.SetSpacecraftDestination(this, destination);
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).BeginMission(destination);
		}
	}

	// Token: 0x06005215 RID: 21013 RVA: 0x001D71A8 File Offset: 0x001D53A8
	public bool CheckReadyToLaunch()
	{
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketPrep).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketStorage).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06005216 RID: 21014 RVA: 0x001D7288 File Offset: 0x001D5488
	public bool CheckAbleToFly()
	{
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketFlight).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06005217 RID: 21015 RVA: 0x001D731C File Offset: 0x001D551C
	private void ClearFlightStatuses()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		foreach (KeyValuePair<ProcessCondition, Guid> keyValuePair in this.conditionStatuses)
		{
			component.RemoveStatusItem(keyValuePair.Value, false);
		}
		this.conditionStatuses.Clear();
	}

	// Token: 0x06005218 RID: 21016 RVA: 0x001D738C File Offset: 0x001D558C
	public void Sim4000ms(float dt)
	{
		bool flag = this.CheckReadyToLaunch();
		LogicPorts component = base.gameObject.GetComponent<LogicPorts>();
		if (flag)
		{
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
			if (spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Grounded || spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Launching)
			{
				component.SendSignal(this.statusPort, 1);
			}
			else
			{
				component.SendSignal(this.statusPort, 0);
			}
			KSelectable component2 = base.GetComponent<KSelectable>();
			using (List<RocketModule>.Enumerator enumerator = this.rocketModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RocketModule rocketModule = enumerator.Current;
					foreach (ProcessCondition processCondition in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketFlight))
					{
						if (processCondition.EvaluateCondition() == ProcessCondition.Status.Failure)
						{
							if (!this.conditionStatuses.ContainsKey(processCondition))
							{
								StatusItem statusItem = processCondition.GetStatusItem(ProcessCondition.Status.Failure);
								this.conditionStatuses[processCondition] = component2.AddStatusItem(statusItem, processCondition);
							}
						}
						else if (this.conditionStatuses.ContainsKey(processCondition))
						{
							component2.RemoveStatusItem(this.conditionStatuses[processCondition], false);
							this.conditionStatuses.Remove(processCondition);
						}
					}
				}
				return;
			}
		}
		this.ClearFlightStatuses();
		component.SendSignal(this.statusPort, 0);
	}

	// Token: 0x0400362A RID: 13866
	public HashedString triggerPort;

	// Token: 0x0400362B RID: 13867
	public HashedString statusPort;

	// Token: 0x0400362D RID: 13869
	private ILaunchableRocket launchable;

	// Token: 0x0400362E RID: 13870
	[Serialize]
	private List<global::Tuple<string, string, string>> DEBUG_ModuleDestructions;

	// Token: 0x0400362F RID: 13871
	private Dictionary<ProcessCondition, Guid> conditionStatuses = new Dictionary<ProcessCondition, Guid>();

	// Token: 0x02001B16 RID: 6934
	public enum ConditionType
	{
		// Token: 0x04007EBE RID: 32446
		Launch,
		// Token: 0x04007EBF RID: 32447
		Flight
	}
}
