using System.Collections.Generic;

public static class ArrayConverter
{
    public static Queue<T> ConvertArrayToQueue<T>(T[] array)
    {
        var queue = new Queue<T>();

        for (int i = 0; i < array.Length; i++)
        {
            queue.Enqueue(array[i]);
        }

        return queue;
    }
}