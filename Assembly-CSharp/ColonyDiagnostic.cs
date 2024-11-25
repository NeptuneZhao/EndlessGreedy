using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000843 RID: 2115
public abstract class ColonyDiagnostic : ISim4000ms
{
	// Token: 0x06003B0D RID: 15117 RVA: 0x001449EE File Offset: 0x00142BEE
	public GameObject GetNextClickThroughObject()
	{
		if (this.aggregatedUniqueClickThroughObjects.Count == 0)
		{
			return null;
		}
		this.clickThroughIndex = (this.clickThroughIndex + 1) % this.aggregatedUniqueClickThroughObjects.Count;
		return this.aggregatedUniqueClickThroughObjects[this.clickThroughIndex];
	}

	// Token: 0x06003B0E RID: 15118 RVA: 0x00144A2C File Offset: 0x00142C2C
	public ColonyDiagnostic(int worldID, string name)
	{
		this.worldID = worldID;
		this.name = name;
		this.id = base.GetType().Name;
		this.IsWorldModuleInterior = ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
		this.colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Bad, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Warning, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, Constants.WARNING_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Good, Constants.POSITIVE_COLOR);
		SimAndRenderScheduler.instance.Add(this, true);
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06003B0F RID: 15119 RVA: 0x00144B61 File Offset: 0x00142D61
	// (set) Token: 0x06003B10 RID: 15120 RVA: 0x00144B69 File Offset: 0x00142D69
	public int worldID { get; protected set; }

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06003B11 RID: 15121 RVA: 0x00144B72 File Offset: 0x00142D72
	// (set) Token: 0x06003B12 RID: 15122 RVA: 0x00144B7A File Offset: 0x00142D7A
	public bool IsWorldModuleInterior { get; private set; }

	// Token: 0x06003B13 RID: 15123 RVA: 0x00144B83 File Offset: 0x00142D83
	public virtual string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06003B14 RID: 15124 RVA: 0x00144B8A File Offset: 0x00142D8A
	public void OnCleanUp()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x00144B97 File Offset: 0x00142D97
	public void Sim4000ms(float dt)
	{
		this.SetResult(ColonyDiagnosticUtility.IgnoreFirstUpdate ? ColonyDiagnosticUtility.NoDataResult : this.Evaluate());
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x00144BB4 File Offset: 0x00142DB4
	public DiagnosticCriterion[] GetCriteria()
	{
		DiagnosticCriterion[] array = new DiagnosticCriterion[this.criteria.Values.Count];
		this.criteria.Values.CopyTo(array, 0);
		return array;
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06003B17 RID: 15127 RVA: 0x00144BEA File Offset: 0x00142DEA
	// (set) Token: 0x06003B18 RID: 15128 RVA: 0x00144BF2 File Offset: 0x00142DF2
	public ColonyDiagnostic.DiagnosticResult LatestResult
	{
		get
		{
			return this.latestResult;
		}
		private set
		{
			this.latestResult = value;
		}
	}

	// Token: 0x06003B19 RID: 15129 RVA: 0x00144BFB File Offset: 0x00142DFB
	public virtual string GetAverageValueString()
	{
		if (this.tracker != null)
		{
			return this.tracker.FormatValueString(Mathf.Round(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)));
		}
		return "";
	}

	// Token: 0x06003B1A RID: 15130 RVA: 0x00144C2C File Offset: 0x00142E2C
	public virtual string GetCurrentValueString()
	{
		return "";
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x00144C33 File Offset: 0x00142E33
	protected void AddCriterion(string id, DiagnosticCriterion criterion)
	{
		if (!this.criteria.ContainsKey(id))
		{
			criterion.SetID(id);
			this.criteria.Add(id, criterion);
		}
	}

	// Token: 0x06003B1C RID: 15132 RVA: 0x00144C58 File Offset: 0x00142E58
	public virtual ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, "", null);
		bool flag = false;
		if (!ClusterManager.Instance.GetWorld(this.worldID).IsDiscovered)
		{
			return diagnosticResult;
		}
		this.aggregatedUniqueClickThroughObjects.Clear();
		foreach (KeyValuePair<string, DiagnosticCriterion> keyValuePair in this.criteria)
		{
			if (ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(this.worldID, this.id, keyValuePair.Key))
			{
				ColonyDiagnostic.DiagnosticResult diagnosticResult2 = keyValuePair.Value.Evaluate();
				if (diagnosticResult2.opinion < diagnosticResult.opinion || (!flag && diagnosticResult2.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal))
				{
					flag = true;
					diagnosticResult.opinion = diagnosticResult2.opinion;
					diagnosticResult.Message = diagnosticResult2.Message;
					diagnosticResult.clickThroughTarget = diagnosticResult2.clickThroughTarget;
					if (diagnosticResult2.clickThroughObjects != null)
					{
						foreach (GameObject item in diagnosticResult2.clickThroughObjects)
						{
							if (!this.aggregatedUniqueClickThroughObjects.Contains(item))
							{
								this.aggregatedUniqueClickThroughObjects.Add(item);
							}
						}
					}
				}
			}
		}
		return diagnosticResult;
	}

	// Token: 0x06003B1D RID: 15133 RVA: 0x00144DC0 File Offset: 0x00142FC0
	public void SetResult(ColonyDiagnostic.DiagnosticResult result)
	{
		this.LatestResult = result;
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06003B1E RID: 15134 RVA: 0x00144DC9 File Offset: 0x00142FC9
	protected string NO_MINIONS
	{
		get
		{
			return this.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		}
	}

	// Token: 0x040023D3 RID: 9171
	private int clickThroughIndex;

	// Token: 0x040023D4 RID: 9172
	private List<GameObject> aggregatedUniqueClickThroughObjects = new List<GameObject>();

	// Token: 0x040023D6 RID: 9174
	public string name;

	// Token: 0x040023D7 RID: 9175
	public string id;

	// Token: 0x040023D9 RID: 9177
	public string icon = "icon_errand_operate";

	// Token: 0x040023DA RID: 9178
	private Dictionary<string, DiagnosticCriterion> criteria = new Dictionary<string, DiagnosticCriterion>();

	// Token: 0x040023DB RID: 9179
	public ColonyDiagnostic.PresentationSetting presentationSetting;

	// Token: 0x040023DC RID: 9180
	private ColonyDiagnostic.DiagnosticResult latestResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x040023DD RID: 9181
	public Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color> colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();

	// Token: 0x040023DE RID: 9182
	public Tracker tracker;

	// Token: 0x040023DF RID: 9183
	protected float trackerSampleCountSeconds = 4f;

	// Token: 0x0200175A RID: 5978
	public enum PresentationSetting
	{
		// Token: 0x04007287 RID: 29319
		AverageValue,
		// Token: 0x04007288 RID: 29320
		CurrentValue
	}

	// Token: 0x0200175B RID: 5979
	public struct DiagnosticResult
	{
		// Token: 0x06009569 RID: 38249 RVA: 0x0035F7E4 File Offset: 0x0035D9E4
		public DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion opinion, string message, global::Tuple<Vector3, GameObject> clickThroughTarget = null)
		{
			this.message = message;
			this.opinion = opinion;
			this.clickThroughTarget = null;
			this.clickThroughObjects = null;
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x0600956B RID: 38251 RVA: 0x0035F80B File Offset: 0x0035DA0B
		// (set) Token: 0x0600956A RID: 38250 RVA: 0x0035F802 File Offset: 0x0035DA02
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x0600956C RID: 38252 RVA: 0x0035F814 File Offset: 0x0035DA14
		public string GetFormattedMessage()
		{
			switch (this.opinion)
			{
			case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WARNING_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
			case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WHITE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.POSITIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			}
			return this.message;
		}

		// Token: 0x04007289 RID: 29321
		public ColonyDiagnostic.DiagnosticResult.Opinion opinion;

		// Token: 0x0400728A RID: 29322
		public global::Tuple<Vector3, GameObject> clickThroughTarget;

		// Token: 0x0400728B RID: 29323
		public List<GameObject> clickThroughObjects;

		// Token: 0x0400728C RID: 29324
		private string message;

		// Token: 0x02002581 RID: 9601
		public enum Opinion
		{
			// Token: 0x0400A70E RID: 42766
			Unset,
			// Token: 0x0400A70F RID: 42767
			DuplicantThreatening,
			// Token: 0x0400A710 RID: 42768
			Bad,
			// Token: 0x0400A711 RID: 42769
			Warning,
			// Token: 0x0400A712 RID: 42770
			Concern,
			// Token: 0x0400A713 RID: 42771
			Suggestion,
			// Token: 0x0400A714 RID: 42772
			Tutorial,
			// Token: 0x0400A715 RID: 42773
			Normal,
			// Token: 0x0400A716 RID: 42774
			Good
		}
	}
}
