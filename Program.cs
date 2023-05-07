using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PolygonDrawer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ввод координат точек
            List<Point> points = new List<Point>();
            int pointCount = 0;
            do
            {
                Console.WriteLine($"Введите координаты точки {pointCount + 1}:");
                Console.Write("x = ");
                double x = double.Parse(Console.ReadLine());
                Console.Write("y = ");
                double y = double.Parse(Console.ReadLine());
                points.Add(new Point(x, y));
                pointCount++;

                Console.WriteLine("Добавить еще точку? (y/n)");
            }
            while (Console.ReadLine().ToLower() == "y");

            // Вывод координат точек
            Console.WriteLine("Координаты точек:");
            foreach (Point point in points)
            {
                Console.WriteLine(point);
            }

            // Создание и вывод полигона
            Polygon polygon = new Polygon(points);
            Console.WriteLine($"Создан полигон с координатами: {polygon}");

            // Создание файла SVG
            Console.WriteLine("Создание файла SVG...");
            string svgOutput = polygon.ToSVG();
            string filePath = "output.svg";
            File.WriteAllText(filePath, svgOutput);
            Console.WriteLine($"Файл {filePath} успешно создан.");
        }
    }

    public struct Point
    {
        public double x, y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }

    public class Polygon
    {
        private List<Point> points;

        public Polygon(List<Point> points)
        {
            this.points = points;
        }

        public override string ToString()
        {
            string output = string.Empty;
            foreach (Point point in points)
            {
                output += $"{point.x},{point.y} ";
            }
            return output.TrimEnd();
        }

        public string ToSVG()
        {
            XmlDocument svgDoc = new XmlDocument();
            XmlNode svgNode = svgDoc.CreateElement("svg");
            svgDoc.AppendChild(svgNode);

            XmlAttribute widthAttr = svgDoc.CreateAttribute("width");
            widthAttr.Value = "400";
            XmlAttribute heightAttr = svgDoc.CreateAttribute("height");
            heightAttr.Value = "400";
            svgNode.Attributes.Append(widthAttr);
            svgNode.Attributes.Append(heightAttr);

            XmlNode polygonNode = svgDoc.CreateElement("polygon");
            XmlAttribute pointsAttr = svgDoc.CreateAttribute("points");
            pointsAttr.Value = this.ToString();
            polygonNode.Attributes.Append(pointsAttr);
            svgNode.AppendChild(polygonNode);

            return svgDoc.OuterXml;
        }
    }
}

