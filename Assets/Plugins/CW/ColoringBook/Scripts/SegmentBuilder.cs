using CW.Common;
using System.Collections.Generic;
using UnityEngine;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This class takes an outline texture of something, where the outline stored in the alpha channel, and then splits each segment apart.</summary>
	public static class SegmentBuilder
	{
		public class Line
		{
			public bool Used;
			public int  Y;
			public int  MinX;
			public int  MaxX;

			public List<Line> Ups = new List<Line>();
			public List<Line> Dns = new List<Line>();
		}

		public class Segment
		{
			public int MinX;
			public int MinY;
			public int MaxX;
			public int MaxY;
			public int Count;

			public List<Line> Lines = new List<Line>();
		}

		private static List<Segment> segments = new List<Segment>();

		private static List<Line> lines = new List<Line>();

		private static List<Line> linkedLines = new List<Line>();

		private static int segmentCount;

		private static int lineCount;

		/// <summary>After calling <b>BuildSegments</b>, this tells you how many segments were detected.</summary>
		public static int GetSegmentCount()
		{
			return segmentCount;
		}

		/// <summary>After calling <b>BuildSegments</b>, this allows you to get the data from a specific segment.</summary>
		public static Segment GetSegment(int i)
		{
			return segments[i];
		}

		/// <summary>This will build segments based on the specified texture and alpha/opacity threshold.</summary>
		public static void BuildSegments(Texture2D texture, float outlineThreshold)
		{
			segmentCount = 0;
			lineCount   = 0;

			var oldCount = 0;
			var makeCopy = false;

			if (texture.isReadable == false)
			{
				makeCopy = true;

				texture = CwHelper.GetReadableCopy(texture);
			}

			for (var y = 0; y < texture.height; y++)
			{
				var newCount = FastFindLines(texture, y, outlineThreshold);

				FastLinkLines(lineCount - newCount - oldCount, lineCount - newCount, lineCount);

				oldCount = newCount;
			}

			if (makeCopy == true)
			{
				Object.DestroyImmediate(texture);
			}

			for (var i = 0; i < lineCount; i++)
			{
				var line = lines[i];

				if (line.Used == false)
				{
					var segment = NewSegment(line.MinX, line.MaxX, line.Y, line.Y + 1);

					// Scan though all connected lines and add to list
					linkedLines.Clear(); linkedLines.Add(line); line.Used = true;

					for (var j = 0; j < linkedLines.Count; j++)
					{
						var linkedLine = linkedLines[j];

						segment.MinX   = Mathf.Min(segment.MinX, linkedLine.MinX);
						segment.MaxX   = Mathf.Max(segment.MaxX, linkedLine.MaxX);
						segment.MinY   = Mathf.Min(segment.MinY, linkedLine.Y    );
						segment.MaxY   = Mathf.Max(segment.MaxY, linkedLine.Y + 1);
						segment.Count += linkedLine.MaxX - linkedLine.MinX;

						AddToScan(linkedLine.Ups);
						AddToScan(linkedLine.Dns);

						segment.Lines.Add(linkedLine);
					}
				}
			}
		}

		private static void AddToScan(List<Line> lines)
		{
			for (var i = lines.Count - 1; i >= 0; i--)
			{
				var line = lines[i];

				if (line.Used == false)
				{
					linkedLines.Add(line); line.Used = true;
				}
			}
		}

		private static void FastLinkLines(int min, int mid, int max)
		{
			for (var i = min; i < mid; i++)
			{
				var oldLine = lines[i];

				for (var j = mid; j < max; j++)
				{
					var newLine = lines[j];

					if (newLine.MinX < oldLine.MaxX && newLine.MaxX > oldLine.MinX)
					{
						oldLine.Ups.Add(newLine);
						newLine.Dns.Add(oldLine);
					}
				}
			}
		}

		private static int FastFindLines(Texture2D texture, int y, float outlineThreshold)
		{
			var line   = default(Line);
			var count  = 0;

			for (var x = 0; x < texture.width; x++)
			{
				if (texture.GetPixel(x, y).a <= outlineThreshold)
				{
					// Start new line?
					if (line == null)
					{
						line = NewLine(x, x, y);
						count += 1;
					}

					// Expand line
					line.MaxX += 1;
				}
				// Terminate line?
				else if (line != null)
				{
					line = null;
				}
			}

			return count;
		}

		private static Line NewLine(int minX, int maxX, int y)
		{
			var line = default(Line);

			if (lineCount >= lines.Count)
			{
				line = new Line();

				lines.Add(line);
			}
			else
			{
				line = lines[lineCount];

				line.Used = false;
				line.Ups.Clear();
				line.Dns.Clear();
			}

			line.MinX = minX;
			line.MaxX = maxX;
			line.Y    = y;

			lineCount += 1;

			return line;
		}

		private static Segment NewSegment(int minX, int maxX, int minY, int maxY)
		{
			var segment = default(Segment);

			if (segmentCount >= segments.Count)
			{
				segment = new Segment();

				segments.Add(segment);
			}
			else
			{
				segment = segments[segmentCount];

				segment.Lines.Clear();
				segment.Count = 0;
			}

			segment.MinX  = minX;
			segment.MaxX  = maxX;
			segment.MinY  = minY;
			segment.MaxY  = maxY;

			segmentCount += 1;

			return segment;
		}
	}
}