using UnityEngine;
using Utils;

namespace Common.Randomizer {

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRandomizer : MonoBehaviour
    {

        private SpriteRenderer m_compSpriteRenderer;

        [SerializeField]
        private Sprite[] m_randomTextures;

        private void Awake()
        {
            if (m_randomTextures.Length == 0) {
                LogUtil.PrintWarning(this, GetType(), "Random textures variable is empty!");
                Destroy(this);
            }

            m_compSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            m_compSpriteRenderer.sprite = m_randomTextures[Random.Range(0, m_randomTextures.Length)];
            Destroy(this);
        }
    }

}