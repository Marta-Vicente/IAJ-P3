using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    [Serializable]
    public class Tp : Action
    {
        [JsonIgnore]
        protected AutonomousCharacter Character { get; set; }
        public Tp(AutonomousCharacter character) : base("Teleport")
        {
            Character = character;
        }

        public override bool CanExecute()
        {
            return Character.baseStats.Level >= 2 && Character.baseStats.Mana >= 5;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var level = (int)worldModel.GetProperty(Properties.LEVEL);
            var currentMana = (int)worldModel.GetProperty(Properties.MANA);
            return level >= 2 && currentMana >= 5;
        }

        public override bool CanExecute(WorldModelFEAR worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var level = (int)worldModel.GetProperty(Properties.LEVEL);
            var currentMana = (int)worldModel.GetProperty(Properties.MANA);
            return level >= 2 && currentMana >= 5;
        }

        public override void Execute()
        {
            GameManager.Instance.Teleport();
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.SURVIVE_GOAL)
            {
                change -= goal.InsistenceValue;
            }
 
            return change;
        }

        
        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);

            var mana = (int)worldModel.GetProperty(Properties.MANA);
            worldModel.SetProperty(Properties.MANA, mana - 5);

            worldModel.SetProperty(Properties.POSITION, Character.initialPositon);

            var surviveValue = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);

            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, surviveValue);


        }

        public override void ApplyActionEffects(WorldModelFEAR worldModel)
        {
            base.ApplyActionEffects(worldModel);

            var mana = (int)worldModel.GetProperty(Properties.MANA);
            worldModel.SetProperty(Properties.MANA, mana - 5);

            worldModel.SetProperty(Properties.POSITION, Character.initialPositon);

            var surviveValue = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);

            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, surviveValue);


        }

        public override float GetHValue(WorldModel worldModel)
        {
            var currentHP = (int)worldModel.GetProperty(Properties.HP);
            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);

            return (currentHP / maxHP) + base.GetHValue(worldModel);
        }
        
    }
}
