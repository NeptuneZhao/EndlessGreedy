using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000612 RID: 1554
public abstract class DevToolEntityTarget
{
	// Token: 0x06002649 RID: 9801
	public abstract string GetTag();

	// Token: 0x0600264A RID: 9802
	[return: TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})]
	public abstract Option<ValueTuple<Vector2, Vector2>> GetScreenRect();

	// Token: 0x0600264B RID: 9803 RVA: 0x000D543B File Offset: 0x000D363B
	public string GetDebugName()
	{
		return "[" + this.GetTag() + "] " + this.ToString();
	}

	// Token: 0x020013F7 RID: 5111
	public class ForUIGameObject : DevToolEntityTarget
	{
		// Token: 0x060088E5 RID: 35045 RVA: 0x0032F421 File Offset: 0x0032D621
		public ForUIGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x060088E6 RID: 35046 RVA: 0x0032F430 File Offset: 0x0032D630
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			RectTransform component = this.gameObject.GetComponent<RectTransform>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Canvas componentInParent = this.gameObject.GetComponentInParent<Canvas>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			if (!componentInParent.worldCamera.IsNullOrDestroyed())
			{
				DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
				CS$<>8__locals1.camera = componentInParent.worldCamera;
				Vector3[] array = new Vector3[4];
				component.GetWorldCorners(array);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[0]), ref CS$<>8__locals1), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[2]), ref CS$<>8__locals1));
			}
			if (componentInParent.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				Vector3[] array2 = new Vector3[4];
				component.GetWorldCorners(array2);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[0]), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[2]));
			}
			return Option.None;
		}

		// Token: 0x060088E7 RID: 35047 RVA: 0x0032F553 File Offset: 0x0032D753
		public override string GetTag()
		{
			return "UI";
		}

		// Token: 0x060088E8 RID: 35048 RVA: 0x0032F55A File Offset: 0x0032D75A
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x060088E9 RID: 35049 RVA: 0x0032F567 File Offset: 0x0032D767
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x060088EA RID: 35050 RVA: 0x0032F587 File Offset: 0x0032D787
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_1(Vector2 coord)
		{
			return new Vector2(coord.x, (float)Screen.height - coord.y);
		}

		// Token: 0x0400687F RID: 26751
		public GameObject gameObject;
	}

	// Token: 0x020013F8 RID: 5112
	public class ForWorldGameObject : DevToolEntityTarget
	{
		// Token: 0x060088EB RID: 35051 RVA: 0x0032F5A1 File Offset: 0x0032D7A1
		public ForWorldGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x060088EC RID: 35052 RVA: 0x0032F5B0 File Offset: 0x0032D7B0
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			KCollider2D component = this.gameObject.GetComponent<KCollider2D>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.min), ref CS$<>8__locals1), DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.max), ref CS$<>8__locals1));
		}

		// Token: 0x060088ED RID: 35053 RVA: 0x0032F66C File Offset: 0x0032D86C
		public override string GetTag()
		{
			return "World";
		}

		// Token: 0x060088EE RID: 35054 RVA: 0x0032F673 File Offset: 0x0032D873
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x060088EF RID: 35055 RVA: 0x0032F680 File Offset: 0x0032D880
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x04006880 RID: 26752
		public GameObject gameObject;
	}

	// Token: 0x020013F9 RID: 5113
	public class ForSimCell : DevToolEntityTarget
	{
		// Token: 0x060088F0 RID: 35056 RVA: 0x0032F6A0 File Offset: 0x0032D8A0
		public ForSimCell(int cellIndex)
		{
			this.cellIndex = cellIndex;
		}

		// Token: 0x060088F1 RID: 35057 RVA: 0x0032F6B0 File Offset: 0x0032D8B0
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Vector2 a = Grid.CellToPosCCC(this.cellIndex, Grid.SceneLayer.Background);
			Vector2 b = Grid.HalfCellSizeInMeters * Vector2.one;
			Vector2 v = a - b;
			Vector2 v2 = a + b;
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v), ref CS$<>8__locals1), DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v2), ref CS$<>8__locals1));
		}

		// Token: 0x060088F2 RID: 35058 RVA: 0x0032F755 File Offset: 0x0032D955
		public override string GetTag()
		{
			return "Sim Cell";
		}

		// Token: 0x060088F3 RID: 35059 RVA: 0x0032F75C File Offset: 0x0032D95C
		public override string ToString()
		{
			return this.cellIndex.ToString();
		}

		// Token: 0x060088F4 RID: 35060 RVA: 0x0032F769 File Offset: 0x0032D969
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x04006881 RID: 26753
		public int cellIndex;
	}
}
