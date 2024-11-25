using System;
using System.IO;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000964 RID: 2404
[SerializationConfig(MemberSerialization.OptIn)]
public class MinionModifiers : Modifiers, ISaveLoadable
{
	// Token: 0x06004648 RID: 17992 RVA: 0x00190B58 File Offset: 0x0018ED58
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.addBaseTraits)
		{
			foreach (Klei.AI.Attribute attribute in Db.Get().Attributes.resources)
			{
				if (this.attributes.Get(attribute) == null)
				{
					this.attributes.Add(attribute);
				}
			}
			foreach (Disease disease in Db.Get().Diseases.resources)
			{
				AmountInstance amountInstance = this.AddAmount(disease.amount);
				this.attributes.Add(disease.cureSpeedBase);
				amountInstance.SetValue(0f);
			}
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			if (component != null)
			{
				component.AddProvider(GlobalChoreProvider.Instance);
				base.gameObject.AddComponent<QualityOfLifeNeed>();
			}
		}
	}

	// Token: 0x06004649 RID: 17993 RVA: 0x00190C70 File Offset: 0x0018EE70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<ChoreConsumer>() != null)
		{
			base.Subscribe<MinionModifiers>(1623392196, MinionModifiers.OnDeathDelegate);
			base.Subscribe<MinionModifiers>(-1506069671, MinionModifiers.OnAttachFollowCamDelegate);
			base.Subscribe<MinionModifiers>(-485480405, MinionModifiers.OnDetachFollowCamDelegate);
			base.Subscribe<MinionModifiers>(-1988963660, MinionModifiers.OnBeginChoreDelegate);
			AmountInstance amountInstance = this.GetAmounts().Get("Calories");
			if (amountInstance != null)
			{
				AmountInstance amountInstance2 = amountInstance;
				amountInstance2.OnMaxValueReached = (System.Action)Delegate.Combine(amountInstance2.OnMaxValueReached, new System.Action(this.OnMaxCaloriesReached));
			}
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			base.transform.SetPosition(position);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			this.SetupDependentAttribute(Db.Get().Attributes.CarryAmount, Db.Get().AttributeConverters.CarryAmountFromStrength);
		}
	}

	// Token: 0x0600464A RID: 17994 RVA: 0x00190D70 File Offset: 0x0018EF70
	private AmountInstance AddAmount(Amount amount)
	{
		AmountInstance instance = new AmountInstance(amount, base.gameObject);
		return this.amounts.Add(instance);
	}

	// Token: 0x0600464B RID: 17995 RVA: 0x00190D98 File Offset: 0x0018EF98
	private void SetupDependentAttribute(Klei.AI.Attribute targetAttribute, AttributeConverter attributeConverter)
	{
		Klei.AI.Attribute attribute = attributeConverter.attribute;
		AttributeInstance attributeInstance = attribute.Lookup(this);
		AttributeModifier target_modifier = new AttributeModifier(targetAttribute.Id, attributeConverter.Lookup(this).Evaluate(), attribute.Name, false, false, false);
		this.GetAttributes().Add(target_modifier);
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(delegate()
		{
			target_modifier.SetValue(attributeConverter.Lookup(this).Evaluate());
		}));
	}

	// Token: 0x0600464C RID: 17996 RVA: 0x00190E2C File Offset: 0x0018F02C
	private void OnDeath(object data)
	{
		global::Debug.LogFormat("OnDeath {0} -- {1} has died!", new object[]
		{
			data,
			base.name
		});
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			minionIdentity.GetComponent<Effects>().Add("Mourning", true);
		}
	}

	// Token: 0x0600464D RID: 17997 RVA: 0x00190EAC File Offset: 0x0018F0AC
	private void OnMaxCaloriesReached()
	{
		base.GetComponent<Effects>().Add("WellFed", true);
	}

	// Token: 0x0600464E RID: 17998 RVA: 0x00190EC0 File Offset: 0x0018F0C0
	private void OnBeginChore(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x0600464F RID: 17999 RVA: 0x00190EF0 File Offset: 0x0018F0F0
	public override void OnSerialize(BinaryWriter writer)
	{
		base.OnSerialize(writer);
	}

	// Token: 0x06004650 RID: 18000 RVA: 0x00190EF9 File Offset: 0x0018F0F9
	public override void OnDeserialize(IReader reader)
	{
		base.OnDeserialize(reader);
	}

	// Token: 0x06004651 RID: 18001 RVA: 0x00190F02 File Offset: 0x0018F102
	private void OnAttachFollowCam(object data)
	{
		base.GetComponent<Effects>().Add("CenterOfAttention", false);
	}

	// Token: 0x06004652 RID: 18002 RVA: 0x00190F16 File Offset: 0x0018F116
	private void OnDetachFollowCam(object data)
	{
		base.GetComponent<Effects>().Remove("CenterOfAttention");
	}

	// Token: 0x04002DBE RID: 11710
	public bool addBaseTraits = true;

	// Token: 0x04002DBF RID: 11711
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDeathDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04002DC0 RID: 11712
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnAttachFollowCamDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnAttachFollowCam(data);
	});

	// Token: 0x04002DC1 RID: 11713
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDetachFollowCamDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnDetachFollowCam(data);
	});

	// Token: 0x04002DC2 RID: 11714
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnBeginChoreDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnBeginChore(data);
	});
}
