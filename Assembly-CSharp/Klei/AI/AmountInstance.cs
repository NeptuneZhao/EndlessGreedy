using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F3C RID: 3900
	[SerializationConfig(MemberSerialization.OptIn)]
	[DebuggerDisplay("{amount.Name} {value} ({deltaAttribute.value}/{minAttribute.value}/{maxAttribute.value})")]
	public class AmountInstance : ModifierInstance<Amount>, ISaveLoadable, ISim200ms
	{
		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060077EF RID: 30703 RVA: 0x002F7988 File Offset: 0x002F5B88
		public Amount amount
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060077F0 RID: 30704 RVA: 0x002F7990 File Offset: 0x002F5B90
		// (set) Token: 0x060077F1 RID: 30705 RVA: 0x002F7998 File Offset: 0x002F5B98
		public bool paused
		{
			get
			{
				return this._paused;
			}
			set
			{
				this._paused = this.paused;
				if (this._paused)
				{
					this.Deactivate();
					return;
				}
				this.Activate();
			}
		}

		// Token: 0x060077F2 RID: 30706 RVA: 0x002F79BB File Offset: 0x002F5BBB
		public float GetMin()
		{
			return this.minAttribute.GetTotalValue();
		}

		// Token: 0x060077F3 RID: 30707 RVA: 0x002F79C8 File Offset: 0x002F5BC8
		public float GetMax()
		{
			return this.maxAttribute.GetTotalValue();
		}

		// Token: 0x060077F4 RID: 30708 RVA: 0x002F79D5 File Offset: 0x002F5BD5
		public float GetDelta()
		{
			return this.deltaAttribute.GetTotalValue();
		}

		// Token: 0x060077F5 RID: 30709 RVA: 0x002F79E4 File Offset: 0x002F5BE4
		public AmountInstance(Amount amount, GameObject game_object) : base(game_object, amount)
		{
			Attributes attributes = game_object.GetAttributes();
			this.minAttribute = attributes.Add(amount.minAttribute);
			this.maxAttribute = attributes.Add(amount.maxAttribute);
			this.deltaAttribute = attributes.Add(amount.deltaAttribute);
		}

		// Token: 0x060077F6 RID: 30710 RVA: 0x002F7A36 File Offset: 0x002F5C36
		public float SetValue(float value)
		{
			this.value = Mathf.Min(Mathf.Max(value, this.GetMin()), this.GetMax());
			return this.value;
		}

		// Token: 0x060077F7 RID: 30711 RVA: 0x002F7A5C File Offset: 0x002F5C5C
		public void Publish(float delta, float previous_value)
		{
			if (this.OnDelta != null)
			{
				this.OnDelta(delta);
			}
			if (this.OnValueChanged != null && previous_value != this.value)
			{
				float obj = this.value - previous_value;
				this.OnValueChanged(obj);
			}
			if (this.OnMaxValueReached != null && previous_value < this.GetMax() && this.value >= this.GetMax())
			{
				this.OnMaxValueReached();
			}
			if (this.OnMinValueReached != null && previous_value > this.GetMin() && this.value <= this.GetMin())
			{
				this.OnMinValueReached();
			}
		}

		// Token: 0x060077F8 RID: 30712 RVA: 0x002F7AF8 File Offset: 0x002F5CF8
		public float ApplyDelta(float delta)
		{
			float previous_value = this.value;
			this.SetValue(this.value + delta);
			this.Publish(delta, previous_value);
			return this.value;
		}

		// Token: 0x060077F9 RID: 30713 RVA: 0x002F7B29 File Offset: 0x002F5D29
		public string GetValueString()
		{
			return this.amount.GetValueString(this);
		}

		// Token: 0x060077FA RID: 30714 RVA: 0x002F7B37 File Offset: 0x002F5D37
		public string GetDescription()
		{
			return this.amount.GetDescription(this);
		}

		// Token: 0x060077FB RID: 30715 RVA: 0x002F7B45 File Offset: 0x002F5D45
		public string GetTooltip()
		{
			return this.amount.GetTooltip(this);
		}

		// Token: 0x060077FC RID: 30716 RVA: 0x002F7B53 File Offset: 0x002F5D53
		public void Activate()
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x060077FD RID: 30717 RVA: 0x002F7B61 File Offset: 0x002F5D61
		public void Sim200ms(float dt)
		{
		}

		// Token: 0x060077FE RID: 30718 RVA: 0x002F7B64 File Offset: 0x002F5D64
		public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
		{
			if (time_delta == 0f)
			{
				return;
			}
			AmountInstance.BatchUpdateContext batchUpdateContext = new AmountInstance.BatchUpdateContext(amount_instances, time_delta);
			AmountInstance.batch_update_job.Reset(batchUpdateContext);
			int num = 512;
			for (int i = 0; i < amount_instances.Count; i += num)
			{
				int num2 = i + num;
				if (amount_instances.Count < num2)
				{
					num2 = amount_instances.Count;
				}
				AmountInstance.batch_update_job.Add(new AmountInstance.BatchUpdateTask(i, num2));
			}
			GlobalJobManager.Run(AmountInstance.batch_update_job);
			batchUpdateContext.Finish();
			AmountInstance.batch_update_job.Reset(null);
		}

		// Token: 0x060077FF RID: 30719 RVA: 0x002F7BE4 File Offset: 0x002F5DE4
		public void Deactivate()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x040059B6 RID: 22966
		[Serialize]
		public float value;

		// Token: 0x040059B7 RID: 22967
		public AttributeInstance minAttribute;

		// Token: 0x040059B8 RID: 22968
		public AttributeInstance maxAttribute;

		// Token: 0x040059B9 RID: 22969
		public AttributeInstance deltaAttribute;

		// Token: 0x040059BA RID: 22970
		public Action<float> OnDelta;

		// Token: 0x040059BB RID: 22971
		public Action<float> OnValueChanged;

		// Token: 0x040059BC RID: 22972
		public System.Action OnMaxValueReached;

		// Token: 0x040059BD RID: 22973
		public System.Action OnMinValueReached;

		// Token: 0x040059BE RID: 22974
		public bool hide;

		// Token: 0x040059BF RID: 22975
		private bool _paused;

		// Token: 0x040059C0 RID: 22976
		private static WorkItemCollection<AmountInstance.BatchUpdateTask, AmountInstance.BatchUpdateContext> batch_update_job = new WorkItemCollection<AmountInstance.BatchUpdateTask, AmountInstance.BatchUpdateContext>();

		// Token: 0x02002333 RID: 9011
		private class BatchUpdateContext
		{
			// Token: 0x0600B5F2 RID: 46578 RVA: 0x003C8B80 File Offset: 0x003C6D80
			public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
			{
				for (int num = 0; num != amount_instances.Count; num++)
				{
					UpdateBucketWithUpdater<ISim200ms>.Entry value = amount_instances[num];
					value.lastUpdateTime = 0f;
					amount_instances[num] = value;
				}
				this.amount_instances = amount_instances;
				this.time_delta = time_delta;
				this.results = ListPool<AmountInstance.BatchUpdateContext.Result, AmountInstance>.Allocate();
				this.results.Capacity = this.amount_instances.Count;
			}

			// Token: 0x0600B5F3 RID: 46579 RVA: 0x003C8BF0 File Offset: 0x003C6DF0
			public void Finish()
			{
				foreach (AmountInstance.BatchUpdateContext.Result result in this.results)
				{
					result.amount_instance.Publish(result.delta, result.previous);
				}
				this.results.Recycle();
			}

			// Token: 0x04009E02 RID: 40450
			public List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances;

			// Token: 0x04009E03 RID: 40451
			public float time_delta;

			// Token: 0x04009E04 RID: 40452
			public ListPool<AmountInstance.BatchUpdateContext.Result, AmountInstance>.PooledList results;

			// Token: 0x02003510 RID: 13584
			public struct Result
			{
				// Token: 0x0400D74B RID: 55115
				public AmountInstance amount_instance;

				// Token: 0x0400D74C RID: 55116
				public float previous;

				// Token: 0x0400D74D RID: 55117
				public float delta;
			}
		}

		// Token: 0x02002334 RID: 9012
		private struct BatchUpdateTask : IWorkItem<AmountInstance.BatchUpdateContext>
		{
			// Token: 0x0600B5F4 RID: 46580 RVA: 0x003C8C60 File Offset: 0x003C6E60
			public BatchUpdateTask(int start, int end)
			{
				this.start = start;
				this.end = end;
			}

			// Token: 0x0600B5F5 RID: 46581 RVA: 0x003C8C70 File Offset: 0x003C6E70
			public void Run(AmountInstance.BatchUpdateContext context)
			{
				for (int num = this.start; num != this.end; num++)
				{
					AmountInstance amountInstance = (AmountInstance)context.amount_instances[num].data;
					float num2 = amountInstance.GetDelta() * context.time_delta;
					if (num2 != 0f)
					{
						context.results.Add(new AmountInstance.BatchUpdateContext.Result
						{
							amount_instance = amountInstance,
							previous = amountInstance.value,
							delta = num2
						});
						amountInstance.SetValue(amountInstance.value + num2);
					}
				}
			}

			// Token: 0x04009E05 RID: 40453
			private int start;

			// Token: 0x04009E06 RID: 40454
			private int end;
		}
	}
}
