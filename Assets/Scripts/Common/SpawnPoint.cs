using NaughtyAttributes;
using UnityEngine;

namespace Common {

    public class SpawnPoint : MonoBehaviour
    {

        private readonly DropdownList<Vector2> m_dropdownListDirection = new DropdownList<Vector2>() {
            { "Zero", Vector2.zero },
            { "Right", Vector2.right },
            { "Left", Vector2.left},
            { "Up", Vector2.up },
            { "Down", Vector2.down }
        };

        [Dropdown("m_dropdownListDirection")]
        public Vector2 m_direction;

    }

}