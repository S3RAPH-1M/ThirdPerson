using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace ThirdPerson
{
    [BepInPlugin("com.servph.thirdperson", "Third Person", "1.2.0")]
    public class TPPEntry : BaseUnityPlugin
    {
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
        public ConfigEntry<float> Camera_ADS_Speed { get; private set; }
        public ConfigEntry<float> CameraUN_ADS_Speed { get; private set; }
        public ConfigEntry<float> CrosshairSize { get; private set; }
        public ConfigEntry<float> CrosshairThickness { get; private set; }
        public ConfigEntry<float> CrosshairGap { get; private set; }
        public ConfigEntry<Color> Crosshaircolor { get; private set; }
        public static bool Enabled = false;




        public Vector3 TPPVector = new Vector3(0.0398f, 0.3402f, -1.4504f);
        public Vector3 AimingVector = new Vector3(0.2392f, 0.143f, -0.256f);
        public Vector3 AimingVectorLeft = new Vector3(-0.3392f, 0.143f, -0.256f);

        public void Awake()
        {
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
            IsInTPP = false;


        }



        public void Update()
        {
            if (!Singleton<GameWorld>.Instantiated)
            {
                this.LocalPlayer = null;
                return;
            }

            GameWorld gameWorld = Singleton<GameWorld>.Instance;

            if (this.LocalPlayer == null && gameWorld.RegisteredPlayers.Count > 0)
            {
                this.LocalPlayer = gameWorld.RegisteredPlayers[0];
                return;
            }

            if (Input.GetKeyDown(ThirdPersonBind.Value))
            {
                ThirdPerson.Value = !ThirdPerson.Value;
            }

            if (Input.GetKeyDown(FirstPersonADSBind.Value))
            {
                FirstPersonADS.Value = !FirstPersonADS.Value;
            }


            Instance.LocalPlayer.HitReaction.enabled = Instance.LocalPlayer.PointOfView != EPointOfView.ThirdPerson;

            if (this.LocalPlayer != null && this.LocalPlayer.CameraContainer != null && this.LocalPlayer.ActiveHealthController != null && ThirdPerson.Value)
            {
                Instance.LocalPlayer.PointOfView = EPointOfView.ThirdPerson;
                IsInTPP = true;
                float cameraADSSpeed = Camera_ADS_Speed.Value;
                float cameraUNADSSpeed = CameraUN_ADS_Speed.Value;

                if (Instance.LocalPlayer.HandsController is Player.ItemHandsController controller)
                {
                    if (Instance.LocalPlayer.CameraPosition != null)
                    {
                        if (controller.IsAiming && FirstPersonADS.Value == false)
                        {
                            if (SwapShoulder.Value == true)
                            {
                                Instance.LocalPlayer.CameraPosition.localPosition = Vector3.Lerp(Instance.LocalPlayer.CameraPosition.localPosition, AimingVectorLeft, cameraADSSpeed * Time.deltaTime);
                                Instance.LocalPlayer.CameraPosition.localRotation = Quaternion.Lerp(Instance.LocalPlayer.CameraPosition.localRotation, Quaternion.identity, cameraADSSpeed * Time.deltaTime);


                            }
                            else if (SwapShoulder.Value == false)
                            {
                                Instance.LocalPlayer.CameraPosition.localPosition = Vector3.Lerp(Instance.LocalPlayer.CameraPosition.localPosition, AimingVector, cameraADSSpeed * Time.deltaTime);
                                Instance.LocalPlayer.CameraPosition.localRotation = Quaternion.Lerp(Instance.LocalPlayer.CameraPosition.localRotation, Quaternion.identity, cameraADSSpeed * Time.deltaTime);

                            }
                        }
                        if (controller.IsAiming && FirstPersonADS.Value)
                        {
                            if (HasFPPAimed == false)
                            {
                                Instance.LocalPlayer.CameraPosition.localPosition = new Vector3(0f, 0f, 0f);
                                HasFPPAimed = true;
                            }
                            Instance.LocalPlayer.PointOfView = EPointOfView.FirstPerson;
                        }
                        else if (!controller.IsAiming)
                        {
                            HasFPPAimed = false;
                            Instance.LocalPlayer.CameraPosition.localPosition = Vector3.Lerp(Instance.LocalPlayer.CameraPosition.localPosition, TPPVector, cameraUNADSSpeed * Time.deltaTime);
                        }

                        if (Input.GetKeyDown(SwapShoulderBind.Value))
                        {
                            SwapShoulder.Value = !SwapShoulder.Value;
                        }
                    }
                }
            }

            if (ThirdPerson.Value == false && IsInTPP == true)
            {
                Instance.LocalPlayer.PointOfView = EPointOfView.FirstPerson;
                IsInTPP = false;
            }

        }
        public void OnGUI()
        {
            if (Instance.LocalPlayer != null && Instance.LocalPlayer.HandsController is Player.FirearmController && CrosshairEnabled.Value)
            {
               Crosshair.Draw();
            }
        }
    }
}