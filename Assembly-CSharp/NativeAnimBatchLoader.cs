using System;
using UnityEngine;

// Token: 0x020004EE RID: 1262
public class NativeAnimBatchLoader : MonoBehaviour
{
	// Token: 0x06001C0E RID: 7182 RVA: 0x000938F8 File Offset: 0x00091AF8
	private void Start()
	{
		if (this.generateObjects)
		{
			for (int i = 0; i < this.enableObjects.Length; i++)
			{
				if (this.enableObjects[i] != null)
				{
					this.enableObjects[i].GetComponent<KBatchedAnimController>().visibilityType = KAnimControllerBase.VisibilityType.Always;
					this.enableObjects[i].SetActive(true);
				}
			}
		}
		if (this.setTimeScale)
		{
			Time.timeScale = 1f;
		}
		if (this.destroySelf)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x00093974 File Offset: 0x00091B74
	private void LateUpdate()
	{
		if (this.destroySelf)
		{
			return;
		}
		if (this.performUpdate)
		{
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
		}
		if (this.performRender)
		{
			KAnimBatchManager.Instance().Render();
		}
	}

	// Token: 0x04000FD8 RID: 4056
	public bool performTimeUpdate;

	// Token: 0x04000FD9 RID: 4057
	public bool performUpdate;

	// Token: 0x04000FDA RID: 4058
	public bool performRender;

	// Token: 0x04000FDB RID: 4059
	public bool setTimeScale;

	// Token: 0x04000FDC RID: 4060
	public bool destroySelf;

	// Token: 0x04000FDD RID: 4061
	public bool generateObjects;

	// Token: 0x04000FDE RID: 4062
	public GameObject[] enableObjects;
}
