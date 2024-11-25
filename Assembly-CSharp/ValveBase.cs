using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000793 RID: 1939
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ValveBase")]
public class ValveBase : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06003504 RID: 13572 RVA: 0x00120EDF File Offset: 0x0011F0DF
	// (set) Token: 0x06003503 RID: 13571 RVA: 0x00120ED6 File Offset: 0x0011F0D6
	public float CurrentFlow
	{
		get
		{
			return this.currentFlow;
		}
		set
		{
			this.currentFlow = value;
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06003505 RID: 13573 RVA: 0x00120EE7 File Offset: 0x0011F0E7
	public HandleVector<int>.Handle AccumulatorHandle
	{
		get
		{
			return this.flowAccumulator;
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06003506 RID: 13574 RVA: 0x00120EEF File Offset: 0x0011F0EF
	public float MaxFlow
	{
		get
		{
			return this.maxFlow;
		}
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x00120EF7 File Offset: 0x0011F0F7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.flowAccumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x00120F1C File Offset: 0x0011F11C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.UpdateAnim();
		this.OnCmpEnable();
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x00120F77 File Offset: 0x0011F177
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.flowAccumulator);
		Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x00120FB4 File Offset: 0x0011F1B4
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
		ConduitFlow.Conduit conduit = flowManager.GetConduit(this.inputCell);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			this.OnMassTransfer(0f);
			this.UpdateAnim();
			return;
		}
		ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
		float num = Mathf.Min(contents.mass, this.currentFlow * dt);
		float num2 = 0f;
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			Game.Instance.accumulators.Accumulate(this.flowAccumulator, num2);
			if (num2 > 0f)
			{
				flowManager.RemoveElement(this.inputCell, num2);
			}
		}
		this.OnMassTransfer(num2);
		this.UpdateAnim();
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x001210A9 File Offset: 0x0011F2A9
	protected virtual void OnMassTransfer(float amount)
	{
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x001210AC File Offset: 0x0011F2AC
	public virtual void UpdateAnim()
	{
		float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
		if (averageRate > 0f)
		{
			int i = 0;
			while (i < this.animFlowRanges.Length)
			{
				if (averageRate <= this.animFlowRanges[i].minFlow)
				{
					if (this.curFlowIdx != i)
					{
						this.curFlowIdx = i;
						this.controller.Play(this.animFlowRanges[i].animName, (averageRate <= 0f) ? KAnim.PlayMode.Once : KAnim.PlayMode.Loop, 1f, 0f);
						return;
					}
					return;
				}
				else
				{
					i++;
				}
			}
			return;
		}
		this.controller.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001F73 RID: 8051
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04001F74 RID: 8052
	[SerializeField]
	public float maxFlow = 0.5f;

	// Token: 0x04001F75 RID: 8053
	[Serialize]
	private float currentFlow;

	// Token: 0x04001F76 RID: 8054
	[MyCmpGet]
	protected KBatchedAnimController controller;

	// Token: 0x04001F77 RID: 8055
	protected HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001F78 RID: 8056
	private int curFlowIdx = -1;

	// Token: 0x04001F79 RID: 8057
	private int inputCell;

	// Token: 0x04001F7A RID: 8058
	private int outputCell;

	// Token: 0x04001F7B RID: 8059
	[SerializeField]
	public ValveBase.AnimRangeInfo[] animFlowRanges;

	// Token: 0x0200164A RID: 5706
	[Serializable]
	public struct AnimRangeInfo
	{
		// Token: 0x060091C0 RID: 37312 RVA: 0x003516BD File Offset: 0x0034F8BD
		public AnimRangeInfo(float min_flow, string anim_name)
		{
			this.minFlow = min_flow;
			this.animName = anim_name;
		}

		// Token: 0x04006F3A RID: 28474
		public float minFlow;

		// Token: 0x04006F3B RID: 28475
		public string animName;
	}
}
