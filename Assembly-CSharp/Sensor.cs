using System;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public class Sensor
{
	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06001A19 RID: 6681 RVA: 0x0008AB83 File Offset: 0x00088D83
	// (set) Token: 0x06001A18 RID: 6680 RVA: 0x0008AB7A File Offset: 0x00088D7A
	public bool IsEnabled { get; private set; } = true;

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06001A1A RID: 6682 RVA: 0x0008AB8B File Offset: 0x00088D8B
	// (set) Token: 0x06001A1B RID: 6683 RVA: 0x0008AB93 File Offset: 0x00088D93
	public string Name { get; private set; }

	// Token: 0x06001A1C RID: 6684 RVA: 0x0008AB9C File Offset: 0x00088D9C
	public Sensor(Sensors sensors, bool active)
	{
		this.sensors = sensors;
		this.SetActive(active);
		this.Name = base.GetType().Name;
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x0008ABCA File Offset: 0x00088DCA
	public Sensor(Sensors sensors)
	{
		this.sensors = sensors;
		this.Name = base.GetType().Name;
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public ComponentType GetComponent<ComponentType>()
	{
		return this.sensors.GetComponent<ComponentType>();
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06001A1F RID: 6687 RVA: 0x0008ABFE File Offset: 0x00088DFE
	public GameObject gameObject
	{
		get
		{
			return this.sensors.gameObject;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0008AC0B File Offset: 0x00088E0B
	public Transform transform
	{
		get
		{
			return this.gameObject.transform;
		}
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x0008AC18 File Offset: 0x00088E18
	public void SetActive(bool enabled)
	{
		this.IsEnabled = enabled;
	}

	// Token: 0x06001A22 RID: 6690 RVA: 0x0008AC21 File Offset: 0x00088E21
	public void Trigger(int hash, object data = null)
	{
		this.sensors.Trigger(hash, data);
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x0008AC30 File Offset: 0x00088E30
	public virtual void Update()
	{
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x0008AC32 File Offset: 0x00088E32
	public virtual void ShowEditor()
	{
	}

	// Token: 0x04000EE0 RID: 3808
	protected Sensors sensors;
}
