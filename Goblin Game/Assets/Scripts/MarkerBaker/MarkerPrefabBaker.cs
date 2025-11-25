using System.Collections.Generic;
using UnityEngine;

// Goal: Replace marker GameObjects in-scene with specified prefabs based on name prefixes.

public class MarkerPrefabBaker : MonoBehaviour
{
    [System.Serializable]
    public class MarkerMapping
    {
        [Tooltip("Prefix of the marker name to look for: ex) 'Marker_'")]
        public string markerPrefix;

        [Tooltip("Prefab to instantiate at the location of markers with this prefix")]
        public GameObject prefab;
    }

    [Tooltip("Mappings from marker name prefix to prefab")]
    public List<MarkerMapping> mappings = new List<MarkerMapping>();

    [Tooltip("Destroy original markers after baking")]
    public bool destroyMarkersAfterBake = false;

    // if we want to keep track of what was spawned
    [HideInInspector]
    public List<GameObject> bakedInstances = new List<GameObject>();

    // Main bake Function, called by the editor script
    public void BakeMarkers()
    {
        bakedInstances.Clear();
        if (mappings == null || mappings.Count == 0)
        {
            Debug.LogWarning($"[MarkerPrefabBaker] No mappings set on {name}. Bake Aborted.");
            return;
        }

        // build lookup dictionary for faster matching
        var dict = new Dictionary<string, GameObject>();

        // for each mapping, add it to dictionary.
        AddToDictionary(dict);

        // collect all markers (in my case, they will all be transforms).
        var markers = new List<Transform>();
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            // skip self
            if (t == this.transform) continue;

            // for each of the key-value pairs in the dictionary
            foreach (var kvp in dict)
            {
                // check name == prefix match
                if (t.name.StartsWith(kvp.Key))
                {
                    // add the marker to the list
                    markers.Add(t);
                    break;
                }
            }
        }

        if (markers.Count == 0)
        {
            Debug.Log($"[MarkerPrefabBaker] No matching markers found under {name}.");
            return;
        }

        // Process each marker and spawn prefabs at marker locations.
        ProcessAndSpawnPrefabsAtMarkers(dict, markers);

        Debug.Log($"[MarkerPrefabBaker] Baked {bakedInstances.Count} markers under {name}.");
    }
    private void AddToDictionary(Dictionary<string, GameObject> dict)
    {
        foreach (var m in mappings)
        {
            if (!string.IsNullOrEmpty(m.markerPrefix) && m.prefab != null)
            {
                dict[m.markerPrefix] = m.prefab;
            }
        }
    }

    private void ProcessAndSpawnPrefabsAtMarkers(Dictionary<string, GameObject> dict, List<Transform> markers)
    {
        foreach (var marker in markers)
        {
            GameObject prefab = null;
            foreach (var kvp in dict)
            {
                if (marker.name.StartsWith(kvp.Key))
                {
                    prefab = kvp.Value;
                    break;
                }
            }

            if (prefab == null) continue;

            var instance = Instantiate(prefab, marker.position, marker.rotation, this.transform);
            instance.name = prefab.name;
            bakedInstances.Add(instance);

            if (destroyMarkersAfterBake)
            {
                DestroyImmediate(marker.gameObject);
            }
        }
    }

}
