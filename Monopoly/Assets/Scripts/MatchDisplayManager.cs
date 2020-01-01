using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MatchDisplayManager : MonoBehaviour
{
    [SerializeField]
    private MatchManager m_MatchManager;
    [SerializeField]
    private MatchUIManager m_MatchUIManager;
    [SerializeField]
    private MatchRenderer m_MatchRenderer;
    
    private static MatchDisplayManager s_Instance;
    private Queue<Action<Action>> m_DisplayActionQueue;

    void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }
        s_Instance = this;
        m_DisplayActionQueue = new Queue<Action<Action>>();

        m_MatchManager.onInitDisplay += Init;
    }

    public void Init(MatchManager matchManager, PlayerManager playerManager, GridManager gridManager)
    {
        m_MatchUIManager.Init(matchManager, playerManager);
        m_MatchRenderer.Init(matchManager, playerManager, gridManager);
    }

    public static void InvokeAction(Action action)
    {
        action();
    }
    public static void EnqueueAction(Action<Action> action)
    {
        s_Instance.m_DisplayActionQueue.Enqueue(action);
        if (s_Instance.m_DisplayActionQueue.Count == 1) s_Instance.dequeueAction();
    }

    private void dequeueAction()
    {
        if (m_DisplayActionQueue.Count > 0)
        {
            Action<Action> action = m_DisplayActionQueue.Peek();
            action(() =>
            {
                m_DisplayActionQueue.Dequeue();
                dequeueAction();
            });
        }
        else return;
    }
}
