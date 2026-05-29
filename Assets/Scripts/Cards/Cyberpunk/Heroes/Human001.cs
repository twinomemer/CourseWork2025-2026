using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScene;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Cards.Cyberpunk.Heroes
{
    public class Human001 : Hero
    {
        protected override void Awake()
        {
            base.Awake();
            
            Name = "Человек 001";
            Tech = "Cyber";
            Damage = 3;
            MaxHealth = 100;
            IsActive = true;
            Balance = 5;
            Health = MaxHealth;
            IsSpecial = true;
            Spell = "Критический урон";
            CardDescription = "(пас) Имеет 50% шанс нанести дополнительные 40% урона при атаке";
            SpellPower = (decimal)1.4;

            var soundArrays = new SoundArrays();
            soundArrays.soundArray = new List<AudioClip>();
            for (var i = 0; i < 4; i++)
            {
                var fullPath = $"SoundEffects/cyber_attack{i + 1}";
                var sound = Resources.Load<AudioClip>(fullPath);
                if (sound != null)
                    soundArrays.soundArray.Add(sound);
                else
                    Debug.LogError($"Звук не найден: {fullPath}");
            }
            randSounds.Add(soundArrays);
        }
        
        public override void Attack(Card target)
        {
            PlaySound(0, true);
            var n = Random.Range(1, 100);
            if (n <= 50)
            {
                OnCardStateChanged?.Invoke($"Герой {Name} наносит критический удар!");
                var baseDamage = AmplifiedDamage;
                AmplifiedDamage = (int)(AmplifiedDamage * SpellPower);
                base.Attack(target);
                AmplifiedDamage = baseDamage;
            }
            else base.Attack(target);
        }
    }
}
