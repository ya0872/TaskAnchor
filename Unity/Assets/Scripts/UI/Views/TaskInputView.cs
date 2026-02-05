using UnityEngine;
using TMPro; // InputFieldを使うために必要
using UnityEngine.UI; // Buttonを使うために必要

public class TaskInputView : TaskView
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField inputField; // 文字入力欄
    [SerializeField] private Button addButton;          // 追加ボタン

    // リストを管理しているスクリプトへの参照（ここに命令を送る）
    [SerializeField] private TaskListView taskListView;

    // ---------------------------------------------------------
    // 【AI生成】初期化とイベント登録
    // ボタンが押されたら OnAddButtonClicked を呼ぶように設定
    // ---------------------------------------------------------
    private void Start()
    {
        if (addButton != null)
        {
            addButton.onClick.AddListener(OnAddButtonClicked);
        }
    }

    // ---------------------------------------------------------
    // 【AI生成】追加ボタンが押された時の処理
    // 1. 入力が空じゃないかチェック
    // 2. TaskListViewに生成を依頼
    // 3. 入力欄を空っぽに戻す
    // ---------------------------------------------------------
    private void OnAddButtonClicked()
    {
        // 入力欄やリストの参照がない場合はエラーを出して中断
        if (inputField == null || taskListView == null)
        {
            Debug.LogError("[TaskInputView] 設定エラー: Inspectorで InputField または TaskListView を設定してください");
            return;
        }

        // 入力された文字を取得
        string text = inputField.text;

        // 空文字や空白だけなら何もしない
        if (string.IsNullOrWhiteSpace(text))
        {
            Debug.LogWarning("[TaskInputView] 文字が入力されていません");
            return;
        }

        Debug.Log($"[TaskInputView] 追加ボタン押下: {text}");

        // ★ここでTaskListViewの生成メソッドを呼ぶ！
        // (さっき作った GenerateTestTask を再利用する)
        taskListView.GenerateTestTask(text);

        // 入力欄をクリアする（次の入力のために）
        inputField.text = "";
    }

    // 元々の継承メソッド（今回は空でOK）
    public override void Render(TaskViewModel viewModel)
    {
    }
}