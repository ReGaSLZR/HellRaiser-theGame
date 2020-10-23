using UnityEngine;
using UnityEngine.UI;
using Zenject;
using GamePlay.Stats;
using UniRx;
using TMPro;
using Scriptables;
using NaughtyAttributes;
using Utils;
using static Scriptables.PlaySettings;
using Audio;

namespace GamePlay.UI.Merchant {

    public class GamePlayMerchantButton : MonoBehaviour
    {

        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStatsGetter;
        [Inject]
        private readonly GamePlayStatsModel.Setter m_modelStatsSetter;
        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;
        [Inject]
        private readonly ColorScheme m_colorScheme;
        [Inject]
        private readonly AudioTheme m_audioTheme;

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
        private TextMeshProUGUI m_textStock;

        [SerializeField]
        [Required]
        private RawImage m_rawImageIcon;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textPrice;

        private MerchantItem m_merchantItem;

        private bool m_hasBoughtItem;

        public void SetMerchantItem(MerchantItem item)
        {
            m_merchantItem = item;
            m_hasBoughtItem = false;

            SetDefaults();
        }

        private void Start()
        {
            m_textPrice.color = m_colorScheme.m_moneyGain;

            InitObservers();
        }

        private void InitObservers() {
            //update button (enabled/disabled state) and button text on Inventory Money change
            m_modelStatsGetter.GetInventoryMoney()
                .Where(money => m_merchantItem != null)
                .Subscribe(money => {
                    UpdateButtonInteractable();
                    UpdateButtonText();
                })
                .AddTo(this);

            //do stuff upon button click
            m_button.OnClickAsObservable()
                .Subscribe(_ => {
                    LogUtil.PrintInfo(gameObject, GetType(), "Bought merchant item: " + m_merchantItem.m_spielTitle);

                    m_modelSFX.PlaySFX(m_merchantItem.m_clipOnPurchase);

                    m_hasBoughtItem = true;
                    m_textButton.text = m_merchantItem.m_spielButtonBuyAgain;

                    //update stocks
                    m_merchantItem.m_stocks--;
                    m_textStock.text = m_merchantItem.m_stocks.ToString();

                    //apply the stat change
                    switch (m_merchantItem.m_stat)
                    {
                        default:
                        case MerchantItem.ItemType.STAT_HEALTH:
                            {
                                m_modelStatsSetter.AddActiveCharacterHealth(m_merchantItem.m_value);
                                break;
                            }
                        case MerchantItem.ItemType.STAT_STAMINA:
                            {
                                m_modelStatsSetter.AddActiveCharacterStamina(m_merchantItem.m_value);
                                break;
                            }
                    }

                    //subtract the money value from inventory
                    m_modelStatsSetter.AddInventoryMoney(m_merchantItem.m_price * -1);
                })
                .AddTo(this);
        }

        private void UpdateButtonInteractable() {
            m_button.interactable = (m_merchantItem.m_stocks > 0)
                    && (m_modelStatsGetter.GetInventoryMoney().Value >= m_merchantItem.m_price);
        }

        private void UpdateButtonText() {
            m_textButton.text = (m_merchantItem.m_stocks <= 0) ? m_merchantItem.m_spielButtonOutOfStock :
                        (m_modelStatsGetter.GetInventoryMoney().Value < m_merchantItem.m_price) ? m_merchantItem.m_spielButtonDisabled :
                        m_hasBoughtItem ? m_merchantItem.m_spielButtonBuyAgain : m_merchantItem.m_spielButtonNormal;
        }

        private void SetDefaults() {
            m_rawImageIcon.texture = m_merchantItem.m_itemIcon;
            UpdateButtonText();
            UpdateButtonInteractable();

            m_textTitle.text = m_merchantItem.m_spielTitle;
            m_textDescription.text = m_merchantItem.m_spielDescription;

            m_textStock.text = m_merchantItem.m_stocks.ToString("N0");
            m_textPrice.text = m_merchantItem.m_price.ToString("N0");
        }

    }

}