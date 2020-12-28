using System.Collections.Generic;

namespace SDT.Redis
{
    public interface IKeyCommand
    {
        /// <summary>
        /// 删除给定的一个或多个 key 。
        /// 不存在的 key 会被忽略。
        /// 时间复杂度：O(N)， N 为被删除的 key 的数量。
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>被删除 key 的数量。</returns>
        long Del(params string[] keys);

        /// <summary>
        /// 检查给定 key 是否存在。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 为给定 key 设置生存时间，当 key 过期时(生存时间为 0 )，它会被自动删除。
        /// 可以对一个已经带有生存时间的 key 执行 EXPIRE 命令，新指定的生存时间会取代旧的生存时间。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sec">时间（秒）</param>
        /// <returns>当 key 不存在或者不能为 key 设置生存时间时(比如在低于 2.1.3 版本的 Redis 中你尝试更新 key 的生存时间)，返回 0 。</returns>
        bool Expire(string key, int sec);

        /// <summary>
        /// 查找所有符合给定模式 pattern 的 key 。
        /// KEYS * 匹配数据库中所有 key 。
        /// KEYS h?llo 匹配 hello ， hallo 和 hxllo 等。
        ///  KEYS h*llo 匹配 hllo 和 heeeeello 等。
        ///  KEYS h[ae]llo 匹配 hello 和 hallo ，但不匹配 hillo 。
        ///  KEYS 的速度非常快，但在一个大的数据库中使用它仍然可能造成性能问题，如果你需要从一个数据集中查找特定的 key ，你最好还是用 Redis 的集合结构(set)来代替。
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns>符合给定模式的 key 列表。</returns>
        List<string> Keys(string pattern);

        /// <summary>
        /// 将当前数据库的 key 移动到给定的数据库 db 当中。
        /// 如果当前数据库(源数据库)和给定数据库(目标数据库)有相同名字的给定 key ，或者 key 不存在于当前数据库，那么 MOVE 没有任何效果。
        /// 因此，也可以利用这一特性，将 MOVE 当作锁(locking)原语(primitive)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        bool Move(string key, int dbIndex);

        /// <summary>
        /// 移除给定 key 的生存时间，将这个 key 从『易失的』(带生存时间 key )转换成『持久的』(一个不带生存时间、永不过期的 key )。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>当生存时间移除成功时，返回 1 .
        /// 如果 key 不存在或 key 没有设置生存时间，返回 0 。</returns>
        bool Persist(string key);

        /// <summary>
        /// 从当前数据库中随机返回(不删除)一个 key 。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <returns>当数据库不为空时，返回一个 key 。
        /// 当数据库为空时，返回 nil 。</returns>
        string RandomKey();

        /// <summary>
        /// 将 key 改名为 newkey 。
        /// 当 key 和 newkey 相同，或者 key 不存在时，返回一个错误。
        /// 当 newkey 已经存在时， RENAME 命令将覆盖旧值。
        /// 时间复杂度：O(1)
        /// 改名成功时提示 OK ，失败时候返回一个错误
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newKey"></param>
        void Rename(string key, string newKey);

        /// <summary>
        /// 当且仅当 newkey 不存在时，将 key 改名为 newkey 。
        /// 当 key 不存在时，返回一个错误。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newKey"></param>
        /// <returns>修改成功时，返回 1 。
        /// 如果 newkey 已经存在，返回 0 。</returns>
        bool RenameNx(string key, string newKey);

        /// <summary>
        /// 排序默认以数字作为对象，值被解释为双精度浮点数，然后进行比较。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderDesc"></param>
        /// <param name="sortAlphabetic"></param>
        /// <returns>返回或保存给定列表、集合、有序集合 key 中经过排序的元素。</returns>
        List<T> Sort<T>(string key, int skip = 0, int take = -1, bool orderDesc = false, bool sortAlphabetic = false);

        /// <summary>
        /// 以秒为单位，返回给定 key 的剩余生存时间(TTL, time to live)。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>当 key 不存在时，返回 -2 。
        /// 当 key 存在但没有设置剩余生存时间时，返回 -1 。
        /// 否则，以秒为单位，返回 key 的剩余生存时间。</returns>
        int TTL(string key);

        /// <summary>
        /// 返回 key 所储存的值的类型。
        /// 时间复杂度：O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>none (key不存在)
        /// string (字符串)
        /// list(列表)
        /// set(集合)
        /// zset(有序集)
        /// hash(哈希表)</returns>
        string Type(string key);
    }
}
