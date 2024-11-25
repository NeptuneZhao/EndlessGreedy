using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005AC RID: 1452
public class RocketLaunchConditionVisualizer : KMonoBehaviour
{
	// Token: 0x0600229D RID: 8861 RVA: 0x000C0CE8 File Offset: 0x000BEEE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.clusterModule = base.GetComponent<RocketModuleCluster>();
		}
		else
		{
			this.launchConditionManager = base.GetComponent<LaunchConditionManager>();
		}
		this.UpdateAllModuleData();
		base.Subscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x000C0D3A File Offset: 0x000BEF3A
	protected override void OnCleanUp()
	{
		base.Unsubscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
	}

	// Token: 0x0600229F RID: 8863 RVA: 0x000C0D53 File Offset: 0x000BEF53
	private void OnAnyRocketModuleChanged(object obj)
	{
		this.UpdateAllModuleData();
	}

	// Token: 0x060022A0 RID: 8864 RVA: 0x000C0D5C File Offset: 0x000BEF5C
	private void UpdateAllModuleData()
	{
		if (this.moduleVisualizeData != null)
		{
			this.moduleVisualizeData = null;
		}
		bool flag = this.clusterModule != null;
		List<Ref<RocketModuleCluster>> list = null;
		List<RocketModule> list2 = null;
		if (flag)
		{
			list = new List<Ref<RocketModuleCluster>>(this.clusterModule.CraftInterface.ClusterModules);
			this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[list.Count];
			list.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b)
			{
				int y = Grid.PosToXY(a.Get().transform.GetPosition()).y;
				int y2 = Grid.PosToXY(b.Get().transform.GetPosition()).y;
				return y.CompareTo(y2);
			});
		}
		else
		{
			list2 = new List<RocketModule>(this.launchConditionManager.rocketModules);
			list2.Sort(delegate(RocketModule a, RocketModule b)
			{
				int y = Grid.PosToXY(a.transform.GetPosition()).y;
				int y2 = Grid.PosToXY(b.transform.GetPosition()).y;
				return y.CompareTo(y2);
			});
			this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[list2.Count];
		}
		for (int i = 0; i < this.moduleVisualizeData.Length; i++)
		{
			RocketModule rocketModule = flag ? list[i].Get() : list2[i];
			Building component = rocketModule.GetComponent<Building>();
			this.moduleVisualizeData[i] = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData
			{
				Module = rocketModule,
				RangeMax = Mathf.FloorToInt((float)component.Def.WidthInCells / 2f),
				RangeMin = -Mathf.FloorToInt((float)(component.Def.WidthInCells - 1) / 2f)
			};
		}
	}

	// Token: 0x04001384 RID: 4996
	public RocketLaunchConditionVisualizer.RocketModuleVisualizeData[] moduleVisualizeData;

	// Token: 0x04001385 RID: 4997
	private LaunchConditionManager launchConditionManager;

	// Token: 0x04001386 RID: 4998
	private RocketModuleCluster clusterModule;

	// Token: 0x020013A6 RID: 5030
	public struct RocketModuleVisualizeData
	{
		// Token: 0x04006764 RID: 26468
		public RocketModule Module;

		// Token: 0x04006765 RID: 26469
		public Vector2I OriginOffset;

		// Token: 0x04006766 RID: 26470
		public int RangeMin;

		// Token: 0x04006767 RID: 26471
		public int RangeMax;
	}
}
