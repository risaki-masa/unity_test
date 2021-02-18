using UnityEngine;

/// <summary>
/// 平行移動を繰り返すクラス
/// </summary>
public sealed class TranslationRepeater : MonoBehaviour
{
    [SerializeField]
    private float start_position    = 0.0f;

    [SerializeField]
    private float end_position      = 0.0f;
    
    [SerializeField]
    private float m_speed           = 0.0f;

    /// <summary>
    /// 更新する時の処理
    /// </summary>
    private void Update()
    {
        transform.Translate( new Vector3( m_speed, 0.0f, 0.0f ) );

        var position = transform.localPosition;
        if ( position.x > end_position ) return;

        position.x              = start_position;
        transform.localPosition = position;
    }
}
