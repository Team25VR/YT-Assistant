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
        public const string Version = "1.4.0";

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
        private static bool SwitchReady = true;
        private static int GenerateAmount = 1;

        public static void Prefix()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton && SwitchReady)
            {
                SwitchReady = false;

                if (GenerateAmount == 0)
                {
                    GenerateAmount = 1;
                }
                if (GenerateAmount > 5)
                {
                    GenerateAmount = 5;
                }

                GorillaComputer.instance.roomToJoin = "";
                GorillaComputer.instance.roomToJoin = HashString(DateTime.Now.Ticks.ToString()).ToUpper();

                GenerateAmount = 0;
            }
            if (ControllerInputPoller.instance.rightControllerSecondaryButton && SwitchReady)
            {
                SwitchReady = false;

                GenerateAmount += 1;
            }
            SwitchReady = true;
        }

        public static string HashString(string source)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new
            System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(source);
            data = x.ComputeHash(data);
            string ret = "";

            for (int i = 0; i < GenerateAmount; i++)
            {
                ret += data[i].ToString("x2").ToLower();
            }

            return ret;
        }

    }
}