using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using System.Text.RegularExpressions;

/// <summary>
/// シーン名を管理するクラスのスクリプトを作成するクラス
/// </summary>
public static class SceneNameScriptCreator
{
private const string MENU_ITEM_NAME     = "Tools/Create/Scene Name";
private const string SCRIPT_FILE_PATH   = "Assets/Scripts/Constants/SceneName.cs";

    /// <summary>
    /// シーンリストを変更した時の処理
    /// </summary>
    [MenuItem( MENU_ITEM_NAME )]
    private static void Create() 
    {
        if ( !CanCreate() ) return;

        CreateDirectoryIfNotExists();
        var script_string = CreateScriptString();

        File.WriteAllText( SCRIPT_FILE_PATH, script_string, Encoding.UTF8 );
        AssetDatabase.Refresh( ImportAssetOptions.ImportRecursive );

        EditorUtility.DisplayDialog( SCRIPT_FILE_PATH, "シーン名の作成が完了しました。", "OK" );
    }

    /// <summary>
    /// 作成できるか判別する値を取得
    /// </summary>
    private static bool CanCreate() => !EditorApplication.isPlaying && !EditorApplication.isCompiling;

    /// <summary>
    /// ディレクトリが存在しない場合、作成
    /// </summary>
    private static void CreateDirectoryIfNotExists() 
    {
        var directory_path = Path.GetDirectoryName( SCRIPT_FILE_PATH );
        if( Directory.Exists( directory_path ) ) return;
        
        Directory.CreateDirectory( directory_path );
    }

    /// <summary>
    /// スクリプトの文字列を作成
    /// </summary>
    private static string CreateScriptString() 
    {
        var contents = EditorBuildSettings.scenes
            .Select( scene => Path.GetFileNameWithoutExtension( scene.path ) )
            .Distinct()
            .Aggregate( String.Empty, ( concated, scene_name ) => 
            {
                var adding_format_string    = @"    public const string {0} = ""{1}"";{2}";
                var constant_name           = ToSnakeCaseFromUpperCamelCase( scene_name );

                return concated + String.Format( 
                    adding_format_string, 
                    constant_name, 
                    scene_name, 
                    Environment.NewLine
                );
            } )
        ;

        var builder = new StringBuilder()
            .AppendLine( "/// <summary>" )
            .AppendLine( "/// シーンの名前を管理するクラス" )
            .AppendLine( "/// <summary>" )
            .AppendLine( "public static class SceneNames" )
            .AppendLine( "{" )
            .Append( contents )
            .AppendLine( "}" )
        ;

        return builder.ToString();
    }

    /// <summary>
    /// アッパーキャメルケースからスネークケースに変換
    /// </summary>
    private static string ToSnakeCaseFromUpperCamelCase( string value ) 
    {
        return Regex.Replace( value, "([a-z])([A-Z])", "$1_$2" ).ToUpper();
    }
}
