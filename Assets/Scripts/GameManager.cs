using Assets.Scripts.IAJ.Unity.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game.NPCs;
using Assets.Scripts.IAJ.Unity.Formations;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.BehaviourTrees;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioSource Shield;
    public AudioSource Smite;
    public AudioSource Level;
    public AudioSource Sword;
    public AudioSource Open;
    public AudioSource Drink;
    public AudioSource Sleep;
    public AudioSource Flash;
    public static class GameConstants
    {
        public const float UPDATE_INTERVAL = 2.0f;
        public const int TIME_LIMIT = 150;
        public const int PICKUP_RANGE = 8;

    }
    //public fields, seen by Unity in Editor

    public AutonomousCharacter Character;

    [Header("UI Objects")]
    public Text HPText;
    public Text ShieldHPText;
    public Text ManaText;
    public Text TimeText;
    public Text XPText;
    public Text LevelText;
    public Text MoneyText;
    public Text DiaryText;
    public GameObject GameEnd;

    [Header("Enemy Settings")]
    public bool SleepingNPCs;
    public bool BehaviourTreeNPCs;
    public bool StochasticWorld = false;

    //fields
    public List<GameObject> chests { get; set; }
    public List<GameObject> skeletons { get; set; }
    public List<GameObject> orcs { get; set; }
    public List<GameObject> dragons { get; set; }
    public List<GameObject> enemies { get; set; }
    public Dictionary<string, List<GameObject>> disposableObjects { get; set; }
    public bool WorldChanged { get; set; }

    private float nextUpdateTime = 0.0f;
    private float enemyAttackCooldown = 0.0f;
    public bool gameEnded { get; set; } = false;
    public Vector3 initialPosition { get; set; }

    public List<FormationManager> Formations { get; set; }

    public GameObject NavAgent;

    public GameObject FormationOrc3;
    public GameObject FormationOrc4;
    public GameObject FormationOrc5;

    public bool UseFormationLine;
    public bool UseFormationTri;
    public bool FirstStart = true;

    void Awake()
    {
        StartGame();
        FirstStart = false;


        //To use line make orc 3,4 using formation = true, 5 is anchor, get rid of nav mach to 1000y
        /*if (UseFormationLine)
        {
            try
            {
                var monster5 = FormationOrc5;
                var monster3 = FormationOrc3;
                var monster4 = FormationOrc4;

                //monster5.GetComponent<Orc>().isAnchor = true;

                var lineForm = new LineFormation();
                var lineFormationManager = new FormationManager(new List<Monster>()
            {
                monster5.GetComponent<Orc>(),
                monster4.GetComponent<Orc>(),
                monster3.GetComponent<Orc>()
            },
                lineForm, monster5.transform.position, monster5.transform.forward);


                lineFormationManager.AddCharacter(monster5.GetComponent<Orc>());
                lineFormationManager.AddCharacter(monster3.GetComponent<Orc>());
                lineFormationManager.AddCharacter(monster4.GetComponent<Orc>());


                this.Formations = new List<FormationManager>()
            {
                lineFormationManager
            };


            }
            catch
            {
                Debug.Log("Normal Dungeon, Formations off");
                this.Formations = null;
            }
        }

        //To use triangule make orc 3,4,5 using formation = true, no one is anchor, put nav mach at 0
        else if (UseFormationTri)
        {
            try
            {
                var monster5 = FormationOrc5;
                var monster3 = FormationOrc3;
                var monster4 = FormationOrc4;

                // NavAgent.transform.position = new Vector3(NavAgent.transform.position.x, 0, NavAgent.transform.position.z);
                monster5.GetComponent<Orc>().isAnchor = false;
                NavAgent.GetComponent<Orc>().setNavAgentBehavior();

                var triangularFormation = new TriangularFormation();
                var trianguleFormationManager = new FormationManager(new List<Monster>()
            {
                NavAgent.GetComponent<Orc>(),
                monster5.GetComponent<Orc>(),
                monster4.GetComponent<Orc>(),
                monster3.GetComponent<Orc>()
            },
                triangularFormation, NavAgent.GetComponent<Orc>().transform.position, NavAgent.GetComponent<Orc>().transform.forward);


                trianguleFormationManager.AddCharacter(monster5.GetComponent<Orc>());
                trianguleFormationManager.AddCharacter(monster3.GetComponent<Orc>());
                trianguleFormationManager.AddCharacter(monster4.GetComponent<Orc>());


                this.Formations = new List<FormationManager>()
            {
                trianguleFormationManager
            };

            }
            catch
            {
                Debug.Log("Normal Dungeon, Formations off");
                this.Formations = null;
            }
        }*/

    }

    public void StartGame()
    {
        //if (FirstStart)
            Instance = this;
        //else
            //Instance = new GameManager();

        UpdateDisposableObjects();

        this.WorldChanged = false;
        if (!FirstStart)
        {
            this.Character.ResetCharacter();
            this.Character.StartCharacter();
        }
        this.Character = GameObject.FindGameObjectWithTag("Player").GetComponent<AutonomousCharacter>();

        this.initialPosition = this.Character.gameObject.transform.position;

        this.gameEnded = false;
        this.GameEnd.SetActive(false);
    }

    public void UpdateDisposableObjects()
    {
        this.enemies = new List<GameObject>();
        this.disposableObjects = new Dictionary<string, List<GameObject>>();
        this.chests = GameObject.FindGameObjectsWithTag("Chest").ToList();
        this.skeletons = GameObject.FindGameObjectsWithTag("Skeleton").ToList();
        this.orcs = GameObject.FindGameObjectsWithTag("Orc").ToList();
        this.dragons = GameObject.FindGameObjectsWithTag("Dragon").ToList();
        this.enemies.AddRange(this.skeletons);
        this.enemies.AddRange(this.orcs);
        this.enemies.AddRange(this.dragons);

     
        //adds all enemies to the disposable objects collection
        foreach (var enemy in this.enemies)
        {
            if (disposableObjects.ContainsKey(enemy.name))
            {
                this.disposableObjects[enemy.name].Add(enemy);
            }
            else this.disposableObjects.Add(enemy.name, new List<GameObject>() { enemy });
        }
        //add all chests to the disposable objects collection
        foreach (var chest in this.chests)
        {
            if (disposableObjects.ContainsKey(chest.name))
            {
                this.disposableObjects[chest.name].Add(chest);
            }
            else this.disposableObjects.Add(chest.name, new List<GameObject>() { chest });
        }
        //adds all health potions to the disposable objects collection
        foreach (var potion in GameObject.FindGameObjectsWithTag("HealthPotion"))
        {
            if (disposableObjects.ContainsKey(potion.name))
            {
                this.disposableObjects[potion.name].Add(potion);
            }
            else this.disposableObjects.Add(potion.name, new List<GameObject>() { potion });
        }
        //adds all mana potions to the disposable objects collection
        foreach (var potion in GameObject.FindGameObjectsWithTag("ManaPotion"))
        {
            if (disposableObjects.ContainsKey(potion.name))
            {
                this.disposableObjects[potion.name].Add(potion);
            }
            else this.disposableObjects.Add(potion.name, new List<GameObject>() { potion });
        }
    }

    void FixedUpdate()
    {
        if (!this.gameEnded)
        {
            
            if (Time.time > this.nextUpdateTime)
            {
                this.nextUpdateTime = Time.time + GameConstants.UPDATE_INTERVAL;
                this.Character.baseStats.Time += GameConstants.UPDATE_INTERVAL;
            }


            this.HPText.text = "HP: " + this.Character.baseStats.HP;
            this.XPText.text = "XP: " + this.Character.baseStats.XP;
            this.ShieldHPText.text = "Shield HP: " + this.Character.baseStats.ShieldHP;
            this.LevelText.text = "Level: " + this.Character.baseStats.Level;
            this.TimeText.text = "Time: " + this.Character.baseStats.Time;
            this.ManaText.text = "Mana: " + this.Character.baseStats.Mana;
            this.MoneyText.text = "Money: " + this.Character.baseStats.Money;

            if (this.Character.baseStats.HP <= 0 || this.Character.baseStats.Time >= GameConstants.TIME_LIMIT)
            {
                this.GameEnd.SetActive(true);
                this.gameEnded = true;
                //Debug.Log(Character.MaxIterations);
                StartGame();
                this.GameEnd.GetComponentInChildren<Text>().text = "You Died";
            }
            else if (this.Character.baseStats.Money >= 25)
            {
                this.GameEnd.SetActive(true);
                this.gameEnded = true;
                //Debug.Log(Character.MaxIterations);
                StartGame();
                this.GameEnd.GetComponentInChildren<Text>().text = "Victory \n GG EZ";
            }

            if (Formations != null)
            {
                foreach (var fm in Formations)
                {
                    if (fm.SlotAssignment.Count != 0 && (fm.SlotAssignment.Keys.First().usingFormation || fm.SlotAssignment.Keys.Last().usingFormation))
                        fm.UpdateSlots();
                }
            }
        }
        else
        {
            StartGame();
        }
    }

    public void SwordAttack(GameObject enemy)
    {
        int damage = 0;

        Monster.EnemyStats enemyData = enemy.GetComponent<Monster>().enemyStats;

        if (enemy != null && enemy.activeSelf && InMeleeRange(enemy))
        {
            this.Character.AddToDiary(" I Sword Attacked " + enemy.name);

            if (Formations != null)
            {
                foreach (var fm in Formations)
                {
                    if (enemy.GetComponent<Orc>() != null && fm.SlotAssignment.ContainsKey(enemy.GetComponent<Orc>()))
                    {
                        fm.RemoveCharacter(enemy.GetComponent<Orc>());
                        fm.SlotAssignment.Remove(enemy.GetComponent<Orc>());
                    }
                }
            }

            if (this.StochasticWorld)
            {
                damage = enemy.GetComponent<Monster>().DmgRoll.Invoke();

                //attack roll = D20 + attack modifier. Using 7 as attack modifier (+4 str modifier, +3 proficiency bonus)
                int attackRoll = RandomHelper.RollD20() + 7;

                if (attackRoll >= enemyData.AC)
                {
                    if (enemy.GetComponent<Monster>().usingFormation)
                    {
                        foreach (var fm in Formations)
                        {
                            fm.RemoveCharacter(enemy.GetComponent<Orc>());
                            fm.SlotAssignment.Remove(enemy.GetComponent<Orc>());
                        }
                    }
                    //there was an hit, enemy is destroyed, gain xp
                    this.enemies.Remove(enemy);
                    this.disposableObjects.Remove(enemy.name);
                    enemy.SetActive(false);
                }
            }
            else
            {
                if (enemy.GetComponent<Monster>().usingFormation)
                {
                    foreach (var fm in Formations)
                    {
                        fm.RemoveCharacter(enemy.GetComponent<Orc>());
                        fm.SlotAssignment.Remove(enemy.GetComponent<Orc>());
                    }
                }
                damage = enemyData.SimpleDamage;
                this.enemies.Remove(enemy);
                this.disposableObjects.Remove(enemy.name);
                enemy.SetActive(false);
            }

            this.Character.baseStats.XP += enemyData.XPvalue;

            int remainingDamage = damage - this.Character.baseStats.ShieldHP;
            this.Character.baseStats.ShieldHP = Mathf.Max(0, this.Character.baseStats.ShieldHP - damage);

            if (remainingDamage > 0)
            {
                this.Character.baseStats.HP -= remainingDamage;
            }

            this.WorldChanged = true;
            if (Sword != null) Sword.Play();
        }
    }

    public void EnemyAttack(GameObject enemy)
    {
        if (Time.time > this.enemyAttackCooldown)
        {
            int damage = 0;

            Monster monster = enemy.GetComponent<Monster>();
            if (Formations != null)
            {
                foreach (var fm in Formations)
                {
                    if (enemy.GetComponent<Orc>() != null && fm.SlotAssignment.ContainsKey(enemy.GetComponent<Orc>()))
                    {
                        fm.RemoveCharacter(enemy.GetComponent<Orc>());
                        fm.SlotAssignment.Remove(enemy.GetComponent<Orc>());
                    }
                }
            }

            if (enemy.activeSelf && monster.InWeaponRange(GameObject.FindGameObjectWithTag("Player")))
            {

                this.Character.AddToDiary(" I was Attacked by " + enemy.name);
                this.enemyAttackCooldown = Time.time + GameConstants.UPDATE_INTERVAL;

                if (this.StochasticWorld)
                {
                    damage = monster.DmgRoll.Invoke();

                    //attack roll = D20 + attack modifier. Using 7 as attack modifier (+4 str modifier, +3 proficiency bonus)
                    int attackRoll = RandomHelper.RollD20() + 7;

                    if (attackRoll >= monster.enemyStats.AC)
                    {
                        if (enemy.GetComponent<Monster>().usingFormation)
                        {
                            foreach (var fm in Formations)
                            {
                                fm.RemoveCharacter(enemy.GetComponent<Orc>());
                                fm.SlotAssignment.Remove(enemy.GetComponent<Orc>());
                            }
                        }
                        //there was an hit, enemy is destroyed, gain xp
                        this.enemies.Remove(enemy);
                        this.disposableObjects.Remove(enemy.name);
                        enemy.SetActive(false);
                    }
                }
                else
                {
                    if (enemy.GetComponent<Monster>().usingFormation)
                    {
                        foreach (var fm in Formations)
                        {
                            fm.RemoveCharacter(enemy.GetComponent<Orc>());
                            fm.SlotAssignment.Remove(enemy.GetComponent<Orc>());
                        }
                    }
                    damage = monster.enemyStats.SimpleDamage;
                    this.enemies.Remove(enemy);
                    this.disposableObjects.Remove(enemy.name);
                    enemy.SetActive(false);
                }

                this.Character.baseStats.XP += monster.enemyStats.XPvalue;

                int remainingDamage = damage - this.Character.baseStats.ShieldHP;
                this.Character.baseStats.ShieldHP = Mathf.Max(0, this.Character.baseStats.ShieldHP - damage);

                if (remainingDamage > 0)
                {
                    this.Character.baseStats.HP -= remainingDamage;
                    this.Character.AddToDiary(" I was wounded with " + remainingDamage + " damage");
                }

                this.WorldChanged = true;

                if (Sword != null) Sword.Play();
            }
        }
    }

    public void DivineSmite(GameObject enemy)
    {
        if (enemy != null && enemy.activeSelf && InDivineSmiteRange(enemy) && this.Character.baseStats.Mana >= 2)
        {
            if (enemy.tag.Equals("Skeleton"))
            {
                this.Character.baseStats.XP += 3;
                this.Character.AddToDiary(" I Smited " + enemy.name);
                this.enemies.Remove(enemy);
                this.disposableObjects.Remove(enemy.name);
                enemy.SetActive(false);
            }
            this.Character.baseStats.Mana -= 2;

            this.WorldChanged = true;

            if (this.Smite != null) Smite.Play();
        }
    }

    public void ShieldOfFaith()
    {
        if (this.Character.baseStats.Mana >= 5)
        {
            this.Character.baseStats.ShieldHP = 5;
            this.Character.baseStats.Mana -= 5;
            this.Character.AddToDiary(" My Shield of Faith will protect me!");
            this.WorldChanged = true;

            if(this.Shield != null) Shield.Play();
            
        }
    }

    public void PickUpChest(GameObject chest)
    {

        if (chest != null && chest.activeSelf && InChestRange(chest))
        {
            this.Character.AddToDiary(" I opened  " + chest.name);
            this.chests.Remove(chest);
            this.disposableObjects.Remove(chest.name);
            chest.SetActive(false);
            this.Character.baseStats.Money += 5;
            this.WorldChanged = true;
            if(Open != null) Open.Play();
        }
    }


    public void GetManaPotion(GameObject manaPotion)
    {
        if (manaPotion != null && manaPotion.activeSelf && InPotionRange(manaPotion))
        {
            this.Character.AddToDiary(" I drank " + manaPotion.name);
            this.disposableObjects.Remove(manaPotion.name);
            manaPotion.SetActive(false);
            this.Character.baseStats.Mana = 10;
            this.WorldChanged = true;
            if(Drink != null) Drink.Play();
        }
    }

    public void GetHealthPotion(GameObject potion)
    {
        if (potion != null && potion.activeSelf && InPotionRange(potion))
        {
            this.Character.AddToDiary(" I drank " + potion.name);
            this.disposableObjects.Remove(potion.name);
            potion.SetActive(false);
            this.Character.baseStats.HP = this.Character.baseStats.MaxHP;
            this.WorldChanged = true;
            if(Drink == null) Drink.Play();
        }
    }

    public void LevelUp()
    {
        if (this.Character.baseStats.Level >= 4) return;

        if (this.Character.baseStats.XP >= this.Character.baseStats.Level * 10)
        {
            if (!this.Character.LevelingUp)
            {
                this.Character.AddToDiary(" I am trying to level up...");
                this.Character.LevelingUp = true;
                this.Character.StopTime = Time.time + AutonomousCharacter.LEVELING_INTERVAL;
            }
            else if (this.Character.StopTime < Time.time)
            { 
                this.Character.baseStats.Level++;
                this.Character.baseStats.MaxHP += 10;
                this.Character.baseStats.XP = 0;
                this.Character.AddToDiary(" I leveled up to level " + this.Character.baseStats.Level);
                this.Character.LevelingUp = false;
                this.WorldChanged = true;
                if (Level != null) Level.Play();
            }
        }
    }

    public void LayOnHands()
    {
        if (this.Character.baseStats.Level >= 2 && this.Character.baseStats.Mana >= 7)
        {
            this.Character.AddToDiary(" With my Mana I Lay Hands and recovered all my health.");
            this.Character.baseStats.HP = this.Character.baseStats.MaxHP;
            this.Character.baseStats.Mana -= 7;
            this.WorldChanged = true;
        }
    }

    public void DivineWrath()
    {
        if (this.Character.baseStats.Level >= 3 && this.Character.baseStats.Mana >= 10)
        {
            //kill all enemies in the map
            foreach (var enemy in this.enemies)
            {
                this.Character.baseStats.XP += enemy.GetComponent<Monster>().enemyStats.XPvalue;
                this.Character.AddToDiary(" I used the Divine Wrath and all monsters were killed! \nSo ends a day's work...");
                enemy.SetActive(false);
                this.disposableObjects.Remove(enemy.name);
            }

            enemies.Clear();
            this.WorldChanged = true;
        }
    }

    public void Rest()
    {
        if (!this.Character.Resting)
        {
            this.Character.AddToDiary(" I am resting");
            this.Character.Resting = true;
            this.Character.StopTime = Time.time + AutonomousCharacter.RESTING_INTERVAL;
            if (Sleep != null && !Sleep.isPlaying)
            {
                Sleep.Play();
            }
        }
        else if (this.Character.StopTime < Time.time)
        {
            this.Character.baseStats.HP += AutonomousCharacter.REST_HP_RECOVERY;
            this.Character.baseStats.HP = Mathf.Min(this.Character.baseStats.HP, this.Character.baseStats.MaxHP);
            this.Character.Resting = false;
            this.WorldChanged = true;
        }
    }

    public void Teleport()
    {
        if (this.Character.baseStats.Level >= 2 && this.Character.baseStats.Mana >= 5)
        {
            this.Character.AddToDiary(" With my Mana I teleported away from danger.");
            this.Character.transform.position = this.initialPosition;
            this.Character.baseStats.Mana -= 5;
            this.WorldChanged = true;
            if (Flash != null) Flash.Play();    
        }

    }


    private bool CheckRange(GameObject obj, float maximumSqrDistance)
    {
        var distance = (obj.transform.position - this.Character.gameObject.transform.position).sqrMagnitude;
        return distance <= maximumSqrDistance;
    }


    public bool InMeleeRange(GameObject enemy)
    {
        return this.CheckRange(enemy, GameConstants.PICKUP_RANGE);
    }

    public bool InDivineSmiteRange(GameObject enemy)
    {
        return this.CheckRange(enemy, GameConstants.PICKUP_RANGE * 10);
    }

    public bool InChestRange(GameObject chest)
    {

        return this.CheckRange(chest, GameConstants.PICKUP_RANGE);
    }

    public bool InPotionRange(GameObject potion)
    {
        return this.CheckRange(potion, GameConstants.PICKUP_RANGE);
    }

}
