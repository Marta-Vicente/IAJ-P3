using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.Action;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.RL
{
    public class Q_Learning
    {
        public float learningRate;
        public float discountRate;
        public float randomnessRate;
        public float lenghOfWalk;
        public CurrentStateWorldModel currentStateWorldModel;

        public Action lastAction;
        public WorldModel lastStateWorldModel;
        protected System.Random RandomGenerator { get; set; }

        public Q_Learning(CurrentStateWorldModel currentStateWorldModel)
        {
            learningRate = 0.1f;
            discountRate = 0.9f;
            randomnessRate = 0.5f;
            lenghOfWalk = 0;
            this.currentStateWorldModel = currentStateWorldModel;
            RandomGenerator = new System.Random();
        }

        /*
        public Q_Learning(Q_Table table, CurrentStateWorldModel currentStateWorldModel)
        {
            qTable = table;
            learningRate = 0.0f;
            discountRate = 0.0f;
            randomnessRate = 0.0f;
            lenghOfWalk = 0.0f;
            this.currentStateWorldModel = currentStateWorldModel;
            RandomGenerator = new System.Random();
        }
        */

        public Action ChooseAction()
        {

            Action[] currentActions = currentStateWorldModel.GetExecutableActions();
            Action action = null;

            WorldModel wm = new WorldModel(currentStateWorldModel.Actions);
            wm.SetAllProperties(currentStateWorldModel.GameManager);

            if(RandomGenerator.Next(100)/100 < randomnessRate)
            {
                action = currentActions[RandomGenerator.Next(currentActions.Length)];
            }
            else
            {
                action = Q_Table.GetBest(wm, currentActions);
            }

            lastAction = action;
            lastStateWorldModel = wm;
            return action;
        }

        public void ResolveAction()
        {
            if (currentStateWorldModel != null) return;
            WorldModel WmNew = new WorldModel(currentStateWorldModel.Actions);
            WmNew.SetAllProperties(currentStateWorldModel.GameManager);

            Action bestAction = Q_Table.GetBest(WmNew, currentStateWorldModel.GetExecutableActions());

            Tuple<float, WorldModel> regist = Q_Table.FindOrAdd(lastStateWorldModel, lastAction);
            Tuple<float, WorldModel> newRegist = Q_Table.FindOrAdd(WmNew, bestAction);

            float Q = regist.Item1;
            float MaxQ = newRegist.Item1;
            float reward = CalculateReward(currentStateWorldModel);

            float newQ = (1 - learningRate) * Q + learningRate * (reward + (discountRate * MaxQ));

            Q_Table.UpdateOrAdd(lastStateWorldModel, lastAction, newQ, currentStateWorldModel);
        }

        public float CalculateReward(WorldModel state)
        {
            if (!state.IsTerminal()) 
                return 0f;
            else
                return state.GetScore();
        }
    }
}
