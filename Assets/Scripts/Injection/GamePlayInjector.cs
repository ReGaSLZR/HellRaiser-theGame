using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GamePlayInjector : MonoInstaller<GamePlayInjector>
{

    [SerializeField]
    [Required]
    private AudioModel m_modelAudio;

    public override void InstallBindings()
    {
        Container.Bind<AudioModel.Getter>().FromInstance(m_modelAudio);
        Container.Bind<AudioModel.Setter>().FromInstance(m_modelAudio);

    }


}
