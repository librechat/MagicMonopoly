using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MatchUIManager : MonoBehaviour
{    
    private Dictionary<MatchManager.MatchState, MatchStageUIBase> m_StageUIManager;

    [SerializeField]
    private RectTransform playerInfoPanel;
    [SerializeField]
    private RectTransform playerInfoPrefab;
    private List<PlayerInfoUI> m_PlayerInfos;
    
    void Awake()
    {
        // Init the UIManagers
        m_StageUIManager = new Dictionary<MatchManager.MatchState, MatchStageUIBase>();

        m_StageUIManager.Add(MatchManager.MatchState.Prepare, GetComponent<PrepareUI>());
        m_StageUIManager.Add(MatchManager.MatchState.PreStage, GetComponent<PreStageUI>());
        m_StageUIManager.Add(MatchManager.MatchState.MoveStage, GetComponent<MoveStageUI>());

        m_StageUIManager.Add(MatchManager.MatchState.IdleGridEvent, GetComponent<IdleGridEventUI>());
        m_StageUIManager.Add(MatchManager.MatchState.BasicGridEvent, GetComponent<BasicGridEventUI>());
        m_StageUIManager.Add(MatchManager.MatchState.ShopGridEvent, GetComponent<ShopGridEventUI>());

        m_StageUIManager.Add(MatchManager.MatchState.PostStage, GetComponent<PostStageUI>());
        m_StageUIManager.Add(MatchManager.MatchState.Result, GetComponent<ResultUI>());
    }

    public void Init(MatchManager matchManager, PlayerManager playerManager)
    {        
        Dictionary<MatchManager.MatchState, StageControllerBase> controllers = matchManager.StateControllers;

        List<MatchManager.MatchState> states = MatchManager.StateList;

        for (int i = 0; i < states.Count; i++)
        {
            m_StageUIManager[states[i]].Init(controllers[states[i]]);
        }

        playerManager.onInitPlayersUIEvent += initPlayersUI;
    }

    private void initPlayersUI(List<Player> players)
    {
        m_PlayerInfos = new List<PlayerInfoUI>();
        for (int i = 0; i < players.Count; i++)
        {
            // init player info panel
            PlayerInfoUI playerInfo = Instantiate(playerInfoPrefab, playerInfoPanel).GetComponent<PlayerInfoUI>();
            playerInfo.Init(players[i]);

            m_PlayerInfos.Add(playerInfo);
        }
    }
}
