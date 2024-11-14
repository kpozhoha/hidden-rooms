using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorControllerData : MonoBehaviour
{
    [SerializeField] private TutorData _tutorData;
    private Queue<Tutor> _tutors;
    
    public int Count => _tutors.Count;

    private void Awake()
    {
        _tutors = _tutorData.TutorList;
    }

    public Tutor GetTutor()
    {
        return _tutors.Dequeue();
    }
}
