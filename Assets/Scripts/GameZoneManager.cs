using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class GameZoneManager : MonoBehaviour
{
    public static GameZoneManager Instance { get; private set; }

    [SerializeField] private GameZone _zone;
    public GameZone GetGameZone => _zone;

    private void Awake()
    {
        Instance = this;
    }

    public enum GameZone
    {
        Savezone,
        Battlezone
    }
}
