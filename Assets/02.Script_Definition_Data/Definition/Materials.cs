using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Materials
	{
		public static Material Set(MaterialType type)
		{
			Material result = Resources.Load<Material>("3D/DefaultMat");

			switch(type)
			{
				case MaterialType.Default1:
					result = Resources.Load<Material>("3D/DefaultMat");
					break;

				case MaterialType.ObjDefault1:
					result = Resources.Load<Material>("3D/ObjMat");
					break;
			}

			return result;
		}
	}
}
