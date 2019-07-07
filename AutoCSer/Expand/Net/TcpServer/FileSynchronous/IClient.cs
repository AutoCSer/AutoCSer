using System;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 文件同步客户端操作接口
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// 获取服务端文件信息
        /// </summary>
        /// <param name="path">文件名称</param>
        /// <param name="onGetServerFile">服务端文件信息回调处理</param>
        void Get(string path, Action<ReturnValue<ListFileItem>> onGetServerFile);
        /// <summary>
        /// 小文件上传
        /// </summary>
        /// <param name="path">文件名称</param>
        /// <param name="lastWriteTime">文件最后修改事件</param>
        /// <param name="data">文件数据</param>
        /// <param name="onUploaded">上传回调处理</param>
        void UploadAll(string path, DateTime lastWriteTime, SubArray<byte> data, Action<ReturnValue<SynchronousState>> onUploaded);
        /// <summary>
        /// 创建文件上传
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="listFileItem">列表文件数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="data">文件数据</param>
        /// <param name="onCreated">上传回调处理</param>
        void CreateUpload(string path, ListFileItem listFileItem, long index, SubArray<byte> data, Action<ReturnValue<UploadFileIdentity>> onCreated);
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="fileIdentity">服务端返回文件信息</param>
        /// <param name="data">文件数据</param>
        /// <param name="onUploaded">上传回调处理</param>
        void Upload(UploadFileIdentity fileIdentity, SubArray<byte> data, Action<ReturnValue<SynchronousState>> onUploaded);
        /// <summary>
        /// 创建文件下载
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="listFileItem">列表文件数据</param>
        /// <param name="onCreated">下载回调处理</param>
        void CreateDownload(string path, ListFileItem listFileItem, Action<ReturnValue<DownloadFileIdentity>> onCreated);
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="tick">时钟周期标识</param>
        /// <param name="identity">文件编号</param>
        /// <param name="onDownload">下载回调处理</param>
        void Download(long tick, long identity, Action<ReturnValue<DownloadData>> onDownload);
    }
}
