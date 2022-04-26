using UnityEngine;

public static class HexMetrics
{
	/// <summary>
	/// Distance from center of hex to a corner
	/// </summary>
	public const float outerRadius = 2f;

	/// <summary>
	/// Distance from center of hex to center of edge
	/// </summary>
	public const float innerRadius = outerRadius * 0.866025404f;

	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius)
	};
}
