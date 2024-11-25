using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200058A RID: 1418
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/MinimumOperatingTemperature")]
public class MinimumOperatingTemperature : KMonoBehaviour, ISim200ms, IGameObjectEffectDescriptor
{
	// Token: 0x060020F5 RID: 8437 RVA: 0x000B8943 File Offset: 0x000B6B43
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.TestTemperature(true);
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000B8952 File Offset: 0x000B6B52
	public void Sim200ms(float dt)
	{
		this.TestTemperature(false);
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000B895C File Offset: 0x000B6B5C
	private void TestTemperature(bool force)
	{
		bool flag;
		if (this.primaryElement.Temperature < this.minimumTemperature)
		{
			flag = false;
		}
		else
		{
			flag = true;
			for (int i = 0; i < this.building.PlacementCells.Length; i++)
			{
				int i2 = this.building.PlacementCells[i];
				float num = Grid.Temperature[i2];
				float num2 = Grid.Mass[i2];
				if ((num != 0f || num2 != 0f) && num < this.minimumTemperature)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			this.lastOffTime = Time.time;
		}
		if ((flag != this.isWarm && !flag) || (flag != this.isWarm && flag && Time.time > this.lastOffTime + 5f) || force)
		{
			this.isWarm = flag;
			this.operational.SetFlag(MinimumOperatingTemperature.warmEnoughFlag, this.isWarm);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.TooCold, !this.isWarm, this);
		}
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x000B8A66 File Offset: 0x000B6C66
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x060020F9 RID: 8441 RVA: 0x000B8A80 File Offset: 0x000B6C80
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x04001271 RID: 4721
	[MyCmpReq]
	private Building building;

	// Token: 0x04001272 RID: 4722
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001273 RID: 4723
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04001274 RID: 4724
	public float minimumTemperature = 275.15f;

	// Token: 0x04001275 RID: 4725
	private const float TURN_ON_DELAY = 5f;

	// Token: 0x04001276 RID: 4726
	private float lastOffTime;

	// Token: 0x04001277 RID: 4727
	public static readonly Operational.Flag warmEnoughFlag = new Operational.Flag("warm_enough", Operational.Flag.Type.Functional);

	// Token: 0x04001278 RID: 4728
	private bool isWarm;

	// Token: 0x04001279 RID: 4729
	private HandleVector<int>.Handle partitionerEntry;
}
