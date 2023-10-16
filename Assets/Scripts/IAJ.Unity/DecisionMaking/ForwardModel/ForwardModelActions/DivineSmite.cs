using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using Assets.Scripts.IAJ.Unity.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class DivineSmite : WalkToTargetAndExecuteAction
    {
        private float expectedHPChange;
        private float expectedXPChange;
        private int xpChange;
        private int enemyAC;
        private int enemySimpleDamage;
        //how do you like lambda's in c#?
        private Func<int> dmgRoll;

        public DivineSmite(AutonomousCharacter character, GameObject target) : base("DivineSmite",character,target)
        {
            if (target.tag.Equals("Skeleton"))
            {
                this.dmgRoll = () => RandomHelper.RollD6();
                this.enemySimpleDamage = 0;
                this.expectedHPChange = 0;
                this.xpChange = 3;
                this.expectedXPChange = 2.7f;
                this.enemyAC = 10;
            }
        }

        public override void Execute()
        {
            base.Execute();
            GameManager.Instance.DivineSmite(this.Target);
        }

        public override bool CanExecute()
        {
            return Character.baseStats.Mana >= 2 && base.CanExecute();
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            int mana = (int)worldModel.GetProperty(Properties.MANA);
            return mana >= 2 && base.CanExecute(worldModel);
        }

        public override bool CanExecute(WorldModelFEAR worldModel)
        {
            int mana = (int)worldModel.GetProperty(Properties.MANA);
            return mana >= 2 && base.CanExecute(worldModel);
        }


        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);

            int hp = (int)worldModel.GetProperty(Properties.HP);
            int xp = (int)worldModel.GetProperty(Properties.XP);
            int shieldHp = (int)worldModel.GetProperty(Properties.ShieldHP);

            var surviveValue = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, surviveValue);

            worldModel.SetProperty(this.Target.name, false);
            worldModel.SetProperty(Properties.XP, xp + this.xpChange);

            var mana = (int)worldModel.GetProperty(Properties.MANA);
            worldModel.SetProperty(Properties.MANA, mana - 2);

            if (GameManager.Instance.StochasticWorld)
            {
                //there was an hit, enemy is destroyed, gain xp
                //disables the target object so that it can't be reused again
                worldModel.SetProperty(this.Target.name, false);
                worldModel.SetProperty(Properties.XP, xp + this.xpChange);
            }
        }

        public override void ApplyActionEffects(WorldModelFEAR worldModel)
        {
            base.ApplyActionEffects(worldModel);

            int hp = (int)worldModel.GetProperty(Properties.HP);
            int xp = (int)worldModel.GetProperty(Properties.XP);
            int shieldHp = (int)worldModel.GetProperty(Properties.ShieldHP);

            var surviveValue = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, surviveValue);

            worldModel.SetProperty(this.Target.name, false);
            worldModel.SetProperty(Properties.XP, xp + this.xpChange);

            var mana = (int)worldModel.GetProperty(Properties.MANA);
            worldModel.SetProperty(Properties.MANA, mana - 2);

            if (GameManager.Instance.StochasticWorld)
            {
                //there was an hit, enemy is destroyed, gain xp
                //disables the target object so that it can't be reused again
                worldModel.SetProperty(this.Target.name, false);
                worldModel.SetProperty(Properties.XP, xp + this.xpChange);
            }
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.SURVIVE_GOAL)
            {
                change += this.expectedHPChange;
            }
            else if (goal.Name == AutonomousCharacter.GAIN_LEVEL_GOAL)
            {
                change += -this.expectedXPChange;
            }

            return change;
        }

        public override float GetHValue(WorldModel worldModel)
        {
            //float time = (float) worldModel.GetProperty(Properties.TIME);
            //return GetHValue(worldModel);// + (time/150f)*3;
            //somehow it fucks here???
            return 1f;
        }

    }
}
