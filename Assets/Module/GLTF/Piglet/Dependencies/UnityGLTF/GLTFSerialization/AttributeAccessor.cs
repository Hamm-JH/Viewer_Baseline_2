﻿using GLTF.Schema;

namespace GLTF
{
	public class AttributeAccessor
	{
		public AccessorId AccessorId { get; set; }
		public NumericArray AccessorContent { get; set; }
		public byte[] Buffer { get; set; }   // ASSET_TODO : should change to IntPtr?

		public AttributeAccessor()
		{
			AccessorContent = new NumericArray();
		}
	}
}
