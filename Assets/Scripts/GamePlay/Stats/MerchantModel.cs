using UnityEngine;
using UniRx;
using Scriptables;

namespace GamePlay.Stats {

    public class MerchantModel : MonoBehaviour, MerchantModel.Getter, MerchantModel.Setter
    {

        #region Interfaces

        public interface Getter {
            ReactiveProperty<bool> IsViewingMerchantGoods();
            ReactiveProperty<MerchantItem[]> GetMerchantItems();
        }

        public interface Setter {
            void SetIsViewingMerchantGoods(bool isViewingMerchantGoods);
            void SetMerchantItems(MerchantItem[] merchantItems);
        }

        #endregion

        private ReactiveProperty<bool> m_isViewingMerchantGoods = new ReactiveProperty<bool>(false);
        private ReactiveProperty<MerchantItem[]> m_merchantItems = new ReactiveProperty<MerchantItem[]>();

        public ReactiveProperty<bool> IsViewingMerchantGoods()
        {
            return m_isViewingMerchantGoods;
        }

        public ReactiveProperty<MerchantItem[]> GetMerchantItems() {
            return m_merchantItems;
        }

        public void SetIsViewingMerchantGoods(bool isViewingMerchantGoods)
        {
            m_isViewingMerchantGoods.Value = isViewingMerchantGoods;
        }

        public void SetMerchantItems(MerchantItem[] merchantItems) {
            m_merchantItems.Value = merchantItems;
        }

    }


}