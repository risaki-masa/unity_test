using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log( "この文字列は出力されません。" );
        UnityEngine.Debug.Log( "この文字列は出力されます。" );
    }
}