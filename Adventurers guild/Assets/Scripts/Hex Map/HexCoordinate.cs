using UnityEngine;

[System.Serializable]
public struct HexCoordinate
{

	[SerializeField]
	private int x, y;

	public int X
	{
		get
		{
			return x;
		}
	}

	public int Y
	{
		get
		{
			return y;
		}
	}

	public int Z
	{
		get
		{
			return -X - Y;
		}
	}

	public HexCoordinate(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static HexCoordinate FromOffsetCoordinates(int x, int y)
	{
		return new HexCoordinate(x - y / 2, y);
	}

	public static HexCoordinate FromPosition(Vector3 position)
	{
		float x = position.x / (HexMetrics.innerRadius * 2f);
		float z = -x;

		float offset = position.y / (HexMetrics.outerRadius * 3f);
		x -= offset;
		z -= offset;

		int iX = Mathf.RoundToInt(x);
		int iZ = Mathf.RoundToInt(z);
		int iY = Mathf.RoundToInt(-x - z);

		if (iX + iZ + iY != 0)
		{
			float dX = Mathf.Abs(x - iX);
			float dY = Mathf.Abs(z - iZ);
			float dZ = Mathf.Abs(-x - z - iY);

			if (dX > dY && dX > dZ)
			{
				iX = -iZ - iY;
			}
			else if (dZ > dY)
			{
				iY = -iX - iZ;
			}
		}

		return new HexCoordinate(iX, iY);
	}



	public override string ToString()
	{
		return "(" +
			X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines()
	{
		return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
	}
}