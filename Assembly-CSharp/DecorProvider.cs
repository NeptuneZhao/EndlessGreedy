using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000835 RID: 2101
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/DecorProvider")]
public class DecorProvider : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003A62 RID: 14946 RVA: 0x0013F8B8 File Offset: 0x0013DAB8
	private void AddDecor()
	{
		this.currDecor = 0f;
		if (this.decor != null)
		{
			this.currDecor = this.decor.GetTotalValue();
		}
		if (this.prefabId.HasTag(GameTags.Stored))
		{
			this.currDecor = 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Transparent[num] && Grid.Solid[num] && this.simCellOccupier == null)
		{
			this.currDecor = 0f;
		}
		if (this.currDecor == 0f)
		{
			return;
		}
		this.cellCount = 0;
		int num2 = 5;
		if (this.decorRadius != null)
		{
			num2 = (int)this.decorRadius.GetTotalValue();
		}
		Extents extents = this.occupyArea.GetExtents();
		extents.x = Mathf.Max(extents.x - num2, 0);
		extents.y = Mathf.Max(extents.y - num2, 0);
		extents.width = Mathf.Min(extents.width + num2 * 2, Grid.WidthInCells - 1);
		extents.height = Mathf.Min(extents.height + num2 * 2, Grid.HeightInCells - 1);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatCollectDecorProviders", base.gameObject, extents, GameScenePartitioner.Instance.decorProviderLayer, this.onCollectDecorProvidersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatSolidCheck", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, this.refreshPartionerCallback);
		int num3 = extents.x + extents.width;
		int num4 = extents.y + extents.height;
		int num5 = extents.x;
		int num6 = extents.y;
		int x;
		int y;
		Grid.CellToXY(num, out x, out y);
		num3 = Math.Min(num3, Grid.WidthInCells);
		num4 = Math.Min(num4, Grid.HeightInCells);
		num5 = Math.Max(0, num5);
		num6 = Math.Max(0, num6);
		int num7 = (num3 - num5) * (num4 - num6);
		if (this.cells == null || this.cells.Length != num7)
		{
			this.cells = new int[num7];
		}
		for (int i = num5; i < num3; i++)
		{
			for (int j = num6; j < num4; j++)
			{
				if (Grid.VisibilityTest(x, y, i, j, false))
				{
					int num8 = Grid.XYToCell(i, j);
					if (Grid.IsValidCell(num8))
					{
						Grid.Decor[num8] += this.currDecor;
						int[] array = this.cells;
						int num9 = this.cellCount;
						this.cellCount = num9 + 1;
						array[num9] = num8;
					}
				}
			}
		}
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x0013FB4E File Offset: 0x0013DD4E
	public void Clear()
	{
		if (this.currDecor == 0f)
		{
			return;
		}
		this.RemoveDecor();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
	}

	// Token: 0x06003A64 RID: 14948 RVA: 0x0013FB84 File Offset: 0x0013DD84
	private void RemoveDecor()
	{
		if (this.currDecor == 0f)
		{
			return;
		}
		for (int i = 0; i < this.cellCount; i++)
		{
			int num = this.cells[i];
			if (Grid.IsValidCell(num))
			{
				Grid.Decor[num] -= this.currDecor;
			}
		}
	}

	// Token: 0x06003A65 RID: 14949 RVA: 0x0013FBD8 File Offset: 0x0013DDD8
	public void Refresh()
	{
		this.Clear();
		this.AddDecor();
		bool flag = this.prefabId.HasTag(RoomConstraints.ConstraintTags.Decor20);
		bool flag2 = this.decor.GetTotalValue() >= 20f;
		if (flag != flag2)
		{
			if (flag2)
			{
				this.prefabId.AddTag(RoomConstraints.ConstraintTags.Decor20, false);
			}
			else
			{
				this.prefabId.RemoveTag(RoomConstraints.ConstraintTags.Decor20);
			}
			int cell = Grid.PosToCell(this);
			if (Grid.IsValidCell(cell))
			{
				Game.Instance.roomProber.SolidChangedEvent(cell, true);
			}
		}
	}

	// Token: 0x06003A66 RID: 14950 RVA: 0x0013FC60 File Offset: 0x0013DE60
	public float GetDecorForCell(int cell)
	{
		for (int i = 0; i < this.cellCount; i++)
		{
			if (this.cells[i] == cell)
			{
				return this.currDecor;
			}
		}
		return 0f;
	}

	// Token: 0x06003A67 RID: 14951 RVA: 0x0013FC95 File Offset: 0x0013DE95
	public void SetValues(EffectorValues values)
	{
		this.baseDecor = (float)values.amount;
		this.baseRadius = (float)values.radius;
		if (base.IsInitialized())
		{
			this.UpdateBaseDecorModifiers();
		}
	}

	// Token: 0x06003A68 RID: 14952 RVA: 0x0013FCC0 File Offset: 0x0013DEC0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.decor = this.GetAttributes().Add(Db.Get().BuildingAttributes.Decor);
		this.decorRadius = this.GetAttributes().Add(Db.Get().BuildingAttributes.DecorRadius);
		this.UpdateBaseDecorModifiers();
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x0013FD1C File Offset: 0x0013DF1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectDecorProvidersCallback = new Action<object>(this.OnCollectDecorProviders);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "DecorProvider.OnSpawn");
		AttributeInstance attributeInstance = this.decor;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.decorRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		this.Refresh();
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x0013FDD0 File Offset: 0x0013DFD0
	private void UpdateBaseDecorModifiers()
	{
		Attributes attributes = this.GetAttributes();
		if (this.baseDecorModifier != null)
		{
			attributes.Remove(this.baseDecorModifier);
			attributes.Remove(this.baseDecorRadiusModifier);
			this.baseDecorModifier = null;
			this.baseDecorRadiusModifier = null;
		}
		if (this.baseDecor != 0f)
		{
			this.baseDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, this.baseDecor, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			this.baseDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, this.baseRadius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(this.baseDecorModifier);
			attributes.Add(this.baseDecorRadiusModifier);
		}
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x0013FE9B File Offset: 0x0013E09B
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x0013FEA3 File Offset: 0x0013E0A3
	private void OnCollectDecorProviders(object data)
	{
		((List<DecorProvider>)data).Add(this);
	}

	// Token: 0x06003A6D RID: 14957 RVA: 0x0013FEB1 File Offset: 0x0013E0B1
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.overrideName))
		{
			return base.GetComponent<KSelectable>().GetName();
		}
		return this.overrideName;
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x0013FED4 File Offset: 0x0013E0D4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			AttributeInstance attributeInstance = this.decor;
			attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
			AttributeInstance attributeInstance2 = this.decorRadius;
			attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		}
		this.Clear();
	}

	// Token: 0x06003A6F RID: 14959 RVA: 0x0013FF54 File Offset: 0x0013E154
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.decor != null && this.decorRadius != null)
		{
			float totalValue = this.decor.GetTotalValue();
			float totalValue2 = this.decorRadius.GetTotalValue();
			string arg = (this.baseDecor > 0f) ? "produced" : "consumed";
			string text = (this.baseDecor > 0f) ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED;
			text = text + "\n\n" + this.decor.GetAttributeValueTooltip();
			string text2 = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DECORPROVIDED, arg, text2, totalValue2), string.Format(text, text2, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.baseDecor != 0f)
		{
			string arg2 = (this.baseDecor >= 0f) ? "produced" : "consumed";
			string format = (this.baseDecor >= 0f) ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED;
			string text3 = GameUtil.AddPositiveSign(this.baseDecor.ToString(), this.baseDecor > 0f);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DECORPROVIDED, arg2, text3, this.baseRadius), string.Format(format, text3, this.baseRadius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06003A70 RID: 14960 RVA: 0x001400E9 File Offset: 0x0013E2E9
	public static int GetLightDecorBonus(int cell)
	{
		if (Grid.LightIntensity[cell] > 0)
		{
			return DECOR.LIT_BONUS;
		}
		return 0;
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x00140100 File Offset: 0x0013E300
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x0400231B RID: 8987
	public const string ID = "DecorProvider";

	// Token: 0x0400231C RID: 8988
	public float baseRadius;

	// Token: 0x0400231D RID: 8989
	public float baseDecor;

	// Token: 0x0400231E RID: 8990
	public string overrideName;

	// Token: 0x0400231F RID: 8991
	public System.Action refreshCallback;

	// Token: 0x04002320 RID: 8992
	public Action<object> refreshPartionerCallback;

	// Token: 0x04002321 RID: 8993
	public Action<object> onCollectDecorProvidersCallback;

	// Token: 0x04002322 RID: 8994
	public AttributeInstance decor;

	// Token: 0x04002323 RID: 8995
	public AttributeInstance decorRadius;

	// Token: 0x04002324 RID: 8996
	private AttributeModifier baseDecorModifier;

	// Token: 0x04002325 RID: 8997
	private AttributeModifier baseDecorRadiusModifier;

	// Token: 0x04002326 RID: 8998
	[MyCmpReq]
	private KPrefabID prefabId;

	// Token: 0x04002327 RID: 8999
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x04002328 RID: 9000
	[MyCmpGet]
	public SimCellOccupier simCellOccupier;

	// Token: 0x04002329 RID: 9001
	private int[] cells;

	// Token: 0x0400232A RID: 9002
	private int cellCount;

	// Token: 0x0400232B RID: 9003
	public float currDecor;

	// Token: 0x0400232C RID: 9004
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400232D RID: 9005
	private HandleVector<int>.Handle solidChangedPartitionerEntry;
}
