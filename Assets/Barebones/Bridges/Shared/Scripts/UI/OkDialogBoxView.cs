﻿using Aevien.UI;
using Barebones.MasterServer;
using UnityEngine;

namespace Barebones.Games
{
    [RequireComponent(typeof(UIView))]
    public class OkDialogBoxView : PopupViewComponent
    {
        public override void OnOwnerStart()
        {
            Msf.Events.AddEventListener(MsfEventKeys.showOkDialogBox, OnShowDialogBoxEventHandler);
            Msf.Events.AddEventListener(MsfEventKeys.hideOkDialogBox, OnHideDialogBoxEventHandler);
        }

        private void OnShowDialogBoxEventHandler(EventMessage message)
        {
            var messageData = message.GetData<OkDialogBoxEventMessage>();

            SetLables(messageData.Message);

            SetButtonsClick(() =>
            {
                messageData.OkCallback?.Invoke();
                Owner.Hide();
            });

            Owner.Show();
        }

        private void OnHideDialogBoxEventHandler(EventMessage message)
        {
            Owner.Hide();
        }
    }
}
