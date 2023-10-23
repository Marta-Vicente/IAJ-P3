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
    public class Q_Table
    {

        public Dictionary<CurrentStateWorldModel, Dictionary<Action, Tuple<float, CurrentStateWorldModel>>> Table;
        public Action[] Actions;

        public Q_Table() { }

        public Q_Table(CurrentStateWorldModel State, Action[] actions)
        {
            InitTable();
            Table[State] = new Dictionary<Action, Tuple<float, CurrentStateWorldModel>>();
            Actions = actions;
        }

        public Q_Table(Dictionary<CurrentStateWorldModel, Dictionary<Action, Tuple<float, CurrentStateWorldModel>>> table, Action[] actions)
        {
            Table = table;
            Actions = actions;
        }

        public void InitTable()
        {
            Table = new Dictionary<CurrentStateWorldModel, Dictionary<Action, Tuple<float, CurrentStateWorldModel>>>();
        }

        public Tuple<float, CurrentStateWorldModel> FindOrAdd(CurrentStateWorldModel State, Action Action)
        {
            if (Table.ContainsKey(State))
            {
                if (Table[State].ContainsKey(Action)) return Table[State][Action];
                else
                {
                    Table[State].Add(Action, new Tuple<float, CurrentStateWorldModel>(0f, null));
                    return Table[State][Action];
                }
            }
            else
            {
                Table.Add(State,new Dictionary<Action, Tuple<float, CurrentStateWorldModel>>());
                return FindOrAdd(State, Action);
            }
        }

        public void UpdateOrAdd(CurrentStateWorldModel State, Action Action, float Q, CurrentStateWorldModel newState)
        {
            if (Table.ContainsKey(State))
            {
                if (Table[State].ContainsKey(Action)) 
                    Table[State][Action] = new Tuple<float, CurrentStateWorldModel>(Q, newState);
                else
                {
                    Table[State].Add(Action, new Tuple<float, CurrentStateWorldModel>(Q, newState));
                }
            }
            else
            {
                Table.Add(State, new Dictionary<Action, Tuple<float, CurrentStateWorldModel>>());
                Table[State].Add(Action, new Tuple<float, CurrentStateWorldModel>(Q, newState));
            }
        }


        public Action GetBest(CurrentStateWorldModel State, Action[] actions)
        {
            Dictionary<Action, Tuple<float, CurrentStateWorldModel>> row = Table[State];

            Action best = null;
            float bestQ = 0f;

            foreach (var action in actions)
            {
                Tuple<float, CurrentStateWorldModel> t = this.FindOrAdd(State, action);
                if (t.Item1 >= bestQ)
                {
                    bestQ = t.Item1;
                    best = action;
                }
            }

            return best;
        }



    }
}
