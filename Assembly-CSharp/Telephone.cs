using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000B2F RID: 2863
public class Telephone : StateMachineComponent<Telephone.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06005569 RID: 21865 RVA: 0x001E7D04 File Offset: 0x001E5F04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Components.Telephones.Add(this);
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x0600556A RID: 21866 RVA: 0x001E7D63 File Offset: 0x001E5F63
	protected override void OnCleanUp()
	{
		Components.Telephones.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600556B RID: 21867 RVA: 0x001E7D78 File Offset: 0x001E5F78
	public void AddModifierDescriptions(List<Descriptor> descs, string effect_id)
	{
		Effect effect = Db.Get().effects.Get(effect_id);
		string text;
		string text2;
		if (effect.Id == this.babbleEffect)
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE_TOOLTIP;
		}
		else if (effect.Id == this.chatEffect)
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT_TOOLTIP;
		}
		else
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE_TOOLTIP;
		}
		foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
		{
			Descriptor item = new Descriptor(text.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()), text2.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Effect, false);
			item.IncreaseIndent();
			descs.Add(item);
		}
	}

	// Token: 0x0600556C RID: 21868 RVA: 0x001E7EE4 File Offset: 0x001E60E4
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		this.AddModifierDescriptions(list, this.babbleEffect);
		this.AddModifierDescriptions(list, this.chatEffect);
		this.AddModifierDescriptions(list, this.longDistanceEffect);
		return list;
	}

	// Token: 0x0600556D RID: 21869 RVA: 0x001E7F4A File Offset: 0x001E614A
	public void HangUp()
	{
		this.isInUse = false;
		this.wasAnswered = false;
		this.RemoveTag(GameTags.LongDistanceCall);
	}

	// Token: 0x040037E2 RID: 14306
	public string babbleEffect;

	// Token: 0x040037E3 RID: 14307
	public string chatEffect;

	// Token: 0x040037E4 RID: 14308
	public string longDistanceEffect;

	// Token: 0x040037E5 RID: 14309
	public string trackingEffect;

	// Token: 0x040037E6 RID: 14310
	public bool isInUse;

	// Token: 0x040037E7 RID: 14311
	public bool wasAnswered;

	// Token: 0x02001B85 RID: 7045
	public class States : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone>
	{
		// Token: 0x0600A3A0 RID: 41888 RVA: 0x0038A0B4 File Offset: 0x003882B4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Telephone.States.CreateStatusItems();
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleRecurringChore(new Func<Telephone.StatesInstance, Chore>(this.CreateChore), null).Enter(delegate(Telephone.StatesInstance smi)
			{
				using (List<Telephone>.Enumerator enumerator = Components.Telephones.Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.isInUse)
						{
							smi.GoTo(this.ready.speaker);
						}
					}
				}
			});
			this.ready.idle.WorkableStartTransition((Telephone.StatesInstance smi) => smi.master.GetComponent<TelephoneCallerWorkable>(), this.ready.calling.dial).TagTransition(GameTags.TelephoneRinging, this.ready.ringing, false).PlayAnim("off");
			this.ready.calling.ScheduleGoTo(15f, this.ready.talking.babbling);
			this.ready.calling.dial.PlayAnim("on_pre").OnAnimQueueComplete(this.ready.calling.animHack);
			this.ready.calling.animHack.ScheduleActionNextFrame("animHack_delay", delegate(Telephone.StatesInstance smi)
			{
				smi.GoTo(this.ready.calling.pre);
			});
			this.ready.calling.pre.PlayAnim("on").Enter(delegate(Telephone.StatesInstance smi)
			{
				this.RingAllTelephones(smi);
			}).OnAnimQueueComplete(this.ready.calling.wait);
			this.ready.calling.wait.PlayAnim("on", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (Telephone.StatesInstance smi) => smi.CallAnswered(), UpdateRate.SIM_4000ms);
			this.ready.ringing.PlayAnim("on_receiving", KAnim.PlayMode.Loop).Transition(this.ready.answer, (Telephone.StatesInstance smi) => smi.GetComponent<Telephone>().isInUse, UpdateRate.SIM_33ms).TagTransition(GameTags.TelephoneRinging, this.ready.speaker, true).ScheduleGoTo(15f, this.ready.speaker).Exit(delegate(Telephone.StatesInstance smi)
			{
				smi.GetComponent<Telephone>().RemoveTag(GameTags.TelephoneRinging);
			});
			this.ready.answer.PlayAnim("on_pre_loop_receiving").OnAnimQueueComplete(this.ready.talking.chatting);
			this.ready.talking.ScheduleGoTo(25f, this.ready.hangup).Enter(delegate(Telephone.StatesInstance smi)
			{
				this.UpdatePartyLine(smi);
			});
			this.ready.talking.babbling.PlayAnim("on_loop", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (Telephone.StatesInstance smi) => smi.CallAnswered(), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.babbling, null);
			this.ready.talking.chatting.PlayAnim("on_loop_pre").QueueAnim("on_loop", true, null).Transition(this.ready.talking.babbling, (Telephone.StatesInstance smi) => !smi.CallAnswered(), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.partyLine, null);
			this.ready.speaker.PlayAnim("on_loop_nobody", KAnim.PlayMode.Loop).Transition(this.ready, (Telephone.StatesInstance smi) => !smi.CallAnswered(), UpdateRate.SIM_4000ms).Transition(this.ready.answer, (Telephone.StatesInstance smi) => smi.GetComponent<Telephone>().isInUse, UpdateRate.SIM_33ms);
			this.ready.hangup.OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0600A3A1 RID: 41889 RVA: 0x0038A4EC File Offset: 0x003886EC
		private Chore CreateChore(Telephone.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<TelephoneCallerWorkable>();
			WorkChore<TelephoneCallerWorkable> workChore = new WorkChore<TelephoneCallerWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600A3A2 RID: 41890 RVA: 0x0038A54C File Offset: 0x0038874C
		public void UpdatePartyLine(Telephone.StatesInstance smi)
		{
			int myWorldId = smi.GetMyWorldId();
			bool flag = false;
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				telephone.RemoveTag(GameTags.TelephoneRinging);
				if (telephone.isInUse && myWorldId != telephone.GetMyWorldId())
				{
					flag = true;
					telephone.AddTag(GameTags.LongDistanceCall);
				}
			}
			Telephone component = smi.GetComponent<Telephone>();
			component.RemoveTag(GameTags.TelephoneRinging);
			if (flag)
			{
				component.AddTag(GameTags.LongDistanceCall);
			}
		}

		// Token: 0x0600A3A3 RID: 41891 RVA: 0x0038A5F4 File Offset: 0x003887F4
		public void RingAllTelephones(Telephone.StatesInstance smi)
		{
			Telephone component = smi.master.GetComponent<Telephone>();
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				if (component != telephone && telephone.GetComponent<Operational>().IsOperational)
				{
					TelephoneCallerWorkable component2 = telephone.GetComponent<TelephoneCallerWorkable>();
					if (component2 != null && component2.worker == null)
					{
						telephone.AddTag(GameTags.TelephoneRinging);
					}
				}
			}
		}

		// Token: 0x0600A3A4 RID: 41892 RVA: 0x0038A690 File Offset: 0x00388890
		private static void CreateStatusItems()
		{
			if (Telephone.States.partyLine == null)
			{
				Telephone.States.partyLine = new StatusItem("PartyLine", BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO, "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
				Telephone.States.partyLine.resolveStringCallback = delegate(string str, object obj)
				{
					Telephone component = ((Telephone.StatesInstance)obj).GetComponent<Telephone>();
					int num = 0;
					foreach (Telephone telephone in Components.Telephones.Items)
					{
						if (telephone.isInUse && telephone != component)
						{
							num++;
							if (num == 1)
							{
								str = str.Replace("{Asteroid}", telephone.GetMyWorld().GetProperName());
								str = str.Replace("{Duplicant}", telephone.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
							}
						}
					}
					if (num > 1)
					{
						str = string.Format(BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO_NUM, num);
					}
					return str;
				};
				Telephone.States.partyLine.resolveTooltipCallback = delegate(string str, object obj)
				{
					Telephone component = ((Telephone.StatesInstance)obj).GetComponent<Telephone>();
					foreach (Telephone telephone in Components.Telephones.Items)
					{
						if (telephone.isInUse && telephone != component)
						{
							string text = BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO;
							text = text.Replace("{Duplicant}", telephone.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
							text = text.Replace("{Asteroid}", telephone.GetMyWorld().GetProperName());
							str = str + text + "\n";
						}
					}
					return str;
				};
			}
			if (Telephone.States.babbling == null)
			{
				Telephone.States.babbling = new StatusItem("Babbling", BUILDING.STATUSITEMS.TELEPHONE.BABBLE.NAME, BUILDING.STATUSITEMS.TELEPHONE.BABBLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
				Telephone.States.babbling.resolveTooltipCallback = delegate(string str, object obj)
				{
					Telephone.StatesInstance statesInstance = (Telephone.StatesInstance)obj;
					str = str.Replace("{Duplicant}", statesInstance.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
					return str;
				};
			}
		}

		// Token: 0x04007FF7 RID: 32759
		private GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State unoperational;

		// Token: 0x04007FF8 RID: 32760
		private Telephone.States.ReadyStates ready;

		// Token: 0x04007FF9 RID: 32761
		private static StatusItem partyLine;

		// Token: 0x04007FFA RID: 32762
		private static StatusItem babbling;

		// Token: 0x02002623 RID: 9763
		public class ReadyStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
		{
			// Token: 0x0400A9AB RID: 43435
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State idle;

			// Token: 0x0400A9AC RID: 43436
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State ringing;

			// Token: 0x0400A9AD RID: 43437
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State answer;

			// Token: 0x0400A9AE RID: 43438
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State speaker;

			// Token: 0x0400A9AF RID: 43439
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State hangup;

			// Token: 0x0400A9B0 RID: 43440
			public Telephone.States.ReadyStates.CallingStates calling;

			// Token: 0x0400A9B1 RID: 43441
			public Telephone.States.ReadyStates.TalkingStates talking;

			// Token: 0x02003542 RID: 13634
			public class CallingStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
			{
				// Token: 0x0400D7C6 RID: 55238
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State dial;

				// Token: 0x0400D7C7 RID: 55239
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State animHack;

				// Token: 0x0400D7C8 RID: 55240
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State pre;

				// Token: 0x0400D7C9 RID: 55241
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State wait;
			}

			// Token: 0x02003543 RID: 13635
			public class TalkingStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
			{
				// Token: 0x0400D7CA RID: 55242
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State babbling;

				// Token: 0x0400D7CB RID: 55243
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State chatting;
			}
		}
	}

	// Token: 0x02001B86 RID: 7046
	public class StatesInstance : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.GameInstance
	{
		// Token: 0x0600A3AA RID: 41898 RVA: 0x0038A82E File Offset: 0x00388A2E
		public StatesInstance(Telephone smi) : base(smi)
		{
		}

		// Token: 0x0600A3AB RID: 41899 RVA: 0x0038A838 File Offset: 0x00388A38
		public bool CallAnswered()
		{
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				if (telephone.isInUse && telephone != base.smi.GetComponent<Telephone>())
				{
					telephone.wasAnswered = true;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A3AC RID: 41900 RVA: 0x0038A8B4 File Offset: 0x00388AB4
		public bool CallEnded()
		{
			using (List<Telephone>.Enumerator enumerator = Components.Telephones.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.isInUse)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
