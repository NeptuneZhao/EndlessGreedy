using System;
using UnityEngine;

// Token: 0x02000C7C RID: 3196
public class PrefabDefinedUIPosition
{
	// Token: 0x0600626D RID: 25197 RVA: 0x0024C081 File Offset: 0x0024A281
	public void SetOn(GameObject gameObject)
	{
		if (this.position.HasValue)
		{
			gameObject.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = gameObject.rectTransform().anchoredPosition;
	}

	// Token: 0x0600626E RID: 25198 RVA: 0x0024C0BD File Offset: 0x0024A2BD
	public void SetOn(Component component)
	{
		if (this.position.HasValue)
		{
			component.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = component.rectTransform().anchoredPosition;
	}

	// Token: 0x040042CE RID: 17102
	private Option<Vector2> position;
}
