using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000601 RID: 1537
public class RoomType : Resource
{
	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060025B9 RID: 9657 RVA: 0x000D20FC File Offset: 0x000D02FC
	// (set) Token: 0x060025BA RID: 9658 RVA: 0x000D2104 File Offset: 0x000D0304
	public string tooltip { get; private set; }

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060025BB RID: 9659 RVA: 0x000D210D File Offset: 0x000D030D
	// (set) Token: 0x060025BC RID: 9660 RVA: 0x000D2115 File Offset: 0x000D0315
	public string description { get; set; }

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x060025BD RID: 9661 RVA: 0x000D211E File Offset: 0x000D031E
	// (set) Token: 0x060025BE RID: 9662 RVA: 0x000D2126 File Offset: 0x000D0326
	public string effect { get; private set; }

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x060025BF RID: 9663 RVA: 0x000D212F File Offset: 0x000D032F
	// (set) Token: 0x060025C0 RID: 9664 RVA: 0x000D2137 File Offset: 0x000D0337
	public RoomConstraints.Constraint primary_constraint { get; private set; }

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060025C1 RID: 9665 RVA: 0x000D2140 File Offset: 0x000D0340
	// (set) Token: 0x060025C2 RID: 9666 RVA: 0x000D2148 File Offset: 0x000D0348
	public RoomConstraints.Constraint[] additional_constraints { get; private set; }

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060025C3 RID: 9667 RVA: 0x000D2151 File Offset: 0x000D0351
	// (set) Token: 0x060025C4 RID: 9668 RVA: 0x000D2159 File Offset: 0x000D0359
	public int priority { get; private set; }

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060025C5 RID: 9669 RVA: 0x000D2162 File Offset: 0x000D0362
	// (set) Token: 0x060025C6 RID: 9670 RVA: 0x000D216A File Offset: 0x000D036A
	public bool single_assignee { get; private set; }

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060025C7 RID: 9671 RVA: 0x000D2173 File Offset: 0x000D0373
	// (set) Token: 0x060025C8 RID: 9672 RVA: 0x000D217B File Offset: 0x000D037B
	public RoomDetails.Detail[] display_details { get; private set; }

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060025C9 RID: 9673 RVA: 0x000D2184 File Offset: 0x000D0384
	// (set) Token: 0x060025CA RID: 9674 RVA: 0x000D218C File Offset: 0x000D038C
	public bool priority_building_use { get; private set; }

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x060025CB RID: 9675 RVA: 0x000D2195 File Offset: 0x000D0395
	// (set) Token: 0x060025CC RID: 9676 RVA: 0x000D219D File Offset: 0x000D039D
	public RoomTypeCategory category { get; private set; }

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x060025CD RID: 9677 RVA: 0x000D21A6 File Offset: 0x000D03A6
	// (set) Token: 0x060025CE RID: 9678 RVA: 0x000D21AE File Offset: 0x000D03AE
	public RoomType[] upgrade_paths { get; private set; }

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x060025CF RID: 9679 RVA: 0x000D21B7 File Offset: 0x000D03B7
	// (set) Token: 0x060025D0 RID: 9680 RVA: 0x000D21BF File Offset: 0x000D03BF
	public string[] effects { get; private set; }

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x060025D1 RID: 9681 RVA: 0x000D21C8 File Offset: 0x000D03C8
	// (set) Token: 0x060025D2 RID: 9682 RVA: 0x000D21D0 File Offset: 0x000D03D0
	public int sortKey { get; private set; }

	// Token: 0x060025D3 RID: 9683 RVA: 0x000D21DC File Offset: 0x000D03DC
	public RoomType(string id, string name, string description, string tooltip, string effect, RoomTypeCategory category, RoomConstraints.Constraint primary_constraint, RoomConstraints.Constraint[] additional_constraints, RoomDetails.Detail[] display_details, int priority = 0, RoomType[] upgrade_paths = null, bool single_assignee = false, bool priority_building_use = false, string[] effects = null, int sortKey = 0) : base(id, name)
	{
		this.tooltip = tooltip;
		this.description = description;
		this.effect = effect;
		this.category = category;
		this.primary_constraint = primary_constraint;
		this.additional_constraints = additional_constraints;
		this.display_details = display_details;
		this.priority = priority;
		this.upgrade_paths = upgrade_paths;
		this.single_assignee = single_assignee;
		this.priority_building_use = priority_building_use;
		this.effects = effects;
		this.sortKey = sortKey;
		if (this.upgrade_paths != null)
		{
			RoomType[] upgrade_paths2 = this.upgrade_paths;
			for (int i = 0; i < upgrade_paths2.Length; i++)
			{
				Debug.Assert(upgrade_paths2[i] != null, name + " has a null upgrade path. Maybe it wasn't initialized yet.");
			}
		}
	}

	// Token: 0x060025D4 RID: 9684 RVA: 0x000D228C File Offset: 0x000D048C
	public RoomType.RoomIdentificationResult isSatisfactory(Room candidate_room)
	{
		if (this.primary_constraint != null && !this.primary_constraint.isSatisfied(candidate_room))
		{
			return RoomType.RoomIdentificationResult.primary_unsatisfied;
		}
		if (this.additional_constraints != null)
		{
			RoomConstraints.Constraint[] additional_constraints = this.additional_constraints;
			for (int i = 0; i < additional_constraints.Length; i++)
			{
				if (!additional_constraints[i].isSatisfied(candidate_room))
				{
					return RoomType.RoomIdentificationResult.primary_satisfied;
				}
			}
		}
		return RoomType.RoomIdentificationResult.all_satisfied;
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x000D22DC File Offset: 0x000D04DC
	public string GetCriteriaString()
	{
		string text = string.Concat(new string[]
		{
			"<b>",
			this.Name,
			"</b>\n",
			this.tooltip,
			"\n\n",
			ROOMS.CRITERIA.HEADER
		});
		if (this == Db.Get().RoomTypes.Neutral)
		{
			text = text + "\n    • " + ROOMS.CRITERIA.NEUTRAL_TYPE;
		}
		text += ((this.primary_constraint == null) ? "" : ("\n    • " + this.primary_constraint.name));
		if (this.additional_constraints != null)
		{
			foreach (RoomConstraints.Constraint constraint in this.additional_constraints)
			{
				text = text + "\n    • " + constraint.name;
			}
		}
		return text;
	}

	// Token: 0x060025D6 RID: 9686 RVA: 0x000D23B4 File Offset: 0x000D05B4
	public string GetRoomEffectsString()
	{
		if (this.effects != null && this.effects.Length != 0)
		{
			string text = ROOMS.EFFECTS.HEADER;
			foreach (string id in this.effects)
			{
				Effect effect = Db.Get().effects.Get(id);
				text += Effect.CreateTooltip(effect, false, "\n    • ", false);
			}
			return text;
		}
		return null;
	}

	// Token: 0x060025D7 RID: 9687 RVA: 0x000D2420 File Offset: 0x000D0620
	public void TriggerRoomEffects(KPrefabID triggerer, Effects target)
	{
		if (this.primary_constraint == null)
		{
			return;
		}
		if (triggerer == null)
		{
			return;
		}
		if (this.effects == null)
		{
			return;
		}
		if (this.primary_constraint.building_criteria(triggerer))
		{
			foreach (string effect_id in this.effects)
			{
				target.Add(effect_id, true);
			}
		}
	}

	// Token: 0x020013F1 RID: 5105
	public enum RoomIdentificationResult
	{
		// Token: 0x04006873 RID: 26739
		all_satisfied,
		// Token: 0x04006874 RID: 26740
		primary_satisfied,
		// Token: 0x04006875 RID: 26741
		primary_unsatisfied
	}
}
