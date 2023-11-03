using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.Action;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using Assets.Scripts.IAJ.Unity.DecisionMaking.RL;

public class Q_Learning
{
    public float learningRate;
    public float discountRate;
    public float randomnessRate;
    public float lenghOfWalk;
    public List<Action> allActions;
    public CurrentStateWorldModel currentStateWorldModel;

    public Action lastAction;
    public WorldModel lastStateWorldModel;
    protected System.Random RandomGenerator { get; set; }

    public bool Doing = false;

    public Q_Learning(CurrentStateWorldModel currentStateWorldModel, List<Action> allActions)
    {
        learningRate = 0.5f;
        discountRate = 0.9f;
        randomnessRate = 0.00f;
        lenghOfWalk = 0;
        this.currentStateWorldModel = currentStateWorldModel;
        RandomGenerator = new System.Random();
        this.allActions = allActions;
    }

    public Action ChooseAction()
    {
        Doing = true;
        Action[] currentActions = currentStateWorldModel.GetExecutableActions();
        Action action = null;

        WorldModel wm = new WorldModel(currentStateWorldModel.Actions);
        wm.SetAllProperties(currentStateWorldModel.GameManager);

        if((float)RandomGenerator.Next(100)/100 < randomnessRate)
        {
            action = currentActions[RandomGenerator.Next(currentActions.Length)];
        }
        else
        {
            action = Q_Table_Instance.Table.GetBest(wm, currentActions);
        }

        lastAction = action;
        lastStateWorldModel = wm;

        foreach(var a in allActions)
        {
            if (a.Equals(lastAction)) return a;
        }

        return null;
    }

    public void ResolveAction()
    {
        if (currentStateWorldModel == null || lastStateWorldModel == null) return;
        WorldModel WmNew = new WorldModel(currentStateWorldModel.Actions);
        WmNew.SetAllProperties(currentStateWorldModel.GameManager);

        Action bestAction = Q_Table_Instance.Table.GetBest(WmNew, currentStateWorldModel.GetExecutableActions());

        Tuple<float, float> regist = Q_Table_Instance.Table.FindOrAdd(lastStateWorldModel, lastAction);
        Tuple<float, float> newRegist = Q_Table_Instance.Table.FindOrAdd(WmNew, bestAction);

        float Q = regist.Item1;
        float MaxQ = newRegist.Item1;
        float reward = CalculateReward(currentStateWorldModel);

        float newQ = (1 - learningRate) * Q + learningRate * (reward + (discountRate * MaxQ));

        Q_Table_Instance.Table.UpdateOrAdd(lastStateWorldModel, lastAction, newQ, currentStateWorldModel);

        lastStateWorldModel = null;
        Doing = false;
    }

    public float CalculateReward(WorldModel state)
    {
        if (!state.IsTerminal()) 
            return 0f;
        else
            return state.GetScore();
    }
}


