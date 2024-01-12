using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiPerception : MonoBehaviour
{
    [SerializeField] protected string tagName = "";
    [SerializeField] protected float distance = 5;
    [SerializeField] protected float maxAngle = 45;

    public abstract GameObject[] GetGameObjects();



}
