namespace Injection
{

    using Zenject;
    using Data.Storage;

    public abstract class BaseInjector<T> : MonoInstaller<T>
        where T: MonoInstaller<T>
    {

        private void OnApplicationQuit()
        {
            SceneData.ClearStoredLevel();
        }

    }

}