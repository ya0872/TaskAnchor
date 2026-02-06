using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using Application = UnityEngine.Application;

/// <summary>
/// タスク一覧を表示するクラス。
/// Windows常駐アプリとしての機能（トレイアイコン等）もここで管理する。
/// </summary>
public class TaskListView : TaskView
{
    [Header("UI References")]
    [SerializeField] private GameObject taskItemPrefab;
    [SerializeField] private Transform contentParent;

    private NotifyIcon _notifyIcon;

    // =================================================================
    // 1. 初期化 (Start)
    // =================================================================
    protected override void Start()
    {
        base.Start();
        Debug.Log("[TaskListView] Start: リストビューの初期化を開始");

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        // 【AI実装】Windows固有機能のセットアップ
        // 起動時の競合を防ぐため、コルーチンで遅延実行させる
        StartCoroutine(SetupWindowStyleDelayed());
        SetupTrayIcon();
#endif

        // 【修正】型を TaskViewModel に戻しました
        // Start時に初期描画を行う要件に対応
        var initialData = new TaskViewModel(0, "初期タスク: アプリ起動", false);
        Render(initialData);
    }

    // =================================================================
    // 2. 描画フロー
    // =================================================================

    protected override void BeforeRender()
    {
        Debug.Log("[TaskListView] BeforeRender: 既存のタスクリストをクリアします");
        if (contentParent != null)
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
        }
    }

    protected override void Refresh()
    {
        Debug.Log("[TaskListView] Refresh: データの更新を行います");

        // テスト用データの生成
        var testData = new TaskViewModel(1, "更新されたタスク", false);
        Render(testData);
    }

    protected override void AfterRender()
    {
        Debug.Log("[TaskListView] AfterRender: 描画完了後の処理（現在は空）");
    }

    // 【修正】型を TaskViewModel に戻しました
    protected override void Render(TaskViewModel viewModel)
    {
        Debug.Log($"[TaskListView] Render: タスク生成 -> {viewModel.Title}");
        // 共通メソッドを使って生成
        GenerateTestTask(viewModel.Title);
    }

    // =================================================================
    // タスク生成処理
    // =================================================================
    /// <summary>
    /// 指定されたタイトルでタスク項目を生成する
    /// TaskInputView からも呼ばれる
    /// </summary>
    public void GenerateTestTask(string title)
    {
        if (taskItemPrefab == null || contentParent == null) return;

        // 【AI実装】プレハブのインスタンス化
        GameObject newItem = Instantiate(taskItemPrefab, contentParent);

        // テキスト設定
        var textComp = newItem.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null)
        {
            textComp.text = title;
        }

        // 削除ボタン設定
        var deleteBtn = newItem.GetComponentInChildren<UnityEngine.UI.Button>();
        if (deleteBtn != null)
        {
            deleteBtn.interactable = false; // 最初は無効
            deleteBtn.onClick.AddListener(() =>
            {
                Debug.Log($"[TaskListView] Delete: '{title}' を削除します");
                Destroy(newItem);
            });
        }

        // チェックボックス設定
        var toggle = newItem.GetComponentInChildren<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((bool isOn) =>
            {
                // 【AI実装】完了状態の視覚的フィードバック
                if (textComp != null)
                {
                    textComp.fontStyle = isOn ? FontStyles.Strikethrough : FontStyles.Normal;
                    textComp.color = isOn ? UnityEngine.Color.gray : UnityEngine.Color.white;
                }
                // チェック時のみ削除ボタンを有効化
                if (deleteBtn != null) deleteBtn.interactable = isOn;
            });
        }
    }

    // =================================================================
    // Windows API / 常駐設定 (AI実装エリア)
    // =================================================================

    private void OnApplicationQuit()
    {
        if (_notifyIcon != null) { _notifyIcon.Dispose(); _notifyIcon = null; }
    }

    private void SetupTrayIcon()
    {
        try
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = SystemIcons.Application;
            _notifyIcon.Text = "ToDo App";
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += (sender, args) => Application.Quit();
        }
        catch (Exception e) { Debug.LogError("TrayIcon Error: " + e.Message); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20; 
    private const uint WS_SYSMENU = 0x00080000;
    private const uint WS_EX_TOOLWINDOW = 0x00000080; 
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;

    // 【AI実装】ウィンドウ設定の遅延適用
    private IEnumerator SetupWindowStyleDelayed()
    {
        // 1フレーム待機してUnityの初期化完了を待つ
        yield return null; 
        SetupWindowStyle();
    }

    private void SetupWindowStyle()
    {
        IntPtr hWnd = GetActiveWindow();
        uint style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, style & ~WS_SYSMENU);
        uint exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW);
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        Debug.Log("[TaskListView] WindowStyle: 常駐化設定（枠削除・タスクバー非表示）を適用しました");
    }
#endif
}