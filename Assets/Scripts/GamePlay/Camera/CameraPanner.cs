using GamePlay.Input;
using UnityEngine;
using UniRx;
using Zenject;
using Cinemachine;
using Utils;

namespace GamePlay.Camera {

    public class CameraPanner : MonoBehaviour
    {

        [Inject]
        private BaseInputModel m_modelInput;

        [Header("VirtualCam Body Defaults")]

        [SerializeField]
        private float m_screenX;

        [SerializeField]
        private float m_screenY;

        [Header("VirtualCam Body - Screen X-Y modifiers")]

        [SerializeField]
        [Range(0.1f, 1f)]
        private float m_panHorizontal;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float m_panVertical;

        private CinemachineVirtualCamera[] m_cameras;

        private void Awake()
        {
            m_cameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();

            if (m_cameras.Length == 0) {
                LogUtil.PrintError(gameObject, GetType(), "No Virtual Cameras found. Scene must at least have one. Destroying...");
                Destroy(this);
            }
        }

        private void Start()
        {
            m_modelInput.m_cameraPanDirection
                .Subscribe(direction => Pan(direction))
                .AddTo(this);
        }

        private void Pan(CameraPanDirection direction) {
            CinemachineVirtualCamera activeCam = GetActiveCamera();
            CinemachineFramingTransposer transposer = activeCam.GetCinemachineComponent<CinemachineFramingTransposer>();

            switch (direction) {
                case CameraPanDirection.PAN_UP: {
                        transposer.m_ScreenY = (m_screenY + m_panVertical);
                        break;
                    }
                case CameraPanDirection.PAN_DOWN: {
                        transposer.m_ScreenY = (m_screenY - m_panVertical);
                        break;
                    }
                case CameraPanDirection.PAN_LEFT: {
                        transposer.m_ScreenX = (m_screenX + m_panHorizontal);
                        break;
                    }
                case CameraPanDirection.PAN_RIGHT: {
                        transposer.m_ScreenX = (m_screenX - m_panHorizontal);
                        break;
                    }
                case CameraPanDirection.NO_PAN:
                default: {
                        transposer.m_ScreenX = m_screenX;
                        transposer.m_ScreenY = m_screenY;
                        break;
                }

            }
        }

        private CinemachineVirtualCamera GetActiveCamera() {
            for (int x=0; x<m_cameras.Length; x++) {
                if (m_cameras[x].isActiveAndEnabled) {
                    return m_cameras[x];
                }
            }

            return null;
        }

    }


}