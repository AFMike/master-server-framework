using Aevien.UI;
using Barebones.Logging;
using Barebones.MasterServer;
using Barebones.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Aevien.SpaceBattles
{
    public class GamesListView : UIView
    {
        #region INSPECTOR

        [Header("Prefabs"), SerializeField]
        private GameCardView gameCardViewPrefab;

        #endregion

        private RectTransform gamesListContainer;
        private TextMeshProUGUI infoText;
        private UIButton updateGamesListButton;
        private MainMenuManager mainMenuManager;

        protected override void Start()
        {
            base.Start();

            mainMenuManager = FindObjectOfType<MainMenuManager>();

            gamesListContainer = ChildComponent<RectTransform>("gamesListContainer");
            infoText = ChildComponent<TextMeshProUGUI>("infoText");
            updateGamesListButton = ChildComponent<UIButton>("updateGamesListButton");
        }

        /// <summary>
        /// Invokes when view is shown
        /// </summary>
        protected override void OnShow()
        {
            UpdateList();
        }

        /// <summary>
        /// Invokes when find game request is finished
        /// </summary>
        /// <param name="games"></param>
        private void OnFindGamesCallbackHandler(List<GameInfoPacket> games)
        {
            updateGamesListButton.SetInteractable(true);

            if (games.Count > 0)
            {
                DrawGamesList(games);
                OutputMessage(string.Empty);

                foreach(var game in games)
                {
                    Logs.Info(game);
                }
            }
            else
            {
                OutputMessage("No Game found! Please try again or contact to administrator.");
                Logs.Warn("No Game found! Please try again or contact to administrator.");
            }
        }

        /// <summary>
        /// Draws games list view
        /// </summary>
        /// <param name="games"></param>
        public void DrawGamesList(List<GameInfoPacket> games)
        {
            if (gameCardViewPrefab)
            {
                foreach (var game in games)
                {
                    var gameCard = Instantiate(gameCardViewPrefab, gamesListContainer, false);
                    gameCard.SetTitle(game.Name);
                    gameCard.SetButtonClick(()=> {
                        Hide();
                        mainMenuManager.StartGame(game);
                    });
                }
            }
        }

        /// <summary>
        /// Clears games list view
        /// </summary>
        public void ClearGamesList()
        {
            foreach(Transform i in gamesListContainer)
            {
                Destroy(i.gameObject);
            }
        }

        /// <summary>
        /// Updates games list view
        /// </summary>
        public void UpdateList()
        {
            // Show message at the middle of screen
            OutputMessage("Loading games list. Please wait...");

            // Set the Update button inactive
            updateGamesListButton.SetInteractable(false);

            // Clear list of games
            ClearGamesList();

            // Let's find games again to refresh list
            MsfTimer.WaitForSeconds(1f, () => Msf.Client.Matchmaker.FindGames(OnFindGamesCallbackHandler));
        }

        /// <summary>
        /// Output games list view message
        /// </summary>
        /// <param name="msg"></param>
        public void OutputMessage(string msg)
        {
            infoText.text = msg;
        }
    }
}