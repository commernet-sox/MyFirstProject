using System.Collections.Generic;

namespace CPC.Redis
{
    public interface ISetCommand
    {
        /// <summary>
        /// 将一个或多个 member 元素加入到集合 key 当中，已经存在于集合的 member 元素将被忽略。
        /// 假如 key 不存在，则创建一个只包含 member 元素作成员的集合。
        /// 当 key 不是集合类型时，返回一个错误。
        /// 时间复杂度:O(N)， N 是被添加的元素的数量。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="members"></param>
        /// <returns>被添加到集合中的新元素的数量，不包括被忽略的元素。</returns>
        int SAdd<T>(string key, params T[] members);

        /// <summary>
        /// 返回集合 key 的基数(集合中元素的数量)。
        /// 时间复杂度:O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>集合的基数。
        /// 当 key 不存在时，返回 0 。</returns>
        int SCard(string key);

        /// <summary>
        /// 返回一个集合的全部成员，该集合是所有给定集合之间的差集。
        /// 不存在的 key 被视为空集。
        /// 时间复杂度:O(N)， N 是所有给定集合的成员数量之和。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns>差集成员的列表。</returns>
        HashSet<T> SDiff<T>(params string[] keys);

        /// <summary>
        /// 返回一个集合的全部成员，该集合是所有给定集合的交集。
        /// 不存在的 key 被视为空集。
        /// 当给定集合当中有一个空集时，结果也为空集(根据集合运算定律)。
        /// 时间复杂度:O(N* M)， N 为给定集合当中基数最小的集合， M 为给定集合的个数。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns>交集成员的列表。</returns>
        HashSet<T> SInter<T>(params string[] keys);

        /// <summary>
        /// 返回一个集合的全部成员，该集合是所有给定集合的并集。
        /// 不存在的 key 被视为空集。
        /// 时间复杂度:O(N)， N 是所有给定集合的成员数量之和。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns>并集成员的列表。</returns>
        HashSet<T> SUnion<T>(params string[] keys);

        /// <summary>
        /// 返回集合 key 中的所有成员。
        /// 不存在的 key 被视为空集合。
        /// 时间复杂度:O(N)， N 为集合的基数。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>集合中的所有成员。</returns>
        HashSet<T> SMembers<T>(string key);

        /// <summary>
        /// 判断 member 元素是否集合 key 的成员。
        /// 时间复杂度:O(1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns>如果 member 元素是集合的成员，返回 1 。
        /// 如果 member 元素不是集合的成员，或 key 不存在，返回 0 。</returns>
        bool SIsMember<T>(string key, T member);

        /// <summary>
        /// 移除并返回集合中的一个随机元素。
        /// 时间复杂度:O(1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>被移除的随机元素。
        /// 当 key 不存在或 key 是空集时，返回 nil 。</returns>
        T SPop<T>(string key);

        /// <summary>
        /// 如果命令执行时，只提供了 key 参数，那么返回集合中的一个随机元素。
        /// 从 Redis 2.6 版本开始， SRANDMEMBER 命令接受可选的 count 参数：
        /// 如果 count 为正数，且小于集合基数，那么命令返回一个包含 count 个元素的数组，数组中的元素各不相同。如果 count 大于等于集合基数，那么返回整个集合。
        /// 如果 count 为负数，那么命令返回一个数组，数组中的元素可能会重复出现多次，而数组的长度为 count 的绝对值。
        /// 该操作和 SPOP 相似，但 SPOP 将随机元素从集合中移除并返回，而 SRANDMEMBER 则仅仅返回随机元素，而不对集合进行任何改动。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T SRandMember<T>(string key);

        /// <summary>
        /// 移除集合 key 中的一个或多个 member 元素，不存在的 member 元素会被忽略。
        /// 当 key 不是集合类型，返回一个错误。
        /// 时间复杂度:O(N)， N 为给定 member 元素的数量。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="members">被成功移除的元素的数量，不包括被忽略的元素。</param>
        /// <returns></returns>
        int SRem<T>(string key, params T[] members);
    }
}
