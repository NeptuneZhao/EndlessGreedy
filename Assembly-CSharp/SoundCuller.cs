using System;
using UnityEngine;

// Token: 0x02000511 RID: 1297
public struct SoundCuller
{
	// Token: 0x06001CD6 RID: 7382 RVA: 0x00098654 File Offset: 0x00096854
	public static bool IsAudibleWorld(Vector2 pos)
	{
		bool result = false;
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x00098688 File Offset: 0x00096888
	public bool IsAudible(Vector2 pos)
	{
		return SoundCuller.IsAudibleWorld(pos) && this.min.LessEqual(pos) && pos.LessEqual(this.max);
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x000986B0 File Offset: 0x000968B0
	public bool IsAudibleNoCameraScaling(Vector2 pos, float falloff_distance_sq)
	{
		return (pos.x - this.cameraPos.x) * (pos.x - this.cameraPos.x) + (pos.y - this.cameraPos.y) * (pos.y - this.cameraPos.y) < falloff_distance_sq;
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x0009870B File Offset: 0x0009690B
	public bool IsAudible(Vector2 pos, float falloff_distance_sq)
	{
		if (!SoundCuller.IsAudibleWorld(pos))
		{
			return false;
		}
		pos = this.GetVerticallyScaledPosition(pos, false);
		return this.IsAudibleNoCameraScaling(pos, falloff_distance_sq);
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x00098733 File Offset: 0x00096933
	public bool IsAudible(Vector2 pos, HashedString sound_path)
	{
		return sound_path.IsValid && this.IsAudible(pos, KFMOD.GetSoundEventDescription(sound_path).falloffDistanceSq);
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x00098754 File Offset: 0x00096954
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		float num = 1f;
		float num2;
		if (pos.y > this.max.y)
		{
			num2 = Mathf.Abs(pos.y - this.max.y);
		}
		else if (pos.y < this.min.y)
		{
			num2 = Mathf.Abs(pos.y - this.min.y);
			num = -1f;
		}
		else
		{
			num2 = 0f;
		}
		float extraYRange = TuningData<SoundCuller.Tuning>.Get().extraYRange;
		num2 = ((num2 < extraYRange) ? num2 : extraYRange);
		float num3 = num2 * num2 / (4f * this.zoomScaler);
		num3 *= num;
		Vector3 result = new Vector3(pos.x, pos.y + num3, 0f);
		if (objectIsSelectedAndVisible)
		{
			result.z = pos.z;
		}
		return result;
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x00098828 File Offset: 0x00096A28
	public static SoundCuller CreateCuller()
	{
		SoundCuller result = default(SoundCuller);
		Camera main = Camera.main;
		Vector3 vector = main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		result.min = new Vector3(vector2.x, vector2.y, 0f);
		result.max = new Vector3(vector.x, vector.y, 0f);
		result.cameraPos = main.transform.GetPosition();
		Audio audio = Audio.Get();
		float num = CameraController.Instance.OrthographicSize / (audio.listenerReferenceZ - audio.listenerMinZ);
		if (num <= 0f)
		{
			num = 2f;
		}
		else
		{
			num = 1f;
		}
		result.zoomScaler = num;
		return result;
	}

	// Token: 0x04001044 RID: 4164
	private Vector2 min;

	// Token: 0x04001045 RID: 4165
	private Vector2 max;

	// Token: 0x04001046 RID: 4166
	private Vector2 cameraPos;

	// Token: 0x04001047 RID: 4167
	private float zoomScaler;

	// Token: 0x020012D4 RID: 4820
	public class Tuning : TuningData<SoundCuller.Tuning>
	{
		// Token: 0x040064B2 RID: 25778
		public float extraYRange;
	}
}
