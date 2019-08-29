using UnityEngine;
using UniRx;
using Zenject;
using GamePlay.Stats;
using UnityEngine.UI;
using Utils;
using GamePlay.Mission;

namespace GamePlay.UI.Merchant {

    public class GamePlayMerchantPanel : MonoBehaviour
    {

        [Inject]
        private readonly MerchantModel.Getter m_modelMerchantGetter;
        [Inject]
        private readonly MerchantModel.Setter m_modelMerchantSetter;
        [Inject]
        private readonly MissionModel.TimerSetter m_modelTimer;

        [SerializeField]
        private GamePlayMerchantButton[] m_buttonsInOrder;

        [SerializeField]
        private Button m_buttonClose;

        private void Start()
        {
            //pause/start the timer when Player is/isn't viewing this panel
            m_modelMerchantGetter.IsViewingMerchantGoods()
                .Subscribe(isViewing => {
                    if (isViewing) {
                        m_modelTimer.PauseTimer();
                    }
                    else {
                        m_modelTimer.StartTimer();
                    }   
                })
                .AddTo(this);

            //ping Merchant Model to close this panel upon "close" button click
            m_buttonClose.OnClickAsObservable()
                .Subscribe(_ => m_modelMerchantSetter.SetIsViewingMerchantGoods(false))
                .AddTo(this);

            //update content of Merchant buttons
            m_modelMerchantGetter.GetMerchantItems()
                .Where(items => (items != null))
                .Subscribe(items => {
                    for (int x=0; x<m_buttonsInOrder.Length; x++) {
                        if (items[x] != null)
                        {
                            m_buttonsInOrder[x].SetMerchantItem(items[x]);
                            m_buttonsInOrder[x].gameObject.SetActive(true);
                        }
                        else {
                            LogUtil.PrintError(gameObject, GetType(), "Cannot display NULL MerchantItem.");   
                        }
                    }
                })
                .AddTo(this);

            SetMerchantButtonsEnabled(false);
            m_buttonClose.interactable = true;
        }

        private void SetMerchantButtonsEnabled(bool isEnabled) {
            for (int x = 0; x < m_buttonsInOrder.Length; x++)
            {
                m_buttonsInOrder[x].gameObject.SetActive(isEnabled);
            }
        }

    }

}