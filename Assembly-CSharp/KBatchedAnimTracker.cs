using System;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public class KBatchedAnimTracker : MonoBehaviour
{
	// Token: 0x06001BE5 RID: 7141 RVA: 0x0009213C File Offset: 0x0009033C
	private void Start()
	{
		if (this.controller == null)
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				this.controller = parent.GetComponent<KBatchedAnimController>();
				if (this.controller != null)
				{
					break;
				}
				parent = parent.parent;
			}
		}
		if (this.controller == null)
		{
			global::Debug.Log("Controller Null for tracker on " + base.gameObject.name, base.gameObject);
			base.enabled = false;
			return;
		}
		this.controller.onAnimEnter += this.OnAnimStart;
		this.controller.onAnimComplete += this.OnAnimStop;
		this.controller.onLayerChanged += this.OnLayerChanged;
		this.forceUpdate = true;
		if (this.myAnim != null)
		{
			return;
		}
		this.myAnim = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = this.myAnim;
		kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Combine(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x00092254 File Offset: 0x00090454
	private Vector4 MyAnimGetPosition()
	{
		if (this.myAnim != null && this.controller != null && this.controller.transform == this.myAnim.transform.parent)
		{
			Vector3 pivotSymbolPosition = this.myAnim.GetPivotSymbolPosition();
			return new Vector4(pivotSymbolPosition.x - this.controller.Offset.x, pivotSymbolPosition.y - this.controller.Offset.y, pivotSymbolPosition.x, pivotSymbolPosition.y);
		}
		return base.transform.GetPosition();
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x000922FC File Offset: 0x000904FC
	private void OnDestroy()
	{
		if (this.controller != null)
		{
			this.controller.onAnimEnter -= this.OnAnimStart;
			this.controller.onAnimComplete -= this.OnAnimStop;
			this.controller.onLayerChanged -= this.OnLayerChanged;
			this.controller = null;
		}
		if (this.myAnim != null)
		{
			KBatchedAnimController kbatchedAnimController = this.myAnim;
			kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Remove(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
		}
		this.myAnim = null;
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x000923A0 File Offset: 0x000905A0
	private void LateUpdate()
	{
		if (this.controller != null && (this.controller.IsVisible() || this.forceAlwaysVisible || this.forceUpdate))
		{
			this.UpdateFrame();
		}
		if (!this.alive)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x000923ED File Offset: 0x000905ED
	public void SetAnimControllers(KBatchedAnimController controller, KBatchedAnimController parentController)
	{
		this.myAnim = controller;
		this.controller = parentController;
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x00092400 File Offset: 0x00090600
	private void UpdateFrame()
	{
		this.forceUpdate = false;
		bool flag = false;
		if (this.controller.CurrentAnim != null)
		{
			Matrix2x3 symbolLocalTransform = this.controller.GetSymbolLocalTransform(this.symbol, out flag);
			Vector3 position = this.controller.transform.GetPosition();
			if (flag && (this.previousMatrix != symbolLocalTransform || position != this.previousPosition || (this.useTargetPoint && this.targetPoint != this.previousTargetPoint) || (this.matchParentOffset && this.myAnim.Offset != this.controller.Offset)))
			{
				this.previousMatrix = symbolLocalTransform;
				this.previousPosition = position;
				Matrix2x3 overrideTransformMatrix = ((this.useTargetPoint || this.myAnim == null) ? this.controller.GetTransformMatrix() : this.controller.GetTransformMatrix(new Vector2(this.myAnim.animWidth * this.myAnim.animScale, -this.myAnim.animHeight * this.myAnim.animScale))) * symbolLocalTransform;
				float z = base.transform.GetPosition().z;
				base.transform.SetPosition(overrideTransformMatrix.MultiplyPoint(this.offset));
				if (this.useTargetPoint)
				{
					this.previousTargetPoint = this.targetPoint;
					Vector3 position2 = base.transform.GetPosition();
					position2.z = 0f;
					Vector3 vector = this.targetPoint - position2;
					float num = Vector3.Angle(vector, Vector3.right);
					if (vector.y < 0f)
					{
						num = 360f - num;
					}
					base.transform.localRotation = Quaternion.identity;
					base.transform.RotateAround(position2, new Vector3(0f, 0f, 1f), num);
					float sqrMagnitude = vector.sqrMagnitude;
					this.myAnim.GetBatchInstanceData().SetClipRadius(base.transform.GetPosition().x, base.transform.GetPosition().y, sqrMagnitude, true);
				}
				else
				{
					Vector3 v = this.controller.FlipX ? Vector3.left : Vector3.right;
					Vector3 v2 = this.controller.FlipY ? Vector3.down : Vector3.up;
					base.transform.up = overrideTransformMatrix.MultiplyVector(v2);
					base.transform.right = overrideTransformMatrix.MultiplyVector(v);
					if (this.myAnim != null)
					{
						KBatchedAnimInstanceData batchInstanceData = this.myAnim.GetBatchInstanceData();
						if (batchInstanceData != null)
						{
							batchInstanceData.SetOverrideTransformMatrix(overrideTransformMatrix);
						}
					}
				}
				base.transform.SetPosition(new Vector3(base.transform.GetPosition().x, base.transform.GetPosition().y, z));
				if (this.matchParentOffset)
				{
					this.myAnim.Offset = this.controller.Offset;
				}
				this.myAnim.SetDirty();
			}
		}
		if (this.myAnim != null && flag != this.myAnim.enabled && this.synchronizeEnabledState)
		{
			this.myAnim.enabled = flag;
		}
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x00092742 File Offset: 0x00090942
	[ContextMenu("ForceAlive")]
	private void OnAnimStart(HashedString name)
	{
		this.alive = true;
		base.enabled = true;
		this.forceUpdate = true;
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x00092759 File Offset: 0x00090959
	private void OnAnimStop(HashedString name)
	{
		if (!this.forceAlwaysAlive)
		{
			this.alive = false;
		}
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x0009276A File Offset: 0x0009096A
	private void OnLayerChanged(int layer)
	{
		this.myAnim.SetLayer(layer);
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x00092778 File Offset: 0x00090978
	public void SetTarget(Vector3 target)
	{
		this.targetPoint = target;
		this.targetPoint.z = 0f;
	}

	// Token: 0x04000FB6 RID: 4022
	public KBatchedAnimController controller;

	// Token: 0x04000FB7 RID: 4023
	public Vector3 offset = Vector3.zero;

	// Token: 0x04000FB8 RID: 4024
	public HashedString symbol;

	// Token: 0x04000FB9 RID: 4025
	public Vector3 targetPoint = Vector3.zero;

	// Token: 0x04000FBA RID: 4026
	public Vector3 previousTargetPoint;

	// Token: 0x04000FBB RID: 4027
	public bool useTargetPoint;

	// Token: 0x04000FBC RID: 4028
	public bool fadeOut = true;

	// Token: 0x04000FBD RID: 4029
	public bool forceAlwaysVisible;

	// Token: 0x04000FBE RID: 4030
	public bool matchParentOffset;

	// Token: 0x04000FBF RID: 4031
	public bool forceAlwaysAlive;

	// Token: 0x04000FC0 RID: 4032
	private bool alive = true;

	// Token: 0x04000FC1 RID: 4033
	private bool forceUpdate;

	// Token: 0x04000FC2 RID: 4034
	private Matrix2x3 previousMatrix;

	// Token: 0x04000FC3 RID: 4035
	private Vector3 previousPosition;

	// Token: 0x04000FC4 RID: 4036
	public bool synchronizeEnabledState = true;

	// Token: 0x04000FC5 RID: 4037
	[SerializeField]
	private KBatchedAnimController myAnim;
}
