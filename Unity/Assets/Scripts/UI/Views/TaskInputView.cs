using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// タスク入力欄と追加ボタンを管理するクラス。
/// 【変更点】TaskControllerを作らず、ここから直接リストに追加命令を出すようにしました。
/// </summary>
public class TaskInputView : TaskView
{
    [Header("Input UI")]
    [Tooltip("文字を入力する場所")]
    [SerializeField] private TMP_InputField inputField;

    [Tooltip("追加ボタン")]
    [SerializeField] private Button addButton;

    [Header("Connection")]
    [Tooltip("【重要】タスクを追加する先のリストビューをここにセットする")]
    [SerializeField] private TaskListView taskListView;

    // =================================================================
    // オーバーライド
    // =================================================================

    protected override void Start()
    {
        Debug.Log("[TaskInputView] Start: 入力ビューの初期化");

        // エラー回避のためのダミー初期化
        Render(new TaskViewModel(0, "", false));

        // ボタンのイベント登録
        if (addButton != null)
        {
            Debug.Log("[TaskInputView] Setup: Addボタンにクリック処理を登録します");
            addButton.onClick.AddListener(OnAddButtonClicked);
        }
        else
        {
            Debug.LogError("[TaskInputView] Error: AddButton がInspectorで設定されていません！");
        }

        // 接続チェック
        if (taskListView == null)
        {
            Debug.LogWarning("[TaskInputView] Warning: 操作先の TaskListView が設定されていません。追加機能は動きません。");
        }
    }

    protected override void Render(TaskViewModel viewModel)
    {
        // 画面更新時に入力欄をクリア
        if (inputField != null)
        {
            inputField.text = "";
        }
    }

    // =================================================================
    // ボタンクリック時の処理
    // =================================================================
    private void OnAddButtonClicked()
    {
        // 安全確認
        if (inputField == null) return;

        // 1. 入力された文字を取得
        string text = inputField.text;

        // 空文字チェック
        if (string.IsNullOrEmpty(text))
        {
            Debug.LogWarning("[TaskInputView] Warning: 文字が入力されていません。処理を中断します。");
            return;
        }

        Debug.Log($"[TaskInputView] Action: 追加ボタン押下 -> 内容: '{text}'");

        // 2. 【AI修正】コントローラーを使わず、直接リストビューに命令を出す！
        if (taskListView != null)
        {
            Debug.Log("[TaskInputView] Command: TaskListView にタスク生成を命令します");
            taskListView.GenerateTestTask(text);
        }
        else
        {
            Debug.LogError("[TaskInputView] Error: TaskListView が設定されていないため、追加できません！");
        }

        // 3. 入力欄をクリアして次の入力に備える
        inputField.text = "";
    }
}