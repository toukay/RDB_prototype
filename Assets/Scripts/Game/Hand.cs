using System.Collections.Generic;
using Cards.StateMachine;
using UnityEngine;

namespace Game
{
    public class Hand : MonoBehaviour
    {
        
        [SerializeField] float bendAngle = 20f;
        [SerializeField] float bendHeight = 0.12f;
        [SerializeField] float bendSpacing = -2f;

        [SerializeField] private GameObject cardPrefab;
        
        [SerializeField] private Transform pivot;
        
        private float _fullAngle;
        private float _pivotLocationFactor;
        private float _cardWidth;
        private const int OffsetZ = -1;
        
        /// <summary>
        ///     List with all cards.
        /// </summary>
        public List<Card> Cards { get; private set; }
        
        
        private void Awake()
        {
            Cards = new List<Card>();

            if (cardPrefab != null)
            {
                SpriteRenderer spriteRenderer = cardPrefab.GetComponentInChildren<SpriteRenderer>();
                
                if (spriteRenderer != null)
                {
                    _cardWidth = spriteRenderer.bounds.size.x;
                }
            }
            
            _fullAngle = bendAngle;
            _pivotLocationFactor = CloserEdge(pivot, Camera.main, Screen.width, Screen.height);
        }
        
        
        /// <summary>
        ///     Play the card.
        /// </summary>
        /// <param name="card"></param>
        public void PlayCard(Card card)
        {
            RemoveCard(card);
            SortHand();
            BendHand();
        }
        
        /// <summary>
        ///     Add a card to the hand.
        /// </summary>
        /// <param name="card"></param>
        public virtual void AddCard(Card card)
        {

            Cards.Add(card);
            SortHand();
            BendHand();
        }
        
        /// <summary>
        ///     Remove a card from the hand.
        /// </summary>
        /// <param name="card"></param>
        public virtual void RemoveCard(Card card)
        {
            Cards.Remove(card);
            SortHand();
            BendHand();
        }
        
        /// <summary>
        ///     Clear all hand.
        /// </summary>
        protected virtual void ClearHand()
        {
            Card[] childCards = GetComponentsInChildren<Card>();
            foreach (var uiCardHand in childCards)
                Destroy(uiCardHand.gameObject);

            Cards.Clear();
        }
        
        private void SortHand()
        {
            if (Cards.Count <= 0) return;
            
            float layerZ = 0;
            foreach (var card in Cards)
            {
                Vector3 localCardPosition = card.transform.localPosition;
                localCardPosition.z = layerZ;
                card.transform.localPosition = localCardPosition;
                layerZ += OffsetZ;
            }
        }
        
        private void BendHand()
        {
            if (Cards.Count <= 0) return;
            
            float anglePerCard = _fullAngle / Cards.Count;
            float firstAngle = CalcFirstAngle(_fullAngle);
            float handWidth = CalcHandWidth(Cards.Count);

            //calc first position of the offset on X axis
            float offsetX = pivot.position.x - handWidth / 2;

            for (var i = 0; i < Cards.Count; i++)
            {
                Card card = Cards[i];

                //set card Z angle
                float angleTwist = (firstAngle + i * anglePerCard) * _pivotLocationFactor;
                
                //calc x position
                float xPos = offsetX + _cardWidth / 2;

                //calc y position
                float yDistance = Mathf.Abs(angleTwist) * bendHeight;
                float yPos = pivot.position.y - (yDistance * _pivotLocationFactor);
                
                //set position
                if (!card.IsDragging && !card.IsHovering)
                {
                    int zAxisRot = _pivotLocationFactor == 1 ? 0 : 180;
                    Vector3 rotation = new Vector3(0, 0, angleTwist - zAxisRot);
                    Vector3 position = new Vector3(xPos, yPos, card.transform.position.z);
                    card.RotateTo(rotation);
                    card.MoveTo(position, card.HandMovementSpeed);
                }

                //increment offset
                offsetX += _cardWidth + bendSpacing;
            }
        }
        
        /// <summary>
        ///     Calculus of the angle of the first card.
        /// </summary>
        /// <param name="fullAngle"></param>
        /// <returns></returns>
        private static float CalcFirstAngle(float fullAngle)
        {
            float magicMathFactor = 0.1f;
            return -(fullAngle / 2) + fullAngle * magicMathFactor;
        }

        /// <summary>
        ///     Calculus of the width of the player's hand.
        /// </summary>
        /// <param name="quantityOfCards"></param>
        /// <returns></returns>
        private float CalcHandWidth(int quantityOfCards)
        {
            float widthCards = quantityOfCards * _cardWidth;
            float widthSpacing = (quantityOfCards - 1) * bendSpacing;
            return widthCards + widthSpacing;
        }
        
        public int CloserEdge(Transform transform, Camera camera, int width, int height)
        {
            //edge points according to the screen/camera
            Vector3 worldPointTop = camera.ScreenToWorldPoint(new Vector3(width / 2, height));
            Vector3 worldPointBot = camera.ScreenToWorldPoint(new Vector3(width / 2, 0));

            //distance from the pivot to the screen edge
            float deltaTop = Vector2.Distance(worldPointTop, transform.position);
            float deltaBottom = Vector2.Distance(worldPointBot, transform.position);

            return deltaBottom <= deltaTop ? 1 : -1;
        }
    }
}