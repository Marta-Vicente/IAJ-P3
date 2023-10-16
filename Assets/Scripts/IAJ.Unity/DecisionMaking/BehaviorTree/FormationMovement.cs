using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.EnemyTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Game;
using Assets.Scripts.Game.NPCs;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree;


namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.BehaviourTrees
{
    class FormationMovement : Selector
    {
        public IsCharacterNearTarget checker;

        private Orc orc;

        private OrcBasicTree orcDeafault;

        public FormationMovement(Monster character, GameObject target, OrcBasicTree orcBehavior)
        {
            orc = (Orc)character;

            this.orcDeafault = orcBehavior;

            checker = new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance);
        }

        public override Result Run()
        {

            if (checker.Run() == Result.Success)
            {
                orc.usingFormation = false;
                orc.BehaviourTree = orcDeafault;
                return Result.Running;
            }

            return Result.Success;
        }

    }
}
