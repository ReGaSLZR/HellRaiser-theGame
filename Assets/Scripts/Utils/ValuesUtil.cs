using UnityEngine;

namespace Utils {

    public static class ValuesUtil
    {

        public static int GetValueFromVector2Range(Vector2 range)
        {
            return Mathf.RoundToInt(Random.Range(range.x, range.y));
        }


    }


}