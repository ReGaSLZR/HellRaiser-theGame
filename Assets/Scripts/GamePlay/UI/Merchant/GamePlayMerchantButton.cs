using UnityEngine;
using UnityEngine.UI;
using Zenject;
using GamePlay.Stats;
using UniRx;
using TMPro;
using Scriptables;
using NaughtyAttributes;
using Utils;

namespace GamePlay.UI.Merchant {

    public class GamePlayMerchantButton : MonoBehaviour
    {

        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStatsGetter;
        [Inject]
        private readonly GamePlayStatsModel.Setter m_modelStatsSetter;

        [SerializeField]
        [Required]
        private Button m_button;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textButton;

        [Space]

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textTitle;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textDescription;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textPrice;

        private MerchantItem m_merchantItem;

        public void SetMerchantItem(MerchantItem item)
        {
            m_merchantItem = item;

            SetDefaults();
        }

        private void Start()
        {
            InitObservers();
        }

        private void InitObservers() {
            //update button (enabled/disabled state) and button text on Inventory Money change
            m_modelStatsGetter.GetInventoryMoney()
                .Subscribe(money => {
                    m_button.interactable = (money >= m_merchantItem.m_price);

                    if (!m_button.interactable)
                    {
                        m_textButton.text = m_merchantItem.m_spielButtonDisabled;
                    }
                })
                .AddTo(this);

            //do stuff upon button click
            m_button.OnClickAsObservable()
                .Subscribe(_ => {
                    LogUtil.PrintInfo(gameObject, GetType(), "Bought merchant item: " + m_merchantItem.m_spielTitle);
                    m_textButton.text = m_merchantItem.m_spielButtonBuyAgain;

                    //subtract the money value from inventory
                    m_modelStatsSetter.AddInventoryMoney(m_merchantItem.m_price * -1);

                    //apply the stat change
                    switch (m_merchantItem.m_stat)
                    {
                        default:
                        case MerchantItem.STAT_HEALTH:
                            {
                                m_modelStatsSetter.AddActiveCharacterHealth(m_merchantItem.m_value);
                                break;
                            }
                        case MerchantItem.STAT_STAMINA:
                            {
                                m_modelStatsSetter.AddActiveCharacterStamina(m_merchantItem.m_value);
                                break;
                            }
                    }
                })
                .AddTo(this);
        }

        private void SetDefaults() {
            //configure the text content
            m_textButton.text =
                (m_modelStatsGetter.GetInventoryMoney().Value >= m_merchantItem.m_price) ?
                m_merchantItem.m_spielButtonNormal : m_merchantItem.m_spielButtonDisabled;
            m_textTitle.text = m_merchantItem.m_spielTitle;
            m_textDescription.text = m_merchantItem.m_spielDescription;
            m_textPrice.text = m_merchantItem.m_price.ToString();
        }

    }

}