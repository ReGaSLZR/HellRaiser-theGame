namespace Common.UI
{

    using UniRx;
    using UniRx.Triggers;
    using UnityEngine;
    using UnityEngine.UI;

    public class GraphicsHyperlink : MonoBehaviour
    {

        [SerializeField] private MaskableGraphic[] m_graphicsToClick;
        [SerializeField] private string m_link;

        private void Awake()
        {
            if ((m_graphicsToClick.Length == 0) || (m_link == null))
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            foreach (MaskableGraphic graphic in m_graphicsToClick)
            {
                if (graphic != null)
                {
                    graphic.OnPointerClickAsObservable()
                        .Subscribe(_ => Application.OpenURL(m_link))
                        .AddTo(this);
                }
            }
        }

    }

}