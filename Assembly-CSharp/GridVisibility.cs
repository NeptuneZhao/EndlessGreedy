using System;
using UnityEngine;

// Token: 0x020008E1 RID: 2273
[AddComponentMenu("KMonoBehaviour/scripts/GridVisibility")]
public class GridVisibility : KMonoBehaviour
{
	// Token: 0x06004122 RID: 16674 RVA: 0x001724C8 File Offset: 0x001706C8
	protected override void OnSpawn()
	{
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "GridVisibility.OnSpawn");
		this.OnCellChange();
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld != null && !base.gameObject.HasTag(GameTags.Stored))
		{
			myWorld.SetDiscovered(false);
		}
	}

	// Token: 0x06004123 RID: 16675 RVA: 0x0017252C File Offset: 0x0017072C
	private void OnCellChange()
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Revealed[num])
		{
			int baseX;
			int baseY;
			Grid.PosToXY(base.transform.GetPosition(), out baseX, out baseY);
			GridVisibility.Reveal(baseX, baseY, this.radius, this.innerRadius);
			Grid.Revealed[num] = true;
		}
		FogOfWarMask.ClearMask(num);
	}

	// Token: 0x06004124 RID: 16676 RVA: 0x001725A4 File Offset: 0x001707A4
	public static void Reveal(int baseX, int baseY, int radius, float innerRadius)
	{
		int num = (int)Grid.WorldIdx[baseY * Grid.WidthInCells + baseX];
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				int num2 = baseY + i;
				int num3 = baseX + j;
				if (num2 >= 0 && Grid.HeightInCells - 1 >= num2 && num3 >= 0 && Grid.WidthInCells - 1 >= num3)
				{
					int num4 = num2 * Grid.WidthInCells + num3;
					if (Grid.Visible[num4] < 255 && num == (int)Grid.WorldIdx[num4])
					{
						Vector2 vector = new Vector2((float)j, (float)i);
						float num5 = Mathf.Lerp(1f, 0f, (vector.magnitude - innerRadius) / ((float)radius - innerRadius));
						Grid.Reveal(num4, (byte)(255f * num5), false);
					}
				}
			}
		}
	}

	// Token: 0x06004125 RID: 16677 RVA: 0x0017266F File Offset: 0x0017086F
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
	}

	// Token: 0x04002B38 RID: 11064
	public int radius = 18;

	// Token: 0x04002B39 RID: 11065
	public float innerRadius = 16.5f;
}
