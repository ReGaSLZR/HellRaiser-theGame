using NaughtyAttributes;
using UnityEngine;

namespace GamePlayInput {

    public class GamePlayInputController : MonoBehaviour
    {

        [SerializeField]
        [Required]
        private PlaySettings m_playSettings;

        [SerializeField]
        private BaseInputModel[] m_inputTypes;

        public BaseInputModel GetBaseInput()
        {
            for (int x = 0; x < m_inputTypes.Length; x++)
            {
                if (m_playSettings.GetInputType() == m_inputTypes[x].m_inputType)
                {
                    LogUtil.PrintInfo(gameObject, GetType(), "GetBaseInput() type got: " + m_inputTypes[x].m_inputType.ToString());
                    DestroyUnusedInputTypes(x);
                    return m_inputTypes[x];
                }
            }

            LogUtil.PrintWarning(gameObject, GetType(), "GetBaseInput(): Cannot find input type "
                + m_playSettings.GetInputType().ToString() + " from PlaySettings.");
            return null;
        }

        private void DestroyUnusedInputTypes(int excludedIndex)
        {
            for (int x = 0; x < m_inputTypes.Length; x++)
            {
                if (x != excludedIndex)
                {
                    Destroy(m_inputTypes[x].gameObject);
                }
            }

            m_inputTypes = new BaseInputModel[] {m_inputTypes[excludedIndex]};
        }

    }

}