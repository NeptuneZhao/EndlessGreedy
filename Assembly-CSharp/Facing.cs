using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000564 RID: 1380
[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
	// Token: 0x06001FFB RID: 8187 RVA: 0x000B4217 File Offset: 0x000B2417
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("Facing", 35);
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000B4231 File Offset: 0x000B2431
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateMirror();
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x000B4240 File Offset: 0x000B2440
	public void Face(float target_x)
	{
		float x = base.transform.GetLocalPosition().x;
		if (target_x < x)
		{
			this.SetFacing(true);
			return;
		}
		if (target_x > x)
		{
			this.SetFacing(false);
		}
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x000B4278 File Offset: 0x000B2478
	public void Face(Vector3 target_pos)
	{
		int num = Grid.CellColumn(Grid.PosToCell(base.transform.GetLocalPosition()));
		int num2 = Grid.CellColumn(Grid.PosToCell(target_pos));
		if (num > num2)
		{
			this.SetFacing(true);
			return;
		}
		if (num2 > num)
		{
			this.SetFacing(false);
		}
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x000B42BE File Offset: 0x000B24BE
	[ContextMenu("Flip")]
	public void SwapFacing()
	{
		this.SetFacing(!this.facingLeft);
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x000B42CF File Offset: 0x000B24CF
	private void UpdateMirror()
	{
		if (this.kanimController != null && this.kanimController.FlipX != this.facingLeft)
		{
			this.kanimController.FlipX = this.facingLeft;
			bool flag = this.facingLeft;
		}
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x000B430A File Offset: 0x000B250A
	public bool GetFacing()
	{
		return this.facingLeft;
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x000B4312 File Offset: 0x000B2512
	public void SetFacing(bool mirror_x)
	{
		this.facingLeft = mirror_x;
		this.UpdateMirror();
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x000B4324 File Offset: 0x000B2524
	public int GetFrontCell()
	{
		int cell = Grid.PosToCell(this);
		if (this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000B4350 File Offset: 0x000B2550
	public int GetBackCell()
	{
		int cell = Grid.PosToCell(this);
		if (!this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x0400120B RID: 4619
	[MyCmpGet]
	private KAnimControllerBase kanimController;

	// Token: 0x0400120C RID: 4620
	private LoggerFS log;

	// Token: 0x0400120D RID: 4621
	[Serialize]
	public bool facingLeft;
}
