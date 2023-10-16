using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.EnemyTasks;
using UnityEngine;
using Assets.Scripts.Game;
using Assets.Scripts.Game.NPCs;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.BehaviourTrees
{
    class NavMachBT : Sequence
    {
        public NavMachBT(Monster character, Vector3 PositionA, Vector3 PositionB)
        {
            this.children = new List<Task>()
            {
                new NavAgentMovement(character, PositionA, PositionB)
            };

        }

    }
}
