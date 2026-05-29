using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards.Biopunk.Heroes
{
    public class BugMarshal : Hero
    {
        protected override void Awake()
        {
            base.Awake();
            
            Name = "Жучий маршал";
            Tech = "Bio";
            Damage = 3;
            MaxHealth = 100;
            IsActive = true;
            Balance = 5;
            Health = MaxHealth;
            IsSpecial = true;
            Spell = "Дополнительное здоровье";
            SpellPower = (decimal)1.2;
            CardDescription = "(пас) Увеличивает здоровье всех союзных карт на 20%";

            var soundArrays = new SoundArrays();
            soundArrays.soundArray = new List<AudioClip>();
            for (var i = 0; i < 4; i++)
            {
                var fullPath = $"SoundEffects/bio_attack{i + 1}";
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
            base.Attack(target);
        }
    }
}
