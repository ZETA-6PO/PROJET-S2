using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomName;

    private LobbyManager _lobbyManager;

    private void Start()
    {
        _lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _roomName)
    {
        this.roomName.text = _roomName;
    }

    

    public void OnClickItem()
    {
        _lobbyManager.JoinRoom(roomName.text);
    }
}
