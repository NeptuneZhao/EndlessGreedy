﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D57 RID: 3415
public class ClusterLocationFilterSideScreen : SideScreenContent
{
	// Token: 0x06006B8F RID: 27535 RVA: 0x00287331 File Offset: 0x00285531
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicClusterLocationSensor>() != null;
	}

	// Token: 0x06006B90 RID: 27536 RVA: 0x0028733F File Offset: 0x0028553F
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.sensor = target.GetComponent<LogicClusterLocationSensor>();
		this.Build();
	}

	// Token: 0x06006B91 RID: 27537 RVA: 0x0028735C File Offset: 0x0028555C
	private void ClearRows()
	{
		if (this.emptySpaceRow != null)
		{
			Util.KDestroyGameObject(this.emptySpaceRow);
		}
		foreach (KeyValuePair<AxialI, GameObject> keyValuePair in this.worldRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.worldRows.Clear();
	}

	// Token: 0x06006B92 RID: 27538 RVA: 0x002873D8 File Offset: 0x002855D8
	private void Build()
	{
		this.headerLabel.SetText(UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.HEADER);
		this.ClearRows();
		this.emptySpaceRow = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
		this.emptySpaceRow.SetActive(true);
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (!worldContainer.IsModuleInterior)
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
				gameObject.gameObject.name = worldContainer.GetProperName();
				AxialI myWorldLocation = worldContainer.GetMyWorldLocation();
				global::Debug.Assert(!this.worldRows.ContainsKey(myWorldLocation), "Adding two worlds/POI with the same cluster location to ClusterLocationFilterSideScreen UI: " + worldContainer.GetProperName());
				this.worldRows.Add(myWorldLocation, gameObject);
			}
		}
		this.Refresh();
	}

	// Token: 0x06006B93 RID: 27539 RVA: 0x002874CC File Offset: 0x002856CC
	private void Refresh()
	{
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.EMPTY_SPACE_ROW);
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite("hex_soft", "ui", false).first;
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Color.black;
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
		{
			this.sensor.SetSpaceEnabled(!this.sensor.ActiveInSpace);
			this.Refresh();
		};
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.ActiveInSpace ? 1 : 0);
		using (Dictionary<AxialI, GameObject>.Enumerator enumerator = this.worldRows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<AxialI, GameObject> kvp = enumerator.Current;
				ClusterGridEntity clusterGridEntity = ClusterGrid.Instance.cellContents[kvp.Key][0];
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(clusterGridEntity.GetProperName());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(clusterGridEntity, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(clusterGridEntity, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.sensor.SetLocationEnabled(kvp.Key, !this.sensor.CheckLocationSelected(kvp.Key));
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.CheckLocationSelected(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(ClusterGrid.Instance.GetCellRevealLevel(kvp.Key) == ClusterRevealLevel.Visible);
			}
		}
	}

	// Token: 0x0400494D RID: 18765
	private LogicClusterLocationSensor sensor;

	// Token: 0x0400494E RID: 18766
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x0400494F RID: 18767
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04004950 RID: 18768
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04004951 RID: 18769
	private Dictionary<AxialI, GameObject> worldRows = new Dictionary<AxialI, GameObject>();

	// Token: 0x04004952 RID: 18770
	private GameObject emptySpaceRow;
}
