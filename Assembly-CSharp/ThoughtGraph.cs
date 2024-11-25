using System;
using System.Collections.Generic;

// Token: 0x020005CD RID: 1485
public class ThoughtGraph : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance>
{
	// Token: 0x06002435 RID: 9269 RVA: 0x000C9FCC File Offset: 0x000C81CC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.initialdelay;
		this.initialdelay.ScheduleGoTo(1f, this.nothoughts);
		this.nothoughts.OnSignal(this.thoughtsChanged, this.displayingthought, (ThoughtGraph.Instance smi) => smi.HasThoughts()).OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (ThoughtGraph.Instance smi) => smi.HasThoughts());
		this.displayingthought.DefaultState(this.displayingthought.pre).Enter("CreateBubble", delegate(ThoughtGraph.Instance smi)
		{
			smi.CreateBubble();
		}).Exit("DestroyBubble", delegate(ThoughtGraph.Instance smi)
		{
			smi.DestroyBubble();
		}).ScheduleGoTo((ThoughtGraph.Instance smi) => this.thoughtDisplayTime.Get(smi), this.cooldown);
		this.displayingthought.pre.ScheduleGoTo((ThoughtGraph.Instance smi) => TuningData<ThoughtGraph.Tuning>.Get().preLengthInSeconds, this.displayingthought.talking);
		this.displayingthought.talking.Enter(new StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State.Callback(ThoughtGraph.BeginTalking));
		this.cooldown.OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (ThoughtGraph.Instance smi) => smi.HasImmediateThought()).ScheduleGoTo(20f, this.nothoughts);
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x000CA17E File Offset: 0x000C837E
	private static void BeginTalking(ThoughtGraph.Instance smi)
	{
		if (smi.currentThought == null)
		{
			return;
		}
		if (SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
		{
			smi.GetSMI<SpeechMonitor.Instance>().PlaySpeech(smi.currentThought.speechPrefix, smi.currentThought.sound);
		}
	}

	// Token: 0x040014A4 RID: 5284
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.Signal thoughtsChanged;

	// Token: 0x040014A5 RID: 5285
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.Signal thoughtsChangedImmediate;

	// Token: 0x040014A6 RID: 5286
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.FloatParameter thoughtDisplayTime;

	// Token: 0x040014A7 RID: 5287
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State initialdelay;

	// Token: 0x040014A8 RID: 5288
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State nothoughts;

	// Token: 0x040014A9 RID: 5289
	public ThoughtGraph.DisplayingThoughtState displayingthought;

	// Token: 0x040014AA RID: 5290
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State cooldown;

	// Token: 0x020013C4 RID: 5060
	public class Tuning : TuningData<ThoughtGraph.Tuning>
	{
		// Token: 0x040067D7 RID: 26583
		public float preLengthInSeconds;
	}

	// Token: 0x020013C5 RID: 5061
	public class DisplayingThoughtState : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040067D8 RID: 26584
		public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State pre;

		// Token: 0x040067D9 RID: 26585
		public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State talking;
	}

	// Token: 0x020013C6 RID: 5062
	public new class Instance : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008846 RID: 34886 RVA: 0x0032E218 File Offset: 0x0032C418
		public Instance(IStateMachineTarget master) : base(master)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06008847 RID: 34887 RVA: 0x0032E23E File Offset: 0x0032C43E
		public bool HasThoughts()
		{
			return this.thoughts.Count > 0;
		}

		// Token: 0x06008848 RID: 34888 RVA: 0x0032E250 File Offset: 0x0032C450
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

		// Token: 0x06008849 RID: 34889 RVA: 0x0032E290 File Offset: 0x0032C490
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

		// Token: 0x0600884A RID: 34890 RVA: 0x0032E2ED File Offset: 0x0032C4ED
		public void RemoveThought(Thought thought)
		{
			if (!this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Remove(thought);
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x0600884B RID: 34891 RVA: 0x0032E321 File Offset: 0x0032C521
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

		// Token: 0x0600884C RID: 34892 RVA: 0x0032E350 File Offset: 0x0032C550
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

		// Token: 0x0600884D RID: 34893 RVA: 0x0032E429 File Offset: 0x0032C629
		public void DestroyBubble()
		{
			NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, false, null, null, null);
			NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, false, null, null, null, null);
		}

		// Token: 0x040067DA RID: 26586
		private List<Thought> thoughts = new List<Thought>();

		// Token: 0x040067DB RID: 26587
		public Thought currentThought;
	}
}
