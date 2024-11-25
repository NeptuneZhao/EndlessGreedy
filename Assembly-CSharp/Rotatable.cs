using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005B0 RID: 1456
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Rotatable")]
public class Rotatable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060022AC RID: 8876 RVA: 0x000C1216 File Offset: 0x000BF416
	public Orientation Orientation
	{
		get
		{
			return this.orientation;
		}
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x000C1220 File Offset: 0x000BF420
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			this.SetSize(component.Def.WidthInCells, component.Def.HeightInCells);
		}
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x000C1278 File Offset: 0x000BF478
	public void SetSize(int width, int height)
	{
		this.width = width;
		this.height = height;
		if (width % 2 == 0)
		{
			this.pivot = new Vector3(-0.5f, 0.5f, 0f);
			this.visualizerOffset = new Vector3(0.5f, 0f, 0f);
			return;
		}
		this.pivot = new Vector3(0f, 0.5f, 0f);
		this.visualizerOffset = Vector3.zero;
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x000C12F8 File Offset: 0x000BF4F8
	public Orientation Rotate()
	{
		switch (this.permittedRotations)
		{
		case PermittedRotations.R90:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.R90 : Orientation.Neutral);
			break;
		case PermittedRotations.R360:
			this.orientation = (this.orientation + 1) % Orientation.NumRotations;
			break;
		case PermittedRotations.FlipH:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipH : Orientation.Neutral);
			break;
		case PermittedRotations.FlipV:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipV : Orientation.Neutral);
			break;
		}
		this.OrientVisualizer(this.orientation);
		return this.orientation;
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x000C1384 File Offset: 0x000BF584
	public void SetOrientation(Orientation new_orientation)
	{
		this.orientation = new_orientation;
		this.OrientVisualizer(new_orientation);
		this.OrientCollider(new_orientation);
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x000C139C File Offset: 0x000BF59C
	public void Match(Rotatable other)
	{
		this.pivot = other.pivot;
		this.visualizerOffset = other.visualizerOffset;
		this.permittedRotations = other.permittedRotations;
		this.orientation = other.orientation;
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x000C13F4 File Offset: 0x000BF5F4
	public float GetVisualizerRotation()
	{
		PermittedRotations permittedRotations = this.permittedRotations;
		if (permittedRotations - PermittedRotations.R90 <= 1)
		{
			return -90f * (float)this.orientation;
		}
		return 0f;
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000C1421 File Offset: 0x000BF621
	public bool GetVisualizerFlipX()
	{
		return this.orientation == Orientation.FlipH;
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x000C142C File Offset: 0x000BF62C
	public bool GetVisualizerFlipY()
	{
		return this.orientation == Orientation.FlipV;
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x000C1438 File Offset: 0x000BF638
	public Vector3 GetVisualizerPivot()
	{
		Vector3 result = this.pivot;
		Orientation orientation = this.orientation;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
			}
		}
		else
		{
			result.x = -this.pivot.x;
		}
		return result;
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x000C1474 File Offset: 0x000BF674
	private Vector3 GetVisualizerOffset()
	{
		Orientation orientation = this.orientation;
		Vector3 result;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
				result = this.visualizerOffset;
			}
			else
			{
				result = new Vector3(this.visualizerOffset.x, 1f, this.visualizerOffset.z);
			}
		}
		else
		{
			result = new Vector3(-this.visualizerOffset.x, this.visualizerOffset.y, this.visualizerOffset.z);
		}
		return result;
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x000C14E8 File Offset: 0x000BF6E8
	private void OrientVisualizer(Orientation orientation)
	{
		float visualizerRotation = this.GetVisualizerRotation();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Pivot = this.GetVisualizerPivot();
		component.Rotation = visualizerRotation;
		component.Offset = this.GetVisualizerOffset();
		component.FlipX = this.GetVisualizerFlipX();
		component.FlipY = this.GetVisualizerFlipY();
		base.Trigger(-1643076535, this);
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x000C1544 File Offset: 0x000BF744
	private void OrientCollider(Orientation orientation)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component == null)
		{
			return;
		}
		float num = 0.5f * (float)((this.width + 1) % 2);
		float num2 = 0f;
		switch (orientation)
		{
		case Orientation.R90:
			num2 = -90f;
			goto IL_11B;
		case Orientation.R180:
			num2 = -180f;
			goto IL_11B;
		case Orientation.R270:
			num2 = -270f;
			goto IL_11B;
		case Orientation.FlipH:
			component.offset = new Vector2(num + (float)(this.width % 2) - 1f, 0.5f * (float)this.height);
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		case Orientation.FlipV:
			component.offset = new Vector2(num, -0.5f * (float)(this.height - 2));
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		}
		component.offset = new Vector2(num, 0.5f * (float)this.height);
		component.size = new Vector2((float)this.width, (float)this.height);
		IL_11B:
		if (num2 != 0f)
		{
			Matrix2x3 n = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n2 = Matrix2x3.Rotate(num2 * 0.017453292f);
			Matrix2x3 matrix2x = Matrix2x3.Translate(this.pivot + new Vector3(num, 0f, 0f)) * n2 * n;
			Vector2 vector = new Vector2(-0.5f * (float)this.width, 0f);
			Vector2 vector2 = new Vector2(0.5f * (float)this.width, (float)this.height);
			Vector2 vector3 = new Vector2(0f, 0.5f * (float)this.height);
			vector = matrix2x.MultiplyPoint(vector);
			vector2 = matrix2x.MultiplyPoint(vector2);
			vector3 = matrix2x.MultiplyPoint(vector3);
			float num3 = Mathf.Min(vector.x, vector2.x);
			float num4 = Mathf.Max(vector.x, vector2.x);
			float num5 = Mathf.Min(vector.y, vector2.y);
			float num6 = Mathf.Max(vector.y, vector2.y);
			component.offset = vector3;
			component.size = new Vector2(num4 - num3, num6 - num5);
		}
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x000C17CC File Offset: 0x000BF9CC
	public CellOffset GetRotatedCellOffset(CellOffset offset)
	{
		return Rotatable.GetRotatedCellOffset(offset, this.orientation);
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000C17DC File Offset: 0x000BF9DC
	public static CellOffset GetRotatedCellOffset(CellOffset offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new CellOffset(offset.y, -offset.x);
		case Orientation.R180:
			return new CellOffset(-offset.x, -offset.y);
		case Orientation.R270:
			return new CellOffset(-offset.y, offset.x);
		case Orientation.FlipH:
			return new CellOffset(-offset.x, offset.y);
		case Orientation.FlipV:
			return new CellOffset(offset.x, -offset.y);
		}
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000C186C File Offset: 0x000BFA6C
	public static CellOffset GetRotatedCellOffset(int x, int y, Orientation orientation)
	{
		return Rotatable.GetRotatedCellOffset(new CellOffset(x, y), orientation);
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000C187B File Offset: 0x000BFA7B
	public Vector3 GetRotatedOffset(Vector3 offset)
	{
		return Rotatable.GetRotatedOffset(offset, this.orientation);
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000C188C File Offset: 0x000BFA8C
	public static Vector3 GetRotatedOffset(Vector3 offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector3(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector3(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector3(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector3(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector3(offset.x, -offset.y);
		}
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000C191C File Offset: 0x000BFB1C
	public Vector2I GetRotatedOffset(Vector2I offset)
	{
		switch (this.orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector2I(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector2I(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector2I(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector2I(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector2I(offset.x, -offset.y);
		}
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000C19B3 File Offset: 0x000BFBB3
	public Orientation GetOrientation()
	{
		return this.orientation;
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060022C0 RID: 8896 RVA: 0x000C19BB File Offset: 0x000BFBBB
	public bool IsRotated
	{
		get
		{
			return this.orientation > Orientation.Neutral;
		}
	}

	// Token: 0x0400139B RID: 5019
	[Serialize]
	[SerializeField]
	private Orientation orientation;

	// Token: 0x0400139C RID: 5020
	[SerializeField]
	private Vector3 pivot = Vector3.zero;

	// Token: 0x0400139D RID: 5021
	[SerializeField]
	private Vector3 visualizerOffset = Vector3.zero;

	// Token: 0x0400139E RID: 5022
	public PermittedRotations permittedRotations;

	// Token: 0x0400139F RID: 5023
	[SerializeField]
	private int width;

	// Token: 0x040013A0 RID: 5024
	[SerializeField]
	private int height;
}
