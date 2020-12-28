using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

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
    public void FadeIn( float time, Action on_completed = null ) 
    {
        StartCoroutine( ChangeAlphaValueOverTime( time, on_completed, true ) );
    }

    /// <summary>
    /// フェードアウトする
    /// </summary>
    public void FadeOut( float time, Action on_completed = null ) 
    {
        StartCoroutine( ChangeAlphaValueOverTime( time, on_completed ) );
    }

    /// <summary>
    /// 時間経過でアルファ値を変更
    /// </summary>
    private IEnumerator ChangeAlphaValueOverTime( 
        float   max_time, 
        Action  on_completed, 
        bool    is_reversing = false 
    ) {
        if ( !is_reversing ) m_image.enabled = true;

        var elapsed_time    = 0.0f;
        var color           = m_image.color;

        while ( elapsed_time < max_time )
        {
            var elapsed_rate    = Mathf.Min( elapsed_time / max_time, 1.0f );
            color.a             = is_reversing ? 1.0f - elapsed_rate : elapsed_rate;
            m_image.color       = color;
            
            yield return null;
            elapsed_time += Time.deltaTime;
        }

        if ( is_reversing )         m_image.enabled = false;
        if ( on_completed != null ) on_completed();
    }
}
