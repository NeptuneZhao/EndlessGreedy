using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x02000B42 RID: 2882
[AddComponentMenu("KMonoBehaviour/scripts/UserNavigation")]
public class UserNavigation : KMonoBehaviour
{
	// Token: 0x06005616 RID: 22038 RVA: 0x001ECA1C File Offset: 0x001EAC1C
	public UserNavigation()
	{
		for (global::Action action = global::Action.SetUserNav1; action <= global::Action.SetUserNav10; action++)
		{
			this.hotkeyNavPoints.Add(UserNavigation.NavPoint.Invalid);
		}
	}

	// Token: 0x06005617 RID: 22039 RVA: 0x001ECA63 File Offset: 0x001EAC63
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(1983128072, delegate(object worlds)
		{
			global::Tuple<int, int> tuple = (global::Tuple<int, int>)worlds;
			int first = tuple.first;
			int second = tuple.second;
			int num = Grid.PosToCell(CameraController.Instance.transform.position);
			if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != second)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(second);
				float x = Mathf.Clamp(CameraController.Instance.transform.position.x, world.minimumBounds.x, world.maximumBounds.x);
				float y = Mathf.Clamp(CameraController.Instance.transform.position.y, world.minimumBounds.y, world.maximumBounds.y);
				Vector3 position = new Vector3(x, y, CameraController.Instance.transform.position.z);
				CameraController.Instance.SetPosition(position);
			}
			this.worldCameraPositions[second] = new UserNavigation.NavPoint
			{
				pos = CameraController.Instance.transform.position,
				orthoSize = CameraController.Instance.targetOrthographicSize
			};
			if (!this.worldCameraPositions.ContainsKey(first))
			{
				WorldContainer world2 = ClusterManager.Instance.GetWorld(first);
				Vector2I vector2I = world2.WorldOffset + new Vector2I(world2.Width / 2, world2.Height / 2);
				this.worldCameraPositions.Add(first, new UserNavigation.NavPoint
				{
					pos = new Vector3((float)vector2I.x, (float)vector2I.y),
					orthoSize = CameraController.Instance.targetOrthographicSize
				});
			}
			CameraController.Instance.SetTargetPosForWorldChange(this.worldCameraPositions[first].pos, this.worldCameraPositions[first].orthoSize, false);
		});
	}

	// Token: 0x06005618 RID: 22040 RVA: 0x001ECA88 File Offset: 0x001EAC88
	public void SetWorldCameraStartPosition(int world_id, Vector3 start_pos)
	{
		if (!this.worldCameraPositions.ContainsKey(world_id))
		{
			this.worldCameraPositions.Add(world_id, new UserNavigation.NavPoint
			{
				pos = new Vector3(start_pos.x, start_pos.y),
				orthoSize = CameraController.Instance.targetOrthographicSize
			});
			return;
		}
		this.worldCameraPositions[world_id] = new UserNavigation.NavPoint
		{
			pos = new Vector3(start_pos.x, start_pos.y),
			orthoSize = CameraController.Instance.targetOrthographicSize
		};
	}

	// Token: 0x06005619 RID: 22041 RVA: 0x001ECB20 File Offset: 0x001EAD20
	private static int GetIndex(global::Action action)
	{
		int result = -1;
		if (global::Action.SetUserNav1 <= action && action <= global::Action.SetUserNav10)
		{
			result = action - global::Action.SetUserNav1;
		}
		else if (global::Action.GotoUserNav1 <= action && action <= global::Action.GotoUserNav10)
		{
			result = action - global::Action.GotoUserNav1;
		}
		return result;
	}

	// Token: 0x0600561A RID: 22042 RVA: 0x001ECB50 File Offset: 0x001EAD50
	private void SetHotkeyNavPoint(global::Action action, Vector3 pos, float ortho_size)
	{
		int index = UserNavigation.GetIndex(action);
		if (index < 0)
		{
			return;
		}
		this.hotkeyNavPoints[index] = new UserNavigation.NavPoint
		{
			pos = pos,
			orthoSize = ortho_size
		};
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("UserNavPoint_set", false), Vector3.zero, 1f);
		instance.setParameterByName("userNavPoint_ID", (float)index, false);
		KFMOD.EndOneShot(instance);
	}

	// Token: 0x0600561B RID: 22043 RVA: 0x001ECBC0 File Offset: 0x001EADC0
	private void GoToHotkeyNavPoint(global::Action action)
	{
		int index = UserNavigation.GetIndex(action);
		if (index < 0)
		{
			return;
		}
		UserNavigation.NavPoint navPoint = this.hotkeyNavPoints[index];
		if (navPoint.IsValid())
		{
			CameraController.Instance.SetTargetPos(navPoint.pos, navPoint.orthoSize, true);
			EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("UserNavPoint_recall", false), Vector3.zero, 1f);
			instance.setParameterByName("userNavPoint_ID", (float)index, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x0600561C RID: 22044 RVA: 0x001ECC38 File Offset: 0x001EAE38
	public bool Handle(KButtonEvent e)
	{
		bool flag = false;
		for (global::Action action = global::Action.GotoUserNav1; action <= global::Action.GotoUserNav10; action++)
		{
			if (e.TryConsume(action))
			{
				this.GoToHotkeyNavPoint(action);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (global::Action action2 = global::Action.SetUserNav1; action2 <= global::Action.SetUserNav10; action2++)
			{
				if (e.TryConsume(action2))
				{
					Camera baseCamera = CameraController.Instance.baseCamera;
					Vector3 position = baseCamera.transform.GetPosition();
					this.SetHotkeyNavPoint(action2, position, baseCamera.orthographicSize);
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x04003863 RID: 14435
	[Serialize]
	private List<UserNavigation.NavPoint> hotkeyNavPoints = new List<UserNavigation.NavPoint>();

	// Token: 0x04003864 RID: 14436
	[Serialize]
	private Dictionary<int, UserNavigation.NavPoint> worldCameraPositions = new Dictionary<int, UserNavigation.NavPoint>();

	// Token: 0x02001B9C RID: 7068
	[Serializable]
	private struct NavPoint
	{
		// Token: 0x0600A3DE RID: 41950 RVA: 0x0038ADB7 File Offset: 0x00388FB7
		public bool IsValid()
		{
			return this.orthoSize != 0f;
		}

		// Token: 0x04008032 RID: 32818
		public Vector3 pos;

		// Token: 0x04008033 RID: 32819
		public float orthoSize;

		// Token: 0x04008034 RID: 32820
		public static readonly UserNavigation.NavPoint Invalid = new UserNavigation.NavPoint
		{
			pos = Vector3.zero,
			orthoSize = 0f
		};
	}
}
