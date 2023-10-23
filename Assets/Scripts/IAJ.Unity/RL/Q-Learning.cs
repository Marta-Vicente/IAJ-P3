﻿using Assets.Scripts.Game;
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
        public Q_Table qTable;
        public Action[] Actions;
        public float learningRate;
        public float discountRate;
        public float randomnessRate;
        public float lenghOfWalk;
        public CurrentStateWorldModel currentStateWorldModel;

        public Action lastAction;
        public CurrentStateWorldModel lastStateWorldModel;
        protected System.Random RandomGenerator { get; set; }

        public Q_Learning(Action[] actions, CurrentStateWorldModel currentStateWorldModel)
        {
            Actions = actions;
            qTable = new Q_Table(currentStateWorldModel, actions);
            learningRate = 0.1f;
            discountRate = 0.9f;
            randomnessRate = 0.5f;
            lenghOfWalk = 0;
            this.currentStateWorldModel = currentStateWorldModel;
            RandomGenerator = new System.Random();
        }

        public Q_Learning(Action[] actions, Q_Table table, CurrentStateWorldModel currentStateWorldModel)
        {
            Actions = actions;
            qTable = table;
            learningRate = 0.0f;
            discountRate = 0.0f;
            randomnessRate = 0.0f;
            lenghOfWalk = 0.0f;
            this.currentStateWorldModel = currentStateWorldModel;
            RandomGenerator = new System.Random();
        }

        public Action ChooseAction()
        {
            Action[] currentActions = currentStateWorldModel.GetExecutableActions();
            Action action = null;

            if(RandomGenerator.Next(100)/100 < randomnessRate)
            {
                action = currentActions[RandomGenerator.Next(currentActions.Length)];
            }
            else
            {
                action = qTable.GetBest(currentStateWorldModel, currentActions);
            }

            lastAction = action;
            lastStateWorldModel = currentStateWorldModel;
            return action;
        }

        public void ResolveAction()
        {
            Tuple<float, CurrentStateWorldModel> regist = qTable.FindOrAdd(lastStateWorldModel, lastAction);
            Tuple<float, CurrentStateWorldModel> newRegist = qTable.FindOrAdd(currentStateWorldModel, 
                            qTable.GetBest(currentStateWorldModel, currentStateWorldModel.GetExecutableActions()));

            float Q = regist.Item1;
            float MaxQ = newRegist.Item1;
            float reward = 0f;

            float newQ = (1 - learningRate) * Q + learningRate * (reward + (discountRate * MaxQ));

            qTable.UpdateOrAdd(lastStateWorldModel, lastAction, newQ, currentStateWorldModel);
        }
    }
}
