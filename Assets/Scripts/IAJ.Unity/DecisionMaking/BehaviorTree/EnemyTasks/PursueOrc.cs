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
    class PursueOrc : Pursue
    {
        public static float timeToWaagh = 7f;
        private float time = timeToWaagh + 1f;
        private WAARRGGHHH scream;

        private float lastUpdate = 0f;
        public PursueOrc(Monster character, GameObject target, float _range):base(character, target, _range) 
        {
            scream = new WAARRGGHHH(character as Orc, target);
        }

        public override Result Run()
        {
            time += Time.deltaTime;
            if (time > timeToWaagh || Time.time - lastUpdate > timeToWaagh) 
            {
                scream.Run();
                time = 0f;
                lastUpdate = Time.time;
            }

            return base.Run();
        }

    }
}
