using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tileset",menuName = "Create new Tileset")]
public class TileSet : ScriptableObject
{
	public Location location;
	public Sprite[] Ground;
	public Sprite GroundRightEdge;
	public Sprite GroundLeftEdge;
	public Sprite[] Water;

	public Sprite[] Decoration;
	public Sprite[] HugeDecoration;

	public Color BackgroundColor;

	public Sprite GetRandomGround()
	{
		return Ground[Random.Range(0, Ground.Length)];
	}

	public Sprite GetRandomWater()
	{
		return Water[Random.Range(0, Water.Length)];
	}

	public Sprite GetRandomDecoration()
	{
		return Decoration[Random.Range(0, Decoration.Length)];
	}

	public Sprite GetRandomHugeDecoration()
	{
		return HugeDecoration[Random.Range(0,HugeDecoration.Length)];
	}
}

public enum Location
{
	Forest,Desert
}
