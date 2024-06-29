using AOT.Framework;
using Cysharp.Threading.Tasks;
using AOT.Framework.Debug;
using AOT.Framework.Fsm;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
using YooAsset;

namespace Src
{
    public class DownloadProcedure:ProcedureBase
    {
        private bool downloadDone; //下载是否完成
        private int downloadingMaxNum = 10;
        private int failedTryAgain = 3;
        private int timeout = 60;
        public DownloadProcedure(IFsm handle) : base(handle)
        {
        }

        protected internal override void OnInit()
        {
            EDebug.Log("DownloadProcedure OnInit");
            downloadDone = false;
        }

        protected internal override void OnEnter()
        {
            EDebug.Log("DownloadProcedure OnEnter");
            
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            EDebug.Log("DownloadProcedure OnUpdate");
            this.ChangeState<EnterGameProcedure>();
        }

        protected internal override void OnLeave()
        {
            EDebug.Log("DownloadProcedure OnLeave");
        }
        
        
        /// <summary>
        /// 更新版本清单资源
        /// </summary>
        public async UniTask UpdatePackageManifest()
        {
            var package = GameEntry.GetModule<ResourceManager>().defaultPackage;
            var operation = package.UpdatePackageManifestAsync(GameEntry.GetModule<ResourceManager>().packageVersion);
            await operation.ToUniTask();
            if (operation.Status != EOperationStatus.Succeed)
            {
                //更新资源版本清单失败 
                EDebug.LogError(operation.Error);
                return;
            }
            //下载补丁
            await Download();
        }
        
        
        private async UniTask Download()
        {
            var package = GameEntry.GetModule<ResourceManager>().defaultPackage;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain, timeout);
            //没有需要下载的资源
            if (downloader.TotalDownloadCount == 0)
            {
                EDebug.Log("没有资源更新，直接进入游戏加载环节");
                downloadDone = true;
                return;
            }
            //需要下载的文件总数和总大小
            int totalDownloadCount = downloader.TotalDownloadCount;
            long totalDownloadBytes = downloader.TotalDownloadBytes;
            EDebug.Log($"文件总数:{totalDownloadCount}:::总大小:{totalDownloadBytes}");
            await UniTask.DelayFrame(1);
            //进行下载
            //注册回调方法
            downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
            downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
            downloader.OnDownloadOverCallback = OnDownloadOverFunction;
            downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;
            //开启下载
            downloader.BeginDownload();
            await downloader.ToUniTask();
            //检测下载结果
            if (downloader.Status == EOperationStatus.Succeed)
            {
                //下载成功
                downloadDone = true;
            }
            else
            {
                //下载失败
                EDebug.LogError("更新失败！");
            }
        }

        private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
        {
            EDebug.Log(Utility.String.Format("开始下载：文件名：{0}, 文件大小：{1}", fileName, sizeBytes));
        }

        private void OnDownloadOverFunction(bool isSucceed)
        {
            EDebug.Log("下载" + (isSucceed ? "成功" : "失败"));
        }

        private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
        {
            EDebug.Log(string.Format("文件总数：{0}, 已下载文件数：{1}, 下载总大小：{2}, 已下载大小：{3}", totalDownloadCount,
                currentDownloadCount, totalDownloadBytes, currentDownloadBytes));
        }

        private void OnDownloadErrorFunction(string fileName, string error)
        {
            EDebug.LogError(string.Format("下载出错：文件名：{0}, 错误信息：{1}", fileName, error));
        }
    }
}