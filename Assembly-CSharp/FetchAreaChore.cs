using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class FetchAreaChore : Chore<FetchAreaChore.StatesInstance>
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060016F9 RID: 5881 RVA: 0x0007C20E File Offset: 0x0007A40E
	public bool IsFetching
	{
		get
		{
			return base.smi.pickingup;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060016FA RID: 5882 RVA: 0x0007C21B File Offset: 0x0007A41B
	public bool IsDelivering
	{
		get
		{
			return base.smi.delivering;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060016FB RID: 5883 RVA: 0x0007C228 File Offset: 0x0007A428
	public GameObject GetFetchTarget
	{
		get
		{
			return base.smi.sm.fetchTarget.Get(base.smi);
		}
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x0007C248 File Offset: 0x0007A448
	public FetchAreaChore(Chore.Precondition.Context context) : base(context.chore.choreType, context.consumerState.consumer, context.consumerState.choreProvider, false, null, null, null, context.masterPriority.priority_class, context.masterPriority.priority_value, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new FetchAreaChore.StatesInstance(this, context);
	}

	// Token: 0x060016FD RID: 5885 RVA: 0x0007C2B0 File Offset: 0x0007A4B0
	public override void Cleanup()
	{
		base.Cleanup();
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x0007C2B8 File Offset: 0x0007A4B8
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.Begin(context);
		base.Begin(context);
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x0007C2CD File Offset: 0x0007A4CD
	protected override void End(string reason)
	{
		base.smi.End();
		base.End(reason);
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x0007C2E1 File Offset: 0x0007A4E1
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.fetchTarget.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x0007C314 File Offset: 0x0007A514
	private static bool IsPickupableStillValidForChore(Pickupable pickupable, FetchChore chore)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		if ((chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst)))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it is not or does not contain one of these tags: {1}", pickupable, string.Join<Tag>(",", chore.tags)));
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it does not have the required tag: {1}", pickupable, chore.requiredTag));
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it has the forbidden tags: {1}", pickupable, string.Join<Tag>(",", chore.forbiddenTags)));
			return false;
		}
		return pickupable.isChoreAllowedToPickup(chore.choreType);
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x0007C3F0 File Offset: 0x0007A5F0
	public static void GatherNearbyFetchChores(FetchChore root_chore, Chore.Precondition.Context context, int x, int y, int radius, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts)
	{
		ListPool<ScenePartitionerEntry, FetchAreaChore>.PooledList pooledList = ListPool<ScenePartitionerEntry, FetchAreaChore>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x - radius, y - radius, radius * 2 + 1, radius * 2 + 1, GameScenePartitioner.Instance.fetchChoreLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			(pooledList[i].obj as FetchChore).CollectChoresFromGlobalChoreProvider(context.consumerState, succeeded_contexts, null, failed_contexts, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x020011B9 RID: 4537
	public class StatesInstance : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.GameInstance
	{
		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x060080DF RID: 32991 RVA: 0x0031353F File Offset: 0x0031173F
		public Tag RootChore_RequiredTag
		{
			get
			{
				return this.rootChore.requiredTag;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x060080E0 RID: 32992 RVA: 0x0031354C File Offset: 0x0031174C
		public bool RootChore_ValidateRequiredTagOnTagChange
		{
			get
			{
				return this.rootChore.validateRequiredTagOnTagChange;
			}
		}

		// Token: 0x060080E1 RID: 32993 RVA: 0x0031355C File Offset: 0x0031175C
		public StatesInstance(FetchAreaChore master, Chore.Precondition.Context context) : base(master)
		{
			this.rootContext = context;
			this.rootChore = (context.chore as FetchChore);
		}

		// Token: 0x060080E2 RID: 32994 RVA: 0x003135C0 File Offset: 0x003117C0
		public void Begin(Chore.Precondition.Context context)
		{
			base.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
			this.chores.Clear();
			this.chores.Add(this.rootChore);
			int x;
			int y;
			Grid.CellToXY(Grid.PosToCell(this.rootChore.destination.transform.GetPosition()), out x, out y);
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList2 = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			if (this.rootChore.allowMultifetch)
			{
				FetchAreaChore.GatherNearbyFetchChores(this.rootChore, context, x, y, 3, pooledList, pooledList2);
			}
			float num = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(context.consumerState.consumer).GetTotalValue());
			Pickupable pickupable = context.data as Pickupable;
			if (pickupable == null)
			{
				global::Debug.Assert(pooledList.Count > 0, "succeeded_contexts was empty");
				FetchChore fetchChore = (FetchChore)pooledList[0].chore;
				global::Debug.Assert(fetchChore != null, "fetch_chore was null");
				DebugUtil.LogWarningArgs(new object[]
				{
					"Missing root_fetchable for FetchAreaChore",
					fetchChore.destination,
					fetchChore.tagsFirst
				});
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
			}
			global::Debug.Assert(pickupable != null, "root_fetchable was null");
			List<Pickupable> list = new List<Pickupable>();
			list.Add(pickupable);
			float num2 = pickupable.UnreservedAmount;
			float minTakeAmount = pickupable.MinTakeAmount;
			int num3 = 0;
			int num4 = 0;
			Grid.CellToXY(Grid.PosToCell(pickupable.transform.GetPosition()), out num3, out num4);
			int num5 = 9;
			num3 -= 3;
			num4 -= 3;
			Tag prefabTag = pickupable.GetComponent<KPrefabID>().PrefabTag;
			IEnumerable<object> first = GameScenePartitioner.Instance.AsyncSafeEnumerate(num3, num4, num5, num5, GameScenePartitioner.Instance.pickupablesLayer);
			IEnumerable<object> second = GameScenePartitioner.Instance.AsyncSafeEnumerate(num3, num4, num5, num5, GameScenePartitioner.Instance.storedPickupablesLayer);
			foreach (object obj in first.Concat(second))
			{
				if (num2 > num)
				{
					break;
				}
				Pickupable pickupable2 = obj as Pickupable;
				KPrefabID kprefabID = pickupable2.KPrefabID;
				if (!kprefabID.HasTag(GameTags.StoredPrivate) && !(kprefabID.PrefabTag != prefabTag) && pickupable2.UnreservedAmount > 0f && (this.rootChore.criteria != FetchChore.MatchCriteria.MatchID || this.rootChore.tags.Contains(kprefabID.PrefabTag)) && (this.rootChore.criteria != FetchChore.MatchCriteria.MatchTags || kprefabID.HasTag(this.rootChore.tagsFirst)) && (!this.rootChore.requiredTag.IsValid || kprefabID.HasTag(this.rootChore.requiredTag)) && !kprefabID.HasAnyTags(this.rootChore.forbiddenTags) && !list.Contains(pickupable2) && this.rootContext.consumerState.consumer.CanReach(pickupable2) && !kprefabID.HasTag(GameTags.MarkedForMove))
				{
					float unreservedAmount = pickupable2.UnreservedAmount;
					list.Add(pickupable2);
					num2 += unreservedAmount;
					if (list.Count >= 10)
					{
						break;
					}
				}
			}
			num2 = Mathf.Min(num, num2);
			if (minTakeAmount > 0f)
			{
				num2 -= num2 % minTakeAmount;
			}
			this.deliveries.Clear();
			float num6 = Mathf.Min(this.rootChore.originalAmount, num2);
			if (minTakeAmount > 0f)
			{
				num6 -= num6 % minTakeAmount;
			}
			this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(this.rootContext, num6, new Action<FetchChore>(this.OnFetchChoreCancelled)));
			float num7 = num6;
			int num8 = 0;
			while (num8 < pooledList.Count && num7 < num2)
			{
				Chore.Precondition.Context context2 = pooledList[num8];
				FetchChore fetchChore2 = context2.chore as FetchChore;
				if (fetchChore2 != this.rootChore && fetchChore2.overrideTarget == null && fetchChore2.driver == null && fetchChore2.tagsHash == this.rootChore.tagsHash && fetchChore2.requiredTag == this.rootChore.requiredTag && fetchChore2.forbidHash == this.rootChore.forbidHash)
				{
					num6 = Mathf.Min(fetchChore2.originalAmount, num2 - num7);
					if (minTakeAmount > 0f)
					{
						num6 -= num6 % minTakeAmount;
					}
					this.chores.Add(fetchChore2);
					this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(context2, num6, new Action<FetchChore>(this.OnFetchChoreCancelled)));
					num7 += num6;
					if (this.deliveries.Count >= 10)
					{
						break;
					}
				}
				num8++;
			}
			num7 = Mathf.Min(num7, num2);
			float num9 = num7;
			this.fetchables.Clear();
			int num10 = 0;
			while (num10 < list.Count && num9 > 0f)
			{
				Pickupable pickupable3 = list[num10];
				num9 -= pickupable3.UnreservedAmount;
				this.fetchables.Add(pickupable3);
				num10++;
			}
			this.fetchAmountRequested = num7;
			this.reservations.Clear();
			pooledList.Recycle();
			pooledList2.Recycle();
		}

		// Token: 0x060080E3 RID: 32995 RVA: 0x00313B50 File Offset: 0x00311D50
		public void End()
		{
			foreach (FetchAreaChore.StatesInstance.Delivery delivery in this.deliveries)
			{
				delivery.Cleanup();
			}
			this.deliveries.Clear();
		}

		// Token: 0x060080E4 RID: 32996 RVA: 0x00313BB0 File Offset: 0x00311DB0
		public void SetupDelivery()
		{
			if (this.deliveries.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			FetchAreaChore.StatesInstance.Delivery nextDelivery = this.deliveries[0];
			if (FetchAreaChore.StatesInstance.s_transientDeliveryTags.Contains(nextDelivery.chore.requiredTag))
			{
				nextDelivery.chore.requiredTag = Tag.Invalid;
			}
			this.deliverables.RemoveAll(delegate(Pickupable x)
			{
				if (x == null || x.TotalAmount <= 0f)
				{
					return true;
				}
				if (x.KPrefabID.HasTag(GameTags.MarkedForMove))
				{
					return true;
				}
				if (!FetchAreaChore.IsPickupableStillValidForChore(x, nextDelivery.chore))
				{
					global::Debug.LogWarning(string.Format("Removing deliverable {0} for a delivery to {1} which did not request it", x, nextDelivery.chore.destination));
					return true;
				}
				return false;
			});
			if (this.deliverables.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			base.sm.deliveryDestination.Set(nextDelivery.destination, base.smi);
			base.sm.deliveryObject.Set(this.deliverables[0], base.smi);
			if (!(nextDelivery.destination != null))
			{
				base.smi.GoTo(base.sm.delivering.deliverfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.delivering.movetostorage);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.deliveries[0].destination))
			{
				this.GoTo(base.sm.delivering.storing);
				return;
			}
			this.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x060080E5 RID: 32997 RVA: 0x00313D48 File Offset: 0x00311F48
		public void SetupFetch()
		{
			if (this.reservations.Count <= 0)
			{
				this.GoTo(base.sm.delivering.next);
				return;
			}
			this.SetFetchTarget(this.reservations[0].pickupable);
			base.sm.fetchResultTarget.Set(null, base.smi);
			base.sm.fetchAmount.Set(this.reservations[0].amount, base.smi, false);
			if (!(this.reservations[0].pickupable != null))
			{
				this.GoTo(base.sm.fetching.fetchfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.fetching.movetopickupable);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.reservations[0].pickupable))
			{
				this.GoTo(base.sm.fetching.pickup);
				return;
			}
			this.GoTo(base.sm.fetching.fetchfail);
		}

		// Token: 0x060080E6 RID: 32998 RVA: 0x00313E91 File Offset: 0x00312091
		public void SetFetchTarget(Pickupable fetching)
		{
			base.sm.fetchTarget.Set(fetching, base.smi);
			if (fetching != null)
			{
				fetching.Subscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
		}

		// Token: 0x060080E7 RID: 32999 RVA: 0x00313ECC File Offset: 0x003120CC
		public void DeliverFail()
		{
			if (this.deliveries.Count > 0)
			{
				this.deliveries[0].Cleanup();
				this.deliveries.RemoveAt(0);
			}
			this.GoTo(base.sm.delivering.next);
		}

		// Token: 0x060080E8 RID: 33000 RVA: 0x00313F20 File Offset: 0x00312120
		public void DeliverComplete()
		{
			Pickupable pickupable = base.sm.deliveryObject.Get<Pickupable>(base.smi);
			if (!(pickupable == null) && pickupable.TotalAmount > 0f)
			{
				if (this.deliveries.Count > 0)
				{
					FetchAreaChore.StatesInstance.Delivery delivery = this.deliveries[0];
					Chore chore = delivery.chore;
					delivery.Complete(this.deliverables);
					delivery.Cleanup();
					if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore)
					{
						this.deliveries.RemoveAt(0);
					}
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			if (this.deliveries.Count > 0 && this.deliveries[0].chore.amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				FetchAreaChore.StatesInstance.Delivery delivery2 = this.deliveries[0];
				Chore chore2 = delivery2.chore;
				delivery2.Complete(this.deliverables);
				delivery2.Cleanup();
				if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore2)
				{
					this.deliveries.RemoveAt(0);
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			base.smi.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x060080E9 RID: 33001 RVA: 0x0031409C File Offset: 0x0031229C
		public void FetchFail()
		{
			if (base.smi.sm.fetchTarget.Get(base.smi) != null)
			{
				base.smi.sm.fetchTarget.Get(base.smi).Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x060080EA RID: 33002 RVA: 0x00314134 File Offset: 0x00312334
		public void FetchComplete()
		{
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x060080EB RID: 33003 RVA: 0x00314178 File Offset: 0x00312378
		public void SetupDeliverables()
		{
			foreach (GameObject gameObject in base.sm.fetcher.Get<Storage>(base.smi).items)
			{
				if (!(gameObject == null))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					if (!(component == null) && !component.HasTag(GameTags.MarkedForMove))
					{
						Pickupable component2 = component.GetComponent<Pickupable>();
						if (component2 != null)
						{
							this.deliverables.Add(component2);
						}
					}
				}
			}
		}

		// Token: 0x060080EC RID: 33004 RVA: 0x0031421C File Offset: 0x0031241C
		public void ReservePickupables()
		{
			ChoreConsumer consumer = base.sm.fetcher.Get<ChoreConsumer>(base.smi);
			float num = this.fetchAmountRequested;
			foreach (Pickupable pickupable in this.fetchables)
			{
				if (num <= 0f)
				{
					break;
				}
				if (!pickupable.KPrefabID.HasTag(GameTags.MarkedForMove))
				{
					float num2 = Math.Min(num, pickupable.UnreservedAmount);
					num -= num2;
					FetchAreaChore.StatesInstance.Reservation item = new FetchAreaChore.StatesInstance.Reservation(consumer, pickupable, num2);
					this.reservations.Add(item);
				}
			}
		}

		// Token: 0x060080ED RID: 33005 RVA: 0x003142D0 File Offset: 0x003124D0
		private void OnFetchChoreCancelled(FetchChore chore)
		{
			int i = 0;
			while (i < this.deliveries.Count)
			{
				if (this.deliveries[i].chore == chore)
				{
					if (this.deliveries.Count == 1)
					{
						this.StopSM("AllDelivericesCancelled");
						return;
					}
					if (i == 0)
					{
						base.sm.currentdeliverycancelled.Trigger(this);
						return;
					}
					this.deliveries[i].Cleanup();
					this.deliveries.RemoveAt(i);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060080EE RID: 33006 RVA: 0x0031435C File Offset: 0x0031255C
		public void UnreservePickupables()
		{
			foreach (FetchAreaChore.StatesInstance.Reservation reservation in this.reservations)
			{
				reservation.Cleanup();
			}
			this.reservations.Clear();
		}

		// Token: 0x060080EF RID: 33007 RVA: 0x003143BC File Offset: 0x003125BC
		public bool SameDestination(FetchChore fetch)
		{
			using (List<FetchChore>.Enumerator enumerator = this.chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.destination == fetch.destination)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060080F0 RID: 33008 RVA: 0x00314420 File Offset: 0x00312620
		public void OnMarkForMove(object data)
		{
			GameObject x = base.smi.sm.fetchTarget.Get(base.smi);
			GameObject gameObject = data as GameObject;
			if (x != null)
			{
				if (x == gameObject)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
					base.smi.sm.fetchTarget.Set(null, base.smi);
					return;
				}
				global::Debug.LogError("Listening for MarkForMove on the incorrect fetch target. Subscriptions did not update correctly.");
			}
		}

		// Token: 0x04006121 RID: 24865
		private List<FetchChore> chores = new List<FetchChore>();

		// Token: 0x04006122 RID: 24866
		private List<Pickupable> fetchables = new List<Pickupable>();

		// Token: 0x04006123 RID: 24867
		private List<FetchAreaChore.StatesInstance.Reservation> reservations = new List<FetchAreaChore.StatesInstance.Reservation>();

		// Token: 0x04006124 RID: 24868
		private List<Pickupable> deliverables = new List<Pickupable>();

		// Token: 0x04006125 RID: 24869
		public List<FetchAreaChore.StatesInstance.Delivery> deliveries = new List<FetchAreaChore.StatesInstance.Delivery>();

		// Token: 0x04006126 RID: 24870
		private FetchChore rootChore;

		// Token: 0x04006127 RID: 24871
		private Chore.Precondition.Context rootContext;

		// Token: 0x04006128 RID: 24872
		private float fetchAmountRequested;

		// Token: 0x04006129 RID: 24873
		public bool delivering;

		// Token: 0x0400612A RID: 24874
		public bool pickingup;

		// Token: 0x0400612B RID: 24875
		private static Tag[] s_transientDeliveryTags = new Tag[]
		{
			GameTags.Garbage,
			GameTags.Creatures.Deliverable
		};

		// Token: 0x020023BC RID: 9148
		public struct Delivery
		{
			// Token: 0x17000C0C RID: 3084
			// (get) Token: 0x0600B775 RID: 46965 RVA: 0x003CD863 File Offset: 0x003CBA63
			// (set) Token: 0x0600B776 RID: 46966 RVA: 0x003CD86B File Offset: 0x003CBA6B
			public Storage destination { readonly get; private set; }

			// Token: 0x17000C0D RID: 3085
			// (get) Token: 0x0600B777 RID: 46967 RVA: 0x003CD874 File Offset: 0x003CBA74
			// (set) Token: 0x0600B778 RID: 46968 RVA: 0x003CD87C File Offset: 0x003CBA7C
			public float amount { readonly get; private set; }

			// Token: 0x17000C0E RID: 3086
			// (get) Token: 0x0600B779 RID: 46969 RVA: 0x003CD885 File Offset: 0x003CBA85
			// (set) Token: 0x0600B77A RID: 46970 RVA: 0x003CD88D File Offset: 0x003CBA8D
			public FetchChore chore { readonly get; private set; }

			// Token: 0x0600B77B RID: 46971 RVA: 0x003CD898 File Offset: 0x003CBA98
			public Delivery(Chore.Precondition.Context context, float amount_to_be_fetched, Action<FetchChore> on_cancelled)
			{
				this = default(FetchAreaChore.StatesInstance.Delivery);
				this.chore = (context.chore as FetchChore);
				this.amount = this.chore.originalAmount;
				this.destination = this.chore.destination;
				this.chore.SetOverrideTarget(context.consumerState.consumer);
				this.onCancelled = on_cancelled;
				this.onFetchChoreCleanup = new Action<Chore>(this.OnFetchChoreCleanup);
				this.chore.FetchAreaBegin(context, amount_to_be_fetched);
				FetchChore chore = this.chore;
				chore.onCleanup = (Action<Chore>)Delegate.Combine(chore.onCleanup, this.onFetchChoreCleanup);
			}

			// Token: 0x0600B77C RID: 46972 RVA: 0x003CD948 File Offset: 0x003CBB48
			public void Complete(List<Pickupable> deliverables)
			{
				using (new KProfiler.Region("FAC.Delivery.Complete", null))
				{
					if (!(this.destination == null) && !this.destination.IsEndOfLife())
					{
						FetchChore chore = this.chore;
						chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
						float num = this.amount;
						Pickupable pickupable = null;
						int num2 = 0;
						while (num2 < deliverables.Count && num > 0f)
						{
							if (deliverables[num2] == null)
							{
								if (num < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
								{
									this.destination.ForceStore(this.chore.tagsFirst, num);
								}
							}
							else if (!FetchAreaChore.IsPickupableStillValidForChore(deliverables[num2], this.chore))
							{
								global::Debug.LogError(string.Format("Attempting to store {0} in a {1} which did not request it", deliverables[num2], this.destination));
							}
							else
							{
								Pickupable pickupable2 = deliverables[num2].Take(num);
								if (pickupable2 != null && pickupable2.TotalAmount > 0f)
								{
									num -= pickupable2.TotalAmount;
									this.destination.Store(pickupable2.gameObject, false, false, true, false);
									pickupable = pickupable2;
									if (pickupable2 == deliverables[num2])
									{
										deliverables[num2] = null;
									}
								}
							}
							num2++;
						}
						if (this.chore.overrideTarget != null)
						{
							this.chore.FetchAreaEnd(this.chore.overrideTarget.GetComponent<ChoreDriver>(), pickupable, true);
						}
						this.chore = null;
					}
				}
			}

			// Token: 0x0600B77D RID: 46973 RVA: 0x003CDB00 File Offset: 0x003CBD00
			private void OnFetchChoreCleanup(Chore chore)
			{
				if (this.onCancelled != null)
				{
					this.onCancelled(chore as FetchChore);
				}
			}

			// Token: 0x0600B77E RID: 46974 RVA: 0x003CDB1B File Offset: 0x003CBD1B
			public void Cleanup()
			{
				if (this.chore != null)
				{
					FetchChore chore = this.chore;
					chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
					this.chore.FetchAreaEnd(null, null, false);
				}
			}

			// Token: 0x04009F8A RID: 40842
			private Action<FetchChore> onCancelled;

			// Token: 0x04009F8B RID: 40843
			private Action<Chore> onFetchChoreCleanup;
		}

		// Token: 0x020023BD RID: 9149
		public struct Reservation
		{
			// Token: 0x17000C0F RID: 3087
			// (get) Token: 0x0600B77F RID: 46975 RVA: 0x003CDB54 File Offset: 0x003CBD54
			// (set) Token: 0x0600B780 RID: 46976 RVA: 0x003CDB5C File Offset: 0x003CBD5C
			public float amount { readonly get; private set; }

			// Token: 0x17000C10 RID: 3088
			// (get) Token: 0x0600B781 RID: 46977 RVA: 0x003CDB65 File Offset: 0x003CBD65
			// (set) Token: 0x0600B782 RID: 46978 RVA: 0x003CDB6D File Offset: 0x003CBD6D
			public Pickupable pickupable { readonly get; private set; }

			// Token: 0x0600B783 RID: 46979 RVA: 0x003CDB78 File Offset: 0x003CBD78
			public Reservation(ChoreConsumer consumer, Pickupable pickupable, float reservation_amount)
			{
				this = default(FetchAreaChore.StatesInstance.Reservation);
				if (reservation_amount <= 0f)
				{
					global::Debug.LogError("Invalid amount: " + reservation_amount.ToString());
				}
				this.amount = reservation_amount;
				this.pickupable = pickupable;
				this.handle = pickupable.Reserve("FetchAreaChore", consumer.gameObject, reservation_amount);
			}

			// Token: 0x0600B784 RID: 46980 RVA: 0x003CDBD0 File Offset: 0x003CBDD0
			public void Cleanup()
			{
				if (this.pickupable != null)
				{
					this.pickupable.Unreserve("FetchAreaChore", this.handle);
				}
			}

			// Token: 0x04009F8E RID: 40846
			private int handle;
		}
	}

	// Token: 0x020011BA RID: 4538
	public class States : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore>
	{
		// Token: 0x060080F2 RID: 33010 RVA: 0x003144C8 File Offset: 0x003126C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetching;
			base.Target(this.fetcher);
			this.fetching.DefaultState(this.fetching.next).Enter("ReservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.ReservePickupables();
			}).Exit("UnreservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.UnreservePickupables();
			}).Enter("pickingup-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = true;
			}).Exit("pickingup-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = false;
			});
			this.fetching.next.Enter("SetupFetch", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupFetch();
			});
			this.fetching.movetopickupable.InitializeStates(this.fetcher, this.fetchTarget, new Func<FetchAreaChore.StatesInstance, CellOffset[]>(this.GetFetchOffset), this.fetching.pickup, this.fetching.fetchfail, NavigationTactics.ReduceTravelDistance).Target(this.fetchTarget).EventHandlerTransition(GameHashes.TagsChanged, this.fetching.fetchfail, (FetchAreaChore.StatesInstance smi, object obj) => smi.RootChore_ValidateRequiredTagOnTagChange && smi.RootChore_RequiredTag.IsValid && !this.fetchTarget.Get(smi).HasTag(smi.RootChore_RequiredTag)).Target(this.fetcher);
			this.fetching.pickup.DoPickup(this.fetchTarget, this.fetchResultTarget, this.fetchAmount, this.fetching.fetchcomplete, this.fetching.fetchfail).Exit(delegate(FetchAreaChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.fetchTarget.Get(smi);
				if (gameObject != null)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(smi.OnMarkForMove));
				}
			});
			this.fetching.fetchcomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchComplete();
			});
			this.fetching.fetchfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchFail();
			});
			this.delivering.DefaultState(this.delivering.next).OnSignal(this.currentdeliverycancelled, this.delivering.deliverfail).Enter("SetupDeliverables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDeliverables();
			}).Enter("delivering-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = true;
			}).Exit("delivering-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = false;
			});
			this.delivering.next.Enter("SetupDelivery", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDelivery();
			});
			this.delivering.movetostorage.InitializeStates(this.fetcher, this.deliveryDestination, new Func<FetchAreaChore.StatesInstance, CellOffset[]>(this.GetFetchOffset), this.delivering.storing, this.delivering.deliverfail, NavigationTactics.ReduceTravelDistance).Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				if (this.deliveryObject.Get(smi) != null && this.deliveryObject.Get(smi).GetComponent<MinionIdentity>() != null)
				{
					this.deliveryObject.Get(smi).transform.SetLocalPosition(Vector3.zero);
					KBatchedAnimTracker component = this.deliveryObject.Get(smi).GetComponent<KBatchedAnimTracker>();
					component.symbol = new HashedString("snapTo_chest");
					component.offset = new Vector3(0f, 0f, 1f);
				}
			});
			this.delivering.storing.DoDelivery(this.fetcher, this.deliveryDestination, this.delivering.delivercomplete, this.delivering.deliverfail);
			this.delivering.deliverfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverFail();
			});
			this.delivering.delivercomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverComplete();
			});
		}

		// Token: 0x060080F3 RID: 33011 RVA: 0x003148D8 File Offset: 0x00312AD8
		private CellOffset[] GetFetchOffset(FetchAreaChore.StatesInstance smi)
		{
			WorkerBase component = this.fetcher.Get(smi).GetComponent<WorkerBase>();
			if (!(component != null))
			{
				return null;
			}
			return component.GetFetchCellOffsets();
		}

		// Token: 0x0400612C RID: 24876
		public FetchAreaChore.States.FetchStates fetching;

		// Token: 0x0400612D RID: 24877
		public FetchAreaChore.States.DeliverStates delivering;

		// Token: 0x0400612E RID: 24878
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher;

		// Token: 0x0400612F RID: 24879
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchTarget;

		// Token: 0x04006130 RID: 24880
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchResultTarget;

		// Token: 0x04006131 RID: 24881
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter fetchAmount;

		// Token: 0x04006132 RID: 24882
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryDestination;

		// Token: 0x04006133 RID: 24883
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryObject;

		// Token: 0x04006134 RID: 24884
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter deliveryAmount;

		// Token: 0x04006135 RID: 24885
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.Signal currentdeliverycancelled;

		// Token: 0x020023BF RID: 9151
		public class FetchStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x04009F90 RID: 40848
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x04009F91 RID: 40849
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Pickupable> movetopickupable;

			// Token: 0x04009F92 RID: 40850
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State pickup;

			// Token: 0x04009F93 RID: 40851
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchfail;

			// Token: 0x04009F94 RID: 40852
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchcomplete;
		}

		// Token: 0x020023C0 RID: 9152
		public class DeliverStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x04009F95 RID: 40853
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x04009F96 RID: 40854
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Storage> movetostorage;

			// Token: 0x04009F97 RID: 40855
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State storing;

			// Token: 0x04009F98 RID: 40856
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State deliverfail;

			// Token: 0x04009F99 RID: 40857
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State delivercomplete;
		}
	}
}
