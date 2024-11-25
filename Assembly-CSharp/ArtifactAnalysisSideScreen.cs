using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D46 RID: 3398
public class ArtifactAnalysisSideScreen : SideScreenContent
{
	// Token: 0x06006AE0 RID: 27360 RVA: 0x00283DE4 File Offset: 0x00281FE4
	public override string GetTitle()
	{
		if (this.targetArtifactStation != null)
		{
			return string.Format(base.GetTitle(), this.targetArtifactStation.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x06006AE1 RID: 27361 RVA: 0x00283E11 File Offset: 0x00282011
	public override void ClearTarget()
	{
		this.targetArtifactStation = null;
		base.ClearTarget();
	}

	// Token: 0x06006AE2 RID: 27362 RVA: 0x00283E20 File Offset: 0x00282020
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<ArtifactAnalysisStation.StatesInstance>() != null;
	}

	// Token: 0x06006AE3 RID: 27363 RVA: 0x00283E2C File Offset: 0x0028202C
	private void RefreshRows()
	{
		if (this.undiscoveredRow == null)
		{
			this.undiscoveredRow = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
			HierarchyReferences component = this.undiscoveredRow.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED);
			component.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED_TOOLTIP);
			component.GetReference<Image>("icon").sprite = Assets.GetSprite("unknown");
			component.GetReference<Image>("icon").color = Color.grey;
		}
		List<string> analyzedArtifactIDs = ArtifactSelector.Instance.GetAnalyzedArtifactIDs();
		this.undiscoveredRow.SetActive(analyzedArtifactIDs.Count == 0);
		foreach (string text in analyzedArtifactIDs)
		{
			if (!this.rows.ContainsKey(text))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(text, gameObject);
				GameObject artifactPrefab = Assets.GetPrefab(text);
				HierarchyReferences component2 = gameObject.GetComponent<HierarchyReferences>();
				component2.GetReference<LocText>("label").SetText(artifactPrefab.GetProperName());
				component2.GetReference<Image>("icon").sprite = Def.GetUISprite(artifactPrefab, text, false).first;
				component2.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenEvent(artifactPrefab);
				};
			}
		}
	}

	// Token: 0x06006AE4 RID: 27364 RVA: 0x00283FDC File Offset: 0x002821DC
	private void OpenEvent(GameObject artifactPrefab)
	{
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.ArtifactReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.artifact = artifactPrefab;
		artifactPrefab.GetComponent<KPrefabID>();
		artifactPrefab.GetComponent<InfoDescription>();
		string text = artifactPrefab.PrefabID().Name.ToUpper();
		text = text.Replace("ARTIFACT_", "");
		string key = "STRINGS.UI.SPACEARTIFACTS." + text + ".ARTIFACT";
		string text2 = string.Format("<b>{0}</b>", artifactPrefab.GetProperName());
		StringEntry stringEntry;
		Strings.TryGet(key, out stringEntry);
		if (stringEntry != null && !stringEntry.String.IsNullOrWhiteSpace())
		{
			text2 = text2 + "\n\n" + stringEntry.String;
		}
		if (text2 != null && !text2.IsNullOrWhiteSpace())
		{
			statesInstance.SetTextParameter("desc", text2);
		}
		statesInstance.ShowEventPopup();
	}

	// Token: 0x06006AE5 RID: 27365 RVA: 0x002840B2 File Offset: 0x002822B2
	public override void SetTarget(GameObject target)
	{
		this.targetArtifactStation = target;
		base.SetTarget(target);
		this.RefreshRows();
	}

	// Token: 0x040048DB RID: 18651
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x040048DC RID: 18652
	private GameObject targetArtifactStation;

	// Token: 0x040048DD RID: 18653
	[SerializeField]
	private GameObject rowContainer;

	// Token: 0x040048DE RID: 18654
	private Dictionary<string, GameObject> rows = new Dictionary<string, GameObject>();

	// Token: 0x040048DF RID: 18655
	private GameObject undiscoveredRow;
}
