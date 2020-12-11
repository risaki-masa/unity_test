using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Test : MonoBehaviour
{
    // public SceneType m_scene_type = SceneType.SCENE1;

    void Start()
    {
        SceneManager.LoadScene( (int)SceneType.STAGE_SELECT );
        // StartCoroutine( ChangeScene() );
    }

    IEnumerator ChangeScene() 
    {
        var operation = SceneManager.LoadSceneAsync( "Scene3" );
        operation.completed += ( self ) => Debug.Log( "Loaded." );

        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(5.0f);
        operation.allowSceneActivation = true;

        while ( !operation.isDone )
        { 
            yield return null;

            Debug.Log( "Now Loading." );
            Debug.Log( operation.isDone );
            Debug.Log( operation.progress );
        }
    }
}
