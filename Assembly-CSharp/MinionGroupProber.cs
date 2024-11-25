using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000963 RID: 2403
[AddComponentMenu("KMonoBehaviour/scripts/MinionGroupProber")]
public class MinionGroupProber : KMonoBehaviour, IGroupProber, ISim200ms
{
	// Token: 0x0600463C RID: 17980 RVA: 0x0019074C File Offset: 0x0018E94C
	public static void DestroyInstance()
	{
		MinionGroupProber.Instance = null;
	}

	// Token: 0x0600463D RID: 17981 RVA: 0x00190754 File Offset: 0x0018E954
	public static MinionGroupProber Get()
	{
		return MinionGroupProber.Instance;
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x0019075C File Offset: 0x0018E95C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MinionGroupProber.Instance = this;
		this.cells = new Dictionary<object, short>[Grid.CellCount];
		for (int i = 0; i < Grid.CellCount; i++)
		{
			this.cells[i] = new Dictionary<object, short>();
		}
		this.cell_cleanup_index = 0;
		this.cell_checks_per_frame = Grid.CellCount / 500;
	}

	// Token: 0x0600463F RID: 17983 RVA: 0x001907BC File Offset: 0x0018E9BC
	public bool IsReachable(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		foreach (KeyValuePair<object, short> keyValuePair in this.cells[cell])
		{
			object key = keyValuePair.Key;
			short value = keyValuePair.Value;
			KeyValuePair<short, short> keyValuePair2;
			if (this.valid_serial_nos.TryGetValue(key, out keyValuePair2) && (value == keyValuePair2.Key || value == keyValuePair2.Value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004640 RID: 17984 RVA: 0x00190854 File Offset: 0x0018EA54
	public bool IsReachable(int cell, CellOffset[] offsets)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		foreach (CellOffset offset in offsets)
		{
			if (this.IsReachable(Grid.OffsetCell(cell, offset)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004641 RID: 17985 RVA: 0x00190898 File Offset: 0x0018EA98
	public bool IsAllReachable(int cell, CellOffset[] offsets)
	{
		if (this.IsReachable(cell))
		{
			return true;
		}
		foreach (CellOffset offset in offsets)
		{
			if (this.IsReachable(Grid.OffsetCell(cell, offset)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004642 RID: 17986 RVA: 0x001908DA File Offset: 0x0018EADA
	public bool IsReachable(Workable workable)
	{
		return this.IsReachable(Grid.PosToCell(workable), workable.GetOffsets());
	}

	// Token: 0x06004643 RID: 17987 RVA: 0x001908F0 File Offset: 0x0018EAF0
	public void Occupy(object prober, short serial_no, IEnumerable<int> cells)
	{
		foreach (int num in cells)
		{
			Dictionary<object, short> obj = this.cells[num];
			lock (obj)
			{
				this.cells[num][prober] = serial_no;
			}
		}
	}

	// Token: 0x06004644 RID: 17988 RVA: 0x0019096C File Offset: 0x0018EB6C
	public void SetValidSerialNos(object prober, short previous_serial_no, short serial_no)
	{
		object obj = this.access;
		lock (obj)
		{
			this.valid_serial_nos[prober] = new KeyValuePair<short, short>(previous_serial_no, serial_no);
		}
	}

	// Token: 0x06004645 RID: 17989 RVA: 0x001909BC File Offset: 0x0018EBBC
	public bool ReleaseProber(object prober)
	{
		object obj = this.access;
		bool result;
		lock (obj)
		{
			result = this.valid_serial_nos.Remove(prober);
		}
		return result;
	}

	// Token: 0x06004646 RID: 17990 RVA: 0x00190A04 File Offset: 0x0018EC04
	public void Sim200ms(float dt)
	{
		int i = 0;
		while (i < this.cell_checks_per_frame)
		{
			this.pending_removals.Clear();
			foreach (KeyValuePair<object, short> keyValuePair in this.cells[this.cell_cleanup_index])
			{
				KeyValuePair<short, short> keyValuePair2;
				if (!this.valid_serial_nos.TryGetValue(keyValuePair.Key, out keyValuePair2) || (keyValuePair2.Key != keyValuePair.Value && keyValuePair2.Value != keyValuePair.Value))
				{
					this.pending_removals.Add(keyValuePair.Key);
				}
			}
			foreach (object key in this.pending_removals)
			{
				this.cells[this.cell_cleanup_index].Remove(key);
			}
			i++;
			this.cell_cleanup_index = (this.cell_cleanup_index + 1) % this.cells.Length;
		}
	}

	// Token: 0x04002DB7 RID: 11703
	private static MinionGroupProber Instance;

	// Token: 0x04002DB8 RID: 11704
	private Dictionary<object, short>[] cells;

	// Token: 0x04002DB9 RID: 11705
	private Dictionary<object, KeyValuePair<short, short>> valid_serial_nos = new Dictionary<object, KeyValuePair<short, short>>();

	// Token: 0x04002DBA RID: 11706
	private List<object> pending_removals = new List<object>();

	// Token: 0x04002DBB RID: 11707
	private int cell_cleanup_index;

	// Token: 0x04002DBC RID: 11708
	private int cell_checks_per_frame;

	// Token: 0x04002DBD RID: 11709
	private readonly object access = new object();
}
