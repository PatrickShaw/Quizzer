using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Quizzer;
using System.IO;
using System.Globalization; 
using System.Threading;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging; 
using System.Windows.Input.StylusPlugIns;
using System.Windows.Ink;
using System.Windows.Input;

public partial  class CustomInkCanvas : InkCanvas
{ 
    public int dRenderindex = 0;
    private FilterPlugin filter = new FilterPlugin();
    public CustomInkCanvas()
    {
         dRenderindex = this.StylusPlugIns.IndexOf(this.DynamicRenderer);
    }
    //New 
    private void InkCanvas_StylusMove(object sender, StylusEventArgs e)
    {
    }


    private void InkCanvas_Loaded(object sender, RoutedEventArgs e)
    {
    }
}
// A StylusPlugin that restricts the input area. 
class FilterPlugin : StylusPlugIn
{
    List<double> pressureList = new List<double>();
    const int recordLimit = 4;
    bool realPressureEnabled = false;
    protected override void OnStylusDown(RawStylusInput rawStylusInput)
    {
        // Call the base class before modifying the data.  
        base.OnStylusDown(rawStylusInput);
        pressureList.Clear();
        quarterAverageDistance = 8;
        // Restrict the stylus input.
        StylusPointCollection points = rawStylusInput.GetStylusPoints();
        for (int i = 0; i <= points.Count - 1; i++)
        {
            StylusPoint sp = points[i];
            sp.PressureFactor = ((float)i / (float)points.Count);
            RecordPressure(sp.PressureFactor);
        }
        if (rawStylusInput.GetStylusPoints()[rawStylusInput.GetStylusPoints().Count - 1].PressureFactor == 0.5)
        {
            realPressureEnabled = true;
        }
        else
        { realPressureEnabled = false; }
        oldPoint = points[points.Count - 1];
    }
    //OnStylusDown
    public void RecordPressure(double pressureT)
    {
        pressureList.Insert(0, pressureT);
        if (pressureList.Count > recordLimit)
        {
            pressureList.RemoveAt(pressureList.Count - 1);
        }
    } 

    protected override void OnStylusMove(RawStylusInput rawStylusInput)
    {
        // Call the base class before modifying the data. 
        base.OnStylusMove(rawStylusInput);

        // Restrict the stylus input.
        Filter(rawStylusInput);

    }
    //OnStylusMove


    protected override void OnStylusUp(RawStylusInput rawStylusInput)
    {
        // Call the base class before modifying the data. 
        base.OnStylusUp(rawStylusInput);

        // Restrict the stylus input5
        Filter(rawStylusInput);

    }
    //OnStylusUp

    double quarterAverageDistance = 8;
    StylusPoint oldPoint = new StylusPoint(0, 0);
    //
    private void Filter(RawStylusInput rawStylusInput)
    {





        if (realPressureEnabled)
        {
            // Get the StylusPoints that have come in. 
            StylusPointCollection stylusPoints = rawStylusInput.GetStylusPoints();
            // Modify the (X,Y) data to move the points  
            //' inside the acceptable input area, if necessary.   
            double dX = stylusPoints[stylusPoints.Count - 1].X - oldPoint.X;
            double dY = stylusPoints[stylusPoints.Count - 1].Y - oldPoint.Y;
            Debug.WriteLine(rawStylusInput.GetStylusPoints()[0].PressureFactor);
            double distance = Math.Sqrt(dX * dX + dY * dY);
            double avgPressure = 0;
            double factorTotal = 0;
            for (int i = 0; i <= pressureList.Count - 1; i++)
            {
                float factor = (pressureList.Count * pressureList.Count - i * i);
                factorTotal += factor;
                avgPressure += pressureList[i] * factor;
            }
            if (factorTotal != 0)
            {
                avgPressure /= factorTotal;
            }
            //Debug.WriteLine(avgPressure)
            quarterAverageDistance += distance;
            quarterAverageDistance /= 2.0;

            float distancePressure = (float)(Math.Max(0, Math.Min(0.9, distance / 8.0)));
            RecordPressure(distancePressure);
            float finalPressure = ((float)avgPressure * (float)pressureList.Count + distancePressure * (float)(recordLimit * 2 - pressureList.Count)) / ((float)(recordLimit * 2));
            Debug.WriteLine("A: " + avgPressure.ToString() + "| D: " + distancePressure.ToString() + " | F: " + finalPressure.ToString());
            for (int i = 0; i <= stylusPoints.Count - 1; i++)
            {
                StylusPoint spT = stylusPoints[i];
                spT.PressureFactor = finalPressure;
                stylusPoints[i] = spT;
            }
            //For i As Long = 0 To stylusPoints.Count - 1
            //    Dim sp As StylusPoint = stylusPoints(i)
            //    If sp.X < 50 Then
            //        sp.X = 50
            //    End If
            //    If sp.X > 250 Then
            //        sp.X = 250
            //    End If
            //    If sp.Y < 50 Then
            //        sp.Y = 50
            //    End If
            //    If sp.Y > 250 Then
            //        sp.Y = 250
            //    End If
            //    stylusPoints(i) = sp
            //    Debug.WriteLine(stylusPoints(i).X.ToString() + ", " + stylusPoints(i).Y.ToString())
            //Next
            // Copy the modified StylusPoints back to the RawStylusInput.
            rawStylusInput.SetStylusPoints(stylusPoints);
            oldPoint = stylusPoints[stylusPoints.Count - 1];
        }
    }
    //Filter
}
//FilterPlugin
