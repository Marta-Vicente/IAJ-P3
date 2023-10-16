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
    class OffRoutBT: Sequence
    {
        public OffRoutBT(Monster character, Vector3 PositionA)
        {
            this.children = new List<Task>()
            {
                new MoveTo(character, PositionA, 1f),
            };

        }

    }
}
