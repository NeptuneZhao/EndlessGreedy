using System;
using UnityEngine;

// Token: 0x02000575 RID: 1397
public class KBoxCollider2D : KCollider2D
{
	// Token: 0x17000155 RID: 341
	// (get) Token: 0x0600205D RID: 8285 RVA: 0x000B578E File Offset: 0x000B398E
	// (set) Token: 0x0600205E RID: 8286 RVA: 0x000B5796 File Offset: 0x000B3996
	public Vector2 size
	{
		get
		{
			return this._size;
		}
		set
		{
			this._size = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000B57A8 File Offset: 0x000B39A8
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = this.size * 0.9999f;
		Vector2 vector3 = new Vector2(vector.x - vector2.x * 0.5f, vector.y - vector2.y * 0.5f);
		Vector2 vector4 = new Vector2(vector.x + vector2.x * 0.5f, vector.y + vector2.y * 0.5f);
		Vector2I vector2I = new Vector2I((int)vector3.x, (int)vector3.y);
		Vector2I vector2I2 = new Vector2I((int)vector4.x, (int)vector4.y);
		int width = vector2I2.x - vector2I.x + 1;
		int height = vector2I2.y - vector2I.y + 1;
		return new Extents(vector2I.x, vector2I.y, width, height);
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000B58B4 File Offset: 0x000B3AB4
	public override bool Intersects(Vector2 intersect_pos)
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.size.x * 0.5f, vector.y - this.size.y * 0.5f);
		Vector2 vector3 = new Vector2(vector.x + this.size.x * 0.5f, vector.y + this.size.y * 0.5f);
		return intersect_pos.x >= vector2.x && intersect_pos.x <= vector3.x && intersect_pos.y >= vector2.y && intersect_pos.y <= vector3.y;
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06002061 RID: 8289 RVA: 0x000B59A0 File Offset: 0x000B3BA0
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._size.x, this._size.y, 0f));
		}
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000B5A04 File Offset: 0x000B3C04
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.bounds.center, new Vector3(this._size.x, this._size.y, 0f));
	}

	// Token: 0x04001244 RID: 4676
	[SerializeField]
	private Vector2 _size;
}
