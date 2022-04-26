using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OffsetCoordinate
{
    private int x, y;

	public OffsetCoordinate(int _x, int _y)
	{
		x = _x;
		y = _y;
	}
	
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

	public override string ToString()
	{
		return "(" +
			X.ToString() + ", " + Y.ToString();
	}

	public string ToStringOnSeparateLines()
	{
		return X.ToString() + "\n" + Y.ToString();
	}

}
