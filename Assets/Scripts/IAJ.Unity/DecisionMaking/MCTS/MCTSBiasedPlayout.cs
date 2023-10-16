using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.Action;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using Assets.Scripts.IAJ.Unity.Utils;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSBiasedPlayout : MCTS
    {

        public MCTSBiasedPlayout(CurrentStateWorldModel currentStateWorldModel) : base(currentStateWorldModel)
        {
            this.MaxIterations = 10000;
            this.MaxPlayoutsPerNode = 10;
        }

        // Selection and Expantion
        protected override float Playout(WorldModel initialStateForPlayout)
        {
            Action[] executableActions;

            var currentDepth = 0;

            while (!initialStateForPlayout.IsTerminal())
            {
                executableActions = initialStateForPlayout.GetExecutableActions();
                var actionPicked = GetBestActionH(executableActions, initialStateForPlayout);
                actionPicked.ApplyActionEffects(initialStateForPlayout);
                currentDepth++;
            }
            if (currentDepth > this.MaxPlayoutDepthReached) this.MaxPlayoutDepthReached = currentDepth;

            return initialStateForPlayout.GetScore();
        }
        


        private Action GetBestActionH(Action[] actions, WorldModel worldModel)
        {
            var largertH = 0f;
            List<Action> biasActions = new List<Action>();
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].GetHValue(worldModel) > largertH) largertH = actions[i].GetHValue(worldModel);
            }

            List<Pair<Action, int>> bestActionsPair = new List<Pair<Action, int>>();
            for (int i = 0; i < actions.Length; i++)
            {
                var H = actions[i].GetHValue(worldModel);
                if (H <= 0f) H = 0.001f;
                int biasValue = (int)(largertH / H);

                //Everyone gets a chance
                if(biasValue > 10) biasValue = 10;
                else if(biasValue < 2) biasValue = 0;

                bestActionsPair.Add(new Pair<Action, int>(actions[i], biasValue));
            }

            foreach(var pair in bestActionsPair)
            {
                for (int i = 0;i < pair.Right; i++)
                {
                    biasActions.Add(pair.Left);
                }
            }

            if (biasActions.Count == 0) return actions[RandomGenerator.Next(actions.Length)];

            var ActionNumber = RandomGenerator.Next(biasActions.Count);

            return biasActions[ActionNumber];
        }
        
        
        /*       
        private Action GetBestActionH(Action[] actions, WorldModel worldModel)
        {
            var bestH = 1000f;
            if (actions[0] == null) return null;

            
            var bestAction = actions[0];
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].GetHValue(worldModel) < bestH)
                {
                    bestH = actions[i].GetHValue(worldModel);
                    bestAction = actions[i];
                }  
            }    

            return bestAction;
        }
        */
    }
}
