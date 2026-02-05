using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Runtime.InteropServices;

// 【AI生成】Windows機能を使うための参照（csc.rspの設定が必要）
using System.Windows.Forms;
using System.Drawing;
using Application = UnityEngine.Application; // UnityのApplicationと区別

/// <summary>
/// タスク一覧を表示・管理するクラス。
/// Windows固有の機能（トレイアイコン、最前面固定など）もここで管理する。
/// </summary>
public class TaskListView : TaskView
{
    [Header("UI References")]
    [Tooltip("生成するタスク項目のプレハブ")]
    [SerializeField] private GameObject taskItemPrefab;

    [Tooltip("生成したタスクを配置する親オブジェクト")]
    [SerializeField] private Transform contentParent;

    // トレイアイコン（右下の^に出るアイコン）の保持用
    private NotifyIcon _notifyIcon;

    // =================================================================
    // オーバーライド（初期化）
    // =================================================================
    protected override void Start()
    {
        Debug.Log("[TaskListView] Start: リストビューの初期化を開始します");

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        Debug.Log("[TaskListView] System: Windows専用設定（ウィンドウ枠削除・トレイアイコン）を適用します");
        
        // 1. ウィンドウ設定（最前面 & タスクバーから消す）
        SetupWindowStyle();

        // 2. トレイアイコン(^)の作成
        SetupTrayIcon();
#endif

        // 初期表示データ（ダミー）を作成
        Render(new TaskViewModel(0, "リスト初期化用", false));
    }

    /// <summary>
    /// アプリ終了時の後始末
    /// </summary>
    private void OnApplicationQuit()
    {
        // アイコンを消さないと、アプリが消えても右下に残り続けてしまうため掃除する
        if (_notifyIcon != null)
        {
            Debug.Log("[TaskListView] System: トレイアイコンを破棄します");
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }
    }

    /// <summary>
    /// トレイアイコン（通知領域のアイコン）を作成する
    /// </summary>
    private void SetupTrayIcon()
    {
        try
        {
            // トレイアイコンを作成
            _notifyIcon = new NotifyIcon();

            // システム標準の「アプリケーションアイコン」を使用
            _notifyIcon.Icon = SystemIcons.Application;

            // マウスを乗せた時に出る文字
            _notifyIcon.Text = "ToDo App";

            // アイコンを表示
            _notifyIcon.Visible = true;

            // ダブルクリックしたらアプリを終了する（緊急脱出用）
            _notifyIcon.DoubleClick += (sender, args) =>
            {
                Debug.Log("[TaskListView] TrayIcon: ダブルクリック検知 -> アプリ終了");
                Application.Quit();
            };

            Debug.Log("[TaskListView] System: トレイアイコンの作成に成功しました");
        }
        catch (Exception e)
        {
            Debug.LogError("[TaskListView] Error: トレイアイコンの作成に失敗しました: " + e.Message);
        }
    }

    // =================================================================
    // 定期更新（脱出キー監視）
    // =================================================================
    private void Update()
    {
        // ×ボタンを消しているため、ESCキーで終了できるように監視
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("[TaskListView] Input: ESCキー押下 -> アプリ終了");
            Application.Quit();
        }
    }

    // =================================================================
    // 再描画・生成処理
    // =================================================================
    protected override void Refresh()
    {
        base.Refresh();
    }

    protected override void Render(TaskViewModel viewModel)
    {
        Debug.Log("[TaskListView] Render: テストタスクを生成します");
        GenerateTestTask("初期タスク: 牛乳を買う");
    }

    /// <summary>
    /// 指定したタイトルでタスクを生成し、各種イベントを登録する
    /// </summary>
    public void GenerateTestTask(string title)
    {
        if (taskItemPrefab == null || contentParent == null)
        {
            Debug.LogError("[TaskListView] Error: Prefab または Content が設定されていません");
            return;
        }

        // 1. プレハブ生成
        GameObject newItem = Instantiate(taskItemPrefab, contentParent);
        Debug.Log($"[TaskListView] Generate: タスク生成 -> '{title}'");

        // 2. テキスト設定（変数名を修正済み: textComp）
        var textComp = newItem.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null)
        {
            textComp.text = title;
        }

        // 3. 削除ボタン設定
        var deleteBtn = newItem.GetComponentInChildren<UnityEngine.UI.Button>();
        if (deleteBtn != null)
        {
            // 【重要】未完了タスクは削除できないように最初は無効化
            deleteBtn.interactable = false;

            deleteBtn.onClick.AddListener(() =>
            {
                Debug.Log($"[TaskListView] Action: 削除ボタン押下 -> '{title}' を削除");
                Destroy(newItem);
            });
        }

        // 4. チェックボックス（完了）設定
        var toggle = newItem.GetComponentInChildren<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((bool isOn) =>
            {
                Debug.Log($"[TaskListView] Checkbox: '{title}' 状態変更 -> {isOn}");

                // チェック状態に合わせて見た目を変える
                if (textComp != null)
                {
                    // 打ち消し線の切り替え
                    textComp.fontStyle = isOn ? FontStyles.Strikethrough : FontStyles.Normal;
                    // 色をグレーにして「終わった感」を出す
                    textComp.color = isOn ? UnityEngine.Color.gray : UnityEngine.Color.white;
                }

                // チェックが入っている時だけ、削除ボタンを押せるようにする（誤操作防止）
                if (deleteBtn != null)
                {
                    deleteBtn.interactable = isOn;
                }
            });
        }
    }

    // =================================================================
    // Windows API (魔法の呪文エリア)
    // =================================================================
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    // Windowsのシステム関数を呼び出すための定義
    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    // 定数定義（スタイルのIDなど）
    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20; 
    private const uint WS_SYSMENU = 0x00080000;      // タイトルバーのボタン類
    private const uint WS_EX_TOOLWINDOW = 0x00000080; // タスクバーから隠す設定
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1); // 最前面
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;

    /// <summary>
    /// ウィンドウの見た目を変更する（枠なし、最前面、タスクバー隠し）
    /// </summary>
    private void SetupWindowStyle()
    {
        IntPtr hWnd = GetActiveWindow();

        // 1. ×ボタンなどを消す
        uint style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, style & ~WS_SYSMENU);

        // 2. タスクバーから消す（ツールウィンドウ化）
        uint exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW);

        // 3. 最前面固定
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
    }
#endif
}