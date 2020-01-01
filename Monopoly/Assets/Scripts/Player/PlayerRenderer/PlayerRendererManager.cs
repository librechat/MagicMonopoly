using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRendererManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_PlayerPrefab;
    
    private GridRendererManager m_GridRendererManager;
    private List<Transform> m_Players;

    public void Awake()
    {
        m_GridRendererManager = GetComponent<GridRendererManager>();
    }

    public void Init(PlayerManager manager)
    {
        manager.onInitPlayersRenderEvent += initPlayers;
        manager.onSetHomePlayersRenderEvent += setHomePlayers;
    }

    private void initPlayers(List<Player> playerList)
    {
        m_Players = new List<Transform>();
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].onMovePlayerRenderEvent += movePlayer;

            Transform playerTransform = Instantiate(m_PlayerPrefab);
            m_Players.Add(playerTransform);

            // todo: set up skins or colors
            PlayerRenderer player = m_Players[i].GetComponent<PlayerRenderer>();
            player.Init(playerList[i]);
        }
    }

    private void setHomePlayers(List<Player> players, List<int> posList)
    {
        MatchDisplayManager.InvokeAction(() =>
        {
            List<Vector3> renderPosList = m_GridRendererManager.GetGridsPos(posList);

            for (int i = 0; i < renderPosList.Count; i++)
            {
                m_Players[i].position = renderPosList[i];
            }
            List<Vector3> posListNoDupe = renderPosList.Distinct().ToList();
            for (int i = 0; i < posListNoDupe.Count; i++) alignPlayersAt(posListNoDupe[i]);
        });
    }

    private void movePlayer(int player, int remainStep, int origin, int target)
    {
        Vector3 originPos = m_GridRendererManager.GetGridPos(origin);
        Vector3 targetPos = m_GridRendererManager.GetGridPos(target);

        MatchDisplayManager.EnqueueAction((Action callback) =>
        {
            if (remainStep == 0) m_GridRendererManager.SteppedGrid(target);
            else m_GridRendererManager.PassedGrid(target);
            
            IEnumerator moveCoroutine = movePlayerCoroutine(player, originPos, targetPos, callback);
            StartCoroutine(moveCoroutine);
        });
    }

    private void alignPlayersAt(Vector3 pos)
    {
        List<int> players = getPlayersAt(pos);
        List<Vector3> alignedPos = getAlignedPosAt(pos, players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            m_Players[players[i]].position = alignedPos[i];
        }        
    }
    private List<int> getPlayersAt(Vector3 pos)
    {
        List<int> players = new List<int>();
        for (int i = 0; i < m_Players.Count; i++)
        {
            if (Vector3.Distance(m_Players[i].position, pos) < 0.5f)
            {
                players.Add(i);
            }
        }
        return players;
    }
    private List<Vector3> getAlignedPosAt(Vector3 pos, int numberOfPlayers)
    {
        float width = 1.0f / numberOfPlayers;
        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            float w = (i + 1) * width - width / 2.0f - 0.5f;
            posList.Add(pos + new Vector3(0, w, 0));
        }
        return posList;
    }

    private IEnumerator movePlayerCoroutine(int player, Vector3 originPos, Vector3 targetPos, Action callback)
    {        
        List<int> playersAtOrigin = getPlayersAt(originPos);
        playersAtOrigin.Remove(player);
        List<Vector3> originAlignedOffset = getAlignedPosAt(originPos, playersAtOrigin.Count);
        
        List<int> playersAtTarget = getPlayersAt(targetPos);
        playersAtTarget.Add(player);
        List<Vector3> targetAlignedOffset = getAlignedPosAt(targetPos, playersAtTarget.Count);

        List<int> movingPlayers = playersAtOrigin.Concat(playersAtTarget).ToList();
        List<Vector3> targetList = originAlignedOffset.Concat(targetAlignedOffset).ToList();
        List<Vector3> originList = new List<Vector3>();
        for (int i = 0; i < movingPlayers.Count; i++) originList.Add(m_Players[movingPlayers[i]].position);
        
        float timeMax = 0.2f;
        IEnumerator transition = AnimTransition.Transition(EasingFunction.Ease.EaseInOutQuad, timeMax,
            (float ratio) =>
            {
                for (int p = 0; p < movingPlayers.Count; p++)
                {
                    m_Players[movingPlayers[p]].position = Vector3.Lerp(originList[p], targetList[p], ratio);
                }
            }
        );
        yield return StartCoroutine(transition);

        for (int p = 0; p < playersAtTarget.Count; p++)
        {
            m_Players[movingPlayers[p]].position = targetList[p];
        }
        yield return new WaitForSeconds(0.05f);

        callback();
        yield return null;
    }
}
