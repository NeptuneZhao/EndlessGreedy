using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D1E RID: 3358
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenEntryRow")]
public class ReportScreenEntryRow : KMonoBehaviour
{
	// Token: 0x060068EC RID: 26860 RVA: 0x00274870 File Offset: 0x00272A70
	private List<ReportManager.ReportEntry.Note> Sort(List<ReportManager.ReportEntry.Note> notes, ReportManager.ReportEntry.Order order)
	{
		if (order == ReportManager.ReportEntry.Order.Ascending)
		{
			notes.Sort((ReportManager.ReportEntry.Note x, ReportManager.ReportEntry.Note y) => x.value.CompareTo(y.value));
		}
		else if (order == ReportManager.ReportEntry.Order.Descending)
		{
			notes.Sort((ReportManager.ReportEntry.Note x, ReportManager.ReportEntry.Note y) => y.value.CompareTo(x.value));
		}
		return notes;
	}

	// Token: 0x060068ED RID: 26861 RVA: 0x002748D2 File Offset: 0x00272AD2
	public static void DestroyStatics()
	{
		ReportScreenEntryRow.notes = null;
	}

	// Token: 0x060068EE RID: 26862 RVA: 0x002748DC File Offset: 0x00272ADC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.added.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnPositiveNoteTooltip);
		this.removed.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNegativeNoteTooltip);
		this.net.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNetNoteTooltip);
		this.name.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNetNoteTooltip);
	}

	// Token: 0x060068EF RID: 26863 RVA: 0x00274960 File Offset: 0x00272B60
	private string OnNoteTooltip(float total_accumulation, string tooltip_text, ReportManager.ReportEntry.Order order, ReportManager.FormattingFn format_fn, Func<ReportManager.ReportEntry.Note, bool> is_note_applicable_cb, ReportManager.GroupFormattingFn group_format_fn = null)
	{
		ReportScreenEntryRow.notes.Clear();
		this.entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (is_note_applicable_cb(note))
			{
				ReportScreenEntryRow.notes.Add(note);
			}
		});
		string text = "";
		float num = 0f;
		if (this.entry.contextEntries.Count > 0)
		{
			num = (float)this.entry.contextEntries.Count;
		}
		else
		{
			num = (float)ReportScreenEntryRow.notes.Count;
		}
		num = Mathf.Max(num, 1f);
		foreach (ReportManager.ReportEntry.Note note2 in this.Sort(ReportScreenEntryRow.notes, this.reportGroup.posNoteOrder))
		{
			string arg = format_fn(note2.value);
			if (this.toggle.gameObject.activeInHierarchy && group_format_fn != null)
			{
				arg = group_format_fn(note2.value, num);
			}
			text = string.Format(UI.ENDOFDAYREPORT.NOTES.NOTE_ENTRY_LINE_ITEM, text, note2.note, arg);
		}
		string arg2 = format_fn(total_accumulation);
		if (this.entry.context != null)
		{
			return string.Format(tooltip_text + "\n" + text, arg2, this.entry.context);
		}
		if (group_format_fn != null)
		{
			arg2 = group_format_fn(total_accumulation, num);
			return string.Format(tooltip_text + "\n" + text, arg2, UI.ENDOFDAYREPORT.MY_COLONY);
		}
		return string.Format(tooltip_text + "\n" + text, arg2, UI.ENDOFDAYREPORT.MY_COLONY);
	}

	// Token: 0x060068F0 RID: 26864 RVA: 0x00274AFC File Offset: 0x00272CFC
	private string OnNegativeNoteTooltip()
	{
		return this.OnNoteTooltip(-this.entry.Negative, this.reportGroup.negativeTooltip, this.reportGroup.negNoteOrder, this.reportGroup.formatfn, (ReportManager.ReportEntry.Note note) => this.IsNegativeNote(note), this.reportGroup.groupFormatfn);
	}

	// Token: 0x060068F1 RID: 26865 RVA: 0x00274B54 File Offset: 0x00272D54
	private string OnPositiveNoteTooltip()
	{
		return this.OnNoteTooltip(this.entry.Positive, this.reportGroup.positiveTooltip, this.reportGroup.posNoteOrder, this.reportGroup.formatfn, (ReportManager.ReportEntry.Note note) => this.IsPositiveNote(note), this.reportGroup.groupFormatfn);
	}

	// Token: 0x060068F2 RID: 26866 RVA: 0x00274BAA File Offset: 0x00272DAA
	private string OnNetNoteTooltip()
	{
		if (this.entry.Net > 0f)
		{
			return this.OnPositiveNoteTooltip();
		}
		return this.OnNegativeNoteTooltip();
	}

	// Token: 0x060068F3 RID: 26867 RVA: 0x00274BCB File Offset: 0x00272DCB
	private bool IsPositiveNote(ReportManager.ReportEntry.Note note)
	{
		return note.value > 0f;
	}

	// Token: 0x060068F4 RID: 26868 RVA: 0x00274BDD File Offset: 0x00272DDD
	private bool IsNegativeNote(ReportManager.ReportEntry.Note note)
	{
		return note.value < 0f;
	}

	// Token: 0x060068F5 RID: 26869 RVA: 0x00274BF0 File Offset: 0x00272DF0
	public void SetLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
	{
		this.entry = entry;
		this.reportGroup = reportGroup;
		ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList pos_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
		entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (this.IsPositiveNote(note))
			{
				pos_notes.Add(note);
			}
		});
		ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList neg_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
		entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (this.IsNegativeNote(note))
			{
				neg_notes.Add(note);
			}
		});
		LayoutElement component = this.name.GetComponent<LayoutElement>();
		if (entry.context == null)
		{
			component.minWidth = (component.preferredWidth = this.nameWidth);
			if (entry.HasContextEntries())
			{
				this.toggle.gameObject.SetActive(true);
				this.spacer.minWidth = this.groupSpacerWidth;
			}
			else
			{
				this.toggle.gameObject.SetActive(false);
				this.spacer.minWidth = this.groupSpacerWidth + this.toggle.GetComponent<LayoutElement>().minWidth;
			}
			this.name.text = reportGroup.stringKey;
		}
		else
		{
			this.toggle.gameObject.SetActive(false);
			this.spacer.minWidth = this.contextSpacerWidth;
			this.name.text = entry.context;
			component.minWidth = (component.preferredWidth = this.nameWidth - this.indentWidth);
			if (base.transform.GetSiblingIndex() % 2 != 0)
			{
				this.bgImage.color = this.oddRowColor;
			}
		}
		if (this.addedValue != entry.Positive)
		{
			string text = reportGroup.formatfn(entry.Positive);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num;
				if (entry.contextEntries.Count > 0)
				{
					num = (float)entry.contextEntries.Count;
				}
				else
				{
					num = (float)pos_notes.Count;
				}
				num = Mathf.Max(num, 1f);
				text = reportGroup.groupFormatfn(entry.Positive, num);
			}
			this.added.text = text;
			this.addedValue = entry.Positive;
		}
		if (this.removedValue != entry.Negative)
		{
			string text2 = reportGroup.formatfn(-entry.Negative);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num2;
				if (entry.contextEntries.Count > 0)
				{
					num2 = (float)entry.contextEntries.Count;
				}
				else
				{
					num2 = (float)neg_notes.Count;
				}
				num2 = Mathf.Max(num2, 1f);
				text2 = reportGroup.groupFormatfn(-entry.Negative, num2);
			}
			this.removed.text = text2;
			this.removedValue = entry.Negative;
		}
		if (this.netValue != entry.Net)
		{
			string text3 = (reportGroup.formatfn == null) ? entry.Net.ToString() : reportGroup.formatfn(entry.Net);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num3;
				if (entry.contextEntries.Count > 0)
				{
					num3 = (float)entry.contextEntries.Count;
				}
				else
				{
					num3 = (float)(pos_notes.Count + neg_notes.Count);
				}
				num3 = Mathf.Max(num3, 1f);
				text3 = reportGroup.groupFormatfn(entry.Net, num3);
			}
			this.net.text = text3;
			this.netValue = entry.Net;
		}
		pos_notes.Recycle();
		neg_notes.Recycle();
	}

	// Token: 0x0400470A RID: 18186
	[SerializeField]
	public new LocText name;

	// Token: 0x0400470B RID: 18187
	[SerializeField]
	public LocText added;

	// Token: 0x0400470C RID: 18188
	[SerializeField]
	public LocText removed;

	// Token: 0x0400470D RID: 18189
	[SerializeField]
	public LocText net;

	// Token: 0x0400470E RID: 18190
	private float addedValue = float.NegativeInfinity;

	// Token: 0x0400470F RID: 18191
	private float removedValue = float.NegativeInfinity;

	// Token: 0x04004710 RID: 18192
	private float netValue = float.NegativeInfinity;

	// Token: 0x04004711 RID: 18193
	[SerializeField]
	public MultiToggle toggle;

	// Token: 0x04004712 RID: 18194
	[SerializeField]
	private LayoutElement spacer;

	// Token: 0x04004713 RID: 18195
	[SerializeField]
	private Image bgImage;

	// Token: 0x04004714 RID: 18196
	public float groupSpacerWidth;

	// Token: 0x04004715 RID: 18197
	public float contextSpacerWidth;

	// Token: 0x04004716 RID: 18198
	private float nameWidth = 164f;

	// Token: 0x04004717 RID: 18199
	private float indentWidth = 6f;

	// Token: 0x04004718 RID: 18200
	[SerializeField]
	private Color oddRowColor;

	// Token: 0x04004719 RID: 18201
	private static List<ReportManager.ReportEntry.Note> notes = new List<ReportManager.ReportEntry.Note>();

	// Token: 0x0400471A RID: 18202
	private ReportManager.ReportEntry entry;

	// Token: 0x0400471B RID: 18203
	private ReportManager.ReportGroup reportGroup;
}
