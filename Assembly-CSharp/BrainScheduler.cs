using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000420 RID: 1056
[AddComponentMenu("KMonoBehaviour/scripts/BrainScheduler")]
public class BrainScheduler : KMonoBehaviour, IRenderEveryTick, ICPULoad
{
	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06001685 RID: 5765 RVA: 0x00078CE4 File Offset: 0x00076EE4
	private bool isAsyncPathProbeEnabled
	{
		get
		{
			return !TuningData<BrainScheduler.Tuning>.Get().disableAsyncPathProbes;
		}
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x00078CF3 File Offset: 0x00076EF3
	public List<BrainScheduler.BrainGroup> debugGetBrainGroups()
	{
		return this.brainGroups;
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x00078CFC File Offset: 0x00076EFC
	protected override void OnPrefabInit()
	{
		this.brainGroups.Add(new BrainScheduler.DupeBrainGroup());
		this.brainGroups.Add(new BrainScheduler.CreatureBrainGroup());
		Components.Brains.Register(new Action<Brain>(this.OnAddBrain), new Action<Brain>(this.OnRemoveBrain));
		CPUBudget.AddRoot(this);
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			CPUBudget.AddChild(this, brainGroup, brainGroup.LoadBalanceThreshold());
		}
		CPUBudget.FinalizeChildren(this);
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x00078DA4 File Offset: 0x00076FA4
	private void OnAddBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.AddBrain(brain);
				test = true;
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = this.isAsyncPathProbeEnabled;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x00078E2C File Offset: 0x0007702C
	private void OnRemoveBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				test = true;
				brainGroup.RemoveBrain(brain);
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = false;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x00078EB0 File Offset: 0x000770B0
	public void PrioritizeBrain(Brain brain)
	{
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.PrioritizeBrain(brain);
			}
		}
	}

	// Token: 0x0600168B RID: 5771 RVA: 0x00078F14 File Offset: 0x00077114
	public float GetEstimatedFrameTime()
	{
		return TuningData<BrainScheduler.Tuning>.Get().frameTime;
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x00078F20 File Offset: 0x00077120
	public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
	{
		return false;
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x00078F24 File Offset: 0x00077124
	public void RenderEveryTick(float dt)
	{
		if (Game.IsQuitting() || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			brainGroup.RenderEveryTick(dt);
		}
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x00078F84 File Offset: 0x00077184
	protected override void OnForcedCleanUp()
	{
		CPUBudget.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x04000C98 RID: 3224
	public const float millisecondsPerFrame = 33.33333f;

	// Token: 0x04000C99 RID: 3225
	public const float secondsPerFrame = 0.033333328f;

	// Token: 0x04000C9A RID: 3226
	public const float framesPerSecond = 30.000006f;

	// Token: 0x04000C9B RID: 3227
	private List<BrainScheduler.BrainGroup> brainGroups = new List<BrainScheduler.BrainGroup>();

	// Token: 0x02001187 RID: 4487
	private class Tuning : TuningData<BrainScheduler.Tuning>
	{
		// Token: 0x04006046 RID: 24646
		public bool disableAsyncPathProbes;

		// Token: 0x04006047 RID: 24647
		public float frameTime = 5f;
	}

	// Token: 0x02001188 RID: 4488
	public abstract class BrainGroup : ICPULoad
	{
		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06007FEB RID: 32747 RVA: 0x0030E896 File Offset: 0x0030CA96
		// (set) Token: 0x06007FEC RID: 32748 RVA: 0x0030E89E File Offset: 0x0030CA9E
		public Tag tag { get; private set; }

		// Token: 0x06007FED RID: 32749 RVA: 0x0030E8A8 File Offset: 0x0030CAA8
		protected BrainGroup(Tag tag)
		{
			this.tag = tag;
			this.probeSize = this.InitialProbeSize();
			this.probeCount = this.InitialProbeCount();
			string str = tag.ToString();
			this.increaseLoadLabel = "IncLoad" + str;
			this.decreaseLoadLabel = "DecLoad" + str;
		}

		// Token: 0x06007FEE RID: 32750 RVA: 0x0030E92B File Offset: 0x0030CB2B
		public void AddBrain(Brain brain)
		{
			this.brains.Add(brain);
		}

		// Token: 0x06007FEF RID: 32751 RVA: 0x0030E93C File Offset: 0x0030CB3C
		public void RemoveBrain(Brain brain)
		{
			int num = this.brains.IndexOf(brain);
			if (num != -1)
			{
				this.brains.RemoveAt(num);
				this.OnRemoveBrain(num, ref this.nextUpdateBrain);
				this.OnRemoveBrain(num, ref this.nextPathProbeBrain);
			}
			if (this.priorityBrains.Contains(brain))
			{
				List<Brain> list = new List<Brain>(this.priorityBrains);
				list.Remove(brain);
				this.priorityBrains = new Queue<Brain>(list);
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06007FF0 RID: 32752 RVA: 0x0030E9AE File Offset: 0x0030CBAE
		public int BrainCount
		{
			get
			{
				return this.brains.Count;
			}
		}

		// Token: 0x06007FF1 RID: 32753 RVA: 0x0030E9BB File Offset: 0x0030CBBB
		public void PrioritizeBrain(Brain brain)
		{
			if (!this.priorityBrains.Contains(brain))
			{
				this.priorityBrains.Enqueue(brain);
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06007FF2 RID: 32754 RVA: 0x0030E9D7 File Offset: 0x0030CBD7
		// (set) Token: 0x06007FF3 RID: 32755 RVA: 0x0030E9DF File Offset: 0x0030CBDF
		public int probeSize { get; private set; }

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06007FF4 RID: 32756 RVA: 0x0030E9E8 File Offset: 0x0030CBE8
		// (set) Token: 0x06007FF5 RID: 32757 RVA: 0x0030E9F0 File Offset: 0x0030CBF0
		public int probeCount { get; private set; }

		// Token: 0x06007FF6 RID: 32758 RVA: 0x0030E9FC File Offset: 0x0030CBFC
		public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
		{
			if (this.debugFreezeLoadAdustment)
			{
				return false;
			}
			bool flag = frameTimeDelta > 0f;
			int num = 0;
			int num2 = Math.Max(this.probeCount, Math.Min(this.brains.Count, CPUBudget.coreCount));
			num += num2 - this.probeCount;
			this.probeCount = num2;
			float num3 = Math.Min(1f, (float)this.probeCount / (float)CPUBudget.coreCount);
			float num4 = num3 * (float)this.probeSize;
			float num5 = num3 * (float)this.probeSize;
			float num6 = currentFrameTime / num5;
			float num7 = frameTimeDelta / num6;
			if (num == 0)
			{
				float num8 = num4 + num7 / (float)CPUBudget.coreCount;
				int num9 = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int)(num8 / num3));
				num += num9 - this.probeSize;
				this.probeSize = num9;
			}
			if (num == 0)
			{
				int num10 = Math.Max(1, (int)num3 + (flag ? 1 : -1));
				int probeSize = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int)((num5 + num7) / (float)num10));
				int num11 = Math.Min(this.brains.Count, num10 * CPUBudget.coreCount);
				num += num11 - this.probeCount;
				this.probeCount = num11;
				this.probeSize = probeSize;
			}
			if (num == 0 && flag)
			{
				int num12 = this.probeSize + this.ProbeSizeStep();
				num += num12 - this.probeSize;
				this.probeSize = num12;
			}
			if (num >= 0 && num <= 0 && this.brains.Count > 0)
			{
				global::Debug.LogWarning("AdjustLoad() failed");
			}
			return num != 0;
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x0030EB80 File Offset: 0x0030CD80
		public void ResetLoad()
		{
			this.probeSize = this.InitialProbeSize();
			this.probeCount = this.InitialProbeCount();
		}

		// Token: 0x06007FF8 RID: 32760 RVA: 0x0030EB9A File Offset: 0x0030CD9A
		private void IncrementBrainIndex(ref int brainIndex)
		{
			brainIndex++;
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x06007FF9 RID: 32761 RVA: 0x0030EBB4 File Offset: 0x0030CDB4
		private void ClampBrainIndex(ref int brainIndex)
		{
			brainIndex = MathUtil.Clamp(0, this.brains.Count - 1, brainIndex);
		}

		// Token: 0x06007FFA RID: 32762 RVA: 0x0030EBCD File Offset: 0x0030CDCD
		private void OnRemoveBrain(int removedIndex, ref int brainIndex)
		{
			if (removedIndex < brainIndex)
			{
				brainIndex--;
				return;
			}
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x06007FFB RID: 32763 RVA: 0x0030EBF0 File Offset: 0x0030CDF0
		private void AsyncPathProbe()
		{
			this.pathProbeJob.Reset(null);
			for (int num = 0; num != this.brains.Count; num++)
			{
				this.ClampBrainIndex(ref this.nextPathProbeBrain);
				Brain brain = this.brains[this.nextPathProbeBrain];
				if (brain.IsRunning())
				{
					Navigator component = brain.GetComponent<Navigator>();
					if (component != null)
					{
						component.executePathProbeTaskAsync = true;
						component.PathProber.potentialCellsPerUpdate = this.probeSize;
						component.pathProbeTask.Update();
						this.pathProbeJob.Add(component.pathProbeTask);
						if (this.pathProbeJob.Count == this.probeCount)
						{
							break;
						}
					}
				}
				this.IncrementBrainIndex(ref this.nextPathProbeBrain);
			}
			CPUBudget.Start(this);
			GlobalJobManager.Run(this.pathProbeJob);
			CPUBudget.End(this);
		}

		// Token: 0x06007FFC RID: 32764 RVA: 0x0030ECC8 File Offset: 0x0030CEC8
		public void RenderEveryTick(float dt)
		{
			this.BeginBrainGroupUpdate();
			int num = this.InitialProbeCount();
			int num2 = 0;
			while (num2 != this.brains.Count && num != 0)
			{
				this.ClampBrainIndex(ref this.nextUpdateBrain);
				this.debugMaxPriorityBrainCountSeen = Mathf.Max(this.debugMaxPriorityBrainCountSeen, this.priorityBrains.Count);
				Brain brain;
				if (this.AllowPriorityBrains() && this.priorityBrains.Count > 0)
				{
					brain = this.priorityBrains.Dequeue();
				}
				else
				{
					brain = this.brains[this.nextUpdateBrain];
					this.IncrementBrainIndex(ref this.nextUpdateBrain);
				}
				if (brain.IsRunning())
				{
					brain.UpdateBrain();
					num--;
				}
				num2++;
			}
			this.EndBrainGroupUpdate();
		}

		// Token: 0x06007FFD RID: 32765 RVA: 0x0030ED88 File Offset: 0x0030CF88
		public void AccumulatePathProbeIterations(Dictionary<string, int> pathProbeIterations)
		{
			foreach (Brain brain in this.brains)
			{
				Navigator component = brain.GetComponent<Navigator>();
				if (!(component == null) && !pathProbeIterations.ContainsKey(brain.name))
				{
					pathProbeIterations.Add(brain.name, component.PathProber.updateCount);
				}
			}
		}

		// Token: 0x06007FFE RID: 32766
		protected abstract int InitialProbeCount();

		// Token: 0x06007FFF RID: 32767
		protected abstract int InitialProbeSize();

		// Token: 0x06008000 RID: 32768
		protected abstract int MinProbeSize();

		// Token: 0x06008001 RID: 32769
		protected abstract int IdealProbeSize();

		// Token: 0x06008002 RID: 32770
		protected abstract int ProbeSizeStep();

		// Token: 0x06008003 RID: 32771
		public abstract float GetEstimatedFrameTime();

		// Token: 0x06008004 RID: 32772
		public abstract float LoadBalanceThreshold();

		// Token: 0x06008005 RID: 32773
		public abstract bool AllowPriorityBrains();

		// Token: 0x06008006 RID: 32774 RVA: 0x0030EE0C File Offset: 0x0030D00C
		public virtual void BeginBrainGroupUpdate()
		{
			if (Game.BrainScheduler.isAsyncPathProbeEnabled)
			{
				this.AsyncPathProbe();
			}
		}

		// Token: 0x06008007 RID: 32775 RVA: 0x0030EE20 File Offset: 0x0030D020
		public virtual void EndBrainGroupUpdate()
		{
		}

		// Token: 0x04006049 RID: 24649
		protected List<Brain> brains = new List<Brain>();

		// Token: 0x0400604A RID: 24650
		protected Queue<Brain> priorityBrains = new Queue<Brain>();

		// Token: 0x0400604B RID: 24651
		private string increaseLoadLabel;

		// Token: 0x0400604C RID: 24652
		private string decreaseLoadLabel;

		// Token: 0x0400604D RID: 24653
		public bool debugFreezeLoadAdustment;

		// Token: 0x0400604E RID: 24654
		public int debugMaxPriorityBrainCountSeen;

		// Token: 0x0400604F RID: 24655
		private WorkItemCollection<Navigator.PathProbeTask, object> pathProbeJob = new WorkItemCollection<Navigator.PathProbeTask, object>();

		// Token: 0x04006050 RID: 24656
		private int nextUpdateBrain;

		// Token: 0x04006051 RID: 24657
		private int nextPathProbeBrain;
	}

	// Token: 0x02001189 RID: 4489
	private class DupeBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x06008008 RID: 32776 RVA: 0x0030EE22 File Offset: 0x0030D022
		public DupeBrainGroup() : base(GameTags.DupeBrain)
		{
		}

		// Token: 0x06008009 RID: 32777 RVA: 0x0030EE36 File Offset: 0x0030D036
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x0600800A RID: 32778 RVA: 0x0030EE42 File Offset: 0x0030D042
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x0600800B RID: 32779 RVA: 0x0030EE4E File Offset: 0x0030D04E
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x0600800C RID: 32780 RVA: 0x0030EE5A File Offset: 0x0030D05A
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x0600800D RID: 32781 RVA: 0x0030EE66 File Offset: 0x0030D066
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x0600800E RID: 32782 RVA: 0x0030EE72 File Offset: 0x0030D072
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x0030EE7E File Offset: 0x0030D07E
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x06008010 RID: 32784 RVA: 0x0030EE8A File Offset: 0x0030D08A
		public override bool AllowPriorityBrains()
		{
			return this.usePriorityBrain;
		}

		// Token: 0x06008011 RID: 32785 RVA: 0x0030EE92 File Offset: 0x0030D092
		public override void BeginBrainGroupUpdate()
		{
			base.BeginBrainGroupUpdate();
			this.usePriorityBrain = !this.usePriorityBrain;
		}

		// Token: 0x04006054 RID: 24660
		private bool usePriorityBrain = true;

		// Token: 0x0200239C RID: 9116
		public class Tuning : TuningData<BrainScheduler.DupeBrainGroup.Tuning>
		{
			// Token: 0x04009F0D RID: 40717
			public int initialProbeCount = 1;

			// Token: 0x04009F0E RID: 40718
			public int initialProbeSize = 1000;

			// Token: 0x04009F0F RID: 40719
			public int minProbeSize = 100;

			// Token: 0x04009F10 RID: 40720
			public int idealProbeSize = 1000;

			// Token: 0x04009F11 RID: 40721
			public int probeSizeStep = 100;

			// Token: 0x04009F12 RID: 40722
			public float estimatedFrameTime = 2f;

			// Token: 0x04009F13 RID: 40723
			public float loadBalanceThreshold = 0.1f;
		}
	}

	// Token: 0x0200118A RID: 4490
	private class CreatureBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x06008012 RID: 32786 RVA: 0x0030EEA9 File Offset: 0x0030D0A9
		public CreatureBrainGroup() : base(GameTags.CreatureBrain)
		{
		}

		// Token: 0x06008013 RID: 32787 RVA: 0x0030EEB6 File Offset: 0x0030D0B6
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x06008014 RID: 32788 RVA: 0x0030EEC2 File Offset: 0x0030D0C2
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x06008015 RID: 32789 RVA: 0x0030EECE File Offset: 0x0030D0CE
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x06008016 RID: 32790 RVA: 0x0030EEDA File Offset: 0x0030D0DA
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x06008017 RID: 32791 RVA: 0x0030EEE6 File Offset: 0x0030D0E6
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x06008018 RID: 32792 RVA: 0x0030EEF2 File Offset: 0x0030D0F2
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x06008019 RID: 32793 RVA: 0x0030EEFE File Offset: 0x0030D0FE
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x0600801A RID: 32794 RVA: 0x0030EF0A File Offset: 0x0030D10A
		public override bool AllowPriorityBrains()
		{
			return true;
		}

		// Token: 0x0200239D RID: 9117
		public class Tuning : TuningData<BrainScheduler.CreatureBrainGroup.Tuning>
		{
			// Token: 0x04009F14 RID: 40724
			public int initialProbeCount = 5;

			// Token: 0x04009F15 RID: 40725
			public int initialProbeSize = 1000;

			// Token: 0x04009F16 RID: 40726
			public int minProbeSize = 100;

			// Token: 0x04009F17 RID: 40727
			public int idealProbeSize = 300;

			// Token: 0x04009F18 RID: 40728
			public int probeSizeStep = 100;

			// Token: 0x04009F19 RID: 40729
			public float estimatedFrameTime = 1f;

			// Token: 0x04009F1A RID: 40730
			public float loadBalanceThreshold = 0.1f;
		}
	}
}
