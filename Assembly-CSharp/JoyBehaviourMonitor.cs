using System;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200098B RID: 2443
public class JoyBehaviourMonitor : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance>
{
	// Token: 0x0600474D RID: 18253 RVA: 0x00197F3C File Offset: 0x0019613C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.EventHandler(GameHashes.TagsChanged, delegate(JoyBehaviourMonitor.Instance smi, object data)
		{
			TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
			if (!tagChangedEventData.added)
			{
				return;
			}
			if (tagChangedEventData.tag == GameTags.PleasantConversation && UnityEngine.Random.Range(0f, 100f) <= 1f)
			{
				smi.GoToOverjoyed();
			}
			smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PleasantConversation);
		}).EventHandler(GameHashes.SleepFinished, delegate(JoyBehaviourMonitor.Instance smi)
		{
			if (smi.ShouldBeOverjoyed())
			{
				smi.GoToOverjoyed();
			}
		}).EventHandler(GameHashes.PowerSaveFinished, delegate(JoyBehaviourMonitor.Instance smi)
		{
			if (smi.ShouldBeOverjoyed())
			{
				smi.GoToOverjoyed();
			}
		});
		this.overjoyed.Transition(this.neutral, (JoyBehaviourMonitor.Instance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms).ToggleExpression((JoyBehaviourMonitor.Instance smi) => smi.happyExpression).ToggleAnims((JoyBehaviourMonitor.Instance smi) => smi.happyLocoAnim).ToggleAnims((JoyBehaviourMonitor.Instance smi) => smi.happyLocoWalkAnim).ToggleTag(GameTags.Overjoyed).Exit(delegate(JoyBehaviourMonitor.Instance smi)
		{
			smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PleasantConversation);
		}).OnSignal(this.exitEarly, this.neutral);
	}

	// Token: 0x04002E8E RID: 11918
	public StateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.Signal exitEarly;

	// Token: 0x04002E8F RID: 11919
	public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04002E90 RID: 11920
	public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State overjoyed;

	// Token: 0x0200194C RID: 6476
	public new class Instance : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C07 RID: 39943 RVA: 0x003717B0 File Offset: 0x0036F9B0
		public Instance(IStateMachineTarget master, string happy_loco_anim, string happy_loco_walk_anim, Expression happy_expression) : base(master)
		{
			this.happyLocoAnim = happy_loco_anim;
			this.happyLocoWalkAnim = happy_loco_walk_anim;
			this.happyExpression = happy_expression;
			Attributes attributes = base.gameObject.GetAttributes();
			this.expectationAttribute = attributes.Add(Db.Get().Attributes.QualityOfLifeExpectation);
			this.qolAttribute = Db.Get().Attributes.QualityOfLife.Lookup(base.gameObject);
		}

		// Token: 0x06009C08 RID: 39944 RVA: 0x00371838 File Offset: 0x0036FA38
		public bool ShouldBeOverjoyed()
		{
			float totalValue = this.qolAttribute.GetTotalValue();
			float totalValue2 = this.expectationAttribute.GetTotalValue();
			float num = totalValue - totalValue2;
			if (num >= TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS)
			{
				float num2 = MathUtil.ReRange(num, TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MAX_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MIN_REACTION_CHANCE, TRAITS.JOY_REACTIONS.MAX_REACTION_CHANCE);
				return UnityEngine.Random.Range(0f, 100f) <= num2;
			}
			return false;
		}

		// Token: 0x06009C09 RID: 39945 RVA: 0x00371899 File Offset: 0x0036FA99
		public void GoToOverjoyed()
		{
			base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
			base.smi.GoTo(base.smi.sm.overjoyed);
		}

		// Token: 0x04007906 RID: 30982
		public string happyLocoAnim = "";

		// Token: 0x04007907 RID: 30983
		public string happyLocoWalkAnim = "";

		// Token: 0x04007908 RID: 30984
		public Expression happyExpression;

		// Token: 0x04007909 RID: 30985
		[Serialize]
		public float transitionTime;

		// Token: 0x0400790A RID: 30986
		private AttributeInstance expectationAttribute;

		// Token: 0x0400790B RID: 30987
		private AttributeInstance qolAttribute;
	}
}
