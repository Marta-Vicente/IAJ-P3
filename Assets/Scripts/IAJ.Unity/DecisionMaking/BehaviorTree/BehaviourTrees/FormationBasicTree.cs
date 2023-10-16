using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.EnemyTasks;
using UnityEngine;
using Assets.Scripts.Game;
using Assets.Scripts.Game.NPCs;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.BehaviourTrees
{
    class FormationBasicTree : Sequence
    {
        public FormationBasicTree(Monster character, GameObject target, OrcBasicTree orcBehavior)
        {
            this.children = new List<Task>()
            {
                new FormationMovement(character, target, orcBehavior)
            };

        }

    }
}
