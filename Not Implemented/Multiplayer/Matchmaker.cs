using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using PlayFab.AuthenticationModels;
using EntityKey = PlayFab.MultiplayerModels.EntityKey;
using System;

namespace MP
{
    public class Matchmaker : MonoBehaviour
    {
        #region Interface

        public static readonly string[] queueOptions = { "BattleDice_OneVsOneQueue", "FFAQueue", "OneVsOneQueue", "TwoVsTwoQueue" };
        public string SelectedQueue { get { return queueOptions[_selection]; } }

        public void FindMatchForPlayer()
        {
            StartCoroutine(FindMatchForPlayerSequence());
        }

        #endregion

        #region Implementation

        [Min(6)]
        [SerializeField] int _checkMatchingStatusInterval = 6;
        [SerializeField] [HideInInspector] int _selection;
        [SerializeField] int _matchTimeout = 120;

        EntityKey _entityKey = null;
        string _ticketID;

        IEnumerator FindMatchForPlayerSequence()
        {
            SetEntityToken();
            yield return new WaitUntil(() => _entityKey != null);
            CreateMatchmakingTicket();
            yield return new WaitWhile(() => string.IsNullOrEmpty(_ticketID));
            yield return StartCoroutine(CheckMatchMakingStatus());
        }

        void CreateMatchmakingTicket()
        {
            PlayFabMultiplayerAPI.CreateMatchmakingTicket(
                new CreateMatchmakingTicketRequest
                {
                    Creator = new MatchmakingPlayer
                    {
                        Entity = _entityKey,
                    },
                    GiveUpAfterSeconds = _matchTimeout,
                    QueueName = SelectedQueue,
                },
                (result) => _ticketID = result.TicketId,
                (error) => print(error.GenerateErrorReport())
                );
        }

        void CancelMatchmaking()
        {
            var r = new CancelAllMatchmakingTicketsForPlayerRequest { Entity = _entityKey, QueueName = SelectedQueue };
            PlayFabMultiplayerAPI.CancelAllMatchmakingTicketsForPlayer(r, (result) => Debug.Log("Cancelled matchmaking for player"), (error) => print(error.GenerateErrorReport()));
        }

        void StartMatch()
        {
            //RoomOptions options = new RoomOptions { MaxPlayers = botMatch ? (byte)1 : (byte)(versusType == MPVersusType.OneVsOne ? 2 : 4), CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "VersusType", versusType.ToString() } } };
            //roomNameToCreate = roomName;
            //PhotonNetwork.CreateRoom(roomName, options, null);
        }

        IEnumerator CheckMatchMakingStatus()
        {
            GetMatchmakingTicketResult matchmakingResult = null;

            if (string.IsNullOrEmpty(_ticketID))
            {
                Debug.LogError("You are trying to get matchmaking status with no ticket ID");
                yield break;
            }

            while (matchmakingResult == null || matchmakingResult.Status != "Matched")
            {
                if (matchmakingResult != null && matchmakingResult.Status == "Canceled")
                    yield break;

                PlayFabMultiplayerAPI.GetMatchmakingTicket(
                new GetMatchmakingTicketRequest
                {
                    QueueName = SelectedQueue,
                    TicketId = _ticketID
                },
                (result) => matchmakingResult = result,
                (error) => print(error.GenerateErrorReport())
                );

                yield return new WaitForSecondsRealtime(_checkMatchingStatusInterval);

                Debug.Log(matchmakingResult.Status);
            }

        }

        void SetEntityToken()
        {
            PlayFabAuthenticationAPI.GetEntityToken
                (new GetEntityTokenRequest(),
                (result) => _entityKey = new EntityKey() { Id = result.Entity.Id, Type = result.Entity.Type},
                (err) => print(err.GenerateErrorReport()));
        }

        #endregion

        #region Callbacks

        private void OnDisable()
        {
            CancelMatchmaking();
        }

        #endregion
    }

    //public enum 
}

