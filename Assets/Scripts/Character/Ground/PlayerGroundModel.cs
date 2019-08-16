using UniRx;
using UnityEngine;

namespace Character.Ground {

    public class PlayerGroundModel : MonoBehaviour, PlayerGroundModel.Getter, PlayerGroundModel.Setter
    {

        #region Interfaces

        public interface Getter
        {
            ReactiveProperty<bool> IsOnGround();
            ReactiveProperty<bool> IsOnWallLeft();
            ReactiveProperty<bool> IsOnWallRight();
        }

        public interface Setter
        {
            void SetGround(PlayerGround groundType, bool isGroundDetected);
        }

        #endregion

        private ReactiveProperty<bool> m_reactiveIsOnGround;
        private ReactiveProperty<bool> m_reactiveIsOnWallLeft;
        private ReactiveProperty<bool> m_reactiveIsOnWallRight;

        private void Awake()
        {
            m_reactiveIsOnGround = new ReactiveProperty<bool>(false);
            m_reactiveIsOnWallLeft = new ReactiveProperty<bool>(false);
            m_reactiveIsOnWallRight = new ReactiveProperty<bool>(false);

        }

        private void DisableAllReactives()
        {
            m_reactiveIsOnGround.Value = false;
            m_reactiveIsOnWallLeft.Value = false;
            m_reactiveIsOnWallRight.Value = false;
        }

        public ReactiveProperty<bool> IsOnGround()
        {
            return m_reactiveIsOnGround;
        }

        public ReactiveProperty<bool> IsOnWallLeft()
        {
            return m_reactiveIsOnWallLeft;
        }

        public ReactiveProperty<bool> IsOnWallRight()
        {
            return m_reactiveIsOnWallRight;
        }

        public void SetGround(PlayerGround groundType, bool isGroundDetected)
        {
            DisableAllReactives();

            switch (groundType)
            {
                default:
                case (PlayerGround.Ground):
                    {
                        m_reactiveIsOnGround.Value = isGroundDetected;
                        break;
                    }
                case (PlayerGround.Wall_Left):
                    {
                        m_reactiveIsOnWallLeft.Value = isGroundDetected;
                        break;
                    }
                case (PlayerGround.Wall_Right):
                    {
                        m_reactiveIsOnWallRight.Value = isGroundDetected;
                        break;
                    }
            }


        }

    }

}