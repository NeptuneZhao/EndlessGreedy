using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D22 RID: 3362
public class ResearchScreen : KModalScreen
{
	// Token: 0x06006911 RID: 26897 RVA: 0x00275F7F File Offset: 0x0027417F
	public bool IsBeingResearched(Tech tech)
	{
		return Research.Instance.IsBeingResearched(tech);
	}

	// Token: 0x06006912 RID: 26898 RVA: 0x00275F8C File Offset: 0x0027418C
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x06006913 RID: 26899 RVA: 0x00275FA4 File Offset: 0x002741A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		Transform transform = base.transform;
		while (this.m_Raycaster == null)
		{
			this.m_Raycaster = transform.GetComponent<GraphicRaycaster>();
			if (this.m_Raycaster == null)
			{
				transform = transform.parent;
			}
		}
	}

	// Token: 0x06006914 RID: 26900 RVA: 0x00275FF6 File Offset: 0x002741F6
	private void ZoomOut()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x06006915 RID: 26901 RVA: 0x00276023 File Offset: 0x00274223
	private void ZoomIn()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x06006916 RID: 26902 RVA: 0x00276050 File Offset: 0x00274250
	public void ZoomToTech(string techID)
	{
		Vector2 a = this.entryMap[Db.Get().Techs.Get(techID)].rectTransform().GetLocalPosition() + new Vector2(-this.foreground.rectTransform().rect.size.x / 2f, this.foreground.rectTransform().rect.size.y / 2f);
		this.forceTargetPosition = -a;
		this.zoomingToTarget = true;
		this.targetZoom = this.maxZoom;
	}

	// Token: 0x06006917 RID: 26903 RVA: 0x002760F8 File Offset: 0x002742F8
	private void Update()
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		RectTransform component = this.scrollContent.GetComponent<RectTransform>();
		if (this.isDragging && !KInputManager.isFocused)
		{
			this.AbortDragging();
		}
		Vector2 anchoredPosition = component.anchoredPosition;
		float t = Mathf.Min(this.effectiveZoomSpeed * Time.unscaledDeltaTime, 0.9f);
		this.currentZoom = Mathf.Lerp(this.currentZoom, this.targetZoom, t);
		Vector2 b = Vector2.zero;
		Vector2 v = KInputManager.GetMousePos();
		Vector2 b2 = this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom);
		component.localScale = new Vector3(this.currentZoom, this.currentZoom, 1f);
		b = (this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom)) - b2;
		float d = this.keyboardScrollSpeed;
		if (this.panUp)
		{
			this.keyPanDelta -= Vector2.up * Time.unscaledDeltaTime * d;
		}
		else if (this.panDown)
		{
			this.keyPanDelta += Vector2.up * Time.unscaledDeltaTime * d;
		}
		if (this.panLeft)
		{
			this.keyPanDelta += Vector2.right * Time.unscaledDeltaTime * d;
		}
		else if (this.panRight)
		{
			this.keyPanDelta -= Vector2.right * Time.unscaledDeltaTime * d;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			Vector2 a = KInputManager.steamInputInterpreter.GetSteamCameraMovement();
			a *= -1f;
			this.keyPanDelta = a * Time.unscaledDeltaTime * d * 2f;
		}
		Vector2 b3 = new Vector2(Mathf.Lerp(0f, this.keyPanDelta.x, Time.unscaledDeltaTime * this.keyPanEasing), Mathf.Lerp(0f, this.keyPanDelta.y, Time.unscaledDeltaTime * this.keyPanEasing));
		this.keyPanDelta -= b3;
		Vector2 vector = Vector2.zero;
		if (this.isDragging)
		{
			Vector2 b4 = KInputManager.GetMousePos() - this.dragLastPosition;
			vector += b4;
			this.dragLastPosition = KInputManager.GetMousePos();
			this.dragInteria = Vector2.ClampMagnitude(this.dragInteria + b4, 400f);
		}
		this.dragInteria *= Mathf.Max(0f, 1f - Time.unscaledDeltaTime * 4f);
		Vector2 vector2 = anchoredPosition + b + this.keyPanDelta + vector;
		if (!this.isDragging)
		{
			Vector2 size = base.GetComponent<RectTransform>().rect.size;
			Vector2 vector3 = new Vector2((-component.rect.size.x / 2f - 250f) * this.currentZoom, -250f * this.currentZoom);
			Vector2 vector4 = new Vector2(250f * this.currentZoom, (component.rect.size.y + 250f) * this.currentZoom - size.y);
			Vector2 a2 = new Vector2(Mathf.Clamp(vector2.x, vector3.x, vector4.x), Mathf.Clamp(vector2.y, vector3.y, vector4.y));
			this.forceTargetPosition = new Vector2(Mathf.Clamp(this.forceTargetPosition.x, vector3.x, vector4.x), Mathf.Clamp(this.forceTargetPosition.y, vector3.y, vector4.y));
			Vector2 vector5 = a2 + this.dragInteria - vector2;
			if (!this.panLeft && !this.panRight && !this.panUp && !this.panDown)
			{
				vector2 += vector5 * this.edgeClampFactor * Time.unscaledDeltaTime;
			}
			else
			{
				vector2 += vector5;
				if (vector5.x < 0f)
				{
					this.keyPanDelta.x = Mathf.Min(0f, this.keyPanDelta.x);
				}
				if (vector5.x > 0f)
				{
					this.keyPanDelta.x = Mathf.Max(0f, this.keyPanDelta.x);
				}
				if (vector5.y < 0f)
				{
					this.keyPanDelta.y = Mathf.Min(0f, this.keyPanDelta.y);
				}
				if (vector5.y > 0f)
				{
					this.keyPanDelta.y = Mathf.Max(0f, this.keyPanDelta.y);
				}
			}
		}
		if (this.zoomingToTarget)
		{
			vector2 = Vector2.Lerp(vector2, this.forceTargetPosition, Time.unscaledDeltaTime * 4f);
			if (Vector3.Distance(vector2, this.forceTargetPosition) < 1f || this.isDragging || this.panLeft || this.panRight || this.panUp || this.panDown)
			{
				this.zoomingToTarget = false;
			}
		}
		component.anchoredPosition = vector2;
	}

	// Token: 0x06006918 RID: 26904 RVA: 0x002766F4 File Offset: 0x002748F4
	protected override void OnSpawn()
	{
		base.Subscribe(Research.Instance.gameObject, -1914338957, new Action<object>(this.OnActiveResearchChanged));
		base.Subscribe(Game.Instance.gameObject, -107300940, new Action<object>(this.OnResearchComplete));
		base.Subscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Show(false);
		});
		this.pointDisplayMap = new Dictionary<string, LocText>();
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id] = Util.KInstantiateUI(this.pointDisplayCountPrefab, this.pointDisplayContainer, true).GetComponentInChildren<LocText>();
			this.pointDisplayMap[researchType.id].text = Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString();
			this.pointDisplayMap[researchType.id].transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(researchType.description);
			this.pointDisplayMap[researchType.id].transform.parent.GetComponentInChildren<Image>().sprite = researchType.sprite;
		}
		this.pointDisplayContainer.transform.parent.gameObject.SetActive(Research.Instance.UseGlobalPointInventory);
		this.entryMap = new Dictionary<Tech, ResearchEntry>();
		List<Tech> resources = Db.Get().Techs.resources;
		resources.Sort((Tech x, Tech y) => y.center.y.CompareTo(x.center.y));
		List<TechTreeTitle> resources2 = Db.Get().TechTreeTitles.resources;
		resources2.Sort((TechTreeTitle x, TechTreeTitle y) => y.center.y.CompareTo(x.center.y));
		float x3 = 0f;
		float y3 = 125f;
		Vector2 b = new Vector2(x3, y3);
		for (int i = 0; i < resources2.Count; i++)
		{
			ResearchTreeTitle researchTreeTitle = Util.KInstantiateUI<ResearchTreeTitle>(this.researchTreeTitlePrefab.gameObject, this.treeTitles, false);
			TechTreeTitle techTreeTitle = resources2[i];
			researchTreeTitle.name = techTreeTitle.Name + " Title";
			Vector3 vector = techTreeTitle.center + b;
			researchTreeTitle.transform.rectTransform().anchoredPosition = vector;
			float num = techTreeTitle.height;
			if (i + 1 < resources2.Count)
			{
				TechTreeTitle techTreeTitle2 = resources2[i + 1];
				Vector3 vector2 = techTreeTitle2.center + b;
				num += vector.y - (vector2.y + techTreeTitle2.height);
			}
			else
			{
				num += 600f;
			}
			researchTreeTitle.transform.rectTransform().sizeDelta = new Vector2(techTreeTitle.width, num);
			researchTreeTitle.SetLabel(techTreeTitle.Name);
			researchTreeTitle.SetColor(i);
		}
		List<Vector2> list = new List<Vector2>();
		float x2 = 0f;
		float y2 = 0f;
		Vector2 b2 = new Vector2(x2, y2);
		for (int j = 0; j < resources.Count; j++)
		{
			ResearchEntry researchEntry = Util.KInstantiateUI<ResearchEntry>(this.entryPrefab.gameObject, this.scrollContent, false);
			Tech tech = resources[j];
			researchEntry.name = tech.Name + " Panel";
			Vector3 v = tech.center + b2;
			researchEntry.transform.rectTransform().anchoredPosition = v;
			researchEntry.transform.rectTransform().sizeDelta = new Vector2(tech.width, tech.height);
			this.entryMap.Add(tech, researchEntry);
			if (tech.edges.Count > 0)
			{
				for (int k = 0; k < tech.edges.Count; k++)
				{
					ResourceTreeNode.Edge edge = tech.edges[k];
					if (edge.path == null)
					{
						list.AddRange(edge.SrcTarget);
					}
					else
					{
						ResourceTreeNode.Edge.EdgeType edgeType = edge.edgeType;
						if (edgeType <= ResourceTreeNode.Edge.EdgeType.QuadCurveEdge || edgeType - ResourceTreeNode.Edge.EdgeType.BezierEdge <= 1)
						{
							list.Add(edge.SrcTarget[0]);
							list.Add(edge.path[0]);
							for (int l = 1; l < edge.path.Count; l++)
							{
								list.Add(edge.path[l - 1]);
								list.Add(edge.path[l]);
							}
							list.Add(edge.path[edge.path.Count - 1]);
							list.Add(edge.SrcTarget[1]);
						}
						else
						{
							list.AddRange(edge.path);
						}
					}
				}
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			list[m] = new Vector2(list[m].x, list[m].y + this.foreground.transform.rectTransform().rect.height);
		}
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetTech(keyValuePair.Key);
		}
		this.CloseButton.soundPlayer.Enabled = false;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		base.StartCoroutine(this.WaitAndSetActiveResearch());
		base.OnSpawn();
		this.scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(250f, -250f);
		this.zoomOutButton.onClick += delegate()
		{
			this.ZoomOut();
		};
		this.zoomInButton.onClick += delegate()
		{
			this.ZoomIn();
		};
		base.gameObject.SetActive(true);
		this.Show(false);
	}

	// Token: 0x06006919 RID: 26905 RVA: 0x00276D9C File Offset: 0x00274F9C
	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		this.isDragging = true;
	}

	// Token: 0x0600691A RID: 26906 RVA: 0x00276DAC File Offset: 0x00274FAC
	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		this.AbortDragging();
	}

	// Token: 0x0600691B RID: 26907 RVA: 0x00276DBB File Offset: 0x00274FBB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Deactivate();
		});
	}

	// Token: 0x0600691C RID: 26908 RVA: 0x00276DE4 File Offset: 0x00274FE4
	private IEnumerator WaitAndSetActiveResearch()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		TechInstance targetResearch = Research.Instance.GetTargetResearch();
		if (targetResearch != null)
		{
			this.SetActiveResearch(targetResearch.tech);
		}
		yield break;
	}

	// Token: 0x0600691D RID: 26909 RVA: 0x00276DF3 File Offset: 0x00274FF3
	public Vector3 GetEntryPosition(Tech tech)
	{
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return Vector3.zero;
		}
		return this.entryMap[tech].transform.GetPosition();
	}

	// Token: 0x0600691E RID: 26910 RVA: 0x00276E29 File Offset: 0x00275029
	public ResearchEntry GetEntry(Tech tech)
	{
		if (this.entryMap == null)
		{
			return null;
		}
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return null;
		}
		return this.entryMap[tech];
	}

	// Token: 0x0600691F RID: 26911 RVA: 0x00276E5C File Offset: 0x0027505C
	public void SetEntryPercentage(Tech tech, float percent)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.SetPercentage(percent);
		}
	}

	// Token: 0x06006920 RID: 26912 RVA: 0x00276E84 File Offset: 0x00275084
	public void TurnEverythingOff()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOff();
		}
	}

	// Token: 0x06006921 RID: 26913 RVA: 0x00276EDC File Offset: 0x002750DC
	public void TurnEverythingOn()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOn();
		}
	}

	// Token: 0x06006922 RID: 26914 RVA: 0x00276F34 File Offset: 0x00275134
	private void SelectAllEntries(Tech tech, bool isSelected)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.QueueStateChanged(isSelected);
		}
		foreach (Tech tech2 in tech.requiredTech)
		{
			this.SelectAllEntries(tech2, isSelected);
		}
	}

	// Token: 0x06006923 RID: 26915 RVA: 0x00276FA0 File Offset: 0x002751A0
	private void OnResearchComplete(object data)
	{
		if (data is Tech)
		{
			Tech tech = (Tech)data;
			ResearchEntry entry = this.GetEntry(tech);
			if (entry != null)
			{
				entry.ResearchCompleted(true);
			}
			this.UpdateProgressBars();
			this.UpdatePointDisplay();
		}
	}

	// Token: 0x06006924 RID: 26916 RVA: 0x00276FE0 File Offset: 0x002751E0
	private void UpdatePointDisplay()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id].text = string.Format("{0}: {1}", Research.Instance.researchTypes.GetResearchType(researchType.id).name, Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString());
		}
	}

	// Token: 0x06006925 RID: 26917 RVA: 0x00277094 File Offset: 0x00275294
	private void OnActiveResearchChanged(object data)
	{
		List<TechInstance> list = (List<TechInstance>)data;
		foreach (TechInstance techInstance in list)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(true);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
		if (list.Count > 0)
		{
			this.currentResearch = list[list.Count - 1].tech;
		}
	}

	// Token: 0x06006926 RID: 26918 RVA: 0x00277130 File Offset: 0x00275330
	private void UpdateProgressBars()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.UpdateProgressBars();
		}
	}

	// Token: 0x06006927 RID: 26919 RVA: 0x00277188 File Offset: 0x00275388
	public void CancelResearch()
	{
		List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
		foreach (TechInstance techInstance in researchQueue)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(false);
			}
		}
		researchQueue.Clear();
	}

	// Token: 0x06006928 RID: 26920 RVA: 0x00277200 File Offset: 0x00275400
	private void SetActiveResearch(Tech newResearch)
	{
		if (newResearch != this.currentResearch && this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, false);
		}
		this.currentResearch = newResearch;
		if (this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, true);
		}
	}

	// Token: 0x06006929 RID: 26921 RVA: 0x0027723C File Offset: 0x0027543C
	public override void Show(bool show = true)
	{
		this.mouseOver = false;
		this.scrollContentChildFitter.enabled = show;
		foreach (Canvas canvas in base.GetComponentsInChildren<Canvas>(true))
		{
			if (canvas.enabled != show)
			{
				canvas.enabled = show;
			}
		}
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null)
		{
			component.interactable = show;
			component.blocksRaycasts = show;
			component.ignoreParentGroups = true;
		}
		this.OnShow(show);
	}

	// Token: 0x0600692A RID: 26922 RVA: 0x002772B4 File Offset: 0x002754B4
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.sideBar.ResetFilter();
		}
		if (show)
		{
			CameraController.Instance.DisableUserCameraControl = true;
			if (DetailsScreen.Instance != null)
			{
				DetailsScreen.Instance.gameObject.SetActive(false);
			}
		}
		else
		{
			CameraController.Instance.DisableUserCameraControl = false;
			if (SelectTool.Instance.selected != null && !DetailsScreen.Instance.gameObject.activeSelf)
			{
				DetailsScreen.Instance.gameObject.SetActive(true);
				DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
	}

	// Token: 0x0600692B RID: 26923 RVA: 0x00277366 File Offset: 0x00275566
	private void AbortDragging()
	{
		this.isDragging = false;
		this.draggingJustEnded = true;
	}

	// Token: 0x0600692C RID: 26924 RVA: 0x00277376 File Offset: 0x00275576
	private void LateUpdate()
	{
		this.draggingJustEnded = false;
	}

	// Token: 0x0600692D RID: 26925 RVA: 0x00277380 File Offset: 0x00275580
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		if (!e.Consumed)
		{
			if (e.IsAction(global::Action.MouseRight) && !this.isDragging && !this.draggingJustEnded)
			{
				ManagementMenu.Instance.CloseAll();
			}
			if (e.IsAction(global::Action.MouseRight) || e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.MouseMiddle))
			{
				this.AbortDragging();
			}
			if (this.panUp && e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (this.panDown && e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
				return;
			}
			if (this.panRight && e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (this.panLeft && e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
		}
		base.OnKeyUp(e);
	}

	// Token: 0x0600692E RID: 26926 RVA: 0x00277468 File Offset: 0x00275668
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		if (!e.Consumed)
		{
			if (e.TryConsume(global::Action.MouseRight))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (e.TryConsume(global::Action.MouseLeft))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (KInputManager.GetMousePos().x > this.sideBar.rectTransform().sizeDelta.x && CameraController.IsMouseOverGameWindow)
			{
				if (e.TryConsume(global::Action.ZoomIn))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
				if (e.TryConsume(global::Action.ZoomOut))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
			}
			if (e.TryConsume(global::Action.Escape))
			{
				ManagementMenu.Instance.CloseAll();
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600692F RID: 26927 RVA: 0x002775D0 File Offset: 0x002757D0
	public static bool TechPassesSearchFilter(string techID, string filterString)
	{
		if (!string.IsNullOrEmpty(filterString))
		{
			filterString = filterString.ToUpper();
			bool flag = false;
			Tech tech = Db.Get().Techs.Get(techID);
			flag = UI.StripLinkFormatting(tech.Name).ToLower().ToUpper().Contains(filterString);
			if (!flag)
			{
				flag = tech.category.ToUpper().Contains(filterString);
				foreach (TechItem techItem in tech.unlockedItems)
				{
					if (SaveLoader.Instance.IsCorrectDlcActiveForCurrentSave(techItem.requiredDlcIds, techItem.forbiddenDlcIds))
					{
						if (UI.StripLinkFormatting(techItem.Name).ToLower().ToUpper().Contains(filterString))
						{
							flag = true;
							break;
						}
						if (UI.StripLinkFormatting(techItem.description).ToLower().ToUpper().Contains(filterString))
						{
							flag = true;
							break;
						}
					}
				}
			}
			return flag;
		}
		return true;
	}

	// Token: 0x06006930 RID: 26928 RVA: 0x002776D4 File Offset: 0x002758D4
	public static bool TechItemPassesSearchFilter(string techItemID, string filterString)
	{
		if (!string.IsNullOrEmpty(filterString))
		{
			filterString = filterString.ToUpper();
			TechItem techItem = Db.Get().TechItems.Get(techItemID);
			bool flag = UI.StripLinkFormatting(techItem.Name).ToLower().ToUpper().Contains(filterString);
			if (!flag)
			{
				flag = techItem.Name.ToUpper().Contains(filterString);
				flag = (flag && techItem.description.ToUpper().Contains(filterString));
			}
			return flag;
		}
		return true;
	}

	// Token: 0x04004747 RID: 18247
	private const float SCROLL_BUFFER = 250f;

	// Token: 0x04004748 RID: 18248
	[SerializeField]
	private Image BG;

	// Token: 0x04004749 RID: 18249
	public ResearchEntry entryPrefab;

	// Token: 0x0400474A RID: 18250
	public ResearchTreeTitle researchTreeTitlePrefab;

	// Token: 0x0400474B RID: 18251
	public GameObject foreground;

	// Token: 0x0400474C RID: 18252
	public GameObject scrollContent;

	// Token: 0x0400474D RID: 18253
	public GameObject treeTitles;

	// Token: 0x0400474E RID: 18254
	public GameObject pointDisplayCountPrefab;

	// Token: 0x0400474F RID: 18255
	public GameObject pointDisplayContainer;

	// Token: 0x04004750 RID: 18256
	private Dictionary<string, LocText> pointDisplayMap;

	// Token: 0x04004751 RID: 18257
	private Dictionary<Tech, ResearchEntry> entryMap;

	// Token: 0x04004752 RID: 18258
	[SerializeField]
	private KButton zoomOutButton;

	// Token: 0x04004753 RID: 18259
	[SerializeField]
	private KButton zoomInButton;

	// Token: 0x04004754 RID: 18260
	[SerializeField]
	private ResearchScreenSideBar sideBar;

	// Token: 0x04004755 RID: 18261
	private Tech currentResearch;

	// Token: 0x04004756 RID: 18262
	public KButton CloseButton;

	// Token: 0x04004757 RID: 18263
	private GraphicRaycaster m_Raycaster;

	// Token: 0x04004758 RID: 18264
	private PointerEventData m_PointerEventData;

	// Token: 0x04004759 RID: 18265
	private Vector3 currentScrollPosition;

	// Token: 0x0400475A RID: 18266
	private bool panUp;

	// Token: 0x0400475B RID: 18267
	private bool panDown;

	// Token: 0x0400475C RID: 18268
	private bool panLeft;

	// Token: 0x0400475D RID: 18269
	private bool panRight;

	// Token: 0x0400475E RID: 18270
	[SerializeField]
	private KChildFitter scrollContentChildFitter;

	// Token: 0x0400475F RID: 18271
	private bool isDragging;

	// Token: 0x04004760 RID: 18272
	private Vector3 dragStartPosition;

	// Token: 0x04004761 RID: 18273
	private Vector3 dragLastPosition;

	// Token: 0x04004762 RID: 18274
	private Vector2 dragInteria;

	// Token: 0x04004763 RID: 18275
	private Vector2 forceTargetPosition;

	// Token: 0x04004764 RID: 18276
	private bool zoomingToTarget;

	// Token: 0x04004765 RID: 18277
	private bool draggingJustEnded;

	// Token: 0x04004766 RID: 18278
	private float targetZoom = 1f;

	// Token: 0x04004767 RID: 18279
	private float currentZoom = 1f;

	// Token: 0x04004768 RID: 18280
	private bool zoomCenterLock;

	// Token: 0x04004769 RID: 18281
	private Vector2 keyPanDelta = Vector3.zero;

	// Token: 0x0400476A RID: 18282
	[SerializeField]
	private float effectiveZoomSpeed = 5f;

	// Token: 0x0400476B RID: 18283
	[SerializeField]
	private float zoomAmountPerScroll = 0.05f;

	// Token: 0x0400476C RID: 18284
	[SerializeField]
	private float zoomAmountPerButton = 0.5f;

	// Token: 0x0400476D RID: 18285
	[SerializeField]
	private float minZoom = 0.15f;

	// Token: 0x0400476E RID: 18286
	[SerializeField]
	private float maxZoom = 1f;

	// Token: 0x0400476F RID: 18287
	[SerializeField]
	private float keyboardScrollSpeed = 200f;

	// Token: 0x04004770 RID: 18288
	[SerializeField]
	private float keyPanEasing = 1f;

	// Token: 0x04004771 RID: 18289
	[SerializeField]
	private float edgeClampFactor = 0.5f;

	// Token: 0x02001E44 RID: 7748
	public enum ResearchState
	{
		// Token: 0x040089F4 RID: 35316
		Available,
		// Token: 0x040089F5 RID: 35317
		ActiveResearch,
		// Token: 0x040089F6 RID: 35318
		ResearchComplete,
		// Token: 0x040089F7 RID: 35319
		MissingPrerequisites,
		// Token: 0x040089F8 RID: 35320
		StateCount
	}
}
