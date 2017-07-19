using System;

namespace ForCheng.CodeMaker
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    [Flags]
    public enum CodeType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 数字
        /// </summary>
        Number = 1,
        /// <summary>
        /// 小写字母
        /// </summary>
        LowerChar = 2,
        /// <summary>
        /// 大写字母
        /// </summary>
        UpperChar = 4,
        /// <summary>
        /// 数字和字母
        /// </summary>
        All = 7,
    }
}