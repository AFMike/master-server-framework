using Barebones.MasterServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MsfBaseClientModule
{
    private void OnEnable()
    {
        //Msf.Events.AddEventListener("SetStringValue", (s) => OnHello());
    }

    private void OnDisable()
    {
        //Msf.Events.RemoveEventListener("SetStringValue", );
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Msf.Events.AddEventListener("SetStringValue", MyHandler);
            Msf.Events.RemoveEventListener("SetStringValue", MyHandler);
        }
    }

    private void MyHandler(EventMessage message)
    {
        // My code here...
    }
}
