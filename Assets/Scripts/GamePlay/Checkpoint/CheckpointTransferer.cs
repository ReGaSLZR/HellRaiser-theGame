namespace GamePlay.Checkpoint
{

    using UnityEngine;
    using Zenject;

    public class CheckpointTransferer : MonoBehaviour
    {

        [Inject]
        private readonly CheckpointModel.Setter m_checkpointModel;

        private void Start()
        {
            m_checkpointModel.TransferToMarker(this);
            Destroy(this);
        }

    }

}