using System;
using GameFramework;

public static partial class Utility
{
    public static partial class String
    {
        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg">字符串参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public static string Format<T1>(string format, T1 arg)
        {
            if (format == null)
            {
                throw new GameFrameworkException("Format is invalid.");
            }

            return string.Format(format, arg.ToString());
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数的类型。</typeparam>
        /// <typeparam name="T2">字符串参数的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg">字符串参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public static string Format<T1,T2>(string format, T1 arg,T2 arg2)
        {
            if (format == null)
            {
                throw new GameFrameworkException("Format is invalid.");
            }

            return string.Format(format, arg.ToString(),arg2.ToString());
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数的类型。</typeparam>
        /// <typeparam name="T2">字符串参数的类型。</typeparam>
        /// <typeparam name="T3">字符串参数的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg">字符串参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public static string Format<T1,T2,T3>(string format, T1 arg,T2 arg2,T3 arg3)
        {
            if (format == null)
            {
                throw new GameFrameworkException("Format is invalid.");
            }

            return string.Format(format, arg.ToString(),arg2.ToString(),arg3.ToString());
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数的类型。</typeparam>
        /// <typeparam name="T2">字符串参数的类型。</typeparam>
        /// <typeparam name="T3">字符串参数的类型。</typeparam>
        /// <typeparam name="T4">字符串参数的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg">字符串参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public static string Format<T1,T2,T3,T4>(string format, T1 arg,T2 arg2,T3 arg3,T4 arg4)
        {
            if (format == null)
            {
                throw new GameFrameworkException("Format is invalid.");
            }

            return string.Format(format, arg.ToString(),arg2.ToString(),arg3.ToString(),arg4.ToString());
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数的类型。</typeparam>
        /// <typeparam name="T2">字符串参数的类型。</typeparam>
        /// <typeparam name="T3">字符串参数的类型。</typeparam>
        /// <typeparam name="T4">字符串参数的类型。</typeparam>
        /// <typeparam name="T5">字符串参数的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg">字符串参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public static string Format<T1,T2,T3,T4,T5>(string format, T1 arg,T2 arg2,T3 arg3,T4 arg4,T5 arg5)
        {
            if (format == null)
            {
                throw new GameFrameworkException("Format is invalid.");
            }

            return string.Format(format, arg.ToString(),arg2.ToString(),arg3.ToString(),arg4.ToString(),arg5.ToString());
        } 
    }
}
