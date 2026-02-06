using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// タスクの入力（追加）を行うビュー。
/// ユーザーが文字を入力し、ボタンを押すとタスクを追加する。
/// </summary>
public class TaskInputView : TaskView
{
    [Header("UI References")]
    [Tooltip("タスク名を入力するフィールド")]
    [SerializeField] private TMP_InputField inputField;

    [Tooltip("追加ボタン")]
    [SerializeField] private Button addButton;

    [Tooltip("操作対象のリストビュー（直接参照）")]
    [SerializeField] private TaskListView taskListView;

    // =================================================================
    // 初期化
    // =================================================================
    protected override void Start()
    {
        base.Start();
        Debug.Log("[TaskInputView] Start: 入力画面の初期化を開始");

        // 【AI実装】ボタンイベントの登録
        // ボタンが押されたら OnAddButtonClicked を呼ぶように設定
        if (addButton != null)
        {
            addButton.onClick.AddListener(OnAddButtonClicked);
        }
        else
        {
            Debug.LogError("[TaskInputView] Error: AddButtonがアタッチされていません");
        }
    }

    // =================================================================
    // イベント処理
    // =================================================================
    /// <summary>
    /// 追加ボタンが押された時の処理
    /// </summary>
    private void OnAddButtonClicked()
    {
        // 【AI実装】入力チェック
        // 空文字やスペースだけの場合は追加しない
        if (inputField == null || string.IsNullOrWhiteSpace(inputField.text))
        {
            Debug.LogWarning("[TaskInputView] Warning: 入力が空のため、タスク追加を中断しました");
            return;
        }

        string newTitle = inputField.text;
        Debug.Log($"[TaskInputView] Action: 追加ボタン押下 -> 入力値: '{newTitle}'");

        // 【AI実装】リストビューへの追加命令
        // 本来はControllerを経由すべきだが、現状はTaskListViewを直接操作する
        if (taskListView != null)
        {
            // TaskListView側のメソッド名は GenerateTestTask のまま維持
            taskListView.GenerateTestTask(newTitle);
            Debug.Log("[TaskInputView] Success: TaskListViewへ追加リクエストを送信しました");
        }
        else
        {
            Debug.LogError("[TaskInputView] Error: TaskListViewの参照がありません（Inspectorで設定してください）");
        }

        // 入力欄をクリアして次の入力に備える
        inputField.text = "";
    }

    // =================================================================
    // TaskView の必須オーバーライド
    // =================================================================

    /// <summary>
    /// 入力画面の描画更新処理
    /// </summary>
    /// <param name="viewModel">表示データ</param>
    // 【修正】型を TaskViewModel に戻しました
    protected override void Render(TaskViewModel viewModel)
    {
        Debug.Log("[TaskInputView] Render: 描画更新リクエストを受信しました");

        // 入力画面では特に表示するデータがないため、ログ出力のみ行う
        // 必要であればここで入力欄の初期値をセットする等の処理を追加可能
        if (viewModel != null)
        {
            Debug.Log($"[TaskInputView] Render Info: ID={viewModel.Id}, Title={viewModel.Title}");
        }
    }

    // 親クラスの仮想メソッドをそのまま使用（ログ出しのため呼ぶ）
    protected override void Refresh()
    {
        base.Refresh();
    }
}