using System;
using System.Collections.Generic;

// Token: 0x02000540 RID: 1344
public class CreatureThoughtGraph : GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>
{
	// Token: 0x06001EEB RID: 7915 RVA: 0x000AD0B0 File Offset: 0x000AB2B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.initialdelay;
		this.initialdelay.ScheduleGoTo(1f, this.nothoughts);
		this.nothoughts.OnSignal(this.thoughtsChanged, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasThoughts()).OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasThoughts());
		this.displayingthought.Enter("CreateBubble", delegate(CreatureThoughtGraph.Instance smi)
		{
			smi.CreateBubble();
		}).Exit("DestroyBubble", delegate(CreatureThoughtGraph.Instance smi)
		{
			smi.DestroyBubble();
		}).ScheduleGoTo((CreatureThoughtGraph.Instance smi) => this.thoughtDisplayTime.Get(smi), this.cooldown);
		this.cooldown.OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasImmediateThought()).ScheduleGoTo(20f, this.nothoughts);
	}

	// Token: 0x0400116C RID: 4460
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChanged;

	// Token: 0x0400116D RID: 4461
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChangedImmediate;

	// Token: 0x0400116E RID: 4462
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.FloatParameter thoughtDisplayTime;

	// Token: 0x0400116F RID: 4463
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State initialdelay;

	// Token: 0x04001170 RID: 4464
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State nothoughts;

	// Token: 0x04001171 RID: 4465
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State displayingthought;

	// Token: 0x04001172 RID: 4466
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State cooldown;

	// Token: 0x02001304 RID: 4868
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001305 RID: 4869
	public new class Instance : GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.GameInstance
	{
		// Token: 0x0600857A RID: 34170 RVA: 0x00326226 File Offset: 0x00324426
		public Instance(IStateMachineTarget master, CreatureThoughtGraph.Def def) : base(master, def)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x0600857B RID: 34171 RVA: 0x0032624D File Offset: 0x0032444D
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x0600857C RID: 34172 RVA: 0x00326255 File Offset: 0x00324455
		public bool HasThoughts()
		{
			return this.thoughts.Count > 0;
		}

		// Token: 0x0600857D RID: 34173 RVA: 0x00326268 File Offset: 0x00324468
		public bool HasImmediateThought()
		{
			bool result = false;
			for (int i = 0; i < this.thoughts.Count; i++)
			{
				if (this.thoughts[i].showImmediately)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600857E RID: 34174 RVA: 0x003262A8 File Offset: 0x003244A8
		public void AddThought(Thought thought)
		{
			if (this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Add(thought);
			if (thought.showImmediately)
			{
				base.sm.thoughtsChangedImmediate.Trigger(base.smi);
				return;
			}
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x0600857F RID: 34175 RVA: 0x00326305 File Offset: 0x00324505
		public void RemoveThought(Thought thought)
		{
			if (!this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Remove(thought);
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x06008580 RID: 34176 RVA: 0x00326339 File Offset: 0x00324539
		private int SortThoughts(Thought a, Thought b)
		{
			if (a.showImmediately == b.showImmediately)
			{
				return b.priority.CompareTo(a.priority);
			}
			if (!a.showImmediately)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06008581 RID: 34177 RVA: 0x00326368 File Offset: 0x00324568
		public void CreateBubble()
		{
			if (this.thoughts.Count == 0)
			{
				return;
			}
			this.thoughts.Sort(new Comparison<Thought>(this.SortThoughts));
			Thought thought = this.thoughts[0];
			if (thought.modeSprite != null)
			{
				NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, true, thought.hoverText, thought.bubbleSprite, thought.sprite, thought.modeSprite);
			}
			else
			{
				NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, true, thought.hoverText, thought.bubbleSprite, thought.sprite);
			}
			base.sm.thoughtDisplayTime.Set(thought.showTime, this, false);
			this.currentThought = thought;
			if (thought.showImmediately)
			{
				this.thoughts.RemoveAt(0);
			}
		}

		// Token: 0x06008582 RID: 34178 RVA: 0x00326441 File Offset: 0x00324641
		public void DestroyBubble()
		{
			NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, false, null, null, null);
			NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, false, null, null, null, null);
		}

		// Token: 0x04006541 RID: 25921
		private List<Thought> thoughts = new List<Thought>();

		// Token: 0x04006542 RID: 25922
		public Thought currentThought;
	}
}
