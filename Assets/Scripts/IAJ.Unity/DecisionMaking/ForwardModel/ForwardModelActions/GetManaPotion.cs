using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    [Serializable]
    public class GetManaPotion : WalkToTargetAndExecuteAction
    {
        public GetManaPotion(AutonomousCharacter character, GameObject target) : base("GetManaPotion",character,target)
        {
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute()) return false;
            return Character.baseStats.Mana < Character.baseStats.MaxMana;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var currentMana = (int)worldModel.GetProperty(Properties.MANA);
            var maxMana = (int)worldModel.GetProperty(Properties.MAXMANA);
            return currentMana < 10;
        } 
        public override bool CanExecute(WorldModelFEAR worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var currentMana = (int)worldModel.GetProperty(Properties.MANA);
            var maxMana = (int)worldModel.GetProperty(Properties.MAXMANA);
            return currentMana < 10;
        }

        public override void Execute()
        {
            base.Execute();
            GameManager.Instance.GetManaPotion(this.Target);
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
            var maxMana = 10;
            worldModel.SetProperty(Properties.MANA, maxMana);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, 0.0f);

            //disables the target object so that it can't be reused again
            worldModel.SetProperty(this.Target.name, false);
        }

        public override void ApplyActionEffects(WorldModelFEAR worldModel)
        {
            base.ApplyActionEffects(worldModel);
            var maxMana = 10;
            worldModel.SetProperty(Properties.MANA, maxMana);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, 0.0f);

            //disables the target object so that it can't be reused again
            worldModel.SetProperty(this.Target.name, false);
        }

        public override float GetHValue(WorldModel worldModel)
        {
            var currentMana = (int)worldModel.GetProperty(Properties.MANA);
            float time = (float)worldModel.GetProperty(Properties.TIME);

            return (currentMana / 10) + base.GetHValue(worldModel) + (time/150)*3;
        }
    }
}
