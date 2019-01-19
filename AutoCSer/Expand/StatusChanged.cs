using System;

namespace AutoCSer
{
    /// <summary>
    /// 状态改变检测
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct StatusChanged<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public T Status;
        /// <summary>
        /// 检测状态是否改变
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool IsChanged(T Status)
        {
            if (this.Status == null)
            {
                if (Status == null) return false;
            }
            else if (this.Status.Equals(Status)) return false;
            this.Status = Status;
            return true;
        }
    }
}
