using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Playfab
{
    public class AutoLoginOnPlay : MonoBehaviour
    {
        public string playerName = "DorTesterPlayer";
        public UnityEvent LoginSuccessful;

        private void Awake()
        {
            var r = new LoginWithCustomIDRequest() { CustomId = playerName, TitleId = "A086D", CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(r, (res) => LoginSuccessful.Invoke(), (err) => print(err.GenerateErrorReport()));
        }
    }
}
