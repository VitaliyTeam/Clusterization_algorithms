﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Clusterization_algorithms
{
    public partial class Form1 : Form
    {
        Graphic graphic;
        Forel forel;
        K_means k_means;
        Route route;
        Dictionary<Point, int> points;
        List<Point> clustersCenter = new List<Point> { };

        public Form1()
        {
            InitializeComponent();
            Graphics graphics = pictBoxArea.CreateGraphics();
            graphic = new Graphic(graphics);

            forel = new Forel(graphic);
            k_means = new K_means(graphic);
            route = new Route();
        }

        private void btnGenPoints_Click(object sender, EventArgs e)
        {
            Console.WriteLine("PictBoxArea: W(X)=" + pictBoxArea.Width + " H(Y)=" + pictBoxArea.Height);

            if(!Int32.TryParse(textBoxSetPointsCount.Text, out int pointsCount))
                pointsCount = 20;

            points = Calculator.setStaticPoints();
            //points = Calculator.generatePoints(pointsCount, pictBoxArea.Width, pictBoxArea.Height);
            graphic.DrawPointDictionary(points);

            textBoxInfo.Text = Calculator.printPoints(points);
        }

        private void btnKMeans_Click(object sender, EventArgs e)
        {
            //List<Point> seeds = new List<Point>{
            //    new Point(200, 200),
            //    new Point(400, 400),
            //    new Point(200, 400)
            //};

            SeedGenerator seedG = new SeedGenerator(); //
            seedG.SetPoints(points);
            seedG.seedCount = 3; // by default

            if (Int32.TryParse(textBoxSetSeedsCount.Text, out int seedCount))
                seedG.seedCount = seedCount;

            List<Point> seeds = seedG.GetSeeds(); // Calculate and get seeds

            k_means.SetPoints(points);
            k_means.SetSeeds(seeds);
            k_means.startK_means();

            clustersCenter = k_means.seeds;

            textBoxInfo.Text = Calculator.printPoints(k_means.getPoints());
        }

        private void btnForel_Click(object sender, EventArgs e)
        {
            forel.setPoints(points);
            forel.Radius = 100;

            if (Int32.TryParse(textBoxSetRadius.Text, out int radius))
                forel.Radius = radius;

            forel.startForel();
            clustersCenter = forel.listOfClastersCenter;

            textBoxInfo.Text = Calculator.printPoints(forel.getPoints());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            pictBoxArea.Image = null;
            textBoxInfo.Clear();
            forel.clearFields();
        }

        private void btnRoute_Click(object sender, EventArgs e)
        {
            route.All_points = Calculator.DictionaryToList(points);
            route.CalculateRoute();
            graphic.DrawRoute(route.RouteList);
        }

        private void btnClustersRoute_Click(object sender, EventArgs e)
        {
            route.All_points = clustersCenter;
            route.CalculateRoute();
            graphic.DrawRoute(route.RouteList);
        }
    }
}
