using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public bool inMatchmaking;
    public TMP_Text btn_text;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);
        PhotonNetwork.JoinLobby();
    }

    public void OnClickStartMatchmaking()
    {
        if (inMatchmaking)
        {
            inMatchmaking = false;
            PhotonNetwork.LeaveRoom();
            btn_text.text = "Launch matchmaking";
        }
        else
        {   
            inMatchmaking = true;
            btn_text.text = "Cancel matchmaking";
            PhotonNetwork.JoinRandomRoom();
        }
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        inMatchmaking = true;
        PhotonNetwork.CreateRoom(null, new RoomOptions { EmptyRoomTtl = 0, MaxPlayers = 2 });
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //start
            photonView.RPC("StartGame", RpcTarget.All);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entered room.");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            inMatchmaking = false;
            photonView.RPC("StartGame", RpcTarget.All);
        }
    }

    // PHOTON VIEW

    [PunRPC]
    public void StartGame()
    {
        Debug.Log("Starting the game.");
        SceneManager.LoadScene("Game");
    }

    public void LeaveLobby()
    {
        inMatchmaking = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("MainMenu");
    }
}