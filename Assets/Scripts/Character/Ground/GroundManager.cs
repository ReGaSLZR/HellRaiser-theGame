using UniRx;
using UnityEngine;

namespace Character.Ground {

    public class GroundManager : MonoBehaviour
    {

        private ReactiveProperty<bool> m_reactiveIsOnGround;
        private ReactiveProperty<bool> m_reactiveIsWallHit;
        private ReactiveProperty<bool> m_reactiveIsOnEdge;

        private GroundType m_wallSide;
        private GroundType m_edgeSide;

        private void Awake()
        {
            m_reactiveIsOnGround = new ReactiveProperty<bool>(false);
            m_reactiveIsWallHit = new ReactiveProperty<bool>(false);
            m_reactiveIsOnEdge = new ReactiveProperty<bool>(false);
        }

        public ReactiveProperty<bool> IsOnGround()
        {
            return m_reactiveIsOnGround;
        }

        public ReactiveProperty<bool> IsWallHit()
        {
            return m_reactiveIsWallHit;
        }

        public ReactiveProperty<bool> IsOnEdge() {
            return m_reactiveIsOnEdge;
        }

        public GroundType GetWallSide() {
            return m_wallSide;
        }

        public GroundType GetEdgeSide() {
            return m_edgeSide;
        }

        public void SetDetectedGround(GroundDetector detector, GroundType groundType, bool isGroundDetected)
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
                case (GroundType.Ground_Left):
                case (GroundType.Ground_Right):
                    {
                        m_edgeSide = groundType;
                        m_reactiveIsOnEdge.Value = !isGroundDetected;
                        break;
                    }
                case (GroundType.Wall_Minus):
                case (GroundType.Wall_Plus):
                    {
                        m_wallSide = groundType;
                        m_reactiveIsWallHit.Value = isGroundDetected;
                        break;
                    }
            }


        }

    }

}