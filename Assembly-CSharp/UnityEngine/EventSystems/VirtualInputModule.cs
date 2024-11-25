using System;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000E20 RID: 3616
	[AddComponentMenu("Event/Virtual Input Module")]
	public class VirtualInputModule : PointerInputModule, IInputHandler
	{
		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06007335 RID: 29493 RVA: 0x002C2BC7 File Offset: 0x002C0DC7
		public string handlerName
		{
			get
			{
				return "VirtualCursorInput";
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06007336 RID: 29494 RVA: 0x002C2BCE File Offset: 0x002C0DCE
		// (set) Token: 0x06007337 RID: 29495 RVA: 0x002C2BD6 File Offset: 0x002C0DD6
		public KInputHandler inputHandler { get; set; }

		// Token: 0x06007338 RID: 29496 RVA: 0x002C2BE0 File Offset: 0x002C0DE0
		protected VirtualInputModule()
		{
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06007339 RID: 29497 RVA: 0x002C2C56 File Offset: 0x002C0E56
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public VirtualInputModule.InputMode inputMode
		{
			get
			{
				return VirtualInputModule.InputMode.Mouse;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x0600733A RID: 29498 RVA: 0x002C2C59 File Offset: 0x002C0E59
		// (set) Token: 0x0600733B RID: 29499 RVA: 0x002C2C61 File Offset: 0x002C0E61
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x0600733C RID: 29500 RVA: 0x002C2C6A File Offset: 0x002C0E6A
		// (set) Token: 0x0600733D RID: 29501 RVA: 0x002C2C72 File Offset: 0x002C0E72
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x0600733E RID: 29502 RVA: 0x002C2C7B File Offset: 0x002C0E7B
		// (set) Token: 0x0600733F RID: 29503 RVA: 0x002C2C83 File Offset: 0x002C0E83
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06007340 RID: 29504 RVA: 0x002C2C8C File Offset: 0x002C0E8C
		// (set) Token: 0x06007341 RID: 29505 RVA: 0x002C2C94 File Offset: 0x002C0E94
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06007342 RID: 29506 RVA: 0x002C2C9D File Offset: 0x002C0E9D
		// (set) Token: 0x06007343 RID: 29507 RVA: 0x002C2CA5 File Offset: 0x002C0EA5
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06007344 RID: 29508 RVA: 0x002C2CAE File Offset: 0x002C0EAE
		// (set) Token: 0x06007345 RID: 29509 RVA: 0x002C2CB6 File Offset: 0x002C0EB6
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06007346 RID: 29510 RVA: 0x002C2CBF File Offset: 0x002C0EBF
		// (set) Token: 0x06007347 RID: 29511 RVA: 0x002C2CC7 File Offset: 0x002C0EC7
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06007348 RID: 29512 RVA: 0x002C2CD0 File Offset: 0x002C0ED0
		// (set) Token: 0x06007349 RID: 29513 RVA: 0x002C2CD8 File Offset: 0x002C0ED8
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x0600734A RID: 29514 RVA: 0x002C2CE1 File Offset: 0x002C0EE1
		public void SetCursor(Texture2D tex)
		{
			this.UpdateModule();
			if (this.m_VirtualCursor)
			{
				this.m_VirtualCursor.GetComponent<RawImage>().texture = tex;
			}
		}

		// Token: 0x0600734B RID: 29515 RVA: 0x002C2D08 File Offset: 0x002C0F08
		public override void UpdateModule()
		{
			GameInputManager inputManager = Global.GetInputManager();
			if (inputManager.GetControllerCount() <= 1)
			{
				return;
			}
			if (this.inputHandler == null || !this.inputHandler.UsesController(this, inputManager.GetController(1)))
			{
				KInputHandler.Add(inputManager.GetController(1), this, int.MaxValue);
				if (!inputManager.usedMenus.Contains(this))
				{
					inputManager.usedMenus.Add(this);
				}
				this.debugName = SceneManager.GetActiveScene().name + "-VirtualInputModule";
			}
			if (this.m_VirtualCursor == null)
			{
				this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
			}
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = base.gameObject.AddComponent<Camera>();
				this.m_canvasCamera.enabled = false;
			}
			if (CameraController.Instance != null)
			{
				this.m_canvasCamera.CopyFrom(CameraController.Instance.overlayCamera);
			}
			else if (this.CursorCanvasShouldBeOverlay)
			{
				this.m_canvasCamera.CopyFrom(GameObject.Find("FrontEndCamera").GetComponent<Camera>());
			}
			if (this.m_canvasCamera != null && this.VCcam == null)
			{
				this.VCcam = GameObject.Find("VirtualCursorCamera").GetComponent<Camera>();
				if (this.VCcam != null)
				{
					if (this.m_virtualCursorCanvas == null)
					{
						this.m_virtualCursorCanvas = GameObject.Find("VirtualCursorCanvas").GetComponent<Canvas>();
						this.m_virtualCursorScaler = this.m_virtualCursorCanvas.GetComponent<CanvasScaler>();
					}
					if (this.CursorCanvasShouldBeOverlay)
					{
						this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
						this.VCcam.orthographic = false;
					}
					else
					{
						this.VCcam.orthographic = this.m_canvasCamera.orthographic;
						this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
						this.VCcam.transform.position = this.m_canvasCamera.transform.position;
						this.VCcam.enabled = true;
						this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceCamera;
						this.m_virtualCursorCanvas.worldCamera = this.VCcam;
					}
				}
			}
			if (this.m_canvasCamera != null && this.VCcam != null)
			{
				this.VCcam.orthographic = this.m_canvasCamera.orthographic;
				this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
				this.VCcam.transform.position = this.m_canvasCamera.transform.position;
				this.VCcam.aspect = this.m_canvasCamera.aspect;
				this.VCcam.enabled = true;
			}
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			if (this.m_virtualCursorScaler != null && this.m_virtualCursorScaler.referenceResolution != vector)
			{
				this.m_virtualCursorScaler.referenceResolution = vector;
			}
			this.m_LastMousePosition = this.m_MousePosition;
			this.m_VirtualCursor.localScale = Vector2.one;
			Vector2 steamCursorMovement = KInputManager.steamInputInterpreter.GetSteamCursorMovement();
			float num = 1f / (4500f / vector.x);
			steamCursorMovement.x *= num;
			steamCursorMovement.y *= num;
			this.m_VirtualCursor.anchoredPosition += steamCursorMovement * this.m_VirtualCursorSpeed;
			this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0f, vector.x), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0f, vector.y));
			KInputManager.virtualCursorPos = new Vector3F(this.m_VirtualCursor.anchoredPosition.x, this.m_VirtualCursor.anchoredPosition.y, 0f);
			this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
		}

		// Token: 0x0600734C RID: 29516 RVA: 0x002C3106 File Offset: 0x002C1306
		public override bool IsModuleSupported()
		{
			return this.m_ForceModuleActive || Input.mousePresent;
		}

		// Token: 0x0600734D RID: 29517 RVA: 0x002C3118 File Offset: 0x002C1318
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				return true;
			}
			bool forceModuleActive = this.m_ForceModuleActive;
			Input.GetButtonDown(this.m_SubmitButton);
			return forceModuleActive | Input.GetButtonDown(this.m_CancelButton) | !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0f) | !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0f) | (this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f | Input.GetMouseButtonDown(0);
		}

		// Token: 0x0600734E RID: 29518 RVA: 0x002C31B0 File Offset: 0x002C13B0
		public override void ActivateModule()
		{
			base.ActivateModule();
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = base.gameObject.AddComponent<Camera>();
				this.m_canvasCamera.enabled = false;
			}
			if (Input.mousePosition.x > 0f && Input.mousePosition.x < (float)Screen.width && Input.mousePosition.y > 0f && Input.mousePosition.y < (float)Screen.height)
			{
				this.m_VirtualCursor.anchoredPosition = Input.mousePosition;
			}
			else
			{
				this.m_VirtualCursor.anchoredPosition = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			}
			this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0f, (float)Screen.width), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0f, (float)Screen.height));
			this.m_VirtualCursor.localScale = Vector2.zero;
			this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
			this.m_LastMousePosition = this.m_VirtualCursor.anchoredPosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			if (this.m_VirtualCursor == null)
			{
				this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
			}
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = GameObject.Find("FrontEndCamera").GetComponent<Camera>();
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x0600734F RID: 29519 RVA: 0x002C336C File Offset: 0x002C156C
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
			this.conButtonStates.affirmativeDown = false;
			this.conButtonStates.affirmativeHoldTime = 0f;
			this.conButtonStates.negativeDown = false;
			this.conButtonStates.negativeHoldTime = 0f;
		}

		// Token: 0x06007350 RID: 29520 RVA: 0x002C33C0 File Offset: 0x002C15C0
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			this.ProcessMouseEvent();
		}

		// Token: 0x06007351 RID: 29521 RVA: 0x002C3400 File Offset: 0x002C1600
		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (Input.GetButtonDown(this.m_SubmitButton))
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (Input.GetButtonDown(this.m_CancelButton))
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x06007352 RID: 29522 RVA: 0x002C3478 File Offset: 0x002C1678
		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = Input.GetAxisRaw(this.m_HorizontalAxis);
			zero.y = Input.GetAxisRaw(this.m_VerticalAxis);
			if (Input.GetButtonDown(this.m_HorizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (Input.GetButtonDown(this.m_VerticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x06007353 RID: 29523 RVA: 0x002C3530 File Offset: 0x002C1730
		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
			bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			if (!flag)
			{
				if (flag2 && this.m_ConsecutiveMoveCount == 1)
				{
					flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag2)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x06007354 RID: 29524 RVA: 0x002C3643 File Offset: 0x002C1843
		protected void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x06007355 RID: 29525 RVA: 0x002C364C File Offset: 0x002C184C
		protected void ProcessMouseEvent(int id)
		{
			if (this.mouseMovementOnly)
			{
				return;
			}
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(id);
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			this.m_CurrentFocusedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
			this.ProcessControllerPress(eventData, true);
			this.ProcessControllerPress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData, false);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06007356 RID: 29526 RVA: 0x002C373C File Offset: 0x002C193C
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06007357 RID: 29527 RVA: 0x002C3784 File Offset: 0x002C1984
		protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				buttonData.position = this.m_VirtualCursor.anchoredPosition;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - buttonData.clickTime < 0.3f)
					{
						PointerEventData pointerEventData = buttonData;
						int clickCount = pointerEventData.clickCount + 1;
						pointerEventData.clickCount = clickCount;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06007358 RID: 29528 RVA: 0x002C3990 File Offset: 0x002C1B90
		public void OnKeyDown(KButtonEvent e)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
				{
					if (this.conButtonStates.affirmativeDown)
					{
						this.conButtonStates.affirmativeHoldTime = this.conButtonStates.affirmativeHoldTime + Time.unscaledDeltaTime;
					}
					if (!this.conButtonStates.affirmativeDown)
					{
						this.leftFirstClick = true;
						this.leftReleased = false;
					}
					this.conButtonStates.affirmativeDown = true;
					return;
				}
				if (e.IsAction(global::Action.MouseRight))
				{
					if (this.conButtonStates.negativeDown)
					{
						this.conButtonStates.negativeHoldTime = this.conButtonStates.negativeHoldTime + Time.unscaledDeltaTime;
					}
					if (!this.conButtonStates.negativeDown)
					{
						this.rightFirstClick = true;
						this.rightReleased = false;
					}
					this.conButtonStates.negativeDown = true;
				}
			}
		}

		// Token: 0x06007359 RID: 29529 RVA: 0x002C3A54 File Offset: 0x002C1C54
		public void OnKeyUp(KButtonEvent e)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
				{
					this.conButtonStates.affirmativeHoldTime = 0f;
					this.leftReleased = true;
					this.leftFirstClick = false;
					this.conButtonStates.affirmativeDown = false;
					return;
				}
				if (e.IsAction(global::Action.MouseRight))
				{
					this.conButtonStates.negativeHoldTime = 0f;
					this.rightReleased = true;
					this.rightFirstClick = false;
					this.conButtonStates.negativeDown = false;
				}
			}
		}

		// Token: 0x0600735A RID: 29530 RVA: 0x002C3AD8 File Offset: 0x002C1CD8
		protected void ProcessControllerPress(PointerInputModule.MouseButtonEventData data, bool leftClick)
		{
			if (this.leftClickData == null)
			{
				this.leftClickData = data.buttonData;
			}
			if (this.rightClickData == null)
			{
				this.rightClickData = data.buttonData;
			}
			if (leftClick)
			{
				PointerEventData buttonData = data.buttonData;
				GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
				buttonData.position = this.m_VirtualCursor.anchoredPosition;
				if (this.leftFirstClick)
				{
					buttonData.button = PointerEventData.InputButton.Left;
					buttonData.eligibleForClick = true;
					buttonData.delta = Vector2.zero;
					buttonData.dragging = false;
					buttonData.useDragThreshold = true;
					buttonData.pressPosition = buttonData.position;
					buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
					buttonData.position = new Vector2(KInputManager.virtualCursorPos.x, KInputManager.virtualCursorPos.y);
					base.DeselectIfSelectionChanged(gameObject, buttonData);
					GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
					if (gameObject2 == null)
					{
						gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					}
					float unscaledTime = Time.unscaledTime;
					if (gameObject2 == buttonData.lastPress)
					{
						if (unscaledTime - buttonData.clickTime < 0.3f)
						{
							PointerEventData pointerEventData = buttonData;
							int clickCount = pointerEventData.clickCount + 1;
							pointerEventData.clickCount = clickCount;
						}
						else
						{
							buttonData.clickCount = 1;
						}
						buttonData.clickTime = unscaledTime;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.pointerPress = gameObject2;
					buttonData.rawPointerPress = gameObject;
					buttonData.clickTime = unscaledTime;
					buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
					if (buttonData.pointerDrag != null)
					{
						ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
					}
					this.leftFirstClick = false;
					return;
				}
				if (this.leftReleased)
				{
					buttonData.button = PointerEventData.InputButton.Left;
					ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
					GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
					{
						ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
					}
					else if (buttonData.pointerDrag != null && buttonData.dragging)
					{
						ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
					}
					buttonData.eligibleForClick = false;
					buttonData.pointerPress = null;
					buttonData.rawPointerPress = null;
					if (buttonData.pointerDrag != null && buttonData.dragging)
					{
						ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
					}
					buttonData.dragging = false;
					buttonData.pointerDrag = null;
					if (gameObject != buttonData.pointerEnter)
					{
						base.HandlePointerExitAndEnter(buttonData, null);
						base.HandlePointerExitAndEnter(buttonData, gameObject);
					}
					this.leftReleased = false;
					return;
				}
			}
			else
			{
				PointerEventData buttonData2 = data.buttonData;
				GameObject gameObject3 = buttonData2.pointerCurrentRaycast.gameObject;
				buttonData2.position = this.m_VirtualCursor.anchoredPosition;
				if (this.rightFirstClick)
				{
					buttonData2.button = PointerEventData.InputButton.Right;
					buttonData2.eligibleForClick = true;
					buttonData2.delta = Vector2.zero;
					buttonData2.dragging = false;
					buttonData2.useDragThreshold = true;
					buttonData2.pressPosition = buttonData2.position;
					buttonData2.pointerPressRaycast = buttonData2.pointerCurrentRaycast;
					buttonData2.position = this.m_VirtualCursor.anchoredPosition;
					base.DeselectIfSelectionChanged(gameObject3, buttonData2);
					GameObject gameObject4 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject3, buttonData2, ExecuteEvents.pointerDownHandler);
					if (gameObject4 == null)
					{
						gameObject4 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
					}
					float unscaledTime2 = Time.unscaledTime;
					if (gameObject4 == buttonData2.lastPress)
					{
						if (unscaledTime2 - buttonData2.clickTime < 0.3f)
						{
							PointerEventData pointerEventData2 = buttonData2;
							int clickCount = pointerEventData2.clickCount + 1;
							pointerEventData2.clickCount = clickCount;
						}
						else
						{
							buttonData2.clickCount = 1;
						}
						buttonData2.clickTime = unscaledTime2;
					}
					else
					{
						buttonData2.clickCount = 1;
					}
					buttonData2.pointerPress = gameObject4;
					buttonData2.rawPointerPress = gameObject3;
					buttonData2.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject3);
					if (buttonData2.pointerDrag != null)
					{
						ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData2.pointerDrag, buttonData2, ExecuteEvents.initializePotentialDrag);
					}
					this.rightFirstClick = false;
					return;
				}
				if (this.rightReleased)
				{
					buttonData2.button = PointerEventData.InputButton.Right;
					ExecuteEvents.Execute<IPointerUpHandler>(buttonData2.pointerPress, buttonData2, ExecuteEvents.pointerUpHandler);
					GameObject eventHandler2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
					if (buttonData2.pointerPress == eventHandler2 && buttonData2.eligibleForClick)
					{
						ExecuteEvents.Execute<IPointerClickHandler>(buttonData2.pointerPress, buttonData2, ExecuteEvents.pointerClickHandler);
					}
					else if (buttonData2.pointerDrag != null && buttonData2.dragging)
					{
						ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject3, buttonData2, ExecuteEvents.dropHandler);
					}
					buttonData2.eligibleForClick = false;
					buttonData2.pointerPress = null;
					buttonData2.rawPointerPress = null;
					if (buttonData2.pointerDrag != null && buttonData2.dragging)
					{
						ExecuteEvents.Execute<IEndDragHandler>(buttonData2.pointerDrag, buttonData2, ExecuteEvents.endDragHandler);
					}
					buttonData2.dragging = false;
					buttonData2.pointerDrag = null;
					if (gameObject3 != buttonData2.pointerEnter)
					{
						base.HandlePointerExitAndEnter(buttonData2, null);
						base.HandlePointerExitAndEnter(buttonData2, gameObject3);
					}
					this.rightReleased = false;
					return;
				}
			}
		}

		// Token: 0x0600735B RID: 29531 RVA: 0x002C3FB4 File Offset: 0x002C21B4
		protected override PointerInputModule.MouseState GetMousePointerEventData(int id)
		{
			PointerEventData pointerEventData;
			bool pointerData = base.GetPointerData(-1, out pointerEventData, true);
			pointerEventData.Reset();
			Vector2 position = RectTransformUtility.WorldToScreenPoint(this.m_canvasCamera, this.m_VirtualCursor.position);
			if (pointerData)
			{
				pointerEventData.position = position;
			}
			Vector2 anchoredPosition = this.m_VirtualCursor.anchoredPosition;
			pointerEventData.delta = anchoredPosition - pointerEventData.position;
			pointerEventData.position = anchoredPosition;
			pointerEventData.scrollDelta = Input.mouseScrollDelta;
			pointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			PointerEventData pointerEventData2;
			base.GetPointerData(-2, out pointerEventData2, true);
			base.CopyFromTo(pointerEventData, pointerEventData2);
			pointerEventData2.button = PointerEventData.InputButton.Right;
			PointerEventData pointerEventData3;
			base.GetPointerData(-3, out pointerEventData3, true);
			base.CopyFromTo(pointerEventData, pointerEventData3);
			pointerEventData3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, base.StateForMouseButton(0), pointerEventData);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, base.StateForMouseButton(1), pointerEventData2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, base.StateForMouseButton(2), pointerEventData3);
			return this.m_MouseState;
		}

		// Token: 0x04004F67 RID: 20327
		private float m_PrevActionTime;

		// Token: 0x04004F68 RID: 20328
		private Vector2 m_LastMoveVector;

		// Token: 0x04004F69 RID: 20329
		private int m_ConsecutiveMoveCount;

		// Token: 0x04004F6A RID: 20330
		private string debugName;

		// Token: 0x04004F6B RID: 20331
		private Vector2 m_LastMousePosition;

		// Token: 0x04004F6C RID: 20332
		private Vector2 m_MousePosition;

		// Token: 0x04004F6D RID: 20333
		public bool mouseMovementOnly;

		// Token: 0x04004F6E RID: 20334
		[SerializeField]
		private RectTransform m_VirtualCursor;

		// Token: 0x04004F6F RID: 20335
		[SerializeField]
		private float m_VirtualCursorSpeed = 1f;

		// Token: 0x04004F70 RID: 20336
		[SerializeField]
		private Vector2 m_VirtualCursorOffset = Vector2.zero;

		// Token: 0x04004F71 RID: 20337
		[SerializeField]
		private Camera m_canvasCamera;

		// Token: 0x04004F72 RID: 20338
		private Camera VCcam;

		// Token: 0x04004F73 RID: 20339
		public bool CursorCanvasShouldBeOverlay;

		// Token: 0x04004F74 RID: 20340
		private Canvas m_virtualCursorCanvas;

		// Token: 0x04004F75 RID: 20341
		private CanvasScaler m_virtualCursorScaler;

		// Token: 0x04004F76 RID: 20342
		private PointerEventData leftClickData;

		// Token: 0x04004F77 RID: 20343
		private PointerEventData rightClickData;

		// Token: 0x04004F78 RID: 20344
		private VirtualInputModule.ControllerButtonStates conButtonStates;

		// Token: 0x04004F79 RID: 20345
		private GameObject m_CurrentFocusedGameObject;

		// Token: 0x04004F7A RID: 20346
		private bool leftReleased;

		// Token: 0x04004F7B RID: 20347
		private bool rightReleased;

		// Token: 0x04004F7C RID: 20348
		private bool leftFirstClick;

		// Token: 0x04004F7D RID: 20349
		private bool rightFirstClick;

		// Token: 0x04004F7E RID: 20350
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04004F7F RID: 20351
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04004F80 RID: 20352
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04004F81 RID: 20353
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04004F82 RID: 20354
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04004F83 RID: 20355
		[SerializeField]
		private float m_RepeatDelay = 0.5f;

		// Token: 0x04004F84 RID: 20356
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		private bool m_ForceModuleActive;

		// Token: 0x04004F85 RID: 20357
		private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

		// Token: 0x02001F4A RID: 8010
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public enum InputMode
		{
			// Token: 0x04008D31 RID: 36145
			Mouse,
			// Token: 0x04008D32 RID: 36146
			Buttons
		}

		// Token: 0x02001F4B RID: 8011
		private struct ControllerButtonStates
		{
			// Token: 0x04008D33 RID: 36147
			public bool affirmativeDown;

			// Token: 0x04008D34 RID: 36148
			public float affirmativeHoldTime;

			// Token: 0x04008D35 RID: 36149
			public bool negativeDown;

			// Token: 0x04008D36 RID: 36150
			public float negativeHoldTime;
		}
	}
}
