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
    class NavAgentMovement : Patrol
    {
        public NavAgentMovement(Monster character, Vector3 PositionA, Vector3 PositionB)
        {
            hunting = new MoveTo(character, character.DefaultPosition, 1f, false, HuntingTimerMax);
            List<Task> tasks = new List<Task>();
            tasks.Add(
                new Sequence(
                        new List<Task>
                        {
                            new MoveTo(character, new Vector3(PositionA.x, PositionA.y, PositionA.z), 1.0f),
                            new MoveTo(character, new Vector3(PositionB.x, PositionB.y, PositionB.z), 1.0f)
                        }
                 )
            );
            this.children = tasks;

            Scream += HeardScream;
        }

        public override Result Run()
        {
            if (IsOnTheHunt)
            {
                Result result = hunting.Run();
                if (result == Result.Success || result == Result.Failure)
                {
                    currentChild = 0;
                    IsOnTheHunt = false;

                }
                return Result.Running;
            }
            return children[0].Run();
        }


        public override void HeardScream(object sender, Vector3 e)
        {
            IsOnTheHunt = true;
            HuntingTarget = e;
            hunting.Target = HuntingTarget;
        }
    }
}
