using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor( typeof( Transform ) )]
sealed public class TransformInspector : Editor
{
    private static readonly GUIContent      PROPERTY_FIELD_LABEL    = new GUIContent( string.Empty );
    private static readonly GUILayoutOption RESET_BUTTON_WIDTH      = GUILayout.Width( 20 );
    private static readonly GUILayoutOption VALUE_FIELD_HEIGHT      = GUILayout.Height( 16 );

    private Transform           m_transform;
    private SerializedProperty  m_position_property;
    private SerializedProperty  m_scale_property;

    /// <summary>
    /// 有効化した時の処理
    /// </summary>
    private void OnEnable()
    {
        m_transform         = target as Transform;
        m_position_property = serializedObject.FindProperty( "m_LocalPosition" );
        m_scale_property    = serializedObject.FindProperty( "m_LocalScale" );
    }

    /// <summary>
    /// インスペクタを描画する時の処理
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPositionUI();
        DrawRotationUI();
        DrawScaleUI();

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 座標のUIを描画
    /// </summary>
    private void DrawPositionUI()
    {
        using ( new EditorGUILayout.HorizontalScope() )
        {
            var is_pushed_reset_button = GUILayout.Button( "P", RESET_BUTTON_WIDTH );
            if ( is_pushed_reset_button ) m_position_property.vector3Value = Vector3.zero;

            EditorGUILayout.PropertyField( m_position_property, PROPERTY_FIELD_LABEL, VALUE_FIELD_HEIGHT );
        }
    }

    /// <summary>
    /// 回転のUIを描画
    /// </summary>
    private void DrawRotationUI()
    {
        var new_value   = Vector3.zero;
        var transforms  = targets.Cast<Transform>().ToList();

        EditorGUI.BeginChangeCheck();

        using ( new EditorGUILayout.HorizontalScope() )
        {
            var current_value                   = TransformUtils.GetInspectorRotation( m_transform );
            var is_pushed_reset_button          = GUILayout.Button( "R", RESET_BUTTON_WIDTH );
            var contain_difference_rotations    = containDifferenceRotationsInSelectedObjects( transforms, current_value );

            if ( contain_difference_rotations ) EditorGUI.showMixedValue = true;
            var field_value = EditorGUILayout.Vector3Field( string.Empty, current_value, VALUE_FIELD_HEIGHT );
            if ( contain_difference_rotations ) EditorGUI.showMixedValue = false;

            new_value = is_pushed_reset_button ? Vector3.zero : field_value;
        }

        if ( EditorGUI.EndChangeCheck() )
        {
            Undo.RecordObjects( targets, "Inspector" );
            transforms.ForEach( transform => TransformUtils.SetInspectorRotation( transform, new_value ) );
        }
    }

    /// <summary>
    /// 選択したオブジェクトに異なる回転が含まれているか判別する値を取得
    /// </summary>
    private bool containDifferenceRotationsInSelectedObjects( IList<Transform> transforms, Vector3 current_value )
    {
        if ( transforms.Count <= 1 ) return false;

        return transforms.Any( transform => current_value != TransformUtils.GetInspectorRotation( transform ) );
    }

    /// <summary>
    /// 大きさのUIを描画
    /// </summary>
    private void DrawScaleUI()
    {
        using ( new EditorGUILayout.HorizontalScope() )
        {
            var is_pushed_reset_button = GUILayout.Button( "S", RESET_BUTTON_WIDTH );
            if ( is_pushed_reset_button ) m_scale_property.vector3Value = Vector3.one;

            EditorGUILayout.PropertyField( m_scale_property, PROPERTY_FIELD_LABEL, VALUE_FIELD_HEIGHT );
        }
    }
}
