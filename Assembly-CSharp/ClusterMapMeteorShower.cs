using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000ABB RID: 2747
public class ClusterMapMeteorShower : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>
{
	// Token: 0x060050FE RID: 20734 RVA: 0x001D1380 File Offset: 0x001CF580
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.traveling;
		this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.arrived, null);
		this.traveling.unidentified.ParamTransition<bool>(this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsTrue);
		this.traveling.identified.ParamTransition<bool>(this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime, null);
		this.arrived.Enter(new StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State.Callback(ClusterMapMeteorShower.DestinationReached));
	}

	// Token: 0x060050FF RID: 20735 RVA: 0x001D143F File Offset: 0x001CF63F
	public static void DestinationReached(ClusterMapMeteorShower.Instance smi)
	{
		smi.DestinationReached();
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x040035D4 RID: 13780
	public StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.BoolParameter IsIdentified;

	// Token: 0x040035D5 RID: 13781
	public ClusterMapMeteorShower.TravelingState traveling;

	// Token: 0x040035D6 RID: 13782
	public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State arrived;

	// Token: 0x02001AFC RID: 6908
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600A1DB RID: 41435 RVA: 0x00384204 File Offset: 0x00382404
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			GameplayEvent gameplayEvent = Db.Get().GameplayEvents.Get(this.eventID);
			List<Descriptor> list = new List<Descriptor>();
			ClusterMapMeteorShower.Instance smi = go.GetSMI<ClusterMapMeteorShower.Instance>();
			if (smi != null && smi.sm.IsIdentified.Get(smi) && gameplayEvent is MeteorShowerEvent)
			{
				List<MeteorShowerEvent.BombardmentInfo> meteorsInfo = (gameplayEvent as MeteorShowerEvent).GetMeteorsInfo();
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in meteorsInfo)
				{
					num += bombardmentInfo.weight;
				}
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo2 in meteorsInfo)
				{
					GameObject prefab = Assets.GetPrefab(bombardmentInfo2.prefab);
					string formattedPercent = GameUtil.GetFormattedPercent((float)Mathf.RoundToInt(bombardmentInfo2.weight / num * 100f), GameUtil.TimeSlice.None);
					string txt = prefab.GetProperName() + " " + formattedPercent;
					Descriptor item = new Descriptor(txt, UI.GAMEOBJECTEFFECTS.TOOLTIPS.METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP, Descriptor.DescriptorType.Effect, false);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x04007E5F RID: 32351
		public string name;

		// Token: 0x04007E60 RID: 32352
		public string description;

		// Token: 0x04007E61 RID: 32353
		public string description_Hidden;

		// Token: 0x04007E62 RID: 32354
		public string name_Hidden;

		// Token: 0x04007E63 RID: 32355
		public string eventID;

		// Token: 0x04007E64 RID: 32356
		public int destinationWorldID;

		// Token: 0x04007E65 RID: 32357
		public float arrivalTime;
	}

	// Token: 0x02001AFD RID: 6909
	public class TravelingState : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State
	{
		// Token: 0x04007E66 RID: 32358
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State unidentified;

		// Token: 0x04007E67 RID: 32359
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State identified;
	}

	// Token: 0x02001AFE RID: 6910
	public new class Instance : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x0600A1DE RID: 41438 RVA: 0x00384360 File Offset: 0x00382560
		public WorldContainer World_Destination
		{
			get
			{
				return ClusterManager.Instance.GetWorld(this.DestinationWorldID);
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x0600A1DF RID: 41439 RVA: 0x00384372 File Offset: 0x00382572
		public string SidescreenButtonText
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identify";
				}
				return "Dev Hide";
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600A1E0 RID: 41440 RVA: 0x0038439C File Offset: 0x0038259C
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identifies the meteor shower";
				}
				return "Dev unidentify back";
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600A1E1 RID: 41441 RVA: 0x003843C6 File Offset: 0x003825C6
		public bool HasBeenIdentified
		{
			get
			{
				return base.sm.IsIdentified.Get(this);
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x0600A1E2 RID: 41442 RVA: 0x003843D9 File Offset: 0x003825D9
		public float IdentifyingProgress
		{
			get
			{
				return this.identifyingProgress;
			}
		}

		// Token: 0x0600A1E3 RID: 41443 RVA: 0x003843E1 File Offset: 0x003825E1
		public AxialI ClusterGridPosition()
		{
			return this.visualizer.Location;
		}

		// Token: 0x0600A1E4 RID: 41444 RVA: 0x003843EE File Offset: 0x003825EE
		public Instance(IStateMachineTarget master, ClusterMapMeteorShower.Def def) : base(master, def)
		{
			this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
			this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
		}

		// Token: 0x0600A1E5 RID: 41445 RVA: 0x0038442D File Offset: 0x0038262D
		private void OnTravellerMoved()
		{
			Game.Instance.Trigger(-1975776133, this);
		}

		// Token: 0x0600A1E6 RID: 41446 RVA: 0x0038443F File Offset: 0x0038263F
		protected override void OnCleanUp()
		{
			this.visualizer.Deselect();
			base.OnCleanUp();
		}

		// Token: 0x0600A1E7 RID: 41447 RVA: 0x00384454 File Offset: 0x00382654
		public void Identify()
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress = 1f;
				base.sm.IsIdentified.Set(true, this, false);
				Game.Instance.Trigger(1427028915, this);
				this.RefreshVisuals(true);
				if (ClusterMapScreen.Instance.IsActive())
				{
					KFMOD.PlayUISound(GlobalAssets.GetSound("ClusterMapMeteor_Reveal", false));
				}
			}
		}

		// Token: 0x0600A1E8 RID: 41448 RVA: 0x003844BC File Offset: 0x003826BC
		public void ProgressIdentifiction(float points)
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress += points;
				this.identifyingProgress = Mathf.Clamp(this.identifyingProgress, 0f, 1f);
				if (this.identifyingProgress == 1f)
				{
					this.Identify();
				}
			}
		}

		// Token: 0x0600A1E9 RID: 41449 RVA: 0x0038450D File Offset: 0x0038270D
		public override void StartSM()
		{
			base.StartSM();
			if (this.DestinationWorldID < 0)
			{
				this.Setup(base.def.destinationWorldID, base.def.arrivalTime);
			}
			this.RefreshVisuals(false);
		}

		// Token: 0x0600A1EA RID: 41450 RVA: 0x00384544 File Offset: 0x00382744
		public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
		{
			if (this.HasBeenIdentified)
			{
				this.selectable.SetName(base.def.name);
				this.descriptor.description = base.def.description;
				this.visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
			}
			else
			{
				this.selectable.SetName(base.def.name_Hidden);
				this.descriptor.description = base.def.description_Hidden;
				this.visualizer.PlayHideAnimation();
			}
			base.Trigger(1980521255, null);
		}

		// Token: 0x0600A1EB RID: 41451 RVA: 0x003845D8 File Offset: 0x003827D8
		public void Setup(int destinationWorldID, float arrivalTime)
		{
			this.DestinationWorldID = destinationWorldID;
			this.ArrivalTime = arrivalTime;
			AxialI location = this.World_Destination.GetComponent<ClusterGridEntity>().Location;
			this.destinationSelector.SetDestination(location);
			this.traveler.RevalidatePath(false);
			int count = this.traveler.CurrentPath.Count;
			float num = arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
			this.Speed = (float)count / num * 600f;
		}

		// Token: 0x0600A1EC RID: 41452 RVA: 0x0038464B File Offset: 0x0038284B
		public float GetSpeed()
		{
			return this.Speed;
		}

		// Token: 0x0600A1ED RID: 41453 RVA: 0x00384653 File Offset: 0x00382853
		public void DestinationReached()
		{
			System.Action onDestinationReached = this.OnDestinationReached;
			if (onDestinationReached == null)
			{
				return;
			}
			onDestinationReached();
		}

		// Token: 0x0600A1EE RID: 41454 RVA: 0x00384665 File Offset: 0x00382865
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A1EF RID: 41455 RVA: 0x0038466C File Offset: 0x0038286C
		public bool SidescreenEnabled()
		{
			return false;
		}

		// Token: 0x0600A1F0 RID: 41456 RVA: 0x0038466F File Offset: 0x0038286F
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x0600A1F1 RID: 41457 RVA: 0x00384672 File Offset: 0x00382872
		public void OnSidescreenButtonPressed()
		{
			this.Identify();
		}

		// Token: 0x0600A1F2 RID: 41458 RVA: 0x0038467A File Offset: 0x0038287A
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x0600A1F3 RID: 41459 RVA: 0x0038467D File Offset: 0x0038287D
		public int ButtonSideScreenSortOrder()
		{
			return SORTORDER.KEEPSAKES;
		}

		// Token: 0x04007E68 RID: 32360
		[Serialize]
		public int DestinationWorldID = -1;

		// Token: 0x04007E69 RID: 32361
		[Serialize]
		public float ArrivalTime;

		// Token: 0x04007E6A RID: 32362
		[Serialize]
		private float Speed;

		// Token: 0x04007E6B RID: 32363
		[Serialize]
		private float identifyingProgress;

		// Token: 0x04007E6C RID: 32364
		public System.Action OnDestinationReached;

		// Token: 0x04007E6D RID: 32365
		[MyCmpGet]
		private InfoDescription descriptor;

		// Token: 0x04007E6E RID: 32366
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x04007E6F RID: 32367
		[MyCmpGet]
		private ClusterMapMeteorShowerVisualizer visualizer;

		// Token: 0x04007E70 RID: 32368
		[MyCmpGet]
		private ClusterTraveler traveler;

		// Token: 0x04007E71 RID: 32369
		[MyCmpGet]
		private ClusterDestinationSelector destinationSelector;
	}
}
