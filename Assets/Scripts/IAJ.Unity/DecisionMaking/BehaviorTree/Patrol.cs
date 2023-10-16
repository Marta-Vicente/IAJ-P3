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
    class Patrol : Selector
    {
        public static event EventHandler<Vector3> Scream;
        public IsCharacterNearTarget checker;
        public MoveTo hunting;
        public bool IsOnTheHunt = false;
        public Vector3 HuntingTarget;
        public float HuntingTimerMax = 20f;

        protected Orc orc;

        protected Patrol()
        {

        }
        public Patrol(Monster character, GameObject target)
        {
            orc = (Orc) character;

            List<Task> tasks = new List<Task>();

            checker = new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance);
            hunting = new MoveTo(character, character.DefaultPosition, 1f, false, HuntingTimerMax);
            tasks.Add(
                new Sequence(new List<Task>
                                {
                                    new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance),
                                    new PursueOrc(character, target, character.enemyStats.WeaponRange),
                                    new LightAttack(character)
                                })
            );
            tasks.Add(
                new Sequence(
                        new List<Task>
                        {
                            new MoveTo(character, new Vector3(character.DefaultPosition.x + 10,
                                                                character.DefaultPosition.y,
                                                                character.DefaultPosition.z), 1.0f, false),
                            new MoveTo(character, new Vector3(character.DefaultPosition.x - 10,
                                                                character.DefaultPosition.y,
                                                                character.DefaultPosition.z), 1.0f, false)
                        }
                 )
            );
            tasks.Add(new MoveTo(character, character.DefaultPosition, 1.0f));

            this.children = tasks;

            Scream += HeardScream;
        }

        public Patrol(Monster character, GameObject target, Vector3 PositionA, Vector3 PositionB)
        {
            orc = (Orc)character;

            List<Task> tasks = new List<Task>();

            checker = new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance);
            hunting = new MoveTo(character, PositionA, 1f, false, HuntingTimerMax);
            tasks.Add(
                new Sequence(new List<Task>
                                {
                                    new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance),
                                    new PursueOrc(character, target, character.enemyStats.WeaponRange),
                                    new LightAttack(character)
                                })
            );
            tasks.Add(
                new Sequence(
                        new List<Task>
                        {
                            new MoveTo(character, new Vector3(PositionA.x, PositionA.y, PositionA.z), 1.0f, false, 50f),
                            new MoveTo(character, new Vector3(PositionB.x, PositionB.y, PositionB.z), 1.0f, false, 50f)
                        }
                 )
            );
            tasks.Add(new MoveTo(character, PositionA, 1.0f));

            this.children = tasks;

            Scream += HeardScream;
        }


        public override Result Run()
        {

            if (IsOnTheHunt && currentChild != 0 && checker.Run() == Result.Failure)
            {
                Result result = hunting.Run();
                if (result == Result.Success || result == Result.Failure)
                {
                    currentChild = 0;
                    IsOnTheHunt = false;
                }
                return Result.Running;
            }


            if (currentChild != 0 && checker.Run() == Result.Success)
            {
                currentChild = 0;
                IsOnTheHunt = false;
                orc.usingFormation = false;
                return Result.Running;
            }

            if (children.Count > this.currentChild)
            {
                Result result = children[currentChild].Run();

                if (result == Result.Running)
                    return Result.Running;

                else if (result == Result.Failure)
                {
                    currentChild++;
                    if (children.Count <= this.currentChild)
                    {
                        currentChild = 0;
                        children[currentChild].Run();
                        return Result.Failure;
                    }

                    return Result.Running;
                }
                else
                {
                    currentChild = 0;
                    children[currentChild].Run();
                    return Result.Success;
                }
            }
            return Result.Success;
        }

        public virtual void HeardScream(object sender, Vector3 e)
        {
            //Debug.Log("WAAAAGHH! at " + e);
            orc.usingFormation = false;
            IsOnTheHunt = true;
            HuntingTarget = e;
            hunting.Target = HuntingTarget; 
            
        }

        public static void OnScream(Vector3 target)
        {
            Scream?.Invoke(null, target);
        }

    }
}
