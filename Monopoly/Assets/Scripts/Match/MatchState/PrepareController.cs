using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrepareController : StageControllerBase
{
    public PrepareController(FiniteStateMachine<MatchManager.MatchState> fsm)
        : base(fsm)
    {


    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update(float timeStep)
    {
        GridManager.Init();
        PlayerManager.Init();

        PlayerManager.SetPlayersAtHome((Player player, List<HomeGrid> homeGrids) =>
        {
            return homeGrids[0].GridId;
        });

        // use different home select methods by different match rules

        /*
        PlayerManager.SetPlayersAtHome((Player player, List<HomeGrid> homeGrids) =>
        {
            return homeGrids.Find( x => x.Group == player.Group).GridId;
        });
        PlayerManager.SetPlayersAtHome((Player player, List<HomeGrid> homeGrids) =>
        {
            int h = UnityEngine.Random.Range(0, homeGrids.Count);
            int home = homeGrids[h].GridId;
            homeGrids.RemoveAt(h);
            return h;
        });*/
        
        parentFSM.IssueCommand("Prepare-PreStage");
        base.Update(timeStep);
    }
}