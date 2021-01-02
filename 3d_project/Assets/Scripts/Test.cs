using System.Collections;
using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Fade m_fade = null;

    private void Start()
    {
        Action on_completed = () => 
        {
            StartCoroutine( Wait3SecondsAndFadeOut() );
        };

        m_fade.FadeIn( 2.0f, on_completed );
    }

    private IEnumerator Wait3SecondsAndFadeOut() 
    {
        yield return new WaitForSeconds( 3.0f );
        m_fade.FadeOut( 2.0f );
    }
}
