using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CED RID: 3309
public class NameDisplayScreen : KScreen
{
	// Token: 0x06006684 RID: 26244 RVA: 0x00264BE3 File Offset: 0x00262DE3
	public static void DestroyInstance()
	{
		NameDisplayScreen.Instance = null;
	}

	// Token: 0x06006685 RID: 26245 RVA: 0x00264BEB File Offset: 0x00262DEB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NameDisplayScreen.Instance = this;
	}

	// Token: 0x06006686 RID: 26246 RVA: 0x00264BF9 File Offset: 0x00262DF9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Health.Register(new Action<Health>(this.OnHealthAdded), null);
		Components.Equipment.Register(new Action<Equipment>(this.OnEquipmentAdded), null);
		this.BindOnOverlayChange();
	}

	// Token: 0x06006687 RID: 26247 RVA: 0x00264C38 File Offset: 0x00262E38
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isOverlayChangeBound && OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = false;
		}
	}

	// Token: 0x06006688 RID: 26248 RVA: 0x00264C90 File Offset: 0x00262E90
	private void BindOnOverlayChange()
	{
		if (this.isOverlayChangeBound)
		{
			return;
		}
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = true;
		}
	}

	// Token: 0x06006689 RID: 26249 RVA: 0x00264CE0 File Offset: 0x00262EE0
	public void RemoveWorldEntries(int worldId)
	{
		this.entries.RemoveAll((NameDisplayScreen.Entry entry) => entry.world_go.IsNullOrDestroyed() || entry.world_go.GetMyWorldId() == worldId);
	}

	// Token: 0x0600668A RID: 26250 RVA: 0x00264D12 File Offset: 0x00262F12
	private void OnOverlayChanged(HashedString new_mode)
	{
		HashedString hashedString = this.lastKnownOverlayID;
		this.lastKnownOverlayID = new_mode;
		this.nameDisplayCanvas.enabled = (this.lastKnownOverlayID == OverlayModes.None.ID);
	}

	// Token: 0x0600668B RID: 26251 RVA: 0x00264D3D File Offset: 0x00262F3D
	private void OnHealthAdded(Health health)
	{
		this.RegisterComponent(health.gameObject, health, false);
	}

	// Token: 0x0600668C RID: 26252 RVA: 0x00264D50 File Offset: 0x00262F50
	private void OnEquipmentAdded(Equipment equipment)
	{
		MinionAssignablesProxy component = equipment.GetComponent<MinionAssignablesProxy>();
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject)
		{
			this.RegisterComponent(targetGameObject, equipment, false);
			return;
		}
		global::Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", new object[]
		{
			component.TargetInstanceID
		});
	}

	// Token: 0x0600668D RID: 26253 RVA: 0x00264D9C File Offset: 0x00262F9C
	private bool ShouldShowName(GameObject representedObject)
	{
		CharacterOverlay component = representedObject.GetComponent<CharacterOverlay>();
		return component != null && component.shouldShowName;
	}

	// Token: 0x0600668E RID: 26254 RVA: 0x00264DC4 File Offset: 0x00262FC4
	public Guid AddAreaText(string initialText, GameObject prefab)
	{
		NameDisplayScreen.TextEntry textEntry = new NameDisplayScreen.TextEntry();
		textEntry.guid = Guid.NewGuid();
		textEntry.display_go = Util.KInstantiateUI(prefab, this.areaTextDisplayCanvas.gameObject, true);
		textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
		this.textEntries.Add(textEntry);
		return textEntry.guid;
	}

	// Token: 0x0600668F RID: 26255 RVA: 0x00264E20 File Offset: 0x00263020
	public GameObject GetWorldText(Guid guid)
	{
		GameObject result = null;
		foreach (NameDisplayScreen.TextEntry textEntry in this.textEntries)
		{
			if (textEntry.guid == guid)
			{
				result = textEntry.display_go;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006690 RID: 26256 RVA: 0x00264E88 File Offset: 0x00263088
	public void RemoveWorldText(Guid guid)
	{
		int num = -1;
		for (int i = 0; i < this.textEntries.Count; i++)
		{
			if (this.textEntries[i].guid == guid)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			UnityEngine.Object.Destroy(this.textEntries[num].display_go);
			this.textEntries.RemoveAt(num);
		}
	}

	// Token: 0x06006691 RID: 26257 RVA: 0x00264EF0 File Offset: 0x002630F0
	public void AddNewEntry(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = new NameDisplayScreen.Entry();
		entry.world_go = representedObject;
		entry.world_go_anim_controller = representedObject.GetComponent<KAnimControllerBase>();
		GameObject original = this.ShouldShowName(representedObject) ? this.nameAndBarsPrefab : this.barsPrefab;
		entry.kprfabID = representedObject.GetComponent<KPrefabID>();
		entry.collider = representedObject.GetComponent<KBoxCollider2D>();
		GameObject gameObject = Util.KInstantiateUI(original, this.nameDisplayCanvas.gameObject, true);
		entry.display_go = gameObject;
		entry.display_go_rect = gameObject.GetComponent<RectTransform>();
		entry.nameLabel = entry.display_go.GetComponentInChildren<LocText>();
		entry.display_go.SetActive(false);
		if (this.worldSpace)
		{
			entry.display_go.transform.localScale = Vector3.one * 0.01f;
		}
		gameObject.name = representedObject.name + " character overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		this.entries.Add(entry);
		UnityEngine.Object component = representedObject.GetComponent<KSelectable>();
		FactionAlignment component2 = representedObject.GetComponent<FactionAlignment>();
		if (component != null)
		{
			if (component2 != null)
			{
				if (component2.Alignment == FactionManager.FactionID.Friendly || component2.Alignment == FactionManager.FactionID.Duplicant)
				{
					this.UpdateName(representedObject);
					return;
				}
			}
			else
			{
				this.UpdateName(representedObject);
			}
		}
	}

	// Token: 0x06006692 RID: 26258 RVA: 0x00265028 File Offset: 0x00263228
	public void RegisterComponent(GameObject representedObject, object component, bool force_new_entry = false)
	{
		NameDisplayScreen.Entry entry = force_new_entry ? null : this.GetEntry(representedObject);
		if (entry == null)
		{
			CharacterOverlay component2 = representedObject.GetComponent<CharacterOverlay>();
			if (component2 != null)
			{
				component2.Register();
				entry = this.GetEntry(representedObject);
			}
		}
		if (entry == null)
		{
			return;
		}
		Transform reference = entry.refs.GetReference<Transform>("Bars");
		entry.bars_go = reference.gameObject;
		if (component is Health)
		{
			if (!entry.healthBar)
			{
				Health health = (Health)component;
				GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject, false);
				gameObject.name = "Health Bar";
				health.healthBar = gameObject.GetComponent<HealthBar>();
				health.healthBar.GetComponent<KSelectable>().entityName = UI.METERS.HEALTH.TOOLTIP;
				health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable = (representedObject.GetComponent<MinionBrain>() != null);
				entry.healthBar = health.healthBar;
				entry.healthBar.autoHide = false;
				gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
				return;
			}
			global::Debug.LogWarningFormat("Health added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is OxygenBreather)
		{
			if (!entry.breathBar)
			{
				GameObject gameObject2 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.breathBar = gameObject2.GetComponent<ProgressBar>();
				entry.breathBar.autoHide = false;
				gameObject2.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", this.ToolTipStyle_Property);
				gameObject2.name = "Breath Bar";
				gameObject2.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BreathBar");
				gameObject2.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("OxygenBreather added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is Equipment)
		{
			if (!entry.suitBar)
			{
				GameObject gameObject3 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBar = gameObject3.GetComponent<ProgressBar>();
				entry.suitBar.autoHide = false;
				gameObject3.name = "Suit Tank Bar";
				gameObject3.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
				gameObject3.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("SuitBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitFuelBar)
			{
				GameObject gameObject4 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitFuelBar = gameObject4.GetComponent<ProgressBar>();
				entry.suitFuelBar.autoHide = false;
				gameObject4.name = "Suit Fuel Bar";
				gameObject4.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");
				gameObject4.GetComponent<KSelectable>().entityName = UI.METERS.FUEL.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("FuelBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitBatteryBar)
			{
				GameObject gameObject5 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBatteryBar = gameObject5.GetComponent<ProgressBar>();
				entry.suitBatteryBar.autoHide = false;
				gameObject5.name = "Suit Battery Bar";
				gameObject5.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BatteryBar");
				gameObject5.GetComponent<KSelectable>().entityName = UI.METERS.BATTERY.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("CoolantBar added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is ThoughtGraph.Instance || component is CreatureThoughtGraph.Instance)
		{
			if (!entry.thoughtBubble)
			{
				GameObject gameObject6 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go, false);
				entry.thoughtBubble = gameObject6.GetComponent<HierarchyReferences>();
				gameObject6.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble";
				GameObject gameObject7 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go, false);
				entry.thoughtBubbleConvo = gameObject7.GetComponent<HierarchyReferences>();
				gameObject7.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble Convo";
				return;
			}
			global::Debug.LogWarningFormat("ThoughtGraph added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else
		{
			if (!(component is GameplayEventMonitor.Instance))
			{
				if (component is Dreamer.Instance && !entry.dreamBubble)
				{
					GameObject gameObject8 = Util.KInstantiateUI(EffectPrefabs.Instance.DreamBubble, entry.display_go, false);
					gameObject8.name = "Dream Bubble";
					entry.dreamBubble = gameObject8.GetComponent<DreamBubble>();
				}
				return;
			}
			if (!entry.gameplayEventDisplay)
			{
				GameObject gameObject9 = Util.KInstantiateUI(EffectPrefabs.Instance.GameplayEventDisplay, entry.display_go, false);
				entry.gameplayEventDisplay = gameObject9.GetComponent<HierarchyReferences>();
				gameObject9.name = "Gameplay Event Display";
				return;
			}
			global::Debug.LogWarningFormat("GameplayEventDisplay added twice {0}", new object[]
			{
				component
			});
			return;
		}
	}

	// Token: 0x06006693 RID: 26259 RVA: 0x0026558A File Offset: 0x0026378A
	public bool IsVisibleToZoom()
	{
		return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < this.HideDistance;
	}

	// Token: 0x06006694 RID: 26260 RVA: 0x002655B0 File Offset: 0x002637B0
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		this.BindOnOverlayChange();
		if (Game.MainCamera == null)
		{
			return;
		}
		if (this.lastKnownOverlayID != OverlayModes.None.ID)
		{
			return;
		}
		int count = this.entries.Count;
		bool flag = this.IsVisibleToZoom();
		bool flag2 = flag && this.lastKnownOverlayID == OverlayModes.None.ID;
		if (this.nameDisplayCanvas.enabled != flag2)
		{
			this.nameDisplayCanvas.enabled = flag2;
		}
		if (flag)
		{
			this.RemoveDestroyedEntries();
			this.Culling();
			this.UpdatePos();
			this.HideDeadProgressBars();
		}
	}

	// Token: 0x06006695 RID: 26261 RVA: 0x00265650 File Offset: 0x00263850
	private void Culling()
	{
		if (this.entries.Count == 0)
		{
			return;
		}
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		int num = Mathf.Min(500, this.entries.Count);
		for (int i = 0; i < num; i++)
		{
			int index = (this.currentUpdateIndex + i) % this.entries.Count;
			NameDisplayScreen.Entry entry = this.entries[index];
			Vector3 position = entry.world_go.transform.GetPosition();
			bool flag = position.x >= (float)vector2I.x && position.y >= (float)vector2I.y && position.x < (float)vector2I2.x && position.y < (float)vector2I2.y;
			if (entry.visible != flag)
			{
				entry.display_go.SetActive(flag);
			}
			entry.visible = flag;
		}
		this.currentUpdateIndex = (this.currentUpdateIndex + num) % this.entries.Count;
	}

	// Token: 0x06006696 RID: 26262 RVA: 0x0026575C File Offset: 0x0026395C
	private void UpdatePos()
	{
		CameraController instance = CameraController.Instance;
		Transform followTarget = instance.followTarget;
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			NameDisplayScreen.Entry entry = this.entries[i];
			if (entry.visible)
			{
				GameObject world_go = entry.world_go;
				if (!(world_go == null))
				{
					Vector3 vector = world_go.transform.GetPosition();
					if (followTarget == world_go.transform)
					{
						vector = instance.followTargetPos;
					}
					else if (entry.world_go_anim_controller != null && entry.collider != null)
					{
						vector.x += entry.collider.offset.x;
						vector.y += entry.collider.offset.y - entry.collider.size.y / 2f;
					}
					entry.display_go_rect.anchoredPosition = (this.worldSpace ? vector : base.WorldToScreen(vector));
				}
			}
		}
	}

	// Token: 0x06006697 RID: 26263 RVA: 0x00265880 File Offset: 0x00263A80
	private void RemoveDestroyedEntries()
	{
		int num = this.entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.entries[i].world_go == null)
			{
				UnityEngine.Object.Destroy(this.entries[i].display_go);
				num--;
				this.entries[i] = this.entries[num];
			}
			else
			{
				i++;
			}
		}
		this.entries.RemoveRange(num, this.entries.Count - num);
	}

	// Token: 0x06006698 RID: 26264 RVA: 0x0026590C File Offset: 0x00263B0C
	private void HideDeadProgressBars()
	{
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.entries[i].visible && !(this.entries[i].world_go == null) && this.entries[i].kprfabID.HasTag(GameTags.Dead) && this.entries[i].bars_go.activeSelf)
			{
				this.entries[i].bars_go.SetActive(false);
			}
		}
	}

	// Token: 0x06006699 RID: 26265 RVA: 0x002659AC File Offset: 0x00263BAC
	public void UpdateName(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " character overlay";
		if (entry.nameLabel != null)
		{
			entry.nameLabel.text = component.GetProperName();
			if (representedObject.GetComponent<RocketModule>() != null)
			{
				entry.nameLabel.text = representedObject.GetComponent<RocketModule>().GetParentRocketName();
			}
		}
	}

	// Token: 0x0600669A RID: 26266 RVA: 0x00265A2C File Offset: 0x00263C2C
	public void SetDream(GameObject minion_go, Dream dream)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.SetDream(dream);
		entry.dreamBubble.GetComponent<KSelectable>().entityName = "Dreaming";
		entry.dreamBubble.gameObject.SetActive(true);
		entry.dreamBubble.SetVisibility(true);
	}

	// Token: 0x0600669B RID: 26267 RVA: 0x00265A94 File Offset: 0x00263C94
	public void StopDreaming(GameObject minion_go)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.StopDreaming();
		entry.dreamBubble.gameObject.SetActive(false);
	}

	// Token: 0x0600669C RID: 26268 RVA: 0x00265AD8 File Offset: 0x00263CD8
	public void DreamTick(GameObject minion_go, float dt)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.Tick(dt);
	}

	// Token: 0x0600669D RID: 26269 RVA: 0x00265B0C File Offset: 0x00263D0C
	public void SetThoughtBubbleDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubble, topic_sprite, "icon_sprite");
		entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubble.gameObject.SetActive(bVisible);
	}

	// Token: 0x0600669E RID: 26270 RVA: 0x00265B7C File Offset: 0x00263D7C
	public void SetThoughtBubbleConvoDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite, Sprite mode_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite, "icon_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite, "icon_sprite_mode");
		entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
	}

	// Token: 0x0600669F RID: 26271 RVA: 0x00265BFE File Offset: 0x00263DFE
	private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target)
	{
		active_bubble.GetReference<Image>(target).sprite = sprite;
	}

	// Token: 0x060066A0 RID: 26272 RVA: 0x00265C10 File Offset: 0x00263E10
	public void SetGameplayEventDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.gameplayEventDisplay == null)
		{
			return;
		}
		entry.gameplayEventDisplay.GetReference<Image>("icon_sprite").sprite = sprite;
		entry.gameplayEventDisplay.GetComponent<KSelectable>().entityName = hover_text;
		entry.gameplayEventDisplay.gameObject.SetActive(bVisible);
	}

	// Token: 0x060066A1 RID: 26273 RVA: 0x00265C70 File Offset: 0x00263E70
	public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.breathBar == null)
		{
			return;
		}
		entry.breathBar.SetUpdateFunc(updatePercentFull);
		entry.breathBar.SetVisibility(bVisible);
	}

	// Token: 0x060066A2 RID: 26274 RVA: 0x00265CB0 File Offset: 0x00263EB0
	public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.healthBar == null)
		{
			return;
		}
		entry.healthBar.OnChange();
		entry.healthBar.SetUpdateFunc(updatePercentFull);
		if (entry.healthBar.gameObject.activeSelf != bVisible)
		{
			entry.healthBar.SetVisibility(bVisible);
		}
	}

	// Token: 0x060066A3 RID: 26275 RVA: 0x00265D10 File Offset: 0x00263F10
	public void SetSuitTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBar == null)
		{
			return;
		}
		entry.suitBar.SetUpdateFunc(updatePercentFull);
		entry.suitBar.SetVisibility(bVisible);
	}

	// Token: 0x060066A4 RID: 26276 RVA: 0x00265D50 File Offset: 0x00263F50
	public void SetSuitFuelDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitFuelBar == null)
		{
			return;
		}
		entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
		entry.suitFuelBar.SetVisibility(bVisible);
	}

	// Token: 0x060066A5 RID: 26277 RVA: 0x00265D90 File Offset: 0x00263F90
	public void SetSuitBatteryDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBatteryBar == null)
		{
			return;
		}
		entry.suitBatteryBar.SetUpdateFunc(updatePercentFull);
		entry.suitBatteryBar.SetVisibility(bVisible);
	}

	// Token: 0x060066A6 RID: 26278 RVA: 0x00265DD0 File Offset: 0x00263FD0
	private NameDisplayScreen.Entry GetEntry(GameObject worldObject)
	{
		return this.entries.Find((NameDisplayScreen.Entry entry) => entry.world_go == worldObject);
	}

	// Token: 0x04004529 RID: 17705
	[SerializeField]
	private float HideDistance;

	// Token: 0x0400452A RID: 17706
	public static NameDisplayScreen Instance;

	// Token: 0x0400452B RID: 17707
	[SerializeField]
	private Canvas nameDisplayCanvas;

	// Token: 0x0400452C RID: 17708
	[SerializeField]
	private Canvas areaTextDisplayCanvas;

	// Token: 0x0400452D RID: 17709
	public GameObject nameAndBarsPrefab;

	// Token: 0x0400452E RID: 17710
	public GameObject barsPrefab;

	// Token: 0x0400452F RID: 17711
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04004530 RID: 17712
	[SerializeField]
	private Color selectedColor;

	// Token: 0x04004531 RID: 17713
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04004532 RID: 17714
	public int fontsize_min = 14;

	// Token: 0x04004533 RID: 17715
	public int fontsize_max = 32;

	// Token: 0x04004534 RID: 17716
	public float cameraDistance_fontsize_min = 6f;

	// Token: 0x04004535 RID: 17717
	public float cameraDistance_fontsize_max = 4f;

	// Token: 0x04004536 RID: 17718
	public List<NameDisplayScreen.Entry> entries = new List<NameDisplayScreen.Entry>();

	// Token: 0x04004537 RID: 17719
	public List<NameDisplayScreen.TextEntry> textEntries = new List<NameDisplayScreen.TextEntry>();

	// Token: 0x04004538 RID: 17720
	public bool worldSpace = true;

	// Token: 0x04004539 RID: 17721
	private bool isOverlayChangeBound;

	// Token: 0x0400453A RID: 17722
	private HashedString lastKnownOverlayID = OverlayModes.None.ID;

	// Token: 0x0400453B RID: 17723
	private int currentUpdateIndex;

	// Token: 0x02001DF9 RID: 7673
	[Serializable]
	public class Entry
	{
		// Token: 0x040088D4 RID: 35028
		public string Name;

		// Token: 0x040088D5 RID: 35029
		public bool visible;

		// Token: 0x040088D6 RID: 35030
		public GameObject world_go;

		// Token: 0x040088D7 RID: 35031
		public GameObject display_go;

		// Token: 0x040088D8 RID: 35032
		public GameObject bars_go;

		// Token: 0x040088D9 RID: 35033
		public KPrefabID kprfabID;

		// Token: 0x040088DA RID: 35034
		public KBoxCollider2D collider;

		// Token: 0x040088DB RID: 35035
		public KAnimControllerBase world_go_anim_controller;

		// Token: 0x040088DC RID: 35036
		public RectTransform display_go_rect;

		// Token: 0x040088DD RID: 35037
		public LocText nameLabel;

		// Token: 0x040088DE RID: 35038
		public HealthBar healthBar;

		// Token: 0x040088DF RID: 35039
		public ProgressBar breathBar;

		// Token: 0x040088E0 RID: 35040
		public ProgressBar suitBar;

		// Token: 0x040088E1 RID: 35041
		public ProgressBar suitFuelBar;

		// Token: 0x040088E2 RID: 35042
		public ProgressBar suitBatteryBar;

		// Token: 0x040088E3 RID: 35043
		public DreamBubble dreamBubble;

		// Token: 0x040088E4 RID: 35044
		public HierarchyReferences thoughtBubble;

		// Token: 0x040088E5 RID: 35045
		public HierarchyReferences thoughtBubbleConvo;

		// Token: 0x040088E6 RID: 35046
		public HierarchyReferences gameplayEventDisplay;

		// Token: 0x040088E7 RID: 35047
		public HierarchyReferences refs;
	}

	// Token: 0x02001DFA RID: 7674
	public class TextEntry
	{
		// Token: 0x040088E8 RID: 35048
		public Guid guid;

		// Token: 0x040088E9 RID: 35049
		public GameObject display_go;
	}
}
