using Satchel;
using System;
using UnityEngine;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

public static class Utils
{
    /// <summary>
    /// Creates and adds a new collider2D to a target object 
    /// and copies the variables from the source
    /// </summary>
    public static Collider2D CopyCollider2DTo(this Collider2D source, GameObject target)
    {
        // Check if params exist
        if (source == null)
        {
            Modding.Logger.LogWarn("No source collider found when copying collider, skipping");
            return null;
        }
        if (target == null)
        {
            Modding.Logger.LogWarn("No target GameObject found when copying collider, skipping");
            return null;
        }

        Collider2D copyCol = null;

        // Check which type of collider this is
        switch (source)
        {
            case BoxCollider2D box:
                var newBox = target.AddComponent<BoxCollider2D>();
                newBox.size = box.size;
                newBox.edgeRadius = box.edgeRadius;
                copyCol = newBox;
                break;

            case CircleCollider2D circle:
                var newCircle = target.AddComponent<CircleCollider2D>();
                newCircle.radius = circle.radius;
                copyCol = newCircle;
                break;

            case CapsuleCollider2D capsule:
                var newCapsule = target.AddComponent<CapsuleCollider2D>();
                newCapsule.size = capsule.size;
                newCapsule.direction = capsule.direction;
                copyCol = newCapsule;
                break;

            case PolygonCollider2D poly:
                var newPoly = target.AddComponent<PolygonCollider2D>();
                newPoly.pathCount = poly.pathCount;
                for (int i = 0; i < poly.pathCount; i++)
                {
                    newPoly.SetPath(i, poly.GetPath(i));
                }
                copyCol = newPoly;
                break;

            case EdgeCollider2D edge:
                var newEdge = target.AddComponent<EdgeCollider2D>();
                newEdge.points = edge.points;
                newEdge.edgeRadius = edge.edgeRadius;
                copyCol = newEdge;
                break;

            case CompositeCollider2D composite:
                var newComposite = target.AddComponent<CompositeCollider2D>();
                newComposite.geometryType = composite.geometryType;
                newComposite.generationType = composite.generationType;
                newComposite.vertexDistance = composite.vertexDistance;
                copyCol = newComposite;
                break;

            default:
                Debug.LogWarning($"Unsupported Collider2D type: {source.GetType()}");
                return null;
        }

        // Copy universal base class properties shared by all Collider2D components
        copyCol.offset = source.offset;
        copyCol.isTrigger = source.isTrigger;
        copyCol.sharedMaterial = source.sharedMaterial;
        copyCol.usedByEffector = source.usedByEffector;
        copyCol.usedByComposite = source.usedByComposite;

        return copyCol;
    }

    public static void RemoveElementFromSendRandomEventV3(PlayMakerFSM fsm, string stateName, int actionIndex, string eventNameToRemove)
    {
        // Get and cast the action
        var randomAction = fsm.GetAction<SendRandomEventV3>(stateName, actionIndex);
        if (randomAction == null) return;

        // Find the index of the event that should be removed
        int targetIndex = Array.FindIndex(randomAction.events, e => e.Name == eventNameToRemove);

        // If the event isn't found in this action, return
        if (targetIndex == -1) return;

        // Convert arrays to lists, remove the item at the specific index, and reassign
        // Playmaker uses several parallel arrays, so all need to be updated
        randomAction.events = randomAction.events.Where((val, idx) => idx != targetIndex).ToArray();

        if (randomAction.weights != null && randomAction.weights.Length > targetIndex)
            randomAction.weights = randomAction.weights.Where((val, idx) => idx != targetIndex).ToArray();

        if (randomAction.trackingInts != null && randomAction.trackingInts.Length > targetIndex)
            randomAction.trackingInts = randomAction.trackingInts.Where((val, idx) => idx != targetIndex).ToArray();

        if (randomAction.eventMax != null && randomAction.eventMax.Length > targetIndex)
            randomAction.eventMax = randomAction.eventMax.Where((val, idx) => idx != targetIndex).ToArray();

        if (randomAction.trackingIntsMissed != null && randomAction.trackingIntsMissed.Length > targetIndex)
            randomAction.trackingIntsMissed = randomAction.trackingIntsMissed.Where((val, idx) => idx != targetIndex).ToArray();

        if (randomAction.missedMax != null && randomAction.missedMax.Length > targetIndex)
            randomAction.missedMax = randomAction.missedMax.Where((val, idx) => idx != targetIndex).ToArray();
    }

}
