using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Utils;
using static UnityEngine.GraphicsBuffer;
using Assets.Scripts.Game;
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel
{
    [Serializable]
    public class WorldModel /*:ISerializable*/
    {
        public Dictionary<string, object> Properties { get; set; }

        [JsonIgnore]
        public List<Action> Actions { get; set; }

        [JsonIgnore]
        protected IEnumerator<Action> ActionEnumerator { get; set; }

        [JsonIgnore]
        private Dictionary<string, float> GoalValues { get; set; }

        [JsonIgnore]
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
        
        /*
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
        */

        public virtual float GetScore()
        {
            int money = (int)this.GetProperty(Game.Properties.MONEY);
            int HP = (int)this.GetProperty(Game.Properties.HP);
            float time = (float)this.GetProperty(Game.Properties.TIME);

            if (HP <= 0 || time >= 150f) return -1.0f;
            else if (money == 25)
            {
                return 1.0f;
            }
            else return 0.0f;
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

        /*
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            try
            {
                WorldModel f = (WorldModel)obj;

                if ((int)f.GetProperty(Game.Properties.HP) != (int)this.GetProperty(Game.Properties.HP)) return false;
                if ((int)f.GetProperty(Game.Properties.MANA) != (int)this.GetProperty(Game.Properties.MANA)) return false;
                if ((int)f.GetProperty(Game.Properties.ShieldHP) != (int)this.GetProperty(Game.Properties.ShieldHP)) return false;
                if ((int)f.GetProperty(Game.Properties.XP) != (int)this.GetProperty(Game.Properties.XP)) return false;
                if ((int)f.GetProperty(Game.Properties.LEVEL) != (int)this.GetProperty(Game.Properties.LEVEL)) return false;
                if ((float)f.GetProperty(Game.Properties.TIME) != (float)this.GetProperty(Game.Properties.TIME)) return false;

                float threshold = 1f;
                float value1 = (float)f.GetProperty(Game.Properties.TIME);
                float value2 = (float)this.GetProperty(Game.Properties.TIME);

                if (Math.Abs(value1 - value2) > threshold)
                {
                    return false;
                }


                if ((int)f.GetProperty(Game.Properties.MONEY) != (int)this.GetProperty(Game.Properties.MONEY)) return false;
                if (Vector3.Distance((Vector3)f.GetProperty(Game.Properties.POSITION), (Vector3)this.GetProperty(Game.Properties.POSITION)) > 1f) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        */

        public void SetAllProperties(GameManager gm)
        {
            SetProperty(Game.Properties.MANA, gm.Character.baseStats.Mana);
            SetProperty(Game.Properties.MAXHP, gm.Character.baseStats.MaxHP);
            SetProperty(Game.Properties.HP, gm.Character.baseStats.HP);
            SetProperty(Game.Properties.ShieldHP, gm.Character.baseStats.ShieldHP);
            SetProperty(Game.Properties.MONEY, gm.Character.baseStats.Money);
            SetProperty(Game.Properties.TIME, gm.Character.baseStats.Time);
            SetProperty(Game.Properties.LEVEL, gm.Character.baseStats.Level);
            SetProperty(Game.Properties.MAXMANA, gm.Character.baseStats.MaxMana);
            SetProperty(Game.Properties.POSITION, gm.Character.gameObject.transform.position);
            SetProperty(Game.Properties.XP, gm.Character.baseStats.XP);

        }

        public int EvaluateHP()
        {
            int HP = (int)this.GetProperty(Game.Properties.HP);

            if (HP <= 2)
                return 0;
            else if (2 < HP && HP <= 6)
                return 1;
            else if (6 < HP && HP <= 12)
                return 2;
            else
                return 3;
        }

        public int EvaluateMana()
        {
            int mana = (int)this.GetProperty(Game.Properties.MANA);

            if (0 <= mana && mana <= 2)
                return 0;
            else if (2 < mana && mana <= 5)
                return 1;
            else
                return 2;
        }

        public int EvaluateShieldHP()
        {
            int shieldHP = (int)this.GetProperty(Game.Properties.ShieldHP);

            if (0 <= shieldHP && shieldHP <= 2)
                return 0;
            else
                return 1;
        }

        public int EvaluateXP()
        {
            int xp = (int)this.GetProperty(Game.Properties.XP);

            if (0 <= xp && xp <= 50)
                return 0;
            else if (50 < xp && xp <= 300)
                return 1;
            else if (300 < xp && xp <= 700)
                return 2;
            else
                return 3;
        }

        public int EvaluateLevel()
        {
            int lvl = (int)this.GetProperty(Game.Properties.LEVEL);

            if (lvl == 1)
                return 0;
            else
                return 1;
        }

        public int EvaluateTime()
        {
            float time = (float)this.GetProperty(Game.Properties.TIME);

            if (0 <= time && time <= 15)
                return 0;
            else if (15 < time && time <= 30)
                return 1;
            else if (30 < time && time <= 45)
                return 2;
            else if (45 < time && time <= 60)
                return 3;
            else if (60 < time && time <= 75)
                return 4;
            else if (75 < time && time <= 90)
                return 5;
            else if (90 < time && time <= 105)
                return 6;
            else if (105 < time && time <= 120)
                return 7;
            else if (120 < time && time <= 135)
                return 8;
            else
                return 9;
        }

        public int EvaluateMoney()
        {
            int money = (int)this.GetProperty(Game.Properties.MONEY);

            if (money == 0)
                return 0;
            else if (money == 5)
                return 1;
            else if (money == 10)
                return 2;
            else if (money == 15)
                return 3;
            else if (money == 20)
                return 4;
            else
                return 5;
        }

        public int EvaluatePosition()
        {
            Vector3 pos = (Vector3)this.GetProperty(Game.Properties.POSITION);

            if (pos.x < 33 && pos.y < 33)
                return 0;
            else if (33 <= pos.x && pos.x < 66 && pos.y < 33)
                return 1;
            else if (66 <= pos.x && pos.y < 33)
                return 2;

            else if (pos.x < 33 && 33 <= pos.y && pos.y < 66)
                return 3;
            else if (33 <= pos.x && pos.x < 66 && 33 <= pos.y && pos.y < 66)
                return 4;
            else if (66 <= pos.x && 33 <= pos.y && pos.y < 66)
                return 5;

            else if (pos.x < 33 && 66 <= pos.y)
                return 6;
            else if (33 <= pos.x && pos.x < 66 && 66 <= pos.y)
                return 7;
            else if (66 <= pos.x && 66 <= pos.y)
                return 8;
            else
                return 9;

        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            try
            {
                WorldModel f = (WorldModel)obj;

                if (f.EvaluateHP() != this.EvaluateHP()) return false;
                if (f.EvaluateMana() != this.EvaluateMana()) return false;
                if (f.EvaluateShieldHP() != this.EvaluateShieldHP()) return false;
                if (f.EvaluateXP() != this.EvaluateXP()) return false;
                if (f.EvaluateLevel() != this.EvaluateLevel()) return false;
                if (f.EvaluateTime() != this.EvaluateTime()) return false;
                if (f.EvaluateMoney() != this.EvaluateMoney()) return false;
                if (f.EvaluatePosition() != this.EvaluatePosition()) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /*
        protected WorldModel(SerializationInfo info, StreamingContext context)
        {
            Properties = (Dictionary<string, object>)info.GetValue("Properties", typeof(Dictionary<string, object>));
            Actions = (List<Action>)info.GetValue("Actions", typeof(List<Action>));
            nextIndex = (int)info.GetValue("nextIndex", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Properties", Properties, typeof(Dictionary<string, object>));
            info.AddValue("Actions", Actions, typeof(List<Action>));
            info.AddValue("nextIndex", nextIndex, typeof(int));
        }
        */
    }
}
