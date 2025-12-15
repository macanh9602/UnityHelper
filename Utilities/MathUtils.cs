using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VTLTools
{
    public static class MathUtils
    {
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetRandomEnum<T>() where T : System.Enum
        {
            System.Array values = System.Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }

        public static List<T> GetAllEnum<T>()
        {

            List<T> enumValues = new();
            foreach (T value in System.Enum.GetValues(typeof(T)))
            {
                enumValues.Add(value);
            }
            return enumValues;
        }

        public static int RandomByWeight(float[] probs)
        {
            float total = probs.Sum();
            float randomPoint = Random.value * total;

            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i]) return i;
                randomPoint -= probs[i];
            }
            return probs.Length - 1;
        }

        public static Vector3 GetRandomPositionInRadius(Vector3 center, float radius)
        {
            return center + Random.insideUnitSphere * radius;
        }

        public static int Fibonacci(int n)
        {
            if (n <= 0)
                return 1;
            if (n == 1)
                return 1;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        public static float ParseFloatWithPoint(string _value)
        {
            if (_value.Contains(","))
                _value = _value.Replace(",", ".");

            if (float.TryParse(_value, out float _result))
            {
                return _result;
            }
            else
            {
                Debug.LogError("Fail to parse: " + _value);
                return 0;
            }
        }

        public static List<T> GetDistinctElements<T>(List<T> list, int n, T _exclude = default)
        {
            var distinctElements = new HashSet<T>();
            var random = new System.Random();

            // Filter out the excluded element if provided and get distinct elements
            var filteredList = new List<T>(list);
            if (!EqualityComparer<T>.Default.Equals(_exclude, default))
            {
                filteredList.RemoveAll(item => item.Equals(_exclude));
            }

            while (distinctElements.Count < n && filteredList.Count > 0)
            {
                var index = random.Next(filteredList.Count);
                var element = filteredList[index];
                distinctElements.Add(element);
            }

            return new List<T>(distinctElements);
        }

        public static bool InsertOrMoveToIndex<T>(List<T> list, T item, int targetIndex)
        {
            if (list == null)
            {
                Debug.LogError("List is null!");
                return false;
            }

            // Clamp targetIndex trong khoảng hợp lệ
            targetIndex = Mathf.Clamp(targetIndex, 0, list.Count);

            // Kiểm tra phần tử đã tồn tại chưa
            int existingIndex = list.IndexOf(item);

            if (existingIndex >= 0)
            {
                // Phần tử đã tồn tại - di chuyển đến vị trí mới
                if (existingIndex == targetIndex)
                {
                    // Đã ở đúng vị trí rồi
                    return false;
                }

                // Xóa khỏi vị trí cũ
                list.RemoveAt(existingIndex);

                // Điều chỉnh targetIndex nếu phần tử cũ ở trước vị trí target
                if (existingIndex < targetIndex)
                {
                    targetIndex--;
                }

                // Chèn vào vị trí mới
                list.Insert(targetIndex, item);
                return false;
            }
            else
            {
                // Phần tử chưa tồn tại - thêm mới
                list.Insert(targetIndex, item);
                return true;
            }
        }

        public static bool MoveToFirst<T>(this List<T> list, T item)
        {
            return InsertOrMoveToIndex(list, item, 0);
        }

        public static bool MoveToLast<T>(this List<T> list, T item)
        {
            if (list == null) return false;
            return InsertOrMoveToIndex(list, item, list.Count);
        }

    }
}