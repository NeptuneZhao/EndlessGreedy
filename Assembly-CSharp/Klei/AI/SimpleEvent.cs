using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F69 RID: 3945
	public class SimpleEvent : GameplayEvent<SimpleEvent.StatesInstance>
	{
		// Token: 0x06007916 RID: 30998 RVA: 0x002FE7E7 File Offset: 0x002FC9E7
		public SimpleEvent(string id, string title, string description, string animFileName, string buttonText = null, string buttonTooltip = null) : base(id, 0, 0)
		{
			this.title = title;
			this.description = description;
			this.buttonText = buttonText;
			this.buttonTooltip = buttonTooltip;
			this.animFileName = animFileName;
		}

		// Token: 0x06007917 RID: 30999 RVA: 0x002FE81D File Offset: 0x002FCA1D
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SimpleEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x04005A82 RID: 23170
		private string buttonText;

		// Token: 0x04005A83 RID: 23171
		private string buttonTooltip;

		// Token: 0x0200235C RID: 9052
		public class States : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>
		{
			// Token: 0x0600B680 RID: 46720 RVA: 0x003CBF76 File Offset: 0x003CA176
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600B681 RID: 46721 RVA: 0x003CBF8C File Offset: 0x003CA18C
			public override EventInfoData GenerateEventPopupData(SimpleEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.minions = smi.minions;
				eventInfoData.artifact = smi.artifact;
				EventInfoData.Option option = eventInfoData.AddOption(smi.gameplayEvent.buttonText, null);
				option.callback = delegate()
				{
					if (smi.callback != null)
					{
						smi.callback();
					}
					smi.StopSM("SimpleEvent Finished");
				};
				option.tooltip = smi.gameplayEvent.buttonTooltip;
				if (smi.textParameters != null)
				{
					foreach (global::Tuple<string, string> tuple in smi.textParameters)
					{
						eventInfoData.SetTextParameter(tuple.first, tuple.second);
					}
				}
				return eventInfoData;
			}

			// Token: 0x04009E84 RID: 40580
			public GameStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}

		// Token: 0x0200235D RID: 9053
		public class StatesInstance : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B683 RID: 46723 RVA: 0x003CC0A8 File Offset: 0x003CA2A8
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SimpleEvent simpleEvent) : base(master, eventInstance, simpleEvent)
			{
			}

			// Token: 0x0600B684 RID: 46724 RVA: 0x003CC0B3 File Offset: 0x003CA2B3
			public void SetTextParameter(string key, string value)
			{
				if (this.textParameters == null)
				{
					this.textParameters = new List<global::Tuple<string, string>>();
				}
				this.textParameters.Add(new global::Tuple<string, string>(key, value));
			}

			// Token: 0x0600B685 RID: 46725 RVA: 0x003CC0DA File Offset: 0x003CA2DA
			public void ShowEventPopup()
			{
				EventInfoScreen.ShowPopup(base.smi.sm.GenerateEventPopupData(base.smi));
			}

			// Token: 0x04009E85 RID: 40581
			public GameObject[] minions;

			// Token: 0x04009E86 RID: 40582
			public GameObject artifact;

			// Token: 0x04009E87 RID: 40583
			public List<global::Tuple<string, string>> textParameters;

			// Token: 0x04009E88 RID: 40584
			public System.Action callback;
		}
	}
}
