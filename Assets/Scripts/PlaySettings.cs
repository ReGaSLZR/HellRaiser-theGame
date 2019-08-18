﻿using UnityEngine;

[CreateAssetMenu(fileName = "New PlaySettings", menuName = "HellRaiser/Create Settings")]
public class PlaySettings : ScriptableObject
{

    [SerializeField]
    private GamePlayInput.InputType m_gamePlayInput;

    public GamePlayInput.InputType GetInputType()
    {
        return m_gamePlayInput;
    }

}