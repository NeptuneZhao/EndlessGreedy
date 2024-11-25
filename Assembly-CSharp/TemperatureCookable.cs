using System;
using UnityEngine;

// Token: 0x02000824 RID: 2084
[AddComponentMenu("KMonoBehaviour/scripts/TemperatureCookable")]
public class TemperatureCookable : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060039A2 RID: 14754 RVA: 0x0013A24D File Offset: 0x0013844D
	public void Sim1000ms(float dt)
	{
		if (this.element.Temperature > this.cookTemperature && this.cookedID != null)
		{
			this.Cook();
		}
	}

	// Token: 0x060039A3 RID: 14755 RVA: 0x0013A270 File Offset: 0x00138470
	private void Cook()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(this.cookedID), position);
		gameObject.SetActive(true);
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
		component2.Temperature = this.element.Temperature;
		component2.Mass = this.element.Mass;
		base.gameObject.DeleteObject();
	}

	// Token: 0x040022A9 RID: 8873
	[MyCmpReq]
	private PrimaryElement element;

	// Token: 0x040022AA RID: 8874
	public float cookTemperature = 273150f;

	// Token: 0x040022AB RID: 8875
	public string cookedID;
}
