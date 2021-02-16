using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スクロールできる画像を管理するクラス
/// </summary>
public class ScrollableImage : MonoBehaviour
{
    [SerializeField]
    private Image m_image = null;

    [SerializeField, Range( 0.0f, 3.0f )]
    private float m_speed = 0.0f;

    /// <summary>
    /// 規定値に戻す
    /// </summary>
    private void Reset() 
    {
        m_image = GetComponent<Image>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        var offset_x    = Mathf.Repeat( Time.time * m_speed, 1.0f );
        var offset      = new Vector2( offset_x, 0.0f );
        
        // TODO: 間違い↓ ※ Sprite-Defaultのマテリアルには、オフセットが存在しないため、uvを直でいじる必要がある
        m_image.material.SetTextureOffset( "_MainTex", offset );
    }
}
