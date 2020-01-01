using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player
{

    public int PlayerId { get { return m_PlayerId; } }
    private int m_PlayerId;
    public string Name { get { return m_Name; } }
    private string m_Name;
    public int Group { get { return m_Group; } }
    private int m_Group;
    public Color Color { get { return m_Color; } }
    private Color m_Color;

    public int Position { get { return m_Position; } set { m_Position = value; } }
    private int m_Position; // grid index

    public int HP { 
        get { return m_HP; } 
        set {
            int previousHP = m_HP;

            if (value > m_MaxHP) m_HP = m_MaxHP;
            else if (value < 0) m_HP = 0; // dead
            else m_HP = value;

            onHPChangedUIEvent(previousHP, m_HP);
            onHPChangedRenderEvent(previousHP, value);
        }
    }
    public int MaxHP
    {
        get { return m_MaxHP; }
        set
        {
            m_MaxHP = value;
            // onMaxHPChangedUIEvent(m_MaxHP);
        }
    }
    private int m_HP;    
    private int m_MaxHP;

    public int MP { 
        get { return m_MP; } 
        set {
            int previousMP = m_MP;

            if (value > m_MaxMP) m_MP = m_MaxMP;
            else if (value < 0) m_MP = 0; // should be impossible
            else m_MP = value;

            onMPChangedUIEvent(previousMP, m_MP);
            onMPChangedRenderEvent(previousMP, value);
        }
    }
    public int MaxMP
    {
        get { return m_MaxMP; }
        set
        {
            m_MaxMP = value;
            // onMaxMPChangedUIEvent(m_MaxMP);
        }
    }
    private int m_MP;
    private int m_MaxMP;

    public int Coins { 
        get { return m_Coins; }
        set {
            int previousCoins = m_Coins;
            
            if (value < 0) m_Coins = 0; // should be impossible, or invoke events
            else m_Coins = value;

            onCoinChangedUIEvent(previousCoins, m_Coins);
            onCoinChangedRenderEvent(previousCoins, value);
        } 
    }
    private int m_Coins;
    
    public SpellBook SpellBook { get { return m_SpellBook; } }
    private SpellBook m_SpellBook;

    private List<CircleBase> m_OwnedCircles;

    public BuffPack Buffs { get { return m_Buffs; } }
    private BuffPack m_Buffs;

    public int StepToMove {
        get { return m_StepToMove; }
        set { m_StepToMove = value; }
    }
    private int m_StepToMove = 0;

    #region Render and UI Event
    public Action<int, int, int, int> onMovePlayerRenderEvent;
    public Action<int, int> onHPChangedRenderEvent;
    public Action<int, int> onMPChangedRenderEvent;
    public Action<int, int> onCoinChangedRenderEvent;
    public Action onBuffChangedRenderEvent;

    public Action<int, int> onHPChangedUIEvent;
    public Action<int, int> onMPChangedUIEvent;
    public Action<int, int> onCoinChangedUIEvent;
    public Action<int> onBuffChangedUIEvent;
    #endregion

    public void Init(PlayerData data, PlayerTypeData config)
    {
        m_PlayerId = data.Id;
        m_Group = data.Group;
        m_Name = data.Name;
        m_Color = ColorTable.PlayerColors[data.ColorId];

        m_MaxHP = config.MaxHP;
        m_HP = m_MaxHP;

        m_MaxMP = config.MaxMP;
        m_MP = m_MaxMP;

        m_Coins = config.Coin;

        m_SpellBook = new SpellBook();
        m_SpellBook.Init();
        m_Buffs = new BuffPack();
        m_Buffs.Init(this);

        m_OwnedCircles = new List<CircleBase>();
    }

    public bool Move(Action completeCallback, Action<int> failCallback, int branch = -1)
    {
        int originPos = m_Position;
        int targetPos = m_Position;
        GridBase currentGrid = GridManager.GetGrid(m_Position);

        if (branch != -1)
        {
            targetPos = currentGrid.GetNextGridIndex(branch);

            m_Position = targetPos;
            currentGrid = GridManager.GetGrid(m_Position);

            m_Buffs.Update(BuffData.BuffTiming.MoveStep);
            m_StepToMove--;

            onMovePlayerRenderEvent(m_PlayerId, m_StepToMove, originPos, targetPos);
            currentGrid.OnPlayerPassed(this);
        }

        int branchCount = 1;
        while (m_StepToMove > 0)
        {
            if (currentGrid.BranchCount > 1)
            {
                break;
            }

            originPos = m_Position;
            targetPos = currentGrid.GetNextGridIndex();

            m_Position = targetPos;
            currentGrid = GridManager.GetGrid(m_Position);

            m_Buffs.Update(BuffData.BuffTiming.MoveStep);
            m_StepToMove--;

            onMovePlayerRenderEvent(m_PlayerId, m_StepToMove, originPos, targetPos);
            currentGrid.OnPlayerPassed(this);            
        }

        if (m_StepToMove > 0)
        {
            failCallback(branchCount);
            return false;
        }
        else
        {
            currentGrid.OnPlayerStepped(this);
            completeCallback();
            return true;
        }
    }

    public void OwnCircles(CircleBase circle)
    {
        m_OwnedCircles.Add(circle);
    }
    public void RemoveCircles(CircleBase circle)
    {
        m_OwnedCircles.Remove(circle);
    }
}
