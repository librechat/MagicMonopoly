using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private MatchPlayerData m_MatchPlayerData;
    [SerializeField]
    private PlayerTypeTable m_PlayerTypeTable;
    
    public List<Player> Players { get { return m_Players; } }
    private List<Player> m_Players;

    public static Player CurrentPlayer { get { return s_Instance.m_Players[s_Instance.m_CurrentPlayerIndex]; } }
    private int m_CurrentPlayerIndex;

    #region Rendering and UI Callbacks
    public Action<List<Player>> onInitPlayersRenderEvent;
    public Action<List<Player>> onInitPlayersUIEvent;
    public Action<List<Player>, List<int>> onSetHomePlayersRenderEvent;
    #endregion

    private static PlayerManager s_Instance;

    #region MonoBehavior Functions
    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }
        s_Instance = this;        
    }
    #endregion

    public static void Init()
    {
        s_Instance.initPlayers(s_Instance.m_MatchPlayerData.PlayerList);
    }
    private void initPlayers(List<PlayerData> playerdatas)
    {        
        m_Players = new List<Player>();
        for (int i = 0; i < playerdatas.Count; i++)
        {
            PlayerTypeData config = m_PlayerTypeTable.GetDataById(playerdatas[i].PlayerTypeId);

            Player player = new Player();
            player.Init(playerdatas[i], config);

            m_Players.Add(player);
        }
        onInitPlayersRenderEvent(m_Players);
        onInitPlayersUIEvent(m_Players);
    }

    public static void SetPlayersAtHome(Func<Player, List<HomeGrid>, int> getHomeId)
    {
        s_Instance.setPlayersAtHome(getHomeId);
    }
    private void setPlayersAtHome(Func<Player, List<HomeGrid>, int> getHomeId)
    {
        List<HomeGrid> homeGrids = GridManager.GetHomeGrids();
        
        List<int> homeList = new List<int>();
        for (int i = 0; i < s_Instance.m_Players.Count; i++)
        {
            int home = getHomeId(m_Players[i], homeGrids);
            
            m_Players[i].Position = home;
            homeList.Add(home);
        }
        onSetHomePlayersRenderEvent(m_Players, homeList);
    }

    public static void SwitchToNextPlayer()
    {
        s_Instance.m_CurrentPlayerIndex = (s_Instance.m_CurrentPlayerIndex == s_Instance.m_Players.Count - 1) ? 0 : s_Instance.m_CurrentPlayerIndex + 1;
        return;
    }
    public static void ChangePlayer(int i)
    {
        if (i >= s_Instance.m_Players.Count)
        {
            Debug.LogError("Illegal Player Index");
            return;
        }

        s_Instance.m_CurrentPlayerIndex = i;
    }
}
