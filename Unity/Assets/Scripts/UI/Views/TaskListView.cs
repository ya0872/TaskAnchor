using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // ボタンを扱うために追加！

public class TaskListView : TaskView
{
    [Header("UI References")]
    [SerializeField] private GameObject taskItemPrefab;
    [SerializeField] private Transform contentParent;

    // StartはコメントアウトのままでOK
    private void Start()
    {
        // Debug.Log("[TaskListView] テスト生成を開始します");
        // GenerateTestTask("牛乳を買う");
    }

    // ---------------------------------------------------------
    // 【AI生成】タスク生成 ＋ 削除機能の追加
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

        // 3. テキストを設定（前回実装済み）
        TextMeshProUGUI textComponent = newItem.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = title;
        }

        // ---------------------------------------------------------
        // ★ここから追加：削除ボタンの設定
        // プレハブの中から "Button" コンポーネントを探して、
        // 「押されたら newItem（自分自身）を破壊する」という命令を追加
        // ---------------------------------------------------------
        Button deleteButton = newItem.GetComponentInChildren<Button>();

        if (deleteButton != null)
        {
            // ラムダ式（=>）を使って、その場で処理を書く
            deleteButton.onClick.AddListener(() =>
            {
                Debug.Log($"[TaskListView] タスク削除: {title}");
                Destroy(newItem); // ゲームオブジェクトをシーンから消滅させる
            });
        }
        else
        {
            Debug.LogWarning("[TaskListView] 削除ボタンが見つかりませんでした");
        }
        // ---------------------------------------------------------

        Debug.Log($"[TaskListView] 生成成功: {title}");
    }

    // 継承元の修正に合わせて protected に変更済み
    protected override void Render(TaskViewModel viewModel)
    {
    }

    public void NotifySubmit(string title)
    {
    }
}