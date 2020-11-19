using UnityEditor;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// シーンの後処理を行うクラス( エディタ拡張 )
/// </summary>
public sealed class ScenePostprocessor : AssetPostprocessor 
{
    private const string BUILD_DIRECTORY_PATH = "Assets/Scenes/Build";
    private const string SCENE_FILE_EXTENSION = ".unity";

    /// <summary>
    /// アセットをインポートした時の処理( アセットへの後処理 )
    /// </summary>
    private static void OnPostprocessAllAssets(
        string[] imported_asset_paths, 
        string[] deleted_asset_paths, 
        string[] asset_paths_after_moving, 
        string[] asset_paths_before_moving 
    ) {
        var scene_list = EditorBuildSettings.scenes.ToList();

        AddScenes   ( scene_list, imported_asset_paths  , asset_paths_after_moving );
        RemoveScenes( scene_list, deleted_asset_paths   , asset_paths_after_moving );

        EditorBuildSettings.scenes = scene_list.ToArray();
    }

    /// <summary>
    /// シーンを追加
    /// </summary>
    private static void AddScenes( 
        List<EditorBuildSettingsScene>  scene_list, 
        string[]                        imported_asset_paths, 
        string[]                        asset_paths_after_moving 
    ) {
        var adding_scenes = imported_asset_paths
            .Concat ( asset_paths_after_moving )
            .Where  ( asset_path        => asset_path.EndsWith( SCENE_FILE_EXTENSION ) )
            .Where  ( scene_file_path   => scene_file_path.StartsWith( BUILD_DIRECTORY_PATH ) )
            .Where  ( scene_file_path   => !scene_list.Any( scene => scene.path == scene_file_path ) )
            .Select ( scene_file_path   => new EditorBuildSettingsScene( scene_file_path, true ) )
        ;

        scene_list.AddRange( adding_scenes );
    }

    /// <summary>
    /// シーンを削除
    /// </summary>
    private static void RemoveScenes( 
        List<EditorBuildSettingsScene>  scene_list, 
        string[]                        deleted_asset_paths, 
        string[]                        asset_paths_after_moving
    ) {
        var moved_scene_files_out_of_build = asset_paths_after_moving
            .Where( asset_path      => asset_path.EndsWith( SCENE_FILE_EXTENSION ) )
            .Where( scene_file_path => !scene_file_path.StartsWith( BUILD_DIRECTORY_PATH ) )
        ;

        scene_list.RemoveAll( scene => deleted_asset_paths.Contains( scene.path ) );
        scene_list.RemoveAll( scene => moved_scene_files_out_of_build.Contains( scene.path ) );
    }
}