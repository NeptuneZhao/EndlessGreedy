using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200085C RID: 2140
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseEmitter")]
public class DiseaseEmitter : KMonoBehaviour
{
	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06003B95 RID: 15253 RVA: 0x00148606 File Offset: 0x00146806
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x00148610 File Offset: 0x00146810
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.emitDiseases != null)
		{
			this.simHandles = new int[this.emitDiseases.Length];
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				this.simHandles[i] = -1;
			}
		}
		this.SimRegister();
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x00148660 File Offset: 0x00146860
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x0014866E File Offset: 0x0014686E
	public void SetEnable(bool enable)
	{
		if (this.enableEmitter == enable)
		{
			return;
		}
		this.enableEmitter = enable;
		if (this.enableEmitter)
		{
			this.SimRegister();
			return;
		}
		this.SimUnregister();
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x00148698 File Offset: 0x00146898
	private void OnCellChanged()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < this.emitDiseases.Length; i++)
			{
				if (Sim.IsValidHandle(this.simHandles[i]))
				{
					SimMessages.ModifyDiseaseEmitter(this.simHandles[i], cell, this.emitRange, this.emitDiseases[i], this.emitRate, this.emitCount);
				}
			}
		}
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x00148710 File Offset: 0x00146910
	private void SimRegister()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "DiseaseEmitter.Modify");
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (this.simHandles[i] == -1)
			{
				this.simHandles[i] = -2;
				SimMessages.AddDiseaseEmitter(Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(DiseaseEmitter.OnSimRegisteredCallback), this, "DiseaseEmitter").index);
			}
		}
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x001487A8 File Offset: 0x001469A8
	private void SimUnregister()
	{
		if (this.simHandles == null)
		{
			return;
		}
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (Sim.IsValidHandle(this.simHandles[i]))
			{
				SimMessages.RemoveDiseaseEmitter(-1, this.simHandles[i]);
			}
			this.simHandles[i] = -1;
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x00148813 File Offset: 0x00146A13
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((DiseaseEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x00148824 File Offset: 0x00146A24
	private void OnSimRegistered(int handle)
	{
		bool flag = false;
		if (this != null)
		{
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				if (this.simHandles[i] == -2)
				{
					this.simHandles[i] = handle;
					flag = true;
					break;
				}
			}
			this.OnCellChanged();
		}
		if (!flag)
		{
			SimMessages.RemoveDiseaseEmitter(-1, handle);
		}
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x00148878 File Offset: 0x00146A78
	public void SetDiseases(List<Disease> diseases)
	{
		this.emitDiseases = new byte[diseases.Count];
		for (int i = 0; i < diseases.Count; i++)
		{
			this.emitDiseases[i] = Db.Get().Diseases.GetIndex(diseases[i].id);
		}
	}

	// Token: 0x04002402 RID: 9218
	[Serialize]
	public float emitRate = 1f;

	// Token: 0x04002403 RID: 9219
	[Serialize]
	public byte emitRange;

	// Token: 0x04002404 RID: 9220
	[Serialize]
	public int emitCount;

	// Token: 0x04002405 RID: 9221
	[Serialize]
	public byte[] emitDiseases;

	// Token: 0x04002406 RID: 9222
	public int[] simHandles;

	// Token: 0x04002407 RID: 9223
	[Serialize]
	private bool enableEmitter;
}
