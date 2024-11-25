using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D7B RID: 3451
public class LogicBroadcastChannelSideScreen : SideScreenContent
{
	// Token: 0x06006C95 RID: 27797 RVA: 0x0028D606 File Offset: 0x0028B806
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicBroadcastReceiver>() != null;
	}

	// Token: 0x06006C96 RID: 27798 RVA: 0x0028D614 File Offset: 0x0028B814
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.sensor = target.GetComponent<LogicBroadcastReceiver>();
		this.Build();
	}

	// Token: 0x06006C97 RID: 27799 RVA: 0x0028D630 File Offset: 0x0028B830
	private void ClearRows()
	{
		if (this.emptySpaceRow != null)
		{
			Util.KDestroyGameObject(this.emptySpaceRow);
		}
		foreach (KeyValuePair<LogicBroadcaster, GameObject> keyValuePair in this.broadcasterRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.broadcasterRows.Clear();
	}

	// Token: 0x06006C98 RID: 27800 RVA: 0x0028D6AC File Offset: 0x0028B8AC
	private void Build()
	{
		this.headerLabel.SetText(UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.HEADER);
		this.ClearRows();
		foreach (object obj in Components.LogicBroadcasters)
		{
			LogicBroadcaster logicBroadcaster = (LogicBroadcaster)obj;
			if (!logicBroadcaster.IsNullOrDestroyed())
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
				gameObject.gameObject.name = logicBroadcaster.gameObject.GetProperName();
				global::Debug.Assert(!this.broadcasterRows.ContainsKey(logicBroadcaster), "Adding two of the same broadcaster to LogicBroadcastChannelSideScreen UI: " + logicBroadcaster.gameObject.GetProperName());
				this.broadcasterRows.Add(logicBroadcaster, gameObject);
				gameObject.SetActive(true);
			}
		}
		this.noChannelRow.SetActive(Components.LogicBroadcasters.Count == 0);
		this.Refresh();
	}

	// Token: 0x06006C99 RID: 27801 RVA: 0x0028D7A8 File Offset: 0x0028B9A8
	private void Refresh()
	{
		using (Dictionary<LogicBroadcaster, GameObject>.Enumerator enumerator = this.broadcasterRows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<LogicBroadcaster, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.gameObject.GetProperName());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("DistanceLabel").SetText(LogicBroadcastReceiver.CheckRange(this.sensor.gameObject, kvp.Key.gameObject) ? UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.IN_RANGE : UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.OUT_OF_RANGE);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key.gameObject, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key.gameObject, "ui", false).second;
				WorldContainer myWorld = kvp.Key.GetMyWorld();
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").sprite = (myWorld.IsModuleInterior ? Assets.GetSprite("icon_category_rocketry") : Def.GetUISprite(myWorld.GetComponent<ClusterGridEntity>(), "ui", false).first);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").color = (myWorld.IsModuleInterior ? Color.white : Def.GetUISprite(myWorld.GetComponent<ClusterGridEntity>(), "ui", false).second);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.sensor.SetChannel(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState((this.sensor.GetChannel() == kvp.Key) ? 1 : 0);
			}
		}
	}

	// Token: 0x04004A0D RID: 18957
	private LogicBroadcastReceiver sensor;

	// Token: 0x04004A0E RID: 18958
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04004A0F RID: 18959
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04004A10 RID: 18960
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04004A11 RID: 18961
	[SerializeField]
	private GameObject noChannelRow;

	// Token: 0x04004A12 RID: 18962
	private Dictionary<LogicBroadcaster, GameObject> broadcasterRows = new Dictionary<LogicBroadcaster, GameObject>();

	// Token: 0x04004A13 RID: 18963
	private GameObject emptySpaceRow;
}
