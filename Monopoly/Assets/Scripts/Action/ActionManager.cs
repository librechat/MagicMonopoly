using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private static ActionManager s_Instance;

    [SerializeField]
    private ActionTable m_ActionTable;

    [SerializeField]
    private BuffTable m_BuffTable;
    public static BuffTable BuffTable { get { return s_Instance.m_BuffTable; } }

    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }
        s_Instance = this;
    }

    public static void InvokeAction(string actionName, Player player, int amount = -1)
    {
        s_Instance.invokeAction(actionName, player, amount);
    }

    private void invokeAction(string actionName, Player player, int amt)
    {
        ActionData action = m_ActionTable.GetDataBy((x) => { return x.Name == actionName; });

        //player.Buffs.Update(BuffData.BuffTiming.Attacked);
        bool fixedAmount = amt < 0;
        int amount = (fixedAmount) ? action.Amount: amt;

        // check buff
        amount = player.Buffs.UpdateShield(BuffData.BuffTiming.Attacked, action.Type, amount);

        switch (action.Type)
        {
            case ActionData.ActionType.Empty:
                break;
            case ActionData.ActionType.HP:
                if (action.Negative) player.HP -= amount;
                else player.HP += amount;
                break;
            case ActionData.ActionType.MP:
                if (action.Negative) player.MP -= amount;
                else player.MP += amount;
                break;
            case ActionData.ActionType.Coin:
                if (action.Negative) player.Coins -= amount;
                else player.Coins += amount;
                break;
            case ActionData.ActionType.Buff:
                if (amount > 0) player.Buffs.Add(action.BuffName, amount);
                break;
            case ActionData.ActionType.Step:
                if (fixedAmount)
                {
                    if (action.Negative) player.StepToMove /= amount;
                    else player.StepToMove *= amount;
                }
                else
                {
                    if (action.Negative) player.StepToMove -= amount;
                    else player.StepToMove += amount;
                }
                break;
            default:
                break;
        }
    }
}