using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using BepInEx;
using GorillaExtensions;
using GorillaNetworking;
using HarmonyLib;
using Pathfinding.Util;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Windows;

namespace YT_Assistant
{
    [BepInPlugin(GUID, Name, Version)]
    public class PluginInfo : BaseUnityPlugin
    {
        public const string GUID = "dev.team25vr.yt_assistant";
        public const string Name = "YT Assistant";
        public const string Version = "1.0.0";

        public void Awake()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("FixedUpdate", MethodType.Normal)]
    public class PluginMain
    {
        static void Prefix(GorillaLocomotion.Player __instance)
        {
            bool SwitchReady = true;

            if (ControllerInputPoller.instance.rightControllerPrimaryButton && SwitchReady)
            {
                SwitchReady = false;

                GorillaComputer.instance.roomToJoin = "";
                GorillaComputer.instance.roomToJoin = "MONKE" + UnityEngine.Random.Range(100, 1000).ToString();

                SwitchReady = true;
            }
            if (ControllerInputPoller.instance.rightControllerSecondaryButton && SwitchReady)
            {
                SwitchReady = false;

                GorillaComputer.instance.roomToJoin = "";
                GorillaComputer.instance.roomToJoin = "GORILLA" + UnityEngine.Random.Range(100, 1000).ToString();

                SwitchReady = true;
            }
            if (ControllerInputPoller.instance.rightControllerPrimaryButton && ControllerInputPoller.instance.rightControllerSecondaryButton && SwitchReady)
            {
                SwitchReady = false;

                PhotonNetwork.Disconnect();

                SwitchReady = true;
            }
        }
    }
}