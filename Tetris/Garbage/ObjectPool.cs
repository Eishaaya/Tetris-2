using System;
using System.Collections.Generic;

namespace Tetris
{
    public interface IPoolable { }
    public class NotMyObjectException : Exception
    {
        public override string Message => "Alien object submitted into the pool";
    }

    sealed class ObjectPool<T>
        where T : IPoolable
    {
        #region Singleton
        public static ObjectPool<T> Instance { get; } = new ObjectPool<T>();
        static ObjectPool() { }
        private ObjectPool() { }
        #endregion Singleton

        private Dictionary<Type, (Func<T> createFunc, Queue<T> objects)> objectMap = new Dictionary<Type, (Func<T>, Queue<T>)>();
        private HashSet<T> borrowedObjects = new HashSet<T>();

        public TSubClass Borrow<TSubClass>()
            where TSubClass : T
        {
            Type type = typeof(TSubClass);
            var queue = objectMap[type].objects;

            if (queue.Count <= 0)
            {
                GrowPool(queue, objectMap[type].createFunc, queue.Count + borrowedObjects.Count);
            }

            T @object = queue.Dequeue();
            borrowedObjects.Add(@object);
            return (TSubClass)@object;
        }

        public void Return<TSubClass>(TSubClass @object)
            where TSubClass : T
        {
            if (!borrowedObjects.Contains(@object)) throw new NotMyObjectException();

            borrowedObjects.Remove(@object);
            Type type = typeof(TSubClass);

            objectMap[type].objects.Enqueue(@object);
        }

        public void Populate<TSubClass>(int populationSize, Func<TSubClass> createFunc)
            where TSubClass : T
        {
            Type type = typeof(TSubClass);

            var queue = new Queue<T>();
            Func<T> createFuncWrapper = () => createFunc();

            objectMap[type] = (createFuncWrapper, queue);

            for (int i = 0; i < populationSize; i++)
            {
                GrowPool(queue, createFuncWrapper);
            }
        }

        private void GrowPool(Queue<T> queue, Func<T> createFunc, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                queue.Enqueue(createFunc());
            }
        }

    }
}
