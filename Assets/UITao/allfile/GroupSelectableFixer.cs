using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GroupSelectableFixer : MonoBehaviour
{
    [System.Serializable]
    public class SelectableGroup
    {
        public string groupName;
        public GameObject[] selectableObjects;
        [HideInInspector] public GameObject lastSelected;
    }

    public SelectableGroup[] groups;
    private Dictionary<GameObject, SelectableGroup> lookup;

    void Start()
    {
        lookup = new Dictionary<GameObject, SelectableGroup>();

        foreach (var group in groups)
        {
            foreach (var obj in group.selectableObjects)
            {
                lookup[obj] = group;
                var trigger = obj.AddComponent<EventTrigger>();
                var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
                entry.callback.AddListener((data) => OnSelect(obj));
                trigger.triggers.Add(entry);
            }
        }
    }

    void Update()
    {
        foreach (var group in groups)
        {
            if (group.lastSelected != null && EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(group.lastSelected);
            }
        }
    }

    void OnSelect(GameObject obj)
    {
        if (lookup.TryGetValue(obj, out var group))
        {
            group.lastSelected = obj;
        }
    }
}
