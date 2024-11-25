using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

// Token: 0x020007A9 RID: 1961
[AddComponentMenu("KMonoBehaviour/scripts/CellSelectionObject")]
public class CellSelectionObject : KMonoBehaviour
{
	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x060035A6 RID: 13734 RVA: 0x00123AE3 File Offset: 0x00121CE3
	public int SelectedCell
	{
		get
		{
			return this.selectedCell;
		}
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x060035A7 RID: 13735 RVA: 0x00123AEB File Offset: 0x00121CEB
	public float FlowRate
	{
		get
		{
			return Grid.AccumulatedFlow[this.selectedCell] / 3f;
		}
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x00123B04 File Offset: 0x00121D04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mCollider = base.GetComponent<KBoxCollider2D>();
		this.mCollider.size = new Vector2(1.1f, 1.1f);
		this.mSelectable = base.GetComponent<KSelectable>();
		this.SelectedDisplaySprite.transform.localScale = Vector3.one * 0.390625f;
		this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
		base.Subscribe(Game.Instance.gameObject, 493375141, new Action<object>(this.ForceRefreshUserMenu));
		this.overlayFilterMap.Add(OverlayModes.Oxygen.ID, () => Grid.Element[this.mouseCell].IsGas);
		this.overlayFilterMap.Add(OverlayModes.GasConduits.ID, () => Grid.Element[this.mouseCell].IsGas);
		this.overlayFilterMap.Add(OverlayModes.LiquidConduits.ID, () => Grid.Element[this.mouseCell].IsLiquid);
		if (CellSelectionObject.selectionObjectA == null)
		{
			CellSelectionObject.selectionObjectA = this;
			return;
		}
		if (CellSelectionObject.selectionObjectB == null)
		{
			CellSelectionObject.selectionObjectB = this;
			return;
		}
		global::Debug.LogError("CellSelectionObjects not properly cleaned up.");
	}

	// Token: 0x060035A9 RID: 13737 RVA: 0x00123C26 File Offset: 0x00121E26
	protected override void OnCleanUp()
	{
		CellSelectionObject.selectionObjectA = null;
		CellSelectionObject.selectionObjectB = null;
		base.OnCleanUp();
	}

	// Token: 0x060035AA RID: 13738 RVA: 0x00123C3A File Offset: 0x00121E3A
	public static bool IsSelectionObject(GameObject testObject)
	{
		return testObject == CellSelectionObject.selectionObjectA.gameObject || testObject == CellSelectionObject.selectionObjectB.gameObject;
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x00123C60 File Offset: 0x00121E60
	private void OnApplicationFocus(bool focusStatus)
	{
		this.isAppFocused = focusStatus;
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x00123C6C File Offset: 0x00121E6C
	private void Update()
	{
		if (!this.isAppFocused || SelectTool.Instance == null)
		{
			return;
		}
		if (Game.Instance == null || !Game.Instance.GameStarted())
		{
			return;
		}
		this.SelectedDisplaySprite.SetActive(PlayerController.Instance.IsUsingDefaultTool() && !DebugHandler.HideUI);
		if (SelectTool.Instance.selected != this.mSelectable)
		{
			this.mouseCell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(this.mouseCell) && Grid.IsVisible(this.mouseCell))
			{
				bool flag = true;
				foreach (KeyValuePair<HashedString, Func<bool>> keyValuePair in this.overlayFilterMap)
				{
					if (keyValuePair.Value == null)
					{
						global::Debug.LogWarning("Filter value is null");
					}
					else if (OverlayScreen.Instance == null)
					{
						global::Debug.LogWarning("Overlay screen Instance is null");
					}
					else if (OverlayScreen.Instance.GetMode() == keyValuePair.Key)
					{
						flag = false;
						if (base.gameObject.layer != LayerMask.NameToLayer("MaskedOverlay"))
						{
							base.gameObject.layer = LayerMask.NameToLayer("MaskedOverlay");
						}
						if (!keyValuePair.Value())
						{
							this.SelectedDisplaySprite.SetActive(false);
							return;
						}
						break;
					}
				}
				if (flag && base.gameObject.layer != LayerMask.NameToLayer("Default"))
				{
					base.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				Vector3 position = Grid.CellToPos(this.mouseCell, 0f, 0f, 0f) + this.offset;
				position.z = this.zDepth;
				base.transform.SetPosition(position);
				this.mSelectable.SetName(Grid.Element[this.mouseCell].name);
			}
			if (SelectTool.Instance.hover != this.mSelectable)
			{
				this.SelectedDisplaySprite.SetActive(false);
			}
		}
		this.updateTimer += Time.deltaTime;
		if (this.updateTimer >= 0.5f)
		{
			this.updateTimer = 0f;
			if (SelectTool.Instance.selected == this.mSelectable)
			{
				this.UpdateValues();
			}
		}
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x00123EF4 File Offset: 0x001220F4
	public void UpdateValues()
	{
		if (!Grid.IsValidCell(this.selectedCell))
		{
			return;
		}
		this.Mass = Grid.Mass[this.selectedCell];
		this.element = Grid.Element[this.selectedCell];
		this.ElementName = this.element.name;
		this.state = this.element.state;
		this.tags = this.element.GetMaterialCategoryTag();
		this.temperature = Grid.Temperature[this.selectedCell];
		this.diseaseIdx = Grid.DiseaseIdx[this.selectedCell];
		this.diseaseCount = Grid.DiseaseCount[this.selectedCell];
		this.mSelectable.SetName(Grid.Element[this.selectedCell].name);
		DetailsScreen.Instance.Trigger(-1514841199, null);
		this.UpdateStatusItem();
		int num = Grid.CellAbove(this.selectedCell);
		bool flag = this.element.IsLiquid && Grid.IsValidCell(num) && (Grid.Element[num].IsGas || Grid.Element[num].IsVacuum);
		if (this.element.sublimateId != (SimHashes)0 && (this.element.IsSolid || flag))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, this);
			bool flag2;
			bool flag3;
			GameUtil.IsEmissionBlocked(this.selectedCell, out flag2, out flag3);
			if (flag2)
			{
				this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, this);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
			}
			else if (flag3)
			{
				this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, this);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			}
			else
			{
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			}
		}
		else
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, false);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
		}
		if (Game.Instance.GetComponent<EntombedItemVisualizer>().IsEntombedItem(this.selectedCell))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BuriedItem, this);
		}
		else
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.BuriedItem, true);
		}
		bool on = CellSelectionObject.IsExposedToSpace(this.selectedCell);
		this.mSelectable.ToggleStatusItem(Db.Get().MiscStatusItems.Space, on, null);
	}

	// Token: 0x060035AE RID: 13742 RVA: 0x001241F6 File Offset: 0x001223F6
	public static bool IsExposedToSpace(int cell)
	{
		return Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space && Grid.Objects[cell, 2] == null;
	}

	// Token: 0x060035AF RID: 13743 RVA: 0x00124224 File Offset: 0x00122424
	private void UpdateStatusItem()
	{
		if (this.element.id == SimHashes.Vacuum || this.element.id == SimHashes.Void)
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalCategory, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalMass, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalDisease, true);
			return;
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalCategory))
		{
			Func<Element> data = () => this.element;
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, data);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalTemperature))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, this);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalMass))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalMass, this);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalDisease))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalDisease, this);
		}
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x001243B4 File Offset: 0x001225B4
	public void OnObjectSelected(object o)
	{
		this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
		this.UpdateStatusItem();
		if (SelectTool.Instance.selected == this.mSelectable)
		{
			this.selectedCell = Grid.PosToCell(base.gameObject);
			this.UpdateValues();
			Vector3 position = Grid.CellToPos(this.selectedCell, 0f, 0f, 0f) + this.offset;
			position.z = this.zDepthSelected;
			base.transform.SetPosition(position);
			this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Selected;
		}
	}

	// Token: 0x060035B1 RID: 13745 RVA: 0x00124461 File Offset: 0x00122661
	public string MassString()
	{
		return string.Format("{0:0.00}", this.Mass);
	}

	// Token: 0x060035B2 RID: 13746 RVA: 0x00124478 File Offset: 0x00122678
	private void ForceRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x04001FE5 RID: 8165
	private static CellSelectionObject selectionObjectA;

	// Token: 0x04001FE6 RID: 8166
	private static CellSelectionObject selectionObjectB;

	// Token: 0x04001FE7 RID: 8167
	[HideInInspector]
	public CellSelectionObject alternateSelectionObject;

	// Token: 0x04001FE8 RID: 8168
	private float zDepth = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection) - 0.5f;

	// Token: 0x04001FE9 RID: 8169
	private float zDepthSelected = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection);

	// Token: 0x04001FEA RID: 8170
	private KBoxCollider2D mCollider;

	// Token: 0x04001FEB RID: 8171
	private KSelectable mSelectable;

	// Token: 0x04001FEC RID: 8172
	private Vector3 offset = new Vector3(0.5f, 0.5f, 0f);

	// Token: 0x04001FED RID: 8173
	public GameObject SelectedDisplaySprite;

	// Token: 0x04001FEE RID: 8174
	public Sprite Sprite_Selected;

	// Token: 0x04001FEF RID: 8175
	public Sprite Sprite_Hover;

	// Token: 0x04001FF0 RID: 8176
	public int mouseCell;

	// Token: 0x04001FF1 RID: 8177
	private int selectedCell;

	// Token: 0x04001FF2 RID: 8178
	public string ElementName;

	// Token: 0x04001FF3 RID: 8179
	public Element element;

	// Token: 0x04001FF4 RID: 8180
	public Element.State state;

	// Token: 0x04001FF5 RID: 8181
	public float Mass;

	// Token: 0x04001FF6 RID: 8182
	public float temperature;

	// Token: 0x04001FF7 RID: 8183
	public Tag tags;

	// Token: 0x04001FF8 RID: 8184
	public byte diseaseIdx;

	// Token: 0x04001FF9 RID: 8185
	public int diseaseCount;

	// Token: 0x04001FFA RID: 8186
	private float updateTimer;

	// Token: 0x04001FFB RID: 8187
	private Dictionary<HashedString, Func<bool>> overlayFilterMap = new Dictionary<HashedString, Func<bool>>();

	// Token: 0x04001FFC RID: 8188
	private bool isAppFocused = true;
}
