using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D58 RID: 3416
public class CometDetectorSideScreen : SideScreenContent
{
	// Token: 0x06006B96 RID: 27542 RVA: 0x00287768 File Offset: 0x00285968
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshOptions();
		}
	}

	// Token: 0x06006B97 RID: 27543 RVA: 0x0028777C File Offset: 0x0028597C
	private void RefreshOptions()
	{
		if (this.clusterDetector != null)
		{
			int num = 0;
			this.SetClusterRow(num++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS, Assets.GetSprite("meteors"), ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower, null);
			this.SetClusterRow(num++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.DUPEMADE, Assets.GetSprite("dupe_made_ballistics"), ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject, null);
			foreach (object obj in Components.Clustercrafts)
			{
				Clustercraft clustercraft = (Clustercraft)obj;
				this.SetClusterRow(num++, clustercraft.Name, Assets.GetSprite("rocket_landing"), ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket, clustercraft);
			}
			for (int i = num; i < this.rowContainer.childCount; i++)
			{
				this.rowContainer.GetChild(i).gameObject.SetActive(false);
			}
			return;
		}
		int num2 = 0;
		this.SetRow(num2++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS, Assets.GetSprite("meteors"), null);
		foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
		{
			this.SetRow(num2++, spacecraft.GetRocketName(), Assets.GetSprite("rocket_landing"), spacecraft.launchConditions);
		}
		for (int j = num2; j < this.rowContainer.childCount; j++)
		{
			this.rowContainer.GetChild(j).gameObject.SetActive(false);
		}
	}

	// Token: 0x06006B98 RID: 27544 RVA: 0x00287944 File Offset: 0x00285B44
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x06006B99 RID: 27545 RVA: 0x00287985 File Offset: 0x00285B85
	public override void SetTarget(GameObject target)
	{
		if (DlcManager.IsExpansion1Active())
		{
			this.clusterDetector = target.GetSMI<ClusterCometDetector.Instance>();
		}
		else
		{
			this.detector = target.GetSMI<CometDetector.Instance>();
		}
		this.RefreshOptions();
	}

	// Token: 0x06006B9A RID: 27546 RVA: 0x002879B0 File Offset: 0x00285BB0
	private void SetClusterRow(int idx, string name, Sprite icon, ClusterCometDetector.Instance.ClusterCometDetectorState state, Clustercraft rocketTarget = null)
	{
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").text = name;
		component.GetReference<Image>("icon").sprite = icon;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.clusterDetector.GetDetectorState() == state && this.clusterDetector.GetClustercraftTarget() == rocketTarget) ? 1 : 0);
		ClusterCometDetector.Instance.ClusterCometDetectorState _state = state;
		Clustercraft _rocketTarget = rocketTarget;
		component2.onClick = delegate()
		{
			this.clusterDetector.SetDetectorState(_state);
			this.clusterDetector.SetClustercraftTarget(_rocketTarget);
			this.RefreshOptions();
		};
	}

	// Token: 0x06006B9B RID: 27547 RVA: 0x00287A80 File Offset: 0x00285C80
	private void SetRow(int idx, string name, Sprite icon, LaunchConditionManager target)
	{
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").text = name;
		component.GetReference<Image>("icon").sprite = icon;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.detector.GetTargetCraft() == target) ? 1 : 0);
		LaunchConditionManager _target = target;
		component2.onClick = delegate()
		{
			this.detector.SetTargetCraft(_target);
			this.RefreshOptions();
		};
	}

	// Token: 0x06006B9C RID: 27548 RVA: 0x00287B38 File Offset: 0x00285D38
	public override bool IsValidForTarget(GameObject target)
	{
		if (DlcManager.IsExpansion1Active())
		{
			return target.GetSMI<ClusterCometDetector.Instance>() != null;
		}
		return target.GetSMI<CometDetector.Instance>() != null;
	}

	// Token: 0x04004953 RID: 18771
	private CometDetector.Instance detector;

	// Token: 0x04004954 RID: 18772
	private ClusterCometDetector.Instance clusterDetector;

	// Token: 0x04004955 RID: 18773
	public GameObject rowPrefab;

	// Token: 0x04004956 RID: 18774
	public RectTransform rowContainer;

	// Token: 0x04004957 RID: 18775
	public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();
}
