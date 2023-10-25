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
    public static class Q_Table
    {

        public static Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>> Table 
            = new Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>>(new Q_Dictionary_Equals());


        //Swap for change
        /*
        public Q_Table(Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>> table)
        {
            Table = table;
        }
        */

        /*
        public static void InitTable()
        {
            Table = new Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>>();
        }
        */

        public static Tuple<float, WorldModel> FindOrAdd(WorldModel State, Action Action)
        {
            if (State == null || Action == null) return null;
            if (Table.ContainsKey(State))
            {
                if (Table[State].ContainsKey(Action)) return Table[State][Action];
                else
                {
                    Table[State].Add(Action, new Tuple<float, WorldModel>(0f, null));
                    return Table[State][Action];
                }
            }
            else
            {
                Table.Add(State,new Dictionary<Action, Tuple<float, WorldModel>>(new Q_Dictionary_Equals_Actions()));
                if (Table[State].ContainsKey(Action)) return Table[State][Action];
                else
                {
                    Table[State].Add(Action, new Tuple<float, WorldModel>(0f, null));
                    return Table[State][Action];
                }
            }
        }

        public static void UpdateOrAdd(WorldModel State, Action Action, float Q, WorldModel newState)
        {
            if(State == null || Action == null) return;
            if (Table.ContainsKey(State))
            {
                if (Table[State].ContainsKey(Action)) 
                    Table[State][Action] = new Tuple<float, WorldModel>(Q, newState);
                else
                {
                    Table[State].Add(Action, new Tuple<float, WorldModel>(Q, newState));
                }
            }
            else
            {
                Table.Add(State, new Dictionary<Action, Tuple<float, WorldModel>>(new Q_Dictionary_Equals_Actions()));
                Table[State].Add(Action, new Tuple<float, WorldModel>(Q, newState));
            }
        }


        public static Action GetBest(WorldModel State, Action[] actions)
        {
            if(actions == null) return null;
            Action best = null;
            float bestQ = float.MinValue;

            foreach (var action in actions)
            {
                Tuple<float, WorldModel> t = FindOrAdd(State, action);
                if (t.Item1 >= bestQ)
                {
                    bestQ = t.Item1;
                    best = action;
                }
            }

            return best;
        }


        public static Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>> ShowInDegug()
        {
            return Table;
        }


    }
}
