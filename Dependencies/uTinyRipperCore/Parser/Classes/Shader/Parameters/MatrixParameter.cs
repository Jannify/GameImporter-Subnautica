﻿using uTinyRipper.Converters;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes.Shaders
{
	public struct MatrixParameter : IAssetReadable, IYAMLExportable
	{
		public MatrixParameter(string name, ShaderParamType type, int index, int rowCount, int columnCount)
		{
			Name = name;
			NameIndex = -1;
			Index = index;
			ArraySize = 0;
			Type = type;
			RowCount = (byte)rowCount;
			ColumnCount = (byte)columnCount;
		}

		public MatrixParameter(string name, ShaderParamType type, int index, int arraySize, int rowCount, int columnCount) :
			this(name, type, index, rowCount, columnCount)
		{
			ArraySize = arraySize;
		}

		public void Read(AssetReader reader)
		{
			NameIndex = reader.ReadInt32();
			Index = reader.ReadInt32();
			ArraySize = reader.ReadInt32();
			Type = (ShaderParamType)reader.ReadByte();
			RowCount = reader.ReadByte();
			ColumnCount = 4;
			reader.AlignStream();
		}

        public YAMLNode ExportYAML(IExportContainer container)
        {
			var node = new YAMLMappingNode();
			node.Add("m_NameIndex", NameIndex);
			node.Add("m_Index", Index);
			node.Add("m_ArraySize", ArraySize);
			node.Add("m_Type", (byte)Type);
			node.Add("m_RowCount", RowCount);
			return node;
        }

        public string Name { get; set; }
		public int NameIndex { get; set; }
		public int Index { get; set; }
		public int ArraySize { get; set; }
		public ShaderParamType Type { get; set; }
		public byte RowCount { get; set; }
		public byte ColumnCount { get; set; }
	}
}
