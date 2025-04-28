# ルール
このプロジェクトで守るべきこと

## 関心の整備


## ディレクトリの扱い

- プロジェクト内のすべてのScriptにAssemblyを設定すること
- ディレクトリは機能ごとにファイルを分ける
  - script,prefab,imageなどの素材もそれぞれ機能ごとに格納する
- 抽象と具象、基底と派生は兄弟関係で並べる


## コード規約

### 命名規約
- 型と名前空間はUpperCamel
- interfaceはIUpperCamel
- 型パラメータがTUpperCamel
- プロパティがlowerCamel
- ローカル変数がlowerCamel
- ローカル定数がALL_UPPER
- 引数はlowerCamel
- privateフィールドは_lowerCamel
- publicフィールドはlowerCamel
- 定数フィールドは全てALL _UPPER
- staticフィールドはUpperCamel

### コーディングの規約
- 型の初期化は明白な場合new()を使用
- unsignedではなくintを用いる
- System.String,System.Int32を重点して使う
- Utilに該当するクラスは、静的で副作用を持たない、staticな関数のみ定義する
- if文のネストは控える　だせェので