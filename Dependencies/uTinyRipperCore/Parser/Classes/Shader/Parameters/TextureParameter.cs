﻿using uTinyRipper.Converters;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes.Shaders
{
	public struct TextureParameter : IAssetReadable, IYAMLExportable
	{
		/// <summary>
		/// 2017.3 and greater
		/// </summary>
		public static bool HasMultiSampled(Version version) => version.IsGreaterEqual(2017, 3);

		public TextureParameter(string name, int index, byte dimension, int sampler)
		{
			Name = name;
			NameIndex = -1;
			Index = index;
			Dim = dimension;
			SamplerIndex = sampler;
			MultiSampled = false;
		}

		public TextureParameter(string name, int index, byte dimension, int sampler, bool multiSampled):
			this(name, index, dimension, sampler)
		{
			MultiSampled = multiSampled;
		}

		public void Read(AssetReader reader)
		{
			NameIndex = reader.ReadInt32();
			Index = reader.ReadInt32();
			SamplerIndex = reader.ReadInt32();

			if (HasMultiSampled(reader.Version))
			{
				MultiSampled = reader.ReadBoolean();
			}
			Dim = reader.ReadByte();
			reader.AlignStream();
		}

        public YAMLNode ExportYAML(IExportContainer container)
        {
			var node = new YAMLMappingNode();
			node.Add("m_NameIndex", NameIndex);
			node.Add("m_Index", Index);
			node.Add("m_SamplerIndex", SamplerIndex);
			if (HasMultiSampled(container.Version))
			{
				node.Add("m_MultiSampled", MultiSampled);
			}
			node.Add("m_Dim", Dim);
			return node;
		}

        public string Name { get; set; }
		public int NameIndex { get; set; }
		public int Index { get; set; }
		public int SamplerIndex { get; set; }
		public bool MultiSampled { get; set; }
		public byte Dim { get; set; }
	}
}
