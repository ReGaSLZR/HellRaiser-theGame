using Character.Skill;
using GamePlay.Input;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Character.AI {

    public class PlayableAI : BaseAI
    {

        [Inject]
        private BaseInputModel m_modelInput;

        //[SerializeField]
        //[Required]
        //private BaseSkill m_skillSecondary;

        //[SerializeField]
        //[Required]
        //private BaseSkill m_skillTertiary;

        private void Awake()
        {
            if(m_targetDetector != null) {
                LogUtil.PrintWarning(gameObject, GetType(), "Awake(): TargetDetector is not needed in PlayerAI.");
                m_targetDetector = null;
            }
        }

        protected override void Start()
        {
            base.Start();
            InitInputObservers();
        }

        private void InitInputObservers() {
            //skill MAIN
            this.FixedUpdateAsObservable()
                .Select(_ => m_modelInput.m_skillMain)
                .Where(isUsingSkill => isUsingSkill)
                .Subscribe(_ => {
                    m_skillMain.UseSkill();
                })
                .AddTo(this);

            ////skill 2
            //this.FixedUpdateAsObservable()
            //   .Select(_ => m_modelInput.m_skill2)
            //   .Subscribe(isUsingSkill => {
            //       m_skillSecondary.UseSkill();
            //   })
            //   .AddTo(this);

            ////skill 3
            //this.FixedUpdateAsObservable()
            //   .Select(_ => m_modelInput.m_skill3)
            //   .Subscribe(isUsingSkill => {
            //       if (isUsingSkill)
            //       {
            //           m_skillTertiary.UseSkill();
            //       }
            //       else
            //       {
            //           m_skillTertiary.StopSkill(false);
            //       }
                   
            //   })
            //   .AddTo(this);
        }

        protected override void OnDeath() {
            m_skillMain.StopSkill(false);
            //m_skillSecondary.StopSkill(false);
            //m_skillTertiary.StopSkill(false);

            base.OnDeath();
        }

    }

}