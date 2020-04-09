using Aevien.UI;
using Barebones.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.MasterServer.Examples.BasicSpawner
{
    public class ClientManager : MsfBaseClientModule
    {
        private MainView mainView;

        protected override void Initialize()
        {
            Msf.Client.Rooms.ForceClientMode = true;

            mainView = ViewsManager.GetView<MainView>("MainView");

            MsfTimer.WaitForEndOfFrame(() =>
            {
                Msf.Events.Invoke(EventKeys.showLoadingInfo, "Signing in... Please wait!");
            });
        }

        protected override void OnClientConnectedToServer()
        {
            MsfTimer.WaitForSeconds(1f, () =>
            {
                Msf.Client.Auth.SignInAsGuest(OnSignedInAsGuest);
            });
        }

        private void OnSignedInAsGuest(AccountInfoPacket accountInfo, string error)
        {
            Msf.Events.Invoke(EventKeys.hideLoadingInfo);

            if (accountInfo == null)
            {
                Msf.Events.Invoke(EventKeys.showOkDialogBox, new OkDialogBoxViewEventMessage(error));

                logger.Error(error);
                return;
            }

            mainView?.Show();

            logger.Info("Successfully signed in!");
        }

        public void Quit()
        {
            Msf.Runtime.Quit();
        }
    }
}
