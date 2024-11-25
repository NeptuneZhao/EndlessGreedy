using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020006CF RID: 1743
public class FilteredStorage
{
	// Token: 0x06002C17 RID: 11287 RVA: 0x000F7B9F File Offset: 0x000F5D9F
	public void SetHasMeter(bool has_meter)
	{
		this.hasMeter = has_meter;
	}

	// Token: 0x06002C18 RID: 11288 RVA: 0x000F7BA8 File Offset: 0x000F5DA8
	public FilteredStorage(KMonoBehaviour root, Tag[] forbidden_tags, IUserControlledCapacity capacity_control, bool use_logic_meter, ChoreType fetch_chore_type)
	{
		this.root = root;
		this.forbiddenTags = forbidden_tags;
		this.capacityControl = capacity_control;
		this.useLogicMeter = use_logic_meter;
		this.choreType = fetch_chore_type;
		root.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		root.Subscribe(-543130682, new Action<object>(this.OnUserSettingsChanged));
		this.filterable = root.FindOrAdd<TreeFilterable>();
		TreeFilterable treeFilterable = this.filterable;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		this.storage = root.GetComponent<Storage>();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(-1852328367, new Action<object>(this.OnFunctionalChanged));
	}

	// Token: 0x06002C19 RID: 11289 RVA: 0x000F7C9B File Offset: 0x000F5E9B
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06002C1A RID: 11290 RVA: 0x000F7CB0 File Offset: 0x000F5EB0
	private void CreateMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.meter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x000F7CFF File Offset: 0x000F5EFF
	private void CreateLogicMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.logicMeter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06002C1C RID: 11292 RVA: 0x000F7D32 File Offset: 0x000F5F32
	public void SetMeter(MeterController meter)
	{
		this.hasMeter = true;
		this.meter = meter;
		this.UpdateMeter();
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x000F7D48 File Offset: 0x000F5F48
	public void CleanUp()
	{
		if (this.filterable != null)
		{
			TreeFilterable treeFilterable = this.filterable;
			treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		}
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("Parent destroyed");
		}
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x000F7DA4 File Offset: 0x000F5FA4
	public void FilterChanged()
	{
		if (this.hasMeter)
		{
			if (this.meter == null)
			{
				this.CreateMeter();
			}
			if (this.logicMeter == null && this.useLogicMeter)
			{
				this.CreateLogicMeter();
			}
		}
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x000F7DF4 File Offset: 0x000F5FF4
	private void OnUserSettingsChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x000F7E0D File Offset: 0x000F600D
	private void OnStorageChanged(object data)
	{
		if (this.fetchList == null)
		{
			this.OnFilterChanged(this.filterable.GetTags());
		}
		this.UpdateMeter();
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x000F7E2E File Offset: 0x000F602E
	private void OnFunctionalChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x000F7E44 File Offset: 0x000F6044
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x000F7E7C File Offset: 0x000F607C
	public bool IsFull()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float num = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
		return num >= 1f;
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x000F7EBD File Offset: 0x000F60BD
	private void OnFetchComplete()
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x000F7ED0 File Offset: 0x000F60D0
	private float GetMaxCapacity()
	{
		float num = this.storage.capacityKg;
		if (this.capacityControl != null)
		{
			num = Mathf.Min(num, this.capacityControl.UserMaxCapacity);
		}
		return num;
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x000F7F04 File Offset: 0x000F6104
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.GetMaxCapacity() - this.storage.storageFullMargin;
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x000F7F18 File Offset: 0x000F6118
	private float GetAmountStored()
	{
		float result = this.storage.MassStored();
		if (this.capacityControl != null)
		{
			result = this.capacityControl.AmountStored;
		}
		return result;
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x000F7F48 File Offset: 0x000F6148
	private bool IsFunctional()
	{
		Operational component = this.storage.GetComponent<Operational>();
		return component == null || component.IsFunctional;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x000F7F74 File Offset: 0x000F6174
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		bool flag = tags != null && tags.Count != 0;
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("");
			this.fetchList = null;
		}
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float amountStored = this.GetAmountStored();
		float num = Mathf.Max(0f, maxCapacityMinusStorageMargin - amountStored);
		if (num > 0f && flag && this.IsFunctional())
		{
			num = Mathf.Max(0f, this.GetMaxCapacity() - amountStored);
			this.fetchList = new FetchList2(this.storage, this.choreType);
			this.fetchList.ShowStatusItem = false;
			this.fetchList.Add(tags, this.requiredTag, this.forbiddenTags, num, Operational.State.Functional);
			this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
		}
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x000F8048 File Offset: 0x000F6248
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x000F806C File Offset: 0x000F626C
	public void SetRequiredTag(Tag tag)
	{
		if (this.requiredTag != tag)
		{
			this.requiredTag = tag;
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x000F8094 File Offset: 0x000F6294
	public void AddForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags == null)
		{
			this.forbiddenTags = new Tag[0];
		}
		if (!this.forbiddenTags.Contains(forbidden_tag))
		{
			this.forbiddenTags = this.forbiddenTags.Append(forbidden_tag);
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x000F80E8 File Offset: 0x000F62E8
	public void RemoveForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags != null)
		{
			List<Tag> list = new List<Tag>(this.forbiddenTags);
			list.Remove(forbidden_tag);
			this.forbiddenTags = list.ToArray();
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x0400196A RID: 6506
	public static readonly HashedString FULL_PORT_ID = "FULL";

	// Token: 0x0400196B RID: 6507
	private KMonoBehaviour root;

	// Token: 0x0400196C RID: 6508
	private FetchList2 fetchList;

	// Token: 0x0400196D RID: 6509
	private IUserControlledCapacity capacityControl;

	// Token: 0x0400196E RID: 6510
	private TreeFilterable filterable;

	// Token: 0x0400196F RID: 6511
	private Storage storage;

	// Token: 0x04001970 RID: 6512
	private MeterController meter;

	// Token: 0x04001971 RID: 6513
	private MeterController logicMeter;

	// Token: 0x04001972 RID: 6514
	private Tag requiredTag = Tag.Invalid;

	// Token: 0x04001973 RID: 6515
	private Tag[] forbiddenTags;

	// Token: 0x04001974 RID: 6516
	private bool hasMeter = true;

	// Token: 0x04001975 RID: 6517
	private bool useLogicMeter;

	// Token: 0x04001976 RID: 6518
	private ChoreType choreType;
}
