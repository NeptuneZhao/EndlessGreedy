using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008B6 RID: 2230
public class FetchOrder2
{
	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06003E68 RID: 15976 RVA: 0x00159CE2 File Offset: 0x00157EE2
	// (set) Token: 0x06003E69 RID: 15977 RVA: 0x00159CEA File Offset: 0x00157EEA
	public float TotalAmount { get; set; }

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06003E6A RID: 15978 RVA: 0x00159CF3 File Offset: 0x00157EF3
	// (set) Token: 0x06003E6B RID: 15979 RVA: 0x00159CFB File Offset: 0x00157EFB
	public int PriorityMod { get; set; }

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06003E6C RID: 15980 RVA: 0x00159D04 File Offset: 0x00157F04
	// (set) Token: 0x06003E6D RID: 15981 RVA: 0x00159D0C File Offset: 0x00157F0C
	public HashSet<Tag> Tags { get; protected set; }

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06003E6E RID: 15982 RVA: 0x00159D15 File Offset: 0x00157F15
	// (set) Token: 0x06003E6F RID: 15983 RVA: 0x00159D1D File Offset: 0x00157F1D
	public FetchChore.MatchCriteria Criteria { get; protected set; }

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06003E70 RID: 15984 RVA: 0x00159D26 File Offset: 0x00157F26
	// (set) Token: 0x06003E71 RID: 15985 RVA: 0x00159D2E File Offset: 0x00157F2E
	public Tag RequiredTag { get; protected set; }

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06003E72 RID: 15986 RVA: 0x00159D37 File Offset: 0x00157F37
	// (set) Token: 0x06003E73 RID: 15987 RVA: 0x00159D3F File Offset: 0x00157F3F
	public Tag[] ForbiddenTags { get; protected set; }

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06003E74 RID: 15988 RVA: 0x00159D48 File Offset: 0x00157F48
	// (set) Token: 0x06003E75 RID: 15989 RVA: 0x00159D50 File Offset: 0x00157F50
	public Storage Destination { get; set; }

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06003E76 RID: 15990 RVA: 0x00159D59 File Offset: 0x00157F59
	// (set) Token: 0x06003E77 RID: 15991 RVA: 0x00159D61 File Offset: 0x00157F61
	private float UnfetchedAmount
	{
		get
		{
			return this._UnfetchedAmount;
		}
		set
		{
			this._UnfetchedAmount = value;
			this.Assert(this._UnfetchedAmount <= this.TotalAmount, "_UnfetchedAmount <= TotalAmount");
			this.Assert(this._UnfetchedAmount >= 0f, "_UnfetchedAmount >= 0");
		}
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x00159DA4 File Offset: 0x00157FA4
	public FetchOrder2(ChoreType chore_type, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags, Storage destination, float amount, Operational.State operationalRequirementDEPRECATED = Operational.State.None, int priorityMod = 0)
	{
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("FetchOrder2 {0} is requesting {1} {2} to {3}", new object[]
				{
					chore_type.Id,
					tags,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		this.choreType = chore_type;
		this.Tags = tags;
		this.Criteria = criteria;
		this.RequiredTag = required_tag;
		this.ForbiddenTags = forbidden_tags;
		this.Destination = destination;
		this.TotalAmount = amount;
		this.UnfetchedAmount = amount;
		this.PriorityMod = priorityMod;
		this.operationalRequirement = operationalRequirementDEPRECATED;
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06003E79 RID: 15993 RVA: 0x00159E70 File Offset: 0x00158070
	public bool InProgress
	{
		get
		{
			bool result = false;
			using (List<FetchChore>.Enumerator enumerator = this.Chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress())
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x00159ECC File Offset: 0x001580CC
	private void IssueTask()
	{
		if (this.UnfetchedAmount > 0f)
		{
			this.SetFetchTask(this.UnfetchedAmount);
			this.UnfetchedAmount = 0f;
		}
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x00159EF4 File Offset: 0x001580F4
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.Chores.Count; i++)
		{
			this.Chores[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x00159F38 File Offset: 0x00158138
	private void SetFetchTask(float amount)
	{
		FetchChore fetchChore = new FetchChore(this.choreType, this.Destination, amount, this.Tags, this.Criteria, this.RequiredTag, this.ForbiddenTags, null, true, new Action<Chore>(this.OnFetchChoreComplete), new Action<Chore>(this.OnFetchChoreBegin), new Action<Chore>(this.OnFetchChoreEnd), this.operationalRequirement, this.PriorityMod);
		fetchChore.validateRequiredTagOnTagChange = this.validateRequiredTagOnTagChange;
		this.Chores.Add(fetchChore);
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x00159FBC File Offset: 0x001581BC
	private void OnFetchChoreEnd(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		if (this.Chores.Contains(fetchChore))
		{
			this.UnfetchedAmount += fetchChore.amount;
			fetchChore.Cancel("FetchChore Redistribution");
			this.Chores.Remove(fetchChore);
			this.IssueTask();
		}
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0015A010 File Offset: 0x00158210
	private void OnFetchChoreComplete(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.Chores.Remove(fetchChore);
		if (this.Chores.Count == 0 && this.OnComplete != null)
		{
			this.OnComplete(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0015A058 File Offset: 0x00158258
	private void OnFetchChoreBegin(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.UnfetchedAmount += fetchChore.originalAmount - fetchChore.amount;
		this.IssueTask();
		if (this.OnBegin != null)
		{
			this.OnBegin(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x0015A0A8 File Offset: 0x001582A8
	public void Cancel(string reason)
	{
		while (this.Chores.Count > 0)
		{
			FetchChore fetchChore = this.Chores[0];
			fetchChore.Cancel(reason);
			this.Chores.Remove(fetchChore);
		}
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x0015A0E6 File Offset: 0x001582E6
	public void Suspend(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0015A0F2 File Offset: 0x001582F2
	public void Resume(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x0015A100 File Offset: 0x00158300
	public void Submit(Action<FetchOrder2, Pickupable> on_complete, bool check_storage_contents, Action<FetchOrder2, Pickupable> on_begin = null)
	{
		this.OnComplete = on_complete;
		this.OnBegin = on_begin;
		this.checkStorageContents = check_storage_contents;
		if (check_storage_contents)
		{
			Pickupable arg = null;
			this.UnfetchedAmount = this.GetRemaining(out arg);
			if (this.UnfetchedAmount > this.Destination.storageFullMargin)
			{
				this.IssueTask();
				return;
			}
			if (this.OnComplete != null)
			{
				this.OnComplete(this, arg);
				return;
			}
		}
		else
		{
			this.IssueTask();
		}
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x0015A16C File Offset: 0x0015836C
	public bool IsMaterialOnStorage(Storage storage, ref float amount, ref Pickupable out_item)
	{
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					foreach (Tag tag in this.Tags)
					{
						if (kprefabID.HasTag(tag))
						{
							amount = component.TotalAmount;
							out_item = component;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0015A238 File Offset: 0x00158438
	public float AmountWaitingToFetch()
	{
		if (!this.checkStorageContents)
		{
			float num = this.UnfetchedAmount;
			for (int i = 0; i < this.Chores.Count; i++)
			{
				num += this.Chores[i].AmountWaitingToFetch();
			}
			return num;
		}
		Pickupable pickupable;
		return this.GetRemaining(out pickupable);
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0015A288 File Offset: 0x00158488
	public float GetRemaining(out Pickupable out_item)
	{
		float num = this.TotalAmount;
		float num2 = 0f;
		out_item = null;
		if (this.IsMaterialOnStorage(this.Destination, ref num2, ref out_item))
		{
			num = Math.Max(num - num2, 0f);
		}
		return num;
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x0015A2C8 File Offset: 0x001584C8
	public bool IsComplete()
	{
		for (int i = 0; i < this.Chores.Count; i++)
		{
			if (!this.Chores[i].isComplete)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x0015A304 File Offset: 0x00158504
	private void Assert(bool condition, string message)
	{
		if (condition)
		{
			return;
		}
		string text = "FetchOrder error: " + message;
		if (this.Destination == null)
		{
			text += "\nDestination: None";
		}
		else
		{
			text = text + "\nDestination: " + this.Destination.name;
		}
		text = text + "\nTotal Amount: " + this.TotalAmount.ToString();
		text = text + "\nUnfetched Amount: " + this._UnfetchedAmount.ToString();
		global::Debug.LogError(text);
	}

	// Token: 0x04002663 RID: 9827
	public Action<FetchOrder2, Pickupable> OnComplete;

	// Token: 0x04002664 RID: 9828
	public Action<FetchOrder2, Pickupable> OnBegin;

	// Token: 0x04002669 RID: 9833
	public bool validateRequiredTagOnTagChange;

	// Token: 0x0400266D RID: 9837
	public List<FetchChore> Chores = new List<FetchChore>();

	// Token: 0x0400266E RID: 9838
	private ChoreType choreType;

	// Token: 0x0400266F RID: 9839
	private float _UnfetchedAmount;

	// Token: 0x04002670 RID: 9840
	private bool checkStorageContents;

	// Token: 0x04002671 RID: 9841
	private Operational.State operationalRequirement = Operational.State.None;
}
