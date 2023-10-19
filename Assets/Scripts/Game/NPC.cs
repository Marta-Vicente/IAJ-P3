﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Game
{
    public class NPC : MonoBehaviour
    {
        [Serializable]
        public struct Stats
        {
            public string Name;
            public int HP;
            public int ShieldHP;
            public int MaxHP;
            public int Mana;
            public int XP;
            public float Time;
            public int Money;
            public int Level;
            public int MaxMana;
        }

        protected GameObject character;

        // Pathfinding
        protected UnityEngine.AI.NavMeshAgent navMeshAgent;
        private Vector3 previousTarget;

        public Stats baseStats;


        void Awake()
        {
            previousTarget = new Vector3(0.0f, 0.0f, 0.0f);
            this.character = this.gameObject;
            navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        public virtual void ResetCharacter()
        {
            this.baseStats.HP = this.baseStats.MaxHP;
            this.baseStats.ShieldHP = 0;
            this.baseStats.Mana = 0;
            this.baseStats.XP = 0;
            this.baseStats.Time = 150;
            this.baseStats.Money = 0;
            this.baseStats.Level = 1;

        }



        #region Navmesh Pathfinding Methods

        public void StartPathfinding(Vector3 targetPosition)
        {
            //if the targetPosition received is the same as a previous target, then this a request for the same target
            //no need to redo the pathfinding search
            if (!this.previousTarget.Equals(targetPosition))
            {

                this.previousTarget = targetPosition;

                navMeshAgent.SetDestination(targetPosition);

            }
        }

        public void StopPathfinding()
        {
            navMeshAgent.isStopped = true;
        }

        // Simple way of calculating distance left to target using Unity's navmesh
        /*
        public float GetDistanceToTarget(Vector3 originalPosition, Vector3 targetPosition)
        {
            var distance = 0.0f;

            NavMeshPath result = new NavMeshPath();
            var r = navMeshAgent.CalculatePath(targetPosition, result);
            if (r == true)
            {
                var currentPosition = originalPosition;
                foreach (var c in result.corners)
                {
                    //Rough estimate, it does not account for shortcuts so we have to multiply it
                    distance += Vector3.Distance(currentPosition, c) * 0.65f;
                    currentPosition = c;
                }
                return distance;
            }

            //Default value
            return 100;
        }
        */
        public float GetDistanceToTarget(Vector3 originalPosition, Vector3 targetPosition)
        {
            return (float) Math.Sqrt(Math.Pow(targetPosition.x - originalPosition.x, 2) + Math.Pow(targetPosition.y - originalPosition.y, 2)) * 3;
        }

        #endregion

    }
}
