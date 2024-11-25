using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007F7 RID: 2039
public class CreatureLightToggleController : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>
{
	// Token: 0x06003864 RID: 14436 RVA: 0x00133D80 File Offset: 0x00131F80
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.light_off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.light_off.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(false);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_on, new Func<CreatureLightToggleController.Instance, object, bool>(CreatureLightToggleController.ShouldProduceLight));
		this.turning_off.BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.dim, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_off, (CreatureLightToggleController.Instance smi) => smi.IsOff(), UpdateRate.SIM_200ms);
		this.light_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_off, (CreatureLightToggleController.Instance smi, object obj) => !CreatureLightToggleController.ShouldProduceLight(smi, obj));
		this.turning_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.brighten, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_on, (CreatureLightToggleController.Instance smi) => smi.IsOn(), UpdateRate.SIM_200ms);
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x00133F0F File Offset: 0x0013210F
	public static bool ShouldProduceLight(CreatureLightToggleController.Instance smi, object obj)
	{
		return !smi.prefabID.HasTag(GameTags.Creatures.Overcrowded) && !smi.prefabID.HasTag(GameTags.Creatures.TrappedInCargoBay);
	}

	// Token: 0x040021DB RID: 8667
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_off;

	// Token: 0x040021DC RID: 8668
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_off;

	// Token: 0x040021DD RID: 8669
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_on;

	// Token: 0x040021DE RID: 8670
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_on;

	// Token: 0x020016D4 RID: 5844
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020016D5 RID: 5845
	public new class Instance : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.GameInstance
	{
		// Token: 0x060093A5 RID: 37797 RVA: 0x00359B08 File Offset: 0x00357D08
		public Instance(IStateMachineTarget master, CreatureLightToggleController.Def def) : base(master, def)
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			this.light = master.GetComponent<Light2D>();
			this.originalLux = this.light.Lux;
			this.originalRange = this.light.Range;
		}

		// Token: 0x060093A6 RID: 37798 RVA: 0x00359B5C File Offset: 0x00357D5C
		public void SwitchLight(bool on)
		{
			this.light.enabled = on;
		}

		// Token: 0x060093A7 RID: 37799 RVA: 0x00359B6C File Offset: 0x00357D6C
		public static void ModifyBrightness(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, CreatureLightToggleController.Instance.ModifyLuxDelegate modify_lux, float time_delta)
		{
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
			for (int num = 0; num != instances.Count; num++)
			{
				UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry entry = instances[num];
				entry.lastUpdateTime = 0f;
				instances[num] = entry;
				CreatureLightToggleController.Instance data = entry.data;
				modify_lux(data, time_delta);
				data.light.Range = data.originalRange * (float)data.light.Lux / (float)data.originalLux;
				data.light.RefreshShapeAndPosition();
				if (data.light.RefreshShapeAndPosition() != Light2D.RefreshResult.None)
				{
					CreatureLightToggleController.Instance.modify_brightness_job.Add(new CreatureLightToggleController.Instance.ModifyBrightnessTask(data.light.emitter));
				}
			}
			GlobalJobManager.Run(CreatureLightToggleController.Instance.modify_brightness_job);
			for (int num2 = 0; num2 != CreatureLightToggleController.Instance.modify_brightness_job.Count; num2++)
			{
				CreatureLightToggleController.Instance.modify_brightness_job.GetWorkItem(num2).Finish();
			}
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
		}

		// Token: 0x060093A8 RID: 37800 RVA: 0x00359C5D File Offset: 0x00357E5D
		public bool IsOff()
		{
			return this.light.Lux == 0;
		}

		// Token: 0x060093A9 RID: 37801 RVA: 0x00359C6D File Offset: 0x00357E6D
		public bool IsOn()
		{
			return this.light.Lux >= this.originalLux;
		}

		// Token: 0x040070E7 RID: 28903
		private const float DIM_TIME = 25f;

		// Token: 0x040070E8 RID: 28904
		private const float GLOW_TIME = 15f;

		// Token: 0x040070E9 RID: 28905
		private int originalLux;

		// Token: 0x040070EA RID: 28906
		private float originalRange;

		// Token: 0x040070EB RID: 28907
		private Light2D light;

		// Token: 0x040070EC RID: 28908
		public KPrefabID prefabID;

		// Token: 0x040070ED RID: 28909
		private static WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object> modify_brightness_job = new WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object>();

		// Token: 0x040070EE RID: 28910
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate dim = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 25f;
			instance.light.Lux = Mathf.FloorToInt(Mathf.Max(0f, (float)instance.light.Lux - num * time_delta));
		};

		// Token: 0x040070EF RID: 28911
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate brighten = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 15f;
			instance.light.Lux = Mathf.CeilToInt(Mathf.Min((float)instance.originalLux, (float)instance.light.Lux + num * time_delta));
		};

		// Token: 0x02002575 RID: 9589
		private struct ModifyBrightnessTask : IWorkItem<object>
		{
			// Token: 0x0600BED9 RID: 48857 RVA: 0x003DA9AC File Offset: 0x003D8BAC
			public ModifyBrightnessTask(LightGridManager.LightGridEmitter emitter)
			{
				this.emitter = emitter;
				emitter.RemoveFromGrid();
			}

			// Token: 0x0600BEDA RID: 48858 RVA: 0x003DA9BB File Offset: 0x003D8BBB
			public void Run(object context)
			{
				this.emitter.UpdateLitCells();
			}

			// Token: 0x0600BEDB RID: 48859 RVA: 0x003DA9C8 File Offset: 0x003D8BC8
			public void Finish()
			{
				this.emitter.AddToGrid(false);
			}

			// Token: 0x0400A6CB RID: 42699
			private LightGridManager.LightGridEmitter emitter;
		}

		// Token: 0x02002576 RID: 9590
		// (Invoke) Token: 0x0600BEDD RID: 48861
		public delegate void ModifyLuxDelegate(CreatureLightToggleController.Instance instance, float time_delta);
	}
}
