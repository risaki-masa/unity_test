using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェードを管理するクラス
/// </summary>
public sealed class Fade : MonoBehaviour
{
    [SerializeField]
    private Image m_image = null;

    /// <summary>
    /// 規定値に戻す
    /// </summary>
    private void Reset() 
    {
        m_image = GetComponent<Image>();
    }

    /// <summary>
    /// フェードインする
    /// </summary>
    public void FadeIn( float duration, Action on_completed = null ) 
    {
        StartCoroutine( ChangeAlphaValueFrom0To1OverTime( duration, on_completed, true ) );
    }

    /// <summary>
    /// フェードアウトする
    /// </summary>
    public void FadeOut( float duration, Action on_completed = null ) 
    {
        StartCoroutine( ChangeAlphaValueFrom0To1OverTime( duration, on_completed ) );
    }

    /// <summary>
    /// 時間経過でアルファ値を「0」から「1」に変更
    /// </summary>
    private IEnumerator ChangeAlphaValueFrom0To1OverTime( 
        float   duration, 
        Action  on_completed, 
        bool    is_reversing = false 
    ) {
        if ( !is_reversing ) m_image.enabled = true;

        var elapsed_time    = 0.0f;
        var color           = m_image.color;

        while ( elapsed_time < duration )
        {
            var elapsed_rate    = Mathf.Min( elapsed_time / duration, 1.0f );
            color.a             = is_reversing ? 1.0f - elapsed_rate : elapsed_rate;
            m_image.color       = color;
            
            yield return null;
            elapsed_time += Time.deltaTime;
        }

        if ( is_reversing )         m_image.enabled = false;
        if ( on_completed != null ) on_completed();
    }
}
