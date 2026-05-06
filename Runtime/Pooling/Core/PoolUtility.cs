using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Pooling
{
    public static class PoolUtility
    {
        private static readonly ObjectPoolManager DefaultObjectPoolManager = new ObjectPoolManager();

        public static T RentObject<T>() where T : class, new()
        {
            return PoolFastGetter<T>.Pool.Rent();
        }

        public static void ReleaseObject<T>(T obj) where T : class, new()
        {
            PoolFastGetter<T>.Pool.Release(obj);
        }

        /// <summary>
        /// Fast getter for object pools with generic type constraint.
        /// </summary>
        /// <typeparam name="T">The type of object in the pool (must be class and have parameterless constructor).</typeparam>
        private static class PoolFastGetter<T>
            where T : class, new()
        {
            public static readonly IObjectPool<T> Pool;

            static PoolFastGetter()
            {
                if (!DefaultObjectPoolManager.TryGetPool(typeof(T).ToCodeString(TypeFormat.Full), out Pool))
                {
                    Pool = DefaultObjectPoolManager.CreatePool<T>(typeof(T).ToCodeString(TypeFormat.Full));
                }
            }
        }
    }
}
