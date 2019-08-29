using UnityEngine;
using NaughtyAttributes;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New Merchant Item", menuName = "HellRaiser/Create Merchant Item")]
    public class MerchantItem : ScriptableObject
    {

        public const string STAT_HEALTH = "STAT_HEALTH";
        public const string STAT_STAMINA = "STAT_STAMINA";
        private readonly string[] m_dropdownStatOptions = new string[] {
            "<Unset>",
            STAT_HEALTH,
            STAT_STAMINA
        };

        public string m_spielButtonNormal = "Buy then use";
        public string m_spielButtonBuyAgain = "Buy Again";
        public string m_spielButtonDisabled = "Not enough funds.";
        public string m_spielButtonOutOfStock = "Out of Stock";

        [Space]

        [ShowAssetPreview]
        public Texture2D m_itemIcon;

        public string m_spielTitle;

        [TextArea]
        public string m_spielDescription;

        [Space]

        [Dropdown("m_dropdownStatOptions")]
        public string m_stat;

        [Range(1, 100)]
        public int m_value;

        [Space]

        [Range(1, 9999)]
        public int m_price;

        [Range(1, 5)]
        public int m_stocks;

    }

}