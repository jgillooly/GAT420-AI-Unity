using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiPerception : MonoBehaviour
{
    [SerializeField] string tagName = "";
    [SerializeField] float distance;
    [SerializeField] float maxAngle = 45;

    public abstract GameObject[] GetGameObjects();



}
