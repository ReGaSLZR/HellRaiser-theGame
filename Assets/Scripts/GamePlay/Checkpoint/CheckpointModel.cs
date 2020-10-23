namespace GamePlay.Checkpoint
{

    using UnityEngine;
    using Data.Storage;

    public class CheckpointModel : MonoBehaviour,
        CheckpointModel.Setter,
        CheckpointModel.Getter
    {

        #region Interfaces

        public interface Setter
        {
            void ClearCheckpoint();
            void SaveCheckpoint(int markerInstanceID, Vector3 spawnPoint);
            void TransferToMarker(CheckpointTransferer transferer);
        }

        public interface Getter
        {
            bool IsCheckpoint(int markerInstanceID);
            bool IsLevelCheckpointTriggered();
        }

        #endregion

        private PlayerData.Checkpoint m_cachedCheckpoint;

        private void Awake()
        {
            m_cachedCheckpoint = PlayerData.LoadCheckpoint();
        }

        public void ClearCheckpoint()
        {
            PlayerData.Clear(PlayerData.Checkpoint.SAVE_PATH);
        }

        public void SaveCheckpoint(int markerInstanceID, Vector3 spawnPoint)
        {
            m_cachedCheckpoint = new PlayerData.Checkpoint(
                    SceneData.GetCurrentSceneIndex(),
                    markerInstanceID,
                    spawnPoint);
            PlayerData.Save(m_cachedCheckpoint);
        }

        public void TransferToMarker(CheckpointTransferer transferer)
        {
            if (SceneData.GetCurrentSceneIndex() == m_cachedCheckpoint.m_missionIndex)
            {
                transferer.gameObject.transform.position
                    = m_cachedCheckpoint.GetSpawnPoint();
            }
        }

        public bool IsCheckpoint(int markerInstanceID)
        {
            return IsLevelCheckpointTriggered()
                && (markerInstanceID == m_cachedCheckpoint.m_checkpointID);
        }

        public bool IsLevelCheckpointTriggered()
        {
            return (SceneData.GetCurrentSceneIndex() == m_cachedCheckpoint.m_missionIndex);
        }

    }

}