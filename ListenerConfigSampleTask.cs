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
}
