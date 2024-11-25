using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A72 RID: 2674
public class RotPile : StateMachineComponent<RotPile.StatesInstance>
{
	// Token: 0x06004DD8 RID: 19928 RVA: 0x001BF389 File Offset: 0x001BD589
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004DD9 RID: 19929 RVA: 0x001BF391 File Offset: 0x001BD591
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004DDA RID: 19930 RVA: 0x001BF3A4 File Offset: 0x001BD5A4
	protected void ConvertToElement()
	{
		PrimaryElement component = base.smi.master.GetComponent<PrimaryElement>();
		float mass = component.Mass;
		float temperature = component.Temperature;
		if (mass <= 0f)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		SimHashes hash = SimHashes.ToxicSand;
		GameObject gameObject = ElementLoader.FindElementByHash(hash).substance.SpawnResource(base.smi.master.transform.GetPosition(), mass, temperature, byte.MaxValue, 0, false, false, false);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(hash).name, gameObject.transform, 1.5f, false);
		Util.KDestroyGameObject(base.smi.gameObject);
	}

	// Token: 0x06004DDB RID: 19931 RVA: 0x001BF458 File Offset: 0x001BD658
	private static string OnRottenTooltip(List<Notification> notifications, object data)
	{
		string text = "";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				text = text + "\n• " + (string)notification.tooltipData + " ";
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODROT.TOOLTIP, text);
	}

	// Token: 0x06004DDC RID: 19932 RVA: 0x001BF4DC File Offset: 0x001BD6DC
	public void TryClearNotification()
	{
		if (this.notification != null)
		{
			base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		}
	}

	// Token: 0x06004DDD RID: 19933 RVA: 0x001BF4FC File Offset: 0x001BD6FC
	public void TryCreateNotification()
	{
		WorldContainer myWorld = base.smi.master.GetMyWorld();
		if (myWorld != null && myWorld.worldInventory.IsReachable(base.smi.master.gameObject.GetComponent<Pickupable>()))
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.FOODROT.NAME, NotificationType.BadMinor, new Func<List<Notification>, object, string>(RotPile.OnRottenTooltip), null, true, 0f, null, null, null, true, false, false);
			this.notification.tooltipData = base.smi.master.gameObject.GetProperName();
			base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		}
	}

	// Token: 0x040033D4 RID: 13268
	private Notification notification;

	// Token: 0x02001A8B RID: 6795
	public class StatesInstance : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.GameInstance
	{
		// Token: 0x0600A07C RID: 41084 RVA: 0x0037FCCB File Offset: 0x0037DECB
		public StatesInstance(RotPile master) : base(master)
		{
		}

		// Token: 0x04007CE6 RID: 31974
		public AttributeModifier baseDecomposeRate;
	}

	// Token: 0x02001A8C RID: 6796
	public class States : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile>
	{
		// Token: 0x0600A07D RID: 41085 RVA: 0x0037FCD4 File Offset: 0x0037DED4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.decomposing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.decomposing.Enter(delegate(RotPile.StatesInstance smi)
			{
				smi.master.TryCreateNotification();
			}).Exit(delegate(RotPile.StatesInstance smi)
			{
				smi.master.TryClearNotification();
			}).ParamTransition<float>(this.decompositionAmount, this.convertDestroy, (RotPile.StatesInstance smi, float p) => p >= 600f).Update("Decomposing", delegate(RotPile.StatesInstance smi, float dt)
			{
				this.decompositionAmount.Delta(dt, smi);
			}, UpdateRate.SIM_200ms, false);
			this.convertDestroy.Enter(delegate(RotPile.StatesInstance smi)
			{
				smi.master.ConvertToElement();
			});
		}

		// Token: 0x04007CE7 RID: 31975
		public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State decomposing;

		// Token: 0x04007CE8 RID: 31976
		public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State convertDestroy;

		// Token: 0x04007CE9 RID: 31977
		public StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.FloatParameter decompositionAmount;
	}
}
