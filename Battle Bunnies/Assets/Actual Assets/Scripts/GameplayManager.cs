using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    struct Monster
    {
        public string name { get; private set; }
        public int hp { get; set; }
        public Ability[] abilities { get; private set; }

        public Monster(string name, int hp, params Ability[] abilities)
        {
            this.name = name;
            this.hp = hp;
            this.abilities = abilities;
        }
    }

    struct Ability
    {
        public enum Type { Fire, Water, Earth, Normal };

        public string name { get; private set; }
        public Type type { get; private set; }
        public int damage { get; private set; }
        public int hitPercent { get; private set; }
        public int critChance { get; private set; }

        public Ability(string name, Type type, int damage, int hitPercent, int critChance)
        {
            this.name = name;
            this.type = type;
            this.damage = damage;
            this.hitPercent = hitPercent;
            this.critChance = critChance;
        }
    }

    class PlayableMonster
    {
        public Monster monster;
        public int id;
        public GameObject sceneObject;
        public bool isSpawned;

        public PlayableMonster(Monster monster, int id, GameObject sceneObject = null)
        {
            this.monster = monster;
            this.id = id;
            this.sceneObject = sceneObject;
            isSpawned = false;
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------- //

    int currentTurnId = 0;
    bool gameOver = false;

    public GameObject[] monsterSceneObjects;

    PlayableMonster[] monsters =
    {
        new PlayableMonster(
            new Monster("Volcanic Bunny", 100,
            new Ability("Normal Ability 1", Ability.Type.Normal, 10, 100, 33),
            new Ability("Normal Ability 2", Ability.Type.Normal, 20, 66, 20),
            new Ability("Fire Ability 1", Ability.Type.Fire, 15, 75, 33),
            new Ability("Fire Ability 2", Ability.Type.Fire, 50, 20, 20)),
            0),

        new PlayableMonster(
            new Monster("Water Bunny", 100,
            new Ability("Normal Ability 1", Ability.Type.Normal, 10, 100, 33),
            new Ability("Normal Ability 2", Ability.Type.Normal, 20, 66, 20),
            new Ability("Water Ability 1", Ability.Type.Fire, 15, 75, 33),
            new Ability("Water Ability 2", Ability.Type.Fire, 50, 20, 20)),
            0)
    };

    public void Start()
    {
        for (int i = 0; i < monsters.Length && i < monsterSceneObjects.Length; i++) {
            monsters[i].sceneObject = monsterSceneObjects[i];
            SetupSceneObject(monsters[i].sceneObject);
        }
    }

    public void Update()
    {
        
    }

    // --------------------------------------------------------------------------------------------------------------------------- //

    private PlayableMonster FindMonster(int id)
    {
        for (int i = 0; i < monsters.Length; i++) {
            if (monsters[i].id == id)
                return monsters[i];
        }

        return null;
    }

    private void SetupSceneObject(GameObject sceneObject)
    {
        // TODO: Implement
    }

    public void SpawnMonster(int id)
    {
        PlayableMonster m = FindMonster(id);

        if (m != null) {
            m.isSpawned = true;
        }
    }

    public void PlayerMonsterAttack(int abilityIndex)
    {
        MonsterAttack(0, abilityIndex);
    }

    public void MonsterAttack(int id, int abilityIndex)
    {
        if (id != currentTurnId || gameOver == true) {
            return;
        }

        PlayableMonster m = FindMonster(id);

        if (m != null && m.isSpawned) {
            PlayableMonster target = monsters[(id + 1) % monsters.Length];
            
            Ability usedAbility = m.monster.abilities[abilityIndex];
            bool attackSuccess = Random.Range(1, 100) < usedAbility.hitPercent;

            if (attackSuccess) {
                float damageMultiplier = Random.Range(1, 100) < usedAbility.critChance ? 1.5f : 1.0f;
                target.monster.hp -= (int)(usedAbility.damage * damageMultiplier);
                target.sceneObject.GetComponent<BunnyBehaviour>().PlayAnimation(2);

                if (target.monster.hp <= 0) {
                    target.monster.hp = 0;
                    MonsterKill(id);
                }
            }

            //currentTurnId = target.id;
            m.sceneObject.GetComponent<BunnyBehaviour>().PlayAnimation(1);
        }
    }

    private void MonsterKill(int id)
    {
        // Play death animation - include despawn functionaility in monster script.
        monsters[id].sceneObject.GetComponent<BunnyBehaviour>().Die();
        gameOver = true;
    }
}
