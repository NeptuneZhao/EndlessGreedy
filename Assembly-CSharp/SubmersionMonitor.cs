using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000823 RID: 2083
[AddComponentMenu("KMonoBehaviour/scripts/SubmersionMonitor")]
public class SubmersionMonitor : KMonoBehaviour, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06003993 RID: 14739 RVA: 0x0013A01F File Offset: 0x0013821F
	public bool Dry
	{
		get
		{
			return this.dry;
		}
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x0013A027 File Offset: 0x00138227
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnMove();
		this.CheckDry();
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMove), "SubmersionMonitor.OnSpawn");
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x0013A060 File Offset: 0x00138260
	private void OnMove()
	{
		this.position = Grid.PosToCell(base.gameObject);
		if (this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.position);
		}
		else
		{
			Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
			Extents extents = new Extents(vector2I.x, vector2I.y, 1, 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDry();
	}

	// Token: 0x06003996 RID: 14742 RVA: 0x0013A101 File Offset: 0x00138301
	private void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x0013A103 File Offset: 0x00138303
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMove));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x0013A137 File Offset: 0x00138337
	public void Configure(float _maxStamina, float _staminaRegenRate, float _cellLiquidThreshold = 0.95f)
	{
		this.cellLiquidThreshold = _cellLiquidThreshold;
	}

	// Token: 0x06003999 RID: 14745 RVA: 0x0013A140 File Offset: 0x00138340
	public void Sim1000ms(float dt)
	{
		this.CheckDry();
	}

	// Token: 0x0600399A RID: 14746 RVA: 0x0013A148 File Offset: 0x00138348
	private void CheckDry()
	{
		if (!this.IsCellSafe())
		{
			if (!this.dry)
			{
				this.dry = true;
				base.Trigger(-2057657673, null);
				return;
			}
		}
		else if (this.dry)
		{
			this.dry = false;
			base.Trigger(1555379996, null);
		}
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x0013A194 File Offset: 0x00138394
	public bool IsCellSafe()
	{
		int cell = Grid.PosToCell(base.gameObject);
		return Grid.IsValidCell(cell) && Grid.IsSubstantialLiquid(cell, this.cellLiquidThreshold);
	}

	// Token: 0x0600399C RID: 14748 RVA: 0x0013A1C8 File Offset: 0x001383C8
	private void OnLiquidChanged(object data)
	{
		this.CheckDry();
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x0600399D RID: 14749 RVA: 0x0013A1D0 File Offset: 0x001383D0
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.DryingOut
			};
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x0600399E RID: 14750 RVA: 0x0013A1DC File Offset: 0x001383DC
	public string WiltStateString
	{
		get
		{
			if (this.Dry)
			{
				return Db.Get().CreatureStatusItems.DryingOut.resolveStringCallback(CREATURES.STATUSITEMS.DRYINGOUT.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x0600399F RID: 14751 RVA: 0x0013A210 File Offset: 0x00138410
	public void SetIncapacitated(bool state)
	{
	}

	// Token: 0x060039A0 RID: 14752 RVA: 0x0013A212 File Offset: 0x00138412
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_SUBMERSION, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_SUBMERSION, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x040022A4 RID: 8868
	private int position;

	// Token: 0x040022A5 RID: 8869
	private bool dry;

	// Token: 0x040022A6 RID: 8870
	protected float cellLiquidThreshold = 0.2f;

	// Token: 0x040022A7 RID: 8871
	private Extents extents;

	// Token: 0x040022A8 RID: 8872
	private HandleVector<int>.Handle partitionerEntry;
}
