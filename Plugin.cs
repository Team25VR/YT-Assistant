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
using Utilla;

namespace YT_Assistant
{
    [BepInPlugin(GUID, Name, Version)]
    [BepInDependency(DEP, Release)]
    [ModdedGamemode]
    public class PluginInfo : BaseUnityPlugin
    {
        public const string GUID = "dev.team25vr.yt_assistant";
        public const string Name = "YT Assistant";
        public const string Version = "1.7.0";

        public const string DEP = "org.legoandmars.gorillatag.utilla";
        public const string Release = "1.5.0";

        public void Awake()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [ModdedGamemodeJoin]
        private void RoomJoined(string gamemode)
        {
            PluginMain.InModded = true;
        }

        [ModdedGamemodeLeave]
        private void RoomLeft(string gamemode)
        {
            PluginMain.InModded = false;
        }
    }

    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("FixedUpdate", MethodType.Normal)]
    public class PluginMain : MonoBehaviour
    {
        public static bool Ready = true;
        public static bool InModded = false;

        public static void Prefix()
        {
            NonModdedFunctions();

            if (InModded)
            {
                ModdedFunctions();
            }
        }

        private static void NonModdedFunctions()
        {
            GenerateRoomOn();
            GenerateNameOn();
        }

        private static void ModdedFunctions()
        {
            DisconnectFromRoomOn();
            CloseGorillaTagOn();
            FlyAdjustableOn();
        }

        private static void GenerateRoomOn()
        {
            if (ControllerInputPoller.instance.rightControllerSecondaryButton && Ready)
            {
                Ready = false;

                GorillaComputer.instance.roomToJoin = "";
                GorillaComputer.instance.roomToJoin = HashStringStart(DateTime.Now.Ticks.ToString()).ToUpper();

                Ready = true;
            }
        }

        private static void GenerateNameOn()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton && Ready)
            {
                Ready = false;

                GorillaComputer.instance.currentName = "";
                GorillaComputer.instance.currentName = HashStringStart(DateTime.Now.Ticks.ToString()).ToUpper();

                Ready = true;
            }
        }

        private static void DisconnectFromRoomOn()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton && ControllerInputPoller.instance.rightControllerSecondaryButton && Ready)
            {
                Ready = false;

                PhotonNetwork.Disconnect();

                Ready = true;
            }
        }

        private static void CloseGorillaTagOn()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton && ControllerInputPoller.instance.leftControllerSecondaryButton && ControllerInputPoller.instance.rightControllerPrimaryButton && ControllerInputPoller.instance.rightControllerSecondaryButton && Ready)
            {
                Ready = false;

                Application.Quit();

                Ready = true;
            }
        }

        public static string HashStringStart(string source)
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

        private static void FlyAdjustableOn()
        {
            bool OtherMode = false;
            if (ControllerInputPoller.instance.leftControllerSecondaryButton && ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                OtherMode = true;
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 8f;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (ControllerInputPoller.instance.leftControllerSecondaryButton && !OtherMode)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 16f;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (ControllerInputPoller.instance.leftControllerSecondaryButton && ControllerInputPoller.instance.rightControllerSecondaryButton)
            {
                OtherMode = true;
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 24f;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (!ControllerInputPoller.instance.leftControllerSecondaryButton)
            {
                OtherMode = false;
            }
        }
    }
}