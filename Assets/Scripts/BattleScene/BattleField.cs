using Cards;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleScene
{
    public class BattleField : MonoBehaviour
    {
        [SerializeField] private Transform player1Sectors;
        [SerializeField] private Transform player2Sectors;
    
        private FieldSector[,] _sectors = new FieldSector[2, 6];

        private void Start()
        {
            InitializeSectors();
        }

        private void InitializeSectors()
        {
            for (var i = 0; i < 6; i++)
            {
                var sectors = player1Sectors.GetComponentsInChildren<FieldSector>();
                _sectors[0, i] = sectors[i];
                _sectors[0, i].Initialize(1, i);
            }
        
            for (var i = 0; i < 6; i++)
            {
                var sectors = player2Sectors.GetComponentsInChildren<FieldSector>();
                _sectors[1, i] = sectors[i];
                _sectors[1, i].Initialize(2, i);
            }
        }
    }
}