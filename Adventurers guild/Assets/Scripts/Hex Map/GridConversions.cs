using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridConversions
{
	public static HexCoordinate HexFromOffsetCoordinates(int x, int y)
	{
		return new HexCoordinate(x - y / 2, y);
	}

	public static OffsetCoordinate OffsetfromHexCoordinate(HexCoordinate hexCoordinate)
	{
		return new OffsetCoordinate(hexCoordinate.X + (hexCoordinate.Y / 2), hexCoordinate.Y);
	}
	public static Vector3 worldPointFromOffsetCoordinates(int x, int y)
    {
		Vector3 worldPoint;
		worldPoint.x = (x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f);
		worldPoint.y = y * (HexMetrics.outerRadius * 1.5f);
		worldPoint.z = 0f;
		return worldPoint;
	}
}
