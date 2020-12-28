using System;

namespace CPC.Redis
{
    public interface IStringCommand
    {
        /// <summary>
        /// 如果 key 已经存在并且是一个字符串， APPEND 命令将 value 追加到 key 原来的值的末尾。
        /// 如果 key 不存在， APPEND 就简单地将给定 key 设为 value ，就像执行 SET key value 一样。
        /// 时间复杂度：平摊O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>追加 value 之后， key 中字符串的长度。</returns>
        int Append(string key, string value);

        /// <summary>
        /// 将 key 中储存的数字值减一。
        /// 如果 key 不存在，那么 key 的值会先被初始化为 0 ，然后再执行 DECR 操作。
        /// 如果值包含错误的类型，或字符串类型的值不能表示为数字，那么返回一个错误。
        /// 本操作的值限制在 64 位(bit)有符号数字表示之内。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>执行 DECR 命令之后 key 的值。</returns>
        long Decr(string key);

        /// <summary>
        /// 将 key 所储存的值减去减量 decrement 。
        /// 如果 key 不存在，那么 key 的值会先被初始化为 0 ，然后再执行 DECRBY 操作。
        /// 如果值包含错误的类型，或字符串类型的值不能表示为数字，那么返回一个错误。
        /// 本操作的值限制在 64 位(bit)有符号数字表示之内。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns>减去 decrement 之后， key 的值。</returns>
        long DecrBy(string key, long count);

        /// <summary>
        /// 将 key 中储存的数字值增一。
        /// 如果 key 不存在，那么 key 的值会先被初始化为 0 ，然后再执行 INCR 操作。
        /// 如果值包含错误的类型，或字符串类型的值不能表示为数字，那么返回一个错误。
        /// 本操作的值限制在 64 位(bit)有符号数字表示之内。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>执行 INCR 命令之后 key 的值。</returns>
        long Incr(string key);

        /// <summary>
        /// 将 key 所储存的值加上增量 increment 。
        /// 如果 key 不存在，那么 key 的值会先被初始化为 0 ，然后再执行 INCRBY 命令。
        /// 如果值包含错误的类型，或字符串类型的值不能表示为数字，那么返回一个错误。
        /// 本操作的值限制在 64 位(bit)有符号数字表示之内。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns>加上 increment 之后， key 的值。</returns>
        long IncrBy(string key, long count);

        /// <summary>
        /// 返回 key 所关联的字符串值。
        /// 如果 key 不存在那么返回特殊值 nil 。
        /// 假如 key 储存的值不是字符串类型，返回一个错误，因为 GET 只能用于处理字符串值。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>当 key 不存在时，返回 nil ，否则，返回 key 的值。
        /// 如果 key 不是字符串类型，那么返回一个错误。</returns>
        T Get<T>(string key);

        /// <summary>
        /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)。
        /// 当 key 存在但不是字符串类型时，返回一个错误。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回给定 key 的旧值。
        /// 当 key 没有旧值时，也即是， key 不存在时，返回 nil 。</returns>
        T GetSet<T>(string key, T value);

        /// <summary>
        /// 将字符串值 value 关联到 key 。
        /// 如果 key 已经持有其他值， SET 就覆写旧值，无视类型。
        /// 对于某个原本带有生存时间（TTL）的键来说， 当 SET 命令成功在这个键上执行时， 这个键原有的 TTL 将被清除。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        bool Set<T>(string key, T value, TimeSpan? ttl = null);

        /// <summary>
        /// 将 key 的值设为 value ，当且仅当 key 不存在。
        /// 若给定的 key 已经存在，则 SETNX 不做任何动作。
        /// SETNX 是『SET if Not eXists』(如果不存在，则 SET)的简写。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        bool SetNx<T>(string key, T value, TimeSpan? ttl = null);

        /// <summary>
        /// 将值 value 关联到 key ，并将 key 的生存时间设为 seconds (以秒为单位)。
        /// 如果 key 已经存在， SETEX 命令将覆写旧值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        bool SetEx<T>(string key, T value, TimeSpan? ttl = null);
    }
}
