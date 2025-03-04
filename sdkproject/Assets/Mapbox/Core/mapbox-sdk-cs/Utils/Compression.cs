﻿//-----------------------------------------------------------------------
// <copyright file="Compression.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;
using Mapbox.IO.Compression;

namespace Mapbox.Utils
{
	/// <summary> Collection of constants used across the project. </summary>
	public static class Compression
	{
		/// <summary>
		///     Decompress the specified buffer previously compressed using GZip.
		/// </summary>
		/// <param name="buffer">
		///     The GZip'ed buffer.
		/// </param>
		/// <returns>
		///     Returns the uncompressed buffer or the buffer in case decompression
		///     is not possible.
		/// </returns>
		public static byte[] Decompress(byte[] buffer)
		{
			// Test for magic bits.
			if (buffer.Length < 2 || buffer[0] != 0x1f || buffer[1] != 0x8b) return buffer;

			using (var stream = new GZipStream(new MemoryStream(buffer), CompressionMode.Decompress))
			{
				const int Size = 4096; // Pagesize.
				var buf = new byte[Size];

				using (var memory = new MemoryStream())
				{
					var count = 0;

					do
					{
						try
						{
							count = stream.Read(buf, 0, Size);
						}
						catch
						{
							// For now we return the uncompressed buffer
							// on error. Assumes the magic check passed
							// by luck.
							return buffer;
						}

						if (count > 0) memory.Write(buf, 0, count);
					} while (count > 0);

					buffer = memory.ToArray();
				}
			}

			return buffer;
		}


		public static byte[] Compress(byte[] raw, CompressionLevel compressionLevel)
		{
			using (var memory = new MemoryStream())
			{
				using (var gzip = new GZipStream(memory, compressionLevel))
				{
					gzip.Write(raw, 0, raw.Length);
				}

				return memory.ToArray();
			}
		}

		public static byte[] CompressModeCompress(byte[] raw)
		{
			using (var memory = new MemoryStream())
			{
				using (var gzip = new GZipStream(memory, CompressionMode.Compress, true))
				{
					gzip.Write(raw, 0, raw.Length);
				}

				return memory.ToArray();
			}
		}
	}
}
