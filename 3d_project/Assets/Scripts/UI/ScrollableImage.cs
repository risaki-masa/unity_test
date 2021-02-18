using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スクロールできる画像を管理するクラス
/// </summary>
public sealed class ScrollableImage : MonoBehaviour
{
    [SerializeField]
    private RawImage m_image = null;

    [SerializeField, Range( 0.0f, 3.0f )]
    private float m_speed = 0.0f;

    /// <summary>
    /// 規定値に戻す
    /// </summary>
    private void Reset() 
    {
        m_image = GetComponent<RawImage>();
    }

    /// <summary>
    /// 更新する時の処理
    /// </summary>
    private void Update()
    {
        var uv_rect     = m_image.uvRect;
        uv_rect.x       = Mathf.Repeat( Time.time * m_speed, 1.0f );
        m_image.uvRect  = uv_rect;
    }
}
