using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInput;

    public GameObject lobbyPanel;

    public GameObject roomPanel;

    public TMP_Text roomName;

    public RoomItem prefabRoomItem;

    private List<RoomItem> roomItemsList = new List<RoomItem>();

    public Transform contentObject;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil((() => PhotonNetwork.IsConnected));
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if (roomInput.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions()
            {
                MaxPlayers = 2,
            });
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //start
            photonView.RPC("StartGame", RpcTarget.All);
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = $"Room name : {PhotonNetwork.CurrentRoom.Name}";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //Debug.Log(roomList[0]!.Name);
        UpdateRoomList(roomList);
    }
    
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void OnConnectedToServer()
    {
        PhotonNetwork.JoinLobby();
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();
        foreach (RoomInfo room in roomList)
        {
            RoomItem newRoom = Instantiate(prefabRoomItem, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }
    
    //PHOTON VIEW

    [PunRPC]
    public void StartGame()
    {
        Debug.Log("Starting the game.");
        SceneManager.LoadScene("Game");
    }



}
