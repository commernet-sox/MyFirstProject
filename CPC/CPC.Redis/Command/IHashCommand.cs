using System.Collections.Generic;

namespace CPC.Redis
{
    public interface IHashCommand
    {
        /// <summary>
        /// 删除哈希表 key 中的一个或多个指定域，不存在的域将被忽略。
        /// 时间复杂度:O(N)， N 为要删除的域的数量
        /// </summary>
        /// <param name="key">。</param>
        /// <param name="hashFields"></param>
        /// <returns>被成功移除的域的数量，不包括被忽略的域。</returns>
        int HDel<T>(string key, params T[] hashFields);

        /// <summary>
        /// 查看哈希表 key 中，给定域 field 是否存在。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns>如果哈希表含有给定域，返回 1 。
        /// 如果哈希表不含有给定域，或 key 不存在，返回 0 。</returns>
        bool HExists<T>(string key, T hashField);

        /// <summary>
        /// 返回哈希表 key 中给定域 field 的值。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns>给定域的值。
        /// 当给定域不存在或是给定 key 不存在时，返回 nil 。</returns>
        TVal HGet<TKey, TVal>(string key, TKey hashField);

        /// <summary>
        /// 返回哈希表 key 中，一个或多个给定域的值。
        /// 如果给定的域不存在于哈希表，那么返回一个 nil 值。
        /// 因为不存在的 key 被当作一个空哈希表来处理，所以对一个不存在的 key 进行 HMGET 操作将返回一个只带有 nil 值的表。
        /// 时间复杂度：O(N)， N 为给定域的数量。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        List<TVal> HGet<TKey, TVal>(string key, params TKey[] hashFields);

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中。
        /// 此命令会覆盖哈希表中已存在的域。
        /// 如果 key 不存在，一个空哈希表被创建并执行 HMSET 操作。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashData"></param>
        void HSet<TKey, TVal>(string key, Dictionary<TKey, TVal> hashData);

        /// <summary>
        /// 将哈希表 key 中的域 field 的值设为 value 。
        /// 如果 key 不存在，一个新的哈希表被创建并进行 HSET 操作。
        /// 如果域 field 已经存在于哈希表中，旧值将被覆盖。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <param name="hashVal"></param>
        bool HSet<TKey, TVal>(string key, TKey hashField, TVal hashVal);

        /// <summary>
        /// 将哈希表 key 中的域 field 的值设置为 value ，当且仅当域 field 不存在。
        /// 若域 field 已经存在，该操作无效。
        /// 如果 key 不存在，一个新哈希表被创建并执行 HSETNX 命令。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <param name="hashVal"></param>
        /// <returns></returns>
        bool HSetNx<TKey, TVal>(string key, TKey hashField, TVal hashVal);

        /// <summary>
        /// 返回哈希表 key 中，所有的域和值。
        /// 在返回值里，紧跟每个域名(field name)之后是域的值(value)，所以返回值的长度是哈希表大小的两倍。
        /// 时间复杂度：O(N)， N 为哈希表的大小。
        /// </summary>
        /// <param name="key"></param>
        /// <returns> 以列表形式返回哈希表的域和域的值。
        /// 若 key 不存在，返回空列表。</returns>
        Dictionary<TKey, TVal> HGetAll<TKey, TVal>(string key);

        /// <summary>
        /// 返回哈希表 key 中的所有域。
        /// 时间复杂度：O(N)， N 为哈希表的大小。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>一个包含哈希表中所有域的表。
        /// 当 key 不存在时，返回一个空表。</returns>
        List<T> HKeys<T>(string key);

        /// <summary>
        /// 返回哈希表 key 中域的数量。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>哈希表中域的数量。
        /// 当 key 不存在时，返回 0 。</returns>
        int HLen(string key);
    }
}
