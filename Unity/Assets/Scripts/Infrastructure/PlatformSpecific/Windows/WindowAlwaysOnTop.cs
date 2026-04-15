using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class AlwaysOnTop : MonoBehaviour
{
    [Tooltip("実行時にWindowsアプリを常に最前面に固定します。Editorでは無効です。")]
    [SerializeField] private bool _alwaysOnTop = true;

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private const int HWND_TOPMOST = -1;
    private const int HWND_NOTOPMOST = -2;

    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOACTIVATE = 0x0010;

    private Coroutine _applyCoroutine;

    void Start()
    {
        ApplyTopmost();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // アプリからフォーカスが外れた（＝別のアプリを操作し始めた）タイミングで最前面を再適用する
        if (!hasFocus && _alwaysOnTop)
        {
            if (_applyCoroutine != null)
            {
                StopCoroutine(_applyCoroutine);
            }
            _applyCoroutine = StartCoroutine(ApplyWithDelay());
        }
    }

    private IEnumerator ApplyWithDelay()
    {
        // OSのメッセージループとUnityイベントの衝突（無限ループ）を避けるため、一瞬だけ待つ
        yield return new WaitForSecondsRealtime(0.1f);
        ApplyTopmost();
    }

    void OnEnable()
    {
        ApplyTopmost();
    }

    void OnDisable()
    {
        Apply(HWND_NOTOPMOST);
    }

    public void SetAlwaysOnTop(bool enable)
    {
        _alwaysOnTop = enable;
        Apply(enable ? HWND_TOPMOST : HWND_NOTOPMOST);
    }

    private void ApplyTopmost()
    {
        if (_alwaysOnTop)
        {
            Apply(HWND_TOPMOST);
        }
    }

    private void Apply(int zOrder)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        try
        {
            IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
            if (hWnd == IntPtr.Zero) return;

            IntPtr insertAfter = new IntPtr(zOrder);
            
            // SWP_SHOWWINDOW はフォーカスや描画イベントを誘発しやすいので外す
            // 位置・サイズを変更せず、アクティブ化もせずにZオーダーだけを変更する
            uint flags = SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE;

            SetWindowPos(hWnd, insertAfter, 0, 0, 0, 0, flags);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"[AlwaysOnTop] Exception: {ex}");
        }
#endif
    }
}