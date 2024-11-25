using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000643 RID: 1603
public class AcousticDisturbance
{
	// Token: 0x0600273C RID: 10044 RVA: 0x000DF6F8 File Offset: 0x000DD8F8
	public static void Emit(object data, int EmissionRadius)
	{
		GameObject gameObject = (GameObject)data;
		Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
		Vector2 vector = gameObject.transform.GetPosition();
		int num = Grid.PosToCell(vector);
		int num2 = EmissionRadius * EmissionRadius;
		AcousticDisturbance.cellsInRange = GameUtil.CollectCellsBreadthFirst(num, (int cell) => !Grid.Solid[cell], EmissionRadius);
		AcousticDisturbance.DrawVisualEffect(num, AcousticDisturbance.cellsInRange);
		for (int i = 0; i < liveMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = liveMinionIdentities[i];
			if (minionIdentity.gameObject != gameObject.gameObject)
			{
				Vector2 vector2 = minionIdentity.transform.GetPosition();
				if (Vector2.SqrMagnitude(vector - vector2) <= (float)num2)
				{
					int item = Grid.PosToCell(vector2);
					if (AcousticDisturbance.cellsInRange.Contains(item))
					{
						StaminaMonitor.Instance smi = minionIdentity.GetSMI<StaminaMonitor.Instance>();
						if (smi != null && smi.IsSleeping())
						{
							minionIdentity.Trigger(-527751701, data);
							minionIdentity.Trigger(1621815900, data);
						}
					}
				}
			}
		}
		AcousticDisturbance.cellsInRange.Clear();
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x000DF810 File Offset: 0x000DDA10
	private static void DrawVisualEffect(int center_cell, HashSet<int> cells)
	{
		SoundEvent.PlayOneShot(GlobalResources.Instance().AcousticDisturbanceSound, Grid.CellToPos(center_cell), 1f);
		foreach (int num in cells)
		{
			int gridDistance = AcousticDisturbance.GetGridDistance(num, center_cell);
			GameScheduler.Instance.Schedule("radialgrid_pre", AcousticDisturbance.distanceDelay * (float)gridDistance, new Action<object>(AcousticDisturbance.SpawnEffect), num, null);
		}
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x000DF8A8 File Offset: 0x000DDAA8
	private static void SpawnEffect(object data)
	{
		Grid.SceneLayer layer = Grid.SceneLayer.InteriorWall;
		int cell = (int)data;
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("radialgrid_kanim", Grid.CellToPosCCC(cell, layer), null, false, layer, false);
		kbatchedAnimController.destroyOnAnimComplete = false;
		kbatchedAnimController.Play(AcousticDisturbance.PreAnims, KAnim.PlayMode.Loop);
		GameScheduler.Instance.Schedule("radialgrid_loop", AcousticDisturbance.duration, new Action<object>(AcousticDisturbance.DestroyEffect), kbatchedAnimController, null);
	}

	// Token: 0x0600273F RID: 10047 RVA: 0x000DF90B File Offset: 0x000DDB0B
	private static void DestroyEffect(object data)
	{
		KBatchedAnimController kbatchedAnimController = (KBatchedAnimController)data;
		kbatchedAnimController.destroyOnAnimComplete = true;
		kbatchedAnimController.Play(AcousticDisturbance.PostAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002740 RID: 10048 RVA: 0x000DF930 File Offset: 0x000DDB30
	private static int GetGridDistance(int cell, int center_cell)
	{
		Vector2I u = Grid.CellToXY(cell);
		Vector2I v = Grid.CellToXY(center_cell);
		Vector2I vector2I = u - v;
		return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
	}

	// Token: 0x0400168B RID: 5771
	private static readonly HashedString[] PreAnims = new HashedString[]
	{
		"grid_pre",
		"grid_loop"
	};

	// Token: 0x0400168C RID: 5772
	private static readonly HashedString PostAnim = "grid_pst";

	// Token: 0x0400168D RID: 5773
	private static float distanceDelay = 0.25f;

	// Token: 0x0400168E RID: 5774
	private static float duration = 3f;

	// Token: 0x0400168F RID: 5775
	private static HashSet<int> cellsInRange = new HashSet<int>();
}
