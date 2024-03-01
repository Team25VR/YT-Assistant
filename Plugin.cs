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
                GorillaComputer.instance.roomToJoin = "MONKE" + UnityEngine.Random.Range(1000, 10000).ToString();
                CodeStaller();

                SwitchReady = true;
            }
            if (ControllerInputPoller.instance.rightControllerSecondaryButton && SwitchReady)
            {
                SwitchReady = false;

                PhotonNetwork.Disconnect();
                CodeStaller();

                SwitchReady = true;
            }
        }

        public static IEnumerator CodeStaller()
        {
            yield return new WaitForSeconds(0.2f);
        }
    }
}