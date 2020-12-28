using CPC.Redis.Lock;
using System;

namespace CPC.Redis
{
    public static class RedisLockExtension
    {
        public static bool TryLock(this RedisClient client, DistributedLockProfile info, Action action)
        {
            using (var result = CreateLock(client, info))
            {
                if (result.IsAcquired)
                {
                    action?.Invoke();
                    return true;
                }
            }

            return false;
        }


        public static IDistributedLock CreateLock(this RedisClient client, string resource, int sec) => CreateLock(client, new DistributedLockProfile { Resource = resource, ExpiryTime = TimeSpan.FromSeconds(sec) });


        public static IDistributedLock CreateLock(this RedisClient client, DistributedLockProfile profile) => CreateLock(new RedisClient[] { client }, profile);

        public static IDistributedLock CreateLock(this RedisClient[] client, DistributedLockProfile profile) => RedisLock.Create(client, profile);
    }



}
