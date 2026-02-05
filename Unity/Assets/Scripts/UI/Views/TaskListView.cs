using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshProを使うために追加

public class TaskListView : TaskView
{
    [Header("UI References")]
    [SerializeField] private GameObject taskItemPrefab; // コピー元のプレハブ
    [SerializeField] private Transform contentParent;   // 生成場所 (Content)

    // ---------------------------------------------------------
    // 【AI生成】テストデータ生成用メソッドの呼び出し
    // ---------------------------------------------------------
    private void Start()
    {
        Debug.Log("[TaskListView] テスト生成を開始します");
        GenerateTestTask("牛乳を買う");
        GenerateTestTask("Unity課題");
        GenerateTestTask("スクリプト作成なしで実装");
    }

    // ---------------------------------------------------------
    // 【AI生成】タスク動的生成ロジック
    // プレハブをインスタンス化し、GetComponentInChildrenで
    // テキストを書き換える実装をAIが提案
    // ---------------------------------------------------------
    public void GenerateTestTask(string title)
    {
        // 1. 設定チェック
        if (taskItemPrefab == null || contentParent == null)
        {
            Debug.LogError("[TaskListView] Prefab または Content が設定されていません");
            return;
        }

        // 2. 生成する
        GameObject newItem = Instantiate(taskItemPrefab, contentParent);

        // 3. 生成したプレハブの中から TextMeshProUGUI コンポーネントを探す
        // (TaskItemViewを使わず、直接コンポーネントを取得する方式)
        TextMeshProUGUI textComponent = newItem.GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent != null)
        {
            // 4. 文字を変更
            textComponent.text = title;
            Debug.Log($"[TaskListView] 生成成功: {title}");
        }
        else
        {
            Debug.LogError("[TaskListView] 生成したプレハブの中に TextMeshProUGUI が見つかりません");
        }
    }
    // ---------------------------------------------------------
    // 【AI生成】終了
    // ---------------------------------------------------------

    // 元々の継承メソッド（既存の実装）
    public override void Render(TaskViewModel viewModel)
    {
    }

    public void NotifySubmit(string title)
    {
    }
}