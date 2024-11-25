using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000887 RID: 2183
public class EntityLuminescence : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>
{
	// Token: 0x06003D40 RID: 15680 RVA: 0x00152C9C File Offset: 0x00150E9C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x0200178C RID: 6028
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400730E RID: 29454
		public Color lightColor;

		// Token: 0x0400730F RID: 29455
		public float lightRange;

		// Token: 0x04007310 RID: 29456
		public float lightAngle;

		// Token: 0x04007311 RID: 29457
		public Vector2 lightOffset;

		// Token: 0x04007312 RID: 29458
		public Vector2 lightDirection;

		// Token: 0x04007313 RID: 29459
		public global::LightShape lightShape;
	}

	// Token: 0x0200178D RID: 6029
	public new class Instance : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>.GameInstance
	{
		// Token: 0x06009614 RID: 38420 RVA: 0x00360E00 File Offset: 0x0035F000
		public Instance(IStateMachineTarget master, EntityLuminescence.Def def) : base(master, def)
		{
			this.light.Color = def.lightColor;
			this.light.Range = def.lightRange;
			this.light.Angle = def.lightAngle;
			this.light.Direction = def.lightDirection;
			this.light.Offset = def.lightOffset;
			this.light.shape = def.lightShape;
		}

		// Token: 0x06009615 RID: 38421 RVA: 0x00360E7C File Offset: 0x0035F07C
		public override void StartSM()
		{
			base.StartSM();
			this.luminescence = Db.Get().Attributes.Luminescence.Lookup(base.gameObject);
			AttributeInstance attributeInstance = this.luminescence;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(this.OnLuminescenceChanged));
			this.RefreshLight();
		}

		// Token: 0x06009616 RID: 38422 RVA: 0x00360EDC File Offset: 0x0035F0DC
		private void OnLuminescenceChanged()
		{
			this.RefreshLight();
		}

		// Token: 0x06009617 RID: 38423 RVA: 0x00360EE4 File Offset: 0x0035F0E4
		public void RefreshLight()
		{
			if (this.luminescence != null)
			{
				int num = (int)this.luminescence.GetTotalValue();
				this.light.Lux = num;
				bool flag = num > 0;
				if (this.light.enabled != flag)
				{
					this.light.enabled = flag;
				}
			}
		}

		// Token: 0x06009618 RID: 38424 RVA: 0x00360F31 File Offset: 0x0035F131
		protected override void OnCleanUp()
		{
			if (this.luminescence != null)
			{
				AttributeInstance attributeInstance = this.luminescence;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(this.OnLuminescenceChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x04007314 RID: 29460
		[MyCmpAdd]
		private Light2D light;

		// Token: 0x04007315 RID: 29461
		private AttributeInstance luminescence;
	}
}
