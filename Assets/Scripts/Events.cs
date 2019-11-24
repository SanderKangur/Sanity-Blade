using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    #region Start
    public static event Action<PlayerInfo> OnStartRoom;
    public static void StartRoom(PlayerInfo data) => OnStartRoom?.Invoke(data);
    #endregion


    #region Lives
    public static event Action<int> OnSetLives;
    public static void SetLives(int amount) => OnSetLives?.Invoke(amount);
    #endregion
}
