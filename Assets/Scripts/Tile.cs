using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject HexRim;
    public GameObject HexBase;

    public Material CurrentMaterial;
    public Material PreviousMaterial;
    public Material AvailableMaterial;
    public Material InactiveMaterial;
    public Material EmptyMaterial;
    
    public Coordinate Coordinate;
    public int CoordinateX;
    public int CoordinateY;
    
    public bool IsAvailable;
    public bool IsCurrent;
    public bool IsPrevious;
    public bool IsSelected;

    public GameObject Item => _item;
    private GameObject _item;

    public void Start()
    {
        CoordinateX = Coordinate?.X ?? 0;
        CoordinateY = Coordinate?.Y ?? 0;
    }

    public void SetItem(string itemTag, GameObject item)
    {
        HexRim.GetComponent<MeshRenderer>().material = GetItemMaterial(itemTag);
        var clone = Instantiate(
            item,
            transform,
            false);
        _item = clone;
    }

    public void DestroyItem()
    {
        Destroy(_item);
    }

    public void SetIsAvailable(bool isAvailable)
    {
        IsAvailable = isAvailable;
        HexRim.GetComponent<MeshRenderer>().material = isAvailable ? AvailableMaterial : InactiveMaterial;
    }
    
    public void SetIsCurrent(bool isCurrent)
    {
        IsCurrent = isCurrent;
        HexRim.GetComponent<MeshRenderer>().material = isCurrent ? CurrentMaterial : InactiveMaterial;
    }
    
    public void SetIsPrevious(bool isPrevious)
    {
        IsPrevious = isPrevious;
        HexRim.GetComponent<MeshRenderer>().material = isPrevious ? PreviousMaterial : InactiveMaterial;
    }

    public Material GetItemMaterial(string itemTag)
    {
        return EmptyMaterial;
    }

    public void SetMaterialInactive()
    {
        HexRim.GetComponent<MeshRenderer>().material = InactiveMaterial;
    }
    
    public void Reset()
    {
        SetIsPrevious(false);
        SetIsCurrent(false);
        SetIsAvailable(false);
        HexRim.GetComponent<MeshRenderer>().material = EmptyMaterial;
    }
}
