using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TutorData : ScriptableObject
{
    [SerializeField] private List<Tutor> m_TutorList;

    public Queue<Tutor> TutorList
    {
        get
        {
            List<Tutor> list = new List<Tutor>();
            m_TutorList.ForEach(t =>
            {
                t.FillQueue();
                list.Add(t);
            });

            return new Queue<Tutor>(list);
        }
    }
}

[System.Serializable]
public class Tutor
{
    public TutorType Type;
    public MoveCharacterType moveCharacter;
    public bool IsMask;
    public bool IsEndTutor;
    public bool IsMaskot;
    private Queue<string> queueTutors;
    [SerializeField] private List<string> _textsTutor;

    public Queue<string> QueueTutors { get => queueTutors; set => queueTutors = value; }

    public void FillQueue()
    {
        queueTutors = new Queue<string>(_textsTutor);
    }
}

