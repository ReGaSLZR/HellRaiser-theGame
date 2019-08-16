using UniRx;
using UnityEngine;

namespace Character.Ground {

    public class PlayerGroundModel : MonoBehaviour, PlayerGroundModel.Getter, PlayerGroundModel.Setter
    {

        #region Interfaces

        public interface Getter
        {
            ReactiveProperty<bool> IsOnGround();
            ReactiveProperty<bool> IsOnWall();
            PlayerGround GetWallSide();
        }

        public interface Setter
        {
            void SetGround(PlayerGround groundType, bool isGroundDetected);
        }

        #endregion

        private ReactiveProperty<bool> m_reactiveIsOnGround;
        private ReactiveProperty<bool> m_reactiveIsOnWall;
        private PlayerGround m_wallSide;

        private void Awake()
        {
            m_reactiveIsOnGround = new ReactiveProperty<bool>(false);
            m_reactiveIsOnWall = new ReactiveProperty<bool>(false);

        }

        private void DisableAllReactives()
        {
            m_reactiveIsOnGround.Value = false;
            m_reactiveIsOnWall.Value = false;
        }

        public ReactiveProperty<bool> IsOnGround()
        {
            return m_reactiveIsOnGround;
        }

        public ReactiveProperty<bool> IsOnWall()
        {
            return m_reactiveIsOnWall;
        }

        public PlayerGround GetWallSide() {
            return m_wallSide;
        }

        public void SetGround(PlayerGround groundType, bool isGroundDetected)
        {

            switch (groundType)
            {
                default:
                case (PlayerGround.Ground):
                    {
                        m_reactiveIsOnGround.Value = isGroundDetected;
                        break;
                    }

                case (PlayerGround.Wall_Left):
                case (PlayerGround.Wall_Right):
                    {
                        m_wallSide = groundType;
                        m_reactiveIsOnWall.Value = isGroundDetected;
                        break;
                    }
            }


        }

    }

}