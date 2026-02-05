using System.Collections.Generic;
using UnityEngine;
using System; // EventHandlerを使うために必要

public abstract class TaskView : MonoBehaviour
{
    // イベントハンドラー（Presenterとの通信用）
    protected EventHandler _eventHandler;

    // ---------------------------------------------------------
    // 【修正】公開範囲を protected に変更（外部から隠す）
    // ---------------------------------------------------------
    protected virtual void Render(TaskViewModel viewModel)
    {
        // 基本クラスでは何もしない（オーバーライド用）
    }

    // ---------------------------------------------------------
    // 【修正】公開範囲を protected に変更（外部から隠す）
    // ---------------------------------------------------------
    protected virtual void Refresh()
    {
        // 画面の更新処理など
    }

    // ---------------------------------------------------------
    // 初期化処理（外部から呼ぶので public のままが一般的だが、
    // 今回の指定にはないため一旦 public のままにしておく）
    // ---------------------------------------------------------
    public virtual void Initialize(EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    // ---------------------------------------------------------
    // 【指定通り】これだけ public（外部公開）
    // ---------------------------------------------------------
    public void Show()
    {
        Debug.Log($"[TaskView] {gameObject.name} を表示します");
        gameObject.SetActive(true);

        // 表示したタイミングで更新をかけるならここでRefreshを呼ぶ
        Refresh();
    }

    // 以前のコードにあったヘルパーメソッド群（protectedのまま）
    protected void BeforeRender() { }
    protected void AfterRender() { }
}