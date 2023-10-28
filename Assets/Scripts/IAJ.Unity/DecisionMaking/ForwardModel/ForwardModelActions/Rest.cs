using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    [Serializable]
    public class Rest : Action
    {
        [JsonIgnore]
        protected AutonomousCharacter Character { get; set; }

        public Rest(AutonomousCharacter character) : base("Rest")
        {
            Character = character;
            Duration = AutonomousCharacter.RESTING_INTERVAL;
        }

        public override bool CanExecute()
        {
            return Character.baseStats.HP < Character.baseStats.MaxHP;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var currentdHP = (int)worldModel.GetProperty(Properties.HP);
            var MaxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            return currentdHP < MaxHP;
        }

        public override bool CanExecute(WorldModelFEAR worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var currentdHP = (int)worldModel.GetProperty(Properties.HP);
            var MaxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            return currentdHP < MaxHP;
        }

        public override void Execute()
        {
            GameManager.Instance.Rest();
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.SURVIVE_GOAL)
            {
                change -= 2;

                if (Character.nearEnemy) change += 3;
            }
            else if (goal.Name == AutonomousCharacter.BE_QUICK_GOAL)
            {
                change += this.Duration*3;
            }
 
            return change;
        }

        public override float GetDuration()
        {
            return base.GetDuration();
        }


        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);

            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            var currentHP = (int)worldModel.GetProperty(Properties.HP);
            var surviveGoal = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            var beQuickGoal = worldModel.GetGoalValue(AutonomousCharacter.BE_QUICK_GOAL);
            float time = (float)worldModel.GetProperty(Properties.TIME);

            var change = 0;
            var enemyNear = 0;

            if (currentHP + 2 <= maxHP )
            {
                change = 2;
            }
            else if (currentHP + 1 <= maxHP)
            {
                change = 1;
            }
            if (Character.nearEnemy)
                enemyNear = 3;

            worldModel.SetProperty(Properties.HP, currentHP + change);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, surviveGoal - change + enemyNear);
            worldModel.SetGoalValue(AutonomousCharacter.BE_QUICK_GOAL, beQuickGoal + this.Duration * 3);
            worldModel.SetProperty(Properties.TIME, time + GetDuration());

        }

        public override void ApplyActionEffects(WorldModelFEAR worldModel)
        {
            base.ApplyActionEffects(worldModel);

            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            var currentHP = (int)worldModel.GetProperty(Properties.HP);
            var surviveGoal = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            var beQuickGoal = worldModel.GetGoalValue(AutonomousCharacter.BE_QUICK_GOAL);
            float time = (float)worldModel.GetProperty(Properties.TIME);

            var change = 0;
            var enemyNear = 0;

            if (currentHP + 2 <= maxHP)
            {
                change = 2;
            }
            else if (currentHP + 1 <= maxHP)
            {
                change = 1;
            }
            if (Character.nearEnemy)
                enemyNear = 3;

            worldModel.SetProperty(Properties.HP, currentHP + change);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, surviveGoal - change + enemyNear);
            worldModel.SetGoalValue(AutonomousCharacter.BE_QUICK_GOAL, beQuickGoal + this.Duration*3);
            worldModel.SetProperty(Properties.TIME, time + GetDuration());

        }

        public override float GetHValue(WorldModel worldModel)
        {
            var currentHP = (int)worldModel.GetProperty(Properties.HP);
            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            float time = (float)worldModel.GetProperty(Properties.TIME);

            return (currentHP / maxHP) + base.GetHValue(worldModel) + ((time)/150f)*3;
        }
        
    }
}
