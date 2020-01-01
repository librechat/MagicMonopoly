using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField]
    private Text m_PlayerName;
    [SerializeField]
    private Text m_HPText;
    [SerializeField]
    private Text m_MPText;
    [SerializeField]
    private Text m_CoinText;
    
    public void Init(Player player)
    {
        m_PlayerName.text = player.Name;
        m_HPText.text = player.HP.ToString();
        m_MPText.text = player.MP.ToString();
        m_CoinText.text = player.Coins.ToString();

        player.onHPChangedUIEvent += onHPChanged;
        player.onMPChangedUIEvent += onMPChanged;
        player.onCoinChangedUIEvent += onCoinChanged;
        player.onBuffChangedUIEvent += onBuffChanged;
    }

    private void onHPChanged(int prehp, int hp)
    {
        m_HPText.text = hp.ToString();
    }
    private void onMPChanged(int premp, int mp)
    {
        m_MPText.text = mp.ToString();
    }
    private void onCoinChanged(int precoin, int coin)
    {
        m_CoinText.text = coin.ToString();
    }
    private void onBuffChanged(int level)
    {

    }
}
