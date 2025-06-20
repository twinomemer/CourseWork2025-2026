using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Biopunk;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScene
{
    public class EnemyDeckManager : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private GridLayoutGroup enemyDeckContainer;
        [SerializeField] private DiscardManager enemyDiscard;
        public List<GameObject> currentEnemyDeck;
        private List<Card> _enemyDeck = new List<Card>();

        private void Start()
        {
            if (IntersceneData.Instance.EnemyID == 1)
            {
                _enemyDeck.Add(gameObject.AddComponent<BiomassSource>());
                _enemyDeck.Add(gameObject.AddComponent<BloodBag>());
                _enemyDeck.Add(gameObject.AddComponent<CockroachTank>());
                _enemyDeck.Add(gameObject.AddComponent<Cavalcade>());
                _enemyDeck.Add(gameObject.AddComponent<FailedExperiment>());
                _enemyDeck.Add(gameObject.AddComponent<Hedge>());
                _enemyDeck.Add(gameObject.AddComponent<KonstantinTheMachineGunner>());
                _enemyDeck.Add(gameObject.AddComponent<ModifiedMan>());
                _enemyDeck.Add(gameObject.AddComponent<MovingMushroomsColony>());
                _enemyDeck.Add(gameObject.AddComponent<Overseer>());
                _enemyDeck.Add(gameObject.AddComponent<Parasite>());
                _enemyDeck.Add(gameObject.AddComponent<PlagueCarrier>());
                _enemyDeck.Add(gameObject.AddComponent<PoisonousPlant>());
                _enemyDeck.Add(gameObject.AddComponent<Predator>());
                _enemyDeck.Add(gameObject.AddComponent<TreeMan>());
            }
            foreach (var cardData in _enemyDeck)
            {
                var cardObj = Instantiate(cardPrefab, enemyDeckContainer.transform);
                cardObj.AddComponent(cardData.GetType());
                cardObj.GetComponent<DraggableCard>().enabled = false;
                currentEnemyDeck.Add(cardObj);
            }
        }

        public void RefreshDeck()
        {
            foreach (var card in enemyDiscard.currentPlayerDiscard.ToList())
            {
                if (!currentEnemyDeck.Contains(card)) currentEnemyDeck.Add(card);
                enemyDiscard.RemoveCardFromDiscard(card);
                            
                card.transform.SetParent(enemyDeckContainer.transform);
                card.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
