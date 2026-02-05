using System;
using UnityEngine;

/// <summary>
/// 全てのView（画面・UI部品）の親クラス。
/// 共通の描画フロー（Show -> Before -> Refresh -> After）を管理する。
/// </summary>
public abstract class TaskView : MonoBehaviour
{
    protected EventHandler _eventHandler;

    // =================================================================
    // Unityライフサイクル
    // =================================================================

    // 【AI生成】子クラスでの拡張を考慮し、virtual修飾子を付与して定義
    protected virtual void Start()
    {
        Debug.Log($"[{gameObject.name}] Start: 初期化を開始します");

        // 【修正】引数なしだとエラーになるので、ダミーデータ(ID=0, タイトル="", 完了=false)を渡す
        Render(new TaskViewModel(0, "", false));
    }

    public virtual void Initialize(EventHandler eventHandler)
    {
        Debug.Log($"[{gameObject.name}] Initialize: EventHandlerを設定しました");
        _eventHandler = eventHandler;
    }

    // =================================================================
    // 公開メソッド
    // =================================================================

    // 【AI生成】描画フローの制御（テンプレートメソッドパターン）の実装
    // 外部からはこのShowメソッドを呼ぶだけで、適切な順序で更新処理が走るように設計
    public void Show()
    {
        Debug.Log($"[{gameObject.name}] Show: 画面を表示し、更新フローを開始します");

        gameObject.SetActive(true);

        // 定義された順序でメソッドを実行
        BeforeRender();
        Refresh();
        AfterRender();

        Debug.Log($"[{gameObject.name}] Show: 更新フロー完了");
    }

    // =================================================================
    // 内部処理（オーバーライド用）
    // =================================================================

    // 【AI修正】外部からの直接呼び出しを防ぐため、publicからprotectedに変更
    protected virtual void Render(TaskViewModel viewModel)
    {
    }

    // 【AI修正】再描画ロジックの分離のため定義
    protected virtual void Refresh()
    {
        Debug.Log($"[{gameObject.name}] Refresh: 再描画要求");
    }

    protected virtual void BeforeRender() { }
    protected virtual void AfterRender() { }
}