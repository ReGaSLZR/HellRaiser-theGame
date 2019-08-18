namespace Character.Movement {

    public class IdleMovement : BaseMovement
    {

        private void Start()
        {
            SetMovementSpeed(0f); //supercedes any other call to SetMovementSpeed.
        }

    }

}