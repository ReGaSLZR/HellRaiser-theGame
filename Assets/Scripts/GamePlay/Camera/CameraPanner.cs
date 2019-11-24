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
        private CinemachineFramingTransposer[] m_cameraTransposers;

        private void Awake()
        {
            m_cameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();

            if (m_cameras.Length == 0) {
                LogUtil.PrintError(gameObject, GetType(), "No Virtual Cameras found. Scene must at least have one. Destroying...");
                Destroy(this);
            }

            m_cameraTransposers = GetAllCameraTransposers();
        }

        private void Start()
        {
            m_modelInput.m_cameraPanDirection
                .Subscribe(direction => Pan(direction))
                .AddTo(this);
        }

        private void Pan(CameraPanDirection direction) {
            CinemachineFramingTransposer transposer = m_cameraTransposers[GetActiveCameraIndex()];

            if (transposer == null) {
                LogUtil.PrintError(gameObject, GetType(), "Pan(): Could not pan. FramingTransposer from active vCam is NULL.");
                return;
            }

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

        private CinemachineFramingTransposer[] GetAllCameraTransposers() {
            CinemachineFramingTransposer[] transposers = new CinemachineFramingTransposer[m_cameras.Length];

            for (int x=0; x<m_cameras.Length; x++) {
                transposers[x] = m_cameras[x].GetCinemachineComponent<CinemachineFramingTransposer>();
            }

            return transposers;
        }

        private int GetActiveCameraIndex() {
            for (int x=0; x<m_cameras.Length; x++) {
                if (m_cameras[x].isActiveAndEnabled) {
                    return x;
                }
            }

            LogUtil.PrintInfo(gameObject, GetType(), "GetActiveCamera(): Could not get an active vCam. Returning 0");
            m_cameras[0].enabled = true;
            return 0;
        }

    }


}