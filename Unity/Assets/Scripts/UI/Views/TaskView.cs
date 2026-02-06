using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 全てのViewの基底クラス。
/// ライフサイクル（Show -> Before -> Refresh -> After）を管理する。
/// </summary>
public abstract class TaskView : MonoBehaviour
{
    // =================================================================
    // ライフサイクル管理 (テンプレートメソッドパターン)
    // =================================================================

    /// <summary>
    /// 画面を表示・更新するための公開メソッド
    /// </summary>
    public void Show()
    {
        // 【AI実装】描画フローの統一
        // どのViewでも必ずこの順序で処理が走るように強制する
        BeforeRender();
        Refresh();
        AfterRender();
    }

    /// <summary>
    /// 描画前の準備（例：既存のリストをクリアする）
    /// </summary>
    protected virtual void BeforeRender()
    {
        // デフォルト実装：何もしない
    }

    /// <summary>
    /// データの更新・再描画要求
    /// </summary>
    protected virtual void Refresh()
    {
        // デフォルト実装：何もしない
    }

    /// <summary>
    /// 描画後の処理（例：レイアウトの強制更新）
    /// </summary>
    protected virtual void AfterRender()
    {
        // デフォルト実装：何もしない
    }

    // =================================================================
    // 描画メソッド
    // =================================================================

    /// <summary>
    /// データを受け取ってUIを更新する抽象メソッド。
    /// 子クラスで必ず実装しなければならない。
    /// </summary>
    /// <param name="viewModel">表示用データ</param>
    // 【修正】型を TaskViewModel に戻しました
    protected abstract void Render(TaskViewModel viewModel);

    // =================================================================
    // Unity イベント
    // =================================================================
    protected virtual void Start()
    {
        // 共通の初期化処理があればここに記述
    }
}