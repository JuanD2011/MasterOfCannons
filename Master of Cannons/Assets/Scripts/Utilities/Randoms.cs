using UnityEngine;
public static class Randoms
{

    /// <summary>
    /// Shuffle an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    public static void Shuffle<T>(ref T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            T aux = array[i];
            int randomIndex = Random.Range(0, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = aux;
        }
    }

    /// <summary>
    /// Takes n random elements from the original array and return them as a new array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="originalArray"></param>
    /// <param name="numRequired">Number of random elements to be choose</param>
    /// <returns></returns>
    public static T[] ChooseRandomElements<T>(T[] originalArray, int numRequired)
    {
        T[] result = new T[numRequired];

        int numToChoose = numRequired;

        for (int numAvailable = originalArray.Length; numAvailable > 0; numAvailable--)
        {
            float prob = (float)numToChoose/(float)numAvailable;
            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = originalArray[numAvailable - 1];
            }

            if (numToChoose == 0) break;
        }

        return result;
    }

}
