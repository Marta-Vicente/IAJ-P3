using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Utils;
using static UnityEngine.GraphicsBuffer;
using Assets.Scripts.Game;
using Assets.Scripts.Game.NPCs;
using System;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel
{
    public class WorldModelFEAR
    {
        // time + hp + money + postion + mana + shield + maxhp + xp + level + maxmana
        // + 3orcs + 5ske + 1drake + 5 chests + 2 h potions + 2 mana potions
        private object[] PropertiesBase = new object[10];
        private object[] PropertiesOrcs = new object[3];
        private object[] PropertiesSke = new object[5];
        private object[] PropertiesDrake = new object[1];
        private object[] PropertiesChest = new object[5];
        private object[] PropertiesHealthPot = new object[2];
        private object[] PropertiesManaPot = new object[2];

        private List<Action> Actions { get; set; }
        protected IEnumerator<Action> ActionEnumerator { get; set; } 

        private float[] GoalValues = new float[4];

        public int nextIndex = 0;

        public WorldModelFEAR(WorldModel parent)
        {
            PropertiesBase[0] = (float) parent.GetProperty(Properties.TIME);
            PropertiesBase[1] = (int)parent.GetProperty(Properties.HP);
            PropertiesBase[2] = (int)parent.GetProperty(Properties.MONEY);
            PropertiesBase[3] = (Vector3)parent.GetProperty(Properties.POSITION);
            PropertiesBase[4] = (int)parent.GetProperty(Properties.MANA);
            PropertiesBase[5] = (int)parent.GetProperty(Properties.ShieldHP);
            PropertiesBase[6] = (int)parent.GetProperty(Properties.MAXHP);
            PropertiesBase[7] = (int)parent.GetProperty(Properties.XP);
            PropertiesBase[8] = (int)parent.GetProperty(Properties.LEVEL);
            PropertiesBase[9] = (int)parent.GetProperty(Properties.MAXMANA);

            PropertiesOrcs[0] = (bool)parent.GetProperty("Orc0");
            PropertiesOrcs[1] = (bool)parent.GetProperty("Orc1");
            PropertiesOrcs[2] = (bool)parent.GetProperty("Orc2");

            PropertiesSke[0] = (bool)parent.GetProperty("Skelleton0");
            PropertiesSke[1] = (bool)parent.GetProperty("Skelleton1");
            PropertiesSke[2] = (bool)parent.GetProperty("Skelleton2");
            PropertiesSke[3] = (bool)parent.GetProperty("Skelleton3");
            PropertiesSke[4] = (bool)parent.GetProperty("Skelleton4");

            PropertiesDrake[0] = (bool)parent.GetProperty("Mighty Dragon");

            PropertiesChest[0] = (bool)parent.GetProperty("Chest0");
            PropertiesChest[1] = (bool)parent.GetProperty("Chest1");
            PropertiesChest[2] = (bool)parent.GetProperty("Chest2");
            PropertiesChest[3] = (bool)parent.GetProperty("Chest3");
            PropertiesChest[4] = (bool)parent.GetProperty("Chest4");

            PropertiesHealthPot[0] = (bool)parent.GetProperty("HealthPotion0");
            PropertiesHealthPot[1] = (bool)parent.GetProperty("HealthPotion1");

            PropertiesManaPot[0] = (bool)parent.GetProperty("ManaPotion0");
            PropertiesManaPot[1] = (bool)parent.GetProperty("ManaPotion1");


            GoalValues[0] = (float)parent.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            GoalValues[1] = (float)parent.GetGoalValue(AutonomousCharacter.GAIN_LEVEL_GOAL);
            GoalValues[2] = (float)parent.GetGoalValue(AutonomousCharacter.BE_QUICK_GOAL);
            GoalValues[3] = (float)parent.GetGoalValue(AutonomousCharacter.GET_RICH_GOAL);


            this.Actions = new List<Action>(parent.Actions);
            this.Actions.Shuffle();
            this.ActionEnumerator = this.Actions.GetEnumerator();
        }

        public WorldModelFEAR(WorldModelFEAR parent)
        {
            Vector3 position = (Vector3)parent.GetProperty(Properties.POSITION);
            Array.Copy(parent.PropertiesBase, this.PropertiesBase, parent.PropertiesBase.Length);
            Array.Copy(parent.PropertiesOrcs, this.PropertiesOrcs, parent.PropertiesOrcs.Length);
            Array.Copy(parent.PropertiesSke, this.PropertiesSke, parent.PropertiesSke.Length);
            Array.Copy(parent.PropertiesDrake, this.PropertiesDrake, parent.PropertiesDrake.Length);
            Array.Copy(parent.PropertiesChest, this.PropertiesChest, parent.PropertiesChest.Length);
            Array.Copy(parent.PropertiesHealthPot, this.PropertiesHealthPot, parent.PropertiesHealthPot.Length);
            Array.Copy(parent.PropertiesManaPot, this.PropertiesManaPot, parent.PropertiesManaPot.Length);
            Array.Copy(parent.GoalValues, this.GoalValues, parent.GoalValues.Length);

            this.Actions = new List<Action>(parent.Actions);
            this.Actions.Shuffle();
            this.ActionEnumerator = this.Actions.GetEnumerator();

            this.SetProperty(Properties.POSITION, position);

        }

        public virtual object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case Properties.TIME:
                    return this.PropertiesBase[0];
                case Properties.HP:
                    return this.PropertiesBase[1];
                case Properties.MONEY:
                    return this.PropertiesBase[2];
                case Properties.POSITION: 
                    return this.PropertiesBase[3];
                case Properties.MANA:
                    return this.PropertiesBase[4];
                case Properties.ShieldHP:
                    return this.PropertiesBase[5];
                case Properties.MAXHP:
                    return this.PropertiesBase[6];
                case Properties.XP:
                    return this.PropertiesBase[7];
                case Properties.LEVEL:
                    return this.PropertiesBase[8];
                case Properties.MAXMANA:
                    return this.PropertiesBase[9];
                case Properties.ORC0:
                    return this.PropertiesOrcs[0];
                case Properties.ORC1:
                    return this.PropertiesOrcs[1];
                case Properties.ORC2:
                    return this.PropertiesOrcs[2];
                case Properties.SKE0:
                    return this.PropertiesSke[0];
                case Properties.SKE1:
                    return this.PropertiesSke[1];
                case Properties.SKE2:
                    return this.PropertiesSke[2];
                case Properties.SKE3:
                    return this.PropertiesSke[3];
                case Properties.SKE4:
                    return this.PropertiesSke[4];
                case Properties.MIGHTDRAGON:
                    return this.PropertiesDrake[0];
                case Properties.HEATHPOT0:
                    return this.PropertiesHealthPot[0];
                case Properties.HEATHPOT1:
                    return this.PropertiesHealthPot[1];
                case Properties.MANAPOT0:
                    return this.PropertiesManaPot[0];
                case Properties.MANAPOT1:
                    return this.PropertiesManaPot[1];
                case Properties.CHEST0:
                    return this.PropertiesChest[0];
                case Properties.CHEST1:
                    return this.PropertiesChest[1];
                case Properties.CHEST2:
                    return this.PropertiesChest[2];
                case Properties.CHEST3:
                    return this.PropertiesChest[3];
                case Properties.CHEST4:
                    return this.PropertiesChest[4];
                default: return null;
            }
        }

        public virtual void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case Properties.TIME:
                    this.PropertiesBase[0] = value;
                    break;
                case Properties.HP:
                    this.PropertiesBase[1] = value;
                    break;
                case Properties.MONEY:
                    this.PropertiesBase[2] = value;
                    break;
                case Properties.POSITION:
                    this.PropertiesBase[3] = value;
                    break;
                case Properties.MANA:
                    this.PropertiesBase[4] = value;
                    break;
                case Properties.ShieldHP:
                    this.PropertiesBase[5] = value;
                    break;
                case Properties.MAXHP:
                    this.PropertiesBase[6] = value;
                    break;
                case Properties.XP:
                    this.PropertiesBase[7] = value;
                    break;
                case Properties.LEVEL:
                    this.PropertiesBase[8] = value;
                    break;
                case Properties.MAXMANA:
                    this.PropertiesBase[9] = value;
                    break;
                case Properties.ORC0:
                    this.PropertiesOrcs[0] = value;
                    break;
                case Properties.ORC1:
                    this.PropertiesOrcs[1] = value;
                    break;
                case Properties.ORC2:
                    this.PropertiesOrcs[2] = value;
                    break;
                case Properties.SKE0:
                    this.PropertiesSke[0] = value;
                    break;
                case Properties.SKE1:
                    this.PropertiesSke[1] = value;
                    break;
                case Properties.SKE2:
                    this.PropertiesSke[2] = value;
                    break;
                case Properties.SKE3:
                    this.PropertiesSke[3] = value;
                    break;
                case Properties.SKE4:
                    this.PropertiesSke[4] = value;
                    break;
                case Properties.MIGHTDRAGON:
                    this.PropertiesDrake[0] = value; 
                    break;
                case Properties.HEATHPOT0:
                    this.PropertiesHealthPot[0] = value;
                    break;
                case Properties.HEATHPOT1:
                    this.PropertiesHealthPot[1] = value;
                    break;
                case Properties.MANAPOT0:
                    this.PropertiesManaPot[0] = value;
                    break;
                case Properties.MANAPOT1:
                    this.PropertiesManaPot[1] = value;
                    break;
                case Properties.CHEST0:
                    this.PropertiesChest[0] = value;
                    break;
                case Properties.CHEST1:
                    this.PropertiesChest[1] = value;
                    break;
                case Properties.CHEST2:
                    this.PropertiesChest[2] = value;
                    break;
                case Properties.CHEST3:
                    this.PropertiesChest[3] = value;
                    break;
                case Properties.CHEST4:
                    this.PropertiesChest[4] = value;
                    break;
            }
        }

        public virtual float GetGoalValue(string goalName)
        {
            switch (goalName)
            {
                case AutonomousCharacter.SURVIVE_GOAL:
                    return GoalValues[0];
                case AutonomousCharacter.GAIN_LEVEL_GOAL:
                    return GoalValues[1];
                case AutonomousCharacter.BE_QUICK_GOAL:
                    return GoalValues[2];
                case AutonomousCharacter.GET_RICH_GOAL:
                    return GoalValues[3];
                default: return -1000f;
            }
        }

        public virtual void SetGoalValue(string goalName, float value)
        {
            switch (goalName)
            {
                case AutonomousCharacter.SURVIVE_GOAL:
                    GoalValues[0] = value;
                    break;
                case AutonomousCharacter.GAIN_LEVEL_GOAL:
                    GoalValues[1] = value;
                    break;
                case AutonomousCharacter.BE_QUICK_GOAL:
                    GoalValues[2] = value;
                    break;
                case AutonomousCharacter.GET_RICH_GOAL:
                    GoalValues[3] = value;
                    break;
            }
        }

        public virtual WorldModelFEAR GenerateChildWorldModel()
        {
            return new WorldModelFEAR(this);
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

            if (HP <= 0) 
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
