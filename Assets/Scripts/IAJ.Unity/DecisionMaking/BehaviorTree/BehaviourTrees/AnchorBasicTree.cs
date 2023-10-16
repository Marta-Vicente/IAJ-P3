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
    class AnchorBasicTree : Sequence
    {
        public AnchorBasicTree(Monster character, GameObject target, Vector3 PositionA, Vector3 PositionB)
        {
            this.children = new List<Task>()
            {
                new Patrol(character, target, PositionA, PositionB)
            };

        }

    }
}
