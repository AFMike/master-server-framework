using Barebones.Networking;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace Barebones.MasterServer
{
    public class MsfHelper
    {
        private const string dictionaryString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int maxGeneratedStringLength = 512;

        /// <summary>
        /// Creates a random string of a given length. Min length is 1, max length <see cref="maxGeneratedStringLength"/>
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string CreateRandomString(int length)
        {
            int clampedLength = Mathf.Clamp(length, 1, maxGeneratedStringLength);

            StringBuilder resultStringBuilder = new StringBuilder();

            for (int i = 0; i < clampedLength; i++)
            {
                resultStringBuilder.Append(dictionaryString[UnityEngine.Random.Range(0, dictionaryString.Length)]);
            }

            return resultStringBuilder.ToString();
        }

        /// <summary>
        /// Create 128 bit unique string
        /// </summary>
        /// <returns></returns>
        public string CreateGuidString()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Retrieves current public IP
        /// </summary>
        /// <param name="callback"></param>
        public void GetPublicIp(Action<MsfIpInfo> callback)
        {
            MsfTimer.Instance.StartCoroutine(GetPublicIPCoroutine(callback));
        }

        /// <summary>
        /// Wait for loading public IP from https://ifconfig.co/json
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator GetPublicIPCoroutine(Action<MsfIpInfo> callback)
        {
            UnityWebRequest www = UnityWebRequest.Get("https://ifconfig.co/json");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var ipInfo = JsonConvert.DeserializeObject<MsfIpInfo>(www.downloadHandler.text);

                Debug.Log(ipInfo);

                callback?.Invoke(ipInfo);
            }
        }
    }
}