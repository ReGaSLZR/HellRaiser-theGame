using UniRx;
using UnityEngine;

namespace Character.Ground {

    public class GroundManager : MonoBehaviour
    {

        private ReactiveProperty<bool> m_reactiveIsOnGround;
        private ReactiveProperty<bool> m_reactiveIsWallHit;
        private GroundType m_wallSide;

        private void Awake()
        {
            m_reactiveIsOnGround = new ReactiveProperty<bool>(false);
            m_reactiveIsWallHit = new ReactiveProperty<bool>(false);

        }

        public ReactiveProperty<bool> IsOnGround()
        {
            return m_reactiveIsOnGround;
        }

        public ReactiveProperty<bool> IsWallHit()
        {
            return m_reactiveIsWallHit;
        }

        public GroundType GetWallSide() {
            return m_wallSide;
        }

        public void SetGround(GroundDetector detector, GroundType groundType, bool isGroundDetected)
        {
            if (detector == null) {
                LogUtil.PrintError(gameObject, GetType(), "SetGround(): detector cannot be NULL.");
                return;
            }

            switch (groundType)
            {
                default:
                case (GroundType.Ground_Center):
                    {
                        m_reactiveIsOnGround.Value = isGroundDetected;
                        break;
                    }

                case (GroundType.Wall_Left):
                case (GroundType.Wall_Right):
                    {
                        m_wallSide = groundType;
                        m_reactiveIsWallHit.Value = isGroundDetected;
                        break;
                    }
            }


        }

    }

}