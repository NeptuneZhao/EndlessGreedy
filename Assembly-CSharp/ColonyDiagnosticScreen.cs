using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C18 RID: 3096
public class ColonyDiagnosticScreen : KScreen, ISim1000ms
{
	// Token: 0x06005EFD RID: 24317 RVA: 0x00234C1C File Offset: 0x00232E1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ColonyDiagnosticScreen.Instance = this;
		this.RefreshSingleWorld(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshSingleWorld));
		MultiToggle multiToggle = this.seeAllButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			bool flag = !AllDiagnosticsScreen.Instance.isHiddenButActive;
			AllDiagnosticsScreen.Instance.Show(!flag);
		}));
	}

	// Token: 0x06005EFE RID: 24318 RVA: 0x00234C92 File Offset: 0x00232E92
	protected override void OnForcedCleanUp()
	{
		ColonyDiagnosticScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06005EFF RID: 24319 RVA: 0x00234CA0 File Offset: 0x00232EA0
	private void RefreshSingleWorld(object data = null)
	{
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in this.diagnosticRows)
		{
			diagnosticRow.OnCleanUp();
			Util.KDestroyGameObject(diagnosticRow.gameObject);
		}
		this.diagnosticRows.Clear();
		this.SpawnTrackerLines(ClusterManager.Instance.activeWorldId);
	}

	// Token: 0x06005F00 RID: 24320 RVA: 0x00234D18 File Offset: 0x00232F18
	private void SpawnTrackerLines(int world)
	{
		this.AddDiagnostic<BreathabilityDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<FoodDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<StressDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RadiationDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<ReactorDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<FloatingRocketDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketFuelDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketOxidizerDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<FarmDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<ToiletDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<BedDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<IdleDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<TrappedDuplicantDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<EntombedDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<PowerUseDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<BatteryDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketsInOrbitDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<MeteorDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		List<ColonyDiagnosticScreen.DiagnosticRow> list = new List<ColonyDiagnosticScreen.DiagnosticRow>();
		foreach (ColonyDiagnosticScreen.DiagnosticRow item in this.diagnosticRows)
		{
			list.Add(item);
		}
		list.Sort((ColonyDiagnosticScreen.DiagnosticRow a, ColonyDiagnosticScreen.DiagnosticRow b) => a.diagnostic.name.CompareTo(b.diagnostic.name));
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in list)
		{
			diagnosticRow.gameObject.transform.SetAsLastSibling();
		}
		list.Clear();
		this.seeAllButton.transform.SetAsLastSibling();
		this.RefreshAll();
	}

	// Token: 0x06005F01 RID: 24321 RVA: 0x00234F60 File Offset: 0x00233160
	private GameObject AddDiagnostic<T>(int worldID, GameObject parent, List<ColonyDiagnosticScreen.DiagnosticRow> parentCollection) where T : ColonyDiagnostic
	{
		T diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic<T>(worldID);
		if (diagnostic == null)
		{
			return null;
		}
		GameObject gameObject = Util.KInstantiateUI(this.linePrefab, parent, true);
		parentCollection.Add(new ColonyDiagnosticScreen.DiagnosticRow(worldID, gameObject, diagnostic));
		return gameObject;
	}

	// Token: 0x06005F02 RID: 24322 RVA: 0x00234FA5 File Offset: 0x002331A5
	public static void SetIndication(ColonyDiagnostic.DiagnosticResult.Opinion opinion, GameObject indicatorGameObject)
	{
		indicatorGameObject.GetComponentInChildren<Image>().color = ColonyDiagnosticScreen.GetDiagnosticIndicationColor(opinion);
	}

	// Token: 0x06005F03 RID: 24323 RVA: 0x00234FB8 File Offset: 0x002331B8
	public static Color GetDiagnosticIndicationColor(ColonyDiagnostic.DiagnosticResult.Opinion opinion)
	{
		switch (opinion)
		{
		case ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
			return Constants.NEGATIVE_COLOR;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
			return Constants.WARNING_COLOR;
		}
		return Color.white;
	}

	// Token: 0x06005F04 RID: 24324 RVA: 0x00234FF5 File Offset: 0x002331F5
	public void Sim1000ms(float dt)
	{
		this.RefreshAll();
	}

	// Token: 0x06005F05 RID: 24325 RVA: 0x00235000 File Offset: 0x00233200
	public void RefreshAll()
	{
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in this.diagnosticRows)
		{
			if (diagnosticRow.worldID == ClusterManager.Instance.activeWorldId)
			{
				this.UpdateDiagnosticRow(diagnosticRow);
			}
		}
		ColonyDiagnosticScreen.SetIndication(ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(ClusterManager.Instance.activeWorldId), this.rootIndicator);
		this.seeAllButton.GetComponentInChildren<LocText>().SetText(string.Format(UI.DIAGNOSTICS_SCREEN.SEE_ALL, AllDiagnosticsScreen.Instance.GetRowCount()));
	}

	// Token: 0x06005F06 RID: 24326 RVA: 0x002350B4 File Offset: 0x002332B4
	private ColonyDiagnostic.DiagnosticResult.Opinion UpdateDiagnosticRow(ColonyDiagnosticScreen.DiagnosticRow row)
	{
		ColonyDiagnostic.DiagnosticResult.Opinion currentDisplayedResult = row.currentDisplayedResult;
		bool activeInHierarchy = row.gameObject.activeInHierarchy;
		if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(row.diagnostic.id))
		{
			this.SetRowActive(row, false);
		}
		else
		{
			switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[row.worldID][row.diagnostic.id])
			{
			case ColonyDiagnosticUtility.DisplaySetting.Always:
				this.SetRowActive(row, true);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
				this.SetRowActive(row, row.diagnostic.LatestResult.opinion < ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.Never:
				this.SetRowActive(row, false);
				break;
			}
			if (row.gameObject.activeInHierarchy && (row.currentDisplayedResult < currentDisplayedResult || (row.currentDisplayedResult < ColonyDiagnostic.DiagnosticResult.Opinion.Normal && !activeInHierarchy)) && row.CheckAllowVisualNotification())
			{
				row.TriggerVisualNotification();
			}
		}
		return row.diagnostic.LatestResult.opinion;
	}

	// Token: 0x06005F07 RID: 24327 RVA: 0x002351A0 File Offset: 0x002333A0
	private void SetRowActive(ColonyDiagnosticScreen.DiagnosticRow row, bool active)
	{
		if (row.gameObject.activeSelf != active)
		{
			row.gameObject.SetActive(active);
			row.ResolveNotificationRoutine();
		}
	}

	// Token: 0x04003F89 RID: 16265
	public GameObject linePrefab;

	// Token: 0x04003F8A RID: 16266
	public static ColonyDiagnosticScreen Instance;

	// Token: 0x04003F8B RID: 16267
	private List<ColonyDiagnosticScreen.DiagnosticRow> diagnosticRows = new List<ColonyDiagnosticScreen.DiagnosticRow>();

	// Token: 0x04003F8C RID: 16268
	public GameObject header;

	// Token: 0x04003F8D RID: 16269
	public GameObject contentContainer;

	// Token: 0x04003F8E RID: 16270
	public GameObject rootIndicator;

	// Token: 0x04003F8F RID: 16271
	public MultiToggle seeAllButton;

	// Token: 0x04003F90 RID: 16272
	public static Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> notificationSoundsActive = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string>
	{
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening,
			"Diagnostic_Active_DuplicantThreatening"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Bad,
			"Diagnostic_Active_Bad"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Warning,
			"Diagnostic_Active_Warning"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Concern,
			"Diagnostic_Active_Concern"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion,
			"Diagnostic_Active_Suggestion"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial,
			"Diagnostic_Active_Tutorial"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			""
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Good,
			""
		}
	};

	// Token: 0x04003F91 RID: 16273
	public static Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> notificationSoundsInactive = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string>
	{
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening,
			"Diagnostic_Inactive_DuplicantThreatening"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Bad,
			"Diagnostic_Inactive_Bad"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Warning,
			"Diagnostic_Inactive_Warning"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Concern,
			"Diagnostic_Inactive_Concern"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion,
			"Diagnostic_Inactive_Suggestion"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial,
			"Diagnostic_Inactive_Tutorial"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			""
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Good,
			""
		}
	};

	// Token: 0x02001D02 RID: 7426
	private class DiagnosticRow : ISim4000ms
	{
		// Token: 0x0600A76A RID: 42858 RVA: 0x0039A3F0 File Offset: 0x003985F0
		public DiagnosticRow(int worldID, GameObject gameObject, ColonyDiagnostic diagnostic)
		{
			global::Debug.Assert(diagnostic != null);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			this.worldID = worldID;
			this.sparkLayer = component.GetReference<SparkLayer>("SparkLayer");
			this.diagnostic = diagnostic;
			this.titleLabel = component.GetReference<LocText>("TitleLabel");
			this.valueLabel = component.GetReference<LocText>("ValueLabel");
			this.indicator = component.GetReference<Image>("Indicator");
			this.image = component.GetReference<Image>("Image");
			this.tooltip = gameObject.GetComponent<ToolTip>();
			this.gameObject = gameObject;
			this.titleLabel.SetText(diagnostic.name);
			this.sparkLayer.colorRules.setOwnColor = false;
			if (diagnostic.tracker == null)
			{
				this.sparkLayer.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				this.sparkLayer.ClearLines();
				global::Tuple<float, float>[] points = diagnostic.tracker.ChartableData(600f);
				this.sparkLayer.NewLine(points, diagnostic.name);
			}
			this.button = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = this.button;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
			{
				KSelectable kselectable = null;
				Vector3 pos = Vector3.zero;
				if (diagnostic.LatestResult.clickThroughTarget != null)
				{
					pos = diagnostic.LatestResult.clickThroughTarget.first;
					kselectable = ((diagnostic.LatestResult.clickThroughTarget.second == null) ? null : diagnostic.LatestResult.clickThroughTarget.second.GetComponent<KSelectable>());
				}
				else
				{
					GameObject nextClickThroughObject = diagnostic.GetNextClickThroughObject();
					if (nextClickThroughObject != null)
					{
						kselectable = nextClickThroughObject.GetComponent<KSelectable>();
						pos = nextClickThroughObject.transform.GetPosition();
					}
				}
				if (kselectable == null)
				{
					CameraController.Instance.ActiveWorldStarWipe(diagnostic.worldID, null);
					return;
				}
				SelectTool.Instance.SelectAndFocus(pos, kselectable);
			}));
			this.defaultIndicatorSizeDelta = Vector2.zero;
			this.Update(true);
			SimAndRenderScheduler.instance.Add(this, true);
		}

		// Token: 0x0600A76B RID: 42859 RVA: 0x0039A57B File Offset: 0x0039877B
		public void OnCleanUp()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x0600A76C RID: 42860 RVA: 0x0039A588 File Offset: 0x00398788
		public void Sim4000ms(float dt)
		{
			this.Update(false);
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600A76D RID: 42861 RVA: 0x0039A591 File Offset: 0x00398791
		// (set) Token: 0x0600A76E RID: 42862 RVA: 0x0039A599 File Offset: 0x00398799
		public GameObject gameObject { get; private set; }

		// Token: 0x0600A76F RID: 42863 RVA: 0x0039A5A4 File Offset: 0x003987A4
		public void Update(bool force = false)
		{
			if (!force && ClusterManager.Instance.activeWorldId != this.worldID)
			{
				return;
			}
			Color color = Color.white;
			global::Debug.Assert(this.diagnostic.LatestResult.opinion > ColonyDiagnostic.DiagnosticResult.Opinion.Unset, string.Format("{0} criteria returned no opinion. Make sure the DiagnosticResult parameters are used or an opinion result is otherwise set in all of its criteria", this.diagnostic));
			this.currentDisplayedResult = this.diagnostic.LatestResult.opinion;
			color = this.diagnostic.colors[this.diagnostic.LatestResult.opinion];
			if (this.diagnostic.tracker != null)
			{
				global::Tuple<float, float>[] data = this.diagnostic.tracker.ChartableData(600f);
				this.sparkLayer.RefreshLine(data, this.diagnostic.name);
				this.sparkLayer.SetColor(color);
			}
			this.indicator.color = this.diagnostic.colors[this.diagnostic.LatestResult.opinion];
			this.tooltip.SetSimpleTooltip((this.diagnostic.LatestResult.Message.IsNullOrWhiteSpace() ? UI.COLONY_DIAGNOSTICS.GENERIC_STATUS_NORMAL.text : this.diagnostic.LatestResult.Message) + "\n\n" + UI.COLONY_DIAGNOSTICS.MUTE_TUTORIAL.text);
			ColonyDiagnostic.PresentationSetting presentationSetting = this.diagnostic.presentationSetting;
			if (presentationSetting == ColonyDiagnostic.PresentationSetting.AverageValue || presentationSetting != ColonyDiagnostic.PresentationSetting.CurrentValue)
			{
				this.valueLabel.SetText(this.diagnostic.GetAverageValueString());
			}
			else
			{
				this.valueLabel.SetText(this.diagnostic.GetCurrentValueString());
			}
			if (!string.IsNullOrEmpty(this.diagnostic.icon))
			{
				this.image.sprite = Assets.GetSprite(this.diagnostic.icon);
			}
			if (color == Constants.NEUTRAL_COLOR)
			{
				color = Color.white;
			}
			this.titleLabel.color = color;
		}

		// Token: 0x0600A770 RID: 42864 RVA: 0x0039A787 File Offset: 0x00398987
		public bool CheckAllowVisualNotification()
		{
			return this.timeOfLastNotification == 0f || GameClock.Instance.GetTime() >= this.timeOfLastNotification + 300f;
		}

		// Token: 0x0600A771 RID: 42865 RVA: 0x0039A7B4 File Offset: 0x003989B4
		public void TriggerVisualNotification()
		{
			if (DebugHandler.NotificationsDisabled)
			{
				return;
			}
			if (this.activeRoutine == null)
			{
				this.timeOfLastNotification = GameClock.Instance.GetTime();
				KFMOD.PlayUISound(GlobalAssets.GetSound(ColonyDiagnosticScreen.notificationSoundsActive[this.currentDisplayedResult], false));
				this.activeRoutine = this.gameObject.GetComponent<KMonoBehaviour>().StartCoroutine(this.VisualNotificationRoutine());
			}
		}

		// Token: 0x0600A772 RID: 42866 RVA: 0x0039A818 File Offset: 0x00398A18
		private IEnumerator VisualNotificationRoutine()
		{
			this.gameObject.GetComponentInChildren<NotificationAnimator>().Begin(false);
			RectTransform indicator = this.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Indicator").rectTransform;
			this.defaultIndicatorSizeDelta = Vector2.zero;
			indicator.sizeDelta = this.defaultIndicatorSizeDelta;
			float bounceDuration = 3f;
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			this.ResolveNotificationRoutine();
			yield break;
		}

		// Token: 0x0600A773 RID: 42867 RVA: 0x0039A828 File Offset: 0x00398A28
		public void ResolveNotificationRoutine()
		{
			this.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Indicator").rectTransform.sizeDelta = Vector2.zero;
			this.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").localPosition = Vector2.zero;
			this.activeRoutine = null;
		}

		// Token: 0x040085BC RID: 34236
		private const float displayHistoryPeriod = 600f;

		// Token: 0x040085BD RID: 34237
		public ColonyDiagnostic diagnostic;

		// Token: 0x040085BE RID: 34238
		public SparkLayer sparkLayer;

		// Token: 0x040085C0 RID: 34240
		public int worldID;

		// Token: 0x040085C1 RID: 34241
		private LocText titleLabel;

		// Token: 0x040085C2 RID: 34242
		private LocText valueLabel;

		// Token: 0x040085C3 RID: 34243
		private Image indicator;

		// Token: 0x040085C4 RID: 34244
		private ToolTip tooltip;

		// Token: 0x040085C5 RID: 34245
		private MultiToggle button;

		// Token: 0x040085C6 RID: 34246
		private Image image;

		// Token: 0x040085C7 RID: 34247
		public ColonyDiagnostic.DiagnosticResult.Opinion currentDisplayedResult;

		// Token: 0x040085C8 RID: 34248
		private Vector2 defaultIndicatorSizeDelta;

		// Token: 0x040085C9 RID: 34249
		private float timeOfLastNotification;

		// Token: 0x040085CA RID: 34250
		private const float MIN_TIME_BETWEEN_NOTIFICATIONS = 300f;

		// Token: 0x040085CB RID: 34251
		private Coroutine activeRoutine;
	}
}
