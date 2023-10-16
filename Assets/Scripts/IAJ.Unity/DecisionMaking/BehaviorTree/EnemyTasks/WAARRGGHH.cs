using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AI;
using Assets.Scripts.Game.NPCs;
using Assets.Scripts.Game;
using static UnityEngine.GraphicsBuffer;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree;
using System.Threading;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.EnemyTasks
{
    class WAARRGGHHH : Task
    {
        public Orc orc;
        public GameObject target;

        public WAARRGGHHH(Orc orc, GameObject target)
        {
            this.target = target;
            this.orc = orc;
        }

        public override Result Run()
        {
            BehaviourTrees.Patrol.OnScream(orc.transform.position);
            orc.PlayAudio();
            orc.VisualizeAudio(orc.transform.position);
            return Result.Success;
        }

    }
}
