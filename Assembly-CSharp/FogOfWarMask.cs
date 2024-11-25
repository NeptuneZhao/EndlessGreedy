using System;
using UnityEngine;

// Token: 0x020008BC RID: 2236
[AddComponentMenu("KMonoBehaviour/scripts/FogOfWarMask")]
public class FogOfWarMask : KMonoBehaviour
{
	// Token: 0x06003EA6 RID: 16038 RVA: 0x0015AF75 File Offset: 0x00159175
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(this.OnReveal));
	}

	// Token: 0x06003EA7 RID: 16039 RVA: 0x0015AF9D File Offset: 0x0015919D
	private void OnReveal(int cell)
	{
		if (Grid.PosToCell(this) == cell)
		{
			Grid.OnReveal = (Action<int>)Delegate.Remove(Grid.OnReveal, new Action<int>(this.OnReveal));
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06003EA8 RID: 16040 RVA: 0x0015AFD4 File Offset: 0x001591D4
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		GameUtil.FloodCollectCells(Grid.PosToCell(this), delegate(int cell)
		{
			Grid.Visible[cell] = 0;
			Grid.PreventFogOfWarReveal[cell] = true;
			return !Grid.Solid[cell];
		}, 300, null, true);
		GameUtil.FloodCollectCells(Grid.PosToCell(this), delegate(int cell)
		{
			bool flag = Grid.PreventFogOfWarReveal[cell];
			if (Grid.Solid[cell] && Grid.Foundation[cell])
			{
				Grid.PreventFogOfWarReveal[cell] = true;
				Grid.Visible[cell] = 0;
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null && gameObject.GetComponent<KPrefabID>().PrefabTag.ToString() == "POIBunkerExteriorDoor")
				{
					Grid.PreventFogOfWarReveal[cell] = false;
					Grid.Visible[cell] = byte.MaxValue;
				}
			}
			return flag || Grid.Foundation[cell];
		}, 300, null, true);
	}

	// Token: 0x06003EA9 RID: 16041 RVA: 0x0015B04B File Offset: 0x0015924B
	public static void ClearMask(int cell)
	{
		if (Grid.PreventFogOfWarReveal[cell])
		{
			GameUtil.FloodCollectCells(cell, new Func<int, bool>(FogOfWarMask.RevealFogOfWarMask), 300, null, true);
		}
	}

	// Token: 0x06003EAA RID: 16042 RVA: 0x0015B074 File Offset: 0x00159274
	public static bool RevealFogOfWarMask(int cell)
	{
		bool flag = Grid.PreventFogOfWarReveal[cell];
		if (flag)
		{
			Grid.PreventFogOfWarReveal[cell] = false;
			Grid.Reveal(cell, byte.MaxValue, false);
		}
		return flag;
	}
}
