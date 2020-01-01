using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchRenderer : MonoBehaviour
{
    private PlayerRendererManager m_PlayerRendererManager;
    private GridRendererManager m_GridRendererManager;

    public void Init(MatchManager matchManager, PlayerManager playerManager, GridManager gridManager)
    {
        m_PlayerRendererManager = GetComponent<PlayerRendererManager>();
        m_GridRendererManager = GetComponent<GridRendererManager>();

        m_PlayerRendererManager.Init(playerManager);
        m_GridRendererManager.Init(gridManager);   
    }
}
