using UnityEngine;

namespace GamePlayInput {

    public class InputOnScreenButtons : BaseInputModel
    {

        [SerializeField]
        private string teststring;

        private void Awake()
        {

        }

        protected override void SetInputType()
        {
            m_inputType = InputType.OnScreenButtons;
        }

    }

}