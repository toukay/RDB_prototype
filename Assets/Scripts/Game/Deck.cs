using System.Collections;
using Cards.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class Deck : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;

        [SerializeField] private Hand hand;
        
        [SerializeField] private Transform handCards;
        
        [SerializeField] private Transform gameView;
        
        private void DrawCard(PointerEventData obj)
        {
            // CardDrawer.DrawCard();
        }
        
        private IEnumerator Start()
        {
            //starting cards
            for (var i = 0; i < 6; i++)
            {
                yield return new WaitForSeconds(0.2f);
                DrawCard();
            }
        }
        
        public void DrawCard()
        {
            //TODO: Consider replace Instantiate by an Object Pool Pattern
            var cardGo = Instantiate(cardPrefab, gameView);
            // cardGo.name = "Card_" + Count;
            var card = cardGo.GetComponent<Card>();
            card.transform.position = transform.position;
            hand.AddCard(card);
        }
    }
}