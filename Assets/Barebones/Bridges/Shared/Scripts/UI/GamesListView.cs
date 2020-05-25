using Aevien.UI;
using Barebones.Logging;
using Barebones.MasterServer;
using Barebones.Networking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Barebones.Games
{
    public class GamesListView : UIView
    {
        [Header("Components"), SerializeField]
        private GameItem gameItemPrefab;
        [SerializeField]
        private RectTransform listContainer;

        public UnityEvent OnStartGameEvent;

        protected override void OnShow()
        {
            FindGames();
        }

        public void FindGames()
        {
            ClearGamesList();

            Msf.Events.Invoke(MsfEventKeys.showLoadingInfo, "Finding rooms... Please wait!");

            MsfTimer.WaitForSeconds(1f, () =>
            {
                Msf.Client.Matchmaker.FindGames((games) =>
                {
                    Msf.Events.Invoke(MsfEventKeys.hideLoadingInfo);

                    if (games.Count == 0)
                    {
                        Msf.Events.Invoke(MsfEventKeys.showOkDialogBox, new OkDialogBoxViewEventMessage("No games found!"));
                        return;
                    }

                    DrawGamesList(games);
                });
            });
        }

        private void DrawGamesList(IEnumerable<GameInfoPacket> games)
        {
            if (listContainer && gameItemPrefab)
            {
                foreach (GameInfoPacket game in games)
                {
                    var gameItemInstance = Instantiate(gameItemPrefab, listContainer, false);
                    gameItemInstance.SetInfo(game, this);

                    Logs.Info(game);
                }
            }
            else
            {
                Logs.Error("Not all components are setup");
            }
        }

        private void ClearGamesList()
        {
            if (listContainer)
            {
                foreach (Transform tr in listContainer)
                {
                    Destroy(tr.gameObject);
                }
            }
        }

        public void StartGame(GameInfoPacket gameInfo)
        {
            OnStartGameEvent?.Invoke();
            Msf.Options.Set(MsfDictKeys.roomId, gameInfo.Id);
        }
    }
}