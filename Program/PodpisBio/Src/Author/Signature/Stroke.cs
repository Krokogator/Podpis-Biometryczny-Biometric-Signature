﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using PodpisBio.Src.Service;

namespace PodpisBio.Src.Author
{
    class Stroke
    {
        public List<Point> Points { get; set; }
        public bool isScaled { get; set; }

        public List<Derivatives> derivatives { get; set; }

        public Stroke()
        {
            Points = new List<Point>();
            derivatives = new List<Derivatives>();
        }

        public Stroke(List<Point> Points) : this()
        {
            this.Points = Points;
        }

        public Stroke(Stroke stroke) : this()
        {
            foreach(Point p in stroke.getPoints())
            {
                this.Points.Add(new Point(p));
            }
            this.derivatives = stroke.getDerivatives();
        }

        public List<Point> getPoints() { return Points; }

        public List<Derivatives> getDerivatives() { return this.derivatives; }

        public void addPoint(Point point)
        {
            if (!this.Points.Any())
            {
                this.Points.Add(point);
                this.derivatives.Add(new Derivatives());
            }
            else
            {
                Dynamics calculator = new Dynamics();
                var prevPoint = this.Points[this.Points.Count - 1];
                var prevDerivatives = this.derivatives[this.derivatives.Count() - 1];
                var derivatives = calculator.calcDerivatives(prevPoint, point, prevDerivatives);
                this.Points.Add(point);
                this.derivatives.Add(derivatives);
                //Debug.WriteLine("Adam oblicza pochodne v = " + derivatives.Velocity + " i acc = " + derivatives.Acc);
            }
        }

        public float getWidth() { return Points.Max(pt => pt.getX()) - Points.Min(pt => pt.getX()); }

        public float getHeight() { return Points.Max(pt => pt.getY()) - Points.Min(pt => pt.getY()); }

        public long getTime() { return Points[Points.Count - 1].getTime() - Points[0].getTime(); }

        // TODO: MK dodaj obliczanie sługości i średniej szybkości
    }
}
