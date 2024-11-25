using System;
using UnityEngine;

// Token: 0x02000BE0 RID: 3040
public class SizePulse : MonoBehaviour
{
	// Token: 0x06005C80 RID: 23680 RVA: 0x0021D6F4 File Offset: 0x0021B8F4
	private void Start()
	{
		if (base.GetComponents<SizePulse>().Length > 1)
		{
			UnityEngine.Object.Destroy(this);
		}
		RectTransform rectTransform = (RectTransform)base.transform;
		this.from = rectTransform.localScale;
		this.cur = this.from;
		this.to = this.from * this.multiplier;
	}

	// Token: 0x06005C81 RID: 23681 RVA: 0x0021D754 File Offset: 0x0021B954
	private void Update()
	{
		float num = this.updateWhenPaused ? Time.unscaledDeltaTime : Time.deltaTime;
		num *= this.speed;
		SizePulse.State state = this.state;
		if (state != SizePulse.State.Up)
		{
			if (state == SizePulse.State.Down)
			{
				this.cur = Vector2.Lerp(this.cur, this.from, num);
				if ((this.from - this.cur).sqrMagnitude < 0.0001f)
				{
					this.cur = this.from;
					this.state = SizePulse.State.Finished;
					if (this.onComplete != null)
					{
						this.onComplete();
					}
				}
			}
		}
		else
		{
			this.cur = Vector2.Lerp(this.cur, this.to, num);
			if ((this.to - this.cur).sqrMagnitude < 0.0001f)
			{
				this.cur = this.to;
				this.state = SizePulse.State.Down;
			}
		}
		((RectTransform)base.transform).localScale = new Vector3(this.cur.x, this.cur.y, 1f);
	}

	// Token: 0x04003DB5 RID: 15797
	public System.Action onComplete;

	// Token: 0x04003DB6 RID: 15798
	public Vector2 from = Vector2.one;

	// Token: 0x04003DB7 RID: 15799
	public Vector2 to = Vector2.one;

	// Token: 0x04003DB8 RID: 15800
	public float multiplier = 1.25f;

	// Token: 0x04003DB9 RID: 15801
	public float speed = 1f;

	// Token: 0x04003DBA RID: 15802
	public bool updateWhenPaused;

	// Token: 0x04003DBB RID: 15803
	private Vector2 cur;

	// Token: 0x04003DBC RID: 15804
	private SizePulse.State state;

	// Token: 0x02001CC7 RID: 7367
	private enum State
	{
		// Token: 0x04008510 RID: 34064
		Up,
		// Token: 0x04008511 RID: 34065
		Down,
		// Token: 0x04008512 RID: 34066
		Finished
	}
}
