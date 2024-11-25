using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D59 RID: 3417
public class CommandModuleSideScreen : SideScreenContent
{
	// Token: 0x06006B9E RID: 27550 RVA: 0x00287B68 File Offset: 0x00285D68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ScheduleUpdate();
		MultiToggle multiToggle = this.debugVictoryButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			SpaceDestination destination = SpacecraftManager.instance.destinations.Find((SpaceDestination match) => match.GetDestinationType() == Db.Get().SpaceDestinationTypes.Wormhole);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			this.target.Launch(destination);
		}));
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
	}

	// Token: 0x06006B9F RID: 27551 RVA: 0x00287BC8 File Offset: 0x00285DC8
	private bool CheckHydrogenRocket()
	{
		RocketModule rocketModule = this.target.rocketModules.Find((RocketModule match) => match.GetComponent<RocketEngine>());
		return rocketModule != null && rocketModule.GetComponent<RocketEngine>().fuelTag == ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
	}

	// Token: 0x06006BA0 RID: 27552 RVA: 0x00287C2F File Offset: 0x00285E2F
	private void ScheduleUpdate()
	{
		this.updateHandle = UIScheduler.Instance.Schedule("RefreshCommandModuleSideScreen", 1f, delegate(object o)
		{
			this.RefreshConditions();
			this.ScheduleUpdate();
		}, null, null);
	}

	// Token: 0x06006BA1 RID: 27553 RVA: 0x00287C59 File Offset: 0x00285E59
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchConditionManager>() != null;
	}

	// Token: 0x06006BA2 RID: 27554 RVA: 0x00287C68 File Offset: 0x00285E68
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<LaunchConditionManager>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchConditionManager component");
			return;
		}
		this.ClearConditions();
		this.ConfigureConditions();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
	}

	// Token: 0x06006BA3 RID: 27555 RVA: 0x00287CDC File Offset: 0x00285EDC
	private void ClearConditions()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.conditionTable.Clear();
	}

	// Token: 0x06006BA4 RID: 27556 RVA: 0x00287D40 File Offset: 0x00285F40
	private void ConfigureConditions()
	{
		foreach (ProcessCondition key in this.target.GetLaunchConditionList())
		{
			GameObject value = Util.KInstantiateUI(this.prefabConditionLineItem, this.conditionListContainer, true);
			this.conditionTable.Add(key, value);
		}
		this.RefreshConditions();
	}

	// Token: 0x06006BA5 RID: 27557 RVA: 0x00287DB8 File Offset: 0x00285FB8
	public void RefreshConditions()
	{
		bool flag = false;
		List<ProcessCondition> launchConditionList = this.target.GetLaunchConditionList();
		foreach (ProcessCondition processCondition in launchConditionList)
		{
			if (!this.conditionTable.ContainsKey(processCondition))
			{
				flag = true;
				break;
			}
			GameObject gameObject = this.conditionTable[processCondition];
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			if (processCondition.GetParentCondition() != null && processCondition.GetParentCondition().EvaluateCondition() == ProcessCondition.Status.Failure)
			{
				gameObject.SetActive(false);
			}
			else if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			ProcessCondition.Status status = processCondition.EvaluateCondition();
			bool flag2 = status == ProcessCondition.Status.Ready;
			component.GetReference<LocText>("Label").text = processCondition.GetStatusMessage(status);
			component.GetReference<LocText>("Label").color = (flag2 ? Color.black : Color.red);
			component.GetReference<Image>("Box").color = (flag2 ? Color.black : Color.red);
			component.GetReference<Image>("Check").gameObject.SetActive(flag2);
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(processCondition.GetStatusTooltip(status));
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
		{
			if (!launchConditionList.Contains(keyValuePair.Key))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.ClearConditions();
			this.ConfigureConditions();
		}
		this.destinationButton.gameObject.SetActive(ManagementMenu.StarmapAvailable());
		this.destinationButton.onClick = delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
	}

	// Token: 0x06006BA6 RID: 27558 RVA: 0x00287FB8 File Offset: 0x002861B8
	protected override void OnCleanUp()
	{
		this.updateHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x04004958 RID: 18776
	private LaunchConditionManager target;

	// Token: 0x04004959 RID: 18777
	public GameObject conditionListContainer;

	// Token: 0x0400495A RID: 18778
	public GameObject prefabConditionLineItem;

	// Token: 0x0400495B RID: 18779
	public MultiToggle destinationButton;

	// Token: 0x0400495C RID: 18780
	public MultiToggle debugVictoryButton;

	// Token: 0x0400495D RID: 18781
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public List<Color> statusColors;

	// Token: 0x0400495E RID: 18782
	private Dictionary<ProcessCondition, GameObject> conditionTable = new Dictionary<ProcessCondition, GameObject>();

	// Token: 0x0400495F RID: 18783
	private SchedulerHandle updateHandle;
}
