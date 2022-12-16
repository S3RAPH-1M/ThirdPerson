using System;
using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace ThirdPerson {
	[BepInPlugin("com.servph.thirdperson", "Third Person", "1.2.0")]
	public class TPPEntry : BaseUnityPlugin {
		public Vector3 TPPVector = new Vector3(0.0398f, 0.3402f, -1.4504f);
		public Vector3 AimingVector = new Vector3(0.2392f, 0.143f, -0.256f);
		public Vector3 AimingVectorLeft = new Vector3(-0.3392f, 0.143f, -0.256f);
		public Player LocalPlayer { get; private set; }
		public ConfigEntry<Boolean> ThirdPerson { get; private set; }
		public ConfigEntry<Boolean> FirstPersonADS { get; private set; }
		public ConfigEntry<KeyCode> ThirdPersonBind { get; private set; }
		public ConfigEntry<KeyCode> FirstPersonADSBind { get; private set; }
		public ConfigEntry<KeyCode> SwapShoulderBind { get; private set; }
		public ConfigEntry<Boolean> SwapShoulder { get; private set; }
		public ConfigEntry<Boolean> CrosshairEnabled { get; private set; }
		public static TPPEntry Instance { get; private set; }
		public Boolean IsInTPP { get; set; }
		public Boolean HasFPPAimed { get; set; }
		public ConfigEntry<Single> Camera_ADS_Speed { get; private set; }
		public ConfigEntry<Single> CameraUN_ADS_Speed { get; private set; }
		public ConfigEntry<Single> CrosshairSize { get; private set; }
		public ConfigEntry<Single> CrosshairThickness { get; private set; }
		public ConfigEntry<Single> CrosshairGap { get; private set; }
		public ConfigEntry<Color> Crosshaircolor { get; private set; }

		public void Awake() {
			Instance = this;
			this.ThirdPerson = this.Config.Bind("Camera Settings", "Goto TPP", false);
			this.FirstPersonADS = this.Config.Bind("Camera Settings", "ADS in First Person", false);
			this.FirstPersonADSBind = this.Config.Bind("Camera Settings", "ADS First Person Bind", KeyCode.None);
			this.ThirdPersonBind = this.Config.Bind("Camera Settings", "TPP Bind", KeyCode.None);
			this.SwapShoulder = this.Config.Bind("Camera Settings", "Swap Shoulder Enabled", false);
			this.SwapShoulderBind = this.Config.Bind("Camera Settings", "Swap Shoulder", KeyCode.None);
			this.CameraUN_ADS_Speed = this.Config.Bind("Camera Settings", "Camera Un-ADS Speed", 6f);
			this.Camera_ADS_Speed = this.Config.Bind("Camera Settings", "Camera ADS Speed", 6f);
			this.CrosshairEnabled = this.Config.Bind("Crosshair Settings", "Crosshair Enabled", false);
			this.CrosshairSize = this.Config.Bind("Crosshair Settings", "Crosshair Size", 5f);
			this.CrosshairThickness = this.Config.Bind("Crosshair Settings", "Crosshair Thickness", 1f);
			this.CrosshairGap = this.Config.Bind("Crosshair Settings", "Crosshair Gap", 1f);
			this.Crosshaircolor = this.Config.Bind("Crosshair Settings", "Crosshair Color", Color.green);
			this.IsInTPP = false;
		}

		public void Update() {
			if (!Singleton<GameWorld>.Instantiated) {
				this.LocalPlayer = null;
				return;
			}

			GameWorld gameWorld = Singleton<GameWorld>.Instance;
			if (this.LocalPlayer == null) {
				if (gameWorld.RegisteredPlayers.Count > 0) {
					this.LocalPlayer = gameWorld.RegisteredPlayers[0];
				}

				return;
			}

			if (Input.GetKeyDown(this.ThirdPersonBind.Value)) {
				this.ThirdPerson.Value = !this.ThirdPerson.Value;
			}

			if (Input.GetKeyDown(this.FirstPersonADSBind.Value)) {
				this.FirstPersonADS.Value = !this.FirstPersonADS.Value;
			}

			if (Instance.LocalPlayer.HitReaction != null) {
				Instance.LocalPlayer.HitReaction.enabled = Instance.LocalPlayer.PointOfView != EPointOfView.ThirdPerson;
			}

			if (this.LocalPlayer.CameraContainer != null && this.ThirdPerson.Value) {
				Instance.LocalPlayer.PointOfView = EPointOfView.ThirdPerson;
				this.IsInTPP = true;
				Single cameraADSSpeed = this.Camera_ADS_Speed.Value;
				Single cameraUNADSSpeed = this.CameraUN_ADS_Speed.Value;
				if (Instance.LocalPlayer.HandsController is Player.ItemHandsController controller) {
					if (Instance.LocalPlayer.CameraPosition != null) {
						if (controller.IsAiming && this.FirstPersonADS.Value == false) {
							switch (this.SwapShoulder.Value) {
								case true:
									Instance.LocalPlayer.CameraPosition.localPosition = Vector3.Lerp(
										Instance.LocalPlayer.CameraPosition.localPosition, this.AimingVectorLeft,
										cameraADSSpeed * Time.deltaTime);
									Instance.LocalPlayer.CameraPosition.localRotation = Quaternion.Lerp(
										Instance.LocalPlayer.CameraPosition.localRotation, Quaternion.identity,
										cameraADSSpeed * Time.deltaTime);
									break;
								case false:
									Instance.LocalPlayer.CameraPosition.localPosition = Vector3.Lerp(
										Instance.LocalPlayer.CameraPosition.localPosition, this.AimingVector,
										cameraADSSpeed * Time.deltaTime);
									Instance.LocalPlayer.CameraPosition.localRotation = Quaternion.Lerp(
										Instance.LocalPlayer.CameraPosition.localRotation, Quaternion.identity,
										cameraADSSpeed * Time.deltaTime);
									break;
							}
						}

						switch (controller.IsAiming) {
							case true when this.FirstPersonADS.Value: {
								if (this.HasFPPAimed == false) {
									Instance.LocalPlayer.CameraPosition.localPosition = new Vector3(0f, 0f, 0f);
									this.HasFPPAimed = true;
								}

								Instance.LocalPlayer.PointOfView = EPointOfView.FirstPerson;
								break;
							}
							case false:
								this.HasFPPAimed = false;
								Instance.LocalPlayer.CameraPosition.localPosition = Vector3.Lerp(
									Instance.LocalPlayer.CameraPosition.localPosition, this.TPPVector,
									cameraUNADSSpeed * Time.deltaTime);
								break;
						}

						if (Input.GetKeyDown(this.SwapShoulderBind.Value)) {
							this.SwapShoulder.Value = !this.SwapShoulder.Value;
						}
					}
				}
			}

			if (!this.ThirdPerson.Value && this.IsInTPP) {
				Instance.LocalPlayer.PointOfView = EPointOfView.FirstPerson;
				this.IsInTPP = false;
			}
		}

		public void OnGUI() {
			if (Instance.LocalPlayer != null && Instance.LocalPlayer.HandsController is Player.FirearmController &&
			    this.CrosshairEnabled.Value) {
				Crosshair.Draw();
			}
		}
	}
}