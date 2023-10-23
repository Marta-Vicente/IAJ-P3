﻿using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace Assets.Scripts.Game
{
    public class FutureStateWorldModel : WorldModel
    {
        public GameManager GameManager { get; set; }
        protected int NextPlayer { get; set; }
        protected Action NextEnemyAction { get; set; }
        protected Action[] NextEnemyActions { get; set; }

        public FutureStateWorldModel(GameManager gameManager, List<Action> actions) : base(actions)
        {
            this.GameManager = gameManager;
            this.NextPlayer = 0;
        }

        public FutureStateWorldModel(FutureStateWorldModel parent) : base(parent)
        {
            this.GameManager = parent.GameManager;
        }

        public override WorldModel GenerateChildWorldModel()
        {
            return new FutureStateWorldModel(this);
        }

        public override bool IsTerminal()
        {
            int HP = (int)this.GetProperty(Properties.HP);
            float time = (float)this.GetProperty(Properties.TIME);
            int money = (int)this.GetProperty(Properties.MONEY);

            /*
            if (money == 25) {
                Debug.Log("Chests: " + money + " " + "Hp: " + HP + " Time" + time + " res:" +
                    (HP <= 0 || time >= GameManager.GameConstants.TIME_LIMIT || (this.NextPlayer == 0 && money == 25)));
                }
            */

            return HP <= 0 ||  time >= GameManager.GameConstants.TIME_LIMIT || (this.NextPlayer == 0 && money == 25);
        }

        public override float GetScore()
        {
            int money = (int)this.GetProperty(Properties.MONEY);
            int HP = (int)this.GetProperty(Properties.HP);
            float time = (float)this.GetProperty(Properties.TIME);

            // TODO : Should Time and other factors be taken into accoun?

            if (HP <= 0 || time >= 150f) return 0.0f;
            else if (money == 25)
            {
                return 1.0f;
            }
            else return 0.0f;
        }

        public override int GetNextPlayer()
        {
            return this.NextPlayer;
        }

        public override void CalculateNextPlayer()
        {
            Vector3 position = (Vector3)this.GetProperty(Properties.POSITION);
            bool enemyEnabled;

            //basically if the character is close enough to an enemy, the next player will be the enemy.
            foreach (var enemy in this.GameManager.enemies)
            {
                enemyEnabled = (bool) this.GetProperty(enemy.name);
                if (enemyEnabled && (enemy.transform.position - position).sqrMagnitude <= 100)
                {
                    this.NextPlayer = 1;
                    this.NextEnemyAction = new SwordAttack(this.GameManager.Character, enemy);
                    this.NextEnemyActions = new Action[] { this.NextEnemyAction };
                    return; 
                }
            }
            this.NextPlayer = 0;
            //if not, then the next player will be player 0
        }

        public override Action GetNextAction()
        {
            Action action;
            if (this.NextPlayer == 1)
            {
                action = this.NextEnemyAction;
                this.NextEnemyAction = null;
                return action;
            }
            else return base.GetNextAction();
        }

        public override Action[] GetExecutableActions()
        {
            if (this.NextPlayer == 1)
            {
                return this.NextEnemyActions;
            }
            else return base.GetExecutableActions();
        }

    }
}
