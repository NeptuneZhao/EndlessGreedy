using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x0200095F RID: 2399
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MiniComet")]
public class MiniComet : KMonoBehaviour, ISim33ms
{
	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06004609 RID: 17929 RVA: 0x0018EE84 File Offset: 0x0018D084
	public Vector3 TargetPosition
	{
		get
		{
			return this.anim.PositionIncludingOffset;
		}
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x0600460A RID: 17930 RVA: 0x0018EE91 File Offset: 0x0018D091
	// (set) Token: 0x0600460B RID: 17931 RVA: 0x0018EE99 File Offset: 0x0018D099
	public Vector2 Velocity
	{
		get
		{
			return this.velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

	// Token: 0x0600460C RID: 17932 RVA: 0x0018EEA4 File Offset: 0x0018D0A4
	private float GetVolume(GameObject gameObject)
	{
		float result = 1f;
		if (gameObject != null && this.selectable != null && this.selectable.IsSelected)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x0600460D RID: 17933 RVA: 0x0018EEE2 File Offset: 0x0018D0E2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Meteor_LP", false);
		this.RandomizeVelocity();
	}

	// Token: 0x0600460E RID: 17934 RVA: 0x0018EF14 File Offset: 0x0018D114
	protected override void OnSpawn()
	{
		this.anim.Offset = this.offsetPosition;
		if (this.spawnWithOffset)
		{
			this.SetupOffset();
		}
		base.OnSpawn();
		this.StartLoopingSound();
		bool flag = this.offsetPosition.x != 0f || this.offsetPosition.y != 0f;
		this.selectable.enabled = !flag;
		this.typeID = base.GetComponent<KPrefabID>().PrefabTag;
	}

	// Token: 0x0600460F RID: 17935 RVA: 0x0018EF97 File Offset: 0x0018D197
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004610 RID: 17936 RVA: 0x0018EFA0 File Offset: 0x0018D1A0
	protected void SetupOffset()
	{
		Vector3 position = base.transform.GetPosition();
		Vector3 position2 = base.transform.GetPosition();
		position2.z = 0f;
		Vector3 vector = new Vector3(this.velocity.x, this.velocity.y, 0f);
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		float num = (float)(myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y;
		float f = Vector3.Angle(Vector3.up, -vector) * 0.017453292f;
		float d = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 vector2 = position2 - vector.normalized * d;
		float num2 = (float)(myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
		if (vector2.x < (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters || vector2.x > num2)
		{
			float num3 = (vector.x < 0f) ? (num2 - position2.x) : (position2.x - (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters);
			f = Vector3.Angle((vector.x < 0f) ? Vector3.right : Vector3.left, -vector) * 0.017453292f;
			d = Mathf.Abs(num3 / Mathf.Cos(f));
		}
		Vector3 b = -vector.normalized * d;
		(position2 + b).z = position.z;
		this.offsetPosition = b;
		this.anim.Offset = this.offsetPosition;
	}

	// Token: 0x06004611 RID: 17937 RVA: 0x0018F164 File Offset: 0x0018D364
	public virtual void RandomizeVelocity()
	{
		float num = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
		float f = num * 3.1415927f / 180f;
		float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
		this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
		base.GetComponent<KBatchedAnimController>().Rotation = -num - 90f;
	}

	// Token: 0x06004612 RID: 17938 RVA: 0x0018F1E6 File Offset: 0x0018D3E6
	public int GetRandomNumOres()
	{
		return UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
	}

	// Token: 0x06004613 RID: 17939 RVA: 0x0018F208 File Offset: 0x0018D408
	[ContextMenu("Explode")]
	private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
	{
		byte b = Grid.WorldIdx[cell];
		this.PlayImpactSound(pos);
		Vector3 vector = pos;
		vector.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		if (this.explosionEffectHash != SpawnFXHashes.None)
		{
			Game.Instance.SpawnFX(this.explosionEffectHash, vector, 0f);
		}
		if (element != null)
		{
			Substance substance = element.substance;
			int randomNumOres = this.GetRandomNumOres();
			Vector2 vector2 = -this.velocity.normalized;
			Vector2 a = new Vector2(vector2.y, -vector2.x);
			float mass = (randomNumOres > 0) ? (this.pe.Mass / (float)randomNumOres) : 1f;
			for (int i = 0; i < randomNumOres; i++)
			{
				Vector2 normalized = (vector2 + a * UnityEngine.Random.Range(-1f, 1f)).normalized;
				Vector3 v = normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
				Vector3 position = vector + normalized.normalized * 1.25f;
				GameObject go = substance.SpawnResource(position, mass, this.pe.Temperature, this.pe.DiseaseIdx, this.pe.DiseaseCount / randomNumOres, false, false, false);
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, v);
			}
		}
		if (this.OnImpact != null)
		{
			this.OnImpact();
		}
	}

	// Token: 0x06004614 RID: 17940 RVA: 0x0018F3A0 File Offset: 0x0018D5A0
	public float GetDistanceFromImpact()
	{
		float num = this.velocity.x / this.velocity.y;
		Vector3 position = base.transform.GetPosition();
		float num2 = 0f;
		while (num2 > -6f)
		{
			num2 -= 1f;
			num2 = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
			float x = num2 * num;
			Vector3 b = new Vector3(x, num2, 0f);
			int num3 = Grid.PosToCell(position + b);
			if (Grid.IsValidCell(num3) && Grid.Solid[num3])
			{
				return b.magnitude;
			}
		}
		return 6f;
	}

	// Token: 0x06004615 RID: 17941 RVA: 0x0018F449 File Offset: 0x0018D649
	public float GetSoundDistance()
	{
		return this.GetDistanceFromImpact();
	}

	// Token: 0x06004616 RID: 17942 RVA: 0x0018F454 File Offset: 0x0018D654
	public void Sim33ms(float dt)
	{
		if (this.hasExploded)
		{
			return;
		}
		if (this.offsetPosition.y > 0f)
		{
			Vector3 b = new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Vector3 vector = this.offsetPosition + b;
			this.offsetPosition = vector;
			this.anim.Offset = this.offsetPosition;
		}
		else
		{
			if (this.anim.Offset != Vector3.zero)
			{
				this.anim.Offset = Vector3.zero;
			}
			if (!this.selectable.enabled)
			{
				this.selectable.enabled = true;
			}
			Vector2 vector2 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * -0.1f;
			Vector2 vector3 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * 1.1f;
			Vector3 position = base.transform.GetPosition();
			Vector3 vector4 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Grid.PosToCell(vector4);
			this.loopingSounds.UpdateVelocity(this.flyingSound, vector4 - position);
			if (vector4.x < vector2.x || vector3.x < vector4.x || vector4.y < vector2.y)
			{
				global::Util.KDestroyGameObject(base.gameObject);
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.PosToCell(this.previousPosition);
			if (num != num2 && Grid.IsValidCell(num) && Grid.Solid[num])
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				this.Explode(position, num, num2, component.Element);
				this.hasExploded = true;
				global::Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.previousPosition = position;
			base.transform.SetPosition(vector4);
		}
		this.age += dt;
	}

	// Token: 0x06004617 RID: 17943 RVA: 0x0018F664 File Offset: 0x0018D864
	private void PlayImpactSound(Vector3 pos)
	{
		if (this.impactSound == null)
		{
			this.impactSound = "Meteor_Large_Impact";
		}
		this.loopingSounds.StopSound(this.flyingSound);
		string sound = GlobalAssets.GetSound(this.impactSound, false);
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			float volume = this.GetVolume(base.gameObject);
			pos.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06004618 RID: 17944 RVA: 0x0018F705 File Offset: 0x0018D905
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
		this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float)this.flyingSoundID);
	}

	// Token: 0x06004619 RID: 17945 RVA: 0x0018F738 File Offset: 0x0018D938
	public void Explode()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		this.Explode(position, num, num, component.Element);
		this.hasExploded = true;
		global::Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04002D8C RID: 11660
	[MyCmpGet]
	private PrimaryElement pe;

	// Token: 0x04002D8D RID: 11661
	public Vector2 spawnVelocity = new Vector2(7f, 9f);

	// Token: 0x04002D8E RID: 11662
	public Vector2 spawnAngle = new Vector2(30f, 150f);

	// Token: 0x04002D8F RID: 11663
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04002D90 RID: 11664
	public int addDiseaseCount;

	// Token: 0x04002D91 RID: 11665
	public byte diseaseIdx = byte.MaxValue;

	// Token: 0x04002D92 RID: 11666
	public Vector2I explosionOreCount = new Vector2I(1, 1);

	// Token: 0x04002D93 RID: 11667
	public Vector2 explosionSpeedRange = new Vector2(0f, 0f);

	// Token: 0x04002D94 RID: 11668
	public string impactSound;

	// Token: 0x04002D95 RID: 11669
	public string flyingSound;

	// Token: 0x04002D96 RID: 11670
	public int flyingSoundID;

	// Token: 0x04002D97 RID: 11671
	private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	// Token: 0x04002D98 RID: 11672
	public bool Targeted;

	// Token: 0x04002D99 RID: 11673
	[Serialize]
	protected Vector3 offsetPosition;

	// Token: 0x04002D9A RID: 11674
	[Serialize]
	protected Vector2 velocity;

	// Token: 0x04002D9B RID: 11675
	private Vector3 previousPosition;

	// Token: 0x04002D9C RID: 11676
	private bool hasExploded;

	// Token: 0x04002D9D RID: 11677
	public string[] craterPrefabs;

	// Token: 0x04002D9E RID: 11678
	public bool spawnWithOffset;

	// Token: 0x04002D9F RID: 11679
	private float age;

	// Token: 0x04002DA0 RID: 11680
	public System.Action OnImpact;

	// Token: 0x04002DA1 RID: 11681
	public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();

	// Token: 0x04002DA2 RID: 11682
	[MyCmpGet]
	private KBatchedAnimController anim;

	// Token: 0x04002DA3 RID: 11683
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04002DA4 RID: 11684
	public Tag typeID;

	// Token: 0x04002DA5 RID: 11685
	private LoopingSounds loopingSounds;

	// Token: 0x04002DA6 RID: 11686
	private List<GameObject> damagedEntities = new List<GameObject>();

	// Token: 0x04002DA7 RID: 11687
	private List<int> destroyedCells = new List<int>();

	// Token: 0x04002DA8 RID: 11688
	private const float MAX_DISTANCE_TEST = 6f;
}
