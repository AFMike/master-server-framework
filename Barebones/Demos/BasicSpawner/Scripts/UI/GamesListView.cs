using Aevien.UI;
using Barebones.Logging;
using Barebones.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.MasterServer.Examples.BasicSpawner
{
    public class GamesListView : UIView
    {
        protected override void OnShow()
        {
            Msf.Events.Invoke(EventKeys.showLoadingInfo, "Finding rooms... Please wait!");

            MsfTimer.WaitForSeconds(1f, () =>
            {
                Msf.Client.Matchmaker.FindGames((games) =>
                {
                    Msf.Events.Invoke(EventKeys.hideLoadingInfo);

                    if (games.Count == 0)
                    {
                        Msf.Events.Invoke(EventKeys.showOkDialogBox, new OkDialogBoxViewEventMessage("No games found!"));
                        return;
                    }

                    foreach(GameInfoPacket game in games)
                    {
                        Logs.Info(game);
                    }
                });
            });
        }
    }
}