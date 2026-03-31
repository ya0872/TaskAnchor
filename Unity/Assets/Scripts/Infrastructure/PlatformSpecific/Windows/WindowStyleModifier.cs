using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowStyleModifier : MonoBehaviour
{
    // Windows APIのインポート
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    // ウィンドウスタイルの定数
    const int GWL_STYLE = -16;
    const int WS_SYSMENU = 0x00080000;    // 閉じる(×)ボタンやアイコンのメニュー
    const int WS_MINIMIZEBOX = 0x00020000;// 最小化ボタン
    const int WS_MAXIMIZEBOX = 0x00010000;// 最大化ボタン
    const int WS_THICKFRAME = 0x00040000; // サイズ変更が可能なウィンドウ枠

    // SetWindowPos用のフラグ
    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_NOZORDER = 0x0004;
    const uint SWP_FRAMECHANGED = 0x0020;

    void Start()
    {
        // エディタ上では動作させず、Windowsのビルド済みアプリでのみ実行する
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        IntPtr hWnd = GetActiveWindow();
        int style = GetWindowLong(hWnd, GWL_STYLE);

        // 各種ボタンを消す（ビット演算でフラグをオフにする）
        style &= ~WS_SYSMENU;
        style &= ~WS_MAXIMIZEBOX;
        style &= ~WS_MINIMIZEBOX;

        // サイズ変更は可能にする（ビット演算でフラグをオンにする）
        style |= WS_THICKFRAME;

        // 変更したスタイルをOSにセット
        SetWindowLong(hWnd, GWL_STYLE, style);

        // ウィンドウ枠の変更をOSに通知して再描画させる
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
#endif
    }
}