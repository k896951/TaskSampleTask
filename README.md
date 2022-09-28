# 偽装ちゃん22用リスナ拡張のサンプル

偽装ちゃん22用拡張リスナのサンプル、TaskSampleTaskです。

サンプルのリスナ定義コードをVisual Studio2022のプロジェクトで提供します。
これをコンパイルするとサンプルのリスナDLLである TaskSampleTask.dll が生成されます。

FakeChan22.exeと同じフォルダに Extend フォルダを作成してこの中に TaskSampleTask.dll をコピーしてから偽装ちゃん22を起動してください。
偽装ちゃん22のリスナ設定に 「SampleTask」が追加されます。

## 拡張リスナコンパイル方法（VisualStudio）

TaskSampleTask.csのコピーされているフォルダに、偽装ちゃん22のアーカイブに含まれるFakeChan22TaskBase.dllをコピーしてください。
Visual Studioでプロジェクトを開いたら参照設定を確認し、FakeChan22TaskBase.dllの登録に問題があれば再登録してください。

問題無ければコンパイルすると、bin/Release もしくは bin/Debug にTaskSampleTask.dll が生成されます。

## DLL(Assembly)の内容

2つのクラスが名称 TaskSampleTask.dll のDLL(Assembly)に含まれています。
DLL名、クラス名は識別名を含める、指定のネーミングルールに従って名付けます。
このサンプルでは識別名が"SampleTask"なので以下のような名称になります。

識別名適用 | 要素 | 名前 | 説明
---|---|---|---
〇 | DLL(Assembl)   | TaskSampleTask.dll         | リスナタスクはこのネームスペースに配置されます。
　 | ネームスペース | FakeChan22.Tasks           | この名称で固定。含まれるクラスはこのネームスペースに配置されます。
〇 | クラス         | ListenerConfigSampleTask   | リスナの設定値が保持されるクラスです。名称は "ListenerConfig" + 識別名 になります。
〇 | クラス         | TaskSampleTask             | リスナの処理実体のクラスです。名称は "Task" + 識別名 になります。


## クラス "ListenerConfigSampleTask"

このサンプルでは識別名が"SampleTask"なので以下のような名称になります。

要素 | 名前 | 説明
---|---|---
コンストラクタ   | public ListenerConfigSampleTask() | 引数無しのコンストラクタが呼び出されます。

このクラスのコンストラクタ内で最低3つのプロパティを設定します

要素 | 名前 | 説明
---|---|---
プロパティ | string LabelName        | リスナ名です。表示の際にこの内容が利用されます。
プロパティ | string ServiceName      | comment.xml のService属性に設定される値です。
プロパティ | string TaskTypeFullName | クラス "Task{識別名}" の完全修飾名です。

あとはこのリスナで必要なプロパティを設定します。
このサンプルではプロパティ MonitoringFolderPath に初期値を設定しています。

## クラス "TaskSampleTask"

このサンプルでは識別名が"SampleTask"なので以下のような名称になります。

要素 | 名前/型 | 説明
---|---|---
コンストラクタ | public TaskSampleTask(ref lsnr, ref queue)  | 引数2個のコンストラクタが呼び出されます。
第1引数        | ListenerConfigSampleTask                    | クラス "ListenerConfigSampleTask"のインスタンスへの参照です。 
第2引数        | MessageQueueWrapper                         | クラス MessageQueueWrapper のインスタンスへの参照です。  テキスト（メッセージ）を登録するキューになります。 


このクラスのコンストラクタ内で最低2つのプロパティを設定します

要素 | 名前 | 説明
---|---|---
プロパティ      | MessQueue        | キスト（メッセージ）を送るキューのクラスへの参照を保持します。
プロパティ      | based            | 必ずfalseにしてください。

このクラスには最低3つのメソッドを実装する必要があります。

要素 | 名前 | 説明
---|---|---
メソッド      | public void TaskStart()        | テキスト（メッセージ）発生待ち処理とキューへ登録する処理を起動します。 
メソッド      | public void TaskStop()            | TaskStart()で起動した処理を終了します。
メソッド      | public void Dispose()            | IDisposableを継承したので必須です。オブジェクト開放を実行します。 

