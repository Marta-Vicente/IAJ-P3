using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine.AI;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.BehaviourTrees;
//using Assets.Scripts.IAJ.Unity.Formations;
using System.Collections.Generic;
using UnityEditor;

namespace Assets.Scripts.Game.NPCs
{

    public class Orc : Monster
    {

        public AudioSource audioSource;
        public GameObject screamView;
        public bool isAnchor = false;
        public bool isNavAgent = false;

        public Orc()
        {
            this.enemyStats.Type = "Orc";
            this.enemyStats.XPvalue = 8;
            this.enemyStats.AC = 14;
            this.baseStats.HP = 15;
            this.DmgRoll = () => RandomHelper.RollD10() + 2;
            this.enemyStats.SimpleDamage = 6;
            this.enemyStats.AwakeDistance = 15;
            this.enemyStats.WeaponRange = 3;
            //usingFormation = false;
        }

        public override void InitializeBehaviourTree()
        {
            var patrols = GameObject.FindGameObjectsWithTag("Patrol");

            float pos = float.MaxValue;
            GameObject closest = null;
            foreach (var p in patrols)
            {
                if (Vector3.Distance(this.agent.transform.position, p.transform.position) < pos)
                {
                    pos = Vector3.Distance(this.agent.transform.position, p.transform.position);
                    closest = p;
                }

            }

            var position1 = closest.transform.GetChild(0).position;
            var position2 = closest.transform.GetChild(1).position;

            //TODO Create a Behavior tree that combines Patrol with other behaviors...
            //var mainTree = new Patrol(this, position1, position2);
            if (this.isAnchor)
            {
                this.usingFormation = false;
                this.BehaviourTree = new AnchorBasicTree(this, Target,
                    new Vector3(25.92593f, this.DefaultPosition.y, 12.46296f),
                    new Vector3(74.07407f, this.DefaultPosition.y, 94.44444f));
            }
            else if (this.usingFormation)
            {
                this.BehaviourTree = new FormationBasicTree(this, Target, new OrcBasicTree(this, Target));
            }
            else if (this.isNavAgent)
                this.setNavAgentBehavior();
            else
                this.BehaviourTree = new OrcBasicTree(this, Target);
            
         }

        public override void FixedUpdate()
        {
            if (GameManager.Instance.gameEnded) return;

            if (usingBehaviourTree)
            {
                if (this.BehaviourTree != null)
                    this.BehaviourTree.Run();
                else
                    this.BehaviourTree = new OrcBasicTree(this, Target);
            }

        }

        public void PlayAudio()
        {
            audioSource.Play();
        }

        public void VisualizeAudio(Vector3 pos)
        {
            pos.y = 5f;
            screamView.transform.position = pos;
            GameObject newObject = Instantiate(screamView);
            Scream s = newObject.AddComponent<Scream>();
            s.centerPosition = pos;
            s.asset = newObject;
            
        }

        public void setNavAgentBehavior()
        {
            this.BehaviourTree = new NavMachBT(this,
                    new Vector3(25.92593f, this.DefaultPosition.y, 12.46296f),
                    new Vector3(74.07407f, this.DefaultPosition.y, 94.44444f));
        }

    }
}
