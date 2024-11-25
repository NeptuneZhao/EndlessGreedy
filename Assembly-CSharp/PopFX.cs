using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D11 RID: 3345
[AddComponentMenu("KMonoBehaviour/scripts/PopFX")]
public class PopFX : KMonoBehaviour
{
	// Token: 0x06006863 RID: 26723 RVA: 0x002716E8 File Offset: 0x0026F8E8
	public void Recycle()
	{
		this.icon = null;
		this.text = "";
		this.targetTransform = null;
		this.lifeElapsed = 0f;
		this.trackTarget = false;
		this.startPos = Vector3.zero;
		this.IconDisplay.color = Color.white;
		this.TextDisplay.color = Color.white;
		PopFXManager.Instance.RecycleFX(this);
		this.canvasGroup.alpha = 0f;
		base.gameObject.SetActive(false);
		this.isLive = false;
		this.isActiveWorld = false;
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
	}

	// Token: 0x06006864 RID: 26724 RVA: 0x0027179C File Offset: 0x0026F99C
	public void Spawn(Sprite Icon, string Text, Transform TargetTransform, Vector3 Offset, float LifeTime = 1.5f, bool TrackTarget = false)
	{
		this.icon = Icon;
		this.text = Text;
		this.targetTransform = TargetTransform;
		this.trackTarget = TrackTarget;
		this.lifetime = LifeTime;
		this.offset = Offset;
		if (this.targetTransform != null)
		{
			this.startPos = this.targetTransform.GetPosition();
			int num;
			int num2;
			Grid.PosToXY(this.startPos, out num, out num2);
			if (num2 % 2 != 0)
			{
				this.startPos.x = this.startPos.x + 0.5f;
			}
		}
		this.TextDisplay.text = this.text;
		this.IconDisplay.sprite = this.icon;
		this.canvasGroup.alpha = 1f;
		this.isLive = true;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		this.Update();
	}

	// Token: 0x06006865 RID: 26725 RVA: 0x00271888 File Offset: 0x0026FA88
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		if (this.isLive)
		{
			this.SetWorldActive(tuple.first);
		}
	}

	// Token: 0x06006866 RID: 26726 RVA: 0x002718B0 File Offset: 0x0026FAB0
	private void SetWorldActive(int worldId)
	{
		int num = Grid.PosToCell((this.trackTarget && this.targetTransform != null) ? this.targetTransform.position : (this.startPos + this.offset));
		this.isActiveWorld = (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] == worldId);
	}

	// Token: 0x06006867 RID: 26727 RVA: 0x00271914 File Offset: 0x0026FB14
	private void Update()
	{
		if (!this.isLive)
		{
			return;
		}
		if (!PopFXManager.Instance.Ready())
		{
			return;
		}
		this.lifeElapsed += Time.unscaledDeltaTime;
		if (this.lifeElapsed >= this.lifetime)
		{
			this.Recycle();
		}
		if (this.trackTarget && this.targetTransform != null)
		{
			Vector3 v = PopFXManager.Instance.WorldToScreen(this.targetTransform.GetPosition() + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * this.lifeElapsed));
			v.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v;
		}
		else
		{
			Vector3 v2 = PopFXManager.Instance.WorldToScreen(this.startPos + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * (this.lifeElapsed / 2f)));
			v2.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v2;
		}
		this.canvasGroup.alpha = (this.isActiveWorld ? (1.5f * ((this.lifetime - this.lifeElapsed) / this.lifetime)) : 0f);
	}

	// Token: 0x0400468A RID: 18058
	private float Speed = 2f;

	// Token: 0x0400468B RID: 18059
	private Sprite icon;

	// Token: 0x0400468C RID: 18060
	private string text;

	// Token: 0x0400468D RID: 18061
	private Transform targetTransform;

	// Token: 0x0400468E RID: 18062
	private Vector3 offset;

	// Token: 0x0400468F RID: 18063
	public Image IconDisplay;

	// Token: 0x04004690 RID: 18064
	public LocText TextDisplay;

	// Token: 0x04004691 RID: 18065
	public CanvasGroup canvasGroup;

	// Token: 0x04004692 RID: 18066
	private Camera uiCamera;

	// Token: 0x04004693 RID: 18067
	private float lifetime;

	// Token: 0x04004694 RID: 18068
	private float lifeElapsed;

	// Token: 0x04004695 RID: 18069
	private bool trackTarget;

	// Token: 0x04004696 RID: 18070
	private Vector3 startPos;

	// Token: 0x04004697 RID: 18071
	private bool isLive;

	// Token: 0x04004698 RID: 18072
	private bool isActiveWorld;
}
