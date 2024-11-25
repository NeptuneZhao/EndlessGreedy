using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005AB RID: 1451
[AddComponentMenu("KMonoBehaviour/scripts/ReportManager")]
public class ReportManager : KMonoBehaviour
{
	// Token: 0x17000188 RID: 392
	// (get) Token: 0x0600228D RID: 8845 RVA: 0x000C0401 File Offset: 0x000BE601
	public List<ReportManager.DailyReport> reports
	{
		get
		{
			return this.dailyReports;
		}
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x000C0409 File Offset: 0x000BE609
	public static void DestroyInstance()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x0600228F RID: 8847 RVA: 0x000C0411 File Offset: 0x000BE611
	// (set) Token: 0x06002290 RID: 8848 RVA: 0x000C0418 File Offset: 0x000BE618
	public static ReportManager Instance { get; private set; }

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06002291 RID: 8849 RVA: 0x000C0420 File Offset: 0x000BE620
	public ReportManager.DailyReport TodaysReport
	{
		get
		{
			return this.todaysReport;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06002292 RID: 8850 RVA: 0x000C0428 File Offset: 0x000BE628
	public ReportManager.DailyReport YesterdaysReport
	{
		get
		{
			if (this.dailyReports.Count <= 1)
			{
				return null;
			}
			return this.dailyReports[this.dailyReports.Count - 1];
		}
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x000C0452 File Offset: 0x000BE652
	protected override void OnPrefabInit()
	{
		ReportManager.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1917495436, new Action<object>(this.OnSaveGameReady));
		this.noteStorage = new ReportManager.NoteStorage();
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x000C0487 File Offset: 0x000BE687
	protected override void OnCleanUp()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x000C048F File Offset: 0x000BE68F
	[CustomSerialize]
	private void CustomSerialize(BinaryWriter writer)
	{
		writer.Write(0);
		this.noteStorage.Serialize(writer);
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x000C04A4 File Offset: 0x000BE6A4
	[CustomDeserialize]
	private void CustomDeserialize(IReader reader)
	{
		if (this.noteStorageBytes == null)
		{
			global::Debug.Assert(reader.ReadInt32() == 0);
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(reader.RawBytes()));
			binaryReader.BaseStream.Position = (long)reader.Position;
			this.noteStorage.Deserialize(binaryReader);
			reader.SkipBytes((int)binaryReader.BaseStream.Position - reader.Position);
		}
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x000C050F File Offset: 0x000BE70F
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.noteStorageBytes != null)
		{
			this.noteStorage.Deserialize(new BinaryReader(new MemoryStream(this.noteStorageBytes)));
			this.noteStorageBytes = null;
		}
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x000C053C File Offset: 0x000BE73C
	private void OnSaveGameReady(object data)
	{
		base.Subscribe(GameClock.Instance.gameObject, -722330267, new Action<object>(this.OnNightTime));
		if (this.todaysReport == null)
		{
			this.todaysReport = new ReportManager.DailyReport(this);
			this.todaysReport.day = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x000C058F File Offset: 0x000BE78F
	public void ReportValue(ReportManager.ReportType reportType, float value, string note = null, string context = null)
	{
		this.TodaysReport.AddData(reportType, value, note, context);
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x000C05A4 File Offset: 0x000BE7A4
	private void OnNightTime(object data)
	{
		this.dailyReports.Add(this.todaysReport);
		int day = this.todaysReport.day;
		ManagementMenuNotification notification = new ManagementMenuNotification(global::Action.ManageReport, NotificationValence.Good, null, string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TITLE, day), NotificationType.Good, (List<Notification> n, object d) => string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TOOLTIP, day), null, true, 0f, delegate(object d)
		{
			ManagementMenu.Instance.OpenReports(day);
		}, null, null, true);
		if (this.notifier == null)
		{
			global::Debug.LogError("Cant notify, null notifier");
		}
		else
		{
			this.notifier.Add(notification, "");
		}
		this.todaysReport = new ReportManager.DailyReport(this);
		this.todaysReport.day = GameUtil.GetCurrentCycle() + 1;
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x000C066C File Offset: 0x000BE86C
	public ReportManager.DailyReport FindReport(int day)
	{
		foreach (ReportManager.DailyReport dailyReport in this.dailyReports)
		{
			if (dailyReport.day == day)
			{
				return dailyReport;
			}
		}
		if (this.todaysReport.day == day)
		{
			return this.todaysReport;
		}
		return null;
	}

	// Token: 0x0600229C RID: 8860 RVA: 0x000C06E0 File Offset: 0x000BE8E0
	public ReportManager()
	{
		Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> dictionary = new Dictionary<ReportManager.ReportType, ReportManager.ReportGroup>();
		dictionary.Add(ReportManager.ReportType.DuplicantHeader, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.DUPLICANT_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.CaloriesCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedCalories(v, GameUtil.TimeSlice.None, true), true, 1, UI.ENDOFDAYREPORT.CALORIES_CREATED.NAME, UI.ENDOFDAYREPORT.CALORIES_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CALORIES_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.StressDelta, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.STRESS_DELTA.NAME, UI.ENDOFDAYREPORT.STRESS_DELTA.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.STRESS_DELTA.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseAdded, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.DISEASE_ADDED.NAME, UI.ENDOFDAYREPORT.DISEASE_ADDED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_ADDED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseStatus, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedDiseaseAmount((int)v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.DISEASE_STATUS.NAME, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.LevelUp, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.LEVEL_UP.NAME, UI.ENDOFDAYREPORT.LEVEL_UP.TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ToiletIncident, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.TOILET_INCIDENT.NAME, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ChoreStatus, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.CHORE_STATUS.NAME, UI.ENDOFDAYREPORT.CHORE_STATUS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CHORE_STATUS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DomesticatedCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.WildCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.RocketsInFlight, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NAME, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.TimeSpentHeader, new ReportManager.ReportGroup(null, true, 2, UI.ENDOFDAYREPORT.TIME_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.WorkTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.WORK_TIME.NAME, UI.ENDOFDAYREPORT.WORK_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.TravelTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.TRAVEL_TIME.NAME, UI.ENDOFDAYREPORT.TRAVEL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.PersonalTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.PERSONAL_TIME.NAME, UI.ENDOFDAYREPORT.PERSONAL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.IdleTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.IDLE_TIME.NAME, UI.ENDOFDAYREPORT.IDLE_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.BaseHeader, new ReportManager.ReportGroup(null, true, 3, UI.ENDOFDAYREPORT.BASE_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.OxygenCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true, 3, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NAME, UI.ENDOFDAYREPORT.OXYGEN_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyCreated, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_USAGE.NAME, UI.ENDOFDAYREPORT.ENERGY_USAGE.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ENERGY_USAGE.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyWasted, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_WASTED.NAME, UI.ENDOFDAYREPORT.NONE, UI.ENDOFDAYREPORT.ENERGY_WASTED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenToilet, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenSublimation, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		this.ReportGroups = dictionary;
		this.dailyReports = new List<ReportManager.DailyReport>();
		base..ctor();
	}

	// Token: 0x0400137D RID: 4989
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x0400137E RID: 4990
	private ReportManager.NoteStorage noteStorage;

	// Token: 0x0400137F RID: 4991
	public Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> ReportGroups;

	// Token: 0x04001380 RID: 4992
	[Serialize]
	private List<ReportManager.DailyReport> dailyReports;

	// Token: 0x04001381 RID: 4993
	[Serialize]
	private ReportManager.DailyReport todaysReport;

	// Token: 0x04001382 RID: 4994
	[Serialize]
	private byte[] noteStorageBytes;

	// Token: 0x0200139D RID: 5021
	// (Invoke) Token: 0x060087B6 RID: 34742
	public delegate string FormattingFn(float v);

	// Token: 0x0200139E RID: 5022
	// (Invoke) Token: 0x060087BA RID: 34746
	public delegate string GroupFormattingFn(float v, float numEntries);

	// Token: 0x0200139F RID: 5023
	public enum ReportType
	{
		// Token: 0x04006723 RID: 26403
		DuplicantHeader,
		// Token: 0x04006724 RID: 26404
		CaloriesCreated,
		// Token: 0x04006725 RID: 26405
		StressDelta,
		// Token: 0x04006726 RID: 26406
		LevelUp,
		// Token: 0x04006727 RID: 26407
		DiseaseStatus,
		// Token: 0x04006728 RID: 26408
		DiseaseAdded,
		// Token: 0x04006729 RID: 26409
		ToiletIncident,
		// Token: 0x0400672A RID: 26410
		ChoreStatus,
		// Token: 0x0400672B RID: 26411
		TimeSpentHeader,
		// Token: 0x0400672C RID: 26412
		TimeSpent,
		// Token: 0x0400672D RID: 26413
		WorkTime,
		// Token: 0x0400672E RID: 26414
		TravelTime,
		// Token: 0x0400672F RID: 26415
		PersonalTime,
		// Token: 0x04006730 RID: 26416
		IdleTime,
		// Token: 0x04006731 RID: 26417
		BaseHeader,
		// Token: 0x04006732 RID: 26418
		ContaminatedOxygenFlatulence,
		// Token: 0x04006733 RID: 26419
		ContaminatedOxygenToilet,
		// Token: 0x04006734 RID: 26420
		ContaminatedOxygenSublimation,
		// Token: 0x04006735 RID: 26421
		OxygenCreated,
		// Token: 0x04006736 RID: 26422
		EnergyCreated,
		// Token: 0x04006737 RID: 26423
		EnergyWasted,
		// Token: 0x04006738 RID: 26424
		DomesticatedCritters,
		// Token: 0x04006739 RID: 26425
		WildCritters,
		// Token: 0x0400673A RID: 26426
		RocketsInFlight
	}

	// Token: 0x020013A0 RID: 5024
	public struct ReportGroup
	{
		// Token: 0x060087BD RID: 34749 RVA: 0x0032C7A0 File Offset: 0x0032A9A0
		public ReportGroup(ReportManager.FormattingFn formatfn, bool reportIfZero, int group, string stringKey, string positiveTooltip, string negativeTooltip, ReportManager.ReportEntry.Order pos_note_order = ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order neg_note_order = ReportManager.ReportEntry.Order.Unordered, bool is_header = false, ReportManager.GroupFormattingFn group_format_fn = null)
		{
			ReportManager.FormattingFn formattingFn;
			if (formatfn == null)
			{
				formattingFn = ((float v) => v.ToString());
			}
			else
			{
				formattingFn = formatfn;
			}
			this.formatfn = formattingFn;
			this.groupFormatfn = group_format_fn;
			this.stringKey = stringKey;
			this.positiveTooltip = positiveTooltip;
			this.negativeTooltip = negativeTooltip;
			this.reportIfZero = reportIfZero;
			this.group = group;
			this.posNoteOrder = pos_note_order;
			this.negNoteOrder = neg_note_order;
			this.isHeader = is_header;
		}

		// Token: 0x0400673B RID: 26427
		public ReportManager.FormattingFn formatfn;

		// Token: 0x0400673C RID: 26428
		public ReportManager.GroupFormattingFn groupFormatfn;

		// Token: 0x0400673D RID: 26429
		public string stringKey;

		// Token: 0x0400673E RID: 26430
		public string positiveTooltip;

		// Token: 0x0400673F RID: 26431
		public string negativeTooltip;

		// Token: 0x04006740 RID: 26432
		public bool reportIfZero;

		// Token: 0x04006741 RID: 26433
		public int group;

		// Token: 0x04006742 RID: 26434
		public bool isHeader;

		// Token: 0x04006743 RID: 26435
		public ReportManager.ReportEntry.Order posNoteOrder;

		// Token: 0x04006744 RID: 26436
		public ReportManager.ReportEntry.Order negNoteOrder;
	}

	// Token: 0x020013A1 RID: 5025
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ReportEntry
	{
		// Token: 0x060087BE RID: 34750 RVA: 0x0032C820 File Offset: 0x0032AA20
		public ReportEntry(ReportManager.ReportType reportType, int note_storage_id, string context, bool is_child = false)
		{
			this.reportType = reportType;
			this.context = context;
			this.isChild = is_child;
			this.accumulate = 0f;
			this.accPositive = 0f;
			this.accNegative = 0f;
			this.noteStorageId = note_storage_id;
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x060087BF RID: 34751 RVA: 0x0032C878 File Offset: 0x0032AA78
		public float Positive
		{
			get
			{
				return this.accPositive;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060087C0 RID: 34752 RVA: 0x0032C880 File Offset: 0x0032AA80
		public float Negative
		{
			get
			{
				return this.accNegative;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x060087C1 RID: 34753 RVA: 0x0032C888 File Offset: 0x0032AA88
		public float Net
		{
			get
			{
				return this.accPositive + this.accNegative;
			}
		}

		// Token: 0x060087C2 RID: 34754 RVA: 0x0032C897 File Offset: 0x0032AA97
		[OnDeserializing]
		private void OnDeserialize()
		{
			this.contextEntries.Clear();
		}

		// Token: 0x060087C3 RID: 34755 RVA: 0x0032C8A4 File Offset: 0x0032AAA4
		public void IterateNotes(Action<ReportManager.ReportEntry.Note> callback)
		{
			ReportManager.Instance.noteStorage.IterateNotes(this.noteStorageId, callback);
		}

		// Token: 0x060087C4 RID: 34756 RVA: 0x0032C8BC File Offset: 0x0032AABC
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.gameHash != -1)
			{
				this.reportType = (ReportManager.ReportType)this.gameHash;
				this.gameHash = -1;
			}
		}

		// Token: 0x060087C5 RID: 34757 RVA: 0x0032C8DC File Offset: 0x0032AADC
		public void AddData(ReportManager.NoteStorage note_storage, float value, string note = null, string dataContext = null)
		{
			this.AddActualData(note_storage, value, note);
			if (dataContext != null)
			{
				ReportManager.ReportEntry reportEntry = null;
				for (int i = 0; i < this.contextEntries.Count; i++)
				{
					if (this.contextEntries[i].context == dataContext)
					{
						reportEntry = this.contextEntries[i];
						break;
					}
				}
				if (reportEntry == null)
				{
					reportEntry = new ReportManager.ReportEntry(this.reportType, note_storage.GetNewNoteId(), dataContext, true);
					this.contextEntries.Add(reportEntry);
				}
				reportEntry.AddActualData(note_storage, value, note);
			}
		}

		// Token: 0x060087C6 RID: 34758 RVA: 0x0032C968 File Offset: 0x0032AB68
		private void AddActualData(ReportManager.NoteStorage note_storage, float value, string note = null)
		{
			this.accumulate += value;
			if (value > 0f)
			{
				this.accPositive += value;
			}
			else
			{
				this.accNegative += value;
			}
			if (note != null)
			{
				note_storage.Add(this.noteStorageId, value, note);
			}
		}

		// Token: 0x060087C7 RID: 34759 RVA: 0x0032C9BA File Offset: 0x0032ABBA
		public bool HasContextEntries()
		{
			return this.contextEntries.Count > 0;
		}

		// Token: 0x04006745 RID: 26437
		[Serialize]
		public int noteStorageId;

		// Token: 0x04006746 RID: 26438
		[Serialize]
		public int gameHash = -1;

		// Token: 0x04006747 RID: 26439
		[Serialize]
		public ReportManager.ReportType reportType;

		// Token: 0x04006748 RID: 26440
		[Serialize]
		public string context;

		// Token: 0x04006749 RID: 26441
		[Serialize]
		public float accumulate;

		// Token: 0x0400674A RID: 26442
		[Serialize]
		public float accPositive;

		// Token: 0x0400674B RID: 26443
		[Serialize]
		public float accNegative;

		// Token: 0x0400674C RID: 26444
		[Serialize]
		public ArrayRef<ReportManager.ReportEntry> contextEntries;

		// Token: 0x0400674D RID: 26445
		public bool isChild;

		// Token: 0x020024A1 RID: 9377
		public struct Note
		{
			// Token: 0x0600BA7D RID: 47741 RVA: 0x003D3767 File Offset: 0x003D1967
			public Note(float value, string note)
			{
				this.value = value;
				this.note = note;
			}

			// Token: 0x0400A268 RID: 41576
			public float value;

			// Token: 0x0400A269 RID: 41577
			public string note;
		}

		// Token: 0x020024A2 RID: 9378
		public enum Order
		{
			// Token: 0x0400A26B RID: 41579
			Unordered,
			// Token: 0x0400A26C RID: 41580
			Ascending,
			// Token: 0x0400A26D RID: 41581
			Descending
		}
	}

	// Token: 0x020013A2 RID: 5026
	public class DailyReport
	{
		// Token: 0x060087C8 RID: 34760 RVA: 0x0032C9CC File Offset: 0x0032ABCC
		public DailyReport(ReportManager manager)
		{
			foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in manager.ReportGroups)
			{
				this.reportEntries.Add(new ReportManager.ReportEntry(keyValuePair.Key, this.noteStorage.GetNewNoteId(), null, false));
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x060087C9 RID: 34761 RVA: 0x0032CA50 File Offset: 0x0032AC50
		private ReportManager.NoteStorage noteStorage
		{
			get
			{
				return ReportManager.Instance.noteStorage;
			}
		}

		// Token: 0x060087CA RID: 34762 RVA: 0x0032CA5C File Offset: 0x0032AC5C
		public ReportManager.ReportEntry GetEntry(ReportManager.ReportType reportType)
		{
			for (int i = 0; i < this.reportEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = this.reportEntries[i];
				if (reportEntry.reportType == reportType)
				{
					return reportEntry;
				}
			}
			ReportManager.ReportEntry reportEntry2 = new ReportManager.ReportEntry(reportType, this.noteStorage.GetNewNoteId(), null, false);
			this.reportEntries.Add(reportEntry2);
			return reportEntry2;
		}

		// Token: 0x060087CB RID: 34763 RVA: 0x0032CAB8 File Offset: 0x0032ACB8
		public void AddData(ReportManager.ReportType reportType, float value, string note = null, string context = null)
		{
			this.GetEntry(reportType).AddData(this.noteStorage, value, note, context);
		}

		// Token: 0x0400674E RID: 26446
		[Serialize]
		public int day;

		// Token: 0x0400674F RID: 26447
		[Serialize]
		public List<ReportManager.ReportEntry> reportEntries = new List<ReportManager.ReportEntry>();
	}

	// Token: 0x020013A3 RID: 5027
	public class NoteStorage
	{
		// Token: 0x060087CC RID: 34764 RVA: 0x0032CAD0 File Offset: 0x0032ACD0
		public NoteStorage()
		{
			this.noteEntries = new ReportManager.NoteStorage.NoteEntries();
			this.stringTable = new ReportManager.NoteStorage.StringTable();
		}

		// Token: 0x060087CD RID: 34765 RVA: 0x0032CAF0 File Offset: 0x0032ACF0
		public void Add(int report_entry_id, float value, string note)
		{
			int note_id = this.stringTable.AddString(note, 6);
			this.noteEntries.Add(report_entry_id, value, note_id);
		}

		// Token: 0x060087CE RID: 34766 RVA: 0x0032CB1C File Offset: 0x0032AD1C
		public int GetNewNoteId()
		{
			int result = this.nextNoteId + 1;
			this.nextNoteId = result;
			return result;
		}

		// Token: 0x060087CF RID: 34767 RVA: 0x0032CB3A File Offset: 0x0032AD3A
		public void IterateNotes(int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
		{
			this.noteEntries.IterateNotes(this.stringTable, report_entry_id, callback);
		}

		// Token: 0x060087D0 RID: 34768 RVA: 0x0032CB4F File Offset: 0x0032AD4F
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(6);
			writer.Write(this.nextNoteId);
			this.stringTable.Serialize(writer);
			this.noteEntries.Serialize(writer);
		}

		// Token: 0x060087D1 RID: 34769 RVA: 0x0032CB7C File Offset: 0x0032AD7C
		public void Deserialize(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num < 5)
			{
				return;
			}
			this.nextNoteId = reader.ReadInt32();
			this.stringTable.Deserialize(reader, num);
			this.noteEntries.Deserialize(reader, num);
		}

		// Token: 0x04006750 RID: 26448
		public const int SERIALIZATION_VERSION = 6;

		// Token: 0x04006751 RID: 26449
		private int nextNoteId;

		// Token: 0x04006752 RID: 26450
		private ReportManager.NoteStorage.NoteEntries noteEntries;

		// Token: 0x04006753 RID: 26451
		private ReportManager.NoteStorage.StringTable stringTable;

		// Token: 0x020024A3 RID: 9379
		private class StringTable
		{
			// Token: 0x0600BA7E RID: 47742 RVA: 0x003D3778 File Offset: 0x003D1978
			public int AddString(string str, int version = 6)
			{
				int num = Hash.SDBMLower(str);
				this.strings[num] = str;
				return num;
			}

			// Token: 0x0600BA7F RID: 47743 RVA: 0x003D379C File Offset: 0x003D199C
			public string GetStringByHash(int hash)
			{
				string result = "";
				this.strings.TryGetValue(hash, out result);
				return result;
			}

			// Token: 0x0600BA80 RID: 47744 RVA: 0x003D37C0 File Offset: 0x003D19C0
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.strings.Count);
				foreach (KeyValuePair<int, string> keyValuePair in this.strings)
				{
					writer.Write(keyValuePair.Value);
				}
			}

			// Token: 0x0600BA81 RID: 47745 RVA: 0x003D382C File Offset: 0x003D1A2C
			public void Deserialize(BinaryReader reader, int version)
			{
				int num = reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					string str = reader.ReadString();
					this.AddString(str, version);
				}
			}

			// Token: 0x0400A26E RID: 41582
			private Dictionary<int, string> strings = new Dictionary<int, string>();
		}

		// Token: 0x020024A4 RID: 9380
		private class NoteEntries
		{
			// Token: 0x0600BA83 RID: 47747 RVA: 0x003D3870 File Offset: 0x003D1A70
			public void Add(int report_entry_id, float value, int note_id)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (!this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[report_entry_id] = dictionary;
				}
				ReportManager.NoteStorage.NoteEntries.NoteEntryKey noteEntryKey = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
				{
					noteHash = note_id,
					isPositive = (value > 0f)
				};
				if (dictionary.ContainsKey(noteEntryKey))
				{
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary2 = dictionary;
					ReportManager.NoteStorage.NoteEntries.NoteEntryKey key = noteEntryKey;
					dictionary2[key] += value;
					return;
				}
				dictionary[noteEntryKey] = value;
			}

			// Token: 0x0600BA84 RID: 47748 RVA: 0x003D38EC File Offset: 0x003D1AEC
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.entries.Count);
				foreach (KeyValuePair<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> keyValuePair in this.entries)
				{
					writer.Write(keyValuePair.Key);
					writer.Write(keyValuePair.Value.Count);
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair2 in keyValuePair.Value)
					{
						writer.Write(keyValuePair2.Key.noteHash);
						writer.Write(keyValuePair2.Key.isPositive);
						writer.WriteSingleFast(keyValuePair2.Value);
					}
				}
			}

			// Token: 0x0600BA85 RID: 47749 RVA: 0x003D39DC File Offset: 0x003D1BDC
			public void Deserialize(BinaryReader reader, int version)
			{
				if (version < 6)
				{
					OldNoteEntriesV5 oldNoteEntriesV = new OldNoteEntriesV5();
					oldNoteEntriesV.Deserialize(reader);
					foreach (OldNoteEntriesV5.NoteStorageBlock noteStorageBlock in oldNoteEntriesV.storageBlocks)
					{
						for (int i = 0; i < noteStorageBlock.entryCount; i++)
						{
							OldNoteEntriesV5.NoteEntry noteEntry = noteStorageBlock.entries.structs[i];
							this.Add(noteEntry.reportEntryId, noteEntry.value, noteEntry.noteHash);
						}
					}
					return;
				}
				int num = reader.ReadInt32();
				this.entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>(num);
				for (int j = 0; j < num; j++)
				{
					int key = reader.ReadInt32();
					int num2 = reader.ReadInt32();
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(num2, ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[key] = dictionary;
					for (int k = 0; k < num2; k++)
					{
						ReportManager.NoteStorage.NoteEntries.NoteEntryKey key2 = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
						{
							noteHash = reader.ReadInt32(),
							isPositive = reader.ReadBoolean()
						};
						dictionary[key2] = reader.ReadSingle();
					}
				}
			}

			// Token: 0x0600BA86 RID: 47750 RVA: 0x003D3B10 File Offset: 0x003D1D10
			public void IterateNotes(ReportManager.NoteStorage.StringTable string_table, int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair in dictionary)
					{
						string stringByHash = string_table.GetStringByHash(keyValuePair.Key.noteHash);
						ReportManager.ReportEntry.Note obj = new ReportManager.ReportEntry.Note(keyValuePair.Value, stringByHash);
						callback(obj);
					}
				}
			}

			// Token: 0x0400A26F RID: 41583
			private static ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer sKeyComparer = new ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer();

			// Token: 0x0400A270 RID: 41584
			private Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>();

			// Token: 0x02003531 RID: 13617
			public struct NoteEntryKey
			{
				// Token: 0x0400D7A1 RID: 55201
				public int noteHash;

				// Token: 0x0400D7A2 RID: 55202
				public bool isPositive;
			}

			// Token: 0x02003532 RID: 13618
			public class NoteEntryKeyComparer : IEqualityComparer<ReportManager.NoteStorage.NoteEntries.NoteEntryKey>
			{
				// Token: 0x0600DF34 RID: 57140 RVA: 0x004322A7 File Offset: 0x004304A7
				public bool Equals(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a, ReportManager.NoteStorage.NoteEntries.NoteEntryKey b)
				{
					return a.noteHash == b.noteHash && a.isPositive == b.isPositive;
				}

				// Token: 0x0600DF35 RID: 57141 RVA: 0x004322C7 File Offset: 0x004304C7
				public int GetHashCode(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a)
				{
					return a.noteHash * (a.isPositive ? 1 : -1);
				}
			}
		}
	}
}
