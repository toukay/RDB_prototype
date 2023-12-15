using System;
using UnityEngine;
using Data;
using DefaultNamespace;
using TMPro;
using UnityEngine.UI;

namespace Cards
{
    public class CreatureCard : MonoBehaviour, ICard, IDraggable
    {
        [SerializeField] private CreatureCardData cardData;
        
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text descriptionText;
        
        [SerializeField] private Image artworkImage;

        private void Start()
        {
            nameText.text = cardData.name;
            costText.text = cardData.cost.ToString();
            // artworkImage.sprite = cardData.artwork;
            attackText.text = cardData.attack.ToString();
            healthText.text = cardData.health.ToString();
            descriptionText.text = cardData.description;
            
        }

        public void OnStartDrag()
        {
            Debug.Log("YOOO I am getting dragged :D");
        }

        public void OnEndDrag()
        {
            Debug.Log("OW well that was fun :#");
        }
    }
}
