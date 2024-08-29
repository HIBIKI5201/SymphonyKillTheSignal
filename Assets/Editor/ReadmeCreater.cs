using UnityEditor;
using UnityEngine;
using System.IO;

public class ReadmeCreator : Editor
{
    [MenuItem("Tools/Create README.md")]
    public static void CreateReadme()
    {
        // READMEファイルのパスを指定
        string path = Application.dataPath + "/README.md";

        // README.md がすでに存在するかをチェック
        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("README.md already exists",
                "README.md ファイルは既に存在します。上書きしますか？", "はい", "いいえ"))
            {
                return;
            }
        }

        // README.md の内容を作成
        string content = "# プロジェクト名\n\n" +
                         "## 概要\n" +
                         "このプロジェクトは〇〇のためのUnityプロジェクトです。\n\n" +
                         "## 使用方法\n" +
                         "1. Unityを開いてプロジェクトをロードします。\n" +
                         "2. `Assets` フォルダにある `MainScene` をダブルクリックしてシーンを開きます。\n" +
                         "3. プレイボタンをクリックしてゲームを開始します。\n\n" +
                         "## 開発者向け情報\n" +
                         "- `Scripts` フォルダには、すべてのC#スクリプトが格納されています。\n" +
                         "- `Prefabs` フォルダには、使用されるすべてのプレハブが含まれています。\n\n" +
                         "## 注意点\n" +
                         "- このプロジェクトは Unity 2021.3.0f1 で動作確認を行っています。\n" +
                         "- プロジェクトのバージョンをアップグレードする際は、必ずバックアップを取ってください。\n";

        // README.md ファイルを作成
        File.WriteAllText(path, content);

        // アセットデータベースを更新して、ファイルをUnityエディタで表示できるようにする
        AssetDatabase.Refresh();

        // ファイルが作成されたことを通知
        Debug.Log("README.md ファイルが作成されました: " + path);
    }
}