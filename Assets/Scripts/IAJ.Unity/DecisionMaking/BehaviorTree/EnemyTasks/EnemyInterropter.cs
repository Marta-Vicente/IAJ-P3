using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AI;
using Assets.Scripts.Game.NPCs;
using Assets.Scripts.Game;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.EnemyTasks
{
    class EnemyInterruptor : Task
    {
        protected NPC Character { get; set; }

        public GameObject Target { get; set; }

        public float range;

        public EnemyInterruptor(NPC character, GameObject target, float _range)
        {
            this.Character = character;
            this.Target = target;
            range = _range;
        }

        public override Result Run()
        {
           return Result.Success;

        }

    }
}
