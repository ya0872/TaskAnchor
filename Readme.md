# TaskAnchor

## 概要
TaskAnchorは、タスク管理を目的としたUnity製アプリケーションです。クリーンアーキテクチャを意識した設計となっており、各レイヤーごとに責務が分離されています。

## ディレクトリ構成
```
TaskAnchor/
├── Unity/
│   ├── Assets/
│   │   ├── Scripts/         # ソースコード（UI, Application, Domain, Infrastructure）
│   │   ├── Plugins/         # 外部プラグイン（SQLite4Unity3d等）
│   │   ├── Prefabs/         # プレハブ
│   │   ├── Scenes/          # シーンファイル
│   │   └── ...
│   ├── Docs/
│   │   └── Blueprint.md     # 設計図（クラス図等）
│   ├── ProjectSettings/     # Unityプロジェクト設定
│   ├── Packages/            # パッケージ管理
│   └── ...
└── ...
```

## 設計図
設計やクラス構成の詳細は、`Unity/Docs/Blueprint.md` を参照してください。


## 使い方（利用者向け）
1. [Releases](https://github.com/ya0872/TaskAnchor/releases) から最新版のzipファイルをダウンロードします。
2. zipを解凍し、フォルダ内の `.exe` ファイルを実行してください。
   - `.exe` と同じ階層にある `*_Data` フォルダも必須です。

## 開発・ビルド（参考情報）

- 使用エディタバージョン：**Unity 2022.3.48f1**

## ライセンス
本プロジェクトのライセンスはリポジトリの LICENSE ファイルを参照してください。
