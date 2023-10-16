using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Utils;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel
{
    public class WorldModel
    {
        private Dictionary<string, object> Properties { get; set; }
        public List<Action> Actions { get; set; }
        protected IEnumerator<Action> ActionEnumerator { get; set; } 

        private Dictionary<string, float> GoalValues { get; set; } 

        public WorldModel Parent { get; set; }

        public int nextIndex = 0;

        public WorldModel(List<Action> actions)
        {
            this.Properties = new Dictionary<string, object>();
            this.GoalValues = new Dictionary<string, float>();
            this.Actions = new List<Action>(actions);
            this.Actions.Shuffle();
            this.ActionEnumerator = this.Actions.GetEnumerator();
        }

        public WorldModel(WorldModel parent)
        {
            this.Properties = new Dictionary<string, object>();
            this.GoalValues = new Dictionary<string, float>();
            this.Actions = new List<Action>(parent.Actions);
            this.Actions.Shuffle();
            this.Parent = parent;
            this.ActionEnumerator = this.Actions.GetEnumerator();
        }

        public virtual object GetProperty(string propertyName)
        {
            if (this.Properties.ContainsKey(propertyName))
            {
                return this.Properties[propertyName];
            }
            else if (this.Parent != null)
            {
                return this.Parent.GetProperty(propertyName);
            }
            else
            {
                return null;
            }

        }

        public virtual void SetProperty(string propertyName, object value)
        {
            this.Properties[propertyName] = value;
        }

        public virtual float GetGoalValue(string goalName)
        {
            //recursive implementation of WorldModel
            if (this.GoalValues.ContainsKey(goalName))
            {
                return this.GoalValues[goalName];
            }
            else if (this.Parent != null)
            {
                return this.Parent.GetGoalValue(goalName);
            }
            else
            {
                return 0;
            }
        }

        public virtual void SetGoalValue(string goalName, float value)
        {
            var limitedValue = value;
            if (value > 10.0f)
            {
                limitedValue = 10.0f;
            }

            else if (value < 0.0f)
            {
                limitedValue = 0.0f;
            }

            this.GoalValues[goalName] = limitedValue;
        }

        public virtual WorldModel GenerateChildWorldModel()
        {
            return new WorldModel(this);
        }

        public float CalculateDiscontentment(List<Goal> goals)
        {
            var discontentment = 0.0f;

            foreach (var goal in goals)
            {
                var newValue = this.GetGoalValue(goal.Name);

                discontentment += goal.GetDiscontentment(newValue);
            }

            return discontentment;
        }

        public virtual Action GetNextAction()
        {
            Action action = null;
            //returns the next action that can be executed or null if no more executable actions exist
            if (this.ActionEnumerator.MoveNext())
            {
                action = this.ActionEnumerator.Current;
            }

            while (action != null && !action.CanExecute(this))
            {
                if (this.ActionEnumerator.MoveNext())
                {
                    action = this.ActionEnumerator.Current;    
                }
                else
                {
                    action = null;
                }
            }

            return action;
        }

        public virtual Action GetNextActionButThisOneMightWork()
        {
            Action action = null;
            if (nextIndex < this.GetExecutableActions().Count())
            {
                action = this.GetExecutableActions()[nextIndex];
                nextIndex++;
            }
            return action;
        }

        public virtual Action[] GetExecutableActions()
        {
            return this.Actions.Where(a => a.CanExecute(this)).ToArray();
        }

        public virtual bool IsTerminal()
        {
            var AllChestsUsed = (int)GetProperty("Money") == 25;

            var dead = false;
            if((int) GetProperty("HP") <= 0) dead = true;

            var timeOut = false;
            if((float) GetProperty("Time") > 150f) timeOut = true;

            return AllChestsUsed || dead || timeOut;
        }
        

        public virtual float GetScore()
        {
            int money = (int)this.GetProperty("Money");
            int HP = (int)this.GetProperty("HP");
            float time = (float)this.GetProperty("Time");

            // TODO : Should Time and other factors be taken into accoun?

            // 0.0 loss , 1 win

            if (HP <= 0 || time >= 150f) 
                return 0.0f;
            else if (money == 25)
                return 1.0f;
            else 
                return 0.0f;
        }

        public virtual int GetNextPlayer()
        {
            return 0;
        }

        public virtual void CalculateNextPlayer()
        {
        }

        public Dictionary<string, object> getD()
        {
            return this.Properties;
        }

        public int getActionNumber()
        {
            return this.Actions.Count;
        }

        public IEnumerator<Action> GetEnumeratorMine()
        {
            return this.ActionEnumerator;
        }

        public List<Action> getAction()
        {
            return this.Actions;
        }
    }
}
