using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008B3 RID: 2227
public class FetchList2 : IFetchList
{
	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06003E36 RID: 15926 RVA: 0x00158604 File Offset: 0x00156804
	// (set) Token: 0x06003E37 RID: 15927 RVA: 0x0015860C File Offset: 0x0015680C
	public bool ShowStatusItem
	{
		get
		{
			return this.bShowStatusItem;
		}
		set
		{
			this.bShowStatusItem = value;
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06003E38 RID: 15928 RVA: 0x00158615 File Offset: 0x00156815
	public bool IsComplete
	{
		get
		{
			return this.FetchOrders.Count == 0;
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06003E39 RID: 15929 RVA: 0x00158628 File Offset: 0x00156828
	public bool InProgress
	{
		get
		{
			if (this.FetchOrders.Count < 0)
			{
				return false;
			}
			bool result = false;
			using (List<FetchOrder2>.Enumerator enumerator = this.FetchOrders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06003E3A RID: 15930 RVA: 0x00158694 File Offset: 0x00156894
	// (set) Token: 0x06003E3B RID: 15931 RVA: 0x0015869C File Offset: 0x0015689C
	public Storage Destination { get; private set; }

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06003E3C RID: 15932 RVA: 0x001586A5 File Offset: 0x001568A5
	// (set) Token: 0x06003E3D RID: 15933 RVA: 0x001586AD File Offset: 0x001568AD
	public int PriorityMod { get; private set; }

	// Token: 0x06003E3E RID: 15934 RVA: 0x001586B8 File Offset: 0x001568B8
	public FetchList2(Storage destination, ChoreType chore_type)
	{
		this.Destination = destination;
		this.choreType = chore_type;
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x00158724 File Offset: 0x00156924
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			this.FetchOrders[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x06003E40 RID: 15936 RVA: 0x00158768 File Offset: 0x00156968
	public void Add(HashSet<Tag> tags, Tag requiredTag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, requiredTag, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x001587F8 File Offset: 0x001569F8
	public void Add(HashSet<Tag> tags, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x0015888C File Offset: 0x00156A8C
	public void Add(Tag tag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		if (!this.MinimumAmount.ContainsKey(tag))
		{
			this.MinimumAmount[tag] = amount;
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, new HashSet<Tag>
		{
			tag
		}, FetchChore.MatchCriteria.MatchTags, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x001588F0 File Offset: 0x00156AF0
	public float GetMinimumAmount(Tag tag)
	{
		float result = 0f;
		this.MinimumAmount.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x00158913 File Offset: 0x00156B13
	private void OnFetchOrderComplete(FetchOrder2 fetch_order, Pickupable fetched_item)
	{
		this.FetchOrders.Remove(fetch_order);
		if (this.FetchOrders.Count == 0)
		{
			if (this.OnComplete != null)
			{
				this.OnComplete();
			}
			FetchListStatusItemUpdater.instance.RemoveFetchList(this);
			this.ClearStatus();
		}
	}

	// Token: 0x06003E45 RID: 15941 RVA: 0x00158954 File Offset: 0x00156B54
	public void Cancel(string reason)
	{
		FetchListStatusItemUpdater.instance.RemoveFetchList(this);
		this.ClearStatus();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Cancel(reason);
		}
	}

	// Token: 0x06003E46 RID: 15942 RVA: 0x001589B8 File Offset: 0x00156BB8
	public void UpdateRemaining()
	{
		this.Remaining.Clear();
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			FetchOrder2 fetchOrder = this.FetchOrders[i];
			foreach (Tag key in fetchOrder.Tags)
			{
				float num = 0f;
				this.Remaining.TryGetValue(key, out num);
				this.Remaining[key] = num + fetchOrder.AmountWaitingToFetch();
			}
		}
	}

	// Token: 0x06003E47 RID: 15943 RVA: 0x00158A60 File Offset: 0x00156C60
	public Dictionary<Tag, float> GetRemaining()
	{
		return this.Remaining;
	}

	// Token: 0x06003E48 RID: 15944 RVA: 0x00158A68 File Offset: 0x00156C68
	public Dictionary<Tag, float> GetRemainingMinimum()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			foreach (Tag key in fetchOrder.Tags)
			{
				dictionary[key] = this.MinimumAmount[key];
			}
		}
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					if (dictionary.ContainsKey(kprefabID.PrefabTag))
					{
						dictionary[kprefabID.PrefabTag] = Math.Max(dictionary[kprefabID.PrefabTag] - component.TotalAmount, 0f);
					}
					foreach (Tag key2 in kprefabID.Tags)
					{
						if (dictionary.ContainsKey(key2))
						{
							dictionary[key2] = Math.Max(dictionary[key2] - component.TotalAmount, 0f);
						}
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06003E49 RID: 15945 RVA: 0x00158C20 File Offset: 0x00156E20
	public void Suspend(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Suspend(reason);
		}
	}

	// Token: 0x06003E4A RID: 15946 RVA: 0x00158C74 File Offset: 0x00156E74
	public void Resume(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Resume(reason);
		}
	}

	// Token: 0x06003E4B RID: 15947 RVA: 0x00158CC8 File Offset: 0x00156EC8
	public void Submit(System.Action on_complete, bool check_storage_contents)
	{
		this.OnComplete = on_complete;
		foreach (FetchOrder2 fetchOrder in this.FetchOrders.GetRange(0, this.FetchOrders.Count))
		{
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchOrderComplete), check_storage_contents, null);
		}
		if (!this.IsComplete && this.ShowStatusItem)
		{
			FetchListStatusItemUpdater.instance.AddFetchList(this);
		}
	}

	// Token: 0x06003E4C RID: 15948 RVA: 0x00158D5C File Offset: 0x00156F5C
	private void ClearStatus()
	{
		if (this.Destination != null)
		{
			KSelectable component = this.Destination.GetComponent<KSelectable>();
			if (component != null)
			{
				this.waitingForMaterialsHandle = component.RemoveStatusItem(this.waitingForMaterialsHandle, false);
				this.materialsUnavailableHandle = component.RemoveStatusItem(this.materialsUnavailableHandle, false);
				this.materialsUnavailableForRefillHandle = component.RemoveStatusItem(this.materialsUnavailableForRefillHandle, false);
			}
		}
	}

	// Token: 0x06003E4D RID: 15949 RVA: 0x00158DC8 File Offset: 0x00156FC8
	public void UpdateStatusItem(MaterialsStatusItem status_item, ref Guid handle, bool should_add)
	{
		bool flag = handle != Guid.Empty;
		if (should_add != flag)
		{
			if (should_add)
			{
				KSelectable component = this.Destination.GetComponent<KSelectable>();
				if (component != null)
				{
					handle = component.AddStatusItem(status_item, this);
					GameScheduler.Instance.Schedule("Digging Tutorial", 2f, delegate(object obj)
					{
						Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging, true);
					}, null, null);
					return;
				}
			}
			else
			{
				KSelectable component2 = this.Destination.GetComponent<KSelectable>();
				if (component2 != null)
				{
					handle = component2.RemoveStatusItem(handle, false);
				}
			}
		}
	}

	// Token: 0x0400264E RID: 9806
	private System.Action OnComplete;

	// Token: 0x04002651 RID: 9809
	private ChoreType choreType;

	// Token: 0x04002652 RID: 9810
	public Guid waitingForMaterialsHandle = Guid.Empty;

	// Token: 0x04002653 RID: 9811
	public Guid materialsUnavailableForRefillHandle = Guid.Empty;

	// Token: 0x04002654 RID: 9812
	public Guid materialsUnavailableHandle = Guid.Empty;

	// Token: 0x04002655 RID: 9813
	public Dictionary<Tag, float> MinimumAmount = new Dictionary<Tag, float>();

	// Token: 0x04002656 RID: 9814
	public List<FetchOrder2> FetchOrders = new List<FetchOrder2>();

	// Token: 0x04002657 RID: 9815
	private Dictionary<Tag, float> Remaining = new Dictionary<Tag, float>();

	// Token: 0x04002658 RID: 9816
	private bool bShowStatusItem = true;
}
