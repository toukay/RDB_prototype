using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Creature Card", menuName = "Cards/Creature")]
    public class CreatureCardData : ScriptableObject
    {
        public new string name;
        public int cost;
        public Sprite artwork;
        public int attack;
        public int health;
        public string description;
    }
}
