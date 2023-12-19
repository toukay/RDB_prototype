using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Starting Deck", menuName = "Deck/Starting")]
    public class StartingDeck : ScriptableObject
    {
        public Stack<CreatureCard> CreatureCards; // TODO: adapt this use any type of card
    }
}