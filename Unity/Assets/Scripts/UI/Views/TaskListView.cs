using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using Application = UnityEngine.Application;

public class TaskListView : TaskView
{
    [Header("UI References")]
    [SerializeField] private GameObject taskItemPrefab;
    [SerializeField] private Transform contentParent;

    private NotifyIcon _notifyIcon;

    protected override void Start()
    {
        base.Start();
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        StartCoroutine(SetupWindowStyleDelayed());
        SetupTrayIcon();
#endif
    }

    protected override void BeforeRender()
    {
        // リストの中身を一旦全部クリアする
        if (contentParent != null)
        {
            foreach (Transform child in contentParent) Destroy(child.gameObject);
        }
    }

    // リスト全体を受け取ってループで描画
    public override void Render(List<TaskViewModel> taskViewModelList)
    {
        if (taskViewModelList == null) return;

        foreach (var viewModel in taskViewModelList)
        {
            GenerateTaskItem(viewModel);
        }
    }

    private void GenerateTaskItem(TaskViewModel viewModel)
    {
        if (taskItemPrefab == null || contentParent == null) return;

        GameObject newItem = Instantiate(taskItemPrefab, contentParent);

        // テキスト設定
        var textComp = newItem.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null)
        {
            textComp.text = viewModel.DisplayTitle;
            // 完了済みなら打ち消し線
            if (viewModel.IsCompleted)
            {
                textComp.fontStyle = FontStyles.Strikethrough;
                textComp.color = UnityEngine.Color.gray;
            }
            else
            {
                textComp.fontStyle = FontStyles.Normal;
                textComp.color = UnityEngine.Color.white;
            }
        }

        // チェックボックス（Toggle）設定
        var toggle = newItem.GetComponentInChildren<Toggle>();
        if (toggle != null)
        {
            // イベント通知のループを防ぐため、リスナー登録前に値をセット
            toggle.SetIsOnWithoutNotify(viewModel.IsCompleted);

            toggle.onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn && !viewModel.IsCompleted) // 未完了 -> 完了 になった時
                {
                    // EventHandlerを通じて完了（削除）を通知
                    if (eventHandler != null)
                    {
                        eventHandler.HandleTaskCompleted(viewModel.TaskId);
                    }
                }
            });
        }

        // 削除ボタン設定
        var deleteBtn = newItem.GetComponentInChildren<UnityEngine.UI.Button>();
        if (deleteBtn != null)
        {
            // 完了しているタスクのみ削除ボタンを押せるようにする
            deleteBtn.interactable = viewModel.IsCompleted;

            deleteBtn.onClick.AddListener(() =>
            {
                Debug.Log($"[TaskListView] Delete Request: ID {viewModel.TaskId}");

                // 【重要】画面から消すだけでなく、データベースからも削除する
                if (eventHandler != null)
                {
                    eventHandler.HandleTaskCompleted(viewModel.TaskId);
                }
            });
        }
    }

    // =================================================================
    // Windows API (常駐化・トレイアイコン機能)
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
        catch (System.Exception e) { Debug.LogError(e.Message); }
    }

    private void Update() { if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit(); }

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern System.IntPtr GetActiveWindow();
    [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern uint GetWindowLong(System.IntPtr hWnd, int nIndex);
    [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern int SetWindowLong(System.IntPtr hWnd, int nIndex, uint dwNewLong);
    [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool SetWindowPos(System.IntPtr hWnd, System.IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20; 
    private const uint WS_SYSMENU = 0x00080000;
    private const uint WS_EX_TOOLWINDOW = 0x00000080; 
    private static readonly System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;

    private IEnumerator SetupWindowStyleDelayed()
    {
        yield return null; 
        SetupWindowStyle();
    }

    private void SetupWindowStyle()
    {
        System.IntPtr hWnd = GetActiveWindow();
        uint style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, style & ~WS_SYSMENU);
        uint exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW);
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
    }
#endif
}