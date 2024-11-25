using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000766 RID: 1894
[AddComponentMenu("KMonoBehaviour/Workable/SingleEntityReceptacle")]
public class SingleEntityReceptacle : Workable, IRender1000ms
{
	// Token: 0x17000358 RID: 856
	// (get) Token: 0x060032D9 RID: 13017 RVA: 0x0011787F File Offset: 0x00115A7F
	public FetchChore GetActiveRequest
	{
		get
		{
			return this.fetchChore;
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x060032DA RID: 13018 RVA: 0x00117887 File Offset: 0x00115A87
	// (set) Token: 0x060032DB RID: 13019 RVA: 0x001178AE File Offset: 0x00115AAE
	protected GameObject occupyingObject
	{
		get
		{
			if (this.occupyObjectRef.Get() != null)
			{
				return this.occupyObjectRef.Get().gameObject;
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				this.occupyObjectRef.Set(null);
				return;
			}
			this.occupyObjectRef.Set(value.GetComponent<KSelectable>());
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x060032DC RID: 13020 RVA: 0x001178D7 File Offset: 0x00115AD7
	public GameObject Occupant
	{
		get
		{
			return this.occupyingObject;
		}
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x060032DD RID: 13021 RVA: 0x001178DF File Offset: 0x00115ADF
	public IReadOnlyList<Tag> possibleDepositObjectTags
	{
		get
		{
			return this.possibleDepositTagsList;
		}
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x001178E7 File Offset: 0x00115AE7
	public bool HasDepositTag(Tag tag)
	{
		return this.possibleDepositTagsList.Contains(tag);
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x001178F8 File Offset: 0x00115AF8
	public bool IsValidEntity(GameObject candidate)
	{
		KPrefabID component = candidate.GetComponent<KPrefabID>();
		if (!SaveLoader.Instance.IsDlcListActiveForCurrentSave(component.requiredDlcIds))
		{
			return false;
		}
		IReceptacleDirection component2 = candidate.GetComponent<IReceptacleDirection>();
		bool flag = this.rotatable != null || component2 == null || component2.Direction == this.Direction;
		int num = 0;
		while (flag && num < this.additionalCriteria.Count)
		{
			flag = this.additionalCriteria[num](candidate);
			num++;
		}
		return flag;
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x060032E0 RID: 13024 RVA: 0x00117977 File Offset: 0x00115B77
	public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x0011797F File Offset: 0x00115B7F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060032E2 RID: 13026 RVA: 0x00117988 File Offset: 0x00115B88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.occupyingObject != null)
		{
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		this.UpdateStatusItem();
		if (this.occupyingObject == null && !this.requestedEntityTag.IsValid)
		{
			this.requestedEntityAdditionalFilterTag = null;
		}
		if (this.occupyingObject == null && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
		base.Subscribe<SingleEntityReceptacle>(-592767678, SingleEntityReceptacle.OnOperationalChangedDelegate);
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x00117A20 File Offset: 0x00115C20
	public void AddDepositTag(Tag t)
	{
		this.possibleDepositTagsList.Add(t);
	}

	// Token: 0x060032E4 RID: 13028 RVA: 0x00117A2E File Offset: 0x00115C2E
	public void AddAdditionalCriteria(Func<GameObject, bool> criteria)
	{
		this.additionalCriteria.Add(criteria);
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x00117A3C File Offset: 0x00115C3C
	public void SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection d)
	{
		this.direction = d;
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x00117A45 File Offset: 0x00115C45
	public virtual void SetPreview(Tag entityTag, bool solid = false)
	{
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x00117A47 File Offset: 0x00115C47
	public virtual void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		this.requestedEntityTag = entityTag;
		this.requestedEntityAdditionalFilterTag = additionalFilterTag;
		this.CreateFetchChore(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		this.SetPreview(entityTag, true);
		this.UpdateStatusItem();
	}

	// Token: 0x060032E8 RID: 13032 RVA: 0x00117A77 File Offset: 0x00115C77
	public void Render1000ms(float dt)
	{
		this.UpdateStatusItem();
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x00117A80 File Offset: 0x00115C80
	protected virtual void UpdateStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.Occupant != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, null, null);
			return;
		}
		if (this.fetchChore == null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNeed, null);
			return;
		}
		bool flag = this.fetchChore.fetcher != null;
		WorldContainer myWorld = this.GetMyWorld();
		if (!flag && myWorld != null)
		{
			foreach (Tag tag in this.fetchChore.tags)
			{
				if (myWorld.worldInventory.GetTotalAmount(tag, true) > 0f)
				{
					if (myWorld.worldInventory.GetTotalAmount(this.requestedEntityAdditionalFilterTag, true) > 0f || this.requestedEntityAdditionalFilterTag == Tag.Invalid)
					{
						flag = true;
						break;
					}
					break;
				}
			}
		}
		if (flag)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemAwaitingDelivery, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNoneAvailable, null);
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x00117BD4 File Offset: 0x00115DD4
	protected void CreateFetchChore(Tag entityTag, Tag additionalRequiredTag)
	{
		if (this.fetchChore == null && entityTag.IsValid && entityTag != GameTags.Empty)
		{
			this.fetchChore = new FetchChore(this.choreType, this.storage, 1f, new HashSet<Tag>
			{
				entityTag
			}, FetchChore.MatchCriteria.MatchID, (additionalRequiredTag.IsValid && additionalRequiredTag != GameTags.Empty) ? additionalRequiredTag : Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, Operational.State.Functional, 0);
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, 1f, base.gameObject.GetMyWorldId());
			this.UpdateStatusItem();
		}
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x00117C9A File Offset: 0x00115E9A
	public virtual void OrderRemoveOccupant()
	{
		this.ClearOccupant();
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x00117CA4 File Offset: 0x00115EA4
	protected virtual void ClearOccupant()
	{
		if (this.occupyingObject)
		{
			this.UnsubscribeFromOccupant();
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.occupyingObject = null;
		this.UpdateActive();
		this.UpdateStatusItem();
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x00117D00 File Offset: 0x00115F00
	public void CancelActiveRequest()
	{
		if (this.fetchChore != null)
		{
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
			this.fetchChore.Cancel("User canceled");
			this.fetchChore = null;
		}
		this.requestedEntityTag = Tag.Invalid;
		this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		this.UpdateStatusItem();
		this.SetPreview(Tag.Invalid, false);
	}

	// Token: 0x060032EE RID: 13038 RVA: 0x00117D70 File Offset: 0x00115F70
	private void OnOccupantDestroyed(object data)
	{
		this.occupyingObject = null;
		this.ClearOccupant();
		if (this.autoReplaceEntity && this.requestedEntityTag.IsValid && this.requestedEntityTag != GameTags.Empty)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
	}

	// Token: 0x060032EF RID: 13039 RVA: 0x00117DC3 File Offset: 0x00115FC3
	protected virtual void SubscribeToOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Subscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x060032F0 RID: 13040 RVA: 0x00117DF1 File Offset: 0x00115FF1
	protected virtual void UnsubscribeFromOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Unsubscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x060032F1 RID: 13041 RVA: 0x00117E20 File Offset: 0x00116020
	private void OnFetchComplete(Chore chore)
	{
		if (this.fetchChore == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore null", new object[]
			{
				base.gameObject
			});
			return;
		}
		if (this.fetchChore.fetchTarget == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore.fetchTarget null", new object[]
			{
				base.gameObject
			});
			return;
		}
		this.OnDepositObject(this.fetchChore.fetchTarget.gameObject);
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x00117E9E File Offset: 0x0011609E
	public void ForceDeposit(GameObject depositedObject)
	{
		if (this.occupyingObject != null)
		{
			this.ClearOccupant();
		}
		this.OnDepositObject(depositedObject);
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x00117EBC File Offset: 0x001160BC
	private void OnDepositObject(GameObject depositedObject)
	{
		this.SetPreview(Tag.Invalid, false);
		MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
		KBatchedAnimController component = depositedObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		this.occupyingObject = this.SpawnOccupyingObject(depositedObject);
		if (this.occupyingObject != null)
		{
			this.ConfigureOccupyingObject(this.occupyingObject);
			this.occupyingObject.SetActive(true);
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		else
		{
			global::Debug.LogWarning(base.gameObject.name + " EntityReceptacle did not spawn occupying entity.");
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("receptacle filled");
			this.fetchChore = null;
		}
		if (!this.autoReplaceEntity)
		{
			this.requestedEntityTag = Tag.Invalid;
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		}
		this.UpdateActive();
		this.UpdateStatusItem();
		if (this.destroyEntityOnDeposit)
		{
			Util.KDestroyGameObject(depositedObject);
		}
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x00117FCE File Offset: 0x001161CE
	protected virtual GameObject SpawnOccupyingObject(GameObject depositedEntity)
	{
		return depositedEntity;
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x00117FD1 File Offset: 0x001161D1
	protected virtual void ConfigureOccupyingObject(GameObject source)
	{
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x00117FD4 File Offset: 0x001161D4
	protected virtual void PositionOccupyingObject()
	{
		if (this.rotatable != null)
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition));
		}
		else
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.occupyingObjectRelativePosition);
		}
		KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x0011806C File Offset: 0x0011626C
	protected void UpdateActive()
	{
		if (this.Equals(null) || this == null || base.gameObject.Equals(null) || base.gameObject == null)
		{
			return;
		}
		if (this.operational != null)
		{
			this.operational.SetActive(this.operational.IsOperational && this.occupyingObject != null, false);
		}
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x001180DE File Offset: 0x001162DE
	protected override void OnCleanUp()
	{
		this.CancelActiveRequest();
		this.UnsubscribeFromOccupant();
		base.OnCleanUp();
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x001180F2 File Offset: 0x001162F2
	private void OnOperationalChanged(object data)
	{
		this.UpdateActive();
		if (this.occupyingObject)
		{
			this.occupyingObject.Trigger(this.operational.IsOperational ? 1628751838 : 960378201, null);
		}
	}

	// Token: 0x04001E0A RID: 7690
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04001E0B RID: 7691
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04001E0C RID: 7692
	[MyCmpGet]
	public Rotatable rotatable;

	// Token: 0x04001E0D RID: 7693
	protected FetchChore fetchChore;

	// Token: 0x04001E0E RID: 7694
	public ChoreType choreType = Db.Get().ChoreTypes.Fetch;

	// Token: 0x04001E0F RID: 7695
	[Serialize]
	public bool autoReplaceEntity;

	// Token: 0x04001E10 RID: 7696
	[Serialize]
	public Tag requestedEntityTag;

	// Token: 0x04001E11 RID: 7697
	[Serialize]
	public Tag requestedEntityAdditionalFilterTag;

	// Token: 0x04001E12 RID: 7698
	[Serialize]
	protected Ref<KSelectable> occupyObjectRef = new Ref<KSelectable>();

	// Token: 0x04001E13 RID: 7699
	[SerializeField]
	private List<Tag> possibleDepositTagsList = new List<Tag>();

	// Token: 0x04001E14 RID: 7700
	[SerializeField]
	private List<Func<GameObject, bool>> additionalCriteria = new List<Func<GameObject, bool>>();

	// Token: 0x04001E15 RID: 7701
	[SerializeField]
	protected bool destroyEntityOnDeposit;

	// Token: 0x04001E16 RID: 7702
	[SerializeField]
	protected SingleEntityReceptacle.ReceptacleDirection direction;

	// Token: 0x04001E17 RID: 7703
	public Vector3 occupyingObjectRelativePosition = new Vector3(0f, 1f, 3f);

	// Token: 0x04001E18 RID: 7704
	protected StatusItem statusItemAwaitingDelivery;

	// Token: 0x04001E19 RID: 7705
	protected StatusItem statusItemNeed;

	// Token: 0x04001E1A RID: 7706
	protected StatusItem statusItemNoneAvailable;

	// Token: 0x04001E1B RID: 7707
	private static readonly EventSystem.IntraObjectHandler<SingleEntityReceptacle> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SingleEntityReceptacle>(delegate(SingleEntityReceptacle component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x020015F1 RID: 5617
	public enum ReceptacleDirection
	{
		// Token: 0x04006E3F RID: 28223
		Top,
		// Token: 0x04006E40 RID: 28224
		Side,
		// Token: 0x04006E41 RID: 28225
		Bottom
	}
}
