using Barebones.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.MasterServer.Examples.BasicSpawner
{
    public class RoomTestsBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            Logs.Info(string.Join(" ", Environment.GetCommandLineArgs()));
        }
    }
}