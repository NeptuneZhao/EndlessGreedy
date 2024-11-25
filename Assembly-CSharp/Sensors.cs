using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BD RID: 1213
[AddComponentMenu("KMonoBehaviour/scripts/Sensors")]
public class Sensors : KMonoBehaviour
{
	// Token: 0x06001A25 RID: 6693 RVA: 0x0008AC34 File Offset: 0x00088E34
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<Brain>().onPreUpdate += this.OnBrainPreUpdate;
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x0008AC54 File Offset: 0x00088E54
	public SensorType GetSensor<SensorType>() where SensorType : Sensor
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (typeof(SensorType).IsAssignableFrom(sensor.GetType()))
			{
				return (SensorType)((object)sensor);
			}
		}
		global::Debug.LogError("Missing sensor of type: " + typeof(SensorType).Name);
		return default(SensorType);
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x0008ACEC File Offset: 0x00088EEC
	public void Add(Sensor sensor)
	{
		this.sensors.Add(sensor);
		if (sensor.IsEnabled)
		{
			sensor.Update();
		}
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x0008AD08 File Offset: 0x00088F08
	public void UpdateSensors()
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (sensor.IsEnabled)
			{
				sensor.Update();
			}
		}
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x0008AD64 File Offset: 0x00088F64
	private void OnBrainPreUpdate()
	{
		this.UpdateSensors();
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x0008AD6C File Offset: 0x00088F6C
	public void ShowEditor()
	{
		foreach (Sensor sensor in this.sensors)
		{
			sensor.ShowEditor();
		}
	}

	// Token: 0x04000EE2 RID: 3810
	public List<Sensor> sensors = new List<Sensor>();
}
