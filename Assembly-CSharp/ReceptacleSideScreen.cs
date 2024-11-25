using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D93 RID: 3475
public class ReceptacleSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x06006D82 RID: 28034 RVA: 0x00292AC4 File Offset: 0x00290CC4
	public override string GetTitle()
	{
		if (this.targetReceptacle == null)
		{
			return Strings.Get(this.titleKey).ToString().Replace("{0}", "");
		}
		return string.Format(Strings.Get(this.titleKey), this.targetReceptacle.GetProperName());
	}

	// Token: 0x06006D83 RID: 28035 RVA: 0x00292B20 File Offset: 0x00290D20
	public void Initialize(SingleEntityReceptacle target)
	{
		if (target == null)
		{
			global::Debug.LogError("SingleObjectReceptacle provided was null.");
			return;
		}
		this.targetReceptacle = target;
		base.gameObject.SetActive(true);
		this.depositObjectMap = new Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity>();
		this.entityToggles.ForEach(delegate(ReceptacleToggle rbi)
		{
			UnityEngine.Object.Destroy(rbi.gameObject);
		});
		this.entityToggles.Clear();
		foreach (Tag tag in this.targetReceptacle.possibleDepositObjectTags)
		{
			List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(tag);
			int num = prefabsWithTag.Count;
			List<IHasSortOrder> list = new List<IHasSortOrder>();
			foreach (GameObject gameObject in prefabsWithTag)
			{
				if (!this.targetReceptacle.IsValidEntity(gameObject))
				{
					num--;
				}
				else
				{
					IHasSortOrder component = gameObject.GetComponent<IHasSortOrder>();
					if (component != null)
					{
						list.Add(component);
					}
				}
			}
			global::Debug.Assert(list.Count == num, "Not all entities in this receptacle implement IHasSortOrder!");
			list.Sort((IHasSortOrder a, IHasSortOrder b) => a.sortOrder - b.sortOrder);
			foreach (IHasSortOrder hasSortOrder in list)
			{
				GameObject gameObject2 = (hasSortOrder as MonoBehaviour).gameObject;
				GameObject gameObject3 = Util.KInstantiateUI(this.entityToggle, this.requestObjectList, false);
				gameObject3.SetActive(true);
				ReceptacleToggle newToggle = gameObject3.GetComponent<ReceptacleToggle>();
				IReceptacleDirection component2 = gameObject2.GetComponent<IReceptacleDirection>();
				string entityName = this.GetEntityName(gameObject2.PrefabID());
				newToggle.title.text = entityName;
				Sprite entityIcon = this.GetEntityIcon(gameObject2.PrefabID());
				if (entityIcon == null)
				{
					entityIcon = this.elementPlaceholderSpr;
				}
				newToggle.image.sprite = entityIcon;
				newToggle.toggle.onClick += delegate()
				{
					this.ToggleClicked(newToggle);
				};
				newToggle.toggle.onPointerEnter += delegate()
				{
					this.CheckAmountsAndUpdate(null);
				};
				ToolTip component3 = newToggle.GetComponent<ToolTip>();
				if (component3 != null)
				{
					component3.SetSimpleTooltip(this.GetEntityTooltip(gameObject2.PrefabID()));
				}
				this.depositObjectMap.Add(newToggle, new ReceptacleSideScreen.SelectableEntity
				{
					tag = gameObject2.PrefabID(),
					direction = ((component2 != null) ? component2.Direction : SingleEntityReceptacle.ReceptacleDirection.Top),
					asset = gameObject2
				});
				this.entityToggles.Add(newToggle);
			}
		}
		this.RestoreSelectionFromOccupant();
		this.selectedEntityToggle = null;
		if (this.entityToggles.Count > 0)
		{
			if (this.entityPreviousSelectionMap.ContainsKey(this.targetReceptacle))
			{
				int index = this.entityPreviousSelectionMap[this.targetReceptacle];
				this.ToggleClicked(this.entityToggles[index]);
			}
			else
			{
				this.subtitleLabel.SetText(Strings.Get(this.subtitleStringSelect).ToString());
				this.requestSelectedEntityBtn.isInteractable = false;
				this.descriptionLabel.SetText(Strings.Get(this.subtitleStringSelectDescription).ToString());
				this.HideAllDescriptorPanels();
			}
		}
		this.onStorageChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1697596308, new Action<object>(this.CheckAmountsAndUpdate));
		this.onOccupantValidChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1820564715, new Action<object>(this.OnOccupantValidChanged));
		this.UpdateState(null);
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06006D84 RID: 28036 RVA: 0x00292F40 File Offset: 0x00291140
	protected virtual void UpdateState(object data)
	{
		this.requestSelectedEntityBtn.ClearOnClick();
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.CheckReceptacleOccupied())
		{
			Uprootable uprootable = this.targetReceptacle.Occupant.GetComponent<Uprootable>();
			if (uprootable != null && uprootable.IsMarkedForUproot)
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					uprootable.ForceCancelUproot(null);
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingRemoval).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			else
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					this.targetReceptacle.OrderRemoveOccupant();
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringEntityDeposited).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			Tag tag = this.targetReceptacle.Occupant.GetComponent<KSelectable>().PrefabID();
			this.ConfigureActiveEntity(tag);
			this.SetResultDescriptions(this.targetReceptacle.Occupant);
		}
		else if (this.targetReceptacle.GetActiveRequest != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			this.ConfigureActiveEntity(this.targetReceptacle.GetActiveRequest.tagsFirst);
			GameObject prefab = Assets.GetPrefab(this.targetReceptacle.GetActiveRequest.tagsFirst);
			if (prefab != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingDelivery).ToString(), prefab.GetProperName()));
				this.SetResultDescriptions(prefab);
			}
		}
		else if (this.selectedEntityToggle != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.targetReceptacle.SetPreview(this.depositObjectMap[this.selectedEntityToggle].tag, false);
			bool flag = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle]);
			this.requestSelectedEntityBtn.isInteractable = flag;
			this.SetImageToggleState(this.selectedEntityToggle.toggle, flag ? ImageToggleState.State.Active : ImageToggleState.State.DisabledActive);
			this.ToggleObjectPicker(true);
			GameObject prefab2 = Assets.GetPrefab(this.selectedDepositObjectTag);
			if (prefab2 != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingSelection).ToString(), prefab2.GetProperName()));
				this.SetResultDescriptions(prefab2);
			}
		}
		else
		{
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = false;
			this.ToggleObjectPicker(true);
		}
		this.UpdateAvailableAmounts(null);
		this.UpdateListeners();
	}

	// Token: 0x06006D85 RID: 28037 RVA: 0x002932C0 File Offset: 0x002914C0
	private void UpdateListeners()
	{
		if (this.CheckReceptacleOccupied())
		{
			if (this.onObjectDestroyedHandle == -1)
			{
				this.onObjectDestroyedHandle = this.targetReceptacle.Occupant.gameObject.Subscribe(1969584890, delegate(object d)
				{
					this.UpdateState(null);
				});
				return;
			}
		}
		else if (this.onObjectDestroyedHandle != -1)
		{
			this.onObjectDestroyedHandle = -1;
		}
	}

	// Token: 0x06006D86 RID: 28038 RVA: 0x0029331C File Offset: 0x0029151C
	private void OnOccupantValidChanged(object obj)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (!this.CheckReceptacleOccupied() && this.targetReceptacle.GetActiveRequest != null)
		{
			bool flag = false;
			ReceptacleSideScreen.SelectableEntity entity;
			if (this.depositObjectMap.TryGetValue(this.selectedEntityToggle, out entity))
			{
				flag = this.CanDepositEntity(entity);
			}
			if (!flag)
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateState(null);
				this.UpdateAvailableAmounts(null);
			}
		}
	}

	// Token: 0x06006D87 RID: 28039 RVA: 0x0029338F File Offset: 0x0029158F
	private bool CanDepositEntity(ReceptacleSideScreen.SelectableEntity entity)
	{
		return this.ValidRotationForDeposit(entity.direction) && (!this.RequiresAvailableAmountToDeposit() || this.GetAvailableAmount(entity.tag) > 0f) && this.AdditionalCanDepositTest();
	}

	// Token: 0x06006D88 RID: 28040 RVA: 0x002933C2 File Offset: 0x002915C2
	protected virtual bool AdditionalCanDepositTest()
	{
		return true;
	}

	// Token: 0x06006D89 RID: 28041 RVA: 0x002933C5 File Offset: 0x002915C5
	protected virtual bool RequiresAvailableAmountToDeposit()
	{
		return true;
	}

	// Token: 0x06006D8A RID: 28042 RVA: 0x002933C8 File Offset: 0x002915C8
	private void ClearSelection()
	{
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			keyValuePair.Key.toggle.Deselect();
		}
	}

	// Token: 0x06006D8B RID: 28043 RVA: 0x00293428 File Offset: 0x00291628
	private void ToggleObjectPicker(bool Show)
	{
		this.requestObjectListContainer.SetActive(Show);
		if (this.scrollBarContainer != null)
		{
			this.scrollBarContainer.SetActive(Show);
		}
		this.requestObjectList.SetActive(Show);
		this.activeEntityContainer.SetActive(!Show);
	}

	// Token: 0x06006D8C RID: 28044 RVA: 0x00293478 File Offset: 0x00291678
	private void ConfigureActiveEntity(Tag tag)
	{
		string properName = Assets.GetPrefab(tag).GetProperName();
		this.activeEntityContainer.GetComponentInChildrenOnly<LocText>().text = properName;
		this.activeEntityContainer.transform.GetChild(0).gameObject.GetComponentInChildrenOnly<Image>().sprite = this.GetEntityIcon(tag);
	}

	// Token: 0x06006D8D RID: 28045 RVA: 0x002934C9 File Offset: 0x002916C9
	protected virtual string GetEntityName(Tag prefabTag)
	{
		return Assets.GetPrefab(prefabTag).GetProperName();
	}

	// Token: 0x06006D8E RID: 28046 RVA: 0x002934D8 File Offset: 0x002916D8
	protected virtual string GetEntityTooltip(Tag prefabTag)
	{
		InfoDescription component = Assets.GetPrefab(prefabTag).GetComponent<InfoDescription>();
		string text = this.GetEntityName(prefabTag);
		if (component != null)
		{
			text = text + "\n\n" + component.description;
		}
		return text;
	}

	// Token: 0x06006D8F RID: 28047 RVA: 0x00293515 File Offset: 0x00291715
	protected virtual Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x06006D90 RID: 28048 RVA: 0x00293530 File Offset: 0x00291730
	public override bool IsValidForTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		return component != null && component.enabled && target.GetComponent<PlantablePlot>() == null && target.GetComponent<EggIncubator>() == null && target.GetComponent<SpecialCargoBayClusterReceptacle>() == null;
	}

	// Token: 0x06006D91 RID: 28049 RVA: 0x00293580 File Offset: 0x00291780
	public override void SetTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		if (component == null)
		{
			global::Debug.LogError("The object selected doesn't have a SingleObjectReceptacle!");
			return;
		}
		this.Initialize(component);
		this.UpdateState(null);
	}

	// Token: 0x06006D92 RID: 28050 RVA: 0x002935B6 File Offset: 0x002917B6
	protected virtual void RestoreSelectionFromOccupant()
	{
	}

	// Token: 0x06006D93 RID: 28051 RVA: 0x002935B8 File Offset: 0x002917B8
	public override void ClearTarget()
	{
		if (this.targetReceptacle != null)
		{
			if (this.CheckReceptacleOccupied())
			{
				this.targetReceptacle.Occupant.gameObject.Unsubscribe(this.onObjectDestroyedHandle);
				this.onObjectDestroyedHandle = -1;
			}
			this.targetReceptacle.Unsubscribe(this.onStorageChangedHandle);
			this.onStorageChangedHandle = -1;
			this.targetReceptacle.Unsubscribe(this.onOccupantValidChangedHandle);
			this.onOccupantValidChangedHandle = -1;
			if (this.targetReceptacle.GetActiveRequest == null)
			{
				this.targetReceptacle.SetPreview(Tag.Invalid, false);
			}
			SimAndRenderScheduler.instance.Remove(this);
			this.targetReceptacle = null;
		}
	}

	// Token: 0x06006D94 RID: 28052 RVA: 0x00293660 File Offset: 0x00291860
	protected void SetImageToggleState(KToggle toggle, ImageToggleState.State state)
	{
		switch (state)
		{
		case ImageToggleState.State.Disabled:
			toggle.GetComponent<ImageToggleState>().SetDisabled();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.desaturatedMaterial;
			return;
		case ImageToggleState.State.Inactive:
			toggle.GetComponent<ImageToggleState>().SetInactive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.defaultMaterial;
			return;
		case ImageToggleState.State.Active:
			toggle.GetComponent<ImageToggleState>().SetActive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.defaultMaterial;
			return;
		case ImageToggleState.State.DisabledActive:
			toggle.GetComponent<ImageToggleState>().SetDisabledActive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.desaturatedMaterial;
			return;
		default:
			return;
		}
	}

	// Token: 0x06006D95 RID: 28053 RVA: 0x0029370B File Offset: 0x0029190B
	public void Render1000ms(float dt)
	{
		this.CheckAmountsAndUpdate(null);
	}

	// Token: 0x06006D96 RID: 28054 RVA: 0x00293714 File Offset: 0x00291914
	private void CheckAmountsAndUpdate(object data)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.UpdateAvailableAmounts(null))
		{
			this.UpdateState(null);
		}
	}

	// Token: 0x06006D97 RID: 28055 RVA: 0x00293738 File Offset: 0x00291938
	private bool UpdateAvailableAmounts(object data)
	{
		bool result = false;
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			if (!DebugHandler.InstantBuildMode && this.hideUndiscoveredEntities && !DiscoveredResources.Instance.IsDiscovered(keyValuePair.Value.tag))
			{
				keyValuePair.Key.gameObject.SetActive(false);
			}
			else if (!keyValuePair.Key.gameObject.activeSelf)
			{
				keyValuePair.Key.gameObject.SetActive(true);
			}
			float availableAmount = this.GetAvailableAmount(keyValuePair.Value.tag);
			if (keyValuePair.Value.lastAmount != availableAmount)
			{
				result = true;
				keyValuePair.Value.lastAmount = availableAmount;
				keyValuePair.Key.amount.text = availableAmount.ToString();
			}
			if (!this.ValidRotationForDeposit(keyValuePair.Value.direction) || availableAmount <= 0f)
			{
				if (this.selectedEntityToggle != keyValuePair.Key)
				{
					this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Disabled);
				}
				else
				{
					this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.DisabledActive);
				}
			}
			else if (this.selectedEntityToggle != keyValuePair.Key)
			{
				this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Inactive);
			}
			else
			{
				this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Active);
			}
		}
		return result;
	}

	// Token: 0x06006D98 RID: 28056 RVA: 0x002938D8 File Offset: 0x00291AD8
	protected float GetAvailableAmount(Tag tag)
	{
		if (this.ALLOW_ORDER_IGNORING_WOLRD_NEED)
		{
			IEnumerable<Pickupable> pickupables = this.targetReceptacle.GetMyWorld().worldInventory.GetPickupables(tag, true);
			float num = 0f;
			foreach (Pickupable pickupable in pickupables)
			{
				num += (float)Mathf.CeilToInt(pickupable.TotalAmount);
			}
			return num;
		}
		return this.targetReceptacle.GetMyWorld().worldInventory.GetAmount(tag, true);
	}

	// Token: 0x06006D99 RID: 28057 RVA: 0x00293968 File Offset: 0x00291B68
	private bool ValidRotationForDeposit(SingleEntityReceptacle.ReceptacleDirection depositDir)
	{
		return this.targetReceptacle.rotatable == null || depositDir == this.targetReceptacle.Direction;
	}

	// Token: 0x06006D9A RID: 28058 RVA: 0x00293990 File Offset: 0x00291B90
	protected virtual void ToggleClicked(ReceptacleToggle toggle)
	{
		if (!this.depositObjectMap.ContainsKey(toggle))
		{
			global::Debug.LogError("Recipe not found on recipe list.");
			return;
		}
		if (this.selectedEntityToggle != null)
		{
			bool flag = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle]);
			this.requestSelectedEntityBtn.isInteractable = flag;
			this.SetImageToggleState(this.selectedEntityToggle.toggle, flag ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled);
		}
		this.selectedEntityToggle = toggle;
		this.entityPreviousSelectionMap[this.targetReceptacle] = this.entityToggles.IndexOf(toggle);
		this.selectedDepositObjectTag = this.depositObjectMap[toggle].tag;
		MutantPlant component = this.depositObjectMap[toggle].asset.GetComponent<MutantPlant>();
		this.selectedDepositObjectAdditionalTag = (component ? component.SubSpeciesID : Tag.Invalid);
		this.UpdateAvailableAmounts(null);
		this.UpdateState(null);
	}

	// Token: 0x06006D9B RID: 28059 RVA: 0x00293A7C File Offset: 0x00291C7C
	private void CreateOrder(bool isInfinite)
	{
		this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
	}

	// Token: 0x06006D9C RID: 28060 RVA: 0x00293A95 File Offset: 0x00291C95
	protected bool CheckReceptacleOccupied()
	{
		return this.targetReceptacle != null && this.targetReceptacle.Occupant != null;
	}

	// Token: 0x06006D9D RID: 28061 RVA: 0x00293ABC File Offset: 0x00291CBC
	protected virtual void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text = component.description;
		}
		else
		{
			KPrefabID component2 = go.GetComponent<KPrefabID>();
			if (component2 != null)
			{
				Element element = ElementLoader.GetElement(component2.PrefabID());
				if (element != null)
				{
					text = element.Description();
				}
			}
			else
			{
				text = go.GetProperName();
			}
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x06006D9E RID: 28062 RVA: 0x00293B24 File Offset: 0x00291D24
	protected virtual void HideAllDescriptorPanels()
	{
		for (int i = 0; i < this.descriptorPanels.Count; i++)
		{
			this.descriptorPanels[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x04004AAC RID: 19116
	protected bool ALLOW_ORDER_IGNORING_WOLRD_NEED = true;

	// Token: 0x04004AAD RID: 19117
	[SerializeField]
	protected KButton requestSelectedEntityBtn;

	// Token: 0x04004AAE RID: 19118
	[SerializeField]
	private string requestStringDeposit;

	// Token: 0x04004AAF RID: 19119
	[SerializeField]
	private string requestStringCancelDeposit;

	// Token: 0x04004AB0 RID: 19120
	[SerializeField]
	private string requestStringRemove;

	// Token: 0x04004AB1 RID: 19121
	[SerializeField]
	private string requestStringCancelRemove;

	// Token: 0x04004AB2 RID: 19122
	public GameObject activeEntityContainer;

	// Token: 0x04004AB3 RID: 19123
	public GameObject nothingDiscoveredContainer;

	// Token: 0x04004AB4 RID: 19124
	[SerializeField]
	protected LocText descriptionLabel;

	// Token: 0x04004AB5 RID: 19125
	protected Dictionary<SingleEntityReceptacle, int> entityPreviousSelectionMap = new Dictionary<SingleEntityReceptacle, int>();

	// Token: 0x04004AB6 RID: 19126
	[SerializeField]
	private string subtitleStringSelect;

	// Token: 0x04004AB7 RID: 19127
	[SerializeField]
	private string subtitleStringSelectDescription;

	// Token: 0x04004AB8 RID: 19128
	[SerializeField]
	private string subtitleStringAwaitingSelection;

	// Token: 0x04004AB9 RID: 19129
	[SerializeField]
	private string subtitleStringAwaitingDelivery;

	// Token: 0x04004ABA RID: 19130
	[SerializeField]
	private string subtitleStringEntityDeposited;

	// Token: 0x04004ABB RID: 19131
	[SerializeField]
	private string subtitleStringAwaitingRemoval;

	// Token: 0x04004ABC RID: 19132
	[SerializeField]
	private LocText subtitleLabel;

	// Token: 0x04004ABD RID: 19133
	[SerializeField]
	private List<DescriptorPanel> descriptorPanels;

	// Token: 0x04004ABE RID: 19134
	public Material defaultMaterial;

	// Token: 0x04004ABF RID: 19135
	public Material desaturatedMaterial;

	// Token: 0x04004AC0 RID: 19136
	[SerializeField]
	private GameObject requestObjectList;

	// Token: 0x04004AC1 RID: 19137
	[SerializeField]
	private GameObject requestObjectListContainer;

	// Token: 0x04004AC2 RID: 19138
	[SerializeField]
	private GameObject scrollBarContainer;

	// Token: 0x04004AC3 RID: 19139
	[SerializeField]
	private GameObject entityToggle;

	// Token: 0x04004AC4 RID: 19140
	[SerializeField]
	private Sprite buttonSelectedBG;

	// Token: 0x04004AC5 RID: 19141
	[SerializeField]
	private Sprite buttonNormalBG;

	// Token: 0x04004AC6 RID: 19142
	[SerializeField]
	private Sprite elementPlaceholderSpr;

	// Token: 0x04004AC7 RID: 19143
	[SerializeField]
	private bool hideUndiscoveredEntities;

	// Token: 0x04004AC8 RID: 19144
	protected ReceptacleToggle selectedEntityToggle;

	// Token: 0x04004AC9 RID: 19145
	protected SingleEntityReceptacle targetReceptacle;

	// Token: 0x04004ACA RID: 19146
	protected Tag selectedDepositObjectTag;

	// Token: 0x04004ACB RID: 19147
	protected Tag selectedDepositObjectAdditionalTag;

	// Token: 0x04004ACC RID: 19148
	protected Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObjectMap;

	// Token: 0x04004ACD RID: 19149
	protected List<ReceptacleToggle> entityToggles = new List<ReceptacleToggle>();

	// Token: 0x04004ACE RID: 19150
	private int onObjectDestroyedHandle = -1;

	// Token: 0x04004ACF RID: 19151
	private int onOccupantValidChangedHandle = -1;

	// Token: 0x04004AD0 RID: 19152
	private int onStorageChangedHandle = -1;

	// Token: 0x02001EB3 RID: 7859
	protected class SelectableEntity
	{
		// Token: 0x04008B47 RID: 35655
		public Tag tag;

		// Token: 0x04008B48 RID: 35656
		public SingleEntityReceptacle.ReceptacleDirection direction;

		// Token: 0x04008B49 RID: 35657
		public GameObject asset;

		// Token: 0x04008B4A RID: 35658
		public float lastAmount = -1f;
	}
}
