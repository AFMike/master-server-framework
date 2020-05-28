using Barebones.MasterServer;
using Barebones.MasterServer.Examples.BasicProfile;
using Barebones.Networking;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Barebones.Games
{
    [AddComponentMenu("MSF/Shared/ProfilesBehaviour")]
    public class ProfilesManager : BaseClientBehaviour
    {
        #region INSPECTOR

        /// <summary>
        /// Invokes when profile is loaded
        /// </summary>
        public UnityEvent OnProfileLoadedEvent;

        #endregion

        /// <summary>
        /// The loaded profile of client
        /// </summary>
        public ObservableProfile Profile { get; protected set; }

        protected override void OnInitialize()
        {
            Profile = new ObservableProfile
            {
                new ObservableString((short)ObservablePropertiyCodes.DisplayName),
                new ObservableString((short)ObservablePropertiyCodes.Avatar),
                new ObservableFloat((short)ObservablePropertiyCodes.Bronze),
                new ObservableFloat((short)ObservablePropertiyCodes.Silver),
                new ObservableFloat((short)ObservablePropertiyCodes.Gold)
            };
        }

        /// <summary>
        /// Invokes when user profile is loaded
        /// </summary>
        public virtual void OnProfileLoaded() { }

        /// <summary>
        /// Get profile data from master
        /// </summary>
        public void LoadProfile()
        {
            Msf.Events.Invoke(MsfEventKeys.showLoadingInfo, "Loading profile... Please wait!");

            MsfTimer.WaitForSeconds(1f, () =>
            {
                Msf.Client.Profiles.GetProfileValues(Profile, (isSuccessful, error) =>
                {
                    if (isSuccessful)
                    {
                        Msf.Events.Invoke(MsfEventKeys.hideLoadingInfo);
                        OnProfileLoadedEvent?.Invoke();
                    }
                    else
                    {
                        Msf.Events.Invoke(MsfEventKeys.showOkDialogBox,
                            new OkDialogBoxViewEventMessage($"When requesting profile data an error occurred. [{error}]"));
                    }
                });
            });
        }
    }
}