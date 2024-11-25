using System;
using UnityEngine;

// Token: 0x020004D7 RID: 1239
public class BatchAnimCamera : MonoBehaviour
{
	// Token: 0x06001AC9 RID: 6857 RVA: 0x0008CC72 File Offset: 0x0008AE72
	private void Awake()
	{
		this.cam = base.GetComponent<Camera>();
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x0008CC80 File Offset: 0x0008AE80
	private void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.right * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.left * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.up * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.down * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		this.ClampToBounds();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.do_pan = true;
				this.last_pan = KInputManager.GetMousePos();
			}
			else if (Input.GetMouseButton(0) && this.do_pan)
			{
				Vector3 vector = this.cam.ScreenToViewportPoint(this.last_pan - KInputManager.GetMousePos());
				Vector3 translation = new Vector3(vector.x * BatchAnimCamera.pan_speed, vector.y * BatchAnimCamera.pan_speed, 0f);
				base.transform.Translate(translation, Space.World);
				this.ClampToBounds();
				this.last_pan = KInputManager.GetMousePos();
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.do_pan = false;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			this.cam.fieldOfView = Mathf.Clamp(this.cam.fieldOfView - axis * BatchAnimCamera.zoom_speed, this.zoom_min, this.zoom_max);
		}
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x0008CE84 File Offset: 0x0008B084
	private void ClampToBounds()
	{
		Vector3 position = base.transform.GetPosition();
		position.x = Mathf.Clamp(base.transform.GetPosition().x, BatchAnimCamera.bounds.min.x, BatchAnimCamera.bounds.max.x);
		position.y = Mathf.Clamp(base.transform.GetPosition().y, BatchAnimCamera.bounds.min.y, BatchAnimCamera.bounds.max.y);
		position.z = Mathf.Clamp(base.transform.GetPosition().z, BatchAnimCamera.bounds.min.z, BatchAnimCamera.bounds.max.z);
		base.transform.SetPosition(position);
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x0008CF58 File Offset: 0x0008B158
	private void OnDrawGizmosSelected()
	{
		DebugExtension.DebugBounds(BatchAnimCamera.bounds, Color.red, 0f, true);
	}

	// Token: 0x04000F2D RID: 3885
	private static readonly float pan_speed = 5f;

	// Token: 0x04000F2E RID: 3886
	private static readonly float zoom_speed = 5f;

	// Token: 0x04000F2F RID: 3887
	public static Bounds bounds = new Bounds(new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f));

	// Token: 0x04000F30 RID: 3888
	private float zoom_min = 1f;

	// Token: 0x04000F31 RID: 3889
	private float zoom_max = 100f;

	// Token: 0x04000F32 RID: 3890
	private Camera cam;

	// Token: 0x04000F33 RID: 3891
	private bool do_pan;

	// Token: 0x04000F34 RID: 3892
	private Vector3 last_pan;
}
