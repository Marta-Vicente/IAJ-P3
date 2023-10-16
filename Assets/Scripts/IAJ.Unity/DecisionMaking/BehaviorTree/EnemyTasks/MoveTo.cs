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
    class MoveTo : Task
    {
        protected NPC Character { get; set; }

        public Vector3 Target { get; set; }

        public float range;

        public float timeout = 10f;

        public float time;

        public bool force;

        public MoveTo(Monster character, Vector3 target, float _range, bool force = true, float timeout = 10f)
        {
            this.Character = character;
            this.Target = target;
            range = _range;
            this.force = force;
            this.timeout = timeout;
        }

        public override Result Run()
        {
            if (Target == null || (!force && time > timeout))
            {
                time = 0;
                return Result.Failure;
            }

            if (Vector3.Distance(Character.transform.position, this.Target) <= range)
            {
                time = 0;
                return Result.Success;
            }
            else
            {
                Character.StartPathfinding(Target);
                time += Time.deltaTime;
                return Result.Running;
            }

        }

    }
}
