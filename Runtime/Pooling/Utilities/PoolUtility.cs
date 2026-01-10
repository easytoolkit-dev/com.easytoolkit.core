namespace EasyToolKit.Core.Pooling
{
    public static class PoolUtility
    {
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
                if (!PoolManagerFactory.DefaultObjectPoolManager.TryGetPool(typeof(T).FullName, out Pool))
                {
                    Pool = PoolManagerFactory.DefaultObjectPoolManager.BuildPool<T>(typeof(T).FullName).Create();
                }
            }
        }

        public static T RentObject<T>() where T : class, new()
        {
            return PoolFastGetter<T>.Pool.Rent();
        }

        public static void ReleaseObject<T>(T obj) where T : class, new()
        {
            PoolFastGetter<T>.Pool.Release(obj);
        }
    }
}
