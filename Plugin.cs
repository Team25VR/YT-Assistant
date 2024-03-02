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
        public const string Version = "1.6.0";

        public void Awake()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("FixedUpdate", MethodType.Normal)]
    public class PluginMain : MonoBehaviour
    {
        public static bool Ready = true;

        public static void Prefix()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton && Ready)
            {
                Ready = false;

                GorillaComputer.instance.currentName = "";
                GorillaComputer.instance.currentName = HashString(DateTime.Now.Ticks.ToString()).ToUpper();

                Ready = true;
            }

            if (ControllerInputPoller.instance.rightControllerSecondaryButton && Ready)
            {
                Ready = false;

                GorillaComputer.instance.roomToJoin = "";
                GorillaComputer.instance.roomToJoin = HashString(DateTime.Now.Ticks.ToString()).ToUpper();

                Ready = true;
            }

            if (GorillaComputer.instance.currentGameMode.ToString().Contains("MODDED"))
            {
                if (ControllerInputPoller.instance.rightControllerPrimaryButton && ControllerInputPoller.instance.rightControllerSecondaryButton && Ready)
                {
                    Ready = false;

                    GorillaComputer.instance.currentName = "";
                    GorillaComputer.instance.roomToJoin = "";
                    PhotonNetwork.Disconnect();

                    Ready = true;
                }

                if (ControllerInputPoller.instance.leftControllerPrimaryButton && ControllerInputPoller.instance.leftControllerSecondaryButton && ControllerInputPoller.instance.rightControllerPrimaryButton && ControllerInputPoller.instance.rightControllerSecondaryButton && Ready)
                {
                    Ready = false;

                    GorillaComputer.instance.currentName = "";
                    GorillaComputer.instance.roomToJoin = "";
                    Application.Quit();

                    Ready = true;
                }
            }
        }

        public static string HashString(string source)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new
            System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(source);
            data = x.ComputeHash(data);
            string ret = "";

            for (int i = 0; i < UnityEngine.Random.Range(2, 4); i++)
            {
                ret += data[i].ToString("x2").ToLower();
            }

            return ret;
        }
    }
}