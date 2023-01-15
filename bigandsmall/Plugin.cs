using BepInEx;
using System;
using GorillaLocomotion;
using UnityEngine;
using System.ComponentModel;
using BepInEx.Configuration;
using Utilla;
using System.Collections.Generic;
using UnityEngine.XR;

namespace bigandsmall
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [Description("HauntedModMenu")]
    [BepInPlugin("com.shibagt.gorillatag.bigandsmall", "big and small", "1.0.0")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [ModdedGamemode]
    public class Plugin : BaseUnityPlugin
    {
        float scale = 1f;
        bool triggerpress;
        bool resetbutton;
        bool triggerpress2;
        bool on = false;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            Player.Instance.scale = 1f;
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Player.Instance.scale = 1f;
        }

        void Update()
        {
            if (on)
            {
                SizeManager sizeManager;
                Player.Instance.TryGetComponent<SizeManager>(out sizeManager);
                List<InputDevice> list = new List<InputDevice>();
                List<InputDevice> list2 = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list2);
                list[0].TryGetFeatureValue(CommonUsages.triggerButton, out triggerpress);
                list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out resetbutton);
                list2[0].TryGetFeatureValue(CommonUsages.triggerButton, out triggerpress2);
                if (resetbutton)
                {
                    Player.Instance.scale = 1f;
                    scale = 1f;
                }
                if (triggerpress)
                {
                    sizeManager.enabled = false;
                    scale = scale + 0.1f;
                    Player.Instance.scale = scale;
                    System.Threading.Thread.Sleep(100);
                }
                if (triggerpress2)
                {
                    sizeManager.enabled = false;
                    scale = scale - 0.1f;
                    Player.Instance.scale = scale;
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            on = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            on = false;
        }
    }
}
