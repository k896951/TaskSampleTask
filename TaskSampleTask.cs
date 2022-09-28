using FakeChan22.Params;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace FakeChan22.Tasks
{
    /// <summary>
    /// リスナ設定値を保持するクラスです。
    /// クラス名称は "ListenerConfig{識別名}"としてください。
    /// このサンプルの識別名は SampleTask です。
    /// </summary>
    [DataContract]
    public class ListenerConfigSampleTask : ListenerConfigBase
    {
        /// <summary>
        /// 監視を行うフォルダのパスです。
        /// </summary>
        [GuiItem(ParamName = "監視フォルダ", Description = "監視を行うフォルダのパスです")]
        [DataMember]
        public string MonitoringFolderPath { get; set; }

        public ListenerConfigSampleTask()
        {
            LabelName = "SampleTask";                            // リスナ名です。表示の際にこの内容が利用されます。
            ServiceName = "SampleTask";                          // comment.xml のService属性に設定される値です。
            TaskTypeFullName = typeof(TaskSampleTask).FullName;  // TaskSampleTaskクラスの完全修飾名です。

            MonitoringFolderPath = @".\";                        // リスナが監視するフォルダのパスを提供するプロパティです。
        }
    }


    /// <summary>
    /// リスナの処理実体のクラスです。
    /// クラス名称は "Task{識別名}"としてください。
    /// このサンプルの識別名は SampleTask です。
    /// </summary>
    public class TaskSampleTask : TaskBase, IDisposable, ITask
    {
        ListenerConfigSampleTask LsnrConfig;

        string TaskName = "SampleTask";

        FileSystemWatcher watcher = null;

        /// <summary>
        /// コンストラクタです。
        /// 第1引数は設定値保持クラスへの参照です。クラス（クラス名)は "ListenerConfig{識別名}"です。
        /// 第2引数はテキスト（メッセージ）を登録するキューへの参照です。
        /// </summary>
        /// <param name="lsnrCfg">リスナ設定値保持クラスです</param>
        /// <param name="queue">テキスト（メッセージ）を送るキューのクラスです</param>
        public TaskSampleTask(ref ListenerConfigSampleTask lsnrCfg, ref MessageQueueWrapper queue)
        {
            LsnrConfig = lsnrCfg;  // リスナの設定値が保存されたクラスへの参照を保持します（必要な場合）
            MessQueue = queue;     // キスト（メッセージ）を送るキューのクラスへの参照を保持します。
            based = false;         // 必ず false に設定してください。

            // ここに必要な初期化処理を記述します。



            // ここまでに必要な初期化処理を記述します。

        }

        /// <summary>
        /// リスナの受信タスク開始メソッドです。
        /// 偽装ちゃん22から呼び出しされます。
        /// </summary>
        public override void TaskStart()
        {
            try
            {
                // ここから必要な起動処理を記述します。

                if (!Directory.Exists(LsnrConfig.MonitoringFolderPath))
                {
                    Logging(String.Format(@"{0}, {1}", TaskName, "監視対象フォルダが存在しない"));
                    return;
                }

                watcher = new FileSystemWatcher();

                watcher.Path = LsnrConfig.MonitoringFolderPath;
                watcher.Filter = "";
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                watcher.SynchronizingObject = null;

                watcher.Changed += (object source, FileSystemEventArgs e) => {

                    string speechText = "ファイルの変更を検知した";
                    string logText = "ファイル [" + e.FullPath + "] が変更された";

                    SendMessage(speechText, logText);
                };

                watcher.Created += (object source, FileSystemEventArgs e) => {

                    string speechText = "ファイルの作成を検知した";
                    string logText = "ファイル [" + e.FullPath + "] 作成された";

                    SendMessage(speechText, logText);
                };

                watcher.Deleted += (object source, FileSystemEventArgs e) =>{

                    string speechText = "ファイルの削除を検知した";
                    string logText = "ファイル [" + e.FullPath + "] が削除された";

                    SendMessage(speechText, logText);
                };

                watcher.Renamed += (object source, RenamedEventArgs e) => {

                    string speechText = "ファイルのリネームを検知した";
                    string logText = "ファイル [" + e.OldFullPath +"] → [" + e.FullPath + "] に名称変更された";

                    SendMessage(speechText, logText);
                };

                watcher.EnableRaisingEvents = true;

                // ここまでに必要な起動処理を記述します。


                IsRunning = true;

            }
            catch (Exception e)
            {
                Logging(String.Format(@"{0}, {1}", TaskName, e.Message));
            }
        }

        /// <summary>
        /// リスナの受信タスク停止メソッドです。
        /// 偽装ちゃん22から呼び出しされます。
        /// </summary>
        public override void TaskStop()
        {
            try
            {
                // ここから必要な停止処理を記述します。
                
                if(watcher != null)
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.Dispose();
                }

                // ここまでに必要な停止処理を記述します。

            }
            catch (Exception e)
            {
                Logging(String.Format(@"{0}, {1}", TaskName, e.Message));
            }

            IsRunning = false;
        }

        /// <summary>
        /// オブジェクト開放の処理です。
        /// IDisposableを指定しているので実装が必要です。
        /// </summary>
        public void Dispose()
        {
            LsnrConfig = null;
            MessQueue = null;

            // 他に開放するオブジェクトがあればここに記述します。

            watcher = null;
        }

        /// <summary>
        /// テキスト（メッセージ）送信共通処理
        /// </summary>
        /// <param name="speechText">発声のテキスト</param>
        /// <param name="logText">ログのテキスト</param>
        private void SendMessage(string speechText, string logText)
        {
            Logging(String.Format(@"{0}, {1}", TaskName, logText));

            var talk = new Params.MessageData()
            {
                LsnrCfg = LsnrConfig,
                OrgMessage = speechText,
                CompatSpeed = -1,
                CompatTone = -1,
                CompatVolume = -1,
                CompatVType = -1,
                TaskId = MessQueue.count + 1
            };

            if (LsnrConfig.IsAsync)
            {
                AsyncTalk(talk);
            }
            else
            {
                SyncTalk(talk);
            }

        }


    }
}
