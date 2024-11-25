using System;
using UnityEngine;

// Token: 0x02000576 RID: 1398
public class KCircleCollider2D : KCollider2D
{
	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06002064 RID: 8292 RVA: 0x000B5A56 File Offset: 0x000B3C56
	// (set) Token: 0x06002065 RID: 8293 RVA: 0x000B5A5E File Offset: 0x000B3C5E
	public float radius
	{
		get
		{
			return this._radius;
		}
		set
		{
			this._radius = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x06002066 RID: 8294 RVA: 0x000B5A70 File Offset: 0x000B3C70
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.radius, vector.y - this.radius);
		Vector2 vector3 = new Vector2(vector.x + this.radius, vector.y + this.radius);
		int width = (int)vector3.x - (int)vector2.x + 1;
		int height = (int)vector3.y - (int)vector2.y + 1;
		return new Extents((int)(vector.x - this._radius), (int)(vector.y - this._radius), width, height);
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06002067 RID: 8295 RVA: 0x000B5B34 File Offset: 0x000B3D34
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._radius * 2f, this._radius * 2f, 0f));
		}
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x000B5B98 File Offset: 0x000B3D98
	public override bool Intersects(Vector2 pos)
	{
		Vector3 position = base.transform.GetPosition();
		Vector2 b = new Vector2(position.x, position.y) + base.offset;
		return (pos - b).sqrMagnitude <= this._radius * this._radius;
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x000B5BF0 File Offset: 0x000B3DF0
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.bounds.center, this.radius);
	}

	// Token: 0x04001245 RID: 4677
	[SerializeField]
	private float _radius;
}
