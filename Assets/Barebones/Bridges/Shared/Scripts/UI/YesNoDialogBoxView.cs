using Aevien.UI;
using Barebones.MasterServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Games
{
    [RequireComponent(typeof(UIView))]
    public class YesNoDialogBoxView : PopupViewComponent
    {
        public override void OnOwnerStart()
        {
            Msf.Events.AddEventListener(MsfEventKeys.showYesNoDialogBox, OnShowDialogBoxEventHandler);
            Msf.Events.AddEventListener(MsfEventKeys.hideYesNoDialogBox, OnHideDialogBoxEventHandler);
        }

        private void OnShowDialogBoxEventHandler(EventMessage message)
        {
            var messageData = message.GetData<YesNoDialogBoxEventMessage>();

            SetLables(messageData.Message);

            SetButtonsClick(() =>
            {
                messageData.YesCallback?.Invoke();
                Owner.Hide();
            }, () =>
            {
                messageData.NoCallback?.Invoke();
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
