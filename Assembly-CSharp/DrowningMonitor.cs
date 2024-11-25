using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000801 RID: 2049
[AddComponentMenu("KMonoBehaviour/scripts/DrowningMonitor")]
public class DrowningMonitor : KMonoBehaviour, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x0600389C RID: 14492 RVA: 0x00134EB3 File Offset: 0x001330B3
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x0600389D RID: 14493 RVA: 0x00134ED5 File Offset: 0x001330D5
	public bool Drowning
	{
		get
		{
			return this.drowning;
		}
	}

	// Token: 0x0600389E RID: 14494 RVA: 0x00134EE0 File Offset: 0x001330E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeToDrown = 75f;
		if (DrowningMonitor.drowningEffect == null)
		{
			DrowningMonitor.drowningEffect = new Effect("Drowning", CREATURES.STATUSITEMS.DROWNING.NAME, CREATURES.STATUSITEMS.DROWNING.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.drowningEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.DROWNING.NAME, false, false, true));
		}
		if (DrowningMonitor.saturatedEffect == null)
		{
			DrowningMonitor.saturatedEffect = new Effect("Saturated", CREATURES.STATUSITEMS.SATURATED.NAME, CREATURES.STATUSITEMS.SATURATED.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.saturatedEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.SATURATED.NAME, false, false, true));
		}
	}

	// Token: 0x0600389F RID: 14495 RVA: 0x00134FF0 File Offset: 0x001331F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.RegisterUpdate1000ms(this);
		this.OnMove();
		this.CheckDrowning(null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMove), "DrowningMonitor.OnSpawn");
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x00135040 File Offset: 0x00133240
	private void OnMove()
	{
		if (this.partitionerEntry.IsValid())
		{
			Extents ext = this.occupyArea.GetExtents();
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, ext);
		}
		else
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDrowning(null);
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x001350BC File Offset: 0x001332BC
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMove));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x001350FC File Offset: 0x001332FC
	private void CheckDrowning(object data = null)
	{
		if (this.drowned)
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!this.IsCellSafe(cell))
		{
			if (!this.drowning)
			{
				this.drowning = true;
				base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Drowning, false);
				base.Trigger(1949704522, null);
			}
			if (this.timeToDrown <= 0f && this.canDrownToDeath)
			{
				DeathMonitor.Instance smi = this.GetSMI<DeathMonitor.Instance>();
				if (smi != null)
				{
					smi.Kill(Db.Get().Deaths.Drowned);
				}
				base.Trigger(-750750377, null);
				this.drowned = true;
			}
		}
		else if (this.drowning)
		{
			this.drowning = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Drowning);
			base.Trigger(99949694, null);
		}
		if (this.livesUnderWater)
		{
			this.saturatedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Saturated, this.saturatedStatusGuid, this.drowning, this);
		}
		else
		{
			this.drowningStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Drowning, this.drowningStatusGuid, this.drowning, this);
		}
		if (this.effects != null)
		{
			if (this.drowning)
			{
				if (this.livesUnderWater)
				{
					this.effects.Add(DrowningMonitor.saturatedEffect, false);
					return;
				}
				this.effects.Add(DrowningMonitor.drowningEffect, false);
				return;
			}
			else
			{
				if (this.livesUnderWater)
				{
					this.effects.Remove(DrowningMonitor.saturatedEffect);
					return;
				}
				this.effects.Remove(DrowningMonitor.drowningEffect);
			}
		}
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x001352A2 File Offset: 0x001334A2
	private static bool CellSafeTest(int testCell, object data)
	{
		return !Grid.IsNavigatableLiquid(testCell);
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x001352AD File Offset: 0x001334AD
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, this, DrowningMonitor.CellSafeTestDelegate);
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x060038A5 RID: 14501 RVA: 0x001352C1 File Offset: 0x001334C1
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Drowning
			};
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x060038A6 RID: 14502 RVA: 0x001352CD File Offset: 0x001334CD
	public string WiltStateString
	{
		get
		{
			if (this.livesUnderWater)
			{
				return "    • " + CREATURES.STATUSITEMS.SATURATED.NAME;
			}
			return "    • " + CREATURES.STATUSITEMS.DROWNING.NAME;
		}
	}

	// Token: 0x060038A7 RID: 14503 RVA: 0x00135300 File Offset: 0x00133500
	private void OnLiquidChanged(object data)
	{
		this.CheckDrowning(null);
	}

	// Token: 0x060038A8 RID: 14504 RVA: 0x0013530C File Offset: 0x0013350C
	public void SlicedSim1000ms(float dt)
	{
		this.CheckDrowning(null);
		if (this.drowning)
		{
			if (!this.drowned)
			{
				this.timeToDrown -= dt;
				if (this.timeToDrown <= 0f)
				{
					this.CheckDrowning(null);
					return;
				}
			}
		}
		else
		{
			this.timeToDrown += dt * 5f;
			this.timeToDrown = Mathf.Clamp(this.timeToDrown, 0f, 75f);
		}
	}

	// Token: 0x040021FF RID: 8703
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002200 RID: 8704
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04002201 RID: 8705
	private OccupyArea _occupyArea;

	// Token: 0x04002202 RID: 8706
	[Serialize]
	[SerializeField]
	private float timeToDrown;

	// Token: 0x04002203 RID: 8707
	[Serialize]
	private bool drowned;

	// Token: 0x04002204 RID: 8708
	private bool drowning;

	// Token: 0x04002205 RID: 8709
	protected const float MaxDrownTime = 75f;

	// Token: 0x04002206 RID: 8710
	protected const float RegenRate = 5f;

	// Token: 0x04002207 RID: 8711
	protected const float CellLiquidThreshold = 0.95f;

	// Token: 0x04002208 RID: 8712
	public bool canDrownToDeath = true;

	// Token: 0x04002209 RID: 8713
	public bool livesUnderWater;

	// Token: 0x0400220A RID: 8714
	private Guid drowningStatusGuid;

	// Token: 0x0400220B RID: 8715
	private Guid saturatedStatusGuid;

	// Token: 0x0400220C RID: 8716
	private Extents extents;

	// Token: 0x0400220D RID: 8717
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400220E RID: 8718
	public static Effect drowningEffect;

	// Token: 0x0400220F RID: 8719
	public static Effect saturatedEffect;

	// Token: 0x04002210 RID: 8720
	private static readonly Func<int, object, bool> CellSafeTestDelegate = (int testCell, object data) => DrowningMonitor.CellSafeTest(testCell, data);
}
