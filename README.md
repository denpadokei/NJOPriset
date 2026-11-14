# NJOPriset

[![Build and Release](https://github.com/denpadokei/NJOPriset/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/denpadokei/NJOPriset/actions/workflows/build-and-release.yml)

曲と難易度ごとに Note Jump Offset を記憶し、次にその譜面を開いたときに自動で復元する MOD です。

## 機能

- レベル選択画面でプレイボタンを押したとき、その曲・難易度の Note Jump Offset を記録
- 曲を開いたとき (難易度変更・コンテンツ更新・譜面の再読み込み) に記録した値を自動で復元
- 記録のキーは levelID・ビートマップ characteristic・難易度の組み合わせ
- 記憶した値は `UserData/NJOPrisetSongs.json` に JSON で保存され、ゲームを終了しても保持
- Harmony で `StandardLevelDetailView.RefreshContent` に Postfix パッチを当て、譜面情報の更新時にも追従

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.19.0 |
| 依存 MOD | BSIPA 4.0.5 / SiraUtil 3.0.0 |

## インストール

1. Beat Saber を 1.19.0 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/NJOPriset/releases) から `NJOPriset-0.1.1-bs1.19.0.zip` をダウンロードし、`Plugins` フォルダをゲームフォルダへ上書きコピーする

## 使い方

1. レベル選択画面で曲と難易度を選ぶ
2. プレイ設定パネルの Note Jump Offset を好みの値に変更する
3. プレイボタンを押すと、その曲・難易度の値が記憶される
4. 次に同じ譜面を開くと、記憶した値が自動で選択される

記憶した内容は `UserData/NJOPrisetSongs.json` に保存されます。設定を消したい場合はこのファイルを削除してください。

## ビルド

```powershell
git clone https://github.com/denpadokei/NJOPriset.git
cd NJOPriset
dotnet build .\NJOPriset\NJOPriset.csproj -c Release
```

`BeatSaberDir` (または `GameDirectory`) に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。既定では `..\Refs` フォルダが参照先になります。

GitHub Actions の `build-and-release.yml` が push ごとにビルドし、`X.X.X` 形式のタグを push するとリリースを作成します。

## ライセンス

MIT
